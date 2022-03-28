<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AgendaAccess.aspx.cs" Inherits="MeetingMinder.Web.AgendaAccess" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script type="text/javascript">
      
       function SaveAccess(obj) {
           var agendaIds = [];
           if ($("#hdnAgenda").val() != "") {
               agendaIds = $("#hdnAgenda").val().split(",");
           }
           if (obj.checked) {
               $('#' + obj.id + ' input[type="checkbox"]').each(function (i, el) {
                   agendaIds.push(el.id);
               });
            //   agendaIds.push(obj.id);
           }
           else {
               $('#' + obj.id + ' input[type="checkbox"]').each(function (i, el) {
                   agendaIds.splice(agendaIds.indexOf(el.id),1);
               });
              // agendaIds.splice(agendaIds.indexOf(obj.id), 1);
           }

           $("#hdnAgenda").val(agendaIds);
       }
   </script>

    <style type="text/css">
          li {
            list-style:none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <article class="content-box minimizer">
			<header>			
				<h2><%--Agenda Access--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>Agenda Access</b></font></legend>
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
                                    Department <label  style="display:inline;margin:-2px;"><span>&nbsp;*</span></label>
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList Visible="false" ID="ddlEntity" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Department" ForeColor="Red">Invalid</asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>

                                     <tr>
                                     <td style="width:95px;">
                                    User  <label  style="display:inline;margin:-2px;"><span>&nbsp;*</span></label>
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlUser" runat="server" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged" AutoPostBack="true" Width="50%" ></asp:DropDownList> <%-- onselectedindexchanged="ddlForum_SelectedIndexChanged"--%>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvUsers" ControlToValidate="ddlUser" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="User"  ForeColor="Red">Invalid</asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>
                                     <tr>
                                     <td style="width:95px;">
                                    Forum <label  style="display:inline;margin:-2px;"><span>&nbsp;*</span></label>
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlForum" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlForum_SelectedIndexChanged"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Forum"   ForeColor="Red">Invalid</asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>
                                     <tr>
                                     <td style="width:95px;">
                                    Meeting <label  style="display:inline;margin:-2px;"><span>&nbsp;*</span></label>
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                         <asp:DropDownList ID="ddlMeeting" onselectedindexchanged="ddlMeeting_SelectedIndexChanged" AutoPostBack="true"  runat="server" Width="50%"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Meeting"  ForeColor="Red">Invalid</asp:RequiredFieldValidator>
                                 
                                                   </td>
                                     </tr>
                               
                                 </table>
                                 <br />
                                    <div style="margin-bottom:15px">
                                 <asp:Label Font-Names="Rupee" CssClass="agenda" ID="lblList" runat="server"></asp:Label>
                            <div class="fullwidth noBorder">
                                 <asp:Button CssClass="btnSave" Width="170px" OnClick="btnSave_Click" id="btnSave" ValidationGroup="a"  runat="server" text="Save Agenda Access" />
                                 </div>
                                 </div>
                                 </div>
                                        <asp:HiddenField ID="hdnAgenda" runat="server" ClientIDMode="Static" />
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />
                               <asp:PostBackTrigger ControlID="btnSave" />
    </Triggers>
</asp:UpdatePanel>
                              
                               </div>
                                 </dl>
                               <br />
						</fieldset>
                             										
									
				</div>					
				
			</section>			
		</article>
    <div class="clearfix">
    </div>
</asp:Content>
