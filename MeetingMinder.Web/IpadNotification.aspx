<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="IpadNotification.aspx.cs" Inherits="MeetingMinder.Web.IpadNotification" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

      function CharsCount(vals) {
                
          var len = vals.length;
          if (len > 0) {
              var charLeft = 150 - len;
                $("#lblChars").attr("style","display:block");
              $("#lblChars").html(charLeft + " character remaining");
          }
          else
          {
           $("#lblChars").attr("style","display:none");
          }
      }
      </script>

<article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Ipad Notification--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>Ipad Notification</b></font></legend>
                             <dl>
                             <div style="width:900px;">
                                  <asp:UpdatePanel id="ResultsUpdatePanel" runat="server">   
                                 <ContentTemplate>

                                    <div style="text-align:center;">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel" DynamicLayout="true">
                        <ProgressTemplate>

                           <img src="img/jquery/ajaxLoader.gif">

                        </ProgressTemplate>
                    </asp:UpdateProgress>
                               
                             </div>                 

                                <div style="margin-bottom:15px"><userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                <div style="margin-bottom:15px">
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                </div>
                           
                              <br /><br />
                                <div style="width:900px;">
                                 <table width="75%">

                                    <tr style="display:none;">
                                     <td style="width:95px;">
                                    Department
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlEntity" Visible="false" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Department" Text="Please Select Department" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
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
                                     <asp:DropDownList ID="ddlForum" runat="server"  Width="50%" 
                                          onselectedindexchanged="ddlForum_SelectedIndexChanged" AutoPostBack="true"   ></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Forum" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>
                                     

                                     <tr>
                                     <td>User list</td>
                                       <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                          <div class="box_top">
		
		
<h2 class="icon users">User List </h2>
		
	
</div>
                                       
                                     <asp:GridView ID="grdReport" ShowHeader="true" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" DataKeyNames ="UserId"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red"  EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable">
                                <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                          <PagerStyle CssClass="paginate_active"   />
                                  <Columns>
                                  
                                          <asp:TemplateField>
                                       
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkHeader" onclick="javascript: fn_select_all(this);" runat="server" />
                                         </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSubAdmin" runat="server" Enabled='<%# Convert.ToBoolean(Eval("DeviceToken").ToString().Length > 0 ? "true": "false") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Name" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("Suffix") +" "+Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                            <asp:HiddenField ID="hdnUdID" runat="server" Value='<%# Eval("DeviceToken") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>                              
                                       
                                            <asp:TemplateField  HeaderText="Designation" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                                                    
                                 </Columns>
                                </asp:GridView>
                                     </td>
                                     </tr>
                                       
                                       <tr>
                                     <td style="width:95px;">
                                    Message
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                       <asp:TextBox ID="txtMessage" runat="server"  MaxLength="150"  onKeyUp="CharsCount(this.value);" Width="214px"></asp:TextBox>
                                       <br /><label id="lblChars" ></label>
 <asp:RequiredFieldValidator ID="rfvMMessage" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtMessage" Display="Dynamic" 
                                        ErrorMessage="Message" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                          
                                     
                                 
                                     </td>
                                     </tr>
                                        
                                              <tr>
                                     <td colspan="3">
                                     <div class="fullwidth noBorder">
<asp:Button ID="btnSend" CssClass="btnSave" runat="server" ValidationGroup="a" Text="Send" onclick="btnSend_Click"></asp:Button>
</div>
                                    </td>
                                     </tr>            
                                 </table>
                                 </div>
                                             </ContentTemplate>
                                    <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSend" />
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />
                               

                              <%-- <asp:PostBackTrigger ControlID="btnSubmit" />
                               <asp:PostBackTrigger ControlID="btnCancel" />
                             --%>
                         
        
    </Triggers>
</asp:UpdatePanel>
                               </div>
                               <br />
						</fieldset>
                             										
					
				</div>					
				
			</section>			
		</article>
    <div class="clearfix">
    </div>
</asp:Content>
