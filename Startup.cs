using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VLTP.Services;

namespace VLTP
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
            services.AddControllersWithViews();

            string appSettingsDirectory = Environment.GetEnvironmentVariable("APPSETTINGS_DIRECTORY");

            //Console.WriteLine("----------------------------------------------------------------");
            //Console.WriteLine("APPSETTINGS_DIRECTORY:");
            //Console.WriteLine(Environment.GetEnvironmentVariable("APPSETTINGS_DIRECTORY"));
            //Console.WriteLine("----------------------------------------------------------------");
            //Console.WriteLine("LOGFILE_DIRECTORY:");
            //Console.WriteLine(Environment.GetEnvironmentVariable("LOGFILE_DIRECTORY"));
            //Console.WriteLine("----------------------------------------------------------------");
            //Console.WriteLine("Logging:");
            //Console.WriteLine(Configuration.GetSection("Logging"));
            //Console.WriteLine("----------------------------------------------------------------");
            //Console.WriteLine("Logging:LogLevel:");
            //Console.WriteLine(Configuration.GetSection("Logging:LogLevel"));
            //Console.WriteLine("----------------------------------------------------------------");
            //Console.WriteLine("Logging:LogLevel:Default:");
            //Console.WriteLine(Configuration.GetSection("Logging:LogLevel:Default"));
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("(\"Logging:LogLevel\")[\"Default\"]:");
            Console.WriteLine(Configuration.GetSection("Logging:LogLevel")["Default"]);
            Console.WriteLine("----------------------------------------------------------------");

            //////////////////////////////////////////////////////   
            // Register Oracle Service
            //////////////////////////////////////////////////////               
            services.AddScoped<IOracleService, OracleService>();
            //////////////////////////////////////////////////////  

            //////////////////////////////////////////////////////   
            // Register Login Service
            //////////////////////////////////////////////////////               
            services.AddScoped<ILoginService, LoginService>();
            ////////////////////////////////////////////////////// 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            string logFileDirectory = Environment.GetEnvironmentVariable("LOGFILE_DIRECTORY");
            if ( String.IsNullOrEmpty(logFileDirectory) == false)
            {
                loggerFactory.AddFile(logFileDirectory + "/VLTP-{Date}.log");
            } 
        }
    }
}
