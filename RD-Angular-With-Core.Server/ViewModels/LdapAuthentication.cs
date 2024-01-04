using Microsoft.OpenApi.Services;
using System.DirectoryServices;
using System.Reflection.PortableExecutable;
using System.Text;

namespace RD_Angular_Core.Server.ViewModels
{
    public class LdapAuthentication
    {
        private string _path;
        private string _filterAttribute;

        public LdapAuthentication(string path)
        {
            _path = path;
        }

        public bool IsAuthenticated(string domain, string username, string pwd)
        {
            string domainAndUsername = domain + @"\" + username;
            System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry(_path, domainAndUsername, pwd);

            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                System.DirectoryServices.SearchResult result = search.FindOne();

                if (null == result)
                {
                    return false;
                }

                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (string)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
            }

            return true;
        }
        public bool IsAuthenticated(string username, string pwd)
        {
            string path = "LDAP://172.29.29.188/CN=users,DC=esupport,DC=net";
            string domainAndUsername = @"esupport\" + username;

            System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry(path, domainAndUsername, pwd);

            try
            {

                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                System.DirectoryServices.SearchResult result = search.FindOne();
                _filterAttribute = (string)result.Properties["cn"][0];
                return true;
            }



            catch
            {
                return false;
            }

        }

        public string GetGroups(string domainName, string userName, string password)
        {

            String domainAndUsername = domainName + @"\" + userName;
            System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry(_path, domainAndUsername, password);
            StringBuilder groupNames = new StringBuilder();
            try
            {    //Bind to the native AdsObject to force authentication.            

                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + userName + ")";
                search.PropertiesToLoad.Add("memberOf");
                System.DirectoryServices.SearchResult result = search.FindOne();
                int propertyCount = result.Properties["memberOf"].Count;
                String dn;
                int equalsIndex, commaIndex;
                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (String)result.Properties["memberOf"][propertyCounter];
                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return null;

                    }
                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
            }
            return groupNames.ToString();

        }

    }

}
