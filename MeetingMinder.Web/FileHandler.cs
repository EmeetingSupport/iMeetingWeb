using MM.Data;
using MM.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace MeetingMinder.Web
{
    public class FileHandler : IHttpHandler, IRequiresSessionState
    {
        public FileHandler()
        { }
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {

            try
            {
                RollDomain objRoll = new RollDomain();
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["Roll"] != null)
                {
                    objRoll = (RollDomain)HttpContext.Current.Session["Roll"];
                }
                else
                {
                    if (HttpContext.Current.Session["UserId"] != null)
                    {
                        objRoll = RollDataProvider.Instance.GetRollByUserId(Guid.Parse(HttpContext.Current.Session["UserId"].ToString()));
                        HttpContext.Current.Session["Roll"] = objRoll;
                    }
                }
                string path = context.Request.Path;

                if (path.ToLower().IndexOf("img/uploads/entitylogo") > -1)
                {
                    if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserId"] != null && objRoll.EntityMaster)
                    {
                        HttpContext.Current.Response.ContentType = "application/octet-stream";
                        HttpContext.Current.Response.WriteFile(path);
                    }
                    else
                    {
                        if (!path.ToLower().EndsWith(".pdf"))
                        {
                            HttpContext.Current.Response.ContentType = "application/octet-stream";
                            HttpContext.Current.Response.WriteFile(path);
                        }
                    }

                    if (HttpContext.Current.Request.Headers["userid"] != null && HttpContext.Current.Request.Headers["AccessToken"] != null)
                    {

                        string userID = HttpContext.Current.Request.Headers["userid"];
                        string AccessToken = HttpContext.Current.Request.Headers["AccessToken"];

                        if (ValidateToken(userID, AccessToken))
                        {
                            //   HttpContext.Current.Response.ContentType = "application/octet-stream";
                            HttpContext.Current.Response.ContentType = "application/octet-stream";
                            HttpContext.Current.Response.WriteFile(path);
                        }
                        else
                        {
                            HttpContext.Current.Response.Headers["TokenExpired"] = "true";
                        }
                    }
                    //else
                    //{
                    //    HttpContext.Current.Response.Redirect("~/default.aspx");
                    //}
                }

                if (path.ToLower().IndexOf("img/uploads/profilepic") > -1)
                {
                    if (HttpContext.Current.Session["UserId"] != null && objRoll.UserMaster)
                    {
                        HttpContext.Current.Response.ContentType = "application/octet-stream";
                        HttpContext.Current.Response.WriteFile(path);
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect("~/default.aspx");
                    }
                }

                if (path.ToLower().IndexOf("img/uploads/photogallery") > -1)
                {
                    if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserId"] != null && objRoll.PhotoGallery)
                    {
                        //   HttpContext.Current.Response.ContentType = "application/octet-stream";
                        HttpContext.Current.Response.WriteFile(path);
                    }
                    else if (HttpContext.Current.Request.Headers["userid"] != null && HttpContext.Current.Request.Headers["AccessToken"] != null)
                    {
                        string userID = HttpContext.Current.Request.Headers["userid"];
                        string AccessToken = HttpContext.Current.Request.Headers["AccessToken"];

                        if (ValidateToken(userID, AccessToken))
                        {
                            HttpContext.Current.Response.ContentType = "application/octet-stream";
                            HttpContext.Current.Response.WriteFile(path);
                        }
                        else
                        {
                            HttpContext.Current.Response.Headers["TokenExpired"] = "true";
                        }
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect("~/default.aspx");
                    }
                }


                if (path.ToLower().IndexOf("img/uploads/forum") > -1)
                {
                    if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserId"] != null && objRoll.PhotoGallery)
                    {
                        //   HttpContext.Current.Response.ContentType = "application/octet-stream";
                        HttpContext.Current.Response.WriteFile(path);
                    }
                    else if (HttpContext.Current.Request.Headers["userid"] != null && HttpContext.Current.Request.Headers["AccessToken"] != null)
                    {
                        string userID = HttpContext.Current.Request.Headers["userid"];
                        string AccessToken = HttpContext.Current.Request.Headers["AccessToken"];

                        if (ValidateToken(userID, AccessToken))
                        {
                            HttpContext.Current.Response.ContentType = "application/octet-stream";
                            HttpContext.Current.Response.WriteFile(path);
                        }
                        else
                        {
                            HttpContext.Current.Response.Headers["TokenExpired"] = "true";
                        }
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect("~/default.aspx");
                    }
                }
                if (path.ToLower().IndexOf("img/uploads/keyinfo") > -1)
                {
                    if (HttpContext.Current.Session["UserId"] != null && objRoll.BoardofDirectore)
                    {
                        HttpContext.Current.Response.ContentType = "application/octet-stream";
                        HttpContext.Current.Response.WriteFile(path);
                    }
                    else if (HttpContext.Current.Request.Headers["userid"] != null && HttpContext.Current.Request.Headers["AccessToken"] != null)
                    {

                        string userID = HttpContext.Current.Request.Headers["userid"];
                        string AccessToken = HttpContext.Current.Request.Headers["AccessToken"];

                        if (ValidateToken(userID, AccessToken))
                        {
                            HttpContext.Current.Response.ContentType = "application/octet-stream";
                            HttpContext.Current.Response.WriteFile(path);
                        }
                        else
                        {
                            HttpContext.Current.Response.Headers["TokenExpired"] = "true";
                        }
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect("~/default.aspx");
                    }
                }

                if (path.ToLower().IndexOf("img/uploads/uploadminutes") > -1)
                {
                    if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserId"] != null && objRoll.UploadMinutes)
                    {
                        HttpContext.Current.Response.ContentType = "application/octet-stream";
                        HttpContext.Current.Response.WriteFile(path);
                    }
                    else if (HttpContext.Current.Request.Headers["userid"] != null && HttpContext.Current.Request.Headers["AccessToken"] != null)
                    {
                        string userID = HttpContext.Current.Request.Headers["userid"];
                        string AccessToken = HttpContext.Current.Request.Headers["AccessToken"];

                        if (ValidateToken(userID, AccessToken))
                        {
                            HttpContext.Current.Response.ContentType = "application/octet-stream";
                            HttpContext.Current.Response.WriteFile(path);
                        }
                        else
                        {
                            HttpContext.Current.Response.Headers["TokenExpired"] = "true";
                        }
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect("~/default.aspx");
                    }
                }

                if (path.ToLower().IndexOf("img/uploads/agenda") > -1)
                {
                    HttpContext.Current.Response.Redirect("~/default.aspx");
                }

                //captcha
                if (path.ToLower().IndexOf("img/uploads/captcha") > -1)
                {
                    MemoryStream memStream = new MemoryStream();
                    string phrase = Convert.ToString(context.Session["captcha"]);
                    if (phrase.Length == 0)
                    {
                        phrase = Guid.NewGuid().ToString().Substring(0, 6);
                        context.Session["Captcha"] = phrase;
                    }
                    //Generate an image from the text stored in session
                    Bitmap imgCapthca = GenerateImage(220, 70, phrase);
                    imgCapthca.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imgBytes = memStream.GetBuffer();

                    imgCapthca.Dispose();
                    memStream.Close();

                    //Write the image as response, so it can be displayed
                    context.Response.ContentType = "image/jpeg";
                    context.Response.BinaryWrite(imgBytes);
                }
            }
            catch (Exception e)
            {
                LogError objEr = new LogError();
                objEr.WriteToFile(" error ex " + e);
                //   HttpContext.Current.Response.Redirect("~/default.aspx");
            }
        }

        public bool ValidateToken(string UserId, string token)
        {
            #region "code comment on 10 Oct 2015 for removing token validation"
            //MeetingMInder.WCFServices.UserManager objUserman = MeetingMInder.WCFServices.Service1.objUserManager.Find(p => p.AcceeToken == token.Trim() && p.UserId == UserId);
            //if (objUserman == null)//when < DateTime.UtcNow.AddHours(-1) ||
            //{

            //    return false;
            //}
            //else
            //{
            //    objUserman.timer.Stop();
            //    objUserman.timer.Start();
            //    return true;
            //}
            #endregion

            System.Data.DataSet ds = MM.Data.UserDataProvider.Instance.InsertDeviceLog("", Guid.Parse(UserId), "", "", "Download", Guid.Empty, "", "", Guid.Empty, "", token, "false", Guid.Empty, "", "");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                System.Data.DataColumnCollection columns = ds.Tables[0].Columns;

                if (columns.Contains("IsExpired"))
                {
                    return false;
                }
            }
            return true;
        }

        public Bitmap GenerateImage(int Width, int Height, string Phrase)
        {
            Bitmap CaptchaImg = new Bitmap(Width, Height);
            Random Randomizer = new Random();
            Graphics Graphic = Graphics.FromImage(CaptchaImg);
            Graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            //Set height and width of captcha image
            Graphic.FillRectangle(new SolidBrush(Color.Black), 0, 0, Width, Height);
            //Rotate text a little bit
            Graphic.RotateTransform(-3);
            Graphic.DrawString(Phrase, new Font("Verdana", 30),
                new SolidBrush(Color.White), 15, 15);
            Graphic.Flush();
            return CaptchaImg;
        }
    }
}
