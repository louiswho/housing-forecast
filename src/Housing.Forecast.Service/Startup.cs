using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Housing.Forecast.Context;
using Housing.Forecast.Context.Repos;
using Housing.Forecast.Context.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace Housing.Forecast.Service
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowInterface",
                    builder => builder.WithOrigins("http://ec2-13-57-218-138.us-west-1.compute.amazonaws.com:9000"));
            });

            services.AddEntityFrameworkNpgsql().AddDbContext<ForecastContext>(options => options.UseNpgsql((Configuration.GetConnectionString("ForecastDB"))));
            services.AddScoped<IForecastContext>(provider => provider.GetService<ForecastContext>());
            services.AddTransient<IRepo<User>, UserRepo>();
            services.AddTransient<IRepo<Room>, RoomRepo>();
            services.AddTransient<ISnapshotRepo, SnapshotRepo>();
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Revature Housing Forecast API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddApplicationInsights(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowInterface");

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Revature Housing Forecast API");
            });

            app.UseMvc();
        }
    }
}
