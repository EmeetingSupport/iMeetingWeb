<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="Audit.aspx.cs" Inherits="MeetingMinder.Web.Audit" %>

<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .sorting th input {
            float: left;
            margin: 5px;
            display: inline-block;
        }

        th, td, caption {
            min-width: 150px;
        }

        .sorting th label {
            margin: 0px;
            font-size: 13px;
            display: inline-block;
        }


        .MainMenu a {
            color: #FFF;
        }

            .MainMenu a:active {
                color: #FF0000;
            }

            .MainMenu a:visited {
            }

            .MainMenu a:hover {
                color: #FFFF00;
            }

        .aspNetDisabled {
            
            font-size:14px !important;
        }
    </style>

    <script type="text/javascript">
        function pageLoad(sender, args) {
            window.scrollTo(0,-1 );
            var obj = $("#ddlReport").val();
            ShowDetails(obj);


            $('#txtFrom').datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                dateFormat: 'mm/yy'
            }).focus(function () {
                var thisCalendar = $(this);
                $('.ui-datepicker-calendar').detach();
                $('.ui-datepicker-close').click(function () {
                    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                    thisCalendar.datepicker('setDate', new Date(year, month, 1));
                });
            });

            $('#txtTo').datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                dateFormat: 'mm/yy'
            }).focus(function () {
                var thisCalendar = $(this);
                $('.ui-datepicker-calendar').detach();
                $('.ui-datepicker-close').click(function () {
                    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                    thisCalendar.datepicker('setDate', new Date(year, month, 1));
                });
            });

        }

       

        function validateDate(sender, args) {
            debugger;
            var objfrom = $("#txtFrom").val();
            var objTo = $("#txtTo").val();
            
            if (objfrom != "" || objTo != "") {

                if (objfrom == "")
                {
                    alert("Please select the start date.");
                    args.IsValid = false;

                    var validatorObjectReport = document.getElementById('<%=rfvTo.ClientID%>');
                    validatorObjectReport.enabled = true;
                    validatorObjectReport.isvalid = true;
                    ValidatorUpdateDisplay(validatorObjectReport);
                    return;
                }

                if (objTo == "") {
                    alert("Please select the end date.")
                    args.IsValid = false;

                    var validatorObjectReport = document.getElementById('<%=rfvTo.ClientID%>');
                    validatorObjectReport.enabled = true;
                    validatorObjectReport.isvalid = true;
                    ValidatorUpdateDisplay(validatorObjectReport);

                   
                        window.scrollTo(0, 500);
                    
                   return;
                }

                var vFrom = document.getElementById('<%=txtFrom.ClientID%>').value;
                var vTo = document.getElementById('<%=txtTo.ClientID%>').value;

                var fromMonth = vFrom.substring(0, 2);
                var fromYear = vFrom.substring(3, 7);
                var fromDate = new Date(fromYear, fromMonth, 01);

                var ToMonth = vTo.substring(0, 2);
                var ToYear = vTo.substring(3, 7);
                var ToDate = new Date(ToYear, ToMonth, 31);

                if (fromDate == '' || ToDate == '') {
                    args.IsValid = false;  // field is empty
                }
                else {

                    var fromDate = vFrom.split('/');
                    var toDate = vTo.split('/')
                    
                    if (new Date(fromDate[0] + '/01/' + fromDate[1]) > new Date(toDate[0] + '/28/' + toDate[1])) {
                        alert("End date should be greater than start date");
                        args.IsValid = false;

                        var validatorObjectReport = document.getElementById('<%=rfvTo.ClientID%>');
                        validatorObjectReport.enabled = true;
                        validatorObjectReport.isvalid = true;
                        ValidatorUpdateDisplay(validatorObjectReport);
                    }
                    else {
                        args.IsValid = true;
                        var validatorObjectReport = document.getElementById('<%=rfvTo.ClientID%>');
                        validatorObjectReport.enabled = false;
                        validatorObjectReport.isvalid = true;
                        ValidatorUpdateDisplay(validatorObjectReport);
                    }
                }

            }

            else
            {
                var validatorObjectReport = document.getElementById('<%=rfvTo.ClientID%>');
                validatorObjectReport.enabled = false;
                validatorObjectReport.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectReport);
            }
        }

        function ShowDetails(obj) {

            if (obj == "0") {
                //  $("#tblReport").css("display","none");
                $("#trForum").css("display", "none");
                $("#trMeeting1").css("display", "none");
                $("#trUser").css("display", "none");
                $("#trSerialNumber").css("display", "none");
                $("#trPresenter").css("display", "none");
                $("#trVenue").css("display", "none");
                $("#trNumber").css("display", "none");
                $("#trMeetingType").css("display", "none")
                $('#trFromDate').css("display", "none")
                $('#trToDate').css("display", "none")

                var validatorObject = document.getElementById('<%=rfvReport.ClientID%>');
                validatorObject.enabled = true;
                validatorObject.isvalid = true;
                ValidatorUpdateDisplay(validatorObject);
            }

            else if (obj == "Meeting") {
                //  $("#tblReport").css("display", "block");
                var validatorObjectReport = document.getElementById('<%=rfvReport.ClientID%>');
                validatorObjectReport.enabled = true;
                validatorObjectReport.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectReport);

                <%-- var validatorObjectForum = document.getElementById('<%=rfvForum.ClientID%>');
                validatorObjectForum.enabled = true;
                validatorObjectForum.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectForum);--%>

                <%--var validatorObjectMeeting = document.getElementById('<%=rfvMeeting.ClientID%>');
                validatorObjectMeeting.enabled = true;
                validatorObjectMeeting.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectMeeting);--%>

                <%--var validatorObject = document.getElementById('<%=rfvUser.ClientID%>');
                validatorObject.enabled = false;--%>

                $("#trForum").css("display", "");
                $("#trMeeting1").css("display", "");

                $("#trUser").css("display", "none");
                $("#trSerialNumber").css("display", "none");
                $("#trPresenter").css("display", "none");
                $("#trVenue").css("display", "");
                $("#trNumber").css("display", "");
                $("#trMeetingType").css("display", "")
                $('#trFromDate').css("display", "")
                $('#trToDate').css("display", "")
            }

            else if (obj == "Minutes") {
                //  $("#tblReport").css("display", "block");
                var validatorObjectReport = document.getElementById('<%=rfvReport.ClientID%>');
                validatorObjectReport.enabled = true;
                validatorObjectReport.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectReport);

                <%--var validatorObjectForum = document.getElementById('<%=rfvForum.ClientID%>');
                validatorObjectForum.enabled = true;
                validatorObjectForum.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectForum);--%>

                <%--var validatorObjectMeeting = document.getElementById('<%=rfvMeeting.ClientID%>');
                validatorObjectMeeting.enabled = true;
                validatorObjectMeeting.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectMeeting);--%>

               <%-- var validatorObject = document.getElementById('<%=rfvUser.ClientID%>');
                validatorObject.enabled = false;--%>

                $("#trForum").css("display", "");
                $("#trMeeting1").css("display", "");

                $("#trUser").css("display", "none");
                $("#trSerialNumber").css("display", "none");
                $("#trPresenter").css("display", "none");
                $("#trVenue").css("display", "none");
                $("#trNumber").css("display", "none");
                $("#trMeetingType").css("display", "none")
                $('#trFromDate').css("display", "")
                $('#trToDate').css("display", "")
            }
            else if (obj == "User" || obj == "Forum_Access" || obj == "Access_Rights") {

                var validatorObjectReport = document.getElementById('<%=rfvReport.ClientID%>');
                validatorObjectReport.enabled = true;
                validatorObjectReport.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectReport);

                <%--var validatorObjectForum = document.getElementById('<%=rfvForum.ClientID%>');
                validatorObjectForum.enabled = false;--%>
              

                <%--var validatorObjectUser = document.getElementById('<%=rfvUser.ClientID%>');
                validatorObjectUser.enabled = true;
                validatorObjectUser.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectUser);--%>

                <%--var validatorObjectMeeting = document.getElementById('<%=rfvMeeting.ClientID%>');
                validatorObjectMeeting.enabled = false;--%>

                $("#trUser").css("display", "");
                $("#trForum").css("display", "none");
                $("#trMeeting1").css("display", "none");
                $("#trSerialNumber").css("display", "none");
                $("#trPresenter").css("display", "none");
                $("#trVenue").css("display", "none");
                $("#trNumber").css("display", "none");
                $("#trMeetingType").css("display", "none")
                $('#trFromDate').css("display", "")
                $('#trToDate').css("display", "")

            }
            else if (obj == "Agenda" || obj == "ConsolidateAgenda") {

                var validatorObjectReport = document.getElementById('<%=rfvReport.ClientID%>');
                validatorObjectReport.enabled = true;
                validatorObjectReport.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectReport);

                <%--var validatorObjectForum = document.getElementById('<%=rfvForum.ClientID%>');
                validatorObjectForum.enabled = true;
                validatorObjectForum.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectForum);--%>

                <%--var validatorObjectUser = document.getElementById('<%=rfvUser.ClientID%>');
                validatorObjectUser.enabled = false;--%>
                

                <%--var validatorObjectMeeting = document.getElementById('<%=rfvMeeting.ClientID%>');
                validatorObjectMeeting.enabled = false;--%>

                //  $("#tblReport").css("display", "block");
                $("#trForum").css("display", "");
                $("#trMeeting1").css("display", "");
                $("#trUser").css("display", "none");
                $("#trSerialNumber").css("display", "");
                $("#trPresenter").css("display", "");
                $("#trVenue").css("display", "none");
                $("#trNumber").css("display", "none");
                $("#trMeetingType").css("display", "none")
                $('#trFromDate').css("display", "")
                $('#trToDate').css("display", "")
            }

            else if (obj == "Forum") {

                var validatorObjectReport = document.getElementById('<%=rfvReport.ClientID%>');
                validatorObjectReport.enabled = true;
                validatorObjectReport.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectReport);

                <%--var validatorObjectForum = document.getElementById('<%=rfvForum.ClientID%>');
                validatorObjectForum.enabled = true;
                validatorObjectForum.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectForum);--%>

                <%--var validatorObjectUser = document.getElementById('<%=rfvUser.ClientID%>');
                validatorObjectUser.enabled = false;--%>


                <%--var validatorObjectMeeting = document.getElementById('<%=rfvMeeting.ClientID%>');
                validatorObjectMeeting.enabled = false;--%>

                //  $("#tblReport").css("display", "block");
                $("#trForum").css("display", "");
                $("#trMeeting1").css("display", "none");
                $("#trUser").css("display", "none");
                $("#trSerialNumber").css("display", "none");
                $("#trPresenter").css("display", "none");
                $("#trVenue").css("display", "none");
                $("#trNumber").css("display", "none");
                $("#trMeetingType").css("display", "none")
                $('#trFromDate').css("display", "")
                $('#trToDate').css("display", "")
            }
            else if (obj == "ConsolidateUser") {
                var validatorObjectReport = document.getElementById('<%=rfvReport.ClientID%>');
                validatorObjectReport.enabled = false;
                // validatorObjectReport.isvalid = false;
                ValidatorUpdateDisplay(validatorObjectReport);

                <%--var validatorObjectForum = document.getElementById('<%=rfvForum.ClientID%>');
                validatorObjectForum.enabled = false;
               // validatorObjectForum.isvalid = false;
                ValidatorUpdateDisplay(validatorObjectForum);--%>

                <%--var validatorObjectUser = document.getElementById('<%=rfvUser.ClientID%>');
                validatorObjectUser.enabled = false;--%>


               <%-- var validatorObjectMeeting = document.getElementById('<%=rfvMeeting.ClientID%>');
                validatorObjectMeeting.enabled = false;--%>

                //  $("#tblReport").css("display", "block");
                $("#trForum").css("display", "none");
                $("#trMeeting1").css("display", "none");
                $("#trUser").css("display", "none");
                $("#trSerialNumber").css("display", "none");
                $("#trPresenter").css("display", "none");
                $("#trVenue").css("display", "none");
                $("#trNumber").css("display", "none");
                $("#trMeetingType").css("display", "none")
                $('#trFromDate').css("display", "")
                $('#trToDate').css("display", "")

            }
}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--About Us--%></h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#054a7f"><b>Audit Report</b></font></legend>
                            <dl>
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                         <div style="margin-bottom:15px">
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                </div>
                             
                               <div style="margin-bottom:15px"><asp:Label ID="lblTotalRecord" runat="server" Font-Bold="True"></asp:Label>
                               </div>  
                  <asp:UpdatePanel id="ResultsUpdatePanel" runat="server">   
                                 <ContentTemplate>
                                    <div style="text-align:center;">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel" DynamicLayout="true">
                        <ProgressTemplate>

                           <img src="img/jquery/ajaxLoader.gif">

                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    </div>  
                                     <div  >
                                 <table width="75%" id="tblReport">
                                            <tr>
                                     <td style="width:95px;">
                                    Report
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;">
                                     <asp:DropDownList ID="ddlReport" ClientIDMode="Static" runat="server" onchange= "ShowDetails(this.value);"  Width="50%">
                                         <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                         <%-- <asp:ListItem Text="Agenda" Value="Agenda"></asp:ListItem>--%>
                                           <asp:ListItem Text="Agenda" Value="ConsolidateAgenda"></asp:ListItem>
                                           <asp:ListItem Text="Consolidate User" Value="ConsolidateUser"></asp:ListItem>
                                      <%--    <asp:ListItem Text="Access Rights" Value="Access_Rights"></asp:ListItem>--%>
                                          <asp:ListItem Text="Forum" Value="Forum"></asp:ListItem>
                                             <asp:ListItem Text="Forum Access" Value="Forum_Access"></asp:ListItem>
                                          <asp:ListItem Text="Meeting" Value="Meeting"></asp:ListItem>
                                          <asp:ListItem Text="Minutes" Value="Minutes"></asp:ListItem>
                                         <asp:ListItem Text="User" Value="User"></asp:ListItem>
                                     </asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvReport" ControlToValidate="ddlReport" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Forum" Text="Please Select Report" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>
                              
                                     <tr id="trForum"  style="display:none;">
                                     <td style="width:95px;">
                                    Forum
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;">
                                     <asp:DropDownList ID="ddlForum" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlForum_SelectedIndexChanged"></asp:DropDownList>
                                     <%--<asp:RequiredFieldValidator Enabled="false" runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Forum" Text="Please Select Forum" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                     
                                 
                                     </td>
                                     </tr>
                                     <tr id="trMeeting1"  style="display:none;">
                                     <td style="width:95px;">
                                    Meeting 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;">
                                         <asp:DropDownList ID="ddlMeeting"   runat="server" Width="50%"></asp:DropDownList>
                                     <%--<asp:RequiredFieldValidator Enabled="false" runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Meeting" Text="Please Select Meeting" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                 
                                                   </td>
                                     </tr>
                                     <tr id="trUser" style="display:none;">

                                            <td style="width:95px;">
                                    User 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" >
                                         <asp:DropDownList ID="ddlUser"   runat="server" Width="50%"></asp:DropDownList>
                                     <%--<asp:RequiredFieldValidator Enabled="false" runat="server" ID="rfvUser" ControlToValidate="ddlUser" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select User" Text="Please Select User" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                 
                                                   </td>
                                     </tr>

                                     <tr id="trSerialNumber" style="display:none;">

                                            <td style="width:95px;">
                                    Serial Number 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" >
                                        <asp:DropDownList ID="ddlSerialNumber" ClientIDMode="Static" runat="server"  Width="50%" >
                                       <asp:ListItem Value="0" Text="Select Serial Number" ></asp:ListItem>
                                        <asp:ListItem Value="ABS" Text="ABS" ></asp:ListItem>
                                         <asp:ListItem Value="C&R" Text="C&R" ></asp:ListItem>
                                         <asp:ListItem Value="CAG" Text="CAG" ></asp:ListItem>
                                         <asp:ListItem Value="CBG" Text="CBG" ></asp:ListItem>
                                         <asp:ListItem Value="CCO" Text="CCO" ></asp:ListItem>
                                         <asp:ListItem Value="CDO" Text="CDO" ></asp:ListItem>
                                         <asp:ListItem Value="CFO" Text="CFO" ></asp:ListItem>
                                         <asp:ListItem Value="CIO" Text="CIO" ></asp:ListItem>
                                         <asp:ListItem Value="COO" Text="COO" ></asp:ListItem>
                                       <asp:ListItem Value="CRO" Text="CRO" ></asp:ListItem>
                                       <asp:ListItem Value="CS&NB" Text="CS&NB" ></asp:ListItem>
                                       <asp:ListItem Value="I&MA" Text="I&MA" ></asp:ListItem>
                                       <asp:ListItem Value="IBG" Text="IBG" ></asp:ListItem>
                                       <asp:ListItem Value="MCG" Text="MCG" ></asp:ListItem>
                                       <asp:ListItem Value="MBG" Text="MBG" ></asp:ListItem>
                                       <asp:ListItem Value="SAMG" Text="SAMG" ></asp:ListItem>
                                         
                                    </asp:DropDownList>
