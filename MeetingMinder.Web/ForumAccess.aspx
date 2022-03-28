<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ForumAccess.aspx.cs" Inherits="MeetingMinder.Web.ForumAccess" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .btnSearch {
            background-color: #0F5F88;
            font-size: 13px;
            width: 90px;
            text-align: center;
            background-color: #0079c2;
            color: #fff;
            padding: 6px;
            behavior: url(css/pie/PIE.htc) !important;
            cursor: pointer;
            /* float: left; */
            text-transform: uppercase;
            border: 0;
            border-radius: 0;
            -moz-border-radius: 0;
            -webkit-border-radius: 0;
        }

       .btnSearch:hover {
           background-color: #0F5F88;
       }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function ValidateCheckBox(sender, args) {
            var count = 0;
            var items = document.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].type == "checkbox") {
                    if (items[i].checked) {
                        count++;
                    }
                }
            }

            if (count == 0) {
                args.IsValid = false;
            }
            if (count > 0) {
                args.IsValid = true;
            }

        }

        function toggle(toggeldivid, toggeltext) {
            var divelement = document.getElementById(toggeldivid);
            var lbltext = document.getElementById(toggeltext);
            if (divelement.style.display == "block") {
                divelement.style.display = "none";
                lbltext.innerHTML = "+ Show Orders";
            }
            else {
                divelement.style.display = "block";
                lbltext.innerHTML = "- Hide Orders";
            }
        }



        $(document).ready(function () {
            if ($('#<%=lblFullName.ClientID %>').length > 0) {
                var id = '<%= lblFullName.ClientID %>';
                $("html, body").animate({
                    scrollTop: $('#' + id
                        ).offset().top
                }, 1500);
            }
        });
    </script>
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Access Rights--%></h2>			
			</header>
			<section>              
				<div>      
						<fieldset>
							<legend><font color="#054a7f"><b>Search User</b></font></legend>
                             <dl>
                                <div style="margin-bottom:15px">
                                <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                <div class="add_remove1" style="margin-bottom:15px">
                                    <asp:TextBox ID="txtSearch" runat="server" style="width: 230px!important;color: #000!important;" placeholder="Enter the Userid / Name"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqfvSearch" runat="server" ControlToValidate="txtSearch" ErrorMessage="Enter pf id or name" class="invalid-side-note" ValidationGroup="s" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:Button  ID="btnSearch" runat="server" Text="Search" CssClass="btnSearch" ValidationGroup="s" OnClick="btnSearch_Click" />
                                    <asp:Button  ID="btnReset" runat="server" Text="Reset" CssClass="btnSearch" Visible="false" OnClick="btnReset_Click" />

                                </div>
                       <%--          <div style="margin-bottom:15px;">
                                    <img width="16" height="16" src="img/icons/icon_list_style_cross.png" alt="" style="vertical-align: middle"/> 

                                    <asp:LinkButton  runat="server" ID="lbRemoveSelected" Text="Remove All Selected Record" 
                                    OnClientClick="return confirm('Are you sure you want to delete selected items?');" onclick="lbRemoveSelected_Click"  ></asp:LinkButton>
                               </div>--%>
                                <div style="margin-bottom:15px">
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                </div>
                               
                               <div>
     <div class="box_top">
		
		
