using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Middleware de Errores
app.UseExceptionHandler("/api/Error/RegistrarError");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
