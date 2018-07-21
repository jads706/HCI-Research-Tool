/*using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Windows.Forms;
using Google.Apis.Requests.Parameters;

namespace HCI_Research_Tool
{
    /// <summary>
    /// YouTube Data API v3 sample: search by keyword.
    /// Relies on the Google APIs Client Library for .NET, v1.7.0 or higher.
    /// See https://developers.google.com/api-client-library/dotnet/get_started
    ///
    /// Set ApiKey to the API key value from the APIs & auth > Registered apps tab of
    ///   https://cloud.google.com/console
    /// Please ensure that you have enabled the YouTube Data API for your project.
    /// </summary>
    internal partial class Search
    {
        public string query;
        string check;
        public string opt1;
        public int max;
        public List<string> vsrf = new List<string>();
        /*public List<string> GetList(string query, string opt1, int max, List<string> vsrf)
        {
            MessageBox.Show(query +", "+opt1+", "+ max);
            Run(query,opt1,max,vsrf).Wait();
            MessageBox.Show(string.Join("\n", vsrf));
            MessageBox.Show(check);
           // Console.WriteLine(string.Join("\n", vsrf));
            return vsrf;
        }
        [STAThread]
        void Adsf(string[] args)
        {
          //  Console.WriteLine("YouTube Data API: Search");
           // Console.WriteLine("========================");

            try
            {
                check = "1";
                //GetList(query,opt1,max,vsrf);
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

//            Console.WriteLine("\nPress any key to continue...");
  //          Console.Read();
        }
        public async Task Run(string query, string opt1, int max)
        {

            List<string> vsr = new List<string>();
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBWXwSnDXopMAZzetd3tkcEIuyVWRI7_7Q",
                //    ApiKey = "AIzaSyDLYwAgMXoK7VLILdLdgSUMImtDG8QY6uA",
                ApplicationName = this.GetType().ToString()
            });
            var nextPageToken = "";
            int count = 0;
            check = "2";
            /*     Console.WriteLine("What would you like to search?");
                 query = Console.ReadLine();
                 Console.WriteLine("How would you like your search filtered? Type 0 for Date, 1 for Rating, 2 for Relevance, 3 for Title, 4 for View Count.");
                 opt1 = Console.ReadLine();
                 Console.WriteLine("How many results would you like?");
                 max = Convert.ToInt32(Console.ReadLine());
            var searchListRequest = youtubeService.Search.List("snippet");

            while (nextPageToken != null && count < max)
            {
                check = "3";
                searchListRequest.Q = query; // Replace with your search term.
                switch (opt1)
                {
                    case "0":
                        searchListRequest.RelevanceLanguage = "en";
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
                        break;
                    case "1":
                        searchListRequest.RelevanceLanguage = "en";
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Rating;
                        break;
                    case "2":
                        searchListRequest.RelevanceLanguage = "en";
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;
                        break;
                    case "3":
                        searchListRequest.RelevanceLanguage = "en";
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Title;
                        break;
                    case "4":
                        searchListRequest.RelevanceLanguage = "en";
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.ViewCount;
                        break;
                        /*case "5":
                          searchListRequest.RelevanceLanguage = "en";
                          searchListRequest.Order = SearchResource.ListRequest.OrderEnum.VideoCount;//only returns channels. Dont use!
                            break;
                }
                searchListRequest.MaxResults = 50;
                searchListRequest.RelevanceLanguage = "en";
                searchListRequest.Type = "video";
                searchListRequest.PageToken = nextPageToken;
                List<string> resultsId = new List<string>();
                List<string> videosInfo = new List<string>();
//                MessageBox.Show("hi");
                var searchListResponse = await searchListRequest.ExecuteAsync();//where the code freezes
                foreach (var searchResult in searchListResponse.Items)
                {
                    switch (searchResult.Id.Kind)
                    {
                        case ("youtube#video"):
                            resultsId.Add(Convert.ToString(searchResult.Id.VideoId));
                            videosInfo.Add(String.Format("{0} (https://www.youtube.com/watch?v={1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                            break;
                    }
                }
                List<string> videos = new List<string>();
                check = "4";
                for (int i = 0; i < searchListRequest.MaxResults; i++)
                {
                    if (count < max)
                    {
                        var VideoStatsRequest = youtubeService.Videos.List("statistics");
                        VideoStatsRequest.Id = resultsId[i];
                        VideoStatsRequest.MaxResults = searchListRequest.MaxResults;
                        var VideoListResponse = await VideoStatsRequest.ExecuteAsync();
                        foreach (var Video in VideoListResponse.Items)
                        {
                            count = count + 1;
                            videos.Add(String.Format("Title: {0} Likes: {1} Dislikes: {2} Views: {3}", videosInfo[i], Video.Statistics.LikeCount, Video.Statistics.DislikeCount, Video.Statistics.ViewCount));
                        }
                    }
                }
                Console.WriteLine(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));
                vsr.Add(String.Format("\n{0}", string.Join("\n", videos)));
                //Console.WriteLine("This search was completed at: " + DateTime.Now.ToString("h:mm:ss tt"));
                //Console.WriteLine(nextPageToken);
                nextPageToken = searchListResponse.NextPageToken;
                //Console.WriteLine(searchListResponse.NextPageToken);
                //Console.WriteLine(count);
                check = "5";
                MessageBox.Show(string.Join("", videos));
            }
            // vsrf.Add(String.Format("All Results: \n{0}", string.Join("", vsr)));
            vsrf.Add(String.Format("All Results: \n{0}", string.Join("", vsr)));
            MessageBox.Show(String.Format("All Results: \n{0}", string.Join("", vsr)));
        }
    }
}*/
