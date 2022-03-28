using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using System.Net;

namespace MeetingMinder.Web
{
    public class Global : System.Web.HttpApplication, IRequiresSessionState
    {
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");

        }
                
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            this.Response.Headers["X-Content-Type-Options"] = "nosniff"; //MIME Sniffing

            Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate, pre-check=0, post-check=0, max-age=0, s-maxage=0"); // HTTP 1.1.
            Response.AppendHeader("Expires", "0"); // Proxies.
            Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.

            HttpContext.Current.Response.AddHeader("x-frame-options", "DENY");

            HttpContext context = HttpContext.Current;
            string path = context.Request.Path;


            //if (path == "images/uploads" || path == "images/uploads/" || path == "/images/uploads" || path == "/images/uploads/")
            //{
            //    Response.StatusCode = 302;
            //    Response.Redirect("Error.aspx?ErrorCode=404");
            //}

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // int lastExtension = path.ToLower().LastIndexOf("login1");

            //if (path.ToLower().IndexOf("login.aspx") > -1)
            //{
            //    HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = "/emeeting";
            //    HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Secure = true;
            //    HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Domain = "/jmjgreen";
            //    Response.Cookies["ASP.NET_SessionId"].Value = Guid.NewGuid().ToString();
            //}

            //if (path.ToLower().IndexOf("login.aspx") > -1)
            //{
            //    HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = "meetinguat;SameSite=Strict";
            //    HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Secure = true;
            //    HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Domain = "meetinguat.sbi.co.in";
            //    Response.Cookies["ASP.NET_SessionId"].Value = Guid.NewGuid().ToString();
            //}

            if (path.ToLower().IndexOf("log1.aspx") > -1)
            {
                HttpContext.Current.RewritePath("default.aspx");
            }

            if (path.ToLower().IndexOf("emeetings_igendapdf") > -1 || path.ToLower().IndexOf("emeeting/meetings_agenda") > -1)
            {
                try
                {
                    string[] AgendaNames = path.Split('-');
                    if (AgendaNames.Count() >= 2)
                    {
                        string[] subPaths = path.Split('/');

                        string strPath = subPaths[subPaths.Count() - 1].Substring(20);

                        string strPathNew = strPath.Substring(0, strPath.Length - 4);

                        //HttpContext.Current.Server.Transfer("~/NoticeMaster.aspx?igendapdf=" + HttpUtility.UrlEncode(strPathNew))
                        HttpContext.Current.RewritePath("~/NoticeMaster.aspx?igendapdf=" + HttpUtility.UrlEncode(strPathNew))
                               //HttpUtility.UrlEncode(AgendaNames[1].Substring(0, AgendaNames[1].Length - 5)))
                               ;//+ HttpUtility.UrlEncode("sd9I0YwL0GBCE+DxeNhMeJiOqiaEppUe"));
                    }
                    else
                    {
                        //HttpContext.Current.Server.Transfer("~/NoticeMaster.aspx?agenda1=11");
                        HttpContext.Current.RewritePath("~/NoticeMaster.aspx?agenda1=11");

                    }
                }
                catch (Exception ex)
                {
                    LogError objEr = new LogError();
                    objEr.HandleException(ex);
                }

            }

            if (path.ToLower().IndexOf("meetings_agenda") > -1 || path.ToLower().IndexOf("emeeting/meetings_agenda") > -1)
            {
                try
                {
                    string[] AgendaNames = path.Split('-');
                    if (AgendaNames.Count() >= 2)
                    {
                        string[] subPaths = path.Split('/');

                        string strPath = subPaths[subPaths.Count() - 1].Substring(16);

                        string strPathNew = strPath.Substring(0, strPath.Length - 4);

                        //HttpContext.Current.Server.Transfer("~/NoticeMaster.aspx?agenda=" + HttpUtility.UrlEncode(strPathNew))
                        HttpContext.Current.RewritePath("~/NoticeMaster.aspx?agenda=" + HttpUtility.UrlEncode(strPathNew));

                        //HttpUtility.UrlEncode(AgendaNames[1].Substring(0, AgendaNames[1].Length - 5)))
                        ;//+ HttpUtility.UrlEncode("sd9I0YwL0GBCE+DxeNhMeJiOqiaEppUe"));
                    }
                    else
                    {
                        //HttpContext.Current.Server.Transfer("~/NoticeMaster.aspx?agenda1=11");
                        HttpContext.Current.RewritePath("~/NoticeMaster.aspx?agenda1=11");

                    }
                }
                catch (Exception ex)
                {
                    //((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                    //Error.Visible = true;

                    LogError objEr = new LogError();
                    objEr.HandleException(ex);
                }

            }

