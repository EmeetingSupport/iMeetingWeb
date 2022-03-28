<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="MeetingMinder.Web.Reports" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta http-equiv="content-type" content="text/plain; charset=UTF-8" />

    <script type="text/javascript">

        function fnExcelReport() {
            var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
            var textRange; var j = 0;
            tab = document.getElementById('grdAgendaReport'); // id of table

            for (j = 0 ; j < tab.rows.length ; j++) {
                tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
                //tab_text=tab_text+"</tr>";
            }

            tab_text = tab_text + "</table>";
            tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
            tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
            tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");

            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
            {
                txtArea1.document.open("txt/html", "replace");
                txtArea1.document.write(tab_text);
                txtArea1.document.close();
                txtArea1.focus();
                sa = txtArea1.document.execCommand("SaveAs", true, "AgendaReport.xls");
            }
            else                 //other browser not tested on IE 11


                var link = document.createElement("a");
            link.download = "AgendaReport.xls";
            link.href = 'data:application/vnd.ms-excel,' + encodeURIComponent(tab_text);
            link.click();
            if (!navigator.userAgent.toLowerCase().includes('chrome')) {
                sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));
                return (sa);
            }
        }

        function Export() {
            var obj = $("#ddlReport").val();

            if (obj == "Agenda") {
                fnExcelReport();
            }
        }

        function pageLoad(sender, args) {

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

            $('#<%=chkMeetingHistory.ClientID%>').click(function () {
                if (this.checked) {
                    $('#trMeetingMISDetails').css("display", "");
                } else {
                    $('#trMeetingMISDetails').css("display", "none");
                }
            });
        }



        function validateDate(sender, args) {

            var vFrom = document.getElementById('<%=txtFrom.ClientID%>').value;
            var vTo = document.getElementById('<%=txtTo.ClientID%>').value;

            if (vFrom == '' || vTo == '') {
                args.IsValid = false;  // field is empty
            }
            else {

                var fromDate = vFrom.split('/');
                var toDate = vTo.split('/')



                if (new Date(fromDate[0] + '/01/' + fromDate[1]) > new Date(toDate[0] + '/28/' + toDate[1])) {
                    alert("End date should be greater than start date");
                    args.IsValid = false;
                }
                else {
                    args.IsValid = true;
                }
            }


        }

        function ShowDetails(obj) {
            if (obj == "0") {
                //  $("#tblReport").css("display","none");
                $("#trForum").css("display", "none");
                //  $("#trSelectItem").css("display", "none");
                $("#trUsers").css("display", "none");
                $('#trMeetingReport').css("display", "none");
                $('#trFromDate').css("display", "none");
                $('#trToDate').css("display", "none");
                $('#btnSubmit').css("display", "none");
                $('#btnExport').css("display", "none");
                $('#trAgendaReport').css("display", "none");
                $('#trMinutesReport').css("display", "none");
                $('#trMeetings').css("display", "none");
                $('#trProceedingReport').css("display", "none");
                $('#trAgendaTitle').css("display", "none");
                $('#trUser').css("display", "none");
                $('#trAttendanceReport').css("display", "none");

                $('#trMeetingMIS').css("display", "none");
                $('#trMeetingMISDetails').css("display", "none");

                var validatorObject = document.getElementById('<%=rfvReports.ClientID%>');
                validatorObject.enabled = true;
                validatorObject.isvalid = true;
                ValidatorUpdateDisplay(validatorObject);
            }
           <%-- else {
                $("#txtFrom").val("");
                $("#trToDate").val("");
              //  $('#<%= ddlForum.ClientID %>').val("0");
        }--%>

            if (obj == "Forum") {
                //  $("#tblReport").css("display", "block");
                var validatorObjectReport = document.getElementById('<%=rfvReports.ClientID%>');
                validatorObjectReport.enabled = true;
                validatorObjectReport.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectReport);

                var validatorObjectForum = document.getElementById('<%=rfvForum.ClientID%>');
                validatorObjectForum.enabled = true;
                validatorObjectForum.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectForum);

                var validatorObjectFrom = document.getElementById('<%=rfvFrom.ClientID%>');
                validatorObjectFrom.enabled = false;
                validatorObjectFrom.isvalid = false;
                ValidatorUpdateDisplay(validatorObjectFrom);

                var validatorObjectTo = document.getElementById('<%=rfvTo.ClientID%>');
                validatorObjectTo.enabled = false;
                validatorObjectTo.isvalid = false;
                ValidatorUpdateDisplay(validatorObjectTo);

                var CustomValidatorObjectTo = document.getElementById('<%=cvTo.ClientID%>');
                CustomValidatorObjectTo.enabled = false;
                CustomValidatorObjectTo.isvalid = false;
                ValidatorUpdateDisplay(CustomValidatorObjectTo);

                $("#trForum").css("display", "");
                $("#trSelectItem").css("display", "");
                $("#trUsers").css("display", "");
                $('#trFromDate').css("display", "none");
                $('#trToDate').css("display", "none");
                $('#btnSubmit').css("display", "block");
                $('#trMeetingReport').css("display", "none");
                $('#btnExport').css("display", "none");
                // $('#trMasterAgendaDetail').css("display", "none");
                $('#trSerialNumber').css("display", "none");
                $('#trAgendaReport').css("display", "none");
                $('#trMinutesReport').css("display", "none");
                $('#trMeetings').css("display", "none");
                $('#trProceedingReport').css("display", "none");
                $('#trAgendaTitle').css("display", "none");

                $('#trUser').css("display", "none");
                $('#trAttendanceReport').css("display", "none");

                $('#trMeetingMIS').css("display", "none");
                $('#trMeetingMISDetails').css("display", "none");

                if ($("#<%=grdReport.ClientID %> tr").length > 0 && $("#<%= grdReport.ClientID %> tr")[0].innerHTML.trim() != "<td colspan=\"4\">No Records Found</td>") {
                    $('#btnExport').css("display", "");
                }
                else {
                    $('#btnExport').css("display", "none");
                }
            }
            if (obj == "Meeting") {
                var validatorObjectForum = document.getElementById('<%=rfvForum.ClientID%>');
                validatorObjectForum.enabled = false;
                validatorObjectForum.isvalid = false;
                ValidatorUpdateDisplay(validatorObjectForum);

                var validatorObjectFrom = document.getElementById('<%=rfvFrom.ClientID%>');
                validatorObjectFrom.enabled = true;
                validatorObjectFrom.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectFrom);

                var validatorObjectTo = document.getElementById('<%=rfvTo.ClientID%>');
                validatorObjectTo.enabled = true;
                validatorObjectTo.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectTo);

                var CustomValidatorObjectTo = document.getElementById('<%=cvTo.ClientID%>');
                CustomValidatorObjectTo.enabled = true;
                CustomValidatorObjectTo.isvalid = true;
                ValidatorUpdateDisplay(CustomValidatorObjectTo);

                $("#trForum").css("display", "none");
                $("#trSelectItem").css("display", "none");
                $("#trUsers").css("display", "none");
                $('#trFromDate').css("display", "");
                $('#trToDate').css("display", "");
                $('#btnSubmit').css("display", "");
                $('#trMeetingReport').css("display", "");
                //  $('#trMasterAgendaDetail').css("display", "none");
                $('#trSerialNumber').css("display", "none");
                $('#trAgendaReport').css("display", "none");
                $('#trMinutesReport').css("display", "none");
                $('#trMeetings').css("display", "none");
                $('#trProceedingReport').css("display", "none");
                $('#trAgendaTitle').css("display", "none");
                $('#trUser').css("display", "none");
                $('#trAttendanceReport').css("display", "none");

                $('#trMeetingMIS').css("display", "");
                $('#<%=chkMeetingHistory.ClientID%>').is(":checked") ? $('#trMeetingMISDetails').css("display", "") : $('#trMeetingMISDetails').css("display", "none");
                

                if ($("#<%=grdMeetingReport.ClientID %> tr").length > 0 && $("#<%= grdMeetingReport.ClientID %> tr")[0].innerHTML.trim() != "<td colspan=\"4\">No Records Found</td>") {
                    $('#btnExport').css("display", "");
                }
                else {
                    $('#btnExport').css("display", "none");
                }

            }

            if (obj == "Agenda") {
                var validatorObjectForum = document.getElementById('<%=rfvForum.ClientID%>');
                validatorObjectForum.enabled = true;
                validatorObjectForum.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectForum);

                var validatorObjectFrom = document.getElementById('<%=rfvFrom.ClientID%>');
                validatorObjectFrom.enabled = false;
                validatorObjectFrom.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectFrom);

                var validatorObjectTo = document.getElementById('<%=rfvTo.ClientID%>');
                validatorObjectTo.enabled = false;
                validatorObjectTo.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectTo);

                var CustomValidatorObjectTo = document.getElementById('<%=cvTo.ClientID%>');
                CustomValidatorObjectTo.enabled = false;
                CustomValidatorObjectTo.isvalid = true;
                ValidatorUpdateDisplay(CustomValidatorObjectTo);

                $("#trForum").css("display", "");
                $("#trSelectItem").css("display", "none");
                $("#trUsers").css("display", "none");
                $('#trFromDate').css("display", "");
                $('#trToDate').css("display", "");
                $('#btnSubmit').css("display", "");
                $('#trMeetingReport').css("display", "none");
                //  $('#trMasterAgendaDetail').css("display", "");
                $('#trSerialNumber').css("display", "");
                $('#trAgendaReport').css("display", "");
                $('#trMinutesReport').css("display", "none");
                $('#trMeetings').css("display", "none");
                $('#trProceedingReport').css("display", "none");
                $('#trAgendaTitle').css("display", "");

                $('#trUser').css("display", "none");
                $('#trAttendanceReport').css("display", "none");

                $('#trMeetingMIS').css("display", "none");
                $('#trMeetingMISDetails').css("display", "none");

                if ($("#<%=grdAgendaReport.ClientID %> tr").length > 0 && $("#<%= grdAgendaReport.ClientID %> tr")[0].innerHTML.trim() != "<td colspan=\"4\">No Records Found</td>") {
                    $('#btnExport').css("display", "");
                }
                else {
                    $('#btnExport').css("display", "none");
                }

            }

            if (obj == "Minutes") {
                var validatorObjectForum = document.getElementById('<%=rfvForum.ClientID%>');
                validatorObjectForum.enabled = true;
                validatorObjectForum.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectForum);

                var validatorObjectFrom = document.getElementById('<%=rfvFrom.ClientID%>');
                validatorObjectFrom.enabled = false;
                validatorObjectFrom.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectFrom);

                var validatorObjectTo = document.getElementById('<%=rfvTo.ClientID%>');
                validatorObjectTo.enabled = false;
                validatorObjectTo.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectTo);

                var CustomValidatorObjectTo = document.getElementById('<%=cvTo.ClientID%>');
                 CustomValidatorObjectTo.enabled = false;
                 CustomValidatorObjectTo.isvalid = true;
                 ValidatorUpdateDisplay(CustomValidatorObjectTo);

                 $("#trForum").css("display", "");
                 $("#trSelectItem").css("display", "none");
                 $("#trUsers").css("display", "none");
                 $('#trFromDate').css("display", "");
                 $('#trToDate').css("display", "");
                 $('#btnSubmit').css("display", "");
                 $('#trMeetingReport').css("display", "none");
                // $('#trMasterAgendaDetail').css("display", "none");
                 $('#trSerialNumber').css("display", "none");
                 $('#trAgendaReport').css("display", "none");
                 $('#trMinutesReport').css("display", "");
                 $('#trMeetings').css("display", "");
                 $('#trProceedingReport').css("display", "none");
                 $('#trAgendaTitle').css("display", "none");

                 $('#trUser').css("display", "none");
                 $('#trAttendanceReport').css("display", "none");

                 $('#trMeetingMIS').css("display", "none");
                 $('#trMeetingMISDetails').css("display", "none");

                 if ($("#<%= grdMinutesReports.ClientID %> tr").length > 0 && $("#<%= grdMinutesReports.ClientID %> tr")[0].innerHTML.trim() != "<td colspan=\"4\">No Records Found</td>") {
                    $('#btnExport').css("display", "");
                }
                else {
                    $('#btnExport').css("display", "none");
                }

            }

            if (obj == "Proceeding") {
                var validatorObjectForum = document.getElementById('<%=rfvForum.ClientID%>');
                validatorObjectForum.enabled = true;
                validatorObjectForum.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectForum);

                var validatorObjectFrom = document.getElementById('<%=rfvFrom.ClientID%>');
                validatorObjectFrom.enabled = false;
                validatorObjectFrom.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectFrom);

                var validatorObjectTo = document.getElementById('<%=rfvTo.ClientID%>');
                validatorObjectTo.enabled = false;
                validatorObjectTo.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectTo);

                var CustomValidatorObjectTo = document.getElementById('<%=cvTo.ClientID%>');
                 CustomValidatorObjectTo.enabled = false;
                 CustomValidatorObjectTo.isvalid = true;
                 ValidatorUpdateDisplay(CustomValidatorObjectTo);

                 $("#trForum").css("display", "");
                 $("#trSelectItem").css("display", "none");
                 $("#trUsers").css("display", "none");
                 $('#trFromDate').css("display", "");
                 $('#trToDate').css("display", "");
                 $('#btnSubmit').css("display", "");
                 $('#trMeetingReport').css("display", "none");
                // $('#trMasterAgendaDetail').css("display", "none");
                 $('#trSerialNumber').css("display", "none");
                 $('#trAgendaReport').css("display", "none");
                 $('#trMinutesReport').css("display", "none");
                 $('#trProceedingReport').css("display", "");
                 $('#trMeetings').css("display", "");
                 $('#trAgendaTitle').css("display", "none");

                 $('#trUser').css("display", "none");
                 $('#trAttendanceReport').css("display", "none");

                 $('#trMeetingMIS').css("display", "none");
                 $('#trMeetingMISDetails').css("display", "none");

                 if ($("#<%= grdProceedingReport.ClientID %> tr").length > 0 && $("#<%= grdProceedingReport.ClientID %> tr")[0].innerHTML.trim() != "<td colspan=\"4\">No Records Found</td>") {
                    $('#btnExport').css("display", "");
                }
                else {
                    $('#btnExport').css("display", "none");
                }

            }

            if (obj == "Attendance") {
                var validatorObjectForum = document.getElementById('<%=rfvForum.ClientID%>');
                validatorObjectForum.enabled = true;
                validatorObjectForum.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectForum);

                var validatorObjectFrom = document.getElementById('<%=rfvFrom.ClientID%>');
                validatorObjectFrom.enabled = false;
                validatorObjectFrom.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectFrom);

                var validatorObjectTo = document.getElementById('<%=rfvTo.ClientID%>');
                validatorObjectTo.enabled = false;
                validatorObjectTo.isvalid = true;
                ValidatorUpdateDisplay(validatorObjectTo);

                var CustomValidatorObjectTo = document.getElementById('<%=cvTo.ClientID%>');
                CustomValidatorObjectTo.enabled = false;
                CustomValidatorObjectTo.isvalid = true;
                ValidatorUpdateDisplay(CustomValidatorObjectTo);

                $("#trForum").css("display", "");
                $("#trSelectItem").css("display", "none");
                $("#trUsers").css("display", "none");
                $('#trFromDate').css("display", "");
                $('#trToDate').css("display", "");
                $('#btnSubmit').css("display", "");
                $('#trMeetingReport').css("display", "none");
                // $('#trMasterAgendaDetail').css("display", "none");
                $('#trSerialNumber').css("display", "none");
                $('#trAgendaReport').css("display", "none");
                $('#trMinutesReport').css("display", "none");
                $('#trProceedingReport').css("display", "none");
                $('#trMeetings').css("display", "");
                $('#trAgendaTitle').css("display", "none");
                $('#trUser').css("display", "");
                $('#trAttendanceReport').css("display", "");

                $('#trMeetingMIS').css("display", "none");
                $('#trMeetingMISDetails').css("display", "none");

                if ($("#<%= grdMeetingReport.ClientID %> tr").length > 0 && $("#<%= grdMeetingReport.ClientID %> tr")[0].innerHTML.trim() != "<td colspan=\"4\">No Records Found</td>") {

                    $('#btnExport').css("display", "");
                }
                else {
                    $('#btnExport').css("display", "none");
                }
            }
        }

        function Validate() {
            var obj = $("#ddlReport").val();
            if (obj == "Agenda" || obj == "Proceeding" || obj == "Minutes") {
                if ($("#txtFrom").val() != "") {
                    var validatorObjectFrom = document.getElementById('<%=rfvFrom.ClientID%>');
                    validatorObjectFrom.enabled = true;
                    validatorObjectFrom.isvalid = true;
                    ValidatorUpdateDisplay(validatorObjectFrom);

                    var validatorObjectTo = document.getElementById('<%=rfvTo.ClientID%>');
                    validatorObjectTo.enabled = true;
                    validatorObjectTo.isvalid = true;
                    ValidatorUpdateDisplay(validatorObjectTo);

                    var CustomValidatorObjectTo = document.getElementById('<%=cvTo.ClientID%>');
                    CustomValidatorObjectTo.enabled = true;
                    CustomValidatorObjectTo.isvalid = true;
                    ValidatorUpdateDisplay(CustomValidatorObjectTo);
                }
                else {
                    $("#txtFrom").val("");
                    $("#txtTo").val("");

                    var validatorObjectFrom = document.getElementById('<%=rfvFrom.ClientID%>');
                     validatorObjectFrom.enabled = false;


                     var validatorObjectTo = document.getElementById('<%=rfvTo.ClientID%>');
                    validatorObjectTo.enabled = false;


                    var CustomValidatorObjectTo = document.getElementById('<%=cvTo.ClientID%>');
                    CustomValidatorObjectTo.enabled = false;

                }
            }
        }

    </script>
    <style type="text/css">
        .gradeA.odd > td {
            min-width: 20px !important;
        }
    </style>

    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--View Report--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>View Report</b></font></legend>
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
                                <div style="width:1100px;">
                                 <table width="75%">

                                    <tr style="display:none">
                                     <td style="width:95px;">
                                    Department
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlEntity" Visible="false" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList>
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Department" Text="Please Select Department" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                     
                                 
                                     </td>
                                     </tr>

                                     <tr >
                                     <td style="width:95px;">
                                    Reports
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlReport" ClientIDMode="Static"   runat="server" onchange="ShowDetails(this.value);" Width="50%"   >
                                         <asp:ListItem Value="0">Select</asp:ListItem>
                                         <asp:ListItem Value="Forum">Forum Report</asp:ListItem>
                                         <asp:ListItem Value="Meeting">Meeting Report</asp:ListItem>
                                         <asp:ListItem Value="Agenda">Agenda Report</asp:ListItem>
                                         <asp:ListItem Value="Minutes">Minutes Report</asp:ListItem>
                                         <asp:ListItem Value="Proceeding">Proceeding Report</asp:ListItem>
                                          <%--  <asp:ListItem Value="Attendance">Attendance Report</asp:ListItem>--%>
                                     </asp:DropDownList>
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Entity" Text="Please Select Entity" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvReports" ControlToValidate="ddlReport" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Report" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>

                                 
                                     </td>
                                     </tr>

                                     <tr id="trForum" style="display:none">
                                     <td style="width:95px;">
                                    Forum
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlForum" runat="server"  Width="50%"
                                          onselectedindexchanged="ddlForum_SelectedIndexChanged"  AutoPostBack="true"   ></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" ClientIDMode="Static" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Forum" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>

                                      <tr  style="display:none" id="trMeetings" >
                                     <td style="width:95px;">
                                    Meeting 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                         <asp:DropDownList ID="ddlMeeting"  AutoPostBack="true"  OnSelectedIndexChanged="ddlMeeting_SelectedIndexChanged" runat="server" Width="50%"></asp:DropDownList>
                                    
                                 
                                                   </td>
                                     </tr>

                                     <tr id="trMasterAgendaDetail"  style="display:none">
                                     <td style="width:95px;">
                                    Master Agenda
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                    <asp:DropDownList ID="ddlAgenda" runat="server"  Width="50%"
                                           AutoPostBack="true"   ></asp:DropDownList>
                                     
                                     
                                 
                                     </td>
                                     </tr>
                                       <tr id="trUser"  style="display:none">
			                                     <td style="width:95px;">
			                                    User
			                                     </td>
			                                     <td width="20px">
			                                     :
			                                     </td>
			                                     <td  style="width:250px;" align="left">
			                                    <asp:DropDownList ID="ddlUser" runat="server"  Width="50%"
			                                           AutoPostBack="true"   ></asp:DropDownList>
			                                     
			                                     
			                                 
			                                     </td>
			                                     </tr>
                                     <tr id="trSerialNumber" style="display:none">
                                     <td style="width:95px;">
                                    Serial Number
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                   <asp:DropDownList ID="ddlSerialNumber" ClientIDMode="Static" runat="server"  Width="50%" >
                                       <%--<asp:ListItem Value="0" Text="Select" ></asp:ListItem>
                                        <asp:ListItem Value="ABS :" Text="ABS" ></asp:ListItem>
                                         <asp:ListItem Value="C&R :" Text="C&R" ></asp:ListItem>
                                         <asp:ListItem Value="CAG :" Text="CAG" ></asp:ListItem>
                                         <asp:ListItem Value="CBG :" Text="CBG" ></asp:ListItem>
                                         <asp:ListItem Value="CCO :" Text="CCO" ></asp:ListItem>
                                         <asp:ListItem Value="CDO :" Text="CDO" ></asp:ListItem>
                                         <asp:ListItem Value="CFO :" Text="CFO" ></asp:ListItem>
                                         <asp:ListItem Value="CIO :" Text="CIO" ></asp:ListItem>
                                         <asp:ListItem Value="COO :" Text="COO" ></asp:ListItem>
                                       <asp:ListItem Value="CRO :" Text="CRO" ></asp:ListItem>
                                       <asp:ListItem Value="CS&NB :" Text="CS&NB" ></asp:ListItem>
                                       <asp:ListItem Value="I&MA :" Text="I&MA" ></asp:ListItem>
                                       <asp:ListItem Value="IBG :" Text="IBG" ></asp:ListItem>
                                       <asp:ListItem Value="MCG.MBG :" Text="MCG" ></asp:ListItem>
                                       <asp:ListItem Value="MBG :" Text="MBG" ></asp:ListItem>
                                       <asp:ListItem Value="SAMG :" Text="SAMG" ></asp:ListItem>--%>
                                         
                                    </asp:DropDownList>
                                     
                                     
                                 
                                     </td>
                                     </tr>

                                       <tr id="trAgendaTitle"  style="display:none">
                                     <td style="width:95px;">
                                    Agenda Title
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                   <asp:TextBox ID="txtAgendaTitle" MaxLength="200" ClientIDMode="Static" runat="server"  Width="50%" >
                                    </asp:TextBox>                                    
                                                                      
                                     </td>
                                     </tr>

                                       <tr style="display:none">
                                     <td id="trSelectItem" style="width:95px;">
                                    Select Items
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlMember" Visible="false"  runat="server" Width="50%" AutoPostBack="true"
                                             onselectedindexchanged="ddlMember_SelectedIndexChanged" ></asp:DropDownList>
                                   
                                     
                                     </td>
                                     </tr>

                                     <tr id="trUsers" style="display:none;" runat="server" clientidmode="Static" visible="false">
                                     <td>User list</td>
                                       <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:GridView ID="grdReport" ShowHeader="true" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" DataKeyNames ="UserId" 
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="70%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable">
                                 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                          <PagerStyle CssClass="paginate_active"   />
                                  <Columns>
                                  
                                      <asp:TemplateField HeaderText="Name" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("Suffix") +" "+Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
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

                                     <tr id="trFromDate" style="display:none">
                                     <td  style="width:95px;">
                                    From 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
