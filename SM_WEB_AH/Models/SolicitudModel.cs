namespace SM_WEB_AH.Models
{
    public class SolicitudModel
    {
        public int Consecutivo { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public string NombreAdmin { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string NombreEstado { get; set; } = string.Empty;
    }
}
