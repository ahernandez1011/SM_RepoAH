namespace Practica2_API.Models
{
    public class Mascota
    {
        public long IdMascota { get; set; }
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public decimal Peso { get; set; }
        public long IdCliente { get; set; }
        public string? ClienteCedula { get; set; }
        public string? ClienteNombre { get; set; }
    }
}
