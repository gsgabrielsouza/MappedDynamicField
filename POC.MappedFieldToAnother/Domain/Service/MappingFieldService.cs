using POC.MappedFieldToAnother.Domain.Entities;
using System.Collections.Generic;

namespace POC.MappedFieldToAnother.Domain.Service
{
    public class MappingFieldService
    {
        public MappingFieldService()
        {
        }
        //public MappingFieldService(IMappingFieldRepository mappingFieldRepository) { }

        public List<MappingField> GetFields()
        {
            return new List<MappingField>
            {
                new MappingField(1, "ProductId", "Product", "ProductId"),
                new MappingField(2, "OrderDate", "PurchaseDate", "DueDate"),
                new MappingField(3, "AmountOrder", "TotalOrderAmount", "GrossAmount"),
                new MappingField(4, "Quantity", "Quantity", "Quantity"),
                new MappingField(5, "Shipping.Method", "ShippingMethod", "Shipping"),
                new MappingField(5, "Shipping.Teste.Testando", "Quantity", "Quantity"),
            };
        }
    }
}
