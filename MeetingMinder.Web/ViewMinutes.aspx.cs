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
using System.Security.Cryptography;
using System.Text;

namespace MeetingMinder.Web
{
    public partial class ViewMinutes : System.Web.UI.Page
    {
        /// <summary>
        /// page load event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {

                //Bind Entity List
                #region
                ///Comment for hide entity name
                BindEntity();
                //   ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                ///Added for hide entity name
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    BindForum(EntityId.ToString());
                    ddlEntity.SelectedValue = EntityId.ToString();
                    // ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion



                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                BindYear();
            }
        }

        /// <summary>
        /// Bind Year to drop down 
        /// </summary>

        private void BindYear()
        {
            int currentYear = DateTime.Now.Year;
            ddlYear.Items.Insert(0, new ListItem("Select Year", "0"));
            for (int i = 2012; i <= currentYear+1; i++)
            {

                ddlYear.Items.Add(i.ToString());
            }
        }

        /// <summary>
        /// Bind Entity list to drop down 
        /// </summary>
        private void BindEntity()
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"]);
                ddlEntity.DataSource = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId));
                ddlEntity.DataBind();
                ddlEntity.DataTextField = "EntityName";
                ddlEntity.DataValueField = "EntityId";
                ddlEntity.DataBind();
                ddlEntity.Items.Insert(0, new ListItem("Select Entity", "0"));
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// drop down list  Selected Index Change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlForum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strForumId = ddlForum.SelectedValue;
                if (strForumId != "0")
                {
                    BindMeeting(strForumId);
                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// Bind Meetings list to drop down
        /// </summary>
        /// <param name="strForumId">string specifying strForumId</param>
        private void BindMeeting(string strForumId)
        {
            try
            {
                ddlMeeting.Items.Clear();
                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                trMinutes.Visible = false;
                Guid forumId;
                if (Guid.TryParse(strForumId, out forumId))
                {
                    if (ddlYear.SelectedValue != "0")
                    {
                        IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).Where(p => DateTime.Parse(p.MeetingDate).Year == Convert.ToInt32(ddlYear.SelectedItem.Text)).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();

                        DateTime dtToday = DateTime.Now.Date;
                        foreach (MeetingDomain item in objMeeting)
                        {
                            DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
                            //if (dtMeeting < dtToday)
                            //{
                            //ddlMeeting.Items.Add(new ListItem(dtMeeting.ToString("dd MMM yyyy") + ' ' + item.MeetingVenue + ' ' + item.MeetingTime, item.MeetingId.ToString()));
                            ddlMeeting.Items.Add(new ListItem(item.MeetingNumber + ' ' + item.ForumName + ' ' + dtMeeting.ToString("MMMM dd, yyyy"), item.MeetingId.ToString()));

                            //}
                        }
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = "Invalid meeting search";
                    Error.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }

        }

        /// Bind Entity list to drop down
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strEntityId = ddlEntity.SelectedValue;
                if (strEntityId != "0")
                {
                    BindForum(strEntityId);
                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// Bind forums to drop down
        /// </summary>
        /// <param name="EntityId">string specifying EntityId</param>
        private void BindForum(string EntityId)
        {
            try
            {

                ddlForum.Items.Clear();
                trMinutes.Visible = false;
                ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                Guid UserId = Guid.Parse(Convert.ToString(Session["UserId"]));
                Guid entityId;
                if (Guid.TryParse(EntityId, out entityId))
                {
                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsByUserAccess(UserId, entityId).OrderBy(p => p.ForumName).ToList();
                    //IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumByEntityId(entityId);                  

                    ddlForum.DataSource = objForum;
                    ddlForum.DataBind();
                    ddlForum.DataTextField = "ForumName";
                    ddlForum.DataValueField = "ForumId";
                    ddlForum.DataBind();
                    ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = "Invalid forum search";
                    Error.Visible = true;
                }

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }


        /// Bind Entity list to drop down
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strMeetingId = ddlMeeting.SelectedValue;
                if (strMeetingId != "0")
                {
                    UploadMinutesDomain objMinutes = UploadMintesDataProvider.Instance.GetUploadMinutesByMeetingId(Guid.Parse(strMeetingId));
                    trMinutes.Visible = true;

                    //IList<DraftMOMDomain> objDraftMom = DraftMOMDataProvider.Instance.GetDraftMOMDomainByMeetingid(Guid.Parse(strMeetingId));
                    //grdDraftMOM.DataSource = objDraftMom;
                    //grdDraftMOM.DataBind();


                    if (objMinutes != null)
                    {
                        if (objMinutes.UploadFile.Length > 0)
                        {
                            ViewState["EncryptionKey"] = objMinutes.EncryptionKey;
                            ViewState["Member"] = objMinutes.UploadFile;
                            lnkView.Visible = true;
                            lblView.Visible = false;
                        }
                        else
                        {
                            lblView.Text = "No file uploaded";
                            lblView.ForeColor = System.Drawing.Color.Red;
                            lblView.Visible = true;
                            lnkView.Visible = false;
                        }
                    }
                    else
                    {
                        lblView.Text = "No file uploaded";
                        lblView.ForeColor = System.Drawing.Color.Red;
                        lblView.Visible = true;
                        lnkView.Visible = false;
                    }

                }
                else
                {
                    trMinutes.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// View uploaded file
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void lnkView_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = Convert.ToString(ViewState["Member"]);

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);
                string FilePath = Server.MapPath(savePath + fileName);
                byte[] decryptedBytes = null;
                byte[] bytesToBeEncrypted = File.ReadAllBytes(FilePath);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged rijndael = new RijndaelManaged())
                    {
                        byte[] ba = Encoding.Default.GetBytes(ViewState["EncryptionKey"].ToString());
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
                    //Set the appropriate ContentType.
                    Response.ContentType = "Application/pdf";
                    //Get the physical path to the file.
                    // string FilePath = Server.MapPath(savePath + fileName);

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + "Minutes.pdf");
                    // Response.WriteFile(FilePath);
                    Response.BinaryWrite(ms.ToArray());
                    Response.Flush();
                }

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

    }
}