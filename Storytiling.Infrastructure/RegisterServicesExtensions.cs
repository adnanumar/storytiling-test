using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Storytiling.ApplicationCore;
using Storytiling.ApplicationCore.Repository;
using Storytiling.ApplicationCore.Settings;
using Storytiling.Infrastructure.Repository;

namespace Storytiling.Infrastructure
{
    public static class RegisterServicesExtensions
    {
        /// <summary>
        /// This extension method registers all Infrastructure Services into DI container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {
            //Register ApplicationCore services
            services.RegisterApplicationCoreServices();

            //Register Data Repositories
            services.AddScoped<IVideoWorkflowRepository, VideoWorkflowRepository>();

            //Register Settings
            services.Configure<DynamoDBSettings>(options => configuration.GetSection(DynamoDBSettings.Section).Bind(options));

            //Register AmazonDynamoDBClient
            services.AddScoped<IAmazonDynamoDB, AmazonDynamoDBClient>(implementationFactory =>
            {
                var dynamoDBSettings=implementationFactory.GetRequiredService<IOptions<DynamoDBSettings>>();
                AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
                // This client will access the US East 1 region.
                clientConfig.RegionEndpoint = RegionEndpoint.USEast2;
                BasicAWSCredentials credentials = new BasicAWSCredentials(dynamoDBSettings.Value!.AccessKey, dynamoDBSettings.Value!.SecretKey);
                return new AmazonDynamoDBClient(credentials, clientConfig);
            });
            return services;
        }
    }
}
