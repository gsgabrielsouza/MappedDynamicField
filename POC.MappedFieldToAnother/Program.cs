using AutoMapper;
using POC.MappedFieldToAnother.Domain.Entities;
using POC.MappedFieldToAnother.Domain.Service;
using POC.MappedFieldToAnother.DTO.Order;
using POC.MappedFieldToAnother.Integration.Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace POC.MappedFieldToAnother
{
    class Program
    {
        private static MapperConfiguration configuration;

        static Program()
        {
            configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EcommerceOrderDTO, Order>();
                //cfg.CreateMap<Bar, BarDto>();
            });
        }

        static void Main(string[] args)
        {
            var ecommerceOrderService = new EcommerceOrderService();
            var mappingFieldService = new MappingFieldService();

            var nonIntegratedOrders = ecommerceOrderService.GetNonIntegraded();
            var mapping = mappingFieldService.GetFields();

            var orders = new List<Order>();
            foreach (var item in nonIntegratedOrders)
            {
                var newOrder = new Order();
                foreach (var map in mapping)
                {
                    var valueEcommerceProperty = item.GetType().GetProperty(map.EcommerceProperty).GetValue(item);

                    // preencher valor novo pedido
                    if (map.IntegratorProperty.IndexOf('.') >= 0)
                    {
                        var propertiesLevel = map.IntegratorProperty.Split('.');

                        PropertyInfo integratorProperty = null;
                        object instanceNavegationProperty = null;
                        for (int i = 0; i < propertiesLevel.Length; i++)
                        {
                            if (i == (propertiesLevel.Length - 1))
                            {
                                integratorProperty = instanceNavegationProperty.GetType().GetProperty(propertiesLevel[i]);
                                integratorProperty.SetValue(instanceNavegationProperty, valueEcommerceProperty);
                            }
                            else
                            {
                                integratorProperty = newOrder.GetType().GetProperty(propertiesLevel[i]);
                                instanceNavegationProperty = Activator.CreateInstance(integratorProperty.PropertyType);
                                integratorProperty.SetValue(newOrder, instanceNavegationProperty);
                            }
                        }
                    }
                    else
                    {
                        newOrder.GetType().GetProperty(map.IntegratorProperty).SetValue(newOrder, valueEcommerceProperty);
                    }
                }
                orders.Add(newOrder);
            }

            foreach (var item in orders)
                Console.WriteLine(JsonSerializer.Serialize(item));

            Console.ReadKey();
        }
    }
}
