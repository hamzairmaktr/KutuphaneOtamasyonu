using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.Business.Concrete;
using IKitaplik.Business.Validations.FluentValidations;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.DataAccess.Concrete.EntityFramework;
using IKitaplik.DataAccess.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<Context>();
builder.Services.AddScoped<IBookRepository, EfBookRepository>();
builder.Services.AddScoped<ICategoryRepository, EfCategoryRepository>();
builder.Services.AddScoped<IStudentRepository, EfStudentRepository>();
builder.Services.AddScoped<IDepositRepository, EfDepositRepository>();
builder.Services.AddScoped<IDonationRepository, EfDonationRepository>();
builder.Services.AddScoped<IMovementRepository, EfMovomentRepository>();

builder.Services.AddScoped<IBookService, BookManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IStudentService, StudentManager>();
builder.Services.AddScoped<IDepositService, DepositManager>();
builder.Services.AddScoped<IDonationService, DonationManager>();
builder.Services.AddScoped<IMovementService, MovementManager>();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddValidatorsFromAssemblyContaining<BookValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CategoryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<StudentValidator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");  // MVC default route
});
app.Run();
