using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JdSoft.Apple.Apns.Notifications;
using System.Configuration;
using System.Web;
using System.IO;

namespace APNS
{
    public class PushNotification
    {
        private string deviceToken = "";
        public string DeviceToken
        {
            get { return deviceToken; }
            set { deviceToken = value; }
        }

        private bool isSandBox = true;
        public bool IsSandBox
        {
            get { return isSandBox; }
            set { isSandBox = value; }
        }

        private IList<string> receiverList = null;
        public IList<string> ReceiverList
        {
            get { return receiverList; }
            set { receiverList = value; }
        }
        private string p12FilePath = "";
        public string P12FilePath
        {
            get { return p12FilePath; }
            set { p12FilePath = value; }
        }

        private string p12FilePassword = "";
        public string P12FilePassword
        {
            get { return p12FilePassword; }
            set { p12FilePassword = value; }
        }
        private int noOfConnections = 1;
        public int NoOfConnections
        {
            get { return noOfConnections; }
            set { noOfConnections = value; }
        }

        private int badge = 1;
        public int Badge
        {
            get { return badge; }
            set { badge = value; }
        }

        private string message = "";
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        //private enum badge;
        /*public enum Badges
        {

        }*/

        public PushNotification()
        {
        }

        public PushNotification(string deviceToken, string p12FilePath, string p12FilePassword, bool isSandBox = true)
        {
            this.deviceToken = deviceToken;
            this.isSandBox = isSandBox;
            this.p12FilePath = p12FilePath;
            this.p12FilePassword = p12FilePassword;
        }