<%--                                     <asp:RequiredFieldValidator Enabled="false" runat="server" ID="rfvSerialNumber" ControlToValidate="ddlSerialNumber" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Seria" Text="Please Select User" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                 
                                                   </td>
                                     </tr>

                                     <tr id="trMeetingType" style="display:none;">

                                            <td style="width:95px;">Meeting Type </td>
                                            <td width="20px">: </td>
                                            <td style="width:250px;">
                                                <asp:DropDownList ID="ddlMeetingType" runat="server" ClientIDMode="Static" Width="50%">
                                                    <asp:ListItem Text="Select Meeting Type" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="CB" Value="CB"></asp:ListItem>
                                                    <asp:ListItem Text="ACB" Value="ACB"></asp:ListItem>
                                                    <asp:ListItem Text="ECCB" Value="ECCB"></asp:ListItem>
                                                    <asp:ListItem Text="RMCB" Value="RMCB"></asp:ListItem>
                                                    <asp:ListItem Text="ITSC" Value="ITSC"></asp:ListItem>
                                                    <asp:ListItem Text="CSCB" Value="CSCB"></asp:ListItem>
                                                    <asp:ListItem Text="CSRC" Value="CSRC"></asp:ListItem>
                                                    <asp:ListItem Text="SCBMF" Value="SCBMF"></asp:ListItem>
                                                    <asp:ListItem Text="SRC" Value="SRC"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            </tr>

                                     <tr id="trPresenter" style="display:none;">

                                            <td style="width:95px;">
                                    Presenter 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" >
                                        <asp:TextBox ID="txtPresenter" Width="300px" runat="server" MaxLength="250"></asp:TextBox>
                                 
                                                   </td>
                                     </tr>

                                     

                                     <tr id="trNumber" style="display:none;">

                                            <td style="width:95px;">
                                    Number 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" >
                                        <asp:TextBox ID="txtNumber" Width="300px" runat="server" MaxLength="250"></asp:TextBox>
                                 
                                                   </td>
                                     </tr>
                                        
                                        <tr id="trFromDate">

                                            <td style="width:95px;">
                                                From
                                            </td>
                                            <td width="20px">: </td>
                                            <td align="left" style="width:250px;"><%--<input type="text" id="demo-3" > <input type="button" value="..." id="demo-3-button"><small> ( click the button )</small>
