using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Requests.Parameters;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;


namespace HCI_Research_Tool
{
    public partial class Form1 : Form
    {
        string errorID;
        public string ind = "     ";
        public int Rcount = 1;
        public string query;
        public string opt1;
        public float max;
        public string elapsedTime;
        public float progress;
        public float prog;
        public string error;
        Stopwatch stopWatch = new Stopwatch();
        TimeSpan ts;
        public List<string> vs = new List<string>();
        public List<string> vsrf = new List<string>();
        int vidcount = 0;
        int videoNum = 0;
        public string VidId;
        string path = "";
        public Form1()
        {
            InitializeComponent();
            //Stopwatch needs to be properly started but can not be started on button click.
            //stopWatch.Start();
            //stopWatch.Stop();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.DarkGray;
        }
        public void button1_Click(object sender, EventArgs e)
         {
             check();
             if (opt1 == "Date" || opt1 == "Rating" || opt1 == "Relevance" || opt1 == "Title" || opt1 == "View Count")
             {
                stopWatch.Reset();                
                button1.Visible = false;
                progressBar1.Value = 0;
                progressBar1.Visible = true;
                prog = 0;
                progress = 0;
                vidcount = 0;
                path = "";
                ProgPerc.Text = "0%";
                Rcount = 1;
                vs = new List<string>();
                vsrf = new List<string>();
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Text File|*.txt";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    path = sfd.FileName;
                }
                else if (File.Exists(path))
                {
                    MessageBox.Show("A file with that search has already been created. Please change search term or delete the old search file.");
                    return;
                }
                stopWatch.Start();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;
                elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
                        ts.Hours, ts.Minutes, ts.Seconds);
                Time.Text = "Elapsed Time: " + elapsedTime;
                backgroundWorker1.RunWorkerAsync();            
            }
             else
             {
                 SCerror();
                button1.Visible = true;
            }
         }
        public void SCerror()
        {
            MessageBox.Show("Please select a Search Criteria");
        }
        public void check()
        {
            opt1 = comboBox1.Text;
        }
        public void MakeText()
        {
            query = textBox1.Text;
            string title = textBox1.Text;
            bool space = title.Contains(' ');
            if (space == true)
            {
                title = title.Replace(' ', '_');
            }
            max = Convert.ToInt32(numericUpDown1.Value);
            string d = DateTime.Today.ToShortDateString();
            bool space2 = d.Contains(' ');
            if (space2 == true)
            {
                d = d.Replace(' ', '_');
            }
            bool slash = d.Contains('/');
            if (slash == true)
            {
                d = d.Replace('/', '-');
            }

            // to create the text file
            using (StreamWriter sw = File.CreateText(path))
                {
                    //sw.WriteLine("\nThis search was started at: " + DateTime.Now.ToString("h:mm:ss tt") + " and ordered by " + opt1);
                    vsrf = GetList(query, opt1, max);
                    string k = String.Format("All Results: \n\n{0}", string.Join("\r\n", getVS()));
                    foreach (String v in vsrf)
                    {
                        sw.WriteLine(k);
                        sw.WriteLine("\nThis search was completed at: " + DateTime.Now.ToString("h:mm:ss tt") + " and ordered by " + opt1);
                    }
                }
     //       MessageBox.Show(error);
     //       MessageBox.Show(errorID);
                error = "Text file created";
                        
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
           // Close();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
        public void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            max = Convert.ToInt32(numericUpDown1.Value);
        }
        public void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (opt1 == "Date" || opt1 == "Rating" || opt1 == "Relevance" || opt1 == "Title" || opt1 == "View Count")
            {
                opt1 = comboBox1.Text;
            }
        }
        public List<string> GetList(string query, string opt1, float max)
        {
            if (Run(query, opt1, max).IsCompleted)
            {
                return vsrf;
            }
            else
            {
                MessageBox.Show("Failure");
                return vsrf;
            }
        }
        public string printParamSTR(String S)
        {
            if (String.IsNullOrEmpty(S))
            { 
                S = "N/A";
                return S;
            }
            else
            {

                return S;
            }
        }
        public string printParamInt(Object i)
        {
            if (i != null)
            {

                return i.ToString();
            }
            else
            {

                return "N/A";
            }
        }
        public string printParamBool(Object B)
        {
            if (B != null)
            {

                return B.ToString();
            }
            else
            {

                return "N/A";
            }
        }
        public string printParamDate(Object D)
        {

            if (D != null)
            {
                return D.ToString();
            }
            else
            {

                return "N/A";
            }
        }
        public string printStringList(Object O, String label)
        {

            if (O != null)
            {
                String ans = "";
                //Console.WriteLine("Hey");
                if (IsStringList(O))
                {
                    List<String> myList = new List<String>();
                    myList = O as List<String>;

                    //Console.WriteLine(label + " (" + myList.Count + ") [");
                    ans = ans + "          " + "\"" + label + "\"" + ": " + "[";

                    for (int i = 0; i < myList.Count; i++)
                    {
                        if (i < myList.Count - 1)
                        {
                            //Console.WriteLine("\t\t" + myList[i] + " ,");
                            ans = ans + "" + "\"" + myList[i] + "\"," + " ";
                        }
                        else
                        {
                            //Console.WriteLine("\t\t" + myList[i]);
                            ans = ans + "\"" + myList[i] + "\"";
                        }
                    }
                    //Console.WriteLine("\t]");
                    ans = ans + "]," + "\n";
                }
                return ans;
            }
            else
            {
                //Console.WriteLine(label + " (0) [ ]");
                return "          " + "\"" + label + "\"" + ": [ ]," + "\n";
            }
        }
        public bool IsStringList(object o)
        {
            if (o == null) return false;
            return o is IList<String> &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
        public async Task Run(string query, string opt1, float max)
        {
            List<string> vsr = new List<string>();
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBWXwSnDXopMAZzetd3tkcEIuyVWRI7_7Q",
                ApplicationName = this.GetType().ToString()
            });
            var nextPageToken = "";
            int count = 0;
            var searchListRequest = youtubeService.Search.List("snippet");
            while (nextPageToken != null && count < max)
            {
                searchListRequest.Q = query; // Replace with your search term.
                switch (opt1)
                {
                    case "Date":
                        searchListRequest.RelevanceLanguage = "en";
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
                        break;
                    case "Rating":
                        searchListRequest.RelevanceLanguage = "en";
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Rating;
                        break;
                    case "Relevance":
                        searchListRequest.RelevanceLanguage = "en";
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;
                        break;
                    case "Title":
                        searchListRequest.RelevanceLanguage = "en";
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Title;
                        break;
                    case "View Count":
                        searchListRequest.RelevanceLanguage = "en";
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.ViewCount;
                        break;
                        /*case "5":
                          searchListRequest.RelevanceLanguage = "en";
                          searchListRequest.Order = SearchResource.ListRequest.OrderEnum.VideoCount;//only returns channels. Dont use!
                            break;*/
                }
                searchListRequest.MaxResults = 50;
                searchListRequest.RelevanceLanguage = "en";
                searchListRequest.Type = "video";
                searchListRequest.PageToken = nextPageToken;
                var searchListResponse = searchListRequest.Execute();
                List<string> resultsId = new List<string>();
                // List<string> videosInfo = new List<string>();
                foreach (var searchResult in searchListResponse.Items)
                {
                    switch (searchResult.Id.Kind)
                    {
                        case ("youtube#video"):
                            resultsId.Add(Convert.ToString(searchResult.Id.VideoId));
                            //videosInfo.Add(String.Format("{0} (https://www.youtube.com/watch?v={1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                            break;
                    }
                }
                List<string> videos = new List<string>();
                for (int i = 0; i < searchListRequest.MaxResults; i++)
                {
                    if (count < max)
                    {
                        var VideoListRequest = youtubeService.Videos.List("snippet, contentDetails, status, statistics, player, topicDetails, recordingDetails, liveStreamingDetails, localizations");
                        VideoListRequest.Id = resultsId[i];
                        VideoListRequest.MaxResults = searchListRequest.MaxResults;
                        var VideoListResponse = VideoListRequest.Execute();
                        String list = "";
                        foreach (var Video in VideoListResponse.Items)
                        {
                            
                            //Console.WriteLine(count);
                            list += "\nVideo" + Rcount + ":\n";
                            list += ind + "{\n";
                            Rcount = Rcount + 1;
                            list += string.Format(ind + "\"{0}\": \"{1}\",\n", "url", "https://www.youtube.com/watch?v=" + resultsId[i]);
                            list += string.Format(ind + "\"{0}\": \"{1}\",\n", "Kind", printParamSTR(Video.Kind));
                            list += string.Format(ind + "\"{0}\": \"{1}\",\n", "etag", printParamSTR(Video.ETag));
                            list += string.Format(ind + "\"{0}\": \"{1}\",\n", "ID", printParamSTR(Video.Id));
                            errorID = printParamSTR(Video.Id);
                            list += ind + "\"Snippet\":\n";
                            list += ind + ind + "{\n";
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Published At", printParamDate(Video.Snippet.PublishedAt));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "ChannelId", printParamSTR(Video.Snippet.ChannelId));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Title", printParamSTR(Video.Snippet.Title));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Description", printParamSTR(Video.Snippet.Description) + "\n");
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Channel Title", printParamSTR(Video.Snippet.ChannelTitle));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Thumbnail url", printParamSTR(Video.Snippet.Thumbnails.Default__.Url));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Thumbnail width", printParamInt(Video.Snippet.Thumbnails.Default__.Width));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Thumbnail height", printParamInt(Video.Snippet.Thumbnails.Default__.Height));
                            list += string.Format("{0}\n", printStringList(Video.Snippet.Tags, "Tags"));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Category Id", printParamSTR(Video.Snippet.CategoryId));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Live Broadcast Content", printParamSTR(Video.Snippet.LiveBroadcastContent));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Default Language", printParamSTR(Video.Snippet.DefaultLanguage));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Localized Title", printParamSTR(Video.Snippet.Localized.Title));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Localized Description", printParamSTR(Video.Snippet.Localized.Description) + "\n");
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Default Audio Language", printParamSTR(Video.Snippet.DefaultAudioLanguage));
                            list += ind + ind + "}\n";

                            list += ind + "\"Content Details\":\n";
                            list += ind + ind + "{\n";

                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Duration", printParamSTR(Video.ContentDetails.Duration));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Dimension", printParamSTR(Video.ContentDetails.Dimension));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Defintion", printParamSTR(Video.ContentDetails.Definition));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Caption", printParamSTR(Video.ContentDetails.Caption));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Licensed Content", printParamBool(Video.ContentDetails.LicensedContent));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Projection", printParamSTR(Video.ContentDetails.Projection));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Custom Thumbnail", printParamBool(Video.ContentDetails.HasCustomThumbnail));
                            list += ind + ind + "}\n";

                            list += ind + "\"Status\":\n";
                            list += ind + ind + "{\n";
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Upload Status", printParamSTR(Video.Status.UploadStatus));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Failure Reason", printParamSTR(Video.Status.FailureReason));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Rejection Reason", printParamSTR(Video.Status.RejectionReason));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Privacy Status", printParamSTR(Video.Status.PrivacyStatus));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Sceduled to be Published At", printParamDate(Video.Status.PublishAt));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "License", printParamSTR(Video.Status.License));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "embeddable", printParamBool(Video.Status.Embeddable));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Public Stats Viewable", printParamBool(Video.Status.PublicStatsViewable));
                            list += ind + ind + "}\n";

                            list += ind + "\"Statistics\":\n";
                            list += ind + ind + "{\n";
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "View Count", printParamInt(Video.Statistics.ViewCount));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Like Count", printParamInt(Video.Statistics.LikeCount));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Dislike Count", printParamInt(Video.Statistics.DislikeCount));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Favorite Count", printParamInt(Video.Statistics.FavoriteCount));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Comment Count", printParamInt(Video.Statistics.CommentCount));
                            list += ind + ind + "}\n";

                            list += ind + "\"Player\":\n";
                            list += ind + ind + "{\n";
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Embed HTML", printParamSTR(Video.Player.EmbedHtml));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Embed Height", printParamInt(Video.Player.EmbedHeight));
                            list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Embed Width", printParamInt(Video.Player.EmbedWidth));
                            list += ind + ind + "}\n";

                            list += ind + "\"Topic Details\":\n";
                            list += ind + ind + "{\n";
                            if (Video.TopicDetails != null)
                            {
                                list += string.Format("{0}\n", printStringList(Video.TopicDetails.TopicIds, "Topic Ids"));
                                list += string.Format("{0}\n", printStringList(Video.TopicDetails.RelevantTopicIds, "Relevant Topic Ids"));
                                list += string.Format("{0}\n", printStringList(Video.TopicDetails.TopicCategories, "Topic Categories"));
                            }
                            else
                            {
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Topic Ids", "N/A");
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Relevant Topic Ids", "N/A");
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Topic Categories", "N/A");
                            }
                            list += ind + ind + "}\n";

                            list += ind + "\"Live Streaming Details\":\n";
                            list += ind + ind + "{\n";
                            if (Video.LiveStreamingDetails != null)
                            {
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Actual Start Time", printParamDate(Video.LiveStreamingDetails.ActualStartTime));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Actual End Time", printParamDate(Video.LiveStreamingDetails.ActualEndTime));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Scheduled Start Time", printParamDate(Video.LiveStreamingDetails.ScheduledStartTime));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Scheduled End Time", printParamDate(Video.LiveStreamingDetails.ScheduledEndTime));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Concurrent Viewers", printParamInt(Video.LiveStreamingDetails.ConcurrentViewers));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Active Live Chat ID", printParamSTR(Video.LiveStreamingDetails.ActiveLiveChatId));
                            }
                            else
                            {
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Actual Start Time", "N/A");
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Actual End Time", "N/A");
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Scheduled Start Time", "N/A");
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Scheduled End Time", "N/A");
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Concurrent Viewers", "N/A");
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Active Live Chat ID", "N/A");
                            }
                            list += ind + ind + "}\n";
                            list += ind + "}\n";

                            //Console.Write(testCount + ": ");
                            //for video Localization
                            list += ind + "\"Localizations\":\n";
                            list += ind + ind + "{\n";
                            if (Video.Localizations != null)
                            {
                                String ans = "";
                                foreach (KeyValuePair<string, Google.Apis.YouTube.v3.Data.VideoLocalization> kvp in Video.Localizations)
                                {
                                    //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                                    //ans = ans + String.Format("Key = {0}, Title = {1}, Description = {2}", 
                                    //kvp.Key, kvp.Value.Title, kvp.Value.Description);
                                    ans += string.Format(ind + ind + "\"{0}\": ", kvp.Key);
                                    ans += string.Format("{{\"{0}\": \"{1}\",", "Title", kvp.Value.Title);
                                    ans += string.Format("\"{0}\": \"{1}\"}},\n", "Description", kvp.Value.Description);
                                }
                                list += ans;
                            }
                            else
                            {
                                String ans = "";
                                ans += string.Format(ind + ind + "\"{0}\": ", "N/A");
                                ans += string.Format("{{\"{0}\": \"{1}\",", "Title", "N/A");
                                ans += string.Format("\"{0}\": \"{1}\"}},\n", "Description", "N/A");
                                list += ans;
                            }
                            list += ind + ind + "}\n";
                            //MessageBox.Show("video part");
                            //Channel/Publisher info
                            list += "\nChannel/Publisher Info:\n";
                            list += ind + "{\n";
                            //Creating Channel list request from api
                            //Also setting parameters like Id, and max results
                            var ChannelListRequest = youtubeService.Channels.List("snippet, contentDetails, statistics, status, brandingSettings, contentOwnerDetails, localizations");
                            ChannelListRequest.Id = Video.Snippet.ChannelId;
                            ChannelListRequest.MaxResults = 1;

                            var ChannelListResponse =ChannelListRequest.Execute();
                            //Loop for info of channel of video
                            foreach (var channel in ChannelListResponse.Items)
                            {
                                //list contains string of info, and the methods printParam to check object and return a string
                                list += string.Format(ind + "\"{0}\": \"{1}\",\n", "Kind", printParamSTR(channel.Kind));
                                list += string.Format(ind + "\"{0}\": \"{1}\",\n", "Channel Etag", printParamSTR(channel.ETag));
                                list += string.Format(ind + "\"{0}\": \"{1}\",\n", "Channel ID", printParamSTR(channel.Id));
                                list += ind + "\"Snippet\":\n";
                                list += ind + ind + "{\n";
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Title", printParamSTR(channel.Snippet.Title));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Description", printParamSTR(channel.Snippet.Description));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Custom URL", printParamSTR(channel.Snippet.CustomUrl));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Published At", printParamDate(channel.Snippet.PublishedAt));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Thumbnail URL", printParamSTR(channel.Snippet.Thumbnails.Default__.Url));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Thumbnail Width", printParamInt(channel.Snippet.Thumbnails.Default__.Width));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Thumbnail Height", printParamInt(channel.Snippet.Thumbnails.Default__.Height));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Defualt Langauge", printParamSTR(channel.Snippet.DefaultLanguage));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Localized Title", printParamSTR(channel.Snippet.Localized.Title));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Description", printParamSTR(channel.Snippet.Localized.Description));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Country", printParamSTR(channel.Snippet.Country));
                                list += ind + ind + "}\n";
                                list += ind + "\"Content Details\":\n";
                                list += ind + ind + "{\n";
                                list += ind + ind + "\"Related Playlists\":\n";
                                list += ind + ind + ind + "{\n";
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Likes", printParamSTR(channel.ContentDetails.RelatedPlaylists.Likes));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Favorites", printParamSTR(channel.ContentDetails.RelatedPlaylists.Favorites));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Uploads", printParamSTR(channel.ContentDetails.RelatedPlaylists.Uploads));
                                list += ind + ind + ind + "}\n";
                                list += ind + ind + "}\n";
                                list += ind + "\"Statistics\":\n";
                                list += ind + ind + "{\n";
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "View Count", printParamInt(channel.Statistics.ViewCount));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Comment Count", printParamInt(channel.Statistics.CommentCount));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Suscriber Count", printParamInt(channel.Statistics.SubscriberCount));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Hidden Suscriber Count", printParamInt(channel.Statistics.HiddenSubscriberCount));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Video Count", printParamInt(channel.Statistics.VideoCount));
                                list += ind + ind + "}\n";
                                //If conditionals to check if object in Toic Details is empty or not
                                list += ind + "\"Topic Details\":\n";
                                list += ind + ind + "{\n";
                                if (channel.TopicDetails != null)//need to format
                                {
                                    list += string.Format("{0}\n", printStringList(channel.TopicDetails.TopicCategories, "Topic Categories"));
                                    list += string.Format("{0}\n", printStringList(channel.TopicDetails.TopicIds, "Topic IDs"));
                                }
                                else
                                {
                                    list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Topic Categories", "[]");
                                    list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Topic Ids", "[]");

                                }
                                list += ind + ind + "}\n";
                                list += ind + "\"Status\":\n";
                                list += ind + ind + "{\n";
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Privacy Status", printParamSTR(channel.Status.PrivacyStatus));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "IsLinked", printParamBool(channel.Status.IsLinked));
                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Linked Status", printParamSTR(channel.Status.LongUploadsStatus));
                                list += ind + ind + "}\n";
                                list += ind + "\"Brand Settings\":\n";
                                list += ind + ind + "{\n";
                                list += ind + ind + "\"Channel\":\n";
                                list += ind + ind + ind + "{\n";
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Title", printParamSTR(channel.BrandingSettings.Channel.Title));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Description", printParamSTR(channel.BrandingSettings.Channel.Description));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Keywords", printParamSTR(channel.BrandingSettings.Channel.Keywords));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Tracking Analytics Account Id", printParamSTR(channel.BrandingSettings.Channel.TrackingAnalyticsAccountId));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Moderate Comments", printParamBool(channel.BrandingSettings.Channel.ModerateComments));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Show Related Channels", printParamBool(channel.BrandingSettings.Channel.ShowRelatedChannels));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Show Browsed View", printParamBool(channel.BrandingSettings.Channel.ShowBrowseView));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Featured Channels Title", printParamSTR(channel.BrandingSettings.Channel.FeaturedChannelsTitle));

                                //printStringList will print out the list of strings in list
                                //list += printStringList(channel.BrandingSettings.Channel.FeaturedChannelsUrls,"Featured Channel URLs");

                                if (channel.BrandingSettings.Channel.FeaturedChannelsUrls != null)
                                {
                                    list += string.Format(ind + ind + ind + "\"Featured Channel URLs\": [");
                                    foreach (string url in channel.BrandingSettings.Channel.FeaturedChannelsUrls)
                                    {
                                        list += string.Format("\"{0}\", ", printParamSTR(url));
                                    }
                                    list += "],";

                                }
                                else
                                {
                                    list += ind + ind + ind + "\"Featured Channel URLs\": []";
                                }
                                list += "\n";

                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Unsuscribed Trailer", printParamSTR(channel.BrandingSettings.Channel.UnsubscribedTrailer));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Profile Color", printParamSTR(channel.BrandingSettings.Channel.ProfileColor));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Default Langauge", printParamSTR(channel.BrandingSettings.Channel.DefaultLanguage));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Country", printParamSTR(channel.BrandingSettings.Channel.Country));
                                list += ind + ind + ind + "}\n";
                                list += ind + ind + "\"Image\":\n";
                                list += ind + ind + ind + "{\n";
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner Image Url", printParamSTR(channel.BrandingSettings.Image.BannerImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner Mobile Image Url", printParamSTR(channel.BrandingSettings.Image.BannerMobileImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner Watch Icon Image Url", printParamSTR(channel.BrandingSettings.Image.WatchIconImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner Tracking Image Url", printParamSTR(channel.BrandingSettings.Image.TrackingImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner Tablet Low Image Url", printParamSTR(channel.BrandingSettings.Image.BannerTabletLowImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner Tablet Image Url", printParamSTR(channel.BrandingSettings.Image.BannerTabletImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner Tablet HD Image Url", printParamSTR(channel.BrandingSettings.Image.BannerTabletHdImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner Tablet Extra HD Image Url", printParamSTR(channel.BrandingSettings.Image.BannerTabletExtraHdImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner Mobile Low Image Url", printParamSTR(channel.BrandingSettings.Image.BannerMobileLowImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner Mobile Medium HD Image Url", printParamSTR(channel.BrandingSettings.Image.BannerMobileMediumHdImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner Mobile HD Image Url", printParamSTR(channel.BrandingSettings.Image.BannerMobileHdImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner Mobile Extra HD Image Url", printParamSTR(channel.BrandingSettings.Image.BannerMobileExtraHdImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner TV Image Url", printParamSTR(channel.BrandingSettings.Image.BannerTvImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner TV Low Image Url", printParamSTR(channel.BrandingSettings.Image.BannerTvLowImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner TV Medium Image Url", printParamSTR(channel.BrandingSettings.Image.BannerTvMediumImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner TV High Image Url", printParamSTR(channel.BrandingSettings.Image.BannerTvHighImageUrl));
                                list += string.Format(ind + ind + ind + "\"{0}\": \"{1}\",\n", "Banner External Url", printParamSTR(channel.BrandingSettings.Image.BannerExternalUrl));
                                list += ind + ind + ind + "}\n";
                                list += ind + ind + "\"Hints\":\n";
                                list += ind + ind + ind + "{\n" + ind + ind + ind + "[";
                                //Loop to get Hint Property and Value in object Hints
                                foreach (var hint in channel.BrandingSettings.Hints)//needs formatting
                                {
                                    list += string.Format("{{\"{0}\": \"{1}\", ", "Property", printParamSTR(hint.Property));
                                    list += string.Format("\"{0}\": \"{1}\"}}, ", "Value", printParamSTR(hint.Value));
                                }
                                list += "]\n";
                                list += ind + ind + ind + "}\n";
                                list += "\n";
                                list += ind + ind + "}\n";
                                list += ind + "\"Content Owner Details\":\n";
                                list += ind + ind + "{\n";
                                //Conditionals to check if Content Owner Details is empty or not
                                if (channel.ContentOwnerDetails != null)
                                {
                                    list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Content Owner", printParamSTR(channel.ContentOwnerDetails.ContentOwner));
                                    list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Time Linked", printParamDate(channel.ContentOwnerDetails.TimeLinked));
                                }

                                else
                                {
                                    list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Content Owner", "N/A");
                                    list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Time Linked", "N/A");
                                }
                                list += ind + ind + "}\n";
                                list += ind + "\"Localization\":\n";
                                list += ind + ind + "{\n";
                                //Checks if Localization is empty or not and then prints in loop the the langauge key, localized title of langauge key
                                //and localized description of langauge key
                                if (channel.Localizations != null)
                                {
                                    String loc = "";
                                    foreach (KeyValuePair<string, Google.Apis.YouTube.v3.Data.ChannelLocalization> kvp2 in channel.Localizations)
                                    {
                                        loc += string.Format(ind + ind + "\"{0}\": ", kvp2.Key);
                                        loc += string.Format("{{\"{0}\": \"{1}\",", "Title", kvp2.Value.Title);
                                        loc += string.Format("\"{0}\": \"{1}\"}},\n", "Description", kvp2.Value.Description);
                                    }
                                    list += loc;
                                }
                                else
                                {
                                    String loc = "";
                                    loc += string.Format(ind + ind + "\"{0}\": ", "N/A");
                                    loc += string.Format("{{\"{0}\": \"{1}\",", "Title", "N/A");
                                    loc += string.Format("\"{0}\": \"{1}\"}},\n", "Description", "N/A");
                                    list += loc;
                                }
                                list += ind + ind + "}\n";

                            }
                            list += ind + "}\n";
                            //MessageBox.Show("channel part");
                            //comments list
                            //These are variables in order to get more tha 100 results with nextpagetoken
                            list += "\nComment Thread:\n";
                            list += ind + "{\n";
                            var CommentThreadCountnextPageToken = "";
                            int maxComThread = 99;
                            //Console.WriteLine("Number comments in Video:" + Video.Statistics.CommentCount);
                            int tempCount = 0;
                            //Console.WriteLine("Hello 1");
                            //conditional for disabled or enabled comments. If commentcount is 0 or more than 0
                            //then it indicates that comments are enabled, and if commentcount is disabled
                            //then commentcount would equal literally nothing and else condition would be intiated

                            if (Video.Statistics.CommentCount >= 0 && Video.LiveStreamingDetails == null)
                            {
                                //Console.WriteLine("Hello 2");
                                error = "comments start";

                                while (CommentThreadCountnextPageToken != null && tempCount < maxComThread)
                                {
                                    //This is an api request for commentThread with parameters initiated
                                    var CommentThreadsListRequest = youtubeService.CommentThreads.List("snippet, replies");
                                    CommentThreadsListRequest.VideoId = resultsId[i];
                                    CommentThreadsListRequest.Order = CommentThreadsResource.ListRequest.OrderEnum.Relevance;
                                    CommentThreadsListRequest.MaxResults = 100;
                                    CommentThreadsListRequest.PageToken = CommentThreadCountnextPageToken;
                                    var CommentThreadsListResponse = CommentThreadsListRequest.Execute();
                                    //Foreach loop that is an indvidual comment thread
                                    //Console.WriteLine("Hello 3");
                                    foreach (var CommentThread in CommentThreadsListResponse.Items)
                                    {

                                        //Api Request for comment with parameters initiated
                                        var CommentsListRequest = youtubeService.Comments.List("snippet");
                                        CommentsListRequest.Id = printParamSTR(CommentThread.Snippet.TopLevelComment.Id);
                                        CommentsListRequest.MaxResults = 1;
                                        CommentsListRequest.TextFormat = CommentsResource.ListRequest.TextFormatEnum.PlainText;
                                        var CommentListResponse = CommentsListRequest.Execute();
                                        if (tempCount < maxComThread)
                                        {
                                            //Console.WriteLine("Hello 4");
                                            tempCount = tempCount + 1;
                                            //Top Level Comment. This loop prints out the top level comment
                                            list += ind + "\"Top Level Comment\":\n";
                                            list += ind + ind + "{\n";
                                            foreach (var Comment in CommentListResponse.Items)
                                            {

                                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Author", printParamSTR(Comment.Snippet.AuthorDisplayName));
                                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Likes", printParamInt(Comment.Snippet.LikeCount));
                                                list += string.Format(ind + ind + "\"{0}\": \"{1}\",\n", "Content", printParamSTR(Comment.Snippet.TextDisplay));

                                            }
                                            //This seprate loop prints out every reply within comment thread that is replying
                                            //to top level comment thread. When you set Parameter parentId instead of Id, then
                                            //replies are printed out
                                            error = "replies";
                                            var repliesListRequest = youtubeService.Comments.List("snippet");
                                            repliesListRequest.ParentId = CommentThread.Snippet.TopLevelComment.Id;
                                            repliesListRequest.MaxResults = 100;
                                            repliesListRequest.TextFormat = CommentsResource.ListRequest.TextFormatEnum.PlainText;
                                            var repliesListResponse = repliesListRequest.Execute();
                                            list += ind + ind + "\"Replies\":\n";
                                            list += ind + ind + ind + "{\n";
                                            foreach (var reply in repliesListResponse.Items)
                                            {
                                                list += string.Format(ind + ind + ind + "{{\"{0}\": \"{1}\", ", "Author", printParamSTR(reply.Snippet.AuthorDisplayName));
                                                list += string.Format("\"{0}\": \"{1}\", ", "Content", printParamSTR(reply.Snippet.TextDisplay));
                                                list += string.Format("\"{0}\": \"{1}\",}}\n", "Likes", printParamInt(reply.Snippet.LikeCount));
                                            }
                                            error = "reply added";
                                            list += ind + ind + ind + "}\n";
                                            list += ind + ind + "}\n";

                                        }
                                    }
                                    error="comment added";

                                    list += ind + "}\n";
                                    CommentThreadCountnextPageToken = CommentThreadsListResponse.NextPageToken;
                                    //Console.WriteLine("Number of Comments Printed:" +tempCount);
                                }
                            }
                            //else condition for comments being disabled
                            else
                            {
                                //Console.WriteLine("It worked, now check video link to see if comments were actually disabled");
                                error="comment disabled";
                                list += ind + "Comments are not avaiable either because comments have been disabled or the video is live at this moment\n" + ind + "}";
                            }
                            
                            list += ";";
                            //MessageBox.Show("comments part");
                            //MessageBox.Show(String.Format("the list", string.Join("\n", list)));
                            videos.Add(list);
                            videoNum = videos.Count;
                            //MessageBox.Show("after adding to list");
                            if (count < max)
                            {
                                vidcount = count+1;
                                prog = vidcount / max;
                                prog = prog * 100;
                                progress = prog;
                                //   MessageBox.Show(progress.ToString());
                                backgroundWorker1.ReportProgress((int)progress);
                            }
                            count = count + 1;
                            
                        }
                    }
                }
                vsr.Add(String.Format("{0}", string.Join("\r\n", videos)));
                nextPageToken = searchListResponse.NextPageToken;
            }
            setVS(vsr);
            vsrf.Add(String.Format("All Results: \n\n{0}", string.Join("", vsr)));
        }
        public List<string> setVS(List<string> vsr)
        {
            vs = vsr;
            return vs;
        }
        public List<string> getVS()
        {
            return vs;
        }
        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker1.WorkerReportsProgress = true;
            MakeText();
            while (stopWatch.IsRunning)
            {
                backgroundWorker1.ReportProgress((int)progress);
                if ((int)progress == 100)
                {
                    stopWatch.Stop();
                    MessageBox.Show(error);
                }
            }
        }
        public void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
                if (progressBar1.Value == 100)
                {
                    progressBar1.Visible = false;
                    ProgPerc.Text = "";
                    Time.Text = "Elapsed Time: 00:00:00";
                    VidsUp.Text = "Videos Completed: 0";
                    button1.Visible = true;
                    textBox1.Text = "";
                }
            button1.Visible = true;
        }
        public void backgroundWorker1_ProgessChanged(object sender, ProgressChangedEventArgs e)
        {
            ts = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
                        ts.Hours, ts.Minutes, ts.Seconds);
            Time.Text = "Elapsed Time: " + elapsedTime;
            int progP = (int)Math.Round(progress);
            int Vupdate = vidcount;
            ProgPerc.Text = progP.ToString() + "%";
            VidsUp.Text = "Videos Completed: "+Vupdate.ToString();
            progressBar1.Value = (int)progress;
            progressBar1.Refresh();
        }
    }
}