            if (path.ToLower().IndexOf("meeting_agenda") > -1 || path.ToLower().IndexOf("emeeting/meeting_agenda") > -1)
            {
                try
                {


                    string[] AgendaNames = path.Split('-');
                    if (AgendaNames.Count() >= 2)
                    {

                        string[] subPaths = path.Split('/');

                        string strPath = subPaths[subPaths.Count() - 1].Substring(15);


                        string strPathNew = strPath.Substring(0, strPath.Length - 5);


                        //HttpContext.Current.Server.Transfer("~/NoticeMaster.aspx?agenda=" + HttpUtility.UrlEncode(strPathNew))
                        HttpContext.Current.RewritePath("~/NoticeMaster.aspx?agenda=" + HttpUtility.UrlEncode(strPathNew));
                        //HttpUtility.UrlEncode(AgendaNames[1].Substring(0, AgendaNames[1].Length - 5)))
                        ;//+ HttpUtility.UrlEncode("sd9I0YwL0GBCE+DxeNhMeJiOqiaEppUe"));
                    }
                    else
                    {
                        // HttpContext.Current.Server.Transfer("~/NoticeMaster.aspx?agenda1=11");
                        HttpContext.Current.RewritePath("~/NoticeMaster.aspx?agenda1=11");
                    }
                }
                catch (Exception ex)
                {
                    //((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                    //Error.Visible = true;

                    LogError objEr = new LogError();
                    objEr.HandleException(ex);
                }
            }


        }


        void Application_EndRequest(object sender, EventArgs e)
        {


        }

        void Application_Start(object sender, EventArgs e)
        {
            Application["UsersLoggedIn"] = new System.Collections.Generic.Dictionary<string, string>();

            //// Code that runs on application startup

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            //string mess = "Error in Path :" + Request.Path; //Get the path of the page
            //mess += "\n\n Error Raw Url :" + Request.RawUrl; //Get the QueryString along with the Virtual Path

            ////Create an Exception object from the Last error that occurred on the server
            //Exception myError = Server.GetLastError();

            //mess += "\n\n Error Message :" + myError.Message; //Get the error message
            //mess += "\n\n Error Source :" + myError.Source;  //Source of the message
            //mess += "\n\n Error Stack Trace :" + myError.StackTrace; //Stack Trace of the error
            //mess += "\n\n Error TargetSite :" + myError.TargetSite; //Method where the error occurred

            // Code that runs when an unhandled error occurs
            HttpContext.Current.Response.StatusCode = 404;
            HttpContext.Current.RewritePath("Error.aspx?ErrorCode=500");
            //  Response.Write(mess);

            //LogError objEr = new LogError();
            //objEr.HandleException(myError);
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started      

        }

        void Session_End(object sender, EventArgs e)
        {
            string userLoggedIn = Session["UserId"] == null ? string.Empty : (string)Session["UserId"];
            if (userLoggedIn.Length > 0)
            {
                System.Collections.Generic.Dictionary<string, string> d = Application["UsersLoggedIn"]
                    as System.Collections.Generic.Dictionary<string, string>;
                if (d != null)
                {
                    lock (d)
                    {
                        d.Remove(userLoggedIn);
                        Application["UsersLoggedIn"] = d;
                    }
                }
            }
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

        private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine(certificate);

            return true;
        }

    }
}
