namespace Suscripciones.Entities
{
    public class RestriccionIP
    {
        public int Id { get; set; }
        public int LlaveId { get; set; }
        public string Ip { get; set; }
        public LlaveAPI Llave { get; set; }
    }
}