<input type="text" id="demo-1" >                                    
                                     --%>
                                                <asp:TextBox ID="txtFrom" runat="server" ClientIDMode="Static" Width="300px"></asp:TextBox>
                                                <%--                                         <asp:CustomValidator ID="cvFrom" runat="server"  ClientIDMode="Static" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Enter start date" Text="Invalid" ForeColor="Red" ClientValidationFunction="validateDate"></asp:CustomValidator>--%>
                                                <%--<asp:RequiredFieldValidator ID="rfvFrom" runat="server" ClientIDMode="Static" ControlToValidate="txtFrom" Display="Dynamic" ErrorMessage="From" ForeColor="Red" Text="Invalid" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                            </td>
                                            <tr id="trToDate">
                                                <%--                                      <asp:RequiredFieldValidator runat="server" ID="rfvTo" ControlToValidate="txtTo" ClientIDMode="Static" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Enter end date" Text="Please enter end date" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                <td style="width:95px;">To </td>
                                                <td width="20px">: </td>
                                                <td align="left" style="width:250px;"><%--<input type="text" id="demo-3" > <input type="button" value="..." id="demo-3-button"><small> ( click the button )</small>
<input type="text" id="demo-1" >                                    
                                     --%>
                                                    <asp:TextBox ID="txtTo" runat="server" ClientIDMode="Static" Width="300px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvTo" runat="server" ClientIDMode="Static" ControlToValidate="txtTo" Display="Dynamic"  ForeColor="Red"  ValidationGroup="a"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cvTo" runat="server" ClientIDMode="Static" ClientValidationFunction="validateDate" Display="Dynamic" ForeColor="Red" ErrorMessage="End date should be greater than start date." Text="" ValidationGroup="a"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                        <tr>

                                            <td colspan="3">
                                                <div class="fullwidth noBorder">
                                                    <asp:Button ID="btnSubmit" runat="server" CssClass="btnSave" OnClick="btnSubmit_Click" Text="Submit" ValidationGroup="a" />
                                                    &nbsp; &nbsp;
                                                    <%--<asp:Button ID="btnExport" runat="server" CssClass="btnCancel" OnClick="btnExport_Click" Text="Export" ValidationGroup="a" Visible="false" />--%>
                                                </div>
                                            </td>
                                 </table>
                 <div class="add_remove" style="margin-bottom:15px; padding-right:10px">
                     <asp:ImageButton ID="lbtnExportToExcel"  ImageUrl="img/icons/actions/excel-icon.png" 
                                                    CausesValidation="false" runat="server" ToolTip="Export To Excel" Visible="false"  OnClick="lbtnExportToExcel_Click" />
                                    <%--<img width="16" height="16" src="img/icons/actions/excel-icon.png" alt="" style="vertical-align: middle" /> --%>
                                   
                                   &nbsp;<%--<asp:LinkButton CausesValidation="false" runat="server" ID="lbExportToExcel" Text="Export To Excel" OnClick="lbExportToExcel_Click"></asp:LinkButton>--%></div>
                 
                                                                 <div class="box_top">
