using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using SM_API_AH.Models;
using SM_API_AH.Services;

namespace SM_API_AH.Hubs
{
    [Authorize]
    public class ChatHub(IConfiguration _config, IUtilesService _utiles) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var consecutivo = _utiles.ObtenerConsecutivoToken();
            await Groups.AddToGroupAsync(Context.ConnectionId, $"usuario-{consecutivo}");
            await base.OnConnectedAsync();
        }

        public async Task UnirseASala(int consecutivoSolicitud)
        {
            if (!TieneAcceso(consecutivoSolicitud))
                throw new HubException("Acceso denegado a esta sala.");

            await Groups.AddToGroupAsync(Context.ConnectionId, $"solicitud-{consecutivoSolicitud}");
        }

        public async Task EnviarMensaje(int consecutivoSolicitud, string mensaje)
        {
            if (!TieneAcceso(consecutivoSolicitud))
                throw new HubException("Acceso denegado a esta sala.");

            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@ConsecutivoUsuario", _utiles.ObtenerConsecutivoToken());
            parameters.Add("@ConsecutivoSolicitud", consecutivoSolicitud);
            parameters.Add("@Mensaje", mensaje);
            var consecutivo = context.QuerySingle<int>("spRegistrarMensaje", parameters,
                commandType: System.Data.CommandType.StoredProcedure);

            var modelo = new MensajeResponseModel
            {
                Consecutivo = consecutivo,
                Mensaje = mensaje,
                FechaHora = DateTime.Now,
                ConsecutivoUsuario = _utiles.ObtenerConsecutivoToken(),
                NombreUsuario = _utiles.ObtenerNombreToken()
            };

            await Clients.Group($"solicitud-{consecutivoSolicitud}").SendAsync("RecibirMensaje", modelo);

            // Notify the other participant so they see the badge even when not on the chat page
            var notifParams = new DynamicParameters();
            notifParams.Add("@ConsecutivoSolicitud", consecutivoSolicitud);
            notifParams.Add("@ConsecutivoUsuario", _utiles.ObtenerConsecutivoToken());
            var otroUsuario = context.QuerySingleOrDefault<int?>("spObtenerInterlocutorSolicitud", notifParams,
                commandType: System.Data.CommandType.StoredProcedure);

            if (otroUsuario.HasValue)
                await Clients.Group($"usuario-{otroUsuario.Value}").SendAsync("NuevoMensaje");
        }

        private bool TieneAcceso(int consecutivoSolicitud)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@ConsecutivoSolicitud", consecutivoSolicitud);
            parameters.Add("@ConsecutivoUsuario", _utiles.ObtenerConsecutivoToken());
            return context.QuerySingle<int>("spValidarAccesoSolicitud", parameters,
                commandType: System.Data.CommandType.StoredProcedure) > 0;
        }
    }
}
