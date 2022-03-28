using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Core;
using MM.Domain;
using MM.Data;
using System.IO;
using System.Data;
using iTextSharp.text.pdf;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using iTextSharp.text.exceptions;

namespace MeetingMinder.Web
{
    public partial class NoticeMaster : System.Web.UI.Page
    {
        /// <summary>
        /// page load event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["agenda1"] != null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('File not found');</script>'");
                return;
            }

            if (Request.QueryString["agenda"] != null)
            {
                try
                {
                    if (Session["UserId"] == null)
                    {
                        Response.Redirect("Login.aspx");
                    }
                    LogError objEr = new LogError();
                    string FilePath_New = "";
                    string agendaName = Convert.ToString(Request.QueryString["agenda"]);
                    //objEr.WriteToFile("  query str" + agendaName);
                    //agendaName = HttpUtility.UrlDecode(agendaName);
                    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);
                    string FilePath = Server.MapPath(savePath + agendaName + ".pdf");
                    //objEr.WriteToFile("  FilePath " + FilePath);
                    string AgendaKey = "";
                    string AgendaEncrKey = "";
                    bool isActive;
                    if (agendaName.IndexOf("_merge") > -1)
                    {
                        agendaName = agendaName.Substring(0, agendaName.Length - 6);
                    }
                    string newName = agendaName;
                    //objEr.WriteToFile("  newName " + newName);
                    DataSet ds = MM.Data.AgendaDataProvider.Instance.GetAgendKeysByName(agendaName + ".pdf", Guid.Parse(Session["UserId"].ToString()));
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string filename = ds.Tables[0].Rows[0]["OldName"].ToString();
                        if (filename.Length > 3)
                        {
                            string NewAgendaName = ds.Tables[0].Rows[0]["AgendaName"].ToString();
                            newName = NewAgendaName.Substring(0, NewAgendaName.Length - 4); ;
                        }
                        //objEr.WriteToFile(" in  newName " + newName);
                        AgendaKey = ds.Tables[0].Rows[0]["AgendaKey"].ToString();
                        AgendaEncrKey = MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[0]["EncryptionKey"].ToString());
                        if (ds.Tables[0].Rows[0]["IsActive"].ToString() == "1")
                        {
                            isActive = true;
                        }
                        else
                        {
                            isActive = false;
                        }
                        //isActive = Convert.ToBoolean(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["IsActive"].ToString()) ? "false" : ds.Tables[0].Rows[0]["IsActive"].ToString());
                        if (!isActive)
                        {
                            Response.Redirect("Login.aspx");
                        }
                    }

                    PdfReader reader = null;
                    if (AgendaKey == "")
                    {
                        reader = new PdfReader(FilePath);
                    }

                    else
                    {
                        string PdfPassword = EncryptionHelper.GetPassword(newName + ".pdf", AgendaKey);
                        //objEr.WriteToFile("  PdfPassword " + PdfPassword);
                        if (PdfPassword == "")
                        {
                            reader = new PdfReader(FilePath);
                        }
                        else
                        {
                            string agendaName_New = Convert.ToString(Request.QueryString["agenda"]);
                            FilePath_New = Server.MapPath(savePath + "00" + agendaName_New + ".pdf");
                            #region aes 128 bit decryption
                            //using (AesCryptoServiceProvider acsp = EncryptionHelper.GetProvider(Encoding.Default.GetBytes(AgendaEncrKey)))
                            //{
                            //    ICryptoTransform ictD = acsp.CreateDecryptor();

                            //    //RawBytes now contains original byte array, still in Encrypted state

                            //    //Decrypt into stream
                            //    //  MemoryStream msD = new MemoryStream(RawBytes, 0, RawBytes.Length);
                            //    using (FileStream msD = new FileStream(FilePath, FileMode.Open))
                            //    {
                            //        using (CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read))
                            //        {
                            //            using (FileStream fsOutput = new FileStream(FilePath_New, FileMode.Create))
                            //            {
                            //                int data;
                            //                while ((data = csD.ReadByte()) != -1)
                            //                {
                            //                    fsOutput.WriteByte((byte)data);
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                            #endregion
                            byte[] decryptedBytes = null;
                            byte[] bytesToBeEncrypted = File.ReadAllBytes(FilePath);
                            using (MemoryStream ms = new MemoryStream())
                            {
                                using (RijndaelManaged rijndael = new RijndaelManaged())
                                {
                                    byte[] ba = Encoding.Default.GetBytes(AgendaEncrKey);
                                    var hexString = BitConverter.ToString(ba);
                                    hexString = hexString.Replace("-", "");

                                    byte[] bsa = Encoding.Default.GetBytes("0000000000000000");
                                    var hexStrings = BitConverter.ToString(bsa);
                                    hexStrings = hexStrings.Replace("-", "");

                                    rijndael.Mode = CipherMode.CBC;
                                    rijndael.Padding = PaddingMode.PKCS7;
                                    rijndael.KeySize = 256;
                                    rijndael.BlockSize = 128;
                                    rijndael.Key = EncryptionHelper.StringToByteArray(hexString);//"3030303030303030303030303030303030303030303030303030303030303030"); //prekey2; //key;

                                    rijndael.IV = EncryptionHelper.StringToByteArray(hexStrings);//"30303030303030303030303030303030");//iv;

                                    using (var cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                                    {
                                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                                        cs.Close();
                                    }
                                    decryptedBytes = ms.ToArray();
                                }
                            }
                            File.WriteAllBytes(FilePath_New, decryptedBytes);
                            reader = new PdfReader(FilePath_New, new System.Text.ASCIIEncoding().GetBytes(PdfPassword));

                        }
                    }
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        //objEr.WriteToFile("  memoryStream ");

                        PdfStamper stamper = new PdfStamper(reader, memoryStream);

                        stamper.Close();
                        reader.Close();

                        /*Response.ContentType = "application/pdf";
                        //Response.ContentType = "application/octet-stream";
                        //Get the physical path to the file.
                        // string FilePath = Server.MapPath(savePath + fileName);

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + "Agenda.pdf");
                        // Response.WriteFile(FilePath);
                        Response.BinaryWrite(memoryStream.ToArray());
                        Response.Flush();

                        if (File.Exists(FilePath_New))
                        {
                            File.Delete(FilePath_New);
                        }
                        Response.End();*/

                        Response.ContentType = "application/pdf";

                        Response.Clear();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.Buffer = true;

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + "Agenda.pdf");
                        Response.BinaryWrite(memoryStream.ToArray());
                        Response.Flush();

                        if (File.Exists(FilePath_New))
                        {
                            File.Delete(FilePath_New);
                        }
                        Response.Close();
                    }

                }
                catch (BadPasswordException exs)
                {
                    LogError objEr = new LogError();
                    objEr.HandleException(exs);
                    return;
                }

                catch (IOException ex)
                {
                    LogError objEr = new LogError();
                    objEr.HandleException(ex);
                    return;
                }

                catch (Exception exsas)
                {
                    //((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                    //Error.Visible = true;
                    LogError objEr = new LogError();
                    objEr.HandleException(exsas);
                    return;
                }
            }

            if (Request.QueryString["igendapdf"] != null)
            {
                try
                {
                    LogError objEr = new LogError();
                    string FilePath_New = "";
                    //objEr.WriteToFile("received ");
                    if (Request.Headers["AccessToken"] != null)
                    {

                        string userID = HttpContext.Current.Request.Headers["userid"];
                        string token = Request.Headers["AccessToken"];

                        if (userID != null && token != null && token.Length > 0)
                        {
                            //objEr.WriteToFile(" token -->" + token + " UserId-->" + userID);

                            string AgendaName = string.Empty;
                            if (Request.QueryString["igendapdf"] != null)
                            {
                                AgendaName = "{AgendaName:" + Convert.ToString(Request.QueryString["igendapdf"]) + ".pdf" + "}";
                            }

                            if (!ValidateToken(token, userID, AgendaName))
                            {
                                //  Response.AppendHeader("TokenExpired", "true");
                                HttpContext.Current.Response.Headers["TokenExpired"] = "true";
                                //Response.StatusCode = 400;
                                Response.End();
                                return;
                            }

                        }
                        else
                        {
                            HttpContext.Current.Response.Headers["TokenExpired"] = "true";
                            //  Response.AppendHeader("TokenExpired", "true");
                            //  Response.StatusCode = 400;
                            Response.End();
                            return;
                        }
                    }
                    else
                    {
                        HttpContext.Current.Response.Headers["TokenExpired"] = "true";
                        //Response.AppendHeader("TokenExpired", "true");
                        // Response.StatusCode = 400;
                        Response.End();
                        return;
                    }


                    string agendaName = Convert.ToString(Request.QueryString["igendapdf"]);
                    //objEr.WriteToFile("  query str" + agendaName);
                    //agendaName = HttpUtility.UrlDecode(agendaName);
                    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);
                    string FilePath = Server.MapPath(savePath + agendaName + ".pdf");
                    //objEr.WriteToFile("  FilePath " + FilePath);
                    string AgendaKey = "";
                    string AgendaEncrKey = "";
                    if (agendaName.IndexOf("_merge") > -1)
                    {
                        agendaName = agendaName.Substring(0, agendaName.Length - 6);
                    }

                    //DataSet ds = MM.Data.AgendaDataProvider.Instance.GetAgendKeysByName(agendaName + ".pdf");
                    //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    //{
                    //    AgendaKey = ds.Tables[0].Rows[0]["AgendaKey"].ToString();
                    //    AgendaEncrKey = MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[0]["EncryptionKey"].ToString());
                    //}
                    bool isActive;
                    string UserIds = "";
                    //UserIds = "90c21df1-992e-4de7-b525-27a2e8814e08";//Session["UserId"].ToString();
                    if (Request.Headers["UserId"] != null)
                    {
                        UserIds = Request.Headers["UserId"];
                    }
                    else
                    {
                        Response.AppendHeader("TokenExpired", "true");
                        Response.StatusCode = 400;
                        Response.End();
                        return;
                    }
                    string newName = agendaName;
                    DataSet ds = MM.Data.AgendaDataProvider.Instance.GetAgendKeysByName(agendaName + ".pdf", Guid.Parse(UserIds));
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string filename = ds.Tables[0].Rows[0]["OldName"].ToString();
                        if (filename.Length > 3)
                        {
                            string NewAgendaName = ds.Tables[0].Rows[0]["AgendaName"].ToString();
                            newName = NewAgendaName.Substring(0, NewAgendaName.Length - 4); ;
                        }
                        AgendaKey = ds.Tables[0].Rows[0]["AgendaKey"].ToString();
                        AgendaEncrKey = MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[0]["EncryptionKey"].ToString());

                        if (ds.Tables[0].Rows[0]["IsActive"].ToString() == "1")
                        {
                            isActive = true;
                        }
                        else
                        {
                            isActive = false;
                        }
                        //  isActive = Convert.ToBoolean(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["IsActive"].ToString()) ? "false" : ds.Tables[0].Rows[0]["IsActive"].ToString());
                        if (!isActive)
                        {
                            Response.Redirect("Login.aspx");
                        }
                    }

                    PdfReader reader = null;
                    if (AgendaKey == "")
                    {
                        reader = new PdfReader(FilePath);
                    }

                    else
                    {
                        string PdfPassword = EncryptionHelper.GetPassword(newName + ".pdf", AgendaKey);
                        //objEr.WriteToFile("  PdfPassword " + PdfPassword);
                        if (PdfPassword == "")
                        {
                            reader = new PdfReader(FilePath);
                        }
                        else
                        {
                            string agendaName_New = Convert.ToString(Request.QueryString["igendapdf"]);
                            FilePath_New = Server.MapPath(savePath + "00" + agendaName_New + ".pdf");
                            #region aes 128 bit decryption
                            //using (AesCryptoServiceProvider acsp = EncryptionHelper.GetProvider(Encoding.Default.GetBytes(AgendaEncrKey)))
                            //{
                            //    ICryptoTransform ictD = acsp.CreateDecryptor();

                            //    //RawBytes now contains original byte array, still in Encrypted state

                            //    //Decrypt into stream
                            //    //  MemoryStream msD = new MemoryStream(RawBytes, 0, RawBytes.Length);
                            //    using (FileStream msD = new FileStream(FilePath, FileMode.Open))
                            //    {
                            //        using (CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read))
                            //        {
                            //            using (FileStream fsOutput = new FileStream(FilePath_New, FileMode.Create))
                            //            {
                            //                int data;
                            //                while ((data = csD.ReadByte()) != -1)
                            //                {
                            //                    fsOutput.WriteByte((byte)data);
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                            #endregion
                            byte[] decryptedBytes = null;
                            byte[] bytesToBeEncrypted = File.ReadAllBytes(FilePath);
                            using (MemoryStream ms = new MemoryStream())
                            {
                                using (RijndaelManaged rijndael = new RijndaelManaged())
                                {
                                    byte[] ba = Encoding.Default.GetBytes(AgendaEncrKey);
                                    var hexString = BitConverter.ToString(ba);
                                    hexString = hexString.Replace("-", "");

                                    byte[] bsa = Encoding.Default.GetBytes("0000000000000000");
                                    var hexStrings = BitConverter.ToString(bsa);
                                    hexStrings = hexStrings.Replace("-", "");

                                    rijndael.Mode = CipherMode.CBC;
                                    rijndael.Padding = PaddingMode.PKCS7;
                                    rijndael.KeySize = 256;
                                    rijndael.BlockSize = 128;
                                    rijndael.Key = EncryptionHelper.StringToByteArray(hexString);//"3030303030303030303030303030303030303030303030303030303030303030"); //prekey2; //key;

                                    rijndael.IV = EncryptionHelper.StringToByteArray(hexStrings);//"30303030303030303030303030303030");//iv;


                                    using (var cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                                    {
                                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                                        cs.Close();
                                    }
                                    decryptedBytes = ms.ToArray();
                                }
                            }
                            File.WriteAllBytes(FilePath_New, decryptedBytes);
                            reader = new PdfReader(FilePath_New, new System.Text.ASCIIEncoding().GetBytes(PdfPassword));

                        }
                    }
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        //objEr.WriteToFile("  memoryStream ");

                        PdfStamper stamper = new PdfStamper(reader, memoryStream);
                        stamper.Close();
                        reader.Close();
                        File.WriteAllBytes(FilePath_New, memoryStream.ToArray());

                        //  Response.End();
                    }

                    EncryptionHelper.EncryptString(FilePath_New, AgendaEncrKey);

                    //Response.ContentType = "application/octet-stream";

                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + "Agenda.pdf");
                    //// Response.WriteFile(FilePath);
                    //Response.BinaryWrite(File.ReadAllBytes(FilePath_New).ToArray());
                    //Response.Flush();

                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.ClearContent();
                    response.ClearHeaders();
                    response.Buffer = true;
                    response.AddHeader("Content-Disposition", "attachment;filename=\"" + "Agenda.pdf");
                    byte[] data = File.ReadAllBytes(FilePath_New).ToArray();
                    response.BinaryWrite(data);
                    //   response.End();
                    if (File.Exists(FilePath_New))
                    {
                        File.Delete(FilePath_New);
                    }
                    response.End();
                }
                catch (BadPasswordException exs)
                {
                    LogError objEr = new LogError();
                    objEr.HandleException(exs);
                    return;
                }

                catch (IOException ex)
                {
                    LogError objEr = new LogError();
                    objEr.HandleException(ex);
                    return;
                }

                catch (Exception exsas)
                {
                    //((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                    //Error.Visible = true;
                    LogError objEr = new LogError();
                    objEr.HandleException(exsas);
                    return;
                }
            }
            //Info.Visible = false;
            //Error.Visible = false;
            // Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                //bind meeting list
                // BindCheckers();

                //Bind Entity List
                #region
                ///Comment for hide entity name
                //   BindEntity();
                //   ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                ///Added for hide entity name
                //if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                // {
                //  Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                //  BindForum(EntityId.ToString());
                //  ddlEntity.SelectedValue = EntityId.ToString();
                // }
                //else
                //{
                //   Response.Redirect("~/login.aspx");
                // }
                #endregion

                // ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));

                //ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));

                //Bind Mintes to grid
                //  BindNotice();
            }
        }

        /// <summary>
        /// validate token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool ValidateToken(string token, string UserId, string AgendaId)
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


            //DataSet ds = MM.Data.UserDataProvider.Instance.InsertDeviceLog("", Guid.Parse(UserId), "", "", "Download", Guid.Empty, "", "", Guid.Empty, "", token, "false", Guid.Empty);
            DataSet ds = MM.Data.UserDataProvider.Instance.InsertDeviceLog("", Guid.Parse(UserId), "", "", "Download", Guid.Empty, "", AgendaId, Guid.Empty, "", token, "false", Guid.Empty, "", "");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                System.Data.DataColumnCollection columns = ds.Tables[0].Columns;

                if (columns.Contains("IsExpired"))
                {
                    return false;
                }
            }
            return true;
            #endregion

        }

        /// <summary>
        /// Bind notice details
        /// </summary>
        //        private void BindNotice()
        //        {
        //            try
        //            {
        //                Guid UserId = Guid.Parse(Session["UserId"].ToString());
        //                grdNotice.DataSource = NoticeDataProvider.Instance.GetNoticeWithMeeting(UserId).OrderBy(p => p.MeetingDate).ToList(); ;
        //                grdNotice.DataBind();
        //            }
        //            catch (Exception ex)
        //            {
        //                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //                Error.Visible = true;

        //                LogError objEr = new LogError();
        //                objEr.HandleException(ex);
        //            }
        //        }

        //        /// <summary>
        //        /// Bind Entity list to drop down 
        //        /// </summary>
        //        private void BindEntity()
        //        {
        //            try
        //            {
        //                string UserId = Convert.ToString(Session["UserId"]);
        //                ddlEntity.DataSource = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId)).OrderBy(p => p.EntityName).ToList(); ;
        //                ddlEntity.DataBind();
        //                ddlEntity.DataTextField = "EntityName";
        //                ddlEntity.DataValueField = "EntityId";
        //                ddlEntity.DataBind();
        //                ddlEntity.Items.Insert(0, new ListItem("Select Entity", "0"));
        //            }
        //            catch (Exception ex)
        //            {
        //                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //                Error.Visible = true;

        //                LogError objEr = new LogError();
        //                objEr.HandleException(ex);
        //            }
        //        }

        //        /// <summary>
        //        /// Bind Checkers List to drop down
        //        /// </summary>
        //        private void BindCheckers()
        //        {
        //            try
        //            {
        //                ddlUser.DataSource = UserDataProvider.Instance.GetAllChecker().OrderBy(p => p.FirstName).ToList(); ;
        //                ddlUser.DataBind();
        //                ddlUser.DataValueField = "UserId";
        //                ddlUser.DataTextField = "UserName";
        //                ddlUser.DataBind();

        //                //Remove logged in users id from drop down
        //                // string UserId = Convert.ToString(Session["UserId"]);
        //                //  ListItem UserIdToRemove = ddlUser.Items.FindByValue(UserId);
        //                //  ddlUser.Items.Remove(UserIdToRemove);

        //                ddlUser.Items.Insert(0, new ListItem("Select User", "0"));
        //            }
        //            catch (Exception ex)
        //            {
        //                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //                Error.Visible = true;

        //                LogError objEr = new LogError();
        //                objEr.HandleException(ex);
        //            }
        //        }

        //        /// <summary>
        //        /// drop down list  Selected Index Change event
        //        /// </summary>
        //        /// <param name="sender">Object specifying sender</param>
        //        /// <param name="e">EventArgs specifying e </param>
        //        protected void ddlForum_SelectedIndexChanged(object sender, EventArgs e)
        //        {
        //            try
        //            {
        //                string strForumId = ddlForum.SelectedValue;
        //                if (strForumId != "0")
        //                {
        //                    BindMeeting(strForumId);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //                Error.Visible = true;

        //                LogError objEr = new LogError();
        //                objEr.HandleException(ex);
        //            }
        //        }

        //        /// <summary>
        //        /// BInd Meeting list to drop down
        //        /// </summary>
        //        /// <param name="ForumId">string specifying ForumId</param>
        //        private void BindMeeting(string ForumId)
        //        {
        //            try
        //            {
        //                string Forum = ddlForum.SelectedItem.Text;
        //                string Entity = ddlEntity.SelectedItem.Text;
        //                txtNotice.Text = "Notice is hereby given that the meeting of the " + Forum + " of " + Entity;
        //                ddlMeeting.Items.Clear();
        //                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
        //                Guid forumId;
        //                if (Guid.TryParse(ForumId, out forumId))
        //                {
        //                    IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId); //MeetingDataProvider.Instance.GetMeetingByForUploadMinutes(forumId);
        //                    DateTime dtToday = DateTime.Now.Date;
        //                    foreach (MeetingDomain item in objMeeting)
        //                    {
        //                        DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
        //                        if (dtMeeting > dtToday)
        //                        {
        //                            ddlMeeting.Items.Add(new ListItem(dtMeeting.ToString("dd MMM yyyy") + ' ' + item.MeetingVenue + ' ' + item.MeetingTime, item.MeetingId.ToString()));
        //                        }
        //                    }
        //                    //ddlMeeting.DataSource = objMeeting;
        //                    //ddlMeeting.DataBind();

        //                    ////Past date ........
        //                    //ddlMeeting.DataTextField = "MeetingDate";
        //                    //ddlMeeting.DataValueField = "MeetingId";
        //                    //ddlMeeting.DataBind();


        //                }
        //                else
        //                {
        //                    ((Label)Error.FindControl("lblError")).Text = "Invalid meeting search";
        //                    Error.Visible = true;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //                Error.Visible = true;

        //                LogError objEr = new LogError();
        //                objEr.HandleException(ex);
        //            }
        //        }

        //        /// <summary>
        //        /// Bind Entity list to drop down
        //        /// </summary>
        //        /// <param name="sender">Object specifying sender</param>
        //        /// <param name="e">EventArgs specifying e </param>
        //        protected void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
        //        {
        //            try
        //            {
        //                string strEntityId = ddlEntity.SelectedValue;
        //                if (strEntityId != "0")
        //                {
        //                    BindForum(strEntityId);

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //                Error.Visible = true;

        //                LogError objEr = new LogError();
        //                objEr.HandleException(ex);
        //            }
        //        }

        //        /// <summary>
        //        /// Bind forums to drop down
        //        /// </summary>
        //        /// <param name="EntityId">sring specifying EntityId</param>
        //        private void BindForum(string EntityId)
        //        {
        //            try
        //            {

        //                ddlForum.Items.Clear();

        //                ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));

        //                Guid entityId;
        //                if (Guid.TryParse(EntityId, out entityId))
        //                {
        //                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumByEntityId(entityId);
        //                    ddlForum.DataSource = objForum;
        //                    ddlForum.DataBind();
        //                    ddlForum.DataTextField = "ForumName";
        //                    ddlForum.DataValueField = "ForumId";
        //                    ddlForum.DataBind();
        //                    ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));

        //                }
        //                else
        //                {
        //                    ((Label)Error.FindControl("lblError")).Text = "Invalid forum search";
        //                    Error.Visible = true;
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //                Error.Visible = true;

        //                LogError objEr = new LogError();
        //                objEr.HandleException(ex);
        //            }
        //        }


        //        /// <summary>
        //        /// button submit click event
        //        /// </summary>
        //        /// <param name="sender">Object specifying sender</param>
        //        /// <param name="e">EventArgs specifying e </param>
        //        protected void btnSubmit_Click(object sender, EventArgs e)
        //        {
        //            try
        //            {
        //                NoticeDomain objNotice = new NoticeDomain();
        //                objNotice.MeetingId = Guid.Parse(ddlMeeting.SelectedValue);
        //                objNotice.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
        //                objNotice.CreatedBy = Guid.Parse(Session["UserId"].ToString());

        //                objNotice.NoticeChecker = Guid.Parse(ddlUser.SelectedValue);
        //                objNotice.NoticeMessage = txtNotice.Text;

        //                if (objNotice.CreatedBy == objNotice.NoticeChecker)
        //                {
        //                    ((Label)Error.FindControl("lblError")).Text = "Checker and Maker cannot be same ";
        //                    Error.Visible = true;
        //                    return;
        //                }
        //                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
        //                if (isMaker)
        //                {

        //                    if (hdnNotice.Value != "")
        //                    {
        //                        objNotice.NoticeId = Guid.Parse(hdnNotice.Value);
        //                        NoticeDataProvider.Instance.Update(objNotice);
        //                        ((Label)Info.FindControl("lblName")).Text = "Notice updated successfully";
        //                        Info.Visible = true;
        //                        ClearData();
        //                        BindNotice();
        //                    }
        //                    else
        //                    {
        //                        UserAcess objUser = new UserAcess();
        //                        //check add permission
        //                        if (objUser.IsAdd(Guid.Parse(ddlEntity.SelectedValue)))
        //                        {

        //                            NoticeDataProvider.Instance.Insert(objNotice);
        //                            ((Label)Info.FindControl("lblName")).Text = "Notice inserted successfully";
        //                            Info.Visible = true;
        //                            ClearData();
        //                            BindNotice();
        //                        }
        //                        else
        //                        {
        //                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
        //                            Error.Visible = true;
        //                        }
        //                    }

        //                }
        //                else
        //                {
        //                    ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
        //                    Error.Visible = true;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //                Error.Visible = true;

        //                LogError objEr = new LogError();
        //                objEr.HandleException(ex);
        //            }
        //        }

        //        /// <summary>
        //        /// Clear Form data
        //        /// </summary>
        //        private void ClearData()
        //        {
        //            try
        //            {
        //                ddlUser.SelectedValue = "0";
        //                ddlMeeting.Items.Clear();
        //                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
        //                ddlForum.Items.Clear();
        //                ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
        //                ddlEntity.SelectedValue = "0";
        //                txtNotice.Text = "";
        //                hdnNotice.Value = "";
        //                btnSubmit.Text = "Submit";
        //            }
        //            catch (Exception ex)
        //            {
        //                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //                Error.Visible = true;

        //                LogError objEr = new LogError();
        //                objEr.HandleException(ex);
        //            }
        //        }

        //        /// <summary>
        //        /// Button cancel event
        //        /// </summary>
        //        /// <param name="sender">Object specifying sender</param>
        //        /// <param name="e">EventArgs specifying e </param>
        //        protected void btnCancel_Click(object sender, EventArgs e)
        //        {
        //            ClearData();
        //        }

        //        /// <summary>
        //        /// gridview page index change event
        //        /// </summary>
        //        /// <param name="sender">Object specifying sender</param>
        //        /// <param name="e">GridViewPageEventArgs specifying e </param>
        //        protected void grdNotice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //        {
        //            try
        //            {
        //                grdNotice.PageIndex = e.NewPageIndex;
        //                BindNotice();
        //            }
        //            catch (Exception ex)
        //            {
        //                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //                Error.Visible = true;

        //                LogError objEr = new LogError();
        //                objEr.HandleException(ex);
        //            }
        //        }

        //        /// <summary>
        //        /// Gridview row delete Event
        //        /// </summary>
        //        /// <param name="sender">Object specifying sender</param>
        //        /// <param name="e">GridViewPageEventArgs specifying e </param>
        //        protected void grdNotice_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //        {

        //        }

        //        /// <summary>
        //        /// Gridview row editing event
        //        /// </summary>
        //        /// <param name="sender">Object specifying sender</param>
        //        /// <param name="e">GridViewPageEventArgs specifying e </param>
        //        protected void grdNotice_RowEditing(object sender, GridViewEditEventArgs e)
        //        {

        //        }

        //        /// <summary>
        //        /// Gridview row sorting event
        //        /// </summary>
        //        /// <param name="sender">Object specifying sender</param>
        //        /// <param name="e">GridViewPageEventArgs specifying e </param>
        //        protected void grdNotice_Sorting(object sender, GridViewSortEventArgs e)
        //        {
        //            try
        //            {
        //                Guid userId = Guid.Parse(Session["UserId"].ToString());
        //                DataTable dt = (DataTable)NoticeDataProvider.Instance.GetNoticeWithMeeting(userId).AsDataTable();
        //                DataView dv = new DataView(dt);
        //                if (Convert.ToString(ViewState["sortDirection"]) == "asc")
        //                {
        //                    ViewState["sortDirection"] = "dsc";
        //                    dv.Sort = e.SortExpression + " DESC";
        //                }
        //                else
        //                {
        //                    ViewState["sortDirection"] = "asc";
        //                    dv.Sort = e.SortExpression + " ASC";
        //                }

        //                grdNotice.DataSource = dv;
        //                grdNotice.DataBind();
        //            }
        //            catch (Exception ex)
        //            {
        //                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //                Error.Visible = true;

        //                LogError objEr = new LogError();
        //                objEr.HandleException(ex);
        //            }
        //        }


        //        /// <summary>
        //        /// Gridview row command event
        //        /// </summary>
        //        /// <param name="sender">Object specifying sender</param>
        //        /// <param name="e">GridViewPageEventArgs specifying e </param>
        //        protected void grdNotice_RowCommand(object sender, GridViewCommandEventArgs e)
        //        {
        //            try
        //            {
        //                UserAcess objUser = new UserAcess();
        //                if (e.CommandName.ToLower().Equals("delete"))
        //                {
        //                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
        //                    if (isMaker)
        //                    {
        //                        string noticeId = Convert.ToString(e.CommandArgument);
        //                        string[] ids = noticeId.Split(',');
        //                        //check edit permission
        //                        if (objUser.isDelete(Guid.Parse(ids[1])))
        //                        {
        //                            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
        //                            Label lblMeeting = (Label)row.Cells[0].FindControl("lblMeet");
        //                            if (lblMeeting.Text.Length > 0)
        //                            {
        //                                DateTime dtNow = DateTime.Now;
        //                                DateTime dtMeetingDate = Convert.ToDateTime(lblMeeting.Text);
        //                                if (dtMeetingDate < dtNow)
        //                                {
        //                                    ((Label)Error.FindControl("lblError")).Text = " Sorry you can't delete past meeting notice. ";
        //                                    Error.Visible = true;
        //                                    return;
        //                                }
        //                            }

        //                            bool bStatus = NoticeDataProvider.Instance.Delete(Guid.Parse(ids[0]));
        //                            if (bStatus == false)
        //                            {
        //                                ((Label)Info.FindControl("lblName")).Text = "Entity Deleted Successfully";
        //                                Info.Visible = true;
        //                                BindNotice();
        //                            }
        //                            else
        //                            {
        //                                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //                                Error.Visible = true;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
        //                            Error.Visible = true;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
        //                        Error.Visible = true;
        //                    }
        //                }

        //                if (e.CommandName.ToLower().Equals("edit"))
        //                {
        //                    ClearData();
        //                    string NoticeIds = Convert.ToString(e.CommandArgument);
        //                    string[] ids = NoticeIds.Split(',');

        //                    //check edit permission
        //                    if (objUser.IsEdit(Guid.Parse(ids[1])))
        //                    {
        //                        NoticeDomain objNotice = NoticeDataProvider.Instance.Get(Guid.Parse(ids[0]));
        //                        DateTime dtNow = DateTime.Now;
        //                        DateTime dtMeetingDate = Convert.ToDateTime(ids[3]);
        //                        if (dtMeetingDate < dtNow)
        //                        {
        //                            ((Label)Error.FindControl("lblError")).Text = " Sorry you cant edit past meeting notice. ";
        //                            Error.Visible = true;
        //                            return;
        //                        }

        //                        txtNotice.Text = objNotice.NoticeMessage;

        //                        //ddlUser.SelectedValue = objNotice.NoticeChecker.ToString();


        //                        if (ddlUser.Items.FindByValue(
        //Convert.ToString(objNotice.NoticeChecker)) != null)
        //                        {
        //                            ddlUser.SelectedValue = objNotice.NoticeChecker.ToString();
        //                        }
        //                        else
        //                        {
        //                            ddlUser.SelectedValue = "0";
        //                        }

        //                        hdnNotice.Value = objNotice.NoticeId.ToString();

        //                        ddlEntity.SelectedValue = ids[1];
        //                        ddlEntity_SelectedIndexChanged(sender, e);
        //                        ddlForum.SelectedValue = ids[2];
        //                        ddlForum_SelectedIndexChanged(sender, e);

        //                        ddlMeeting.SelectedValue = objNotice.MeetingId.ToString();
        //                        btnSubmit.Text = "Update";
        //                    }
        //                    else
        //                    {
        //                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
        //                        Error.Visible = true;
        //                        Info.Visible = false;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //                Error.Visible = true;

        //                LogError objEr = new LogError();
        //                objEr.HandleException(ex);
        //            }

        //        }

    }
}