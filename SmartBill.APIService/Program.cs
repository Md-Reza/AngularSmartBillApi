using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartBill.APIService.Interface;
using SmartBill.APIService.Repository;
using SmartBill.APIService.Security;
using SmartBillApi.DataTransferObject.ViewModel;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration.GetSection("ApplicationSecurity:Issuer").Value,
        ValidAudience = configuration.GetSection("ApplicationSecurity:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("ApplicationSecurity:Key").Value)),
        ClockSkew = TimeSpan.Zero
    };
});

var corsOrigins = builder.Configuration["CorsOrigins"];

if (string.IsNullOrEmpty(corsOrigins))
{
    throw new ArgumentNullException(nameof(corsOrigins), "CorsOrigins configuration is missing or null.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("corsOrigins", policy =>
    {
        policy.WithOrigins(corsOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors();

builder.Services.AddDataProtection().UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
{
    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
    ValidationAlgorithm = ValidationAlgorithm.HMACSHA512
});
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.SuppressCheckForUnhandledSecurityMetadata = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<STokenService, STokenServiceHandler>();
builder.Services.AddScoped<IUserRepo, UserRepository>();
builder.Services.AddScoped<IMasterServiceRepo, MasterServiceRepository>();
builder.Services.AddScoped<IHistoryRepo, HistoryRepository>();

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.SuppressModelStateInvalidFilter = false;
    opt.InvalidModelStateResponseFactory = actionContext =>
    {
        string error = "Data validation failed. Field Names:";
        foreach (var item in actionContext.ModelState)
        {
            error += $@"[{item.Key}]";
        }
        return new BadRequestObjectResult(new ResponseViewModel()
        {
            StatusCode = StatusCodes.Status404NotFound,
            Message = error
        });
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configure the HTTP request pipeline.
var env = app.Environment;
if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

//SS custom security middleware
//app.UseMiddleware<SSecurityMiddleware>();

//string[] headers = ["ClientID"];
//app.UseCors(x => x.AllowAnyOrigin().WithHeaders(headers).AllowAnyMethod().WithHeaders(headers));
// Use the CORS policy with the correct name
app.UseCors("corsOrigins");

app.UseHsts();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
