namespace Practica2_API.Models
{
    public class MascotaCreateRequest
    {
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public decimal Peso { get; set; }
        public long IdCliente { get; set; }
    }
}
