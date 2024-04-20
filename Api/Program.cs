using Application.Middleware;
using Application.Services;
using Persistence;
using Persistence.Seeds;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using Application.Settings;
using Microsoft.IdentityModel.Tokens;
using Persistence.Mapper;
using Applicaton.Settings;
using Core.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("jwtSettings.json");
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddDbContext<RickAndMortyDbContext>(options =>
{
    options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString") ?? builder.Configuration.GetConnectionString("Db"));
});


builder.Services.AddLogging().AddSerilog();
Application.ServiceExtensions.LoggerExtensions.ConfigureLogging();

Log.Information("Logger Started");



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });




builder.Services.AddScoped<JwtGenerator>();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
}
);
builder.Services.ExternalServices();
builder.Services.AddHttpContextAccessor();


// cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000",
            "https://test.intelligrade.xyz",
            "https://www.intelligrade.xyz",
            "https://intelligrade.xyz"
            )
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var migrationSvcScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    try
    {
        var context = migrationSvcScope.ServiceProvider.GetService<RickAndMortyDbContext>().Database;
        var migrations = context.GetPendingMigrations();
        await context.MigrateAsync();
    }
    catch (Exception)
    {

    }
}

AdminSeed.SeedAdmin(app.Services).Wait();


app.UseMiddleware<CustomExceptionHandler>();


if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(Response<NoContent>.Fail(404, "Not Found"));
    }else if(context.Response.StatusCode == 401 && !context.Response.HasStarted)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(Response<NoContent>.Fail(401, "Unauthorized"));
    }
});

app.UseAuthorization();

app.MapControllers();

app.Run();
