using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Rotativa.AspNetCore;
using System.Reflection;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Implementaciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Implementaciones;
using Unna.OperationalReport.Service.Infraestructura.Modulos;
using Unna.OperationalReport.Tools.Cargadores.Bd;
using Unna.OperationalReport.Tools.Cargadores.Generales;
using Unna.OperationalReport.Tools.Seguridad.Infraestructura.Modulos;
using Unna.OperationalReport.Tools.WebComunes.Infraestructura.Errores;
using Unna.OperationalReport.WebSite;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null).AddNewtonsoftJson();
//builder.Services.AddAuthentication().AddMicrosoftAccount(opciones =>
//{
//    opciones.ClientId = builder.Configuration["MicrosoftClientId"];
//    opciones.ClientSecret = builder.Configuration["MicrosoftSecretId"];
//    //opciones.CallbackPath = "/signin-microsoft";
//    //opciones.SignInScheme = "/RegisterExternalUser";

//});
//builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
//opciones.UseSqlServer("name=operacional"));

//builder.Services.AddIdentity<IdentityUser, IdentityRole>(opciones =>
//{
//    opciones.SignIn.RequireConfirmedAccount = false;
//})
//.AddEntityFrameworkStores<ApplicationDbContext>()
//.AddDefaultTokenProviders();


//builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
//    options =>
//    {
//        options.LoginPath = "/Admin/Login";
//        options.AccessDeniedPath = "/Admin/Login";
//        options.ReturnUrlParameter = "/Admin/Index";
//    });

builder.Services.AddRazorPages(
              options =>
              {
                  options.Conventions.AuthorizeFolder("/Admin");
                  //options.Conventions.AllowAnonymousToPage("/Admin/Index");
                  options.Conventions.AllowAnonymousToPage("/Admin/Login");

              })
          .AddRazorRuntimeCompilation()
          .AddRazorPagesOptions(options =>
          {
              options.Conventions.AddPageRoute("/Admin/Index", "");
          });




builder.Services.AddAutofac();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(config =>
    {
        config.LoginPath = "/Admin/Login";
    });

builder.Services.AddAutoMapper(Assembly.Load("Unna.OperationalReport.Service"));

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{

    containerBuilder.RegisterModule(new CifradoModule(builder.Configuration));

    containerBuilder.RegisterModule(new ConfiguracionModule(builder.Configuration));
    //containerBuilder.RegisterModule(new ConfiguracionModule(builder.Configuration));
    containerBuilder.RegisterModule(new ServicioCargador("Unna.OperationalReport.Service"));
    containerBuilder.RegisterModule(new SeguridadModule(builder.Configuration));


    containerBuilder.RegisterModule(new
        CargarSqlServerEF<OperacionalContexto,
            IOperacionalConfiguracion,
            OperacionalConfiguracion, IOperacionalUnidadDeTrabajo>(builder.Configuration, "operacional", "Unna.OperationalReport.Data"));


});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    app.UseHsts();
//}

app.UseHttpsRedirection();

app.UseCors(builder =>
{
    builder.AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .Build();
});


app.UseStaticFiles();

app.UseRouting();
app.UseExceptionHandler(errorApp => errorApp.UseCustomErrors(app.Environment, false));


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.MapControllers();
app.MapRazorPages();
app.Run();
