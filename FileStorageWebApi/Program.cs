using FileStorageWebApi.Domain.Interfaces;
using FileStorageWebApi.Domain.Models;
using FileStorageWebApi.Repositories;
using FileStorageWebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace FileStorageWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("default")));

            builder.Services.AddScoped<IFilesRepository, FilesRepository>();
            builder.Services.AddScoped<IFilesService, FilesService>();
            builder.Services.AddScoped<TemporaryDownloadLinksService>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            builder.Services.AddTransient<FilesProgressService>();

            builder.Services.AddAuthentication("Bearer").AddJwtBearer();
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();
            app.MapControllers();

            // uncomment to set up migrations on local launch, commented to test docker-compose
            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            //    dbContext.Database.Migrate();
            //}
            app.Run();
        }
    }
}