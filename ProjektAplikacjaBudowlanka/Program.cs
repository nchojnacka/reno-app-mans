using ProjektAplikacjaBudowlanka;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BudowlankaDBContext>(options =>
{
<<<<<<< HEAD
    options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;");
=======
    options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;User ID=sa;Password=admin;");
>>>>>>> 00d653865c8f46965b877b509419c2490813b7da
}); 

builder.WebHost.UseKestrel(options =>
{
    options.Limits.MaxConcurrentConnections = 100;
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddAuthentication(options=>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddCookie(c=>
    {
        c.LoginPath = "/Home/Login";
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9")),
            ValidateLifetime = true,
        };
    });
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

app.UseAuthentication()
   .Use(async (context, next) =>
   {
       await next();
   });
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
            name: "pracownicy",
            pattern: "pracownicy/{action=Index}/{id?}",
            defaults: new { controller = "Pracownik" });

    endpoints.MapControllerRoute(
        name: "auth",
        pattern: "auth/{action=Index}/{id?}",
        defaults: new { controller = "Auth" });

    endpoints.MapControllerRoute(
         name: "user",
         pattern: "user/{action=Index}/{id?}",
         defaults: new { controller = "User" });

    endpoints.MapControllerRoute(
          name: "oferta",
          pattern: "oferta/{action=Index}/{id?}",
          defaults: new { controller = "Oferta" });

    endpoints.MapControllerRoute(
      name: "rezerwacja",
      pattern: "rezerwacja/{action=Index}/{id?}",
      defaults: new { controller = "Rezerwacja" });

    endpoints.MapControllerRoute(
      name: "uslugodawca",
      pattern: "uslugodawca/{action=Index}/{id?}",
      defaults: new { controller = "Uslugodawca" });

    endpoints.MapControllerRoute(
      name: "opinia",
      pattern: "opinia/{action=Index}/{id?}",
      defaults: new { controller = "Opinia" });

    endpoints.MapControllerRoute(
      name: "zdjecie",
      pattern: "zdjecie/{action=Index}/{id?}",
      defaults: new { controller = "Zdjecie" });
});


app.Run();
