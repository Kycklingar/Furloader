using Furloader.Sites;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;


namespace Furloader
{

    public struct watchList
    {
        public string site;
        public string watchlistUser;

        public List<watchedUser> users;
    }

    public struct watchedUser
    {
        public string user;
        public bool done;
    }

    public class DataHandler
    {
        private const int version = 2;

        // SQLite objects
        SQLiteConnection cnn;


        private static readonly object locker = new object();


        public DataHandler()
        {
            cnn = new SQLiteConnection("Data Source=FLDB.db;Version=3");
        }

        private bool isFirstLaunch()
        {
            cnn.Open();
            try
            {
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM dbinfo", cnn);
                int i = 0;
                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    i++;

                reader.Close();
                if (i == 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                //MessageBox.Show("Error " + e);
                return true;
            }
            finally
            {
                cnn.Close();
            }
        }

        public int bootDB()
        {
            int dbVersion = 0;
            if (isFirstLaunch())
            {
                //MessageBox.Show("This program stores usernames and passwords in plain text.\nUse on your own risk", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                dbVersion = versionInfo();
            }

            while (version > dbVersion)
            {
                dbVersion++;
                Console.WriteLine("Updating DB to version " + dbVersion);
                dbVersion = updateDB(dbVersion);
                if (dbVersion == 0)
                {
                    Console.WriteLine("Something went wrong. Contact a dev!");
                    break;
                }

                try
                {
                    cnn.Open();
                    SQLiteCommand cmd = new SQLiteCommand("UPDATE dbinfo SET version = @ver", cnn);
                    cmd.Parameters.AddWithValue("ver", dbVersion);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    cnn.Close();
                }

                
            }

            return dbVersion;
        }
        
        private int updateDB(int toDBver)
        {
            int result = 0;
            try
            {
                cnn.Open();

                switch (toDBver)
                {
                    case 1:
                        {
                            string cmdText = Properties.Resources.SQL_001;
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, cnn);
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "INSERT INTO dbinfo VALUES(@version)";
                            cmd.Parameters.Add(new SQLiteParameter("version", 1));
                            cmd.ExecuteNonQuery();
                            result = 1;
                        }
                        break;

                    case 2:
                        {
                            string cmdText = Properties.Resources.SQL_002;
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, cnn);
                            cmd.ExecuteNonQuery();
                            result = 2;
                        }
                        break;

                    default:
                        result = 0;
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                cnn.Close();
            }
            return result;
        }