<%--<input type="text" id="demo-3" > <input type="button" value="..." id="demo-3-button"><small> ( click the button )</small>
<input type="text" id="demo-1" >                                    
                                     --%>
                                       <asp:TextBox ID="txtFrom" ClientIDMode="Static" runat="server" Width="250px"></asp:TextBox>
<%--                                         <asp:CustomValidator ID="cvFrom" runat="server"  ClientIDMode="Static" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Enter start date" Text="Invalid" ForeColor="Red" ClientValidationFunction="validateDate"></asp:CustomValidator>--%>
                                       <asp:RequiredFieldValidator runat="server" ID="rfvFrom" ControlToValidate="txtFrom" ClientIDMode="Static"  Display="Dynamic" ValidationGroup="a" ErrorMessage="From" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>

                                     </td>
                                     </tr>

                                     <tr id="trToDate" style="display:none">
<%--                                      <asp:RequiredFieldValidator runat="server" ID="rfvTo" ControlToValidate="txtTo" ClientIDMode="Static" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Enter end date" Text="Please enter end date" ForeColor="Red"></asp:RequiredFieldValidator>--%>

                                     <td  style="width:95px;">
                                    To 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
<%--<input type="text" id="demo-3" > <input type="button" value="..." id="demo-3-button"><small> ( click the button )</small>
<input type="text" id="demo-1" >                                    
                                     --%>
                                       <asp:TextBox ID="txtTo" ClientIDMode="Static" runat="server" Width="250px"></asp:TextBox>
                                       <asp:RequiredFieldValidator runat="server" ID="rfvTo" ControlToValidate="txtTo" ClientIDMode="Static" Display="Dynamic" ValidationGroup="a" ErrorMessage="To" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                       <asp:CustomValidator ID="cvTo" runat="server"  ClientIDMode="Static"   Display="Dynamic" ValidationGroup="a" ErrorMessage="End date should be greater than start date"  ForeColor="Red" ClientValidationFunction="validateDate"></asp:CustomValidator>
                                     </td>
                                     </tr>

                                     <tr id="trMeetingReport" runat="server" visible="false" clientidmode="Static" style="display:none;" >
                                     <td style="width:95px;">
                                    Meetings
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     
                                  
                                 
                                        <asp:GridView ClientIDMode="Static" ID="grdMeetingReport" ShowHeader="true" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable">
                                 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                          <PagerStyle CssClass="paginate_active"   />
                                  <Columns>
                                  <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                      </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Forum Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblForumName" runat="server" Text='<%# Eval("ForumName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Meeting No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeetingNumber" runat="server" Text='<%# Eval("MeetingNumber").ToString() %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                      <asp:TemplateField HeaderText="Meeting Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeetingDate" runat="server" Text='<%# Convert.ToDateTime(Eval("MeetingDate")).ToString("D") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField  HeaderText="Meeting Time" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblMeetingTime" runat="server" Text='<%# Eval("MeetingTime") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                
                                     <asp:TemplateField  HeaderText="Meeting Venue" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblMeetingVenue" runat="server" Text='<%# Eval("MeetingVenue") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                                           
                                 </Columns>
                                </asp:GridView>
                                     
                                        
                                 
                                     </td>
                                     </tr>

                                     <tr id="trMeetingMIS" runat="server" clientidmode="Static" style="display:none;">
                                         <td style="width:95px;">
                                             Meeting MIS
                                         </td>
                                         <td style="width:20px;">
                                             :
                                         </td>
                                         <td style="width:250px;" align="left">
                                             <asp:CheckBox ID="chkMeetingHistory" runat="server" />
                                         </td>
                                     </tr>

                                     <tr id="trMeetingMISDetails" runat="server" clientidmode="Static" style="display:none;">
                                         <td style="width:95px;">
                                             Meeting History
                                         </td>
                                         <td style="width:20px;">
                                             :
                                         </td>
                                         <td style="width:250px;" align="left">
                                             <asp:GridView ID="grdMeetingHistory" runat="server" CssClass="datatable" AutoGenerateColumns="false" ShowHeader="true"
                                                  CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" GridLines="None" EmptyDataRowStyle-ForeColor="red" 
                                                 Width="100%" EmptyDataRowStyle-HorizontalAlign="Center">

                                                  <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                                  <RowStyle CssClass="gradeA odd" />
                                                  <PagerStyle CssClass="paginate_active"   />
                                                  <Columns>
                                                      <asp:TemplateField HeaderText="Sr. No.">
                                                            <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle  HorizontalAlign="Center" />
                                                       </asp:TemplateField>

                                                      <asp:TemplateField HeaderText="Forum Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblForumName" runat="server" Text='<%# Eval("ForumName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                      </asp:TemplateField>

                                                      <asp:TemplateField HeaderText="Meeting No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMeetingNumber" runat="server" Text='<%# Eval("MeetingNumber").ToString() %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                    
                                                      <asp:TemplateField HeaderText="Meeting Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMeetingDate" runat="server" Text='<%# Convert.ToDateTime(Eval("MeetingDate")).ToString("D") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                      <asp:TemplateField  HeaderText="Meeting Time" >
                                                       <ItemTemplate>
                                                            <asp:Label ID="lblMeetingTime" runat="server" Text='<%# Eval("MeetingTime") %>'></asp:Label>
                                                        </ItemTemplate>
                                                      </asp:TemplateField>
                                                
                                                      <asp:TemplateField  HeaderText="Meeting Venue" >
                                                      <ItemTemplate>
                                                            <asp:Label ID="lblMeetingVenue" runat="server" Text='<%# Eval("MeetingVenue") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField> 

                                                      <asp:TemplateField  HeaderText="Status" >
                                                      <ItemTemplate>
                                                            <asp:Label ID="lblMeetingStatus" runat="server" Text='<%# Eval("Action").ToString()=="U"?"Updated":"Deleted" %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                      <asp:TemplateField  HeaderText="Reason" >
                                                      <ItemTemplate>
                                                            <asp:Label ID="lblMeetingEditOrCancelReason" runat="server" Text='<%# Eval("EditOrCancelReason") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   </Columns>
                                             </asp:GridView>
                                         </td>
                                     </tr>

                                      <tr id="trAgendaReport" runat="server"  visible="false" clientidmode="Static" style="display:none;" >
                              <%--       <td style="width:95px;">
                                    Agendas
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>--%>
                                     <td colspan="3" id="trAgendaDetailsExport" runat="server"  width="100%"   align="left">
                                     
                                     
                                   <div class="box_top">
                              <h2 >Agenda </h2>
                           
                           </div>
                           <div class="box_content">
                              <div class="dataTables_wrapper">
                                         <asp:GridView ID="grdAgendaReport" ClientIDMode="Static" ShowHeader="true" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="0" EmptyDataText="No Records Found" PageSize="10"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" OnPageIndexChanging="grdAgendaReport_PageIndexChanging" OnRowCommand="grdAgendaReport_RowCommand">
                                 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                          <PagerStyle CssClass="paginate_active"   />
                                  <Columns>
                                  <asp:TemplateField HeaderText="Sr. No." ControlStyle-Width="5%">
                                        <ItemTemplate>
                                                <%# Eval("RowNumber") %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                      </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Forum Name" ControlStyle-Width="10%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblForumName" style="display:inline;" runat="server" Text='<%# MM.Core.Encryptor.DecryptString(Eval("ForumName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Meeting No." ControlStyle-Width="20%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeetingNumber" style="display:inline;" runat="server" Text='<%# Eval("MeetingNumber").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                      <asp:TemplateField HeaderText="Meeting" ControlStyle-Width="20%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeetingDate" style="display:inline;" runat="server" Text='<%# Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("D") +" "+MM.Core.Encryptor.DecryptString(Eval("MeetingTime").ToString())+" "+MM.Core.Encryptor.DecryptString(Eval("MeetingVenue").ToString())%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField  HeaderText="Agenda" >
                                        <ItemTemplate>
                                             <asp:LinkButton ID="lnkDownload" runat="server" Text="Download PDFS"  CommandArgument='<%# Eval("MeetingId")%>' CommandName="Download"></asp:LinkButton>
                                               <div onclick="$('#DivAgenda<%# Container.DataItemIndex + 1 %>').toggle();"> <a href="javascript:void(0);"> View Agenda </a></div>
                                            <div style="display:none;" id="DivAgenda<%# Container.DataItemIndex + 1 %>"   > 
                                        <asp:Label ID="lblVal" runat="server" Text='<%# Agenda(Eval("MeetingId"))%>' />
                                                </div>
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                
                                     <%--  <asp:TemplateField HeaderText="Download PDFS">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download"  CommandArgument='<%# Eval("MeetingId")%>' CommandName="Download"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>    --%>                                                                  
                                 </Columns>
                                </asp:GridView>
                                     
                                     <asp:Repeater ID="rptPagerAgenda" runat="server">
                                         
                 <HeaderTemplate>
                     <table>
                     <tr class="paginate_active">
				<td colspan="5"><table>
					<tbody><tr>
                 </HeaderTemplate>
