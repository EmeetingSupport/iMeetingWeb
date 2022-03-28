<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true"
    CodeBehind="EmailNotifications.aspx.cs" Inherits="MeetingMinder.Web.EmailNotifications" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <style type="text/css">
        h3 {
            margin-bottom:0;
        }
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
            width: 75%;
            height: 400px;
            overflow: auto;
            max-height: 70%;
            position: relative;
            top: 15%;
            bottom:10%;
            left: 22%;
            margin: 0 0 0 -10%; /* add negative left margin for half the width to center the div */
            cursor: default;
            border-radius: 4px;
            box-shadow: 0 0 5px rgba(0,0,0,0.9);
            z-index: 1000;
        }

        .close-btn {
            position: absolute;
            right: 25px;
            top: 10px;
            cursor: pointer;
        }

        .floatDiv {
            float: right;
            display: flex;           
        }

         #ContentPlaceHolder1_lnkEmailConfig {
    padding: 0px;
}
        @media screen and (-ms-high-contrast: active), (-ms-high-contrast: none) {
            .floatDiv {
                float: right;
                display: flex;
                margin-top: 40px;
            }
        }
    </style>

    <script language="javascript" type="text/javascript">
        function pageLoad(sender, args) {
            ShowPopUp();
          
        }


        function GenerateTemplates() {
            var val = $("#ddlTemplates").val();
            if (val != "0") {
                EmailTemplates(val)
            }

        }

        function EmailTemplates(val) {

            var one = "";
            var two = "";
            var three = "";
            var four = "";
            var five = "";
            var six = "";
            var seven = "";
            var eight = "";
            var nine = "";

            var ddlMeetVal = $("#ddlMeeting").val();
            var Forum = $("#ddlForum option:selected").text();
            if (ddlMeetVal != "0") {
                var MeetingDetails = ddlMeetVal.split('`');
                //                if (MeetingDetails.count > 0) {

                //                }
                var MeetingDate = MeetingDetails[0];
                var MeetingVenue = MeetingDetails[1];
                var MeetingTime = MeetingDetails[2];


                one = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	<font face='Arial, sans-serif'>Dear {Name},</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"<br />	<font face='Arial, sans-serif'>We are pleased to inform that the Agenda of the " + Forum + " Meeting of <%= strEntity %>, to be held at </font><font face='Arial, sans-serif'><u>" + MeetingTime + "</u></font><font face='Arial, sans-serif'> on </font><font face='Arial, sans-serif'><u>" + MeetingDate + "</u></font><font face='Arial, sans-serif'> has been uploaded on the iMeetings Application. You may log on to the Application to view the Agenda.</font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	<font face='Arial, sans-serif'>For any queries, please contact <b>[FullName]</b>, Contact no. <b>[ContactNo]</b> and Department <b>[DepartmentName].</b></font></p><p style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'><i><b>This is a system generated mail; please do not reply to this mail.</b></i></font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>Warm regards,<br />	 	<%= strEntity %> </font></p>";

                two = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'><font face='Arial, sans-serif'>Dear {Name},</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'><font face='Arial, sans-serif'>We wish to inform that the </font><font face='Arial, sans-serif'><u><b>date</b></u></font><font face='Arial, sans-serif'> " +
    " of the " + Forum + " Meeting of <%= strEntity %>, which was earlier scheduled for </font><font face='Arial, sans-serif'><u> ______ </u></font><font face='Arial, sans-serif'> has now been changed to </font><font face='Arial, sans-serif'><u>" + MeetingDate + "</u></font><font face='Arial, sans-serif'>. The time and venue of the Meeting remain the same.</font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>We regret the inconvenience caused to you.</font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'><font face='Arial, sans-serif'>For any queries, please contact <b>[FullName]</b>, Contact no. <b>[ContactNo]</b> and Department <b>[DepartmentName].</b></font></p><p style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p>" +
"<p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'><i><b>This is a system generated mail; please do not reply to this mail.</b></i></font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>" +
"<font face='Arial, sans-serif'>Warm regards,<br />	 	<%= strEntity %> </font></p>";

                three = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'><font face='Arial, sans-serif'>Dear {Name},</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>We wish to inform that the </font><font face='Arial, sans-serif'><u><b>venue</b></u></font><font face='Arial, sans-serif'> of the " + Forum + " Meeting of <%= strEntity %>, scheduled at <u>" + MeetingTime + "</u> on <u>" + MeetingDate + "</u> has been changed from  </font><font face='Arial, sans-serif'> __________________  to </font><font face='Arial, sans-serif'> <u>" + MeetingVenue + " </u></font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>We regret the inconvenience caused to you.</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
"<font face='Arial, sans-serif'>For any queries, please contact <b>[FullName]</b>, Contact no. <b>[ContactNo]</b> and Department <b>[DepartmentName].</b></font></p><p style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p>" +
"<p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'><i><b>This is a system generated mail; please do not reply to this mail.</b></i></font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>Warm regards,<br />" +
	" <%= strEntity %> </font></p>";

                four = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'><font face='Arial, sans-serif'>Dear {Name},</font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"<font face='Arial, sans-serif'>We wish to inform that the </font><font face='Arial, sans-serif'><u><b>time</b></u></font><font face='Arial, sans-serif'> of the " + Forum + " Meeting of <%= strEntity %>, scheduled on <u> " + MeetingDate + " </u> has been changed from </font><font face='Arial, sans-serif'><u> ________ </u></font><font face='Arial, sans-serif'> to </font><font face='Arial, sans-serif'><u>" + MeetingTime + " </u></font><font face='Arial, sans-serif'> The venue of the Meeting remains the same.</font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'><font face='Arial, sans-serif'>We regret the inconvenience caused to you.</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"<font face='Arial, sans-serif'>For any queries, please contact <b>[FullName]</b>, Contact no. <b>[ContactNo]</b> and Department <b>[DepartmentName].</b></font></p><p style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p>" +
"<p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'><i><b>This is a system generated mail; please do not reply to this mail.</b></i></font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	&nbsp;</p>" +
"<p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>Warm regards, <br />	<%= strEntity %> </font></p>";


                five = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	<font face='Arial, sans-serif'>Dear {Name},</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>We wish to inform that the " + Forum + " Meeting of <%= strEntity %>, scheduled at _______________________ will now be held at " + MeetingTime + " on " + MeetingDate + " at " + MeetingVenue + " </b></u></font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>We regret the inconvenience caused to you.</font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	<font face='Arial, sans-serif'>For any queries, please contact <b>[FullName]</b>, Contact no. <b>[ContactNo]</b> and Department <b>[DepartmentName].</b></font></p><p style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'><i><b>This is a system generated mail; please do not reply to this mail.</b></i></font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"<font face='Arial, sans-serif'>Warm regards,<br />	 	<%= strEntity %> </font></p>";

                six = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'><font face='Arial, sans-serif'>Dear {Name},</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"<br />	<font face='Arial, sans-serif'>We are pleased to inform that the Minutes of the " + Forum + " Meeting of <%= strEntity %> held on </font><font face='Arial, sans-serif'><u>" + MeetingDate + "</u></font><font face='Arial, sans-serif'> have been uploaded on the iMeetings Application. You may log on to the Application to view the Minutes.</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'><font face='Arial, sans-serif'>For any queries, please contact <b>[FullName]</b>, Contact no. <b>[ContactNo]</b> and Department <b>[DepartmentName].</b></font></p><p style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'><i><b>This is a system generated mail; please do not reply to this mail.</b></i></font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>Warm regards,<br />	 	<%= strEntity %> </font></p>";

                seven = "<p style='margin-top: 0.17in; margin-bottom: 0.17in; background: #ffffff; line-height: 150%'>" +
	"<font color='#000000'><font face='Arial, serif'><font size='2'>Dear {Name},​</font></font></font></p>" +
"<p align='JUSTIFY' style='margin-top: 0.17in; margin-bottom: 0.17in; background: #ffffff; line-height: 150%'>" +
	"<font color='#000000'><font face='Arial, serif'><font size='2'><b>NOTICE</b></font></font></font><font color='#000000'><font face='Arial, serif'><font size='2'>&nbsp;</font></font></font><font color='#000000'><font face='Arial, serif'><font size='2'>is hereby given that the meeting of the Board and it&rsquo;s Committees&nbsp;will be held on&nbsp; <u> " + MeetingDate + "</u> at <u> " + MeetingVenue + "</u>,&nbsp;as per the schedule given&nbsp;below:</font></font></font></p>" +
"<table bgcolor='#ffffff' border='1' cellpadding='0' cellspacing='0' width='624'>" +
	"<colgroup> <col width='83' /> <col width='384' /> <col width='155' />	</colgroup>	<tbody>" +
		"<tr valign='TOP'> <td bgcolor='#ffffff' width='83'> <p align='CENTER' style='margin-top: 0.08in'>" +
					"<font color='#1f497d'>&nbsp;</font><font color='#000000'><font face='Arial, serif'><font size='2'><b>S.No.</b></font></font></font></p>" +
			"</td> <td bgcolor='#ffffff' width='384'> <p align='CENTER' style='margin-top: 0.08in'>" +
			 "<font color='#000000'><font face='Arial, serif'><font size='2'><b>Meetings​</b></font></font></font></p>" +
			"</td> <td bgcolor='#ffffff' width='155'> <p align='CENTER' style='margin-top: 0.08in'>" +
			" <font color='#000000'><font face='Arial, serif'><font size='2'><b>Timings</b></font></font></font></p>" +
                "</td> </tr> <tr valign='TOP'> <td bgcolor='#ffffff' width='83'> <p align='CENTER' style='margin-top: 0.08in'>" +
                " <font color='#000000'><font face='Arial, serif'><font size='2'>1.</font></font></font></p>" +
                "</td> <td bgcolor='#ffffff' width='384'> <p style='margin-top: 0.08in'>" +
                " <font color='#000000'><font face='Arial, serif'><font size='2'>&nbsp;</font> " + Forum + "</font></font></p>" +
			"</td> <td bgcolor='#ffffff' width='155'> <p align='CENTER' style='margin-top: 0.08in'>" +
                " <font color='#000000'><font size='2'>&nbsp;</font>" + MeetingTime + "</font></p> </td> </tr>" +
                "<tr valign='TOP'><td bgcolor='#ffffff' width='83'> <p align='CENTER' style='margin-top: 0.08in'>" +
				" <font color='#000000'><font face='Arial, serif'><font size='2'>2.</font></font></font></p>" +
			"</td> <td bgcolor='#ffffff' width='384'> <p style='margin-top: 0.08in'>" +
                " <font color='#000000'><font face='Arial, serif'><font size='2'>&nbsp;</font></font></font></p>" +
                "</td><td bgcolor='#ffffff' width='155'> <p align='CENTER' style='margin-top: 0.08in'>" +
                " <font color='#000000'><font face='Arial, serif'><font size='2'>&nbsp;</font></font></font></p>" +
                "</td> </tr> <tr valign='TOP'> <td bgcolor='#ffffff' width='83'>" +
				"<p align='CENTER' style='margin-top: 0.08in'> <font color='#000000'><font face='Arial, serif'><font size='2'>3.</font></font></font></p>" +
                "</td> <td bgcolor='#ffffff' width='384'> &nbsp;</td> <td bgcolor='#ffffff' width='155'>" +
				"<p align='CENTER' style='margin-top: 0.08in'> <font color='#000000'><font face='Arial, serif'><font size='2'>&nbsp;</font></font></font></p>" +
                "</td> </tr> </tbody></table>" +
                "<p align='JUSTIFY' style='margin-top: 0.17in; margin-bottom: 0.17in; background: #ffffff; line-height: 150%'>" +
                "<font color='#000000'>&nbsp;</font><font color='#000000'><font face='Arial, serif'><font size='2'>The Agenda for the said meetings shall be sent to you shortly.</font></font></font></p>" +
                "<p align='JUSTIFY' style='margin-top: 0.17in; margin-bottom: 0.17in; background: #ffffff; line-height: 150%'>" +
                "<font color='#000000'>&nbsp;<font face='Arial, serif'><font size='2'>We request you to kindly make it convenient to attend the said meetings.</font></font></font></p>" +
                "<p style='margin-top: 0.17in; margin-bottom: 0.17in; background: #ffffff; line-height: 150%'>" +
                "	<font color='#1f497d'>&nbsp;</font><font color='#000000'><font face='Arial, serif'><font size='2'><b>For <%= strEntity %> </b></font></font></font></p>" +
"<p style='margin-top: 0.08in; background: #ffffff; line-height: 150%'>	<font color='#000000'>&nbsp;</font></p><p style='margin-bottom: 0in; background: #ffffff; line-height: 150%'>" +
"	<font color='#000000'><font face='Arial, serif'><font size='2'><b>&nbsp;</b></font></font></font></p><p style='margin-bottom: 0in; background: #ffffff; line-height: 150%'>" +
	"<font color='#000000'><font face='Arial, serif'><font size='2'><b> </b></font></font></font></p><p style='margin-top: 0.08in; line-height: 150%'>" +
	"<br />	&nbsp;</p>";

                eight = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'><font face='Arial, sans-serif'>Dear {Name},</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
   "<br />	<font face='Arial, sans-serif'>Notice is hereby given that the meeting of the " + Forum + " of <%= strEntity %> will be held on </font><font face='Arial, sans-serif'><u>" + MeetingDate + "</u>  at <u>" + MeetingVenue + "</u> </font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'><font face='Arial, sans-serif'>For any queries, please contact <b>[FullName]</b>, Contact no. <b>[ContactNo]</b> and Department <b>[DepartmentName].</b></font></p><p style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'><i><b>This is a system generated mail; please do not reply to this mail.</b></i></font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>Warm regards, <br />	<%= strEntity %> </font></p>";

                nine = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'><font face='Arial, sans-serif'>Dear {Name},</font></p>"
            }

            else {

                one = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	<font face='Arial, sans-serif'>Dear {Name},</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"<br />	<font face='Arial, sans-serif'>We are pleased to inform that the Agenda of the " + Forum + " Meeting of  <%= strEntity %> to be held at </font><font face='Arial, sans-serif'><u>9.30 a.m.</u></font><font face='Arial, sans-serif'> on </font><font face='Arial, sans-serif'><u>Monday, June 17, 2013</u></font><font face='Arial, sans-serif'> has been uploaded on the iMeetings Application. You may log on to the Application to view the Agenda.</font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	<font face='Arial, sans-serif'>For any queries, please contact <b>[FullName]</b>, Contact no. <b>[ContactNo]</b> and Department <b>[DepartmentName].</b></font></p><p style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'><i><b>This is a system generated mail; please do not reply to this mail.</b></i></font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>Warm regards, <br />	<%= strEntity %> </font></p>";

                two = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'><font face='Arial, sans-serif'>Dear {Name},</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'><font face='Arial, sans-serif'>We wish to inform that the </font><font face='Arial, sans-serif'><u><b>date</b></u></font><font face='Arial, sans-serif'>" +
    " of the " + Forum + " Meeting of <%= strEntity %>, which was earlier scheduled for </font><font face='Arial, sans-serif'><u>Monday, June 17, 2013</u></font><font face='Arial, sans-serif'> has now been changed to </font><font face='Arial, sans-serif'><u>Thursday, June 20, 2013</u></font><font face='Arial, sans-serif'>. The time and venue of the Meeting remain the same.</font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>We regret the inconvenience caused to you.</font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'><font face='Arial, sans-serif'>For any queries, please contact <b>[FullName]</b>, Contact no. <b>[ContactNo]</b> and Department <b>[DepartmentName].</b></font></p><p style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p>" +
"<p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'><i><b>This is a system generated mail; please do not reply to this mail.</b></i></font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>" +
"<font face='Arial, sans-serif'>Warm regards,<br />	 	<%= strEntity %> </font></p>";

                three = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'><font face='Arial, sans-serif'>Dear {Name},</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>We wish to inform that the </font><font face='Arial, sans-serif'><u><b>venue</b></u></font><font face='Arial, sans-serif'> of the " + Forum + " Meeting of <%= strEntity %>, scheduled at ___________ has been changed from </font><font face='Arial, sans-serif'>___________________ to </font><font face='Arial, sans-serif'>_____________________</font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>We regret the inconvenience caused to you.</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
"<font face='Arial, sans-serif'>For any queries, please contact <b>[FullName]</b>, Contact no. <b>[ContactNo]</b> and Department <b>[DepartmentName].</b></font></p><p style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p>" +
"<p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'><i><b>This is a system generated mail; please do not reply to this mail.</b></i></font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>Warm regards,<br />" +
	"Departmental Team<br />	<%= strEntity %> </font></p>";

                four = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'><font face='Arial, sans-serif'>Dear {Name},</font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"<font face='Arial, sans-serif'>We wish to inform that the </font><font face='Arial, sans-serif'><u><b>time</b></u></font><font face='Arial, sans-serif'> of the " + Forum + " Meeting of <%= strEntity %>, scheduled on Monday, June 17, 2013 has been changed from </font><font face='Arial, sans-serif'><u>9.30 a.m.</u></font><font face='Arial, sans-serif'> to</font><font face='Arial, sans-serif'><u> 12.30 p.m.</u></font><font face='Arial, sans-serif'> The venue of the Meeting remains the same.</font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'><font face='Arial, sans-serif'>We regret the inconvenience caused to you.</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"<font face='Arial, sans-serif'>For any queries, please contact <b>[FullName]</b>, Contact no. <b>[ContactNo]</b> and Department <b>[DepartmentName].</b></font></p><p style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p>" +
"<p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'><i><b>This is a system generated mail; please do not reply to this mail.</b></i></font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	&nbsp;</p>" +
"<p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>Warm regards, <br />	<%= strEntity %> </font></p>";


                five = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	<font face='Arial, sans-serif'>Dear {Name},</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>We wish to inform that the " + Forum + " Meeting of <%= strEntity %>, scheduled at _________, at ______________ will now be held at _______________</b></u></font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>We regret the inconvenience caused to you.</font></p>" +
"<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	<font face='Arial, sans-serif'>For any queries, please contact <b>[FullName]</b>, Contact no. <b>[ContactNo]</b> and Department <b>[DepartmentName].</b></font></p><p style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'><i><b>This is a system generated mail; please do not reply to this mail.</b></i></font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>	&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"<font face='Arial, sans-serif'>Warm regards,<br /> 	<%= strEntity %> </font></p>";

                six = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'><font face='Arial, sans-serif'>Dear {Name},</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"<br />	<font face='Arial, sans-serif'>We are pleased to inform that the Minutes of the Board " + Forum + " Meeting of <%= strEntity %> held on </font><font face='Arial, sans-serif'><u>May 15, 2013</u></font><font face='Arial, sans-serif'> have been uploaded on the iMeetings Application. You may log on to the Application to view the Minutes.</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'><font face='Arial, sans-serif'>For any queries, please contact <b>[FullName]</b>, Contact no. <b>[ContactNo]</b> and Department <b>[DepartmentName].</b></font></p><p style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'><i><b>This is a system generated mail; please do not reply to this mail.</b></i></font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>Warm regards,<br /> 	<%= strEntity %> </font></p>";

                seven = "<p style='margin-top: 0.17in; margin-bottom: 0.17in; background: #ffffff; line-height: 150%'>"+
	"<font color='#000000'><font face='Arial, serif'><font size='2'>Dear {Name},​</font></font></font></p>" +
"<p align='JUSTIFY' style='margin-top: 0.17in; margin-bottom: 0.17in; background: #ffffff; line-height: 150%'>"+
	"<font color='#000000'><font face='Arial, serif'><font size='2'><b>NOTICE</b></font></font></font><font color='#000000'><font face='Arial, serif'><font size='2'>&nbsp;</font></font></font><font color='#000000'><font face='Arial, serif'><font size='2'>is hereby given that the meeting of the Board and it&rsquo;s Committees&nbsp;will be held on&nbsp; <u>January 16, 2014 </u>at <u>Board Room, 10th Floor, Building No.2, R-Tech Park, Nirlon Complex, Next to Hub Mall, Behind Oracle Building, Goregaon (East), Mumbai - 400063</u>,&nbsp;as per the schedule given&nbsp;below:</font></font></font></p>"+
"<table bgcolor='#ffffff' border='1' cellpadding='0' cellspacing='0' width='624'>"+
	"<colgroup> <col width='83' /> <col width='384' /> <col width='155' />	</colgroup>	<tbody>"+
		"<tr valign='TOP'> <td bgcolor='#ffffff' width='83'> <p align='CENTER' style='margin-top: 0.08in'>"+
					"<font color='#1f497d'>&nbsp;</font><font color='#000000'><font face='Arial, serif'><font size='2'><b>S.No.</b></font></font></font></p>"+
			"</td> <td bgcolor='#ffffff' width='384'> <p align='CENTER' style='margin-top: 0.08in'>"+
			 "<font color='#000000'><font face='Arial, serif'><font size='2'><b>Meetings​</b></font></font></font></p>"+
			"</td> <td bgcolor='#ffffff' width='155'> <p align='CENTER' style='margin-top: 0.08in'>"+
			" <font color='#000000'><font face='Arial, serif'><font size='2'><b>Timings</b></font></font></font></p>"+
                "</td> </tr> <tr valign='TOP'> <td bgcolor='#ffffff' width='83'> <p align='CENTER' style='margin-top: 0.08in'>"+
                " <font color='#000000'><font face='Arial, serif'><font size='2'>1.</font></font></font></p>"+
                "</td> <td bgcolor='#ffffff' width='384'> <p style='margin-top: 0.08in'>"+
                " <font color='#000000'><font face='Arial, serif'><font size='2'>&nbsp;</font></font></font></p>"+
			"</td> <td bgcolor='#ffffff' width='155'> <p align='CENTER' style='margin-top: 0.08in'>"+
                " <font color='#000000'><font size='2'>&nbsp;</font></font></p> </td> </tr>"+
                "<tr valign='TOP'><td bgcolor='#ffffff' width='83'> <p align='CENTER' style='margin-top: 0.08in'>"+
				" <font color='#000000'><font face='Arial, serif'><font size='2'>2.</font></font></font></p>"+
			"</td> <td bgcolor='#ffffff' width='384'> <p style='margin-top: 0.08in'>"+
                " <font color='#000000'><font face='Arial, serif'><font size='2'>&nbsp;</font></font></font></p>"+
                "</td><td bgcolor='#ffffff' width='155'> <p align='CENTER' style='margin-top: 0.08in'>"+
                " <font color='#000000'><font face='Arial, serif'><font size='2'>&nbsp;</font></font></font></p>"+
                "</td> </tr> <tr valign='TOP'> <td bgcolor='#ffffff' width='83'>"+
				"<p align='CENTER' style='margin-top: 0.08in'> <font color='#000000'><font face='Arial, serif'><font size='2'>3.</font></font></font></p>"+
                "</td> <td bgcolor='#ffffff' width='384'> &nbsp;</td> <td bgcolor='#ffffff' width='155'>"+
				"<p align='CENTER' style='margin-top: 0.08in'> <font color='#000000'><font face='Arial, serif'><font size='2'>&nbsp;</font></font></font></p>"+
                "</td> </tr> </tbody></table>"+
                "<p align='JUSTIFY' style='margin-top: 0.17in; margin-bottom: 0.17in; background: #ffffff; line-height: 150%'>"+
                "<font color='#000000'>&nbsp;</font><font color='#000000'><font face='Arial, serif'><font size='2'>The Agenda for the said meetings shall be sent to you shortly.</font></font></font></p>"+
                "<p align='JUSTIFY' style='margin-top: 0.17in; margin-bottom: 0.17in; background: #ffffff; line-height: 150%'>"+
                "<font color='#000000'>&nbsp;<font face='Arial, serif'><font size='2'>We request you to kindly make it convenient to attend the said meetings.</font></font></font></p>"+
                "<p style='margin-top: 0.17in; margin-bottom: 0.17in; background: #ffffff; line-height: 150%'>"+
                "	<font color='#1f497d'>&nbsp;</font><font color='#000000'><font face='Arial, serif'><font size='2'><b>For <%= strEntity %> </b></font></font></font></p>"+
"<p style='margin-top: 0.08in; background: #ffffff; line-height: 150%'>	<font color='#000000'>&nbsp;</font></p><p style='margin-bottom: 0in; background: #ffffff; line-height: 150%'>"+
"	<font color='#000000'><font face='Arial, serif'><font size='2'><b>&nbsp;</b></font></font></font></p><p style='margin-bottom: 0in; background: #ffffff; line-height: 150%'>"+
	"<font color='#000000'><font face='Arial, serif'><font size='2'><b> </b></font></font></font></p><p style='margin-top: 0.08in; line-height: 150%'>"+
	"<br />	&nbsp;</p>";

                eight = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'><font face='Arial, sans-serif'>Dear {Name},</font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
  "<br />	<font face='Arial, sans-serif'>Notice is hereby given that the meeting of the " + Forum + " of <%= strEntity %> will be held on </font><font face='Arial, sans-serif'><u>_____________________</u>  at <u>______________________</u> </font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"&nbsp;</p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'><font face='Arial, sans-serif'>For any queries, please contact <b>[FullName]</b>, Contact no. <b>[ContactNo]</b> and Department <b>[DepartmentName].</b></font></p><p style='margin-left: 0.5in; margin-bottom: 0in'>" +
	"&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'><i><b>This is a system generated mail; please do not reply to this mail.</b></i></font></p><p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'>" +
	"&nbsp;</p><p style='margin-left: 0.5in; margin-bottom: 0in'>	<font face='Arial, sans-serif'>Warm regards,<br /> 	<%= strEntity %> </font></p>";

                nine = "<p align='JUSTIFY' style='margin-left: 0.5in; margin-bottom: 0in; line-height: 100%'><font face='Arial, sans-serif'>Dear {Name},</font></p>"
            }

            switch (val) {
                case "1":
                    CKEDITOR.instances.txtEmailBo.setData(one);
                    $('#txtSubject').val('Upload of Agenda for Board/Committee Meeting');
                    break;
                case "2":
                    CKEDITOR.instances.txtEmailBo.setData(two);
                    $('#txtSubject').val('Date Change for Board/Committee Meeting');
                    break;

                case "3":
                    CKEDITOR.instances.txtEmailBo.setData(three);
                    $('#txtSubject').val('Venue Change for Board/Committee Meeting');
                    break;
                case "4":
                    CKEDITOR.instances.txtEmailBo.setData(four);
                    $('#txtSubject').val('Time Change for Board/Committee Meeting');
                    break;

                case "5":
                    CKEDITOR.instances.txtEmailBo.setData(five);
                    $('#txtSubject').val('Change in Board/Committee Meeting schedule');
                    break;
                case "6":
                    CKEDITOR.instances.txtEmailBo.setData(six);
                    $('#txtSubject').val('Minutes of the Board/Committee Meeting');
                    break;
                case "7":
                    CKEDITOR.instances.txtEmailBo.setData(seven);
                    $('#txtSubject').val('NOTICE FOR BOARD AND COMMITTEE MEETINGS');
                    break;

                case "8":
                    CKEDITOR.instances.txtEmailBo.setData(eight);
                    $('#txtSubject').val('NOTICE FOR BOARD AND COMMITTEE MEETINGS');
                    break;

                case "9":
                    CKEDITOR.instances.txtEmailBo.setData(nine);
                    //$('#txtSubject').val('NOTICE FOR BOARD AND COMMITTEE MEETINGS');
                    break;

                default:
                    CKEDITOR.instances.txtEmailBo.setData('');
                    $('#txtSubject').val('');
                    break;
                //      CKEDITOR.instances.txtEmail.setData(three);     
            }
        }

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
                $("#lblChars").attr("style", "display:block");
                $("#lblChars").html(charLeft + " character remaining");
            }
            else {
                $("#lblChars").attr("style", "display:none");
            }
        }

        function HidePopUp() {
            $("#hdnPopUp").val("0");
            $("#divEmailConfig").hide();
        }

        function ShowPopUp() {
            if ($("#hdnPopUp").val() == "1") {
                $("#divEmailConfig").show();
            }
            return false;
        }

        function specialcharecterValidation(oSrc, args) {
            
            var returnVal = true;
            var v = 0;
           
            $('input[type=text], textarea').each(function () {
                var iChars = '<<()&;‘"/\*;:={}`%+^!-';
                if (this.id != 'txtEmailBo') {

                    var data = document.getElementById(this.id).value;

                    var re = /\\x(0|1)[1|0-9]|(\\x20)/i;

                    if (re.test(data)) {

                        v++;
                        returnVal = false;
                    }

                    for (var i = 0; i < data.length; i++) {

                        if (iChars.indexOf(data.charAt(i)) != -1) {
                            v++;
                            document.getElementById(this.id).value = "";

                            returnVal = false;
                        }
                    }
                }
            });
            if (v != 0) {
                alert("Your input has special characters. \nThese are not allowed.");
            }
            args.IsValid = returnVal;
        }
    </script>
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Email Notification--%></h2>			
			</header>
			<section>  
                
                                             
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>Email Notification</b></font>

                                 <%--<div style="float:right; margin-top:-15px;display:none;">
                                          <asp:LinkButton ID="lnkEmailConfig" runat="server" Text="Configure" OnClick="lnkEmailConfig_Click"></asp:LinkButton>
                                          </div>--%>

							</legend>
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
        
        
<h2 class="icon users">Email Notification</h2>
        
    
</div>
                                 <table width="75%">

                                    <tr style="display:none">
                                     <td style="width:95px;">
                                    Department
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList Visible="false" ID="ddlEntity" runat="server" AutoPostBack="true" Width="36%" 
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
                                     <asp:DropDownList ID="ddlForum" runat="server"  Width="36%" 
                                          onselectedindexchanged="ddlForum_SelectedIndexChanged" ClientIDMode="Static" AutoPostBack="true"   ></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Forum" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
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
                                     <asp:DropDownList ID="ddlMeeting" ClientIDMode="Static" onclick="GenerateTemplates();" runat="server"  Width="36%"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvddlMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Meeting" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>

                                     </tr>
                                        <tr>
                                     <td style="width:95px;">
                                    Template
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlTemplates" ClientIDMode="Static" onclick="EmailTemplates(this.value);"  runat="server"  Width="36%">
                                            <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                     <asp:ListItem Value="1" Text="Upload of Agenda"></asp:ListItem>
                                      <asp:ListItem Value="2" Text="Date Change"></asp:ListItem>
                                       <asp:ListItem Value="3" Text="Venue Change"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Time Change"></asp:ListItem>
                                         <asp:ListItem Value="5" Text="Change in schedule"></asp:ListItem>
                                         <asp:ListItem Value="6" Text="Minutes Upload"></asp:ListItem>
                                         <asp:ListItem Value="8" Text="Meeting Notice"></asp:ListItem>
                                         <asp:ListItem Value="9" Text="Other"></asp:ListItem>
                                     <%--     <asp:ListItem   Value="7" Text="Meeting Notice"></asp:ListItem>--%>
                                     </asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlTemplates" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Templates" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
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
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable">
                                  <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                  <Columns>
                                  
                                         

                                      <asp:TemplateField HeaderText="Name" SortExpression="FirstName">
                                        <HeaderTemplate>
                                           Select All <asp:CheckBox ID="chkHeader" onclick="javascript: fn_select_all(this);" runat="server" />
                                                                                  </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("Suffix") +" "+Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                            <asp:Label Visible="false" ID="lblName" runat="server" Text='<%# Eval("Suffix") +" "+Eval("FirstName") +" "+Eval("MiddleName")+" "+ Eval("LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                              
                                       
                                            <asp:TemplateField  HeaderText="Email" >
                                      <ItemTemplate>
                                          <div style='<%# (((string)DataBinder.Eval(Container.DataItem,"EmailId1")) == "") ? ("display:none") : ("display:block" )%>' >
                                            <asp:CheckBox   ID="chkEmailId" runat="server"  />
                                               <asp:Label ID="lblUserEmail" runat="server" Text='<%# Eval("EmailId1") %>'></asp:Label>
                                       </div><br />
                                          <div style='<%# (((string)DataBinder.Eval(Container.DataItem,"EmailId2")) == "") ? ("display:none") : ("display:block" )%>' >
                                         
                                              <asp:CheckBox ID="chkEmailId2" runat="server"  />
                                                  <asp:Label ID="lblUserEmail1" runat="server" Text='<%# Eval("EmailId2") %>'></asp:Label>
                                                </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>    
                                    
                                                   <asp:TemplateField  HeaderText="Secretary" >
                                      <ItemTemplate>
                                                   <asp:Label ID="lblSecName" runat="server" Text='<%# Eval("SecretaryName") %>'></asp:Label>
                                                 
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    
                                         <asp:TemplateField  HeaderText="Secretary Email" >
                                      <ItemTemplate>
                                          <div style='<%# (((string)DataBinder.Eval(Container.DataItem,"SecretaryEmailID1")) == "") ? ("display:none") : ("display:block" )%>' >

                                            <asp:CheckBox ID="chkSecEmailId" runat="server" />
                                                  <asp:Label ID="lblSecEmailId1" runat="server" Text='<%# Eval("SecretaryEmailID1") %>'></asp:Label>
                                            
                                            </div> <br />
                                          <div style='<%# (((string)DataBinder.Eval(Container.DataItem,"SecretaryEmailID2")) == "") ? ("display:none") : ("display:block" )%>' >

                                              <asp:CheckBox ID="chkSecEmailId2" runat="server"  />  
                                                  <asp:Label ID="lblSecEmailId2" runat="server" Text='<%# Eval("SecretaryEmailID2") %>'></asp:Label>
                                              
                                           </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                                                      
                                 </Columns>
                                </asp:GridView>
                                     </td>
                                     </tr>
                                     
                                        <tr>
                                     <td style="width:95px;">
                                    Subject
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                    <asp:TextBox  AutoComplete="off"  ID="txtSubject" ClientIDMode="Static" Width="280px" runat="server"></asp:TextBox>
                                     <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtSubject"  Display="Dynamic" ValidationGroup="a" ErrorMessage="Subject" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                   <%--  <asp:CustomValidator ID="custValid" runat="server" ControlToValidate="txtSubject" ClientValidationFunction="specialcharecterValidation" ValidationGroup="a"  ErrorMessage="Subject/CC" Text=" ." ForeColor="Red" ></asp:CustomValidator> --%>
                                 
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
                                    <asp:TextBox  AutoComplete="off"  ClientIDMode="Static" Width="280px" ID="txtcc" runat="server"></asp:TextBox>
                                     <asp:RegularExpressionValidator ID="rgvEmail" runat="server" 
                                        ControlToValidate="txtcc" Display="Dynamic" ErrorMessage="Invalid Format" 
                                        SetFocusOnError="True" 
                                        ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(['\&quot;][^\&lt;\&gt;'\&quot;]*['\&quot;]\s*\&lt;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\&gt;))(,\s*((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(['\&quot;][^\&lt;\&gt;'\&quot;]*['\&quot;]\s*\&lt;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\&gt;)))*"></asp:RegularExpressionValidator>
 
                                 
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
                                      <CKEditor:CKEditorControl ID="txtEmailBo" ClientIDMode="Static" runat="server" Height="150px" Width="600px" ></CKEditor:CKEditorControl>                              

                    <asp:RequiredFieldValidator ID="rfvMMessage" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtEmailBo" Display="Dynamic" 
                                        ErrorMessage="Message" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                          
                                     
                                 
                                     </td>
                                     </tr>
                                        
                                              <tr>
                                     <td colspan="3">
                                     <div class="fullwidth noBorder">
