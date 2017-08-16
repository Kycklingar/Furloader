using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Furloader.Sites;
using System.Threading.Tasks;

namespace Furloader
{
    public delegate void changedEventHandler(object sender, EventArgs e);
    public delegate void updateProgress(object sender, EventArgs e);

    enum SITES { FurAffinity, InkBunny }

    public class ProgressArg : EventArgs
    {
        public ProgressArg(bool setFiles, bool setNewFiles)
        {
            files = setFiles;
            newFiles = setNewFiles;
        }

        public ProgressArg(int setAmount)
        {
            amount = setAmount;
        }

        public int amount;
        public bool files;
        public bool newFiles;
    }

    public struct Submission
    {
        public int site;
        public string domain;
        public string id;
        public string title;
        public string author;
        public string pageSource;
        public string fileSource;
        public string filename;
        public string thumbSource;
        public Image thumbnail;
        public bool scrap;
    }

    public class WorkSheduler
    {
        private static readonly object locker = new object();
        private static readonly object locker2 = new object();
        private FurAffinity FA = new FurAffinity();
        private InkBunny IB = new InkBunny();
        //public List<Submission> submissions = new List<Submission>();
        public event ChangedGalleryEvent updateGallery;
        public event updateProgress updateProg;
        private int politeWait = 1000;
        private int threadCount = 0;
        private int maxThreads = 1;
        private bool pauseDownloads = false;
        private bool stopDownloads = false;

        private List<string> usersDownloading = new List<string>();
        private List<string> downloadList = new List<string>();


        public DataHandler datahandler = new DataHandler();


        public WorkSheduler()
        {
            FA.Changed += new changedEventHandler(ListChanged);
            datahandler.bootDB();
        }

        // Move this to the form calling it
        /*
        public void downloadAll(string author, int mode, string siteString, int wait, bool scraps, bool nice = false)
        {
            int site = getSiteIntFromString(siteString);

            if (nice)
            {
                maxThreads = 1;
                politeWait = wait;
            }
            else
            {
                maxThreads = wait;
                politeWait = 0;
            }

            switch (mode)
            {
                case (int)MODE.gallery:
                    if (author == "")
                        Console.WriteLine("Enter a valid user");
                    else
                        getSubmissions(author, site, scraps);
                    break;
                case (int)MODE.watchlist:
                    downloadWatchList(author, site, scraps);
                    break;
                case (int)MODE.subscription:
                    {
                        int pageLimit = 10;

                        Int32.TryParse(author, out pageLimit);

                        startSubscriptions(pageLimit, site);
                    }
                    break;
                case (int)MODE.refresh:
                    if (nice)
                        Console.WriteLine("Updating wait time: " + wait);
                    else
                        Console.WriteLine("Updating max threads: " + wait);
                    break;
                default:
                    break;
            }

        }
        */

        public void changeThreadCount(bool threads, int ammount)
        {
            if (threads)
            {
                if (ammount != maxThreads)
                    Console.WriteLine("Updating max threads: " + ammount);
                maxThreads = ammount;
                politeWait = 0;
            }
            else
            {
                if (ammount != politeWait)
                    Console.WriteLine("Updating wait time: " + ammount + "ms");
                maxThreads = 1;
                politeWait = ammount;
            }
        }

        public bool togglePause()
        {
            if (pauseDownloads)
                pauseDownloads = false;
            else
                pauseDownloads = true;

            return pauseDownloads;
        }

        public void stopExecution()
        {
            stopDownloads = true;
            pauseDownloads = false;
            politeWait = 0;
            while (threadCount > 0 || usersDownloading.Count() > 0)
                Thread.Sleep(20);
            stopDownloads = false;
        }

        private void onUpdateGallery(object sender, EventArgs e)
        {
            if (updateGallery != null)
            {
                updateGallery(sender, e);
            }
        }

        private void onUpdateProgress(EventArgs e)
        {
            if (updateProg != null)
            {
                updateProg(this, e);
            }
        }

