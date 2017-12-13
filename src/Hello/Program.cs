using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Hello
{
    class Program
    {
        static async Task<(string Url, int Length, double Time)> ProcessURL(string url, HttpClient client, CancellationToken ct)
        {
            var start = DateTime.Now;
            var response = await client.GetAsync(url, ct);
            var urlContents = await response.Content.ReadAsByteArrayAsync();
            var diff = (DateTime.Now - start).TotalMilliseconds;
            return (url, urlContents.Length, diff);
        }

        static async Task Main(string[] args)
        {
            var urls = new List<string>
            {
                "http://msdn.microsoft.com",
                "http://msdn.microsoft.com/library/windows/apps/br211380.aspx",
                "http://msdn.microsoft.com/library/hh290136.aspx",
                "http://msdn.microsoft.com/library/dd470362.aspx",
                "http://msdn.microsoft.com/library/aa578028.aspx",
                "http://msdn.microsoft.com/library/ms404677.aspx",
                "http://msdn.microsoft.com/library/ff730837.aspx"
            };

            var ct = new CancellationTokenSource();
            var client = new HttpClient();
            var downloadTasksQuery = from url in urls select ProcessURL(url, client, ct.Token);

            var results = await Task.WhenAll(downloadTasksQuery);

            results.ToList()
                .ForEach(x => Console.WriteLine("{0} {{ Length: {1}, Time: {2} }}", x.Url, x.Length, x.Time));
        }
    }
}
