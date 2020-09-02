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
            string sJobs = System.IO.File.ReadAllText("Jobs/manifest.json"); // читаем JSON и получаем оттуда задания прим. файл manigest.json
            var Jobs = JsonConvert.DeserializeObject<Root>(sJobs); //десериализуем через класс 

            Browser(Jobs);
        }
        static async void Browser(Root root) // в документации вместо кучи авейтеров и результов используется await, но у меня на первом же await приложение вылетает, без исключений
        {
            new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

            var browser = Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var bro = browser;
            var page = bro.GetAwaiter().GetResult().NewPageAsync().GetAwaiter().GetResult(); //новая страница

            NavigationOptions options = new NavigationOptions(); // для использования headers - Referer
            foreach (var work in root.Works)
            {
                options.Referer = work.Referrer;

                var request = page.GoToAsync(work.URL, options).GetAwaiter().GetResult();

                for (int i = 0; i < work.Algo.Count; i++) // в цикле получаем указания куда кликать
                {
                    var waiter = page.WaitForXPathAsync(work.Algo[i].XPath);

                    page.ScreenshotAsync(i + ".jpg"); // делаем скрины
                    page.ClickAsync(work.Algo[i].XPath).GetAwaiter().GetResult();

                    Console.WriteLine(work.Algo[i].XPath);
                }
            }
        }
    }
}
