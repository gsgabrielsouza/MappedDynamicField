using POC.MappedFieldToAnother.DTO.Order;
using System;
using System.Collections.Generic;

namespace POC.MappedFieldToAnother.Integration.Ecommerce
{
    public class EcommerceOrderService
    {
        public EcommerceOrderService()
        {

        }

        public List<EcommerceOrderDTO> GetNonIntegraded()
        {
            List<EcommerceOrderDTO> list = new List<EcommerceOrderDTO>
            {
                new EcommerceOrderDTO(DateTime.UtcNow, 10, 1, 3, "sedex"),
                new EcommerceOrderDTO(DateTime.UtcNow, 10, 1, 3, "sedex"),
                new EcommerceOrderDTO(DateTime.UtcNow, 15, 2, 3, "sedex"),
                new EcommerceOrderDTO(DateTime.UtcNow, 50, 3, 3, "sedex")
            };
            return list;
        }
    }
}
