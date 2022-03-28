<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="DepartmentMaster.aspx.cs" Inherits="MeetingMinder.Web.EntityMaster" %>

<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            $(document).bind('drop dragover', function (e) {
                e.preventDefault();
            });

            if ($('#<%=hdnEntityId.ClientID %>').val().length > 10) {
                var id = '<%= divdetails.ClientID %>';
                $("html, body").animate({
                    scrollTop: $('#' + id
                        ).offset().top
                }, 1500);
            }
        });

        function fn_select_all(chkSelectAll) {
            var IsChecked = chkSelectAll.checked;
            var items = document.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i] != chkSelectAll && items[i].type == "checkbox") {
                    if (items[i].checked != IsChecked) {
                        items[i].click();
                    }
                }
            }
        }

        function testCheckbox() {
            Parent = document.getElementById('cpMain_grdOrder');
            var headchk = document.getElementById('cpMain_grdOrder_chkHeader');
            var items = Parent.getElementsByTagName('input');
            var flg = false;
            for (i = 0; i < items.length; i++) {

                if (items[i] != headchk && items[i].type == "checkbox") {

                    if (items[i].checked) {
                        flg = true;
                    }
                }
            }
            if (flg) {
                var ans = confirm("Are you sure you want to delete selected items?");
                if (ans == true) {
                    return true;
                }
                else {
                    //alert("no");
                    return false;
                }
            }
            else {
                alert("Select item(s) to delete");
                return false;
            }
        }

        function showChecker() {
            if (document.getElementById('chkChecker').checked) {
                $("#divEntity").attr("style", "display:block");
                var validatorObject = document.getElementById('<%=rfvddUser.ClientID%>');
                validatorObject.enabled = true;
                validatorObject.isvalid = true;
                ValidatorUpdateDisplay(validatorObject);
            }
            else {
                $("#divEntity").attr("style", "display:none");
                var validatorObject = document.getElementById('<%=rfvddUser.ClientID%>');
                validatorObject.enabled = false;
            }
        }
    </script>
    <article class="content-box minimizer">
			<header>			
				<h2>  &nbsp;</h2>			
			</header>
			<section class="master-list">              
				<div >
                <fieldset>
                <legend><font color="#054a7f"><b>Department List</b></font></legend>
                            <dl>
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                 
                              <div class="remove-records" style="margin-bottom:15px">
                                                    <label><strong>Note: Switch to respective department  for Edit/Delete or adding forums in other than <%= Session["EntityName"].ToString() %> </strong></label> 
                                  <%--  <img width="16" height="16" src="img/icons/icon_list_style_cross.png" alt="" style="vertical-align: middle"/> 
                                    <asp:LinkButton runat="server" ID="lbRemoveSelected" Text="Remove All Selected Record" 
                                    OnClientClick="return confirm('Are you sure you want to delete selected items?');" onclick="lbRemoveSelected_Click"  ></asp:LinkButton>--%>
                               </div>
                                       <div class="box_top">
		
		
<h2 class="icon users">Department </h2>
		
	
</div>               
                              <div style="margin-bottom:15px">
                                  	<div class="dataTables_wrapper">
                                         <asp:GridView ID="grdEntity" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                   PageSize="10" DataKeyNames="EntityId" OnRowDeleting="grdEntity_RowDeleting"
                                 
                                    OnPageIndexChanging="grdEntity_PageIndexChanging" 
                                    onsorting="grdEntity_Sorting" onrowcommand="grdEntity_RowCommand" 
                                    onrowediting="grdEntity_RowEditing">
                                  <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                              <PagerStyle CssClass="paginate_active"   />
                                  <Columns>

                                       <%-- <asp:TemplateField>
                                        <ItemStyle  HorizontalAlign="Center" />
                                        <HeaderStyle  HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                            <asp:CheckBox ID="chkHeader" onclick="javascript: fn_select_all(this);" runat="server" />
                                         </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSubAdmin" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                  
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                     <%-- <asp:TemplateField HeaderText="Logo">
                                        <ItemTemplate>
                                                <img id="imgLogo" height="50px" width="100px" alt="logo" src="img/Uploads/EntityLogo/<%# Eval("EntityLogo") %>" />
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>--%>
                                
                                    <asp:TemplateField HeaderText="Name" SortExpression="EntityName">
                                  
                                        <ItemTemplate>
                                           <asp:Label ID="lblEncrptionkey" runat="server" Visible="false" Text='<%#Eval("EncryptionKey") %>'></asp:Label>
                                            <asp:Label ID="lblEntityName" runat="server" Text='<%# Eval("EntityName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Forums">
                <ItemStyle HorizontalAlign="Center" Width="10%" />
                <ItemTemplate>
                <asp:LinkButton ID="lbnState" CssClass="AddView" Visible='<%# Eval("EntityId").ToString().ToLower().Equals(Session["EntityId"].ToString().ToLower()) %>' CommandArgument='<%# Bind("EntityId") %>' CausesValidation="false" CommandName="View" Text="Add/View" runat="server" ToolTip="Add OR View Forums"></asp:LinkButton>
                    
                </ItemTemplate>
            </asp:TemplateField>


                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                      <asp:ImageButton ID="lbtnEdit"  Visible='<%# Eval("EntityId").ToString().ToLower().Equals(Session["EntityId"].ToString().ToLower()) %>'  CommandArgument='<%# Bind("EntityId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                        <asp:ImageButton ID="lbtnDelete"  Visible='<%# Convert.ToBoolean(Session["IsSuperAdmin"].ToString())== true && Eval("EntityId").ToString().ToLower().Equals(Session["EntityId"].ToString().ToLower()) %>'  CommandArgument='<%# Bind("EntityId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' /> 
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                                          </div>
                            </div>
                            </dl>
                        
                             
                     <div ID="divdetails" visible="false" runat="server" style="margin-bottom:15px">
                    <div class="fullwidth">

