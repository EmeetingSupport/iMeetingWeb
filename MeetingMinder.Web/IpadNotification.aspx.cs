using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Core;
using MM.Domain;
using MM.Data;
using System.Data;
using APNS;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace MeetingMinder.Web
{
    public partial class IpadNotification : System.Web.UI.Page
    {
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
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion

                // ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
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
                ddlEntity.DataSource = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId)).OrderBy(p => p.EntityName).ToList(); ;
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
        /// Bind Forum List to drop down
        /// </summary>
        /// <param name="EntityId">string specifying EntityId</param>
        private void BindForum(string EntityId)
        {
            try
            {

                ddlForum.Items.Clear();

                ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));

                Guid entityId;
                Guid UserId = Guid.Parse(Convert.ToString(Session["UserId"]));
                if (Guid.TryParse(EntityId, out entityId))
                {
                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsByUserAccess(UserId, entityId).OrderBy(p => p.ForumName).ToList();

                    //ForumDataProvider.Instance.GetForumByEntityId(entityId);
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

        /// <summary>
        /// drop down list Selected Index Change event
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
        /// drop down list  Selected Index Change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlForum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strForumId = ddlForum.SelectedValue;
                if (strForumId != null || strForumId != "0")
                {
                    Guid forumId = Guid.Parse(Convert.ToString(strForumId));
                    IList<UserDomain> objUserDom = UserDataProvider.Instance.GetAllUserForIpadNotification(forumId);
                    grdReport.DataSource = objUserDom;
                    grdReport.DataBind();

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
        /// Button send click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSend_Click(object sender, EventArgs e)
        {
            //TcpClient tcpClient = new TcpClient();

            //try
            //{
            //    tcpClient.Connect("gateway.push.apple.com", 2195);

            //}
            //catch (Exception)
            //{
            //    ((Label)Error.FindControl("lblError")).Text = "Error, Ports are not open.";
            //    Error.Visible = true;
            //    return;
            //}

            try
            {
                int count = 0;
                IList<string> receiverList = new List<string>();
                for (int i = 0; i < grdReport.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdReport.Rows[i].FindControl("chkSubAdmin");
                    if (chkSelect.Checked)
                    {
                        count++;
                        HiddenField hdnUdId = (HiddenField)grdReport.Rows[i].FindControl("hdnUdID");
                        receiverList.Add(hdnUdId.Value);
                        //receiverList.Add("16287cad3f8e709b0909441417f59d3d0a1c8044783b4e323421af1957f8d43c");
                        //receiverList.Add("5acb2a56eb2fde1ebf3ca31528a6ca55996e62f1dac34b57426b15e9fc5de2ba");
                    }
                }

                if (count > 0)
                {
                    PushNotification objNoti = new PushNotification();
                    //  objNoti.DeviceToken = "";//Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DeviceId"]);
                    objNoti.P12FilePassword = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["P12Pass"]);
                    objNoti.P12FilePath = Server.MapPath(".") + "/" + Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["P12"]);
                    //objNoti.P12FilePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["P12"]);
                    objNoti.ReceiverList = receiverList;

                    UserAcess objUser = new UserAcess();
                    if (!objUser.CSVValidation(txtMessage.Text))
                    {
                        txtMessage.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    objNoti.Message = txtMessage.Text;
                    objNoti.NoOfConnections = 1;
                    objNoti.IsSandBox = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Sandbox"]); ;

                    // APNS Push Notification 
                    string apnsUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["APNSUrl"]);
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(apnsUrl);
                    httpWebRequest.ContentType = "application/json;charset=utf-8";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string list = string.Empty;
                        foreach (string device in receiverList)
                        {
                            list += "\"" + device + "\",";
                        }
                        list = list.Substring(0, list.Length - 1);

                        var request = "{\"Message\":\"" + txtMessage.Text.Trim() + "\",\"ReceiverList\":[" + list + "]}";

                        streamWriter.Write(request);
                        streamWriter.Flush();
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    ResponseApi objRes = new ResponseApi();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var res = streamReader.ReadToEnd();
                        var data = (new JavaScriptSerializer()).Deserialize<ResponseApi>(res);
                        objRes.Status = data.Status;
                        objRes.Message = data.Message;
                    }

                    bool status = objRes.Status;

                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objRes);
                    var encrypt = Encryptor.EncryptString(json);

                    //bool status = objNoti.Notify();
                    if (status)
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "Push Notification", "Success", Convert.ToString(ddlForum.SelectedValue), "Notification sent successfully :- " + encrypt + "");

                        ((Label)Info.FindControl("lblName")).Text = " Notification sent successfully";
                        Info.Visible = true;
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "Push Notification", "Failed", Convert.ToString(ddlForum.SelectedValue), "Notification sending failed :- " + encrypt + "");

                        ((Label)Error.FindControl("lblError")).Text = " Notification sending failed";
                        Error.Visible = true;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = "Please select at least one checkbox";
                    Error.Visible = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = ex.Message; //"Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        public class ResponseApi
        {
            public string Message { get; set; }

            public bool Status { get; set; }
        }
    }
}