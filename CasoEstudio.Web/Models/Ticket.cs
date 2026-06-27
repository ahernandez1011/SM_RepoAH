using System.ComponentModel.DataAnnotations;

namespace CasoEstudio.Web.Models
{
    public class Ticket
    {
        public long Consecutivo { get; set; }

        [Required(ErrorMessage = "La placa es obligatoria.")]
        [StringLength(10, ErrorMessage = "La placa no puede tener más de 10 caracteres.")]
        public string PlacaVehiculo { get; set; } = string.Empty;

        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "El monto total es obligatorio.")]
        [Range(0.01, 9999999.99, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal MontoTotal { get; set; }

        [Required(ErrorMessage = "El tipo de vehículo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de vehículo.")]
        public int TipoVehiculo { get; set; }

        // Para consultas con join
        public string? DescripcionTipo { get; set; }
    }
}
