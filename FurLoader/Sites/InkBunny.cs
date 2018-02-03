using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Furloader.Sites
{
    class InkBunny : Website
    {
        public override string Name { get { return "inkbunny"; } }

        private static string baseUrl = "https://inkbunny.net/";

        private static string domain = "inkbunny.net";

        private string sid;

        private string loggedinUsername;

        public override async Task<bool> loginAsync(DataHandler datahandler, string username, string password, string captcha = null)
        {
            try
            {
                WebHandler webHandler = new WebHandler();
                string html = await webHandler.getPageAsync(baseUrl + "api_login.php", string.Format("username={0}&password={1}", username, password));

                dynamic json = JsonConvert.DeserializeObject(html);
                sid = json.sid;

                if(sid == "" || jsonError(json))
                {
                    return false;
                }

                datahandler.setLogin(Name, sid, username, "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            loggedinUsername = username;
            return true;
        }

        public override async Task<bool> checkLogin(loginCookies login)
        {
            WebHandler webHandler = new WebHandler();
            
            string data = string.Format("sid={0}", login.cookies);
            string url = baseUrl + "api_submissions.php";

            string html = await webHandler.getPageAsync(url, data);

            dynamic json = JsonConvert.DeserializeObject(html);

            if(json.sid == "" || jsonError(json))
            {
                return false;
            }

            sid = json.sid;

            loggedinUsername = login.username;
            
            return true;
        }

        public override string validateWatchlistUsername(string username)
        {
            return loggedinUsername;
        }

        public override List<Submission> loadSubbmissionsFromUser(string user, bool scraps = false)
        {
            WebHandler webHandler = new WebHandler();
            string url = baseUrl + "api_search.php";
            string data = string.Format("scraps=no&submissions_per_page=100&get_rid=yes&sid={0}&username={1}", sid, user);
            if (scraps)
                data = string.Format("submissions_per_page=100&get_rid=yes&sid={0}&username={1}", sid, user);

            return scanWithRid(url, data);
        }

        public override List<Submission> getSubscription(int pagelimit)
        {
            string url = baseUrl + "api_search.php";
            string data = string.Format("submissions_per_page=100&get_rid=yes&unread_submissions=yes&sid={0}", sid);

            return scanWithRid(url, data, pagelimit);
        }

        public override List<Submission> getSearchSubs(string searchString)
        {
            string url = baseUrl + "api_search.php";
            string data = string.Format("submissions_per_page=100&get_rid=yes&sid={0}&text={1}", sid, searchString);

            return scanWithRid(url, data);
        }

        public override List<string> getWatchList(string author)
        {
            string url = baseUrl + "api_watchlist.php";
            string data = string.Format("sid={0}", sid);

            WebHandler webHandler = new WebHandler();

            string html = webHandler.getPage(url, data);

            dynamic json = JsonConvert.DeserializeObject(html);

            if (jsonError(json))
                return null;

            List<string> list = new List<string>();

            foreach(dynamic subJson in json.watches)
            {
                list.Add((string)subJson.username);
            }

            return list;
        }

        public override Submission getSubInfo(Submission sub)
        {
            string url = baseUrl + "api_submissions.php";
            WebHandler webHandler = new WebHandler();

            string subId;

            if (sub.id.Contains("fID"))
            {
                subId = sub.pageSource.Substring(sub.pageSource.IndexOf("_") + 1, sub.pageSource.LastIndexOf("_") - 3);
            }
            else
            {
                subId = sub.id;
            }


            string data = string.Format("sid={0}&submission_ids={1}", sid, subId);
            string html = webHandler.getPage(url, data);

            dynamic json = JsonConvert.DeserializeObject(html);

            if (json.submissions[0].scraps == "f")
            {
                sub.scrap = false;
            }
            else
            {
                sub.scrap = true;
            }

            int length = json.submissions[0].files.Count;

            if(length > 1)
            {
                foreach (dynamic subJson in json.submissions[0].files)
                {
                    if (subJson.file_id == sub.id.Substring(3))
                    {
                        sub.fileSource = subJson.file_url_full;
                        sub.filename = subJson.file_name;
                    }
                }
            }
            else
            {
                dynamic subJson = json.submissions[0].files[0];

                sub.fileSource = subJson.file_url_full;
                sub.filename = subJson.file_name;
            }

            
            return sub;
        }

        public override byte[] getImage(string fileSource)
        {
            WebHandler webHandler = new WebHandler();

            if(fileSource.Contains("private_files"))
            {
                return webHandler.getBinary(fileSource + string.Format("?sid={0}", sid));
            }
            return webHandler.getBinary(fileSource);
        }

        private List<Submission> parseSubmissions(dynamic json)
        {
            List<Submission> subs = new List<Submission>();

            foreach (dynamic subJson in json.submissions)
            {
                if ((int)subJson.pagecount > 1)
                {
                    subs.AddRange(getSubFiles((int)subJson.submission_id));
                }
                else
                {
                    Submission sub = new Submission();

                    sub.domain = domain;
                    sub.site = SITES.InkBunny;
                    sub.author = subJson.username;
                    sub.title = subJson.title;
                    sub.id = subJson.submission_id;
                    sub.pageSource = string.Format("IB_{0}", sub.id);

                    subs.Add(sub);
                }
            }
            return subs;
        }

        private List<Submission> scanWithRid(string url, string data, int pageLimit=0)
        {
            WebHandler webHandler = new WebHandler();

            string html = webHandler.getPage(url, data);

            dynamic json = JsonConvert.DeserializeObject(html);

            if (jsonError(json))
                return null;

            int pages = json.pages_count;
            string rid = json.rid;

            List<Submission> subs = new List<Submission>();

            int currentPage = 1;

            while (pages >= currentPage && (pageLimit == 0 || currentPage <= pageLimit))
            {
                if (currentPage > 1)
                {
                    data = string.Format("submissions_per_page=100&sid={0}&rid={1}&page={2}", sid, rid, currentPage);
                    html = webHandler.getPage(url, data);
                    json = JsonConvert.DeserializeObject(html);

                    if (jsonError(json))
                        return null;
                }

                subs.AddRange(parseSubmissions(json));

                currentPage++;
            }
            return subs;
        }

        // Get all the files related to the submission id
        private List<Submission> getSubFiles(int subId)
        {
            string url = baseUrl + "api_submissions.php";
            WebHandler webHandler = new WebHandler();

            string data = string.Format("sid={0}&submission_ids={1}", sid, subId);
            string html = webHandler.getPage(url, data);

            dynamic json = JsonConvert.DeserializeObject(html);

            List<Submission> subs = new List<Submission>();

            string username = json.submissions[0].username;

            string title = json.submissions[0].title;

            int iteration = 0;

            foreach(dynamic subJson in json.submissions[0].files)
            {
                Submission sub = new Submission();

                sub.domain = domain;
                sub.site = SITES.InkBunny;
                sub.author = username;
                sub.title = string.Format("{0}-{1}", title, iteration++);
                sub.id = "fID" + subJson.file_id;
                sub.pageSource = string.Format("IB_{0}_{1}", subId, sub.id);

                subs.Add(sub);

            }
            return subs;
        }

        // Check json for error codes
        private bool jsonError(dynamic json)
        {
            if(json.error_code != null)
            {
                Console.WriteLine(string.Format("{0} : {1}", json.error_code, json.error_message));
                return true;
            }

            return false;
        }

    }
}
