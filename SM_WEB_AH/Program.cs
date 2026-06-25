var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//Dependencias
builder.Services.AddHttpClient();
builder.Services.AddSession();

var app = builder.Build();

app.UseExceptionHandler("/Error/CapturarError");

app.UseSession();

app.UseHsts();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
