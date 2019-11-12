using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotTwitter.DataAccess.Repositories;
using NotTwitter.Library.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using NotTwitter.DataAccess;
using NotTwitter.API.Services;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<NotTwitterDbContext>(opt =>
             opt.UseNpgsql(Configuration.GetConnectionString("NotTwitterDB")));

            services.AddScoped<IGenericRepository, GenericRepository>();
			services.AddHttpClient<AuthInfoService>();

			services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod() // not just GET and POST, but allow all methods
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services.AddControllers(options =>
            {
                options.FormatterMappings.SetMediaTypeMappingForFormat("json",
                    new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NotTwitterAPI", Version = "v1" });
            });

			//Add Auth
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.Authority = "https://nottwitter.auth0.com/";
				options.Audience = "https://api.nottwiter.com";
			});

		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NotTwitterAPI");
            });

            app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseCors("AllowAngular");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
