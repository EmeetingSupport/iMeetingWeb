using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace MeetingMinder.Web
{
    public class LogError
    {
       public void WriteToFile(string sText)
        {
            string sPath = HttpContext.Current.Server.MapPath("~/img/Uploads/Logs/");
            string file = "Log-" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
            bool exists = System.IO.Directory.Exists(sPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(sPath);
            StringBuilder sb = new StringBuilder("Error" + Environment.NewLine);

            try
            {
                FileStream fs = new FileStream(sPath + file, FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs);
                writer.Write(sText);
                writer.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
        }

        //void LogEvent(string sText)
        //{
        //    try
        //    {
        //        if (!EventLog.SourceExists(LogSettings.EventSource))
        //        {
        //            EventLog.CreateEventSource(LogSettings.EventSource, "Application");
        //        }

        //        EventLog log = new EventLog();
        //        log.Source = LogSettings.EventSource;
        //        log.WriteEntry(sText, EventLogEntryType.Error);
        //    }
        //    catch (Exception)
        //    {
        //    }

        //}

        public void HandleException(Exception e)
        {
            string sExceptionDescription = FormatExceptionDescription(e);

            WriteToFile(sExceptionDescription);
            //if ((NotifyMode & UseFile) == UseFile)
            //{
            //    WriteToFile(sExceptionDescription);
            //}

            //if ((NotifyMode & UseEmail) == UseEmail)
            //{
            //    SendEmail(sExceptionDescription);
            //}

            //if ((NotifyMode & UseEventLog) == UseEventLog)
            //{
            //    LogEvent(sExceptionDescription);
            //}
        }

        protected virtual string FormatExceptionDescription(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            HttpContext context = HttpContext.Current;

            //sb.Append("Time of Error: " + DateTime.Now.ToString("g") + Environment.NewLine);
            sb.Append("Time of Error: " + DateTime.Now.ToString() + Environment.NewLine);
            sb.Append("URL: " + context.Request.Url + Environment.NewLine);
            //sb.Append("Form: " + context.Request.Form.ToString() + Environment.NewLine);
            sb.Append("QueryString: " + context.Request.QueryString.ToString() + Environment.NewLine);
            sb.Append("Server Name: " + context.Request.ServerVariables["SERVER_NAME"] + Environment.NewLine);
            sb.Append("User Agent: " + context.Request.UserAgent + Environment.NewLine);
            sb.Append("User IP: " + context.Request.UserHostAddress + Environment.NewLine);
            sb.Append("User Host Name: " + context.Request.UserHostName + Environment.NewLine);
            //sb.Append("User is Authenticated: " + context.User.Identity.IsAuthenticated.ToString() + Environment.NewLine);
            //sb.Append("User Name: " + context.User.Identity.Name + Environment.NewLine);


            while (e != null)
            {
                sb.Append("Message: " + e.Message + Environment.NewLine);
                sb.Append("Source: " + e.Source + Environment.NewLine);
                sb.Append("TargetSite: " + e.TargetSite + Environment.NewLine);
                sb.Append("StackTrace: " + e.StackTrace + Environment.NewLine);
                sb.Append(Environment.NewLine + Environment.NewLine);

                e = e.InnerException;

            }

            sb.Append("\n\n");
            return sb.ToString();
        }
    }
}