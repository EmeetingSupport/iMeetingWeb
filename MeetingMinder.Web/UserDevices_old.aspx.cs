using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Domain;
using MM.Data;
using System.Data;
using MM.Core;

namespace MeetingMinder.Web
{
    public partial class UserDevices_old : System.Web.UI.Page
    {
        /// <summary>
        /// page load event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void Page_Load(object sender, EventArgs e)
        {

            //string FilePath = Server.MapPath(savePath + fileName);
            //byte[] decryptedBytes = null;
            //byte[] bytesToBeEncrypted = File.ReadAllBytes(FilePath);
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (RijndaelManaged rijndael = new RijndaelManaged())
            //    {
            //        byte[] ba = Encoding.Default.GetBytes(lblEncrptionkey.Text);
            //        var hexString = BitConverter.ToString(ba);
            //        hexString = hexString.Replace("-", "");

            //        byte[] bsa = Encoding.Default.GetBytes("0000000000000000");
            //        var hexStrings = BitConverter.ToString(bsa);
            //        hexStrings = hexStrings.Replace("-", "");

            //        rijndael.Mode = CipherMode.CBC;
            //        rijndael.Padding = PaddingMode.PKCS7;
            //        rijndael.KeySize = 256;
            //        rijndael.BlockSize = 128;
            //        rijndael.Key = EncryptionHelper.StringToByteArray(hexString);//"3030303030303030303030303030303030303030303030303030303030303030"); //prekey2; //key;

            //        rijndael.IV = EncryptionHelper.StringToByteArray(hexStrings);//"30303030303030303030303030303030");//iv;

            //        using (var cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
            //        {
            //            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
            //            cs.Close();
            //        }
            //        decryptedBytes = ms.ToArray();
            //    }

                Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                BindIpadUsers();

                #region

                if (System.Web.HttpContext.Current.Session["EntityId"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }

                #endregion
            }
        }

        /// <summary>
        /// Bind ipad all users 
        /// </summary>
        private void BindIpadUsers()
        {
            try
            {
                DataColumn FullName = new DataColumn();
                FullName.ColumnName = "FullName";
                FullName.DataType = System.Type.GetType("System.String");
                FullName.Expression = "Suffix+' '+FirstName+' '+LastName";
                IList<UserDomain> objUser = UserDataProvider.Instance.GetAllIpadUser().OrderBy(p=>p.FirstName).ToList();
                if (objUser.Count > 0)
                {
                    DataTable dt = objUser.AsDataTable();
                    dt.Columns.Add(FullName);
                    ddlUser.DataSource = dt;
                    ddlUser.DataBind();
                    ddlUser.DataValueField = "UserId";
                    ddlUser.DataTextField = "FullName";
                    ddlUser.DataBind();
                }
                ddlUser.Items.Insert(0, new ListItem("Select User", "0"));
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
        /// Dropdonlist selected index change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strUserId = ddlUser.SelectedValue;
                if (strUserId != "0")
                {
                    trUsers.Visible = true;
                    DataSet ds = UserDataProvider.Instance.GetUserDevices(strUserId);
                    grdDevices.DataSource = ds.Tables[0];
                    grdDevices.DataBind();
                }
                else
                {
                    trUsers.Visible = false;
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
        /// Gridview row command event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param
        protected void grdDevices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (e.CommandName.ToLower().Equals("lost"))
                {
                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {
                        string UdId = Convert.ToString(e.CommandArgument);
                        //LinkButton lnk = (LinkButton)(e.CommandSource);
                        //if (lnk.Text.ToLower().Equals("Whitelist"))
                        //{
                                                //}
                        
                        int id = UserDataProvider.Instance.InsertLostDevice(UdId);
                        if (id == 1)
                        {
                            ((Label)Info.FindControl("lblName")).Text = "Device whitelisted successfully.";
                            Info.Visible = true;
                            ddlUser_SelectedIndexChanged(sender, e);
                        }
                        else if (id > 0)
                        {
                            ((Label)Info.FindControl("lblName")).Text = "Device blacklisted successfully.";
                            Info.Visible = true;
                            ddlUser_SelectedIndexChanged(sender, e);
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblName")).Text = "Device whitelisting/blacklisting failed.";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                        Error.Visible = true;
                    }
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