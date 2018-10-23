using BestTVProgram.Core.Models;
using BestTVProgram.Services.Interfaces;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BestTVProgram.Services
{
    public class TVProgramService : IProgramService
    {
        private static List<ChannelWithProgram> channelsWithProgram;
        public List<ChannelWithProgram> ChannelsWithProgram => channelsWithProgram;
        public ChannelWithProgram this[string name] => ChannelsWithProgram.Where(c => c.Channel.Name == name).FirstOrDefault<ChannelWithProgram>();

        public TVProgramService()
        {
            channelsWithProgram = GetChannelsWithProgram();
        }

        public static void RefreshChannelsWithProgram()
        {
            channelsWithProgram = GetChannelsWithProgram();
        }

        public static List<ChannelWithProgram> GetChannelsWithProgram()
        {
            List<ChannelWithProgram> workList = new List<ChannelWithProgram>();
            string htmlCodeOfDownloadedPage;
            int channel_counter = 0;

            List<string> sites = new List<string>();
            sites.Add("http://www.vsetv.com/schedule_package_bybase_day_" + DateTime.Today.ToString("yyyy-MM-dd") + "_nsc_1.html");
            sites.Add("http://www.vsetv.com/schedule_package_rubase_day_" + DateTime.Today.ToString("yyyy-MM-dd") + "_nsc_1.html");
            //string site = "http://www.vsetv.com/schedule_package_bybase_day_" + DateTime.Today.ToString("yyyy-MM-dd") + "_nsc_1.html";
            //"http://www.vsetv.com/schedule_package_rubase_day_" + DateTime.Today.ToString("yyyy-MM-dd") + "_nsc_1.html";

            foreach (var site in sites)
            {


                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
                req.Headers.Add("Accept-Language: ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
                req.UserAgent = "Mozilla / 5.0(Windows NT 6.1; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 68.0.3440.106 Safari / 537.36";
                //            Cache - Control: max - age = 0
                //Upgrade - Insecure - Requests: 1
                //User - Agent: Mozilla / 5.0(Windows NT 6.1; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 68.0.3440.106 Safari / 537.36
                //Accept: text / html,application / xhtml + xml,application / xml; q = 0.9,image / webp,image / apng,*/*;q=0.8
                //Accept-Encoding: gzip, deflate
                //Accept-Language: ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (StreamReader stream = new StreamReader(
                     resp.GetResponseStream(), Encoding.GetEncoding(1251)))
                {
                    htmlCodeOfDownloadedPage = stream.ReadToEnd();
                }

                int start, end, len;
                string startPhrase = @"chlogo"; //@"<div class=""chlogo" //<div class="chlogo">
                string endPhrase = @"<td class=""base";
                end = 0;
                start = htmlCodeOfDownloadedPage.IndexOf(startPhrase, 1) - 11;
                end = htmlCodeOfDownloadedPage.IndexOf(endPhrase, start) - 1;//<li id="comment
                len = end - start;
                if ((end > 0) && (len > 0))
                {
                    string tempHtml = htmlCodeOfDownloadedPage.Substring(start, end - start).Replace("&nbsp;", "").Replace("<br>", "").Replace("\n", "").Replace("\t", "");

                    HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();

                    doc.LoadHtml(tempHtml);
                    foreach (HtmlAgilityPack.HtmlNode n in doc.DocumentNode.SelectNodes("div[@class='chlogo']")) //and @id!='comment-pinned'] //chlogo
                    {
                        bool b = n.HasAttributes;
                        HtmlNode htmlNode = n.NextSibling.NextSibling.NextSibling.NextSibling;

                        ChannelWithProgram channelWithProgram = new ChannelWithProgram();
                        string stringId = n.NextSibling.SelectSingleNode("table" +
                            //"/tbody"//);
                            //+
                            "/tr"//);
                            +
                           "/td"
                           +
                           "/a").Attributes["href"].Value;
                        var v = stringId.Where(s => char.IsDigit(s)).ToArray();
                        stringId = new string(v);

                        int id = Convert.ToInt32(stringId);
                        var result = workList.Select(c => c.Channel.Id).Contains(id);
                        if (result)
                        {
                            continue;
                        }

                        string tempString = n.NextSibling.SelectSingleNode("table" +
                        "/tr"//);
                        +
                       "/td[@class='channeltitle']").InnerHtml;

                        DateTime previousTime=DateTime.MinValue; //midNightTime
                        channelWithProgram.Channel = new Channel { Id = id, Name = tempString };
                        foreach (var item in htmlNode.ChildNodes)
                        {
                            if (item.Attributes.Contains("class"))
                            {
                                if ((item.Attributes["class"].Value == "pasttime") ||
                                    (item.Attributes["class"].Value == "onair") ||
                                    (item.Attributes["class"].Value == "time"))
                                {
                                    tempString = item.InnerText;
                                }
                                if (item.Attributes["class"].Value == "prname2" ||
                                    (item.Attributes["class"].Value == "pastprname2"))
                                {
                                    DateTime dateTime;
                                    try
                                    {
                                        dateTime = DateTime.Parse(tempString, new CultureInfo("ru-RU"));
                                        if (dateTime < previousTime) //если программа перешагнула через полночь
                                        {
                                            dateTime = dateTime.AddDays(1);
                                        }
                                    }
                                    catch
                                    {
                                        dateTime = DateTime.Now;
                                    }

                                    previousTime = dateTime;
                                    channelWithProgram.TVPrograms.Add(new TVProgram { DateTime = dateTime, Program = item.InnerText }); //str1 + " " + item.InnerText
                                }
                            }
                        }
                        channel_counter++;

                        workList.Add(channelWithProgram);
                    }
                }
            }
            return workList;
        }
    }
}
