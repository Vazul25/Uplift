using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Uplift.DataAccess.Data;
using Uplift.DataAccess.Data.Initializer;

namespace Uplift
{
    public class Program
    {
        //public static async Task Main(string[] args)
        public static void Main(string[] args)
        {
            ////https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-part-1/
            //IHost webHost = CreateHostBuilder(args).Build();
            //using (var scope = webHost.Services.CreateScope())
            //{
            //    // Get the DbContext instance
            //    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDBInitializer>();

            //    //Do the migration asynchronously
            //    await dbInitializer.InitializeAsync();
            //}

            //// Run the WebHost, and start accepting requests
            //// There's an async overload, so we may as well use it
            //await webHost.RunAsync();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
