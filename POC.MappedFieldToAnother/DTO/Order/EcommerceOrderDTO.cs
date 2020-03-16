using MapDynamicField.TransferObject;
using System;

namespace POC.MappedFieldToAnother.DTO.Order
{
    public class EcommerceOrderDTO : ISource
    {
        public EcommerceOrderDTO()
        {

        }
        public EcommerceOrderDTO(DateTime dueDate, decimal grossAmount, int productId, int quantity, string shipping)
        {
            DueDate = dueDate;
            GrossAmount = grossAmount;
            ProductId = productId;
            Quantity = quantity;
            //Shipping = shipping;
        }

        public DateTime DueDate { get; set; }
        public decimal GrossAmount { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        //public string Shipping { get; set; }
    }
}
