using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudentInfoEngine.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentInfoEngine
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

            services.AddDbContext<StudentsDBContext>(options =>
           options.UseSqlite());

            services.AddControllers();

            services.AddCors();

            services.AddScoped<IRepository, Repository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, StudentsDBContext context)
        {
            var genders = context.Genders.Any();
            if (!genders)
            {
                context.Genders.Add(new Gender() { Description = "მდედრობითი" });
                context.Genders.Add(new Gender() { Description = "მამრობითი" });
                context.SaveChanges();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
          .SetIsOriginAllowed(origin => true)
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
