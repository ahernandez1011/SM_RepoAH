using SM_API_AH.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<IUtilesService, UtilesService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//Middleware de Errores
app.UseExceptionHandler("/api/Error/RegistrarError");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
