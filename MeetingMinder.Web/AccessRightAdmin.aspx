<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true"
    CodeBehind="AccessRightAdmin.aspx.cs" Inherits="MeetingMinder.Web.AccessRightAdmin" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .access td.btnCheck span {
            width: inherit;
        }

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

    <script type="text/javascript">

        function SelectCheckBox() {
          
            var numChecked = $("#<%= grdAccessRightUser.ClientID %>  [type=checkbox]:input[id*='chkSubAdmin']:checked").length;
            var numTotal = $("#<%= grdAccessRightUser.ClientID %>    [type=checkbox]:input[id*='chkSubAdmin'] ").length;
            if (numTotal == numChecked) {
                  
                  $("input[id*='chkHeader']").attr('checked', true);                
            }
            else {
                
                $("input[id*='chkHeader']").attr('checked', false);              
            }
        }


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


        function CheckCounts() {

            if ($('#<%= grdAccessRightUser.ClientID %> tr:not(:first-child) td:first-child').find('input[type="checkbox"]:checked').length != 0) {
                return confirm('Are you sure you want to delete selected items?');
            }
            else {
                alert("Please Select at least one checkbox");
                return false;
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
                                 <div class="add_remove" style="margin-bottom:15px;">
                                     <div>
                                         <img width="16" height="16" src="img/icons/icon_list_style_cross.png" alt="" style="vertical-align: middle"/> 
                                   <%--<asp:Button ID="btnT" runat="server" Text="Remove All Selected Record"  OnClientClick="return confirm('Are you sure you want to delete selected items?');" onclick="lbRemoveSelected_Click" />--%>
                                    <asp:LinkButton  runat="server" ID="lbRemoveSelected" Text="Remove All Selected Record" 
                                    OnClientClick="return CheckCounts();" onclick="lbRemoveSelected_Click"  ></asp:LinkButton>
                                   &nbsp;|&nbsp; <a href="ForumAccess.aspx">Forum Access Rights</a>
                                     </div>
                                    
                               </div>
                                <div style="margin-bottom:15px">
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                </div>
                               
                               <div>

                             <div class="box_top">
		
		
<h2 class="icon users">Access Right </h2>
		
	
</div>                  <div class="box_content">
		
		<!-- Simple Sorting Table + Pagination: Start -->
		<div class="dataTables_wrapper">
                          <asp:GridView ID="grdAccessRightUser" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    PageSize="10" DataKeyNames="UserId"    onrowediting="grdAccessRight_RowEditing" 
                                      OnRowDeleting="grdAccessRight_RowDeleting"  onsorting="grdAccessRight_Sorting" 
                                      onrowcommand="grdAccessRight_RowCommand"    
                                       onrowdatabound="grdAccessRight_RowDataBound" 
                                       onpageindexchanging="grdAccessRightUser_PageIndexChanging">
                                   <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="access odd" />
                               <PagerStyle CssClass="paginate_active"   />
                                  <Columns>

                                    <asp:TemplateField>
                                        <ItemStyle  HorizontalAlign="Center" />
                                        <HeaderStyle  HorizontalAlign="Center" />
                                        <HeaderTemplate>
                                            <asp:CheckBox class="header_chk" ID="chkHeader" onclick="javascript:fn_select_all(this);" runat="server" />
                                         </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox onchange="SelectCheckBox();"  ID="chkSubAdmin" class="sub_chk"  runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server" Text='<%#  Eval("Suffix") +" "+Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                               

                                     <asp:TemplateField HeaderText="Department" >
                                    
                                        <ItemTemplate>
                                           <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("UserId") %>' />
                                     <asp:GridView ID="grdAccessRightN" ShowHeader="false" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" DataKeyNames ="UserId"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    >
                                  <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                  <Columns>
                                  
                                      <asp:TemplateField HeaderText="Entity Name" SortExpression="EntityName">
                                        <ItemTemplate>
                                          <%# Eval("EntityName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                        <%--           <asp:TemplateField HeaderText="Read" Visible="false" SortExpression="IsRead">
                                   <ItemStyle Wrap="true" />
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkIsRead" Checked='<%# Eval("IsRead") %>' Text="Read"  runat="server" />
                                      
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Add" SortExpression="IsAdd">
                                        <ItemTemplate>
                                             <asp:CheckBox  ID="chkIsAdd" Checked='<%# Eval("IsAdd") %>' Text="Add"  runat="server" />
                                                    <asp:HiddenField ID="hdnEntityId" Value='<%# Eval("EntityId") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Modify" SortExpression="IsUpdate">
                                        <ItemTemplate>
                                             <asp:CheckBox Checked='<%# Eval("IsUpdate") %>'  ID="chkIsUpdate" Text="Modify" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Delete" SortExpression="IsDelete">
                                        <ItemTemplate>
                                             <asp:CheckBox Checked='<%# Eval("IsDelete") %>'  ID="chkIsDelete" Text="Delete"  runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>     --%>       
                                    
                                         <asp:TemplateField Visible="false" HeaderText="Read" SortExpression="IsRead">
                                        <ItemTemplate>
                                             <asp:CheckBox CssClass="btnCheck" ID="chkIsRead" Text="Read"  runat="server" />
                                         
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Add" SortExpression="IsAdd" ItemStyle-CssClass="btnCheck">
                                        <ItemTemplate>
                                             <asp:CheckBox  ID="chkIsAdd" Text="Add"  style="background-color:#F2F2F2;" Enabled="false" runat="server" />
                                                 <asp:HiddenField ID="hdnEntityId" Value='<%# Eval("EntityId") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Modify" SortExpression="IsUpdate" ItemStyle-CssClass="btnCheck">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkIsUpdate" style="background-color:#F2F2F2" Enabled="false" Text="Modify" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Delete" SortExpression="IsDelete" ItemStyle-CssClass="btnCheck">
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
                                  
                                               <tr >
                                     <td style="width:95px;">
                                 Role
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlRoll"  Width="260px" runat="server" 
                                            ></asp:DropDownList>
                                  <asp:RequiredFieldValidator runat="server" ID="rfvRoll" ControlToValidate="ddlRoll" Display="Dynamic" ValidationGroup="a" ErrorMessage="Role" InitialValue="0" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </td>
                                     </tr>
                                  
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

                                     <tr  runat="server" id="trName" visible="false">
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
                                 <%--</table>
                           



                                  <table width="75%">--%>
                                 <tr id="trGrid" runat="server">

                                 <td style="width:95px;">
                                 Department
                                 </td>
                                 <td style="width:20px">
                                 :
                                 </td>
                                 <td align="left">
                                     <asp:GridView ID="grdAccessRight" ShowHeader="false" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" DataKeyNames ="UserId"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red"  EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable">
                                  <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="access odd" />
                                  <Columns>
                                  
                                      <asp:TemplateField HeaderText="Entity Name" SortExpression="EntityName" ItemStyle-CssClass="btnCheck">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEntityName" runat="server" Text='<%# Eval("EntityName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField Visible="false" HeaderText="Read" SortExpression="IsRead" ItemStyle-CssClass="btnCheck">
                                        <ItemTemplate>
                                             <asp:CheckBox CssClass="btnCheck" ID="chkIsRead" Text="Read"  runat="server" />
                                         
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Add" SortExpression="IsAdd" ItemStyle-CssClass="btnCheck">
                                        <ItemTemplate>
                                             <asp:CheckBox CssClass="btnCheck1" ID="chkIsAdd" Text="Add"  runat="server" />
                                                 <asp:HiddenField ID="hdnEntityId" Value='<%# Eval("EntityId") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Modify" SortExpression="IsUpdate" ItemStyle-CssClass="btnCheck">
                                        <ItemTemplate>
                                             <asp:CheckBox CssClass="btnCheck1" ID="chkIsUpdate" Text="Modify" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Delete" SortExpression="IsDelete" ItemStyle-CssClass="btnCheck">
                                        <ItemTemplate>
                                             <asp:CheckBox CssClass="btnCheck1" ID="chkIsDelete" Text="Delete"  runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                             
                                 </Columns>
                                </asp:GridView>

                                 </td>
                                 </tr>
                       
                                   
                               <tr style="display:none">
                               <td style="width:95px;">Access Rights</td>
                               <td width="20px">:</td>
                               <td style="width:250px;" align="left">
                             <%--  <asp:CheckBox ID="chkIsRead" Text="Read" runat="server"></asp:CheckBox>
                               <asp:CheckBox ID="chkIsAdd" Text="Add" runat="server"></asp:CheckBox>
                               <asp:CheckBox ID="chkIsUpdate" Text="Modify" runat="server"></asp:CheckBox>
                               <asp:CheckBox ID="chkIsDelete" Text="Delete" runat="server"></asp:CheckBox>--%>
                               <asp:CustomValidator ID="CustomAccess" runat="server" ErrorMessage="Select Access rights" Display="Dynamic" ValidationGroup="a" ForeColor="Red" Text="Invalid" ClientValidationFunction = "ValidateCheckBox"></asp:CustomValidator>
                               </td>
                               </tr>
                                     <tr id="trBtn" runat="server" align="right">
                                        <td colspan="5" align="right"> 
                                        <div class="fullwidth noBorder">
                                        <asp:Button  ID="btnSubmit" CssClass="btnSave" ValidationGroup="a"  runat="server" Text="Submit" onclick="btnSubmit_Click"></asp:Button> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" CssClass="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click"></asp:Button>
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
    
                           
                               </div>
                               <br />
                               </dl>
                              
						</fieldset>     
                                                         										
							</div>					
				
			</section>			
		</article>
    <div class="clearfix">
    </div>
</asp:Content>
