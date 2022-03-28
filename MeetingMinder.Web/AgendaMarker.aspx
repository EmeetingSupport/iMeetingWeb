<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AgendaMarker.aspx.cs" Inherits="MeetingMinder.Web.AgendaMarker" %>
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
        /*.agenda h3
        {
            color: #666666 !important;
        }*/

        .agenda h3 a {
            color: Blue;
        }

        .agenda a {
            color: Blue;
        }

        #accordion h2 {
            margin-left: -20px;
        }

        #accordion li {
            margin-left: 20px;
            list-style:none;
        }
        .ddrag > li {
        font-size:20px;
        }
         h3 {
        font-size:20px;
        }
        

        /*.inddrag {
        list-style:outside none lower-alpha !important;
        }*/
        @font-face {
            font-family: Rupee;
            src: url("fonts/Rupee_Foradian.eot") /*  IE 8 */;
        }

        @font-face {
            font-family: Rupee;
            src: url("fonts/Rupee_Foradian.ttf") /* CSS3 supported browsers */;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--View Agenda--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>Agenda Marker</b></font></legend>
                             <dl>
                             <div style="width:100%;">
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
                                <div style="width:100%;">
                                    <div class="box_top">
        
        
<h2 class="icon users">View Agenda</h2>
        
    
</div>
                                 <table width="100%">

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
                               
                                 </table>
                                 <br />
                                    <div style="margin-bottom:15px;color:black;">

                                                                                <style type="text/css">
                                                                                    @font-face {
                                                                                        font-family: Rupee;
                                                                                        src: url("fonts/Rupee_Foradian.eot") /*  IE 8 */;
                                                                                    }

                                                                                    @font-face {
                                                                                        font-family: Rupee;
                                                                                        src: url("fonts/Rupee_Foradian.ttf") /* CSS3 supported browsers */;
                                                                                    }
                                                                                </style>
                                        <asp:HiddenField ID="hdnMeeting" runat="server"  ClientIDMode="Static"/>
                                 <asp:Label CssClass="agenda" Font-Names="Rupee" ID="lblList" runat="server"></asp:Label>
                             <div class="fullwidth noBorder">
                                 <asp:Button CssClass="btnSave" Visible="false" Width="110px"  Text="Mark as Read" OnClick="btnUpdateStatus_Click"  runat="server" ID="btnUpdateStatus" />
                               <%--  <asp:Button CssClass="btnSave"  id="btnPrint" width="110px" visible="false" runat="server" onclientclick="javascript:CallPrint();" text="Print Agenda " xmlns:asp="#unknown" />--%>
                                 </div>
                                 </div>
                                 </div>
                                         <asp:HiddenField ID="hdnAllAgenda" runat="server" ClientIDMode="Static" />
                                       <asp:HiddenField ID="hdnAgenda" runat="server" ClientIDMode="Static" />
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                                                
     <asp:PostBackTrigger ControlID="ddlYear" />
                                        <asp:PostBackTrigger ControlID="ddlForum" />
                              <asp:PostBackTrigger ControlID="ddlMeeting" />
                               
                              <asp:PostBackTrigger ControlID="btnUpdateStatus" />
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
