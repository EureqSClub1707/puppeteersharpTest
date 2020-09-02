using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;
using Newtonsoft.Json;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            string sJobs = System.IO.File.ReadAllText("Jobs/manifest.json");
            var Jobs = JsonConvert.DeserializeObject<Root>(sJobs);

            Browser(Jobs);
        }
        static async void Browser(Root root)
        {
            new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

            var browser = Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false
            });

            var bro = browser;
            var page = bro.GetAwaiter().GetResult().NewPageAsync().GetAwaiter().GetResult();

            NavigationOptions options = new NavigationOptions();
            foreach (var work in root.Works)
            {
                options.Referer = work.Referrer;

                var request = page.GoToAsync(work.URL, options).GetAwaiter().GetResult();

                for (int i = 0; i < work.Algo.Count; i++)
                {
                    var waiter = page.WaitForXPathAsync(work.Algo[i].XPath);

                    page.ScreenshotAsync(i + ".jpg");
                    page.ClickAsync(work.Algo[i].XPath).GetAwaiter().GetResult();

                    Console.WriteLine(work.Algo[i].XPath);
                }
            }
        }
    }
}
