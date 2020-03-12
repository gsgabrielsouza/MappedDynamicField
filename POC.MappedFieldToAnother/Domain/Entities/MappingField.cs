namespace POC.MappedFieldToAnother.Domain.Entities
{
    public class MappingField
    {
        public MappingField()
        {

        }
        public MappingField(int id, string integratorProperty, string erpProperty, string ecommerceProperty)
        {
            Id = id;
            IntegratorProperty = integratorProperty;
            ErpProperty = erpProperty;
            EcommerceProperty = ecommerceProperty;
        }

        public int Id { get; set; }
        public string IntegratorProperty { get; set; }
        public string ErpProperty { get; set; }
        public string EcommerceProperty { get; set; }
    }
}
