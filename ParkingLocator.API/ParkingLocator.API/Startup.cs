﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ParkingLocator.Core.Concretes;
using ParkingLocator.Core.Helpers;
using ParkingLocator.Core.Interfaces;

namespace ParkingLocator.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            //    options =>
            //{
            //    options.AddPolicy(MyAllowSpecificOrigins,
            //    builder =>
            //    {
            //        builder.AllowAnyOrigin()
            //               .AllowAnyMethod()
            //               .AllowAnyHeader()
            //               .Build();
            //    });
            //});
            services.AddHttpClient();
            services.AddTransient<IParkingService, ParkingService>();
            services.Configure<ParkingKeyOptions>(options => Configuration.GetSection("ParkingKeyOptions").Bind(options));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "City of Grand Rapids Parking", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IParkingService parkingService)
        {
            app.UseCors(builder => {
                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            }).Build();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "City of Grand Rapids Parking V1");
            });
            //app.UseHealthChecks("/health");
            //app.UseHttpsRedirection();
            app.UseMvc();

            parkingService.UpdateMap();
        }
    }
}
