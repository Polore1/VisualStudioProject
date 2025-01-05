using Microsoft.EntityFrameworkCore;
using ProjectWeb.Data;

// Program.cs
using Autofac.Extensions.DependencyInjection;
using ProjectWeb.Infrastructure;
using ProjectWeb.Interfaces;
using ProjectWeb.Services;
using Autofac;
using Autofac.Core;

var builder = WebApplication.CreateBuilder(args);

// Configurează Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddScoped<IZborService, ZborService>();
builder.Services.AddScoped<IUtilizatorService, UtilizatorService>(); // Adaugă serviciul pentru utilizatori
builder.Services.AddScoped<ICheckinService, CheckinService>(); // Adaugă serviciul pentru check-in
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

// Integrarea Autofac prin ContainerConfigurer
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Folosește direct ContainerConfigurer pentru a înregistra dependențele
    ContainerConfigurer.ConfigureContainer(builder.Services);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

//inject the DbContext
//nume baza de date creata = "Calatorii" 
builder.Services.AddDbContext<ApplicationDB>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("Calatorii"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Calatorii"))));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Zbor}/{action=List}/{id?}");

app.Run();
