using Dapper;
using Practica2_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace Practica2_API.Data
{
    public interface IMascotaRepository
    {
        Task<Mascota> GetByIdAsync(long idMascota);
        Task<List<Mascota>> GetAllAsync();
        Task<List<Mascota>> GetByClienteAsync(long idCliente);
        Task<(bool Success, long IdMascota, string Message)> InsertAsync(MascotaCreateRequest request);
        Task<(bool Success, string Message)> UpdateAsync(long idMascota, MascotaCreateRequest request);
        Task<(bool Success, string Message)> DeleteAsync(long idMascota);
    }

    public class MascotaRepository : IMascotaRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public MascotaRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<Mascota> GetByIdAsync(long idMascota)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdMascota", idMascota);

                var mascota = await connection.QuerySingleOrDefaultAsync<Mascota>(
                    "sp_ObtenerMascotaPorId",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return mascota;
            }
        }

        public async Task<List<Mascota>> GetAllAsync()
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                var mascotas = await connection.QueryAsync<Mascota>(
                    "sp_ObtenerTodasLasMascotas",
                    commandType: CommandType.StoredProcedure);

                return mascotas.ToList();
            }
        }

        public async Task<List<Mascota>> GetByClienteAsync(long idCliente)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdCliente", idCliente);

                var mascotas = await connection.QueryAsync<Mascota>(
                    "sp_ObtenerMascotasPorCliente",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return mascotas.ToList();
            }
        }

        public async Task<(bool Success, long IdMascota, string Message)> InsertAsync(MascotaCreateRequest request)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(request.Nombre))
                return (false, 0, "El nombre de la mascota es requerido");

            if (string.IsNullOrWhiteSpace(request.Especie))
                return (false, 0, "La especie es requerida");

            if (string.IsNullOrWhiteSpace(request.Raza))
                return (false, 0, "La raza es requerida");

            if (request.Peso <= 0)
                return (false, 0, "El peso debe ser mayor a 0");

            if (request.IdCliente <= 0)
                return (false, 0, "El cliente es requerido");

            try
            {
                using (var connection = _connectionProvider.GetConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Nombre", request.Nombre);
                    parameters.Add("@Especie", request.Especie);
                    parameters.Add("@Raza", request.Raza);
                    parameters.Add("@Peso", request.Peso);
                    parameters.Add("@IdCliente", request.IdCliente);
                    parameters.Add("@IdMascota", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync(
                        "sp_InsertarMascota",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    var idMascota = parameters.Get<long>("@IdMascota");
                    return (true, idMascota, "Mascota registrada exitosamente");
                }
            }
            catch (SqlException ex)
            {
                return (false, 0, ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(long idMascota, MascotaCreateRequest request)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(request.Nombre))
                return (false, "El nombre de la mascota es requerido");

            if (string.IsNullOrWhiteSpace(request.Especie))
                return (false, "La especie es requerida");

            if (string.IsNullOrWhiteSpace(request.Raza))
                return (false, "La raza es requerida");

            if (request.Peso <= 0)
                return (false, "El peso debe ser mayor a 0");

            if (request.IdCliente <= 0)
                return (false, "El cliente es requerido");

            try
            {
                using (var connection = _connectionProvider.GetConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdMascota", idMascota);
                    parameters.Add("@Nombre", request.Nombre);
                    parameters.Add("@Especie", request.Especie);
                    parameters.Add("@Raza", request.Raza);
                    parameters.Add("@Peso", request.Peso);
                    parameters.Add("@IdCliente", request.IdCliente);

                    await connection.ExecuteAsync(
                        "sp_ActualizarMascota",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    return (true, "Mascota actualizada exitosamente");
                }
            }
            catch (SqlException ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(long idMascota)
        {
            try
            {
                using (var connection = _connectionProvider.GetConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdMascota", idMascota);

                    await connection.ExecuteAsync(
                        "sp_EliminarMascota",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    return (true, "Mascota eliminada exitosamente");
                }
            }
            catch (SqlException ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