<ItemTemplate>
    <td>
    <asp:LinkButton ID="lnkPage" runat="server" Text = '<%#Eval("Text") %>' CommandArgument = '<%# Eval("Value") %>' Enabled = '<%# Eval("Enabled") %>' OnClick = "Page_Changed"></asp:LinkButton></td>
</ItemTemplate>
                 <FooterTemplate>
                     </tr>
				</tbody></table></td>
			</tr></table>
                 </FooterTemplate>
</asp:Repeater>
                                  </div>
                               </div>
                                 
                                     </td>
                                     </tr>


                                      <tr id="trMinutesReport" runat="server"  visible="false" clientidmode="Static" style="display:none;" >
                                     <td style="width:95px;">
                                    Minutes
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     
                                          
                                   <div class="box_top">
                              <h2 >Minutes </h2>
                           
                           </div>
                           <div class="box_content">
                              <div class="dataTables_wrapper">
                                 
                                         <asp:GridView ID="grdMinutesReports" ShowHeader="true" runat="server" PageSize="10" OnPageIndexChanging="grdMinutesReports_PageIndexChanging" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" 
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" OnRowCommand="grdMinutesReports_RowCommand" >
                                 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                          <PagerStyle CssClass="paginate_active"   />
                                  <Columns>
                                  <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%#  Eval("RowNumber") %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                      </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Forum Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncrptionkey" runat="server" Visible="false" Text='<%#Eval("EncryptionKey") %>'></asp:Label>
                                            <asp:Label ID="lblForumName1" runat="server" Text='<%#MM.Core.Encryptor.DecryptString(Eval("ForumName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Meeting No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeetingNumber" runat="server" Text='<%# Eval("MeetingNumber").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                      <asp:TemplateField HeaderText="Meeting">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeetingDate" runat="server" Text='<%# Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("D") +" "+MM.Core.Encryptor.DecryptString(Eval("MeetingTime").ToString())+" "+MM.Core.Encryptor.DecryptString(Eval("MeetingVenue").ToString())%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Minutes" >
                                        <ItemTemplate>
                                           <asp:LinkButton ID="lbnApprove" CommandArgument='<%# Bind("UploadFile") %>' CausesValidation="false" CommandName="view" Text="View" runat="server" ToolTip="Approve"></asp:LinkButton>
                                           <%--&nbsp; &nbsp;  <asp:LinkButton ID="lbnReject" CommandArgument='<%# Bind("EntityId") %>' CausesValidation="false" CommandName="Decline" Text="Decline" runat="server" ToolTip="Decline"></asp:LinkButton>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                
                                                                                                            
                                 </Columns>
                                </asp:GridView>
                                     <asp:Repeater ID="rptMinute" runat="server">
                                         
                 <HeaderTemplate>
                     <table>
                     <tr class="paginate_active">
				<td colspan="5"><table>
					<tbody><tr>
                 </HeaderTemplate>
