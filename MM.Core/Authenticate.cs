using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices.Protocols;
using System.Net;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.Configuration;

namespace MM.Core
{
    public class UserAuthenticate
    {
        public static bool AuthUser(string Domain, string UserName, string Password)
        {
          
            string path = Convert.ToString(ConfigurationManager.AppSettings["ADSPath"]);
            string AdValue = Convert.ToString(ConfigurationManager.AppSettings["AdValue"]);
            switch (AdValue)
            {
                case "1":
                    try
                    {
                        LdapConnection connect = new LdapConnection(Domain);
                        NetworkCredential credent = new NetworkCredential(UserName, Password);
                        connect.Credential = credent;
                        connect.Bind();
                        return true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        //LogError objEr = new LogError();
                        //objEr.HandleException(ex);
                        return false;
                        break;
                    }

                case "2":
                    try
                    {
                        PrincipalContext adContext = new PrincipalContext(ContextType.Domain);
                        // find a user
                        UserPrincipal user = UserPrincipal.FindByIdentity(adContext, UserName);
                        using (adContext)
                        {
                            bool t = adContext.ValidateCredentials(UserName, Password);
                            return t;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        //LogError objEr = new LogError();
                        //objEr.HandleException(ex);
                        return false;
                        break;
                    }

                case "3":
                    string domainaAndUsername = Domain + @"\" + UserName;
                    DirectoryEntry entrys = new DirectoryEntry(Domain, domainaAndUsername, Password);
                    try
                    {
                        object obj = entrys.NativeObject;
                        DirectorySearcher search = new DirectorySearcher(entrys);
                        search.Filter = "(SAMAccountName=" + UserName + ")";
                        search.PropertiesToLoad.Add("cn");
                        SearchResult result = search.FindOne();
                        if (null == result)
                        {
                            return false;
                            break;
                        }
                        else
                        {
                            return true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        //LogError objEr = new LogError();
                        //objEr.HandleException(ex);
                        return false;
                        break;
                    }

                case "4":
                    try
                    {
                        string domainAndUsername = Domain + @"\" + UserName;
                        DirectoryEntry entry = new DirectoryEntry(path, domainAndUsername, Password);
                        DirectorySearcher search = new DirectorySearcher(entry);
                        search.Filter = "(SAMAccountName=" + UserName + ")";
                        search.PropertiesToLoad.Add("cn");
                        SearchResult result = search.FindOne();
                        if (result != null)
                        {
                            path = result.Path;
                            string filterAttribute = (string)result.Properties["cn"][0];
                            return true;
                            break;
                        }
                        else
                        { return false; break; }

                    }
                    catch (Exception ex)
                    {
                        //LogError objEr = new LogError();
                        //objEr.HandleException(ex);
                        return false; break;
                    }
            }
            return false;
        }

        public static bool CheckExists(string Domain, string UserName)
        {
            try
            {
                PrincipalContext adContext = new PrincipalContext(ContextType.Domain);

                // find a user
                UserPrincipal user = UserPrincipal.FindByIdentity(adContext, UserName);

                if (user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsNTAuthenticated(string path, string domain, string userName, string password)
        {
            try
            {
                string domainAndUsername = domain + @"\" + userName;
                DirectoryEntry entry = new DirectoryEntry(path, domainAndUsername, password);
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + userName + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    path = result.Path;
                    string filterAttribute = (string)result.Properties["cn"][0];
                    return true;
                }
            }
            catch(Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
                return false;
            }
            return false;
        }
    }
}