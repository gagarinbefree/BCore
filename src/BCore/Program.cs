using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using BCore.Dal.Ef;

namespace BCore
{
    public class Program
    {
        public static string GagarinUserId = "d85e1ae0-3a4e-4a13-9eaa-0dc2c6be70f2";

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
       
            host.Run();
        }        
    }
}