<ItemTemplate>
    <td>
    <asp:LinkButton ID="lnkPage" runat="server" Text = '<%#Eval("Text") %>' CommandArgument = '<%# Eval("Value") %>' Enabled = '<%# Eval("Enabled") %>' OnClick = "Page_Changed_Minute"></asp:LinkButton></td>
</ItemTemplate>
                 <FooterTemplate>
                     </tr>
				</tbody></table></td>
			</tr></table>
                 </FooterTemplate>
</asp:Repeater>
                                  </div>
                               </div>
                                    
                                 
                                     </td>
                                     </tr>


                                     <tr id="trProceedingReport" runat="server"  visible="false" clientidmode="Static" style="display:none;" >
                                     <td style="width:95px;">
                                    Proceedings
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     
                                                
                                   <div class="box_top">
                              <h2 >Proceedings </h2>
                           
                           </div>
                           <div class="box_content">
                              <div class="dataTables_wrapper">
                                 
                                         <asp:GridView ID="grdProceedingReport"  AllowPaging="true" PageSize="10" ShowHeader="true" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" 
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="70%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" OnRowCommand="grdProceedingReport_RowCommand" OnPageIndexChanging="grdProceedingReport_PageIndexChanging">
                                 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                          <PagerStyle CssClass="paginate_active"   />
                                  <Columns>
                                  <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                              <%# Container.DataItemIndex + 1 %>
                                            <%-- <%# Eval("RowNumber") %>--%>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                      </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Forum Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncrptionkey" runat="server" Visible="false" Text='<%#Eval("EncryptionKey") %>'></asp:Label>
                                            <asp:Label ID="lblForumName1" runat="server" Text='<%#MM.Core.Encryptor.DecryptString(Eval("ForumName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    
                                      <asp:TemplateField HeaderText="Meeting">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeetingDate" runat="server" Text='<%# Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("D") +" "+MM.Core.Encryptor.DecryptString(Eval("MeetingTime").ToString())+" "+MM.Core.Encryptor.DecryptString(Eval("MeetingVenue").ToString())%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Proceeding" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbnProceding" CommandArgument='<%# Bind("ProcedingName") %>' CausesValidation="false" CommandName="view" Text="View" runat="server" ToolTip="Proceding"></asp:LinkButton>
                                           <%--&nbsp; &nbsp;  <asp:LinkButton ID="lbnReject" CommandArgument='<%# Bind("EntityId") %>' CausesValidation="false" CommandName="Decline" Text="Decline" runat="server" ToolTip="Decline"></asp:LinkButton>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                
                                                                                                            
                                 </Columns>
                                </asp:GridView>
                                     
                                                                     
                                            <asp:Repeater ID="rptProceeding" runat="server">
                                         
                 <HeaderTemplate>
                     <table>
                     <tr class="paginate_active">
				<td colspan="5"><table>
					<tbody><tr>
                 </HeaderTemplate>
