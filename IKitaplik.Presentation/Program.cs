using DevExpress.Office.Services;
using DevExpress.XtraBars;
using DevExpress.XtraReports.Design;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.DataAccess.Concrete.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace IKitaplik.Presentation
{
    internal static class Program
    {

        public static ServiceProvider ServiceProvider { get; private set; }



        static void Shutdown()
        {
            ServiceProvider?.Dispose();
        }

        public static IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Context>();
            services.AddScoped<IBookRepository,EfBookRepository>();
            ServiceProvider = services.BuildServiceProvider();
            return services.BuildServiceProvider();
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var form = ServiceProvider.GetRequiredService<AnaSayfa>();
            Application.Run(form);


            Shutdown();
        }
    }
}