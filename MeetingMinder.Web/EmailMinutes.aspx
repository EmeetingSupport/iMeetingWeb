<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="EmailMinutes.aspx.cs" Inherits="MeetingMinder.Web.EmailMinutes" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
          <style type="text/css">
 /*       .agenda h3
        {
            color: #666666 !important;
        }
        
        .agenda h3 a
        {
            color: Blue;
        }
        
        .agenda a 
        {
            color: Blue;
        }
              span
              {
                  font-style :italic;
                  margin-left: 60px;
              }
              */
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Email Minutes--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>Email Minutes</b></font></legend>
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
                                         <asp:DropDownList ID="ddlMeeting" onselectedindexchanged="ddlMeeting_SelectedIndexChanged" AutoPostBack="true"  runat="server" Width="50%"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Meeting" Text="Please Select Meeting" ForeColor="Red"></asp:RequiredFieldValidator>
                                 
                                                   </td>
                                     </tr>
                               <tr>
                                      <td style="width:95px;">
                                    To 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                         
                                     <asp:TextBox ID="txtTo" runat="server" ></asp:TextBox>
                                             <asp:RequiredFieldValidator runat="server" ID="rfvTo" ControlToValidate="txtTo" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="To" Text="To" ForeColor="Red"></asp:RequiredFieldValidator>
                               
                                     </td>
                               </tr>
                                       <tr>
                                      <td style="width:95px;">
                                    CC 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                         
                                     <asp:TextBox ID="txtCC" runat="server" ></asp:TextBox>
                                        
                                     </td>
                               </tr>

                  <tr>
                                      <td style="width:95px;">
                                    Body
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                         
                                              <CKEditor:CKEditorControl ID="txtEmail" ClientIDMode="Static" runat="server" Height="150px" Width="600px" ></CKEditor:CKEditorControl>                              
      
                                        
                                     </td>
                               </tr>
                                     <tr>
                                         <td colspan="3"><asp:Button ID="btnSend" runat="server" Text="Send Email" OnClick="btnSend_Click" /></td>
                                         
                                     </tr>

                                 </table>
                                 <br />
                                    <asp:HiddenField ID="hdnMeeting" runat="server" ClientIDMode="Static" />
                                    <div style="margin-bottom:15px">

                                      
                                 <asp:Label CssClass="agenda" ID="lblList" runat="server"></asp:Label>
                             <%--<div class="fullwidth noBorder">
                                 <asp:Button CssClass="btnSave" style="width:120px" id="btnPrint" visible="false" runat="server" onclientclick="javascript:CallPrint();" text="Print Agenda" xmlns:asp="#unknown" />
                                 </div>--%>
                                 </div>
                                 </div>
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />

                                 <%--<asp:PostBackTrigger ControlID="lnkView" />--%>


                       
                         
        
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