        public bool Notify()
        {
            NotificationService notificationService = null;
            bool success = false;
            if (this.p12FilePath.Trim() != "")
            {
                try
                {
                    notificationService = new NotificationService(this.isSandBox, this.p12FilePath, this.p12FilePassword, this.noOfConnections);
                    notificationService.SendRetries = 1;
                    notificationService.ReconnectDelay = 5000;

                    notificationService.Error += new NotificationService.OnError(service_Error);
                    notificationService.NotificationTooLong += new NotificationService.OnNotificationTooLong(service_NotificationTooLong);

                    notificationService.BadDeviceToken += new NotificationService.OnBadDeviceToken(service_BadDeviceToken);
                    notificationService.NotificationFailed += new NotificationService.OnNotificationFailed(service_NotificationFailed);
                    notificationService.NotificationSuccess += new NotificationService.OnNotificationSuccess(service_NotificationSuccess);
                    notificationService.Connecting += new NotificationService.OnConnecting(service_Connecting);
                    notificationService.Connected += new NotificationService.OnConnected(service_Connected);
                    notificationService.Disconnected += new NotificationService.OnDisconnected(service_Disconnected);
                    foreach (string _deviceToken in ReceiverList)
                    {
                        Notification notification = new Notification(_deviceToken);

                        notification.Payload.Alert.Body = string.Format(message);
                        notification.Payload.Sound = "default";
                        notification.Payload.Badge = this.badge;

                        //Queue the notification to be sent
                        success = notificationService.QueueNotification(notification);
                    }
                   
                    //success = true;
                }
                catch (Exception ex)
                {
                    LogError objEr = new LogError();
                    objEr.HandleException(ex);
                    // throw ex;
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
                finally
                {
                    //if (notificationService != null)
                    //{
                    //    notificationService.Close();
                    //    //notificationService.Dispose();
                    //}
                }
            }
            return success;
        }

        public bool ReturnNotify(IList<PushNotification> pushList)
        {
            NotificationService notificationService = null;
            bool success = false;
            if (this.p12FilePath.Trim() != "")
            {
                try
                {
                    for (int i = 0; i < pushList.Count; i++)
                    {
                        notificationService = new NotificationService(pushList[i].isSandBox, pushList[i].p12FilePath, pushList[i].p12FilePassword, pushList[i].noOfConnections);
                        notificationService.SendRetries = 1;
                        notificationService.ReconnectDelay = 5000;

                        notificationService.Error += new NotificationService.OnError(service_Error);
                        notificationService.NotificationTooLong += new NotificationService.OnNotificationTooLong(service_NotificationTooLong);

                        notificationService.BadDeviceToken += new NotificationService.OnBadDeviceToken(service_BadDeviceToken);
                        notificationService.NotificationFailed += new NotificationService.OnNotificationFailed(service_NotificationFailed);
                        notificationService.NotificationSuccess += new NotificationService.OnNotificationSuccess(service_NotificationSuccess);
                        notificationService.Connecting += new NotificationService.OnConnecting(service_Connecting);
                        notificationService.Connected += new NotificationService.OnConnected(service_Connected);
                        notificationService.Disconnected += new NotificationService.OnDisconnected(service_Disconnected);

                        Notification notification = new Notification(pushList[i].DeviceToken);

                        notification.Payload.Alert.Body = string.Format(message);
                        notification.Payload.Sound = "default";
                        notification.Payload.Badge = this.badge;

                        //Queue the notification to be sent
                        notificationService.QueueNotification(notification);
                    }
                    success = true;
                }
                catch (Exception ex)
                {
                    LogError objEr = new LogError();
                    objEr.HandleException(ex);
                    // throw ex;
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    
                }
                finally
                {
                    if (notificationService != null)
                    {
                        //notificationService.Close();
                        //notificationService.Dispose();
                        //notificationService = null;
                    }
                }
            }
            return success;
        }

        void service_BadDeviceToken(object sender, BadDeviceTokenException ex)
        {

            Console.WriteLine("Bad Device Token: {0}", ex.Message);

            LogError objEr = new LogError();
            objEr.HandleException(ex);
        }

        void service_Disconnected(object sender)
        {
            Console.WriteLine("Disconnected...");
        }

        void service_Connected(object sender)
        {
            Console.WriteLine("Connected...");
        }

        void service_Connecting(object sender)
        {
            Console.WriteLine("Connecting...");
        }

        void service_NotificationTooLong(object sender, NotificationLengthException ex)
        {
            Console.WriteLine(string.Format("Notification Too Long: {0}", ex.Notification.ToString()));

            LogError objEr = new LogError();
            objEr.HandleException(ex);
        }

        static void service_NotificationSuccess(object sender, Notification notification)
        {
            Console.WriteLine(string.Format("Notification Success: {0}", notification.ToString()));
        }

        static void service_NotificationFailed(object sender, Notification notification)
        {
            Console.WriteLine(string.Format("Notification Failed: {0}", notification.ToString()));
            
                LogError objEr = new LogError();
                objEr.WriteToFile("Notification Failed: "+ notification.ToString());
        }

        static void service_Error(object sender, Exception ex)
        {
            Console.WriteLine(string.Format("Error: {0}", ex.Message));

            LogError objEr = new LogError();
            objEr.HandleException(ex);
        }



    }

     public class LogError
    {
       public void WriteToFile(string sText)
        {
            string sPath = Path.Combine(HttpRuntime.AppDomainAppPath, "img/Uploads/error.txt");
            try
            {
                FileStream fs = new FileStream(sPath, FileMode.Append, FileAccess.Write);
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
            if (context != null)
            {
                sb.Append("URL: " + context.Request.Url + Environment.NewLine);
                sb.Append("Form: " + context.Request.Form.ToString() + Environment.NewLine);
                sb.Append("QueryString: " + context.Request.QueryString.ToString() + Environment.NewLine);
                sb.Append("Server Name: " + context.Request.ServerVariables["SERVER_NAME"] + Environment.NewLine);
                sb.Append("User Agent: " + context.Request.UserAgent + Environment.NewLine);
                sb.Append("User IP: " + context.Request.UserHostAddress + Environment.NewLine);
                sb.Append("User Host Name: " + context.Request.UserHostName + Environment.NewLine);
                sb.Append("User is Authenticated: " + context.User.Identity.IsAuthenticated.ToString() + Environment.NewLine);
                sb.Append("User Name: " + context.User.Identity.Name + Environment.NewLine);
            }
            else
            {
                sb.Append("Time of Error: " + DateTime.Now.ToString("g") + Environment.NewLine);
            }

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
