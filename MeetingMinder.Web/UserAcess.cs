using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using MM.Domain;
using MM.Data;
using System.IO;
using MM.Core;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace MeetingMinder.Web
{
    public class UserAcess
    {
        public bool IsEdit(Guid EntityId)
        {
            IList<AccessRightDomain> objAcess;
            if (HttpContext.Current.Session["AccessRight"] != null)
            {
                objAcess = (IList<AccessRightDomain>)(HttpContext.Current.Session["AccessRight"]);
            }
            else
            {
                objAcess = AccessRightDataProvider.Instance.GetAccessRightByUserId(Guid.Parse(HttpContext.Current.Session["UserId"].ToString()));
            }
            Guid UserId = Guid.Parse(HttpContext.Current.Session["UserId"].ToString());
            //Guid EntityId = Guid.Parse(HttpContext.Current.Session["EntityId"].ToString());

            bool IsMaker = bool.Parse(HttpContext.Current.Session["IsMaker"].ToString());
            var objIsallowed = from Access in objAcess where (Access.EntityId == EntityId && Access.UserId == UserId) select Access;
            if (objIsallowed.Count() > 0 && IsMaker)
            {
                var isEdit = (from rights in objIsallowed
                              select (new
                              {
                                  allowed = rights.IsUpdate
                              })).ToList();
                return (bool)isEdit[0].allowed;
            }
            else
            {
                return false;
            }


        }


        public bool IsAdd(Guid EntityId)
        {
            IList<AccessRightDomain> objAcess;
            if (HttpContext.Current.Session["AccessRight"] != null)
            {
                objAcess = (IList<AccessRightDomain>)(HttpContext.Current.Session["AccessRight"]);
            }
            else
            {
                objAcess = AccessRightDataProvider.Instance.GetAccessRightByUserId(Guid.Parse(HttpContext.Current.Session["UserId"].ToString()));
            }
            Guid UserId = Guid.Parse(HttpContext.Current.Session["UserId"].ToString());
            //Guid EntityId = Guid.Parse(HttpContext.Current.Session["EntityId"].ToString());
            bool IsMaker = bool.Parse(HttpContext.Current.Session["IsMaker"].ToString());

            var objIsallowed = from Access in objAcess
                               where (Access.EntityId == EntityId && Access.UserId == UserId)
                               select Access;
            if (objIsallowed.Count() > 0 && IsMaker)
            {
                var isAdd = (from rights in objIsallowed
                             select (new
                             {
                                 allowed = rights.IsAdd
                             })).ToList();
                return (bool)isAdd[0].allowed;
            }
            else
            {
                return false;
            }


        }


        public bool isDelete(Guid EntityId)
        {
            IList<AccessRightDomain> objAcess;
            if (HttpContext.Current.Session["AccessRight"] != null)
            {
                objAcess = (IList<AccessRightDomain>)(HttpContext.Current.Session["AccessRight"]);
            }
            else
            {
                objAcess = AccessRightDataProvider.Instance.GetAccessRightByUserId(Guid.Parse(HttpContext.Current.Session["UserId"].ToString()));
            }
            Guid UserId = Guid.Parse(HttpContext.Current.Session["UserId"].ToString());
            //Guid EntityId = Guid.Parse(HttpContext.Current.Session["EntityId"].ToString());
            bool IsMaker = bool.Parse(HttpContext.Current.Session["IsMaker"].ToString());

            var objIsallowed = from Access in objAcess
                               where (Access.EntityId == EntityId && Access.UserId == UserId)
                               select Access;
            if (objIsallowed.Count() > 0 && IsMaker)
            {
                var isDelete = (from rights in objIsallowed
                                select (new
                                {
                                    allowed = rights.IsDelete
                                })).ToList();
                return (bool)isDelete[0].allowed;
            }
            else
            {
                return false;
            }


        }
       
        public bool isValidChar(string data)
        {
            bool returnVal = true;
            string iChars = "<<()&;‘\"/\\*;:={}`%+^!-";

            //string reg = "/\\x(0|1)[1|0-9]|(\\x20)/";
            //Regex regex = new Regex(reg,RegexOptions.IgnoreCase);
            //Match match = regex.Match(data);
            //if (!match.Success)
            //{
            //    returnVal = false;
            //}
            string strRegex = @"\\x(0|1)[1|0-9]|(\\x20)";
            Regex myRegex = new Regex(strRegex, RegexOptions.None);

            data = HttpUtility.HtmlEncode(data);
            foreach (Match myMatch in myRegex.Matches(data))
            {
                if (myMatch.Success)
                {
                    returnVal = false;
                }
            }

            for (int i = 0; i < data.Length; i++)
            {

                if (iChars.IndexOf(data[i]) != -1)
                {

                    returnVal = false;
                }

            }

            if(data.StartsWith("=") || data.StartsWith("+") || data.StartsWith("-"))
            {
                return false;
            }
            return true;
        }

        //public bool isValidChar(string data)
        //{
        //    bool returnVal = true;
        //    string iChars = "<<()&;‘\"/\\*;:={}`%+^!-";

        //    //string reg = "/\\x(0|1)[1|0-9]|(\\x20)/";
        //    //Regex regex = new Regex(reg,RegexOptions.IgnoreCase);
        //    //Match match = regex.Match(data);
        //    //if (!match.Success)
        //    //{
        //    //    returnVal = false;
        //    //}
        //    string strRegex = @"\\x(0|1)[1|0-9]|(\\x20)";
        //    Regex myRegex = new Regex(strRegex, RegexOptions.None);


        //    foreach (Match myMatch in myRegex.Matches(data))
        //    {
        //        if (myMatch.Success)
        //        {
        //            returnVal = false;
        //        }
        //    }

        //    for (int i = 0; i < data.Length; i++)
        //    {

        //        if (iChars.IndexOf(data[i]) != -1)
        //        {

        //            returnVal = false;
        //        }

        //    }
        //    return returnVal;
        //}




        public bool isValidChar_old(string data)
        {
            bool returnVal = true;
            string iChars = "<<()&;‘\"/\\*;:={}`%+^!-";

            //string reg = "/\\x(0|1)[1|0-9]|(\\x20)/";
            //Regex regex = new Regex(reg,RegexOptions.IgnoreCase);
            //Match match = regex.Match(data);
            //if (!match.Success)
            //{
            //    returnVal = false;
            //}
            string strRegex = @"\\x(0|1)[1|0-9]|(\\x20)";
            Regex myRegex = new Regex(strRegex, RegexOptions.None);

            
            foreach (Match myMatch in myRegex.Matches(data))
            {
                if (myMatch.Success)
                {
                    returnVal = false;
                }
            }

            for (int i = 0; i < data.Length; i++)
            {

                if (iChars.IndexOf(data[i]) != -1)
                {

                    returnVal = false;
                }

            }
            return true; //returnVal ;
        }

        public void TextHtmlEncode(ref System.Web.UI.WebControls.TextBox txtBox)
        {
            txtBox.Text =  WebUtility.HtmlEncode(txtBox.Text);
        }

        public void TextHtmlEncode(ref string txtBox)
        {
            txtBox = WebUtility.HtmlEncode(txtBox);
        }

        public bool CSVValidation(string data)
        {
            bool returnVal = true;
            var filter = "=,+,-,@,|";
            var strarray = filter.Split(',');
            string dataVal = data.Trim();
            string dd = dataVal.Substring(0, 1);
            foreach (var arr in strarray)
            {
                if (arr == dd)
                {
                    returnVal = false;
                }
            }

            return returnVal;
        }

    }

    public static class MyExtensions
    {
        public static string EncodeHtml(this string txtBox)
        {
            return WebUtility.HtmlEncode(txtBox);
            //System.Uri.EscapeDataString(txtBox);
        }

        public static string DecodeHtml(this string txtBox)
        {
            return WebUtility.HtmlDecode(txtBox);
            //System.Uri.EscapeDataString(txtBox);
        }
    }   
}