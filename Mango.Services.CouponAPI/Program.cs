using Mango.Services.CouponAPI;
using Mango.Services.CouponAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(typeof(MappingConfig).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API V1");
    });
}

app.UseHttpsRedirection();
ApplyMigrations();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

void ApplyMigrations()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (_db.Database.GetPendingMigrations().Any())
        {
            _db.Database.Migrate();
        }
    }
}

//using Mango.Services.CouponAPI.Data;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using System;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);
//var configuration = builder.Configuration;

// Add services to the container.
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
//});

// Configure Authentication
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.SaveToken = true;
//        options.RequireHttpsMetadata = false;
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidAudience = configuration["ApiSettings:JwtOptions:Audience"],
//            ValidIssuer = configuration["ApiSettings:JwtOptions:Issuer"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["ApiSettings:JwtOptions:Secret"]))
//        };
//    });

// Configure Authorization
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Bearer", policy =>
//    {
//        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
//        policy.RequireAuthenticatedUser();
//    });
//});

//builder.Services.AddControllers();

// Configure Swagger
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Coupon API", Version = "v1" });

//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.Http,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
//            },
//            new string[] { }
//        }
//    });
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Coupon API v1"));
//}

//app.UseHttpsRedirection();
//ApplyMigrations();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();

//void ApplyMigrations()
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//        if (dbContext.Database.GetPendingMigrations().Any())
//        {
//            dbContext.Database.Migrate();
//        }
//    }
//}
