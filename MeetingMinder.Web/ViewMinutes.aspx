<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ViewMinutes.aspx.cs" Inherits="MeetingMinder.Web.ViewMinutes" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--View Minutes--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>View Minutes</b></font></legend>
                             <dl>
                             <div>
                                  <asp:UpdatePanel id="ResultsUpdatePanel" runat="server">   
                                 <ContentTemplate>

                                    <div style="text-align:center;">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel" DynamicLayout="true">
                        <ProgressTemplate>

                           <img src="img/jquery/ajaxLoader.gif" alt="Loading"/>

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
                                     <asp:DropDownList ID="ddlForum"  onselectedindexchanged="ddlForum_SelectedIndexChanged" runat="server" AutoPostBack="true" Width="50%" 
                                             ></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Forum" Text="Please Select Forum" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>

                                     <tr>
                                     <td style="width:95px;">
                                     Year 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                         <asp:DropDownList ID="ddlYear" onselectedindexchanged="ddlForum_SelectedIndexChanged" AutoPostBack="true"  runat="server" Width="50%"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvYear" ControlToValidate="ddlYear" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Year" Text="Please Select Year" ForeColor="Red"></asp:RequiredFieldValidator>
                                 
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
                                      <tr  id="trMinutes" runat="server" visible="false">
                                     <td style="width:95px;">
                                    Minutes 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                           <asp:Label ID="lblView" runat="server"  Visible="false" ></asp:Label>
                                    <asp:LinkButton  onclick="lnkView_Click" ID="lnkView" Text="View" Visible="false" runat="server"></asp:LinkButton>
                                                   </td>
                                     </tr>
                                 </table>

                                    <br />

  
                                 </div>
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />

                                 <asp:PostBackTrigger ControlID="lnkView" />


                       
                         
        
    </Triggers>
</asp:UpdatePanel>
                               </div>
                                 </dl>
                               <br />
						</fieldset>
                             										
									
				</div>					
				
			</section>			
		</article>

</asp:Content>
