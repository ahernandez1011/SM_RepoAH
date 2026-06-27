using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using CasoEstudio.Web.Models;

namespace CasoEstudio.Web.Data
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IConfiguration _config;
        private readonly string _conn;

        public TicketRepository(IConfiguration config)
        {
            _config = config;
            _conn = _config.GetConnectionString("CasoEstudio") ?? throw new InvalidOperationException("Connection string 'CasoEstudio' not found.");
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync()
        {
            await using var conn = new SqlConnection(_conn);
            var result = await conn.QueryAsync<Ticket>("dbo.GetTickets", commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<(int code, string message)> InsertTicketAsync(Ticket t)
        {
            await using var conn = new SqlConnection(_conn);

            // Regla de negocio: no permitir más de 2 tickets por tipo de vehículo
            var existingCount = await conn.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM dbo.Tickets WHERE TipoVehiculo = @TipoVehiculo", new { t.TipoVehiculo });
            if (existingCount >= 2)
            {
                return (1, "No se pueden registrar más de 2 tickets para este tipo de vehículo.");
            }

            var p = new DynamicParameters();
            p.Add("@PlacaVehiculo", t.PlacaVehiculo);
            p.Add("@FechaIngreso", t.FechaIngreso);
            p.Add("@MontoTotal", t.MontoTotal);
            p.Add("@TipoVehiculo", t.TipoVehiculo);
            p.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@ResultMessage", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("dbo.InsertTicket", p, commandType: CommandType.StoredProcedure);
            return (p.Get<int>("@ResultCode"), p.Get<string>("@ResultMessage") ?? string.Empty);
        }

        public async Task<IEnumerable<TipoVehiculo>> GetTiposAsync()
        {
            await using var conn = new SqlConnection(_conn);
            var sql = "SELECT TipoVehiculo AS TipoVehiculoId, DescripcionTipo FROM dbo.TiposVehiculos ORDER BY TipoVehiculo";
            var res = await conn.QueryAsync<TipoVehiculo>(sql);
            return res;
        }
    }
}
