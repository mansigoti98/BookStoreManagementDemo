using BookStoreManagement_1.Data;
using BookStoreManagement_1.Models;
using BookStoreManagement_1.ViewModels.Customer;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace BookStoreManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<CartService>();
            builder.Services.AddHttpContextAccessor();

            Log.Logger = new LoggerConfiguration()
       .WriteTo.Console()
       .WriteTo.MSSqlServer(
           connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
           sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true })
       .CreateLogger();

            builder.Host.UseSerilog();
            builder.Host.UseSerilog();
            builder.Services.AddControllersWithViews();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();
            builder.Services.AddTransient<IValidator<CustomerCreateViewModel>, Validator>();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddControllers();
            builder.Services.AddAuthorization();
                                               
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication("AdminScheme")
                .AddCookie("AdminScheme", options =>
                {
                    options.LoginPath = "/Admin/Login";
                    options.AccessDeniedPath = "/Admin/AccessDenied";
                    options.Cookie.Name = "AdminAuth";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                });

            builder.Services.AddAuthentication("CustomerScheme")
                .AddCookie("CustomerScheme", options =>
                {
                    options.LoginPath = "/Customer/Login";
                    options.AccessDeniedPath = "/Customer/AccessDenied";
                    options.Cookie.Name = "CustomerAuth";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                });

            builder.Services.AddDbContext<BookStoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            app.UseRouting();

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseAuthentication();
            app.Use(async (context, next) =>
            {
                var userName = context.User.Identity?.IsAuthenticated == true
                    ? context.User.Identity.Name
                    : "Anonymous";

                Serilog.Context.LogContext.PushProperty("UserName", userName);

                await next.Invoke();
            });
            app.UseAuthorization();

            app.MapControllerRoute(name: "default", pattern: "{controller=Account}/{action=Login}/{id?}");
           
            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
