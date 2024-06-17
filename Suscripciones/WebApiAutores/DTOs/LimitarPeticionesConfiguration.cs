namespace Suscripciones.DTOs
{
    public class LimitarPeticionesConfiguration
    {
        public int PeticionesPorDiaGratuito { get; set; }
        public string[] ListaBlancaRutas { get; set; }
    }
}
