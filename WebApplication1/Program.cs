using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1
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

            builder.Services.AddDbContext<TaskContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TaskContext")));

            builder.Services.AddDbContext<CustomerContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CustomerContext")));

            // to disable CORS run Chrome browser with this command:
            // start "C:\Program Files\Google\Chrome\Application\chrome.exe" "--disable-web-security --user-data-dir='SOME-FOLDER'"
            // for SOME-FOLDER provide windows path, e.g. C:\test, to some non existing folder. Chrome will show warning but still will work
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "debug",
                    policy =>
                    {
                        policy.SetIsOriginAllowed(o => true)
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowAnyOrigin();

            });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors("debug");
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}