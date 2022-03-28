<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="RevisedPdf.aspx.cs" Inherits="MeetingMinder.Web.RevisedPdf" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .agenda h3 {
            color: #666666 !important;
        }

            .agenda h3 a {
                color: Blue;
            }

        .agenda a {
            color: Blue;
        }

        .revised table.datatable tr td {
            text-align: justify;
            word-break: normal;
        }

        @font-face {
            font-family: Rupee;
            src: url("fonts/Rupee_Foradian.eot") /*  IE 8 */;
        }

        @font-face {
            font-family: Rupee;
            src: url("fonts/Rupee_Foradian.ttf") /* CSS3 supported browsers */;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function CallPrint() {

            //  document.getElementById('<%= lblList.ClientID %>').style.textDecoration = 'none';
            var div = document.getElementById('<%= lblList.ClientID %>');
            var anchorList = div.getElementsByTagName('a');

            for (var i = 0; i < anchorList.length; i++) {
                anchorList[i].style.textDecoration = 'none';
                anchorList[i].style.color = 'black';
            }

            var prtContent = document.getElementById('<%= lblList.ClientID %>');
            var Forum = document.getElementById('<%= ddlForum.ClientID %>');
            var ForumName = Forum.options[Forum.selectedIndex].text;

            var Meeting = document.getElementById('<%= ddlMeeting.ClientID %>');
            var MeetingDate = Meeting.options[Meeting.selectedIndex].text;
            var cont = '<div align="center"><h2>Agenda<h2></div><br> For <b>' + ForumName + '</b> on <b>' + MeetingDate + '</b> <br/>' + prtContent.innerHTML;
            var WinPrint = window.open('', '', 'letf=0,top=0,width=800,height=400,toolbar=0,scrollbars=auto,status=0,dir=ltr');
            WinPrint.document.write(cont);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
            prtContent.innerHTML = strOldOne;
            document.getElementById('<%= lblList.ClientID %>').className = "agenda";
        }
    </script>
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Revised Agenda--%></h2>			
			</header>
			<section>              
				<div class="revised">
                 
						<fieldset>
							<legend><font color="#054a7f"><b>Revised Agenda</b></font></legend>
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
                                <div >
                                 <table width="75%">

                                    <tr style="display:none">
                                     <td style="width:95px;">
                                    Entity
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlEntity" Visible="false" runat="server" AutoPostBack="true" Width="50%" 
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
                               
                                 </table>
                                 <br />
                                    <div style="margin-bottom:15px">
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
                              

                                 <asp:Label  CssClass="agenda" ID="lblList" runat="server"></asp:Label>
                           
                                 <asp:Button id="btnPrint" visible="false" runat="server" onclientclick="javascript:CallPrint();" text="Print Agenda" xmlns:asp="#unknown" />
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
