using auth_service.Configuration;
using auth_service.Database;
using auth_service.Extensions;
using auth_service.Models;
using auth_service.Services;
using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace auth_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuration
            builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

            builder.Services.AddEndpointsApiExplorer();

            // Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "ShoppingList Identity API",
                        Description =
                            "An ASP.NET Core Web API to Authorize users on ShoppingList platform",
                        //TermsOfService = new Uri("https://example.com/terms"),
                        //Contact = new OpenApiContact
                        //{
                        //    Name = "Example Contact",
                        //    Url = new Uri("https://example.com/contact"),
                        //},
                        //License = new OpenApiLicense
                        //{
                        //    Name = "Example License",
                        //    Url = new Uri("https://example.com/license"),
                        //},
                    }
                );
            });

            // DBContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddAuthorization();
            builder
                .Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            // Identity
            builder
                .Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddApiEndpoints();
            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, KafkaEmailSender>();

            // Kafka
            var kafkaSettings = builder.Configuration.GetSection("Kafka").Get<KafkaSettings>();
            builder.Services.AddKafka(kafka =>
                kafka
                    .UseConsoleLog()
                    .AddCluster(cluster =>
                        cluster
                            .WithBrokers(new[] { kafkaSettings.KafkaConnection })
                            .CreateTopicIfNotExists(kafkaSettings.TopicNames.AccountManagement)
                            .AddProducer(
                                kafkaSettings.ProducerName,
                                producer =>
                                    producer
                                        .DefaultTopic(kafkaSettings.TopicNames.AccountManagement)
                                        .AddMiddlewares(m => m.AddSerializer<JsonCoreSerializer>())
                            )
                    )
            );

            builder.Services.AddHealthChecks();

            var app = builder.Build();

            app.MapHealthChecks("/health");
            app.MapIdentityApi<ApplicationUser>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(); // {protocol}://{address}:{port}/swagger

                MigrationExtensions.ApplyPendingMigrations(app);
            }

            app.UseHttpsRedirection();

            app.Run();
        }
    }
}
