using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Furloader.Sites
{

    public struct Subs
    {
        public Uri thumbnail;
        public Uri fileSrc;
        public int id;
        public string title;
        //public string description;
        public string author;
        //public List<string> tags;
    }

    public struct loginCookies
    {
        public string username;
        public string password;
        public string cookies;
    }

    public struct LoginData
    {
        public Image Captcha;
    }

    public class Website
    {
        public virtual bool login(DataHandler datahandler) { return false; }
        public virtual async Task<bool> loginAsync(DataHandler datahandler, string username, string password, string captcha = null) { return false; }
        public virtual async Task<LoginData> GetLoginDataAsync() { return new LoginData(); }
        public virtual string validateWatchlistUsername(string username) { return null; }
        public virtual async Task<bool> checkLogin(loginCookies login) { return false; }
        public virtual List<Submission> loadSubbmissionsFromUser(string user, bool scraps = false) { return null; }
        public virtual Submission getNextImage() { return new Submission(); }
        //public virtual void getImage(string pageSource) { }
        public virtual byte[] getImage(string fileSource) { return null; }
        public virtual List<Subs> getThumbsList() { return null; }
        public virtual event changedEventHandler Changed;
        public virtual string getCookies() { return null; }
        public virtual Submission getSubInfo(Submission sub) { return new Submission(); }
        public virtual List<String> getWatchList(string user) { return null; }
        public virtual string Name { get; }
        public virtual List<Submission> getSubscription(int pageLimit) { return null; }
        public virtual List<Submission> getSearchSubs(string searchString) { return null; }
    }
}