<h2 class="icon users">Forum Access Right </h2>
		
	
</div>                    
                  <div class="box_content">
		
		<!-- Simple Sorting Table + Pagination: Start -->
		<div class="dataTables_wrapper">
                                                                 
                          <asp:GridView ID="grdUserForum" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="UserId"    onrowediting="grdUserForum_RowEditing" 
                                      OnRowDeleting="grdUserForum_RowDeleting"  onsorting="grdUserForum_Sorting" 
                                      onrowcommand="grdUserForum_RowCommand"    
                                       onrowdatabound="grdUserForum_RowDataBound" 
                                       onpageindexchanging="grdUserForum_PageIndexChanging">
                                <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                               <PagerStyle CssClass="paginate_active"   />
                                      <Columns>

                                 <%--   <asp:TemplateField>
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

                                     <asp:TemplateField HeaderText="Name"  >
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server" Text='<%#  Eval("Suffix") +" "+Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                               

                                     <asp:TemplateField HeaderText="Forums" >
                                    
                                        <ItemTemplate>
                                           <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("UserId") %>' />
                                     <asp:GridView ID="grdAccessRightN" ShowHeader="false" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" DataKeyNames ="UserId"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable">
                                  <HeaderStyle HorizontalAlign="Center" BackColor="#AE432E" ForeColor="#F2DEDA" Font-Bold="true" />
                                  <Columns>
                                  
                                      <asp:TemplateField HeaderText="Forum Name" SortExpression="ForumName">
                                        <ItemTemplate>
                                          <%# Eval("ForumName") %>    (<%# Eval("EntityName") %>)
                                        </ItemTemplate>
                                    </asp:TemplateField>
                   
                                         <asp:TemplateField Visible="false" HeaderText="Read" SortExpression="IsRead">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkIsRead" Text="Read"  runat="server" />
                                         
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField Visible="false" HeaderText="Add" SortExpression="IsAdd">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkIsAdd" Text="Add"  style="background-color:#F2F2F2;" Enabled="false" runat="server" />
                                                 <asp:HiddenField ID="hdnEntityId" Value='<%# Eval("ForumId") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                          <asp:TemplateField Visible="false" HeaderText="Modify" SortExpression="IsUpdate">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkIsUpdate" style="background-color:#F2F2F2" Enabled="false" Text="Modify" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField Visible="false" HeaderText="Delete" SortExpression="IsDelete">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkIsDelete"  style="background-color:#F2F2F2" Enabled="false" Text="Delete"  runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                    
                                 </Columns>
                                </asp:GridView>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    


                                 

                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         
                                            <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Bind("UserId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Bind("UserId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
            </div>
                      </div>
                               </div>
                               <br />
                                <div style="width:900px;">
                                 <asp:UpdatePanel id="ResultsUpdatePanel" runat="server">   
                                 <ContentTemplate>
                                    <div style="text-align:center;">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel" DynamicLayout="true">
                        <ProgressTemplate>

                           <img src="img/jquery/ajaxLoader.gif">

                        </ProgressTemplate>
                    </asp:UpdateProgress>
                                 <table width="75%">
                                                                                                                
                                               <tr runat="server" id="trDdl">
                                     <td style="width:95px;">
                                 User 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlUser" AutoPostBack="true"  Width="260px" runat="server" 
                                             onselectedindexchanged="ddlUser_SelectedIndexChanged"></asp:DropDownList>
                                  <asp:RequiredFieldValidator runat="server" ID="rfvUser" ControlToValidate="ddlUser" Display="Dynamic" ValidationGroup="a" ErrorMessage="User" InitialValue="0" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </td>
                                     </tr>
                             
                                         <tr runat="server" id="trName" visible="false">
                                     <td style="width:95px;">
                                 User 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                            <asp:Label ID="lblFullName" runat="server" ></asp:Label>   

                                     </td>
                                     </tr>

                                 <tr id="trGrid" runat="server">

                                 <td style="width:95px;">
                                 Forum 
                                 </td>
                                 <td style="width:20px">
                                 :
                                 </td>
                                 <td style="width:250px;" align="left">
                                     <asp:GridView ID="grdAccessRight" ShowHeader="false" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" DataKeyNames ="UserId"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red"  EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable">
                                  <HeaderStyle HorizontalAlign="Center" BackColor="#AE432E" ForeColor="#F2DEDA" Font-Bold="true" />
                                  <Columns>
                                  
                                      <asp:TemplateField HeaderText="Forum Name" SortExpression="ForumName">
                                        <ItemTemplate>
                                        <div  align="left">
                                         <asp:CheckBox ID="chkForum" runat="server" />
                                            <asp:Label ID="lblForumName" runat="server" Text='<%# Eval("ForumName") %>'></asp:Label>
                                            (<%# Eval("EntityName") %>)
                                            <asp:HiddenField ID="hdnForumId" runat="server" Value='<%# Eval("ForumId") %>' />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField Visible="false" HeaderText="Read" SortExpression="IsRead">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkIsRead" Text="Read"  runat="server" />
                                         
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField Visible="false" HeaderText="Add" SortExpression="IsAdd">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkIsAdd" Text="Add"  runat="server" />
                                                 <asp:HiddenField ID="hdnEntityId" Value='<%# Eval("EntityId") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                          <asp:TemplateField Visible="false" HeaderText="Modify" SortExpression="IsUpdate">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkIsUpdate" Text="Modify" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField Visible="false" HeaderText="Delete" SortExpression="IsDelete">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkIsDelete" Text="Delete"  runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                             
                                 </Columns>
                                </asp:GridView>

                                 </td>
                                 </tr>
                       
                                   
                               <tr style="display:none">
                               <td style="width:95px;">Access Right</td>
                               <td width="20px">:</td>
                               <td style="width:250px;" align="left">
                             <asp:CustomValidator ID="CustomAccess" runat="server" ErrorMessage="Select at least one checkbox" Display="Dynamic" ValidationGroup="a" ForeColor="Red" Text="Invalid" ClientValidationFunction = "ValidateCheckBox"></asp:CustomValidator>
                               </td>
                               </tr>
                                     <tr id="trBtn" runat="server" align="right">
                                        <td colspan="5" align="right"> 
                                            <div class="fullwidth noBorder">
                                        <asp:Button CssClass="btnSave"  ID="btnSubmit" ValidationGroup="a"  runat="server" Text="Submit" onclick="btnSubmit_Click"></asp:Button> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button CssClass="btnCancel" ID="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click"></asp:Button>
                                                </div>
                                 </td>
                                 </tr>
                                
                                 </table>
                              
                    
                    </div>                   
                                 </ContentTemplate>
                                    <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSubmit" />
                                    <asp:PostBackTrigger ControlID="btnCancel" />
        
    </Triggers>
</asp:UpdatePanel>
    
                               </dl>
                               </fieldset>     
                                               
                               </div>
                               <br />
			</section>			
		</article>
    <div class="clearfix">
    </div>
</asp:Content>
