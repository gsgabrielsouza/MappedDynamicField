using AutoMapper;
using AutoMapper.Configuration;
using MapDynamicField.TransferObject;
using POC.MappedFieldToAnother.Domain.Entities;
using POC.MappedFieldToAnother.Domain.Service;
using POC.MappedFieldToAnother.DTO.Order;
using POC.MappedFieldToAnother.Integration.Ecommerce;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;

namespace POC.MappedFieldToAnother
{
    class Program
    {
        private static MapperConfiguration configuration;

        static Program()
        {
            //configuration = new MapperConfiguration(cfg =>
            //{
            //    //cfg.CreateMap<Bar, BarDto>();
            //});


        }

        static void Main(string[] args)
        {
            var ecommerceOrderService = new EcommerceOrderService();
            var mappingFieldService = new MappingFieldService();

            var nonIntegratedOrders = ecommerceOrderService.GetNonIntegraded();
            var mapping = mappingFieldService.GetFields();

            var orders = new List<Order>();
            //MyImplementation(nonIntegratedOrders, mapping, orders);
            var order = new Order();

            AutoMapper<EcommerceOrderDTO, Order>(
                mapping.Select(x => new MapDynamicField.TransferObject.PropertyMap(x.EcommerceProperty, x.IntegratorProperty)),
                nonIntegratedOrders.First(),
                order);

            foreach (var item in orders)
                Console.WriteLine(JsonSerializer.Serialize(item));

            Console.ReadKey();
        }

        static void AutoMapper<TSource, TDestination>(IEnumerable<IPropertyMap> propertiesMap, TSource source, TDestination destination)
        {
            Action<IMapperConfigurationExpression> expression = x =>
            {
                var map = x.CreateMap<TSource, TDestination>();

                foreach (var item in propertiesMap)
                {
                    var peDestination = Expression.Parameter(destination.GetType(), "dest");
                    var peSource = Expression.Parameter(source.GetType(), "src");

                    if (item.Destination.IndexOf('.') >= 0)
                    {

                    }
                    else
                    {
                        Expression destinationExpression = Expression.Property(peDestination, item.Destination);
                        Expression sourceExpression = Expression.Property(peSource, item.Source);

                        // source => source.Property
                        Expression <Func<TSource, object>> sourceMapFromExpression =
                            Expression.Lambda<Func<TSource, object>>(Expression.Convert(sourceExpression, typeof(object)),
                                new ParameterExpression[] { peSource });

                        // dest => src.Property
                        Expression<Func<TDestination, object>> destinationMapFromExpression =
                              Expression.Lambda<Func<TDestination, object>>(Expression.Convert(destinationExpression, typeof(object)),
                                new ParameterExpression[] { peDestination });

                        map.ForMember(destinationMapFromExpression, a => a.MapFrom(sourceMapFromExpression));
                    }
                }
            };

            configuration = new MapperConfiguration(expression);
            var mapper = configuration.CreateMapper();

            var result = mapper.Map<Order>(source);
        }

        private static void MyImplementation(List<EcommerceOrderDTO> nonIntegratedOrders, List<MappingField> mapping, List<Order> orders)
        {
            var watch = Stopwatch.StartNew();
            foreach (var item in nonIntegratedOrders)
            {
                var newOrder = new Order();
                MappingFields(mapping, item, newOrder);
                orders.Add(newOrder);
            }
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);


        }

        private static void MappingFields<TSource, TDestination>(IEnumerable<MappingField> mapping, TSource source, TDestination destinantion)
        {
            var destinationType = destinantion.GetType();
            foreach (var map in mapping)
            {
                var valueEcommerceProperty = typeof(TSource).GetProperty(map.EcommerceProperty).GetValue(source);

                if (map.IntegratorProperty.IndexOf('.') >= 0)
                {
                    var propertiesLevel = map.IntegratorProperty.Split('.');

                    PropertyInfo property = null;
                    object navegationFirstLevel = destinantion;
                    object navegationLastLevel = null;
                    for (int i = 0; i < propertiesLevel.Length; i++)
                    {
                        if (i == (propertiesLevel.Length - 1))
                        {
                            property = navegationLastLevel.GetType().GetProperty(propertiesLevel[i]);
                            property.SetValue(navegationLastLevel, valueEcommerceProperty);
                        }
                        else
                        {
                            property = navegationFirstLevel.GetType().GetProperty(propertiesLevel[i]);
                            if (property.GetValue(navegationFirstLevel, null) is null)
                            {
                                navegationLastLevel = Activator.CreateInstance(property.PropertyType);
                                property.SetValue(navegationFirstLevel, navegationLastLevel);
                            }
                            else
                            {
                                navegationLastLevel = navegationFirstLevel;
                                navegationFirstLevel = property.GetValue(navegationFirstLevel, null);
                            }
                        }
                    }
                }
                else
                {
                    destinationType.GetProperty(map.IntegratorProperty).SetValue(destinantion, valueEcommerceProperty);
                }
            }
        }
    }

    public class MyDynamicObject : DynamicObject
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public override bool TryGetMember(
        GetMemberBinder binder, out object result)
        {
            string name = binder.Name.ToLower();
            return dictionary.TryGetValue(name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            dictionary[binder.Name.ToLower()] = value;
            return true;
        }
    }
}
