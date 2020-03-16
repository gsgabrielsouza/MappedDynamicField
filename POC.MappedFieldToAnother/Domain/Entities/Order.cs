using MapDynamicField.TransferObject;
using System;

namespace POC.MappedFieldToAnother.Domain.Entities
{
    public class Order : IDestination
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal AmountOrder { get; set; }
        public int Quantity { get; set; }

        //public Shipping Shipping { get; set; }
    }
}
