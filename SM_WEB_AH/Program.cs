var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//Dependencias
builder.Services.AddHttpClient();
builder.Services.AddSession();

var app = builder.Build();

//Middleware de Errores
app.UseExceptionHandler("/Error/CapturarError");

app.UseSession();

app.UseHsts();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

// Servir PDFs desde fuera de wwwroot para que no se sobreescriban al publicar
var carpetaPdfs = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "Storage", "pdfs"));
Directory.CreateDirectory(carpetaPdfs);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(carpetaPdfs),
    RequestPath = "/pdfs"
});

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();