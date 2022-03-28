using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MeetingMInder.WCFServices
{
    public class UserManager
    {
        public string UserId { get; set; }
        public DateTime StartTime { get; set; }
        public int SessionTimeOut { get; set; }
        public string AcceeToken { get; set; }

        public Timer timer { get; set; } //will hanlde the expiry
        List<UserManager> refofMainList; //will be used to remove the item once it is expired

        public UserManager()
        {

        }
        public UserManager(string accessToken, int SessionTimeOut, List<UserManager> refOfList)
        {
            refofMainList = refOfList;

            timer = new Timer(TimeSpan.FromMinutes(SessionTimeOut).TotalMilliseconds);
            AcceeToken = accessToken;

            timer.Elapsed += new ElapsedEventHandler(Elapsed_Event);
            timer.Start();
        }

        private void Elapsed_Event(object sender, ElapsedEventArgs e)
        {
            timer.Elapsed -= new ElapsedEventHandler(Elapsed_Event);
            var itemToRemove = refofMainList.SingleOrDefault(r => r.AcceeToken == this.AcceeToken);
            if (itemToRemove != null)
            {
                refofMainList.Remove(itemToRemove);
            }
        }
    }

    public class ExpireItems
    {
        public string AcceeToken { get; set; }

        public Timer timer;

        public void Elapsed_Event(object sender, ElapsedEventArgs es)
        {
            try
            {


                timer.Elapsed -= new ElapsedEventHandler(Elapsed_Event);
                timer.AutoReset = false;
                var itemToRemove = MeetingMInder.WCFServices.SBIService.objUserManager.SingleOrDefault(r => r.AcceeToken == this.AcceeToken);
                if (itemToRemove != null)
                {
                    string strddd = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

                    string sPath = strddd + "img\\Uploads\\errorToken.txt";

                    StringBuilder sb = new StringBuilder();
                    sb.Append("Time of Error: " + DateTime.Now.ToString("g") + Environment.NewLine);
                    sb.Append("\n\n");
                    sb.Append(" AcceeToken " + itemToRemove.AcceeToken + Environment.NewLine);
                    sb.Append("\n\n");
                    sb.Append(" timer interval in milisec " + itemToRemove.timer.Interval + Environment.NewLine);
                    sb.Append("\n\n");
                    sb.Append(" UserId " + itemToRemove.UserId + Environment.NewLine);
                    sb.Append("\n\n");
                    sb.Append(" session Timeout " + itemToRemove.SessionTimeOut + Environment.NewLine);
                    sb.Append("\n\n");
                    sb.Append(" Login request Start time " + itemToRemove.StartTime + Environment.NewLine);
                    sb.Append("\n\n");

                    FileStream fs = new FileStream(sPath, FileMode.Append, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(fs);
                    writer.Write(sb.ToString());
                    writer.Close();
                    fs.Close();

                    MeetingMInder.WCFServices.SBIService.objUserManager.Remove(itemToRemove);
                }
            }
            catch (Exception e)
            {
                string strddd = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

                string sPath = strddd + "img\\Uploads\\errorToken.txt";
                StringBuilder sb = new StringBuilder("Error" + Environment.NewLine);
               
                while (e != null)
                {
                    sb.Append("Message: " + e.Message + Environment.NewLine);
                    sb.Append("Source: " + e.Source + Environment.NewLine);
                    sb.Append("TargetSite: " + e.TargetSite + Environment.NewLine);
                    sb.Append("StackTrace: " + e.StackTrace + Environment.NewLine);
                    sb.Append("InnerException: " + e.InnerException);
                    sb.Append(Environment.NewLine + Environment.NewLine);
                    
                }
                FileStream fs = new FileStream(sPath, FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs);
                writer.Write(sb.ToString());
                writer.Close();
                fs.Close();
            }
        }
    }
}
