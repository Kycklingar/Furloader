using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Furloader.Sites
{
    public class FurAffinity : Website
    {
        public override string Name { get { return "furaffinity"; } }

        private const string FABase = "https://www.furaffinity.net/";
        private const string FALoginPage = "https://www.furaffinity.net/login/";
        private const string FACaptcha = "https://furaffinity.net/captcha.jpg";

        private int pos = 0;
        private string userLoginName;

        //List<String> thumbsList;
        List<Subs> subs = new List<Subs>();

        private WebHandler webHandler = new WebHandler();

        public override event changedEventHandler Changed;

        protected virtual void onChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        public override string getCookies()
        {
            Uri fa = new Uri(FABase);
            string cookies = webHandler.getCookies(fa);
            return cookies;
        }

        public override Submission getNextImage()
        {

            if (pos >= subs.Count())
            {
                return new Submission();
            }
            int i = pos;
            pos++;

            string uri = subs[i].thumbnail.ToString();
            Image image = webHandler.getImage(uri);

            Submission sub = new Submission();
            sub.thumbnail = image;
            sub.title = subs[i].title;
            string str = string.Format("{0}view/{1}", FABase, subs[i].id);
            sub.pageSource = str;

            return sub;
        }

        public override List<Subs> getThumbsList()
        {
            return subs;
        }

        public override bool checkLogin(loginCookies login)
        {
            if (login.username == "" || login.cookies == "")
            {
                return false;
            }


            webHandler.setCookies(login.cookies);

            userLoginName = login.username;

            return isLoggedIn();
        }

        public override void login(DataHandler datahandler)
        {
            bool loggedIn = false;
            string html = webHandler.getPage(FALoginPage);
            string username, password;
            //Didn't know what I was doing, cant be bothered to fix
            do
            {

                Image image = webHandler.getImage(FACaptcha);
                furaffinity modal = new furaffinity();
                modal.setCaptcha(image);
                modal.ShowDialog();


                List<string> list;
                list = modal.getInputs();

                username = list[0];
                password = list[1];

                string postData = string.Format("action=login&name={0}&pass={1}&g-recaptcha-response=&use_old_captcha=1&captcha={2}&login={3}",
                    list[0],
                    list[1],
                    list[2],
                    "Login to%C2%A0FurAffinity");

                html = webHandler.getPage(FALoginPage + "?ref=https://furaffinity.net/", postData);

                loggedIn = isLoggedIn();
                if (!loggedIn) MessageBox.Show("Login Failed!");
            } while (!loggedIn);
            MessageBox.Show("Success!");
            Uri uri = new Uri(FABase);
            string cookie = webHandler.getCookies(uri);
            datahandler.setLogin("furaffinity", cookie, username, password);

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
                            sub.id = sid;
                            sub.pageSource = string.Format("{0}view/{1}", FABase, sid);
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
                string page = webHandler.getPage(url);

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
                                sub.id = sid;
                                sub.pageSource = string.Format("{0}view/{1}", FABase, sid);
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
                string html = webHandler.getPage(nextUrl);
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
                            sub.id = sid;
                            sub.pageSource = string.Format("{0}view/{1}", FABase, sid);
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

        private Subs getSubInfo(string pageSource)
        {
            Subs sub = new Subs();
            try
            {
                string subHtml = webHandler.getPage(pageSource);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(subHtml);
                //sub.title = doc.DocumentNode.SelectSingleNode("//td[@class='cat']/b").InnerText;

                sub.author = doc.DocumentNode.SelectSingleNode("//td[@class='cat']/a").InnerText;
                //string url = "http:" + doc.DocumentNode.SelectSingleNode("//img[@id='submissionImg']").GetAttributeValue("src", null);
                string url = "http:" + doc.DocumentNode.SelectSingleNode("//div[@class='alt1 actions aligncenter']/b[2]/a").GetAttributeValue("href", null);
                Uri uri = new Uri(url);
                sub.fileSrc = uri;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return sub;
        }

        public override Submission getSubInfo(Submission sub)
        {

            string subHtml = webHandler.getPage(sub.pageSource);
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

                    string page = webHandler.getPage(watchlistUrl + pageN);

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

        private bool isLoggedIn()
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            string web = webHandler.getPage(FABase);
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
