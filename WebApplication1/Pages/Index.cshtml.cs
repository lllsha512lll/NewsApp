using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        private NewsRootObject? newsObject;
        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            string apiKey = configuration.GetValue<string>("NEWSAPIKEY");
            string getUrl = "https://newsapi.org/v2/top-headlines?country=us&apiKey=" + apiKey;
            newsObject = JsonConvert.DeserializeObject<NewsRootObject>(webHttpE(getUrl));
        }
        public NewsRootObject? GetNews
        {
            get{
                if (newsObject == null)
                    return null;
                else
                    return newsObject;

            }
        }
        public void OnGet()
        {

        }
        //http webrequest
        private string webHttpE(string uri)
        {
            string resultString = "";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.KeepAlive = false;
                //request.Timeout = Properties.Settings.Default.WebTimeoutDefault;
                using (WebResponse response = (WebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader sreader = new StreamReader(dataStream);
                    resultString = sreader.ReadToEnd();
                    response.Close();
                }
            }
            catch (WebException ex)
            {
                resultString = ex.Message;
            }
            return resultString;
        }
        public class NewsRootObject
        {
            public string? status { get; set; }
            public int totalResults { get; set; }
            public List<Article>? articles { get; set; }
        }
        public class Article
        {
            public Source? source { get; set; }
            public string? author { get; set; }
            public string? title { get; set; }
            public string? description { get; set; }
            public string? url { get; set; }
            public string? urlToImage { get; set; }
            public DateTime publishedAt { get; set; }
            public string? content { get; set; }
        }
        public class Source
        {
            public string? id { get; set; }
            public string? name { get; set; }
        }

    }
}