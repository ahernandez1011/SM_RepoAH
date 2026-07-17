using System.ComponentModel.DataAnnotations;

namespace SM_API_AH.Models
{
    public class CambiarPerfilRequestModel
    {
        [Required]
        public int Consecutivo { get; set; }
        [Required]
        public string Identificacion { get; set; } = string.Empty;
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public string CorreoElectronico { get; set; } = string.Empty;
    }
}
