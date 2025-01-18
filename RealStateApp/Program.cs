using Microsoft.AspNetCore.Authentication.Cookies;
using RealStateApp.Core.Application;
using RealStateApp.Infraestructure.Identity;
using RealStateApp.Infraestructure.Persistence;
using RealStateApp.Infraestructure.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationLayerWebApp();
builder.Services.AddSharedInfraestructureLayer(builder.Configuration);
builder.Services.AddIdentityInfraestructureLayerWebApp(builder.Configuration);
builder.Services.AddPersistenceInfraestructureLayer(builder.Configuration);



var app = builder.Build();

var response = await app.Services.SeedIdentityDbAsync();


if (response != null)
{
    await app.Services.SeedDefaulEntitiesAsync(response);
}

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
