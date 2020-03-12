using System;

namespace POC.MappedFieldToAnother.DTO.Order
{
    public class ERPOrderDTO
    {
        public int Product { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public int QuantityProduct { get; set; }

    }
}
