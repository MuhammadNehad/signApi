using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using locationRecordeapi.Data;
using System.Configuration;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using locationRecordeapi.TokenAuthentication;

namespace locationRecordeapi
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
            string conn = Configuration.GetConnectionString("locationRecordeapiContext");
            if (conn.Contains("%CONTENTROOTPATH%"))
            {
                conn = conn.Replace("%CONTENTROOTPATH%", Environment.CurrentDirectory);
            }

            services.AddSingleton<ITokenManager, TokenManager>();

            services.AddDbContext<locationRecordeapiContext>(options => {
                options.UseSqlServer(conn, options => options.EnableRetryOnFailure());


            });
            services.AddAuthentication("BasicAuthentication"
 ).AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthenication", options =>
            {

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, op => {
                op.LoginPath = new PathString("/Emplyees/Login");
                op.LogoutPath = new PathString("/Emplyees/signout");
            }) ;
            services.AddAuthorization(options => {
                options.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthenication").RequireAuthenticatedUser().Build());
            });
            services.AddCors();
            services.AddControllers(options =>
            {
                options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
                options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    //ReferenceHandler = ReferenceHandler.Preserve,
                }));

            });
           


//            services.AddDbContext<locationRecordeapiContext>(options =>
  //                  options.UseSqlServer(Configuration.GetConnectionString("locationRecordeapiContext")));
        }

        public static locationRecordeapiContext DBContextMethod()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            string conn = configuration.GetConnectionString("locationRecordeapiContext");
            if (conn.Contains("%CONTENTROOTPATH%"))
            {
                conn = conn.Replace("%CONTENTROOTPATH%", Environment.CurrentDirectory);
            }
            var OptionsBuilder = new DbContextOptionsBuilder<locationRecordeapiContext>();
            OptionsBuilder.UseSqlServer(conn);
            return new locationRecordeapiContext(OptionsBuilder.Options);

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            //ConnectionStringSettings
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".apk"] = "application/vnd.android.package-archive";
            provider.Mappings[".aab"] = "application/x-authorware-bin";
            app.UseCors(
                    builder => builder
                     .AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                );
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"View")),
                RequestPath = new PathString("/View"),

            })  ;
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                   Path.Combine(Directory.GetCurrentDirectory(), @"apk")),
                RequestPath = new PathString("/apk"),
                ContentTypeProvider = provider
            });
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
