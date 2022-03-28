<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true"
    CodeBehind="ViewAgenda.aspx.cs" Inherits="MeetingMinder.Web.ViewAgenda" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        /*.agenda h3
        {
            color: #666666 !important;
        }*/

        .agenda h3 a {
            color: Blue;
            margin-left: 10px;
        }

        .agenda a {
            color: Blue;
        }

        #accordion h2 {
            margin-left: -20px;
        }

        #accordion li {
            margin-left: 20px;
            list-style: none;
        }

        .ddrag > li {
            font-size: 20px;
        }

        h3 {
            font-size: 20px;
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

        table {
            border: none !important;
        }

            table tr td, table tr th {
                border: none !important;
            }

        .tab tr td {
            border: 1px solid black !important;
        }

        .tab tr:last-child > td {
            border: 1px solid black !important;
        }

        .divPrint{
            color: black !important;
    font-family: times new roman;
    font-size: 14pt;
        }

         .divPrint table{
            color: black !important;
    font-family: times new roman;
    font-size: 14pt;
        }

            .subAgenda td{
            color: black !important;
    font-family: times new roman;
    font-size: 11pt !important;
        }

         b {
             font-weight:bold !important;
         }

         
.divPrint b {
    font-size: 14pt !important;
}
    </style>
    <script language="javascript" type="text/javascript">
        function CallPrint() {

            //  document.getElementById('<%= lblList.ClientID %>').style.textDecoration = 'none';
            var div = document.getElementById('<%= lblList.ClientID %>');
            //var anchorList = div.getElementsByTagName('a');

            //for (var i = 0; i < anchorList.length; i++) {
            //    anchorList[i].style.textDecoration = 'none';
            //    anchorList[i].style.color = 'black';
            //    anchorList[i].style = "display:none";
            //}
            $('#ContentPlaceHolder1_lblList a').css("display", "block").nextAll().remove();
            $("#<%= lblList.ClientID %> a").css("display", "none");
            $('#<%= btnPrint.ClientID %>').css("display", "none");
            var prtContent = document.getElementById('<%= lblList.ClientID %>');
           <%-- var Forum = document.getElementById('<%= ddlForum.ClientID %>');
            var ForumName = Forum.options[Forum.selectedIndex].text;

            var Meeting = document.getElementById('<%= hdnMeeting.ClientID %>').value;
            var MeetingDate = document.getElementById('<%= hdnMeetingDate.ClientID %>').value;
            var MeetingNumber = document.getElementById('<%=hdnMeetingNumber.ClientID%>').value;
            var AgendaNames = document.getElementById('<%=hdnAgendaNames.ClientID%>').value;
            var AllAgendaNames = document.getElementById('<%=hdnAllAgenda.ClientID%>').value;--%>
            
            <%-- var cont = '<style type="text/css">  #accordion li { margin-left: 20px;  list-style:none;  }   label{font-size:24px;} p{font-size:24px;}</style>' +
            '<div>' +
                '<div align="right"><h3>' + ForumName + ' Meeting No.' + MeetingNumber + '/2016-17 dated ' + MeetingDate + '</h3></div>' +
                '<div align="right"><h2 style=" text-decoration:underline;">Private & Confidential</h2></div>' +
                '<div align="center" style="font-size:42px;"> STATE BANK OF INDIA ' +
                '</div>' +
                '<div align="center">' +
                    '<p>Agenda for the meeting of the ' + ForumName + ' <br />' +
            'to be held at ' + Meeting + '</p>' +
                '</div>' +
                '<br />' +
                '<p>The Agenda items are arranged as per the following format:</p>' + AgendaNames +




                AllAgendaNames +
                '<br />' +

                '<p>Any other item with the permission of the Chair.</p><br />' +
                '<p>Central Board Secretariat<br />Corporate Centre, Mumbai <br />' + '<%= DateTime.Now.ToString("dd/MM/yyyy") %>' + '	<span style="float:right;">GM & Secretary, Central Board</span></p><br />' +
            '</div>'--%>
            var cont = '<style type="text/css"> .divPrint b {font-size: 14pt !important;}  .divPrint,.divPrint table,.subAgenda td{color:#000!important;font-family:times new roman}#accordion li{margin-left:20px;list-style:none}table tr td,table tr th{border:none!important}.tab tr:last-child>td{border:1px solid #000!important}.divPrint,.divPrint table{font-size:14pt}.subAgenda td{font-size:11pt!important}b{font-weight:700!important}</style>' + prtContent.innerHTML;

            var WinPrint = window.open('', '', 'letf=0,top=0,width=800,toolbar=0,scrollbars=auto,status=0,dir=ltr');
            WinPrint.document.write(cont);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
            prtContent.innerHTML = strOldOne;
            document.getElementById('<%= lblList.ClientID %>').className = "agenda";
            $('#<%= btnPrint.ClientID %>').css("display", "block");
            $("#<%= lblList.ClientID %> a").css("display", "block");
            
            $("<br/>").insertAfter("#ContentPlaceHolder1_lblList a");

        }
    </script>
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--View Agenda--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>View Agenda</b></font></legend>
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
                                     <asp:DropDownList ID="ddlForum" runat="server"  onselectedindexchanged="ddlForum_SelectedIndexChanged"  AutoPostBack="true" Width="50%"  
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
                                        <asp:HiddenField ID="hdnMeetingNumber" runat="server"  ClientIDMode="Static"/>
                                        <asp:HiddenField ID="hdnMeetingDate" runat="server"  ClientIDMode="Static"/>                                        
                                        <asp:HiddenField ID="hdnAgendaNames" runat="server"  ClientIDMode="Static"/>
                                        <asp:HiddenField ID="hdnAllAgenda" runat="server"  ClientIDMode="Static"/>
                                 <asp:Label CssClass="agenda" Font-Names="Rupee" ID="lblList" runat="server"></asp:Label>
                             <div class="fullwidth noBorder">
                                 <asp:Button   CssClass="btnSave" id="btnPrint" width="115px" visible="false" runat="server" onclientclick="javascript:CallPrint();" text="Print Agenda " xmlns:asp="#unknown" />
                              <asp:Button   CssClass="btnCancel" style="display:none;"   ID="btnExport" Width="130px" runat="server" visible="false" Text="Export To Doc" />
                                 </div>
                                 </div>
                                 </div>
                                     
                                      <%-- <asp:HiddenField ID="hdnMeetingDate" runat="server" ClientIDMode="Static" />--%>
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                                                
     <asp:PostBackTrigger ControlID="ddlYear" />
                                        <asp:PostBackTrigger ControlID="ddlForum" />
                              <asp:PostBackTrigger ControlID="ddlMeeting" />
                                   <asp:PostBackTrigger ControlID="btnExport" />
                               
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
