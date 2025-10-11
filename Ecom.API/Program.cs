using Ecom.API.Middleware;
using Ecom.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
namespace Ecom.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy", builder =>
                {
                    builder.AllowAnyMethod()
                           .AllowAnyHeader()
                           .WithOrigins("https://localhost:4200"); //-------https://localhost:4200 is Angular app url-------//
                });
            });

            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            builder.Services.infrastructureConfiguration(builder.Configuration);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }
            app.UseCors("CORSPolicy");
            app.UseMiddleware<ExceptionsMiddleware>();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
