using System.ComponentModel.DataAnnotations;

namespace SM_API_AH.Models
{
    public class RegistroSolicitudRequestModel
    {
        [Required]
        public string Titulo { get; set; } = string.Empty;
        [Required]
        public string Descripcion { get; set; } = string.Empty;
    }
}
