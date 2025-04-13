using Business;
using Business.AuthsService;
using Business.Cloudinary_Service;
using Business.EmailService;
using DataAccess;
using Interfaces.Repository;
using Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// JWT setting 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
});

// Rejester DI
builder.Services.AddTransient<IAccessTokenService, AccessTokenService>(); // auth service
builder.Services.AddTransient<IRefreshTokenService, RefreshTokenService>(); // auth service

builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient<IBrand, BrandData>();

builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICategory, CategoryData>();

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IProduct, ProductData>();
builder.Services.AddTransient<IImageService, CloudinaryService>();

builder.Services.AddTransient<IStorageService, StorageService>();
builder.Services.AddTransient<IStorage, StorageData>();


builder.Services.AddTransient<IUser, UserData>();
builder.Services.AddTransient<IUserService, UserService>();


builder.Services.AddTransient<IRefreshToken, RefreshTokenData>();

builder.Services.AddTransient<IOTP, OTPData>();
builder.Services.AddTransient<IOTPService, OTPService>();

builder.Services.AddTransient<IEmailService, EmailService>(); 


builder.Services.AddTransient<IFilterService, FilterService>(); 
builder.Services.AddTransient<IFilter, FilterData>(); 

builder.Services.AddTransient<IBasketService, BasketService>(); 
builder.Services.AddTransient<IBasket, BasketData>(); 



// CORS
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//  CORS
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

//  Authentication & Authorization
app.UseAuthentication();  
app.UseAuthorization();

app.MapControllers();

app.Run();
