
// using ClaimCare.Data;
// using ClaimCare.Mapping;
// using ClaimCare.Services.Interfaces;
// using ClaimCare.Services.Implementations;
// using ClaimCare.Services.Generic;
// using ClaimCare.Services;
// using ClaimCare.Models;
// using Microsoft.OpenApi.Models;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;

// var builder = WebApplication.CreateBuilder(args);

// // ✅ CORS (ADD THIS HERE)
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll",
//         policy => policy.AllowAnyOrigin()
//                         .AllowAnyMethod()
//                         .AllowAnyHeader());
// });

// // Services
// builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
// builder.Services.AddScoped<IPatientRepository, PatientRepository>();
// builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
// builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
// builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<IHealthcareProviderRepository, HealthcareProviderRepository>();
// builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
// builder.Services.AddScoped<IClaimDocumentRepository, ClaimDocumentRepository>();
// builder.Services.AddScoped<IInsuranceCompanyRepository, InsuranceCompanyRepository>();
// builder.Services.AddScoped<IInsurancePlanRepository, InsurancePlanRepository>();
// builder.Services.AddAutoMapper(typeof(Program));

// // Token Service
// builder.Services.AddScoped<TokenService>();

// // Controllers
// builder.Services.AddControllers()
// .AddJsonOptions(options =>
// {
//     options.JsonSerializerOptions.ReferenceHandler =
//         System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
// });

// // AutoMapper
// builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// // Database
// builder.Services.AddDbContext<ClaimCareDbContext>(options =>
//     options.UseSqlServer(
//         builder.Configuration.GetConnectionString("DefaultConnection")
//     )
// );

// builder.Services.AddScoped<EmailService>();

// // Swagger with JWT
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(options =>
// {
//     options.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "ClaimCare API",
//         Version = "v1"
//     });

//     options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Name = "Authorization",
//         Type = SecuritySchemeType.Http,
//         Scheme = "bearer",
//         BearerFormat = "JWT",
//         In = ParameterLocation.Header,
//         Description = "Enter token like: Bearer {your token}"
//     });

//     options.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Type = ReferenceType.SecurityScheme,
//                     Id = "Bearer"
//                 }
//             },
//             new string[] {}
//         }
//     });
// });

// // JWT Authentication
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,

//         ValidIssuer = builder.Configuration["Jwt:Issuer"],
//         ValidAudience = builder.Configuration["Jwt:Audience"],

//         IssuerSigningKey = new SymmetricSecurityKey(
//             Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
//         )
//     };
// });

// builder.Services.AddAuthorization();

// var app = builder.Build();

// // Swagger UI
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// // Role Seeding
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<ClaimCareDbContext>();

//     if (!db.Roles.Any())
//     {
//         db.Roles.AddRange(
//             new Role { RoleName = "Admin" },
//             new Role { RoleName = "Patient" },
//             new Role { RoleName = "HealthcareProvider" },
//             new Role { RoleName = "InsuranceCompany" }
//         );

//         db.SaveChanges();
//     }
// }

// // Middleware
// app.UseHttpsRedirection();

// // ✅ CORS MUST BE HERE (VERY IMPORTANT)
// app.UseCors("AllowAll");

// app.UseAuthentication();
// app.UseAuthorization();

// app.MapControllers();

// app.Run();


using ClaimCare.Data;
using ClaimCare.Mapping;
using ClaimCare.Services.Interfaces;
using ClaimCare.Services.Implementations;
using ClaimCare.Services.Generic;
using ClaimCare.Services;
using ClaimCare.Models;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// ✅ Services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>(); // FIXED
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHealthcareProviderRepository, HealthcareProviderRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IClaimDocumentRepository, ClaimDocumentRepository>();
builder.Services.AddScoped<IInsuranceCompanyRepository, InsuranceCompanyRepository>();
builder.Services.AddScoped<IInsurancePlanRepository, InsurancePlanRepository>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Token Service
builder.Services.AddScoped<TokenService>();

// Controllers
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// Database
builder.Services.AddDbContext<ClaimCareDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Email
builder.Services.AddScoped<EmailService>();

// Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ClaimCare API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter token like: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ✅ JWT Authentication FIXED
var jwtKey = builder.Configuration["Jwt:Key"] 
             ?? throw new Exception("JWT Key is missing in appsettings.json");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        )
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ✅ Role Seeding
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ClaimCareDbContext>();

    if (!db.Roles.Any())
    {
        db.Roles.AddRange(
            new Role { RoleName = "Admin" },
            new Role { RoleName = "Patient" },
            new Role { RoleName = "HealthcareProvider" },
            new Role { RoleName = "InsuranceCompany" }
        );

        db.SaveChanges();
    }
}

// Middleware
app.UseHttpsRedirection();

app.UseCors("AllowAll"); // IMPORTANT

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();