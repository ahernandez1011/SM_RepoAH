namespace SM_API_AH.Services
{
    public interface IUtilesService
    {
        string GenerarContrasena();
        Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml);
        string GenerarToken(int consecutivo);
        int ObtenerConsecutivoToken();
    }
}
