namespace SM_API_AH.Models
{
    public class MensajeResponseModel
    {
        public int Consecutivo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public int ConsecutivoUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
    }
}