        private void ListChanged(object sender, EventArgs e)
        {
            /*baseLogin obj = (baseLogin)sender;
            while(true)
            {
                Submission sub = obj.getNextImage();
                if(sub.title == null)
                {
                    break;
                }

                submissions.Add(sub);
                
                onUpdateGallery(sender, EventArgs.Empty);
            }*/
            onUpdateGallery(sender, EventArgs.Empty);
        }

        public void startSubscriptions(int pageLimit, string siteString, bool scraps)
        {
            Website site = getSiteFromString(siteString);

            Thread thread = new Thread(() => getSubscription(pageLimit, site, scraps));
            thread.IsBackground = true;
            thread.Start();
        }

        public void startSearch(string searchString, string siteString, bool scrap)
        {
            Website site = getSiteFromString(siteString);

            Thread thread = new Thread(() => searchDownload(searchString, site, scrap));
            thread.IsBackground = true;
            thread.Start();
        }

        private void getSubscription(int pageLimit, Website site, bool scraps)
        {
            List<Submission> submissions = new List<Submission>();
            submissions = site.getSubscription(pageLimit);

            if (submissions == null || !submissions.Any())
            {
                Console.WriteLine("Error while retrieving submissions.");
                return;
            }

            submissions.Reverse();

            string ID = "Subscription " + site.Name;
            usersDownloading.Add(ID);

            ProgressArg e = new ProgressArg(submissions.Count());
            onUpdateProgress(e);

            downloadSubmissions(submissions, site, scraps);

            Console.WriteLine(string.Format("Subscription on {0} has finished.", site.Name));
            usersDownloading.Remove(ID);

        }

        /*
        public void getSubmissions(string user, string siteString)
        {
            baseLogin site = getSiteFromString(siteString);

            Thread thread = new Thread(() => getSubs(user, site));
            thread.IsBackground = true;
            thread.Start();
        }
        */

        public void getSubmissions(string user, string siteString, bool scraps)
        {
            Website site = getSiteFromString(siteString);

            if (!usersDownloading.Contains(user + " " + site.Name))
            {
                Thread thread = new Thread(() => getSubs(user, site, scraps));
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                Console.WriteLine(user + " is already downloading.");
            }
            //if (scraps)
            //{
            //    if (!usersDownloading.Contains(user + " scraps"))
            //    {
            //        Thread thread = new Thread(() => getSubs(user, site, scraps));
            //        thread.IsBackground = true;
            //        thread.Start();
            //    }
            //    else
            //    {
            //        Console.WriteLine(user + " scraps is already downloading.");
            //    }
            //}

        }

        private void searchDownload(string searchString, Website site, bool scraps)
        {
            List<Submission> subList = new List<Submission>();
            subList = site.getSearchSubs(searchString);

            if (subList == null || !subList.Any())
            {
                Console.WriteLine("Error while retrieving submissions.");
                return;
            }

            subList.Reverse();

            string ID = string.Format("Search for {0} on {1}", searchString, site.Name);

            usersDownloading.Add(ID);

            ProgressArg e = new ProgressArg(subList.Count());
            onUpdateProgress(e);

            downloadSubmissions(subList, site, scraps);

            Console.WriteLine(string.Format("Search for {0} on {1} has finished downloading.", searchString, site.Name));
            usersDownloading.Remove(ID);
        }

        public void downloadWatchList(string author, string siteString, bool scraps)
        {
            Website site = getSiteFromString(siteString);

            Thread thread = new Thread(() => getWatchList(site, author, scraps));
            thread.IsBackground = true;
            thread.Start();
        }

        //private void getWatchList(baseLogin site, string username, bool scraps)
        //{
        //    try
        //    {
        //        List<watchList> WatchList;      // The final watchlist
        //        List<watchList> DBWatchList;   // The watchlist in the db

        //        DBWatchList = datahandler.getWatchList(site.Name, username);

        //        bool notFinishedList = false;

        //        foreach(watchList user in DBWatchList)
        //        {
        //            if (user.done)
        //            {
        //                notFinishedList = true;
        //                break;
        //            }
        //        }

        //        if (notFinishedList)
        //        {
        //            DialogResult dialogResult = MessageBox.Show("Wish to continue old download first?", "Continue?", MessageBoxButtons.YesNoCancel);
        //            if (dialogResult == DialogResult.Yes)
        //            {
        //                WatchList = DBWatchList;
        //            }
        //            else if (dialogResult == DialogResult.No)
        //            {
        //                WatchList = getWatchList(username, site);
        //            }
        //            else
        //            {
        //                WatchList = null;
        //            }

