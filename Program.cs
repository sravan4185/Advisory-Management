using AdvisorManagement.Domain;
using AdvisorManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace AdvisorManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AdvisorContext>(options =>
                options.UseInMemoryDatabase("AdvisorDB"));
            builder.Services.AddScoped<AdvisorService>();

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            // Add Swagger services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdvisorManagement API", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdvisorManagement API v1"));
            }

            app.UseHttpsRedirection();

            // Enable CORS
            app.UseCors();

            //app.UseAuthorization();

            // Define minimal API endpoints
            app.MapGet("/api/advisors", async (AdvisorService advisorService) =>
            {
                return Results.Ok(await advisorService.ListAdvisorsAsync());
            });

            app.MapGet("/api/advisors/{id}", async (int id, AdvisorService advisorService) =>
            {
                var advisor = await advisorService.GetAdvisorAsync(id);
                return advisor != null ? Results.Ok(advisor) : Results.NotFound();
            });

            app.MapPost("/api/advisors", async (Advisor advisor, AdvisorService advisorService) =>
            {
                var createdAdvisor = await advisorService.CreateAdvisorAsync(advisor);
                return Results.Created($"/api/advisors/{createdAdvisor.Id}", createdAdvisor);
            });

            app.MapPut("/api/advisors/{id}", async (int id, Advisor advisor, AdvisorService advisorService) =>
            {
                if (id != advisor.Id)
                {
                    return Results.BadRequest();
                }

                await advisorService.UpdateAdvisorAsync(advisor);
                return Results.NoContent();
            });

            app.MapDelete("/api/advisors/{id}", async (int id, AdvisorService advisorService) =>
            {
                await advisorService.DeleteAdvisorAsync(id);
                return Results.NoContent();
            });

            app.Run();
        }
    }
}
