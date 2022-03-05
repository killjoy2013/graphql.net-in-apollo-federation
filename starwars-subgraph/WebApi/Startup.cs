using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.SystemTextJson;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StarWars;
using GraphiQl;

namespace Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQL()
                .AddSchema<StarWarsSchema>()
                .AddSystemTextJson()
                .AddValidationRule<InputValidationRule>()
                .AddGraphTypes(typeof(StarWarsSchema).Assembly)
                .AddMetrics(_ => true, (provider, _) => provider.GetRequiredService<IOptions<GraphQLSettings>>().Value.EnableMetrics);

            services.Configure<GraphQLSettings>(Configuration.GetSection("GraphQLSettings"));
            services.AddSingleton<StarWarsData>();
            services.AddLogging(builder => builder.AddConsole());
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddGraphiQl(x =>
            {
                x.GraphiQlPath = "/graphiql-ui";
                x.GraphQlApiPath = "/graphql";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

             app.UseCors(builder =>
               builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

    
            app.UseRouting();
            app.UseGraphiQl();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
