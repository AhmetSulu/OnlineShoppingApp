using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineShoppingApp.Business.DataProtection;
using OnlineShoppingApp.Business.Operations.Order;
using OnlineShoppingApp.Business.Operations.Products;
using OnlineShoppingApp.Business.Operations.Setting;
using OnlineShoppingApp.Business.Operations.User;
using OnlineShoppingApp.Data.Context;
using OnlineShoppingApp.Data.Repositories;
using OnlineShoppingApp.Data.UnitOfWork;
using OnlineShoppingApp.WebApi.Middlewares;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };
    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }

    });

});

// Sql Server Connection
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<OnlineShoppingAppDbContext>(options => options.UseSqlServer(connectionString));

// Services
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<IOrderService, OrderManager>();
builder.Services.AddScoped<ISettingService, SettingManager>();
builder.Services.AddScoped<IDataProtection, DataProtection>();

// Protecting user password
var keysDirectory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys"));

builder.Services.AddDataProtection()
       .SetApplicationName("OnlineShoppingApp")
       .PersistKeysToFileSystem(keysDirectory);

// JWT Token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandling();

app.UseMaintenanceMode();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();