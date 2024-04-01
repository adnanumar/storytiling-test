using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Storytiling.ApplicationCore.Services;
using System.Reflection;

namespace Storytiling.ApplicationCore
{
    public static class RegisterServicesExtensions
    {
        /// <summary>
        /// This method registers all ApplicationCore Services into DI container
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterApplicationCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IVideoWorkflowService, VideoWorkflowService>();
            //Register Fluent validations
            services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(RegisterServicesExtensions)));
            return services;
        }
    }
}