<asp:Button ID="btnSend" runat="server"  Text="Send" CssClass="btnSave" ValidationGroup="a" onclick="btnSend_Click"></asp:Button>
</div>
                                    </td>
                                     </tr>            
                                 </table>
                                 </div>
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />
                        <asp:PostBackTrigger ControlID="btnSend" />
        
    </Triggers>
</asp:UpdatePanel>
                               </div>
                                   <asp:Label ID="lblAlert" runat="server" ></asp:Label>
                                 <asp:HiddenField ID="hdnPopUp" runat="server" ClientIDMode="Static" Value="0" />
                          
                               <br />
						</fieldset>
                             										
					       <div id="divEmailConfig" class='overlay-bg'> <div class='overlay-content'>
                      <fieldset style="height:90%;width:800px !important;" >
					<%--		<legend  style="line-height: 30px !important;"><font color="#054a7f"><b>Email Configuration</b></font></legend>--%>

                                                            <div class="box_top">	
<h2 class="icon users">Email Configuration </h2> <%--  <div class="text_box_top">Fields marked with asterisk (*) are required</div>	--%>
</div>            <img src="img/icons/icon_list_style_cross.png"  onclick="HidePopUp();"  alt="Close" class="close-btn" />


                             <div  >
                                  <table width="75%">
                                   <tr>
                                     <td style="width:115px;">
                                   Sender Email <label style="display:inline;margin:-2px;"><span>&nbsp;*</span></label>
                                     </td>
                                     <td style="width:20px;">
                                     :
                                     </td>
                                     <td  align="left">
                                      <asp:TextBox ID="txtSenderEmail" runat="server"  MaxLength="250"></asp:TextBox>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvSender" ControlToValidate="txtSenderEmail"   Display="Dynamic" ValidationGroup="ema" ErrorMessage="Sender Email"  ForeColor="Red">Invalid</asp:RequiredFieldValidator>
                                               </td>
                                     </tr>
                                    <tr>
                                     <td >
                                    Password
                                     </td>
                                     <td>
                                     :
                                     </td>
                                     <td  align="left">
                                         <asp:TextBox TextMode="Password"   ID="txtPassword" runat="server"  MaxLength="250"></asp:TextBox>
                                     </td>
                                     </tr>
                                     <tr>
                                     <td >
                                    Port<label  style="display:inline;margin:-2px;"><span>&nbsp;*</span></label>
                                     </td>
                                     <td>
                                     :
                                     </td>
                                     <td  align="left">
                                    
                                            <asp:TextBox ID="txtPort" runat="server"  MaxLength="250"></asp:TextBox>
                                 <asp:RequiredFieldValidator runat="server" ID="rfvPort" ControlToValidate="txtPort"   Display="Dynamic" ValidationGroup="ema" ErrorMessage="Port"  ForeColor="Red">Invalid</asp:RequiredFieldValidator>
                                         <asp:RegularExpressionValidator runat="server" ID="regPort" ControlToValidate="txtPort"   ValidationGroup="ema" Display="Dynamic" ErrorMessage="Numbers Only" ForeColor="Red" ValidationExpression="^[0-9]+$" >Numbers Only</asp:RegularExpressionValidator>
                                     </td>
                                     </tr>
                                     <tr>
                                     <td>Smtp Client<label  style="display:inline;margin:-2px;"><span>&nbsp;*</span></label></td>
                                       <td>
                                     :
                                     </td>
                                     <td  align="left">
                                      <asp:TextBox ID="txtSmtpClient" runat="server"  MaxLength="250"></asp:TextBox>
                                 <asp:RequiredFieldValidator runat="server" ID="rfvSmtpClient" ControlToValidate="txtSmtpClient"   Display="Dynamic" ValidationGroup="ema" ErrorMessage="Smtp Client"  ForeColor="Red">Invalid</asp:RequiredFieldValidator>
                              
                                     </td>
                                     </tr>
                                     <tr>
                                     <td >
                                    SSL
                                     </td>
                                     <td>
                                     :
                                     </td>
                                     <td  align="left">
                                     <asp:CheckBox ID="chkSSL" runat="server" ></asp:CheckBox>                                                                
                                                     </td>
                                     </tr>
                                      
                                     <tr align="right">
                                        <td colspan="5" align="right"> 
                                        <div class="fullwidth noBorder"> 
                                                 <asp:Button  ID="btnEmailSubmit"  CssClass="btnSave"   ValidationGroup="ema" runat="server" Text="Submit" onclick="btnEmailSubmit_Click"></asp:Button>
                                        </div>
                                           
                                 </td>
                                 </tr>
                                 </table>
 
                                 </div>
                          </fieldset>
       </div>
    </div>  
				</div>					
				
			</section>			
		</article>
    <div class="clearfix">
    </div>
</asp:Content>