<h2 class="icon users">Report</h2>
</div>                   
                              <div style="margin-bottom:15px;overflow:scroll;width:1370px;" >
                                  <div class="box_content">
  <div class="dataTables_wrapper">
                         <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="true"  
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found"  
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" 
                             OnRowCreated="grdReport_RowCreated"
                             >                                  
 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                              <PagerStyle CssClass="paginate_active"   />
                                  <RowStyle CssClass="gradeA odd" />
                                  <Columns>                                                                    
                                 <%--   <asp:TemplateField HeaderText="Sr">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>--%>
                                   
                                    
                                 </Columns>
                                </asp:GridView>




                                  <asp:Repeater ID="rptPager" runat="server" >
                                       <HeaderTemplate>
                     <table>
                     <tr class="paginate_active">
    <td colspan="5"><table>
     <tbody><tr>
                 </HeaderTemplate>

                                      <ItemTemplate>
    <asp:LinkButton ID="lnkPage" runat="server" Text = '<%#Eval("Text") %>' CommandArgument = '<%# Eval("Value") %>' Enabled = '<%# Eval("Enabled") %>' OnClick = "Page_Changed" CssClass="MainMenu"></asp:LinkButton>
</ItemTemplate>
                                      <FooterTemplate>
                     </tr>
    </tbody></table></td>
   </tr></table>
                                          </FooterTemplate>
</asp:Repeater>


      </div>
                                      </div>

                                  <br />
                                                  <%--<asp:GridView ID="grdDep" runat="server" AutoGenerateColumns="true"  
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found"  
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" 
                                                      EmptyDataRowStyle-HorizontalAlign="Center"
                                             OnRowCreated="grdReport_RowCreated"
                                    class="datatable" PageSize="10" >                                  
 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                              <PagerStyle CssClass="paginate_active"   />
                                  <RowStyle CssClass="gradeA odd" />
                                  <Columns>        
                                                                                         
                                  <%--  <asp:TemplateField HeaderText="Sr">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <HeaderTemplate>

                                        </HeaderTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>--%>
                                   
                             
                                 <%--</Columns>
                                </asp:GridView>--%>
                                 </div>
                                      </div>
                                          </ContentTemplate>
                                    <Triggers>
                    <asp:PostBackTrigger ControlID="btnSubmit" />
                   <asp:PostBackTrigger ControlID="lbtnExportToExcel" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlForum" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlMeeting" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlUser" />
                                   
    </Triggers>
</asp:UpdatePanel>
 </dl>
                            </fieldset>
                                </div>
                                </section>
                                </article>
</asp:Content>
