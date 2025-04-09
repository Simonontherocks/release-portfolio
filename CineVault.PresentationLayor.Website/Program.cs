using CineVault.BusinessLogic.Service;
using CineVault.DataAccessLayer;
using CineVault.DataAccessLayer.Context;
using CineVault.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CineVault.PresentationLayor.Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add clascontext

            IConfigurationBuilder configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot config = configBuilder.Build();
            builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IMovieRepository, MovieRepository>(); //<Is hetgeen ik vraag (interface), is hetgeen ik krijg (Klasse)>
            builder.Services.AddScoped<MovieService>();
            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddScoped<MovieService>();
            builder.Services.AddScoped<ApiService>();
            builder.Services.AddScoped<ActorService>();
            builder.Services.AddScoped<IActorRepository, ActorRepository>();
            builder.Services.AddScoped<IDirectorRepository, DirectorRepository>();
            builder.Services.AddScoped<DirectorService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

    }

}
