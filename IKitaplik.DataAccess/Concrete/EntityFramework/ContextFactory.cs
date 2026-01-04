using Core.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace IKitaplik.DataAccess.Concrete.EntityFramework
{
    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<Context>();
            var connectionString = configuration.GetConnectionString("conStringGlobal");
            builder.UseSqlServer(connectionString);

            // Create a dummy user context for design time
            var dummyUserContext = new DesignTimeUserContext();

            return new Context(configuration, dummyUserContext);
        }
    }

    public class DesignTimeUserContext : IUserContext
    {
        public string UserId => "0"; // Default to 0 or any dummy value
    }
}
