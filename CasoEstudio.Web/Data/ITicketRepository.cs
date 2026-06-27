using CasoEstudio.Web.Models;

namespace CasoEstudio.Web.Data
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetTicketsAsync();
        Task<(int code,string message)> InsertTicketAsync(Ticket t);
        Task<IEnumerable<TipoVehiculo>> GetTiposAsync();
    }
}
