using HotelListing.API.Core.CoreConfiguration;
using HotelListing.API.Core.CoreContracts;
using HotelListing.API.Core.CoreMiddleware;
using HotelListing.API.Core.CoreRepository;
using HotelListing.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");
builder.Services.AddDbContext<HotelListingDbContext>(options =>
{
   options.UseSqlServer(connectionString);
});

builder.Services.AddIdentityCore<ApiUser>()
   .AddRoles<IdentityRole>()
   .AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("HotelListingAPI")
   .AddEntityFrameworkStores<HotelListingDbContext>()
   .AddDefaultTokenProviders();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
   options.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel Listing API", Version = "v1" });
   options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
   {
      Description = @"JWT Authorization Header using the Bearer scheme.
                     Enter 'Bearer' [space] and then your token is the next input below.
                     Example: 'Bearer 12345abcdef' ",
      Name = "Authorization",
      In = ParameterLocation.Header,
      Type = SecuritySchemeType.ApiKey,
      Scheme = JwtBearerDefaults.AuthenticationScheme
   });

   options.AddSecurityRequirement(new OpenApiSecurityRequirement
   {
      {
        new OpenApiSecurityScheme
        {
           Reference = new OpenApiReference
           {
               Type = ReferenceType.SecurityScheme,
               Id =JwtBearerDefaults.AuthenticationScheme
           },
           Scheme = "0auth2",
           Name = JwtBearerDefaults.AuthenticationScheme,
           In = ParameterLocation.Header
        },
        new List<string>()
      }
   });
});

builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowAll", b =>
   b.AllowAnyHeader()
   .AllowAnyOrigin()
   .AllowAnyMethod());
});
builder.Host.UseSerilog((context,
                         loggerConfiguration) => loggerConfiguration.WriteTo.Console().ReadFrom.Configuration(context.Configuration));


builder.Services.AddApiVersioning(options =>
{
   options.AssumeDefaultVersionWhenUnspecified = true;
   options.DefaultApiVersion = new ApiVersion(1, 0);
   options.ReportApiVersions = true;
   options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver")
   );
});

builder.Services.AddVersionedApiExplorer(
      options =>
      {
         options.GroupNameFormat = "'v'VVV";
         options.SubstituteApiVersionInUrl = true;
      });

builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IHotelsRepository, HotelsRepository>();
builder.Services.AddScoped<IAuthManager, AuthManager>();

builder.Services.AddAuthentication(options =>
{
   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
   options.TokenValidationParameters = new TokenValidationParameters
   {
      ValidateIssuerSigningKey = true,
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ClockSkew = TimeSpan.Zero,
      ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
      ValidAudience = builder.Configuration["JwtSettings:Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
   };
});

builder.Services.AddResponseCaching(options =>
{
   options.MaximumBodySize = 1024;
   options.UseCaseSensitivePaths = true;
});

builder.Services.AddControllers().AddOData(options =>
{
   //options.EnableQueryFeatures();
   options.Select().Filter().OrderBy();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseResponseCaching();

app.Use(async (context, next) =>
{
   context.Response.GetTypedHeaders().CacheControl =
   new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
   {
      Public = true,
      MaxAge = TimeSpan.FromSeconds(10),
   };

   context.Response.Headers[HeaderNames.Vary] = new string[] { "Accept-Encoding" };

   await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
