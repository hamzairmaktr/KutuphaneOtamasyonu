using FluentValidation;
using IKitaplik.Api.Services;
using IKitaplik.Business.Abstract;
using IKitaplik.Business.Concrete;
using IKitaplik.Business.Validations.FluentValidations;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.DataAccess.Concrete.EntityFramework;
using IKitaplik.DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<Context>();
builder.Services.AddScoped<IBookRepository, EfBookRepository>();
builder.Services.AddScoped<ICategoryRepository, EfCategoryRepository>();
builder.Services.AddScoped<IStudentRepository, EfStudentRepository>();
builder.Services.AddScoped<IDepositRepository, EfDepositRepository>();
builder.Services.AddScoped<IDonationRepository, EfDonationRepository>();
builder.Services.AddScoped<IMovementRepository, EfMovomentRepository>();
builder.Services.AddScoped<IWriterRepository, EfWriterRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<IImageRepository, EfImageRepository>();

builder.Services.AddScoped<IBookService, BookManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IStudentService, StudentManager>();
builder.Services.AddScoped<IDepositService, DepositManager>();
builder.Services.AddScoped<IDonationService, DonationManager>();
builder.Services.AddScoped<IMovementService, MovementManager>();
builder.Services.AddScoped<IWriterService, WriterManager>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IImageService, ImageManager>();
builder.Services.AddScoped<JwtService>();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddValidatorsFromAssemblyContaining<BookValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CategoryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<StudentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<WriterValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
          RateLimitPartition.GetTokenBucketLimiter(
              partitionKey: httpContext.User.Identity?.Name ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? httpContext.Request.Host.ToString(),
              factory: partition => new TokenBucketRateLimiterOptions
              {
                  AutoReplenishment = true,
                  TokenLimit = 50,
                  TokensPerPeriod = 20,
                  ReplenishmentPeriod = TimeSpan.FromSeconds(60),
              })
          );
    options.RejectionStatusCode = 429;
    options.OnRejected = (context, cancellationToken) =>
    {
        context.HttpContext.Response.Headers["Retry-After"] = "60";
        return ValueTask.CompletedTask;
    };
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        var key = builder.Configuration["Jwt:Key"];
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithPreferredScheme("Bearer")
            .WithHttpBearerAuthentication(bearer =>
            {
                bearer.Token = "";
            });
    });
    app.MapOpenApi();
}
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors("AllowAll");

app.Run();
