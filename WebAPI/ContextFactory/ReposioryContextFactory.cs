using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repositories.EFCore;

namespace WebAPI.ContextFactory
{
    public class ReposioryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>

    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            // configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();


            // DbContexOptionsBuilder
            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(configuration.GetConnectionString("sqlConnection")
                , prj => prj.MigrationsAssembly("WebAPI"));

            return new RepositoryContext(builder.Options);


        }
    }
}
