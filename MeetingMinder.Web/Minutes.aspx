<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="Minutes.aspx.cs" Inherits="MeetingMinder.Web.Minutes" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type='text/css'>
        .overlay-bg {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            cursor: pointer;
            background: #000; /* fallback */
            background: rgba(0,0,0,0.75);
        }

        .overlay-content {
            background: #fff;
            padding: 1%;
            width: 700px;
            height: 400px;
            overflow: auto;
            position: relative;
            top: 15%;
            left: 30%;
            margin: 0 0 0 -10%; /* add negative left margin for half the width to center the div */
            cursor: default;
            border-radius: 4px;
            box-shadow: 0 0 5px rgba(0,0,0,0.9);
        }
    </style>



    <script type='text/javascript'>//<![CDATA[ 
        $(window).load(function () {
            $(document).ready(function () {
                // show popup when you click on the link
                //$('.show-popup').click(function (event) {
                //   $('.overlay-bg').show();
                //   alert($(this).parent().next().attr('id'));
                //   event.preventDefault(); // disable normal link function so that it doesn't refresh the page
                //    $(this).parent().next().show(); //display your popup
                //});

                // hide popup when user clicks on close button
                $('.close-btn').click(function () {
                    $('.overlay-bg').hide(); // hide the overlay
                });

                // hides the popup if user clicks anywhere outside the container
                $('.overlay-bg').click(function () {
                    $('.overlay-bg').hide();
                })
                // prevents the overlay from closing if user clicks inside the popup overlay
                $('.overlay-content').click(function () {
                    return false;
                });


            });
        });//]]>  

        function ShowPopUp(agendaId) {//(title, agendaId, minutes) {
            var title = $('#hdn' + agendaId).val();
            var minutes = $('#' + agendaId + 'hdn').val();
            if (minutes == "") {
                $("#btnDelete").attr("style", "display:none");
            }
            else {
                $("#btnDelete").attr("style", "display:inline");
            }
            $('#lblAgendaTitle').html(title);
            $('#hdnAid').val(agendaId);
            $('#txtMinutes').val(minutes);
            $('.overlay-bg').show();
        }

        function tests(tt) {
            alert(tt);
        }
        function Add() {


            var meeting = $('#hdnMeeting').val();
            var agenda = $('#hdnAid').val();;
            var minutes = $('#txtMinutes').val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Minutes.aspx/AddMinutes",
                data: "{'Meeting':'" + meeting + "','Agenda':'" + agenda + "','Minutes': '" + minutes + "'}",
                dataType: "json",

                success: function (data) {
                    alert(data.d);

                    $('#' + agenda + 'hdn').val(minutes);
                    $('.overlay-bg').hide();
                },

                error: function (result) {
                    alert(result.d);
                }
            });
        }

        function delEvent() {
            var meeting = $('#hdnMeeting').val();
            var agenda = $('#hdnAid').val();;
            var minutes = $('#txtMinutes').val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Minutes.aspx/DelMinutes",
                data: "{'AgendaId':'" + agenda + "'}",
                dataType: "json",

                success: function (data) {
                    alert(data.d);

                    $('#' + agenda + 'hdn').val('');
                    $('.overlay-bg').hide();
                },

                error: function (result) {
                    alert(result.d);
                }
            });
        }
    </script>

    <div class='overlay-bg'>
        <div class='overlay-content'>
            <h2>
                <label id="lblAgendaTitle"></label>
            </h2>
            <p>
                <asp:TextBox ID="txtMinutes" Width="630px" Height="270px" TextMode="MultiLine" runat="server" ClientIDMode="Static"></asp:TextBox>
            </p>
            <p>
                <asp:HiddenField ID="hdnAid" runat="server" ClientIDMode="Static" />
                <asp:Button Text="Save" OnClientClick="Add();" ID="btnSave" runat="server" />
                <asp:Button ID="btnClose" runat="server" CssClass="close-btn" Text="Close" />

                <asp:Button ID="btnDelete" OnClientClick="delEvent()" ClientIDMode="Static" runat="server" CssClass="close-btn" Text="Delete" />
            </p>

        </div>
    </div>



    <%--  <div class="overlay-bg">
      <div class="overlay-content">--%>
    <%-- <h2>Low Level LASER Therapy</h2>
       <p>SECOND SET OF INFORMATION DISPLAYED HERE</p>--%>

    <%--   <asp:TextBox TextMode="MultiLine" runat="server" ID="txtMom"></asp:TextBox>
       <button class="close-btn">Close</button>
     </div>
   </div>--%>

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
				<h2>&nbsp;<%--Create Minutes--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>Minutes Details</b></font></legend>
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
                               
                                 </table>
                                 <br />
                                    <asp:HiddenField ID="hdnMeeting" runat="server" ClientIDMode="Static" />
                                    <div style="margin-bottom:15px">
                                 <asp:Label CssClass="agenda" ID="lblList" runat="server"></asp:Label>
                             <div class="fullwidth noBorder">
                                 <asp:Button CssClass="btnSave" style="width:120px" id="btnPrint" visible="false" runat="server" onclientclick="javascript:CallPrint();" text="Print Agenda" xmlns:asp="#unknown" />
                                 </div>
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
