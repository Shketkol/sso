using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SSO.Database;

namespace SSO
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DBSeedsCreation.SetSeeds();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
