using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Data;
using MM.Domain;
using System.Data;
using MM.Core;

namespace MeetingMinder.Web
{
    public partial class SerialNumberMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;
            if (!Page.IsPostBack)
            {

                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

                    BindSerialNumber();
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }

            }
        }


        /// <summary>
        /// Bind Serial Number to grid
        /// </summary>
        private void BindSerialNumber()
        {
            try
            {
                IList<SerialNumberDomain> objSerialNumber = SerialNumberDataProvider.Instance.Get().OrderBy(p => p.SerialNumber).ToList();
                grdSerialNumber.DataSource = objSerialNumber;
                grdSerialNumber.DataBind();

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }
        protected void grdSerialNumber_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grdSerialNumber_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClearData();
            grdSerialNumber.PageIndex = e.NewPageIndex;
            BindSerialNumber();
        }

        protected void grdSerialNumber_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)SerialNumberDataProvider.Instance.Get().AsDataTable();
                DataView dv = new DataView(dt);
                if (Convert.ToString(ViewState["sortDirection"]) == "asc")
                {
                    ViewState["sortDirection"] = "dsc";
                    dv.Sort = e.SortExpression + " DESC";
                }
                else
                {
                    ViewState["sortDirection"] = "asc";
                    dv.Sort = e.SortExpression + " ASC";
                }

                grdSerialNumber.DataSource = dv;
                grdSerialNumber.DataBind();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void grdSerialNumber_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName.ToLower().Equals("edit"))
                {

                    SerialNumberDomain objSerialNumberDomain = SerialNumberDataProvider.Instance.Get(Convert.ToInt32(Convert.ToString(e.CommandArgument)));
                    txtSerialNumber.Text = objSerialNumberDomain.SerialNumber.ToString();
                    hdnSerialNumberId.Value = objSerialNumberDomain.SerialNumberId.ToString();
                    btnInsert.Text = "Update";

                }
                //delete 
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    ClearData();
                    bool status = SerialNumberDataProvider.Instance.Delete(Convert.ToInt32(Convert.ToString(e.CommandArgument)));
                    ((Label)Info.FindControl("lblName")).Text = "Serial Number deleted successfully";

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Serial Number Master", "Success", Convert.ToString(e.CommandArgument), "Serial Number deleted successfully");

                    Info.Visible = true;
                    BindSerialNumber();

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

        protected void grdSerialNumber_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (txtSerialNumber.Text != "")
                {
                    if (!objUser.isValidChar(txtSerialNumber.Text))
                    {
                        txtSerialNumber.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid serial number.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtSerialNumber.Text))
                    {
                        txtSerialNumber.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter email subject";
                    Error.Visible = true;
                    return;
                }

                SerialNumberDomain objSerialNumberDomain = new SerialNumberDomain();
                objSerialNumberDomain.SerialNumber = txtSerialNumber.Text.Trim();


                objSerialNumberDomain.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                if (hdnSerialNumberId.Value == "")
                {

                    objSerialNumberDomain = SerialNumberDataProvider.Instance.Insert(objSerialNumberDomain);
                    if (objSerialNumberDomain.SerialNumberId > 0)
                    {
                        ((Label)Info.FindControl("lblName")).Text = "Serial Number inserted successfully";

                        var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objSerialNumberDomain);
                        var encrypt = Encryptor.EncryptString(json);

                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objSerialNumberDomain.UpdatedBy), "Serial Number Master", "Success", Convert.ToString(objSerialNumberDomain.SerialNumberId), "Serial Number inserted successfully :- " + encrypt + "");

                        BindSerialNumber();
                        ClearData();

                        Info.Visible = true;
                    }
                    else if (objSerialNumberDomain.SerialNumberId == -1)
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objSerialNumberDomain.UpdatedBy), "Serial Number Master", "Failed", Convert.ToString(objSerialNumberDomain.SerialNumberId), "Serial Number already exist");

                        ((Label)Error.FindControl("lblError")).Text = "Serial Number already exist";
                        Error.Visible = true;
                    }
                }
                else
                {

                    objSerialNumberDomain.SerialNumberId = Convert.ToInt32(hdnSerialNumberId.Value);
                    bool status = SerialNumberDataProvider.Instance.Update(objSerialNumberDomain);

                    ((Label)Info.FindControl("lblName")).Text = "Serial Number updated successfully";

                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objSerialNumberDomain);
                    var encrypt = Encryptor.EncryptString(json);

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objSerialNumberDomain.UpdatedBy), "Serial Number Master", "Success", Convert.ToString(objSerialNumberDomain.SerialNumberId), "Serial Number updated successfully :- " + encrypt + "");

                    BindSerialNumber();
                    ClearData();
                    Info.Visible = true;
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
        /// clear form data
        /// </summary>
        private void ClearData()
        {
            txtSerialNumber.Text = "";
            hdnSerialNumberId.Value = "";
            btnInsert.Text = "Save";

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }
    }
}