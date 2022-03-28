<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true"
    CodeBehind="AccessRightsMaster.aspx.cs" Inherits="MeetingMinder.Web.AccessRightsMaster" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <script type = "text/javascript">
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
    </script> 
    <article class="content-box minimizer">
			<header>			
				<h2>Access Rights</h2>			
			</header>
			<section>              
				<div >
                  <div class="table-form">
						<fieldset>
							<legend><font color="#AE432E"><b>Set Access Rights</b></font></legend>
                             <dl>
                                <div style="margin-bottom:15px"><userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                <div style="margin-bottom:15px">
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                </div>
                               
                               <div>
                               <asp:GridView ID="grdAccessRight" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="AccessRightId" OnRowDeleting="grdAccessRight_RowDeleting"
                                 
                                    OnPageIndexChanging="grdAccessRight_PageIndexChanging" 
                                    onsorting="grdAccessRight_Sorting" onrowcommand="grdAccessRight_RowCommand" 
                                    onrowediting="grdAccessRight_RowEditing" 
                                       onprerender="grdAccessRight_PreRender" 
                                       onrowdatabound="grdAccessRight_RowDataBound">
                                    <PagerStyle CssClass="paginate_active"   />
                                  <HeaderStyle HorizontalAlign="Center" BackColor="#AE432E" ForeColor="#F2DEDA" Font-Bold="true" />
                                  <Columns>

                                    <asp:TemplateField>
                                        <ItemStyle  HorizontalAlign="Center" />
                                        <HeaderStyle  HorizontalAlign="Center" />
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkHeader" onclick="javascript: fn_select_all(this);" runat="server" />
                                         </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSubAdmin" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="User Name" SortExpression="UserName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--  <asp:TemplateField HeaderText="Entity Name" SortExpression="EntityName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEntityName" runat="server" Text='<%# Eval("EntityName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Read" SortExpression="IsRead">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkIsRead" Checked='<%# Eval("IsRead") %>' Enabled="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Add" SortExpression="IsAdd">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkIsAdd" Checked='<%# Eval("IsAdd") %>' Enabled="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Modify" SortExpression="IsUpdate">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkIsUpdate" Checked='<%# Eval("IsUpdate") %>' Enabled="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Delete" SortExpression="IsDelete">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkIsDelete" Checked='<%# Eval("IsDelete") %>' Enabled="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                     <asp:TemplateField HeaderText="Entity" >
                                        <ItemTemplate>
                                           <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("UserId") %>' />
                                             <%--<asp:Label ID="lbltoggel" runat="server" >- Hide Orders</asp:Label>--%>
                                             <asp:DataGrid  ID="grdAccessRightN" runat="server"  DataKeyField="AccessRightId" AutoGenerateColumns="false"
                                  style="display:block;"  ItemStyle-Width="100" HeaderStyle-Width="100" CellPadding="10" CellSpacing="10" >
                                
                              
                                    <Columns>

                                 <asp:TemplateColumn HeaderText="Read">
                                        <ItemTemplate>
                                         <input id="chkIsRead" disabled="disabled" checked='<%# Eval("IsRead") %>' type="checkbox" />
                                             <%--<asp:CheckBox ID="chkIsRead" Checked='<%# Eval("IsRead") %>' Enabled="false" runat="server" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>

                                        <asp:TemplateColumn HeaderText="Add" >
                                        <ItemTemplate>
                                         <input id="chkIsAdd"  disabled="disabled" checked='<%# Eval("IsAdd") %>' type="checkbox" />

                                             <%--<asp:CheckBox ID="chkIsAdd" Checked='<%# Eval("IsAdd") %>' Enabled="false" runat="server" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>

                                          <asp:TemplateColumn HeaderText="Modify" >
                                        <ItemTemplate>
                                         <input id="chkIsUpdate"  disabled="disabled" checked='<%# Eval("IsUpdate") %>' type="checkbox" />
                                          <%--   <asp:CheckBox ID="chkIsUpdate" Checked='<%# Eval("IsUpdate") %>' Enabled="false" runat="server" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>

                                         <asp:TemplateColumn HeaderText="Delete" SortExpression="IsDelete">
                                        <ItemTemplate>
                                      <%--  <%# Eval("IsDelete") %>--%>
                                       <input id="chkIsDelete"  disabled="disabled" checked='<%# Eval("IsDelete") %>' type="checkbox" />
                                        <%-- <asp:Label ID="chkIsDelete" Text='<%# Eval("IsDelete") %>' Enabled="false" runat="server" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>

                                  </Columns>
                                    </asp:DataGrid>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    


                                 

                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         
                                            <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Bind("UserId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Bind("AccessRightId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                               </div>
                               <br />
                                <div style="width:900px;">
                                 <table width="75%">
                                 <tr>
                                 <td style="width:95px;">
                                 Entity
                                 </td>
                                 <td style="width:20">
                                 :
                                 </td>
                                 <td style="width:250px;" align="left">
                                 <asp:ListBox ID="lstEntity" Width="260px" SelectionMode="Multiple" runat="server"></asp:ListBox>
                                  <asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="lstEntity" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Entity"  Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>

                                 </td>
                                 </tr>
                                     <tr>
                                     <td style="width:95px;">
                                 Users
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlUser"  Width="260px" runat="server"></asp:DropDownList>
                                  <asp:RequiredFieldValidator runat="server" ID="rfvUser" ControlToValidate="ddlUser" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select User" InitialValue="0" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </td>
                                     </tr>
                                   
                               <tr>
                               <td style="width:95px;">Access Rights</td>
                               <td width="20px">:</td>
                               <td style="width:250px;" align="left">
                               <asp:CheckBox ID="chkIsRead" Text="Read" runat="server"></asp:CheckBox>
                               <asp:CheckBox ID="chkIsAdd" Text="Add" runat="server"></asp:CheckBox>
                               <asp:CheckBox ID="chkIsUpdate" Text="Modify" runat="server"></asp:CheckBox>
                               <asp:CheckBox ID="chkIsDelete" Text="Delete" runat="server"></asp:CheckBox>
                               <asp:CustomValidator ID="CustomAccess" runat="server" ErrorMessage="Select Access rights" Display="Dynamic" ValidationGroup="a" ForeColor="Red" Text="Invalid" ClientValidationFunction = "ValidateCheckBox"></asp:CustomValidator>
                               </td>
                               </tr>
                                     <tr align="right">
                                        <td colspan="5" align="right"> 
                                        <asp:Button  ID="btnSubmit" CssClass="btnSave" CausesValidation="true" ValidationGroup="a" runat="server" Text="Submit" onclick="btnSubmit_Click"></asp:Button> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" CssClass="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click"></asp:Button>
                                 </td>
                                 </tr>
                                
                                 </table>
                               </div>
                               <br />
						</fieldset>
                             										
					</div>									
				</div>					
				
			</section>			
		</article>
    <div class="clearfix">
    </div>
</asp:Content>
