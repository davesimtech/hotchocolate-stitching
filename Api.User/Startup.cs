using Api.User.Types;
using HotChocolate.Execution.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.User
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services
                .AddSingleton(ConnectionMultiplexer.Connect(_configuration.GetConnectionString("RemoteSchemaRedis")))
                .AddGraphQLServer()
                .AddAuthorization()
                .AddApolloTracing(TracingPreference.Always)
                .AddQueryType<QueryType>()
                .InitializeOnStartup()
                .PublishSchemaDefinition(c => c
                    .SetName("courses")
                    .IgnoreRootTypes()
                    .AddTypeExtensionsFromFile("./Stitching.graphql")
                    .PublishToRedis("ApiUser", sp => sp.GetRequiredService<ConnectionMultiplexer>()));

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

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