<div class="infoPersonal">
                       <dt>
                                    <label>
                                      Full Name <span>*</span> :
                                    </label>
                                </dt>

                                <dd>
                                    <asp:TextBox ID="txtEntityName" runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvUser" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtEntityName" Display="Dynamic" 
                                        ErrorMessage="Entity Full Name" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>
                                </div>
                               
                               </div>
<div class="fullwidth">

<div class="infoPersonal">
                                   <dt>
                                    <label>
                                      Short Name <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtShortName" runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvShortName" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtShortName" Display="Dynamic" 
                                        ErrorMessage="Entity Short Name" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>
                                </div>
                                </div>
                                <div class="fullwidth">

<div class="infoPersonal">

                                   <dt style="display:none">
                                    <label>
                                    Enable <span>*</span> :
                                    </label>
                                </dt>
                                <dd style="display:none">
                                 <asp:CheckBox ID="chkEnable" runat="server" Checked="false" />
                                  
                                </dd>
                                </div>
                                </div>

                                <div class="fullwidth">
                                     <%--style="display:none;"--%>
<div class="infoPersonal" style="display:none;">
                                   <dt>
                                    <label>
                                      Logo <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                 <asp:FileUpload style="width:230px" runat="server" ID="fuEntityLogo" />
                                 
                                   <asp:LinkButton ID="lnkView" runat="server" Visible="false" Text="View" onclick="lnkView_Click"></asp:LinkButton>
                                 
                                      <asp:RequiredFieldValidator ID="rfvEntityLogo" runat="server"  Enabled="false"
                                        class="invalid-side-note" ControlToValidate="fuEntityLogo" Display="Dynamic" 
                                        ErrorMessage="Entity Logo" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                              
                                </dd>

                                 </div>
                                 </div>            
                                 <div class="fullwidth">

<div class="infoPersonal">
                                   <dt>
                                    <label>
                                  User Guide-iPad App  :
                                    </label>
                                </dt>
                                <dd>
                            
                              <asp:FileUpload style="width:230px" runat="server" ID="fuMeeting" />
                                 <%--<label>--%><span>Pdf file only &nbsp;</span><%--</label>--%>
                                   <asp:LinkButton ID="lnkViewMeeting" runat="server" Visible="false" Text="View" onclick="lnkViewMeeting_Click"></asp:LinkButton>
                                    &nbsp; &nbsp;    <asp:LinkButton ID="lnkRemoveFile" runat="server" Visible="false" Text="Delete" onclick="lnkRemoveFile_Click"></asp:LinkButton>
                                 
                                </dd>
                                </div>
                                </div>
                                <div class="fullwidth">

<div class="infoPersonal" style="display:none;">
                                  <dt>
                                    <label>
                                    Send to Checker  :
                                    </label>
                                </dt>
                                <dd>
                                <asp:CheckBox Visible="false" onclick="showChecker();" ID="chkChecker" runat="server" ClientIDMode="Static" />
                                </dd>
                                </div>
                                </div>
                                <div class="fullwidth">
<div class="infoPersonal"  style="display:none;">
                                <div id="divEntity" style="display:none">
                                <dt>
                                    <label>
                                      Checker <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                              <asp:DropDownList style="width:230px" ID="ddlUser" runat="server" ></asp:DropDownList>
                                      <asp:RequiredFieldValidator ID="rfvddUser" runat="server" Enabled="false" 
                                        class="invalid-side-note" ControlToValidate="ddlUser" Display="Dynamic" InitialValue="0"
                                        ErrorMessage="Select Checker" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                              
                                </dd>
                                </div>
                                </div>
                                </div>
                                     <dl>
                               
                                      <asp:HiddenField ID="hdnLogo" runat="server" />
                                    </dl>
                                    <div id="divbutton" runat="server"  class="fullwidth noBorder">
                                    <asp:Button ID="btnInsert" CssClass="btnSave" runat="server" Text="Save" ValidationGroup="a" onclick="btnInsert_Click1"  />
                                       <asp:Button ID="btnCancel" CssClass="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click1"  />
                                    </div>
                                    </div>
                        </fieldset>
                                           <asp:HiddenField ID="hdnEntityId" runat="server" />
                                </div>
                                </section>
                                </article>
    <%--     <div class="clearfix">
    </div>--%>
</asp:Content>
