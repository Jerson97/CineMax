using Cinemax.Application;
using Cinemax.Persistence;
using Cinemax.WebApi;
using Cinemax.WebApi.Middleware.ErrorMiddlewares;

var builder = WebApplication.CreateBuilder(args);

//Configuracion de Persistence
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddIdentityAndJwt(builder.Configuration);

// Controladores protegidos por defecto
//builder.Services.AddControllers(options =>
//{
//    var policy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser() 
//        .Build();
//    options.Filters.Add(new AuthorizeFilter(policy)); 
//});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await app.MigrateAndSeedAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