        private void removeWhitespace(string table, string column)
        {
            try
            {
                string str = string.Format("UPDATE {0} SET {1} = RTRIM({1})", table, column);
                SQLiteCommand cmd = new SQLiteCommand(str, cnn);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private int versionInfo()
        {
            int database_version = 0;
            try
            {
                cnn.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT version FROM dbinfo", cnn);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        database_version = (int)reader["version"];
                        Console.WriteLine("Database version: " + database_version.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                cnn.Close();
            }
            return database_version;
        }

        public loginCookies getLogin(string site)
        {
            loginCookies login = new loginCookies();
            try
            {
                cnn.Open();
                string str = string.Format("SELECT site_id FROM sites WHERE site='{0}'", site);
                SQLiteCommand cmd = new SQLiteCommand(str, cnn);
                string site_id = null;
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        site_id = reader[0].ToString();
                    }
                }
                cmd.CommandText = string.Format("SELECT * FROM logins WHERE site_id='{0}'", site_id);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        login.cookies = reader["cookie"].ToString().TrimEnd(' ');
                        login.username = reader["username"].ToString().TrimEnd(' ');
                        login.password = reader["password"].ToString().TrimEnd(' ');
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            cnn.Close();
            return login;
        }

        public void setLogin(string site, string cookie, string username, string password)
        {
            try
            {
                /*
                cnn.Open();

                string str = string.Format("SELECT site_id FROM sites WHERE site='{0}'", site);
                SQLiteCommand cmd = new SQLiteCommand(str, cnn);
                string site_id = null;
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        site_id = reader[0].ToString();
                    }

                }
                cnn.Close();
                if (site_id == null)
                {
                    site_id = createSite(site);
                }
                */

                string site_id = createSite(site);
                setLogins(site_id, cookie, username, password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void setLogins(string site_id, string cookie, string username, string password)
        {
            try
            {
                cnn.Open();
                int ind = 0;
                string str = string.Format("SELECT * FROM logins WHERE site_id='{0}'", site_id);
                SQLiteCommand cmd = new SQLiteCommand(str, cnn);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ind++;
                    }
                }
                if (ind == 0)
                {
                    cmd.CommandText = "INSERT INTO logins VALUES(@site_id, @cookie, @username, @password)";
                    cmd.Parameters.Add(new SQLiteParameter("site_id", site_id));
                    cmd.Parameters.Add(new SQLiteParameter("cookie", cookie));
                    cmd.Parameters.Add(new SQLiteParameter("username", username));
                    cmd.Parameters.Add(new SQLiteParameter("password", password));
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = string.Format("UPDATE logins SET cookie='{0}', username='{1}', password='{2}' WHERE site_id='{3}'", cookie, username, password, site_id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                cnn.Close();
            }
        }

        private string createSite(string site)
        {
            string index = null;
            try
            {
                cnn.Open();

                int i = 0;

                string str = string.Format("SELECT * FROM sites WHERE site='{0}'", site);

                SQLiteCommand cmd = new SQLiteCommand(str, cnn);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        index = reader[0].ToString();
                        i++;
                    }
                }

                if (i == 0)
                {

                    cmd.CommandText = "SELECT COUNT(*) FROM sites";
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        index = reader[0].ToString();
                    }
                    str = string.Format("INSERT INTO sites VALUES(@site_id, @site)");
                    cmd.CommandText = str;
                    cmd.Parameters.Add(new SQLiteParameter("site_id", index));
                    cmd.Parameters.Add(new SQLiteParameter("site", site));
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                cnn.Close();
            }

            return index;
        }

        public bool isPageSource(string source)
        {
            bool result = false;
            lock (locker)
            {
                try
                {
                    cnn.Open();
                    string str = string.Format("SELECT * FROM locations WHERE pagesource='{0}'", source);
                    SQLiteCommand cmd = new SQLiteCommand(str, cnn);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    cnn.Close();
                }

            }
            return result;
        }

        public void createFile(string site, string source, string pagesource, string creator, string name, byte[] file, bool scrap = false)
        {
            try
            {
                string location;
                string path;


                if (scrap)
                {
                    location = string.Format("./downloads/{0}/{1}/scraps/{2}", site, creator, name);
                    path = string.Format("./downloads/{0}/{1}/scraps/", site, creator);
                }
                else
                {
                    location = string.Format("./downloads/{0}/{1}/gallery/{2}", site, creator, name);
                    path = string.Format("./downloads/{0}/{1}/gallery/", site, creator);
                }

                Directory.CreateDirectory(path);
                File.WriteAllBytes(location, file);

                lock (locker)
                {
                    try
                    {
                        cnn.Open();

                        SQLiteCommand cmd = new SQLiteCommand("INSERT INTO locations VALUES(@source, @pagesource, @path)", cnn);
                        cmd.Parameters.Add(new SQLiteParameter("@source", source));
                        cmd.Parameters.Add(new SQLiteParameter("@pagesource", pagesource));
                        cmd.Parameters.Add(new SQLiteParameter("@path", location));
                        cmd.ExecuteNonQuery();


                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);

                    }
                    finally
                    {
                        cnn.Close();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Artist: " + creator);
                Console.WriteLine("Page: " + pagesource);
                Console.WriteLine("Source: " + source);
            }
        }

        public bool isWatchList(string site)
        {
            string site_id = createSite(site);
            bool result = false;

            try
            {
                cnn.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM watchlist WHERE site_id = @id", cnn);
                cmd.Parameters.AddWithValue("id", site_id);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        result = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                cnn.Close();
            }
            return result;
        }

        public watchList getWatchList(string site, string username)
        {
            List<watchedUser> userList = new List<watchedUser>();
            string site_id = createSite(site);
            watchList WL = new watchList();
            WL.site = site;
            WL.watchlistUser = username;
            try
            {
                cnn.Open();
                string str = string.Format("SELECT author, done FROM watchlist WHERE site_id = '{0}' AND watchlist_user = '{1}'", site_id, username);
                SQLiteCommand cmd = new SQLiteCommand(str, cnn);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        watchedUser usr = new watchedUser();

                        usr.user = reader[0].ToString().TrimEnd(' ');
                        string done = reader[1].ToString();

                        if(done.ToLower() == "true")
                        {
                            usr.done = true;
                        }
                        else
                        {
                            usr.done = false;
                        }

                        userList.Add(usr);
                        //string user = reader[0].ToString();
                        //user = user.TrimEnd(' ');
                        //watchlist.Add(user);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                cnn.Close();
            }

            WL.users = userList;
            return WL;
        }

        public void setWatchList(watchList users, bool setDone=true)
        {
            string site_id = createSite(users.site);
            string watchListUser = users.watchlistUser;
            try
            {
                cnn.Open();
                string str = string.Format("DELETE FROM watchlist WHERE site_id='{0}' AND watchlist_user='{1}'", site_id, users.watchlistUser);
                SQLiteCommand cmd = new SQLiteCommand(str, cnn);
                cmd.ExecuteNonQuery();
                foreach (watchedUser user in users.users)
                {
                    cmd = new SQLiteCommand("INSERT INTO watchlist(site_id, author, watchlist_user, done) values(@site_id, @author, @watchlist_user, @done)", cnn);
                    cmd.Parameters.AddWithValue("@site_id", site_id);
                    cmd.Parameters.AddWithValue("@author", user.user);
                    cmd.Parameters.AddWithValue("@watchlist_user", watchListUser);
                    cmd.Parameters.AddWithValue("@done", setDone ? false : user.done);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                cnn.Close();
            }

        }

        public void updateWatchList(string site, string user, string watchListUser, bool done = true)
        {
            string site_id = createSite(site);
            string sDone;
            if (done)
                sDone = "1";
            else
                sDone = "0";

            try
            {
                cnn.Open();
                string str = string.Format("UPDATE watchlist SET done='{2}' WHERE site_id='{0}' AND author='{1}' AND watchlist_user='{3}'", site_id, user, sDone, watchListUser);
                SQLiteCommand cmd = new SQLiteCommand(str, cnn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void nukeWatchList(string site)
        {

            string site_id = createSite(site);
            try
            {
                cnn.Open();
                string str = string.Format("DELETE FROM watchlist WHERE site_id='{0}'", site_id);
                SQLiteCommand cmd = new SQLiteCommand(str, cnn);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                cnn.Close();
            }
        }

    }
}
