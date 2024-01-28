using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using nyt_webapp.Models;

namespace nyt_webapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly string apiKey = "JXVyMXVBhkGkaKWhHRQCG5JXLveCGTfn";

        public async Task<ActionResult> Index(Home Home) //take in input from view
        {

            var apiUrl = "https://api.nytimes.com/svc/search/v2/articlesearch.json";
            string query = Home.word;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync($"{apiUrl}?q={query}&api-key={apiKey}");
                var result = JsonConvert.DeserializeObject<NYTApiResponse>(response);

                // Process the result and pass it to the view
                var articles = result?.Response?.Docs.Select(doc => new Article
                {
                    Title = doc.Headline?.Main,
                    Description = doc.Snippet,
                }).ToList();

                return View(articles);
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