<ItemTemplate>
    <td>
    <asp:LinkButton ID="lnkPage" runat="server" Text = '<%#Eval("Text") %>' CommandArgument = '<%# Eval("Value") %>' Enabled = '<%# Eval("Enabled") %>' OnClick = "Page_Changed_Proceeding"></asp:LinkButton></td>
</ItemTemplate>
                 <FooterTemplate>
                     </tr>
				</tbody></table></td>
			</tr></table>
                 </FooterTemplate>
</asp:Repeater>

                                  </div>
                               </div>
                                         
                                     </td>
                                     </tr>
                                       <tr id="trAttendanceReport" runat="server"  visible="false" clientidmode="Static" style="display:none;" >
			                                     <td style="width:95px;">
			                                    Attendance
			                                     </td>
			                                     <td width="20px">
			                                     :
			                                     </td>
			                                     <td  style="width:250px;" align="left">
			                                     
			                                                
			                                   <div class="box_top">
			                              <h2 >Attendance </h2>
			                           
			                           </div>
			                           <div class="box_content">
			                              <div class="dataTables_wrapper">
			                                 
			                                         <asp:GridView ID="grdAttendanceReport"  AllowPaging="true" PageSize="10" ShowHeader="true" runat="server" AutoGenerateColumns="False"
			                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" 
			                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" OnPageIndexChanging="grdAttendanceReport_PageIndexChanging" Width="70%" EmptyDataRowStyle-HorizontalAlign="Center"
			                                    class="datatable">
			                                 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
			                                  <RowStyle CssClass="gradeA odd" />
			                                          <PagerStyle CssClass="paginate_active"   />
			                                  <Columns>
			                                  <asp:TemplateField HeaderText="Sr. No.">
			                                        <ItemTemplate>
			                                              <%# Eval("RowNumber") %>
			                                        </ItemTemplate>
			                                        <ItemStyle  HorizontalAlign="Center" />
			                                      </asp:TemplateField>
			
			                                      <asp:TemplateField HeaderText="User Name">
			                                        <ItemTemplate>
			                                           
			                                            <asp:Label ID="lblUserName" runat="server" Text='<%# MM.Core.Encryptor.DecryptString(Eval("Suffix").ToString()) +" "+MM.Core.Encryptor.DecryptString(Eval("FirstName").ToString()) +" "+ MM.Core.Encryptor.DecryptString(Eval("LastName").ToString()) %>'></asp:Label>
			                                        </ItemTemplate>
			                                    </asp:TemplateField>
			
			                                    
			                                      <asp:TemplateField HeaderText="Meeting">
			                                        <ItemTemplate>
			                                            <asp:Label ID="lblMeetingDate" runat="server" Text='<%# Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("D") +" "+MM.Core.Encryptor.DecryptString(Eval("MeetingTime").ToString())+" "+MM.Core.Encryptor.DecryptString(Eval("MeetingVenue").ToString())%>'></asp:Label>
			                                        </ItemTemplate>
			                                    </asp:TemplateField>
			
			                                   <asp:TemplateField HeaderText="Attendance" >
			                                        <ItemTemplate>
			                                            <asp:Label ID="lblAttendance" runat="server" Text='<%# (Eval("Attendance").ToString()=="True" ? "Present": "Absent")%>'></asp:Label>
			                                           <%--&nbsp; &nbsp;  <asp:LinkButton ID="lbnReject" CommandArgument='<%# Bind("EntityId") %>' CausesValidation="false" CommandName="Decline" Text="Decline" runat="server" ToolTip="Decline"></asp:LinkButton>--%>
			                                        </ItemTemplate>
			                                    </asp:TemplateField>
			                                                
			                                                                                                            
			                                 </Columns>
			                                </asp:GridView>
			                                     
			                                                                     
			                                            <asp:Repeater ID="rptAttendance" runat="server">
			                                         
			                 <HeaderTemplate>
			                     <table>
			                     <tr class="paginate_active">
							<td colspan="5"><table>
								<tbody><tr>
			                 </HeaderTemplate>
			<ItemTemplate>
			    <td>
			    <asp:LinkButton ID="lnkPage" runat="server" Text = '<%#Eval("Text") %>' CommandArgument = '<%# Eval("Value") %>' Enabled = '<%# Eval("Enabled") %>' OnClick = "Page_Changed_Attendance"></asp:LinkButton></td>
			</ItemTemplate>
			                 <FooterTemplate>
			                     </tr>
							</tbody></table></td>
						</tr></table>
			                 </FooterTemplate>
			</asp:Repeater>
			
			                                  </div>
			                               </div>
			                                         
			                                     </td>
			                                     </tr>
                                     <tr>
                                     <td colspan="3">
                                     <div class="fullwidth noBorder">
