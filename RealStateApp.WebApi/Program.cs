using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application;
using RealStateApp.Infraestructure.Identity;
using RealStateApp.Infraestructure.Persistence;
using RealStateApp.WebApi.Extensions;
using RealStateApp.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressInferBindingSourcesForParameters = true;
    options.SuppressMapClientErrors = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApplicationLayerWebApi();
builder.Services.AddPersistenceInfraestructureLayer(builder.Configuration);
builder.Services.AddIdentityInfraestructureLayerWebApi(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerExtension();
builder.Services.AddApiVersioningExtension();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

var response = await app.Services.SeedIdentityDbAsync();

if (response != null)
{
    await app.Services.SeedDefaulEntitiesAsync(response);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerExtension(app);
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();

app.UseHealthChecks("/health");

app.MapControllers();

await app.RunAsync();