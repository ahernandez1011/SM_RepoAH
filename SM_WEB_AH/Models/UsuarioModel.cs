namespace SM_WEB_AH.Models
{
    public class UsuarioModel
    {
        public int Consecutivo { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Contrasenna { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public bool IndicadorTemp { get; set; }
    }
}