<asp:Button ID="btnExport" CssClass="btnSave" runat="server" Visible="false" Width="140px" ClientIDMode="Static" Text="Export To Excel" OnClientClick="Export()" onclick="btnExport_Click"></asp:Button>
       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;  <asp:Button CssClass="btnCancel" OnClientClick="Validate();" ID="btnSubmit" style="display:none;" CausesValidation="true" ClientIDMode="Static" runat="server" Text="Submit" ValidationGroup="a" OnClick="btnSubmit_Click"  />

</div>
                                     <asp:Label ID="lblView" runat="server"  Visible="false" ></asp:Label>
                                    <asp:LinkButton  onclick="lnkView_Click" ID="lnkView" Text="View" Visible="false" runat="server"></asp:LinkButton>                                  
                                     </td>
                                     </tr>                                 
                                 </table>
                                 </div>
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />
                                   <asp:AsyncPostBackTrigger ControlID="ddlMember" />

                                   <asp:PostBackTrigger ControlID="lnkView" />
                                     <asp:PostBackTrigger ControlID="btnExport" />
                               <asp:PostBackTrigger ControlID="btnSubmit" />
                               <asp:PostBackTrigger ControlID="grdMinutesReports" />
                                        <asp:PostBackTrigger ControlID="grdProceedingReport" />
                             
                         
        
    </Triggers>
</asp:UpdatePanel>
                               </div>
                                 <iframe id="txtArea1" style="display:none"></iframe>
                                   <%-- <input type="button" onclick="fnExcelReport();" value="Export to Excel"/>--%>
                               <br />
						</fieldset>
                             										
					
				</div>					
				
			</section>			
		</article>
    <div class="clearfix">
    </div>
</asp:Content>
