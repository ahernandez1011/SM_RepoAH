using Dapper;
using Practica2_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace Practica2_API.Data
{
    public interface IClienteRepository
    {
        Task<Cliente> GetByIdAsync(long idCliente);
        Task<Cliente> GetByCedulaAsync(string cedula);
        Task<List<Cliente>> GetAllAsync();
        Task<(bool Success, long IdCliente, string Message)> InsertAsync(ClienteCreateRequest request);
        Task<(bool Success, string Message)> UpdateAsync(long idCliente, ClienteCreateRequest request);
        Task<(bool Success, string Message)> DeleteAsync(long idCliente);
    }

    public class ClienteRepository : IClienteRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public ClienteRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<Cliente> GetByIdAsync(long idCliente)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdCliente", idCliente);

                var cliente = await connection.QuerySingleOrDefaultAsync<Cliente>(
                    "sp_ObtenerClientePorId",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return cliente;
            }
        }

        public async Task<Cliente> GetByCedulaAsync(string cedula)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Cedula", cedula);

                var cliente = await connection.QuerySingleOrDefaultAsync<Cliente>(
                    "sp_ObtenerClientePorCedula",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return cliente;
            }
        }

        public async Task<List<Cliente>> GetAllAsync()
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                var clientes = await connection.QueryAsync<Cliente>(
                    "sp_ObtenerTodosLosClientes",
                    commandType: CommandType.StoredProcedure);

                return clientes.ToList();
            }
        }

        public async Task<(bool Success, long IdCliente, string Message)> InsertAsync(ClienteCreateRequest request)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(request.Cedula))
                return (false, 0, "La cédula es requerida");

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return (false, 0, "El nombre es requerido");

            if (string.IsNullOrWhiteSpace(request.Correo))
                return (false, 0, "El correo es requerido");
            try
            {
                using (var connection = _connectionProvider.GetConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Cedula", request.Cedula);
                    parameters.Add("@Nombre", request.Nombre);
                    parameters.Add("@Correo", request.Correo);
                    parameters.Add("@IdCliente", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync(
                        "sp_InsertarCliente",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    var idCliente = parameters.Get<long>("@IdCliente");
                    return (true, idCliente, "Cliente registrado exitosamente");
                }
            }
            catch (SqlException ex)
            {
                return (false, 0, ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(long idCliente, ClienteCreateRequest request)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(request.Cedula))
                return (false, "La cédula es requerida");

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return (false, "El nombre es requerido");

            if (string.IsNullOrWhiteSpace(request.Correo))
                return (false, "El correo es requerido");

            try
            {
                using (var connection = _connectionProvider.GetConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdCliente", idCliente);
                    parameters.Add("@Cedula", request.Cedula);
                    parameters.Add("@Nombre", request.Nombre);
                    parameters.Add("@Correo", request.Correo);

                    await connection.ExecuteAsync(
                        "sp_ActualizarCliente",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    return (true, "Cliente actualizado exitosamente");
                }
            }
            catch (SqlException ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(long idCliente)
        {
            try
            {
                using (var connection = _connectionProvider.GetConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdCliente", idCliente);

                    await connection.ExecuteAsync(
                        "sp_EliminarCliente",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    return (true, "Cliente eliminado exitosamente");
                }
            }
            catch (SqlException ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