        //        }
        //        else
        //        {
        //            WatchList = getWatchList(username, site);
        //        }


        //        // Find new watched users that differs from the database
        //        List<watchList> newWatchList = new List<watchList>();
        //        foreach(watchList user in WatchList)
        //        {
        //            if(DBWatchList.Contains(user))
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                newWatchList.Add(user);
        //            }
        //        }

        //        if(newWatchList.Any())
        //        {
        //            foreach(watchList user in newWatchList)
        //            {
        //                Console.WriteLine("User: " + user.user + " Site: " + user.site);
        //            }
        //        }


        //        // Download the watchlist
        //        if (WatchList != null)
        //        {
        //            foreach (watchList user in WatchList)
        //            {
        //                while (pauseDownloads)
        //                {
        //                    if (stopDownloads)
        //                        break;
        //                    Thread.Sleep(50);
        //                }

        //                if (stopDownloads)
        //                {
        //                    break;
        //                }
        //                getSubs(user.user, site, scraps);
        //                //if (scraps && !stopDownloads)
        //                //{
        //                //    getSubs(user, site, scraps);
        //                //}
        //                if (!stopDownloads)
        //                {
        //                    datahandler.updateWatchList(site.Name, user.user, user.watchlistUser);
        //                }
        //                else
        //                    break;
        //                Thread.Sleep(politeWait);

        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //}

