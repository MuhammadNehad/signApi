using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace locationRecordeapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            //IHost host = CreateHostBuilder(args);
            //await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                        //Host.CreateDefaultBuilder(args)
                        //    .ConfigureWebHostDefaults(webBuilder =>
                        //    {
                        //        webBuilder.UseKestrel()
                        //        .UseContentRoot(Directory.GetCurrentDirectory())
                        //        .UseUrls("http://localhost:5000", "http://QwerZXCAQQWE@WEQDDSADQWS:5000", "http://192.168.1.4:5000")
                        //        .UseIISIntegration().UseStartup<Startup>();
                        //    }).Build();
                        Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
