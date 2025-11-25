using System;
using System.IO;
using System.Reflection;
using AgendaTelefonicaApi.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaTelefonicaApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddControllers();
            builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
            builder.Services.AddDbContext<AgendaDbContext>();

            var xmlCommentsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            builder.Services.AddSwaggerGen(x =>
            {
                x.IncludeXmlComments(xmlCommentsPath);
                x.EnableAnnotations();
            });
            
            var app = builder.Build();

            app.MapControllers();
            app.UseSwagger();
            app.UseSwaggerUI();
            
            app.Run();
        }
    }
    
}
