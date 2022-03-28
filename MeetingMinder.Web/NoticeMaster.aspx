﻿<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="NoticeMaster.aspx.cs" Inherits="MeetingMinder.Web.NoticeMaster" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <%-- <script type="text/javascript">
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
     <script language="javascript" type="text/javascript">

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
    </script>--%>
  <%--  <article class="content-box minimizer">
			<header>			
				<h2>Notice Details</h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#AE432E"><b>Notice list</b></font></legend>
                            <dl>
                             <div >
                                  <asp:UpdatePanel id="ResultsUpdatePanel" runat="server">   
                                 <ContentTemplate>

                                    <div style="text-align:center;">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel" DynamicLayout="true">
                        <ProgressTemplate>

                           <img src="img/jquery/ajaxLoader.gif" alt="">

                        </ProgressTemplate>
                    </asp:UpdateProgress>
                               
                             </div>             
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                 
                             <div style="margin-bottom:15px"><asp:Label ID="lblTotalRecord" runat="server" Font-Bold="True"></asp:Label>
                               </div>  
                           
                                                      
                              <div style="margin-bottom:15px">
                              
                                 <asp:GridView ID="grdNotice" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="NoticeId" OnRowDeleting="grdNotice_RowDeleting"
                                 
                                    OnPageIndexChanging="grdNotice_PageIndexChanging" 
                                    onsorting="grdNotice_Sorting" onrowcommand="grdNotice_RowCommand" 
                                    onrowediting="grdNotice_RowEditing">
                                  <HeaderStyle HorizontalAlign="Center" BackColor="#AE432E" ForeColor="#F2DEDA" Font-Bold="true" />
                                  <Columns>

                              

                                  
                                    <asp:TemplateField HeaderText="Sr. No." HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Meeting" SortExpression="MeetingVenue" >
                                        <ItemTemplate>
                                        <asp:Label ID="lblMeeting"  style="text-align:justify" Width="300px"  runat="server" Text='<%# Eval("MeetingDate", "{0:dd/MM/yyyy}" ) +" "+ Eval("MeetingTime") +" "+ Eval("MeetingVenue") %>' />
                                           <asp:Label ID="lblMeet" runat="server" Visible="false" Text='<%# Eval("MeetingDate") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Notice" SortExpression="NoticeMessage">
                                        <ItemTemplate>
                                        <asp:Label  style="text-align:justify" Width="300px"  ID="chkEntityMaster" runat="server" Text='<%# Eval("NoticeMessage") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    
                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Eval("NoticeId")+","+Eval("EntityId")+","+Eval("ForumId")+","+Eval("MeetingDate") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Eval("NoticeId")+","+Eval("EntityId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                                 </div>

                                     <br /><br />
                                <div style="width:900px;">
                                 <table width="75%">

                                    <tr style="display:none">
                                     <td style="width:95px;">
                                    Entity
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList Visible="false" ID="ddlEntity" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Entity" Text="Please Select Entity" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>
                                     <tr>
                                     <td style="width:95px;">
                                    Forum
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlForum" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlForum_SelectedIndexChanged"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Forum" Text="Please Select Forum" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>
                                     <tr>
                                     <td style="width:95px;">
                                    Meeting 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                         <asp:DropDownList ID="ddlMeeting" runat="server" Width="50%"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Meeting" Text="Please Select Meeting" ForeColor="Red"></asp:RequiredFieldValidator>
                                 
                                                      </td>
                                     </tr>

                                       <tr>
                                     <td style="width:95px;">
                                   Notice
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                    <asp:TextBox ID="txtNotice" Columns="23" runat="server" TextMode="MultiLine" ></asp:TextBox>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvNotice" ControlToValidate="txtNotice" Display="Dynamic" ValidationGroup="a" ErrorMessage="Uplooad Minutes" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </td>
                                     </tr>
                             
                                 <tr>
                                     <td style="width:95px;">
                                    Checker
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="center">
                                    <asp:DropDownList ID="ddlUser" runat="server"  Width="50%"></asp:DropDownList>
                                      <asp:RequiredFieldValidator ID="rfvddUser" runat="server" 
                                        class="invalid-side-note" ControlToValidate="ddlUser" Display="Dynamic" InitialValue="0"
                                        ErrorMessage="Select Checker" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                     </td>
                                     </tr>

                                     <tr align="right">
                                        <td colspan="5" align="right"> 
                                        <asp:HiddenField ID="hdnNotice" runat="server" />
                                        <asp:Button ID="btnSubmit" CausesValidation="true" ValidationGroup="a" runat="server" Text="Submit" onclick="btnSubmit_Click"></asp:Button> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click"></asp:Button>
                                 </td>
                                 </tr>
                                 </table>
                                 </div>
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />

                         
                               <asp:PostBackTrigger ControlID="btnSubmit" />
                               <asp:PostBackTrigger ControlID="btnCancel" />
                                   
    </Triggers>
</asp:UpdatePanel>
                               </div>
                               <br />
						</fieldset>
                             										
									
				</div>					
				</div>
			</section>			
		</article>--%>
</asp:Content>
