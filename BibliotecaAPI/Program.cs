using BibliotecaAPI.Datos;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);


#region Services
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirCliente", policy =>
    {
        policy.WithOrigins("https://localhost:7205")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BibliotecaConnection")));

builder.Services.AddScoped<ILibroRepository, LibroRepository>();
builder.Services.AddScoped<IPrestamoRepository, PrestamoRepository>();
builder.Services.AddScoped<ILectorRepository, LectorRepository>();
#endregion

var app = builder.Build();





#region Mapping
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("PermitirCliente");
app.MapControllers();
app.MapRazorPages();
app.MapFallbackToFile("index.html");
#endregion

app.Run();

