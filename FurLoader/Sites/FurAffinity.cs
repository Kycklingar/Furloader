using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Furloader.Sites
{
    public class FurAffinity : Website
    {
        public override string Name { get { return "furaffinity"; } }

        private const string FABase = "https://www.furaffinity.net/";
        private const string FALoginPage = "https://www.furaffinity.net/login/";
        private const string FACaptcha = "https://furaffinity.net/captcha.jpg";

        private string userLoginName;

        private WebHandler webHandler = new WebHandler();


        //private int pos = 0;

        //List<String> thumbsList;
        //List<Subs> subs = new List<Subs>();


        //public override event changedEventHandler Changed;

        //protected virtual void onChanged(EventArgs e)
        //{
        //    if (Changed != null)
        //    {
        //        Changed(this, e);
        //    }
        //}

        public override string getCookies()
        {
            Uri fa = new Uri(FABase);
            string cookies = webHandler.getCookies(fa);
            return cookies;
        }

        //public override Submission getNextImage()
        //{

        //    if (pos >= subs.Count())
        //    {
        //        return new Submission();
        //    }
        //    int i = pos;
        //    pos++;

        //    string uri = subs[i].thumbnail.ToString();
        //    Image image = webHandler.getImage(uri);

        //    Submission sub = new Submission();
        //    sub.thumbnail = image;
        //    sub.title = subs[i].title;
        //    string str = string.Format("FA_{0}", subs[i].id);
        //    sub.pageSource = str;

        //    return sub;
        //}

        //public override List<Subs> getThumbsList()
        //{
        //    return subs;
        //}

        public override async Task<bool> checkLogin(loginCookies login)
        {
            if (login.username == "" || login.cookies == "")
            {
                return false;
            }


            webHandler.setCookies(login.cookies);

            userLoginName = login.username;

            return await isLoggedInAsync();
        }

        public override async Task<LoginData> GetLoginDataAsync()
        {
            string html = await webHandler.getPageAsync(FALoginPage);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            string captchaLink = doc.GetElementbyId("captcha_img").Attributes["src"].Value;

            return new LoginData { Captcha = await webHandler.getImageAsync(FABase + captchaLink) };
        }

        public override async Task<bool> loginAsync(DataHandler datahandler, string username, string password, string captcha)
        {
            string postData = string.Format("action=login&name={0}&pass={1}&g-recaptcha-response=&use_old_captcha=1&captcha={2}&login={3}",
                username,
                password,
                captcha,
                "Login to%C2%A0FurAffinity");

            await webHandler.getPageAsync(FALoginPage + "?ref=https://furaffinity.net/", postData);

            if (await isLoggedInAsync())
            {
                Uri uri = new Uri(FABase);
                string cookie = webHandler.getCookies(uri);
                datahandler.setLogin("furaffinity", cookie, username, "");
                return true;
            }
            return false;
        }

        public override string validateWatchlistUsername(string username)
        {
            return username.Trim(' ') != "" ? username : userLoginName;
        }

        /*public override void loadSubbmissionsFromUser(string user)
        {
            string url = "http://www.furaffinity.net/gallery/" + user + "/";

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            string page = webHandler.getPage(url);
            doc.LoadHtml(page);

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='submission-list']/center/b"))
            {
                string thumb = node.SelectSingleNode(".//img").GetAttributeValue("src", "null");
                string sid = node.GetAttributeValue("id", "null");
                string title = node.SelectSingleNode(".//span").InnerText;
                if (sid != "null" && thumb != "null")
                {
                    string ssid = sid.Remove(0, 4);
                    int id = Convert.ToInt32(ssid);
                    Subs sub = new Subs();
                    
                    Uri uri = new Uri("http:" + thumb);
                    sub.id = id;
                    sub.thumbnail = uri;
                    sub.title = title;
                    
                    subs.Add(sub);
                }
            }

            onChanged(EventArgs.Empty);
        }*/

        public override List<Submission> loadSubbmissionsFromUser(string user, bool scraps = false)
        {

            List<Submission> subList = new List<Submission>();

            string scrapUrl = FABase + "scraps/" + user + "/";
            string galleryUrl = FABase + "gallery/" + user + "/";
            try
            {
                subList.AddRange(dlGallery(galleryUrl));
                if (scraps)
                    subList.AddRange(dlGallery(scrapUrl));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //if (scraps)
            //    Console.WriteLine("Found " + subList.Count() + " scraps for " + user);
            //else
            //    Console.WriteLine("Found " + subList.Count() + " submissions for " + user);

            return subList;
        }

        public override List<Submission> getSearchSubs(string searchString)
        {
            string url = FABase + "search/";
            string extraData = "perpage=72&order-by=date&order-direction=desc&range=all&rating-general=on&rating-mature=on&rating-adult=on&type-art=on&type-flash=on&type-photo=on&type-music=on&type-story=on&type-poetry=on&mode=extended";

            string data = string.Format("q={0}&{1}&do_search=Search", searchString, extraData);

            string html = webHandler.getPage(url, data);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            int pageNum = 1;

            string nextPage = "";

            List<Submission> subList = new List<Submission>();

            bool done = false;
            while (!done)
            {
                if (pageNum > 1)
                {
                    data = string.Format("q={0}&page={1}&{2}&nex_page={3}", searchString, pageNum - 1, extraData, nextPage);
                    html = webHandler.getPage(url, data);
                    doc.LoadHtml(html);
                }

                HtmlNode next = doc.DocumentNode.SelectSingleNode("//div[@id='search-results']/fieldset/input[@name='next_page']");

                if (next == null)
                    done = true;
                else
                {
                    nextPage = next.GetAttributeValue("value", null);
                }


                HtmlNodeCollection submissionNodes = doc.DocumentNode.SelectNodes("//div[@id='search-results']/fieldset/section/figure");

                if (submissionNodes != null)
                {
                    foreach (HtmlNode node in submissionNodes)
                    {
                        string thumb = node.SelectSingleNode(".//img").GetAttributeValue("src", "null");
                        string sid = node.GetAttributeValue("id", "null");
                        string title = node.SelectSingleNode(".//figcaption/p/a").InnerText;
                        if (sid != "null" && thumb != "null")
                        {
                            sid = sid.Remove(0, 4);
                            Submission sub = new Submission();

                            sub.domain = "www.furaffinity.net";
                            sub.site = SITES.FurAffinity;
                            sub.id = sid;
                            sub.pageSource = string.Format("FA_{0}", sid);
                            sub.thumbSource = "http:" + thumb;
                            sub.title = title;

                            subList.Add(sub);
                        }
                    }
                }
                else
                {
                    done = true;
                }

                pageNum++;
            }

            return subList;
        }

        private List<Submission> dlGallery(string baseUrl)
        {
            bool finished = false;
            int pageN = 1;

            List<Submission> subList = new List<Submission>();

            while (!finished)
            {
                string url = baseUrl + pageN;

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                string page = webHandler.getPage(url, false);

                doc.LoadHtml(page);

                try
                {
                    HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='submission-list']//figure");
                    if (nodes != null)
                    {
                        foreach (HtmlNode node in nodes)
                        {
                            string thumb = node.SelectSingleNode(".//img").GetAttributeValue("src", "null");
                            string sid = node.GetAttributeValue("id", "null");
                            string title = node.SelectSingleNode(".//figcaption/p/a").InnerText;
                            if (sid != "null" && thumb != "null")
                            {
                                sid = sid.Remove(0, 4);
                                Submission sub = new Submission();

                                sub.domain = "www.furaffinity.net";
                                sub.site = SITES.FurAffinity;
                                sub.id = sid;
                                sub.pageSource = string.Format("FA_{0}", sid);
                                sub.thumbSource = "http:" + thumb;
                                sub.title = title;

                                subList.Add(sub);
                            }
                        }
                    }
                    else
                        finished = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    finished = true;
                }
                pageN++;
            }
            return subList;
        }

        public override List<Submission> getSubscription(int pageLimit)
        {
            List<Submission> subList = new List<Submission>();
            string baseUrl = FABase;
            string nextUrl = baseUrl + "msg/submissions/";

            for (int i = 0; i < pageLimit; i++)
            {
                string html = webHandler.getPage(nextUrl, false);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@id='messagecenter-submissions']//figure");

                if (nodes != null)
                {
                    foreach (HtmlNode node in nodes)
                    {
                        string thumb = node.SelectSingleNode(".//img").GetAttributeValue("src", "null");
                        string sid = node.GetAttributeValue("id", "null");
                        string title = node.SelectSingleNode(".//figcaption/label/p/a").InnerText;
                        if (sid != "null" && thumb != "null")
                        {
                            sid = sid.Remove(0, 4);
                            Submission sub = new Submission();

                            sub.domain = "www.furaffinity.net";
                            sub.site = SITES.FurAffinity;
                            sub.id = sid;
                            //sub.pageSource = string.Format("{0}view/{1}", FABase, sid);
                            sub.pageSource = string.Format("FA_{0}", sid);
                            sub.thumbSource = "http:" + thumb;
                            sub.title = title;

                            subList.Add(sub);
                        }
                    }
                }
                else
                {
                    break;
                }

                string next;
                HtmlNode nNode = doc.DocumentNode.SelectSingleNode("//a[@class='more'] | //a[@class='more-half']");
                if (nNode != null)
                {
                    next = nNode.GetAttributeValue("href", null);
                    nextUrl = baseUrl + next;
                }
                else
                {
                    break;
                }
            }
            return subList;
        }

        //private Subs getSubInfo(string pageSource)
        //{
        //    Subs sub = new Subs();
        //    try
        //    {
        //        string subHtml = webHandler.getPage(sourceFromId(pageSource));
        //        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        //        doc.LoadHtml(subHtml);
        //        //sub.title = doc.DocumentNode.SelectSingleNode("//td[@class='cat']/b").InnerText;

        //        sub.author = doc.DocumentNode.SelectSingleNode("//td[@class='cat']/a").InnerText;
        //        //string url = "http:" + doc.DocumentNode.SelectSingleNode("//img[@id='submissionImg']").GetAttributeValue("src", null);
        //        string url = "http:" + doc.DocumentNode.SelectSingleNode("//div[@class='alt1 actions aligncenter']/b[2]/a").GetAttributeValue("href", null);
        //        Uri uri = new Uri(url);
        //        sub.fileSrc = uri;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //    return sub;
        //}

        private string sourceFromId(string pageSource)
        {
            return string.Format("{0}view/{1}/", FABase, pageSource.Remove(0, 3));
        }

        public override Submission getSubInfo(Submission sub)
        {

            string subHtml = webHandler.getPage(sourceFromId(sub.pageSource), false);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(subHtml);
            //sub.title = doc.DocumentNode.SelectSingleNode("//td[@class='cat']/b").InnerText;
            sub.author = doc.DocumentNode.SelectSingleNode("//td[@class='cat']/a").InnerText;
            //string url = "http:" + doc.DocumentNode.SelectSingleNode("//img[@id='submissionImg']").GetAttributeValue("src", null);
            string url = "http:" + doc.DocumentNode.SelectSingleNode("//div[@class='alt1 actions aligncenter']/b[a='Download']/a").GetAttributeValue("href", null);

            sub.fileSource = url;

            int lastIndex = url.LastIndexOf('/');
            sub.filename = url.Substring(lastIndex + 1, (url.Length - lastIndex - 1));

            string scraps = doc.DocumentNode.SelectSingleNode("//a[@class='dotted']").InnerText;
            if (scraps.Contains("Scraps"))
                sub.scrap = true;
            else
                sub.scrap = false;

            return sub;
        }

        /*public override void getImage(string pageSource)
        {
            DataHandler datahandler = new DataHandler();

            
            if (datahandler.isPageSource(pageSource))
            {

            }
            else
            {
                try
                {
                    Subs sub = getSubInfo(pageSource);
                    string filename = sub.fileSrc.Segments[sub.fileSrc.Segments.Length - 1];
                    byte[] image = webHandler.getBinary(sub.fileSrc.ToString());
                    datahandler.createFile("furaffinity", sub.fileSrc.ToString(), pageSource, sub.author, filename, image);
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

        }*/

        public override byte[] getImage(string fileSource)
        {
            return webHandler.getBinary(fileSource);
        }

        public override List<string> getWatchList(string user)
        {
            if (user == "" || user.Replace(" ", "") == "")
                return null;
            List<string> watchList = new List<string>();
            try
            {
                bool finished = false;
                string watchlistUrl = string.Format("{0}watchlist/by/{1}/", FABase, user);
                int pageN = 1;
                while (!finished)
                {

                    string page = webHandler.getPage(watchlistUrl + pageN, false);

                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(page);

                    HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//table[@id='userpage-budlist']/tr");

                    if (nodes != null && nodes.Count() >= 2)
                    {
                        foreach (HtmlNode node in nodes)
                        {
                            HtmlNode subNode = node.SelectSingleNode(".//a");
                            if (subNode == null)
                                continue;
                            string username = subNode.GetAttributeValue("href", null);
                            username = username.Remove(0, 6);
                            username = username.TrimEnd('/');
                            watchList.Add(username);
                        }
                    }
                    else
                        finished = true;
                    pageN++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return watchList;

        }

        private async Task<bool> isLoggedInAsync()
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            string web = await webHandler.getPageAsync(FABase);
            doc.LoadHtml(web);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//a[@href='/submit/']");

            if (node == null)
            {
                return false;
            }

            string cookies = getCookies();

            return true;
        }
    }
}
