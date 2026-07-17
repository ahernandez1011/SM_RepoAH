using System.ComponentModel.DataAnnotations;

namespace SM_API_AH.Models
{
    public class CambiarAccesoRequestModel
    {
        [Required]
        public int Consecutivo { get; set; }
        [Required]
        public string Contrasenna { get; set; } = string.Empty;
    }
}