using Api.Gateway.Config;
using HotChocolate.Execution.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway
{
    public class Startup
    {
        private const string DefaultCorsPolicyName = "Default";
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var apiConfigOptions = _configuration.GetSection("Apis").Get<ApiConfigOptions>();
            services.Configure<ApiConfigOptions>(_configuration.GetSection("Apis"));

            services.AddHttpContextAccessor();
            services.AddHttpClient();

            services.AddHttpClient("users", (sp, c) =>
            {
                c.BaseAddress = new Uri($"{apiConfigOptions.User}/graphql");
            });

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services
                .AddGraphQLServer()
                .AddApolloTracing(TracingPreference.Always)
                .AddQueryType(d => d.Name("Query"))
                .AddMutationType(d => d.Name("Mutation"))
                .AddRemoteSchema("users")
                .InitializeOnStartup();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(DefaultCorsPolicyName);

            app.UseWebSockets();
            app.UseRouting();

            app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