        private void getWatchList(Website site, string userToDownload, bool scraps)
        {
            try
            {
                string username = site.validateWatchlistUsername(userToDownload);

                watchList WatchList = new watchList();


                watchList DBWatchList = datahandler.getWatchList(site.Name, username);
                bool dbNotFinished = false;

                foreach (watchedUser user in DBWatchList.users)
                {
                    if (user.done == false)
                    {
                        dbNotFinished = true;
                        break;
                    }
                }

                if (dbNotFinished)
                {
                    DialogResult mssgbox = MessageBox.Show("Found unfinished download in database. Wish to finish?", "Finish", MessageBoxButtons.YesNoCancel);
                    if (mssgbox == DialogResult.Yes)
                    {
                        DialogResult mssgboxNew = MessageBox.Show("Do you want to find new watched users?", "Find new?", MessageBoxButtons.YesNo);
                        if (mssgboxNew == DialogResult.Yes)
                        {
                            //watchList newWatchList = new watchList();

                            //newWatchList.site = site.Name;
                            //newWatchList.watchlistUser = username;
                            //newWatchList.users = new List<watchedUser>();

                            //List<string> userStrings = site.getWatchList(username);

                            //foreach(string user in userStrings)
                            //{
                            //    watchedUser userW = new watchedUser{ user= user, done = false};
                            //    newWatchList.users.Add(userW);
                            //}
                            watchList newWatchList = makeWatchListFromUserStrings(site.getWatchList(username), site.Name, username);
                            WatchList = compareWatchList(newWatchList, DBWatchList);
                            datahandler.setWatchList(WatchList, false);
                        }
                        else
                        {
                            WatchList = DBWatchList;
                        }
                    }
                    else if (mssgbox == DialogResult.No)
                    {
                        if (DBWatchList.users.Any())
                        {
                            DialogResult msgbox = MessageBox.Show("Want to find new watched users only?", "New watches only?", MessageBoxButtons.YesNo);

                            watchList newWatchList = makeWatchListFromUserStrings(site.getWatchList(username), site.Name, username);

                            if (msgbox == DialogResult.Yes)
                            {
                                WatchList = compareWatchList(newWatchList, DBWatchList);
                                datahandler.setWatchList(WatchList, false);
                            }
                            else
                            {
                                WatchList = newWatchList;
                                datahandler.setWatchList(WatchList);
                            }
                        }
                    }
                    else if (mssgbox == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    if (DBWatchList.users.Any())
                    {
                        DialogResult msgbox = MessageBox.Show("Want to find new watched users only?", "New watches only?", MessageBoxButtons.YesNo);

                        watchList newWatchList = makeWatchListFromUserStrings(site.getWatchList(username), site.Name, username);

                        if (msgbox == DialogResult.Yes)
                        {
                            WatchList = compareWatchList(newWatchList, DBWatchList);
                            datahandler.setWatchList(WatchList, false);
                        }
                        else
                        {
                            WatchList = newWatchList;
                            datahandler.setWatchList(WatchList);
                        }
                    }
                    else
                    {
                        WatchList = makeWatchListFromUserStrings(site.getWatchList(username), site.Name, username);
                        datahandler.setWatchList(WatchList);
                    }

                }

                if (WatchList.users != null && WatchList.users.Any())
                {
                    Console.WriteLine("Found {0} watched users on {1} for {2}", WatchList.users.Count(), site.Name, username);
                    foreach (watchedUser user in WatchList.users)
                    {
                        if (!user.done)
                        {
                            while (pauseDownloads)
                            {
                                if (stopDownloads)
                                    break;
                                Thread.Sleep(50);
                            }

                            if (stopDownloads)
                            {
                                break;
                            }
                            getSubs(user.user, site, scraps);
                            //if (scraps && !stopDownloads)
                            //{
                            //    getSubs(user, site, scraps);
                            //}
                            if (!stopDownloads)
                            {
                                datahandler.updateWatchList(site.Name, user.user, WatchList.watchlistUser);
                            }
                            else
                                break;

                            Thread.Sleep(politeWait);
                        }
                    }
                    Console.WriteLine("Watchlist for {0} on {1} has finished.", username, site.Name);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private watchList compareWatchList(watchList left, watchList right)
        {
            watchList returnList = new watchList();
            returnList.site = left.site;
            returnList.watchlistUser = left.watchlistUser;
            returnList.users = new List<watchedUser>(right.users);

            List<watchedUser> newWatches = new List<watchedUser>();

            foreach (watchedUser leftUser in left.users)
            {
                bool isInRight = false;
                foreach (watchedUser rightUser in right.users)
                {
                    if (leftUser.user == rightUser.user)
                    {
                        isInRight = true;
                        break;
                    }
                }
                if (!isInRight)
                {
                    newWatches.Add(leftUser);
                }
            }

            List<watchedUser> oldInRight = new List<watchedUser>();

            foreach (watchedUser rightUser in right.users)
            {
                bool isInLeft = false;
                foreach (watchedUser leftUser in left.users)
                {
                    if (rightUser.user == leftUser.user)
                    {
                        isInLeft = true;
                        break;
                    }
                }
                if (!isInLeft)
                {
                    returnList.users.Remove(rightUser);
                }
            }

            returnList.users.AddRange(newWatches);

            return returnList;
        }

        private watchList getWatchList(string user, Website site)
        {
            List<string> userStrings = site.getWatchList(user);
            if (userStrings == null)
            {
                Console.WriteLine("Error while getting watchlist.");
                return new watchList();
            }
            //datahandler.setWatchList(site.Name, userStrings, user);
            return makeWatchListFromUserStrings(userStrings, site.Name, user);

        }

        private watchList makeWatchListFromUserStrings(List<string> userList, string site, string username)
        {
            watchList newWatchList = new watchList();
            newWatchList.site = site;
            newWatchList.watchlistUser = username;
            newWatchList.users = new List<watchedUser>();
            foreach (string user in userList)
            {
                watchedUser usr = new watchedUser { user = user, done = false };
                newWatchList.users.Add(usr);
            }
            return newWatchList;
        }

        public bool getLogin(string siteString)
        {
            Website site = getSiteFromString(siteString);

            if (site != null)
            {
                //Thread thread = new Thread(() => loginSite(site));
                //thread.IsBackground = true;
                //thread.Start();

                return site.login(datahandler);
            }
            return false;
        }

        public async Task<LoginData> GetLoginDataAsync(string siteString)
        {
            Website site = getSiteFromString(siteString);
            if(site != null)
            {
                return await site.GetLoginDataAsync();
            }
            return new LoginData();
        }

        public async Task<bool> loginSiteAsync(string siteString, string username, string password, string captcha = null)
        {
            Website site = getSiteFromString(siteString);

            if (site != null)
            {
                return await site.loginAsync(datahandler, username, password, captcha);
            }
            return false;
        }

        public async Task<bool> checkLogin(string siteString)
        {
            Website site = getSiteFromString(siteString);
            if (site != null)
            {
                loginCookies login = datahandler.getLogin(site.Name);

                return await site.checkLogin(login);
            }
            return false;
        }

        private void getSubs(string user, Website site, bool scraps)
        {
            string userThreadID;
            //if (scraps)
            //    userThreadID = user + " scraps";
            //else

            userThreadID = string.Format("{0} {1}", user, site.Name);

            usersDownloading.Add(userThreadID);

            List<Submission> submissions;


            submissions = site.loadSubbmissionsFromUser(user, scraps);

            if (submissions == null)
            {
                Console.WriteLine("Error while retrieving submissions.");
                return;
            }

            submissions.Reverse();

            Console.WriteLine(string.Format("Found {0} submissions for {1} on {2}", submissions.Count, user, site.Name));

            ProgressArg e = new ProgressArg(submissions.Count());
            onUpdateProgress(e);

            downloadSubmissions(submissions, site, scraps);

            Console.WriteLine(userThreadID + " has finished downloading.");
            usersDownloading.Remove(userThreadID);
        }

        private void downloadSubmissions(List<Submission> submissions, Website site, bool scrap)
        {
            foreach (Submission sub in submissions)
            {
                lock (locker)
                {
                    if (isBeingDownloaded(sub.pageSource))
                    {
                        continue;
                    }
                    else if (datahandler.isPageSource(sub.pageSource))
                    {
                        onUpdateProgress(new ProgressArg(true, false));
                        continue;
                    }
                    downloadList.Add(sub.pageSource);

                }

                while (true)
                {
                    lock (locker2)
                    {
                        if (!(pauseDownloads || threadCount >= maxThreads))
                        {
                            threadCount++;
                            break;
                        }
                    }
                    Thread.Sleep(10);
                }

                Thread.Sleep(politeWait);

                if (stopDownloads)
                {
                    downloadList.Remove(sub.pageSource.ToString());
                    threadCount--;
                    break;
                }

                lock (locker)
                {
                    Thread thread = new Thread(() => downloadImage(sub, site, scrap));
                    thread.IsBackground = true;
                    thread.Start();
                }

            }
        }

        private bool isBeingDownloaded(string page)
        {

            return downloadList.Contains(page);

        }

        private Website getSiteFromString(string siteString)
        {
            Website site;
            switch (siteString.ToLower())
            {
                case "fa":
                    site = FA;
                    break;
                case "furaffinity":
                    site = FA;
                    break;
                case "inkbunny":
                    site = IB;
                    break;
                default:
                    site = null;
                    break;
            }
            return site;
        }

        private Website getSiteFromInt(int x)
        {
            Website site;
            switch (x)
            {
                case (int)SITES.FurAffinity:
                    site = FA;
                    break;
                default:
                    site = null;
                    break;
            }
            return site;
        }

        private int getSiteIntFromString(string site)
        {
            string ssite = "";
            if (site != null)
                ssite = site.ToLower();

            switch (ssite)
            {
                case "furaffinity":
                    return (int)SITES.FurAffinity;
                default:
                    return -1;
            }
        }

        private bool downloadImage(Submission sub, Website site, bool scraps = false)
        {
            bool status = false;
            try
            {
                sub = site.getSubInfo(sub);

                if (!sub.scrap || (sub.scrap && scraps))
                {
                    byte[] image = site.getImage(sub.fileSource);

                    datahandler.createFile(sub.domain, sub.fileSource, sub.pageSource, sub.author, sub.filename, image, sub.scrap);
                }
                status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e + Environment.NewLine + sub.pageSource.ToString() + Environment.NewLine + sub.id);
            }
            lock (locker)
                downloadList.Remove(sub.pageSource.ToString());
            ProgressArg prog = new ProgressArg(true, true);
            onUpdateProgress(prog);
            threadCount--;
            return status;
        }

    }
}
