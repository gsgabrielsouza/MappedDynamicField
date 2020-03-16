namespace POC.MappedFieldToAnother.Domain.Entities
{
    public class Shipping
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public string Carrier { get; set; }

        public Teste Teste { get; set; }
    }
    public class Teste
    {
        public int Testando { get; set; }
    }
}
