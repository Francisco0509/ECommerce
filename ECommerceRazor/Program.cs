using ECommerceRazor.DataAccess;
using ECommerceRazor.DataAccess.Repository;
using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using ECommerceRazor.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlConn")
        )
);


//Soporte para la configuración de Stripe
builder.Services.Configure<ConfiguracionStripe>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
//.AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider);

//Configurar requerimiento de confirmación de Email
builder.Services.Configure<IdentityOptions>(options => {
    options.SignIn.RequireConfirmedAccount = true;
});

//Soporte para cooockies de Autenticación y Autorización
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login"; //Ruta predeterminada de inicio de sesión
    options.LogoutPath = $"/Identity/Account/Logout"; //Ruta predeterminada de cierre de sesión
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied"; //Ruta predeterminada de acceso denegado
    options.SlidingExpiration = true; //Renueva la cookie si el usuario está activo
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); //Tiempo de expiración de la cookie
    options.Cookie.HttpOnly = true; //La cookie no es accesible desde JavaScript
});

builder.Services.AddDistributedMemoryCache();
//Soporte para trabajo con sesiones
builder.Services.AddSession(options => { 
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//Agregar soporte para EmailSender
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

string key = builder.Configuration.GetSection("Stripe:ClaveSecreta").Get<string>();
StripeConfiguration.ApiKey = key;

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

//Redirigir manualmente a la p�gina de inicio
app.MapGet("/", context => { 
    context.Response.Redirect("/Cliente/Inicio/Index");
    return Task.CompletedTask;
});

app.Run();
