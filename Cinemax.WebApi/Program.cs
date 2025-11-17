using Cinemax.Application;
using Cinemax.Persistence;
using Cinemax.WebApi;
using Cinemax.WebApi.Middleware.ErrorMiddlewares;
using Microsoft.OpenApi.Models;

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
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API CineMax", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
});

#region CORS


builder.Services.AddCors(o =>
{
    o.AddPolicy("CorsPolicy",
          builder => builder
              .WithOrigins(new string[] {
                "http://localhost:3000" ,
                "http://localhost:3001" ,
                "http://localhost:5173" ,
                "http://localhost:5174"
              })
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

#endregion

var app = builder.Build();

await app.MigrateAndSeedAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run(); 