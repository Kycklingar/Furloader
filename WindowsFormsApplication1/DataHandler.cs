using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;

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
        static string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\FLDB.mdf;Integrated Security=True";
        SqlConnection cnn = new SqlConnection(connectionString);
        private static int version = 5;
        private static readonly object locker = new object();
        private static string "Test";


        public DataHandler()
        {
        }


        public int bootDB()
        {
            if (isFirstLaunch())
            {
                MessageBox.Show("This program stores usernames and passwords in plain text.\nUse on your own risk", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                initDB();
            }

            int dbVersion = versionInfo();

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
                    SqlCommand cmd = new SqlCommand("UPDATE dbinfo SET version = @ver", cnn);
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

        // TODO: Update from sql files
        private int updateDB(int toDBver)
        {
            int result = 0;
            try
            {
                cnn.Open();
                switch (toDBver)
                {
                    case 2:
                        {
                            SqlCommand cmd = new SqlCommand("CREATE TABLE watchlist (site_id INT, author CHAR(64))", cnn);
                            cmd.ExecuteNonQuery();
                            result = 2;
                        }
                        break;
                    case 3:
                        {
                            SqlCommand cmd = new SqlCommand("TRUNCATE TABLE watchlist", cnn);
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "ALTER TABLE watchlist ADD done BIT DEFAULT 0, watchlist_user VARCHAR(64)";
                            cmd.ExecuteNonQuery();
                            result = 3;
                        }
                        break;
                    case 4:
                        {
                            string str = "ALTER TABLE locations ALTER COLUMN source VARCHAR(255) NOT NULL; ";
                            str += "ALTER TABLE locations ALTER COLUMN pagesource VARCHAR(255) NOT NULL; ";
                            str += "ALTER TABLE locations ALTER COLUMN path VARCHAR(255) NOT NULL;";
                            SqlCommand cmd = new SqlCommand(str, cnn);
                            cmd.CommandTimeout = 0;
                            cmd.ExecuteNonQuery();

                            removeWhitespace("locations", "source");
                            removeWhitespace("locations", "pagesource");
                            removeWhitespace("locations", "path");

                            str = "ALTER TABLE logins ALTER COLUMN cookie VARCHAR(255); ";
                            str += "ALTER TABLE logins ALTER COLUMN username VARCHAR(255); ";
                            str += "ALTER TABLE logins ALTER COLUMN password VARCHAR(255);";
                            cmd.CommandText = str;
                            cmd.ExecuteNonQuery();

                            removeWhitespace("logins", "cookie");
                            removeWhitespace("logins", "username");
                            removeWhitespace("logins", "password");


                            str = "ALTER TABLE sites ALTER COLUMN site VARCHAR(255) NOT NULL";
                            cmd.CommandText = str;
                            cmd.ExecuteNonQuery();

                            removeWhitespace("sites", "site");


                            str = "ALTER TABLE watchlist ALTER COLUMN author VARCHAR(64)";
                            cmd.CommandText = str;
                            cmd.ExecuteNonQuery();

                            removeWhitespace("watchlist", "author");

                            result = 4;
                        }
                        break;
                    case 5:
                        {
                            SqlCommand cmd = new SqlCommand("CREATE INDEX index_location ON locations(pagesource);", cnn);
                            cmd.ExecuteNonQuery();
                            result = 5;
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
                SqlCommand cmd = new SqlCommand(str, cnn);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void initDB()
        {
            // This is horrible!!

            /*  DB layout V2:
             *  TABLE       | COLUMN
             *  ------------|-----------------------------------------------
             *  dbinfo      |INT    |
             *              |version|
             * -------------|-----------------------------------------------
             *  logins      |INT    |VARCHAR|CHAR       |CHAR    |
             *              |site_id|cookie |username   |password|
             *  ------------|-----------------------------------------------
             *  locations   |INT    |CHAR   |CHAR       |CHAR  |
             *              |hash_id|source |pagesource |path  |
             *  ------------|-----------------------------------------------
             *  hashes      |INT    |BINARY(128)| //MD5 hash
             *              |hash_id|hash       |
             *  ------------|-----------------------------------------------
             *  sites       |INT    |CHAR   |
             *              |site_id|site   |
             *  ------------------------------------------------------------
             *  watchlist   |INT    |CHAR   |
             *              |site_id|author   |
             *              
             */
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("CREATE TABLE dbinfo (version INT)", cnn);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE logins (site_id INT, cookie CHAR(255), username CHAR(255), password CHAR(255))";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE locations (hash_id INT, source CHAR(255), pagesource CHAR(255), path CHAR(255))";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE hashes (hash_id INT, hash BINARY(128))";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE sites (site_id INT, site CHAR(255))";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE watchlist (site_id INT, author CHAR(64), watchlist_user VARCHAR(64), done BIT DEFAULT 0)";
                cmd.ExecuteNonQuery();


                cmd.CommandText = "INSERT INTO dbinfo VALUES(@version)";
                cmd.Parameters.Add(new SqlParameter("version", version));
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e);
            }
            finally
            {
                cnn.Close();
            }

        }

        private int versionInfo()
        {
            int database_version = 0;
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SELECT version FROM dbinfo", cnn);
                using (SqlDataReader reader = cmd.ExecuteReader())
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
            cnn.Close();
            return database_version;
        }

        public loginCookies getLogin(string site)
        {
            loginCookies login = new loginCookies();
            try
            {
                cnn.Open();
                string str = string.Format("SELECT site_id FROM sites WHERE site='{0}'", site);
                SqlCommand cmd = new SqlCommand(str, cnn);
                string site_id = null;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        site_id = reader[0].ToString();
                    }
                }
                cmd.CommandText = string.Format("SELECT * FROM logins WHERE site_id='{0}'", site_id);
                using (SqlDataReader reader = cmd.ExecuteReader())
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
                SqlCommand cmd = new SqlCommand(str, cnn);
                string site_id = null;
                using (SqlDataReader reader = cmd.ExecuteReader())
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
                SqlCommand cmd = new SqlCommand(str, cnn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ind++;
                    }
                }
                if (ind == 0)
                {
                    cmd.CommandText = "INSERT INTO logins VALUES(@site_id, @cookie, @username, @password)";
                    cmd.Parameters.Add(new SqlParameter("site_id", site_id));
                    cmd.Parameters.Add(new SqlParameter("cookie", cookie));
                    cmd.Parameters.Add(new SqlParameter("username", username));
                    cmd.Parameters.Add(new SqlParameter("password", password));
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

                SqlCommand cmd = new SqlCommand(str, cnn);

                using (SqlDataReader reader = cmd.ExecuteReader())
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
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        index = reader[0].ToString();
                    }
                    str = string.Format("INSERT INTO sites VALUES(@site_id, @site)");
                    cmd.CommandText = str;
                    cmd.Parameters.Add(new SqlParameter("site_id", index));
                    cmd.Parameters.Add(new SqlParameter("site", site));
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

        private bool isFirstLaunch()
        {
            cnn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'dbinfo'", cnn);
                int i = 0;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    i++;

                reader.Close();
                cnn.Close();
                if (i == 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error " + e);
                cnn.Close();
                return false;
            }
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
                    SqlCommand cmd = new SqlCommand(str, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
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

                        SqlCommand cmd = new SqlCommand("INSERT INTO locations VALUES(@hash_id, @source, @pagesource, @path)", cnn);
                        cmd.Parameters.Add(new SqlParameter("@hash_id", "0"));
                        cmd.Parameters.Add(new SqlParameter("@source", source));
                        cmd.Parameters.Add(new SqlParameter("@pagesource", pagesource));
                        cmd.Parameters.Add(new SqlParameter("@path", location));
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM watchlist WHERE site_id = @id", cnn);
                cmd.Parameters.AddWithValue("id", site_id);
                using (SqlDataReader reader = cmd.ExecuteReader())
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
                SqlCommand cmd = new SqlCommand(str, cnn);
                using (SqlDataReader reader = cmd.ExecuteReader())
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

        // Set the watchlist in the db. Set the done for the specific user if setDone == false
        public void setWatchList(watchList users, bool setDone=true)
        {
            string site_id = createSite(users.site);
            string watchListUser = users.watchlistUser;
            try
            {
                cnn.Open();
                string str = string.Format("DELETE FROM watchlist WHERE site_id='{0}' AND watchlist_user='{1}'", site_id, users.watchlistUser);
                SqlCommand cmd = new SqlCommand(str, cnn);
                cmd.ExecuteNonQuery();
                foreach (watchedUser user in users.users)
                {
                    cmd = new SqlCommand("INSERT INTO watchlist(site_id, author, watchlist_user, done) values(@site_id, @author, @watchlist_user, @done)", cnn);
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
                SqlCommand cmd = new SqlCommand(str, cnn);
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
                SqlCommand cmd = new SqlCommand(str, cnn);
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
