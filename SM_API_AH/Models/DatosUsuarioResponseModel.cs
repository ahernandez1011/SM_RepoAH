using System.ComponentModel.DataAnnotations;

namespace SM_API_AH.Models
{
    public class DatosUsuarioResponseModel
    {
        public string Identificacion { get; set; } = string.Empty;  
        public string Nombre { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Contrasenna { get; set; } = string.Empty;
    }
}
