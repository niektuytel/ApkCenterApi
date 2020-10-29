using ApkCenterAdminApi.Models;
using ApkCenterAdminApi.Src;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ApkCenterAdminApi
{
    public class Program
    {
        private const string _api = "http://192.168.2.7:51630";
        public const string DataDirectory = "D:/Programming/Own_Programs/Pukkol/Server/ApkCenter/Data/";
        public const string AppsDirectory =  DataDirectory + "Apps/";

        public static Requests MyRequests;
        public static Apps MyApps;
        public static Errors MyErrors;
        public static Categories MyCategories;
        public static Country MyCountry;
        public static Search MySearch;

        //will added in future
        //public static HomeKeys MyHomeKeys;
        //MyHomeKeys = new HomeKeys();

        public static void Main(string[] args)
        {
            MyRequests = new Requests();
            MyApps = new Apps();
            MyErrors = new Errors();
            MyCategories = new Categories();
            MyCountry = new Country();
            MySearch = new Search();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls(_api);
                webBuilder.UseIISIntegration();
                webBuilder.UseStartup<Startup>();
            });

    }
}
