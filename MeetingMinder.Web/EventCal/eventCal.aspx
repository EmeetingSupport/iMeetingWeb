<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eventCal.aspx.cs" Inherits="MeetingMinder.Web.EventCal.eventCal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Event Calendar</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
     <%-- <link href="multiple-select.css" rel="stylesheet" type="text/css" />--%>
           

    <link href="cupertino/jquery-ui-1.7.3.custom.css" rel="stylesheet" type="text/css" />

    <link href="fullcalendar/fullcalendar.css" rel="stylesheet" type="text/css" />
  
   <script src="jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
 
      <script src="jquery/jquery-ui-1.7.3.custom.min.js" type="text/javascript"></script>
      
      <script src="jquery/jquery.qtip-1.0.0-rc3.min.js" type="text/javascript"></script>
      
    <script src="fullcalendar/fullcalendar.min.js" type="text/javascript"></script>
 
   
    <%-- <script src="scripts/calendarscript.js" type="text/javascript"></script>--%>
    <script src="scripts/SchedulerTest.js" type="text/javascript"></script>
    <script src="jquery/jquery-ui-timepicker-addon-0.6.2.min.js" type="text/javascript"></script>

       
    <script type="text/javascript">


        // This function will execute after file uploaded successfully
        function uploadComplete() {
            document.getElementById('<%=lblMsg.ClientID %>').innerHTML = "Photo Uploaded Successfully";
        }
        // This function will execute if file upload fails
        function uploadError() {
            document.getElementById('<%=lblMsg.ClientID %>').innerHTML = "Photo upload Failed.";
        }

        function uploadCompleteAttach() {
            document.getElementById('<%=lblMsgFile.ClientID %>').innerHTML = "File Uploaded Successfully";
        }
        // This function will execute if file upload fails
        function uploadErrorAttach() {
            document.getElementById('<%=lblMsgFile.ClientID %>').innerHTML = "File upload Failed.";
        }


        function uploadCompleteAdd() {
            document.getElementById('<%=lblAddMsg.ClientID %>').innerHTML = "Photo Uploaded Successfully";
        }

        function uploadErrorAdd() {
            document.getElementById('<%=lblAddMsg.ClientID %>').innerHTML = "Photo upload Failed.";
        }

        function uploadCompleteAttachAdd() {
            document.getElementById('<%=lblAddMsgFile.ClientID %>').innerHTML = "File Uploaded Successfully";
        }

        function uploadErrorAttachAdd() {
            document.getElementById('<%=lblAddMsgFile.ClientID %>').innerHTML = "File upload Failed.";
        }

        function Add() {

            var isValid = false;
            var entId = $("#eventId").val();
            if (entId != "") {
                isValid = Page_ClientValidate('update');
                if (isValid) {
                    $("#btnAdd").click();
                }

            }
            else {

                isValid = Page_ClientValidate('add');
                if (isValid) {
                    $("#btnAdd").click();
                }
            }


        }

        function Update() {
            $("#btnUpdate").click();
        }

        function FileUp() {
            $("#fileUploadPhotosAdd").click();
        }

        function FileUpAttach() {
            $("#fileUploadAttachmentsAdd").click();
        }

        function deleteRow(ctrl) {
            $(ctrl.parentNode.parentNode).remove();
        }
        var cnt = 2;
        function addAttendis() {
            var txtName = $("#txtName").val();
            var txtDesignation = $("#txtDesignation").val();
            var txtOrganization = $("#txtOrganization").val();
            var fuPhoto = $("#fuPhoto").val();
            var txtTwitter = $("#txtTwitter").val();
            var file = $("#fuPhoto").clone(true);
            //<input id="fuPhoto' + cnt + '" value="' + fuPhoto + '" type="file" />
            $('#tblAtten  tr:last-child').after('<tr><td><input name="txtName" value= "' + txtName + '" type="text" /></td><td><input name="txtDesignation"  value= "' + txtDesignation + '"  type="text" /></td><td><input name="txtOrganization"  type="text"  value= "' + txtOrganization + '"  /></td><td id="tr' + cnt + '"></td><td><input  value= "' + txtTwitter + '"  name="txtTwitter" type="text" /><input type="button" title="Remove" value="Remove" onclick="RemoveRow();" /></td></tr>');
            $("#tr" + cnt).html(file[0]);
            $("#fuPhoto").val('');
            cnt++;
        }

        function RemoveRow() {

        }

    
        
    </script>
    <style type='text/css'>
        body {
            margin-top: 40px;
            text-align: center;
            font-size: 14px;
            font-family: "Lucida Grande",Helvetica,Arial,Verdana,sans-serif;
        }

        #calendar {
            width: 900px;
            margin: 0 auto;
        }
        /* css for timepicker */
        .ui-timepicker-div dl {
            text-align: left;
        }

            .ui-timepicker-div dl dt {
                height: 25px;
            }

            .ui-timepicker-div dl dd {
                margin: -25px 0 10px 65px;
            }

        .style1 {
            width: 100%;
        }

        /* table fields alignment*/
        .alignRight {
            text-align: right;
            padding-right: 10px;
            padding-bottom: 10px;
        }

        .alignLeft {
            text-align: left;
            padding-bottom: 10px;
        }
        
        .hiddenEvent{display: none;}
.fc-other-month .fc-day-number { display:none;}

td.fc-other-month .fc-day-number {
     visibility: hidden;
     display:none;
}


    </style>


</head>
<body>   
    <form id="form1" runat="server">
     
        <asp:HiddenField ID="hdnAtttendees" ClientIDMode="Static" runat="server" />
   

        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" EnablePageMethods="true" runat="server">
        </asp:ToolkitScriptManager>
        <a href="../default.aspx" >Back To Menu</a><br />
        <div id="calendar">
        </div>
        <div id="updatedialog" style="font: 70% 'Trebuchet MS', sans-serif; margin: 50px;"
            title="Update or Delete Event">
        </div>
        <div id="addDialog" style="font: 70% 'Trebuchet MS', sans-serif; margin: 50px;" title="Add Meeting">
            <table id="tblAdd" cellpadding="0" class="style1">
                <tr style="display:none">
                    <td class="alignRight">Name of Visitor:</td>
                    <td class="alignLeft">
                        <asp:TextBox ID="txtAddNameOfVisitor" ClientIDMode="Static" runat="server"></asp:TextBox><br />
                    </td>
                </tr>
                <tr  style="display:none">
                    <td class="alignRight">Designation of Visitor:</td>
                    <td class="alignLeft">
                        <asp:TextBox ID="txtAddDesignOfVisitor" ClientIDMode="Static" runat="server"></asp:TextBox><br />
                    </td>
                </tr>
                <tr style="display:none">
                    <td class="alignRight">Name of Visitor Organisation:</td>
                    <td class="alignLeft">

                        <asp:TextBox  ID="txtAddNameOfVisitorOrg" ClientIDMode="Static" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td class="alignRight">Meeting Title * :</td>
                    <td class="alignLeft">
                        <asp:TextBox ID="txtAddAppointment" ClientIDMode="Static" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                        <asp:RequiredFieldValidator runat="server" ID="rfvtxtAddAppoint" ControlToValidate="txtAddAppointment"  Display="Dynamic" ValidationGroup="add" ErrorMessage="Appointment" Text="Inavalid" ForeColor="Red"></asp:RequiredFieldValidator>

                    </td>
                </tr>
         
                <tr style="display:none">
                    <td class="alignRight">Upload Photo:</td>
                    <td class="alignLeft">
                        <%--        <input  type="button" value="Browse..." id="fileUploadPhotAdds" /><span id="spnPhoto">No file selected</span>--%>
                        <asp:FileUpload Visible="false" ID="fileUploadPhotosAdd" ClientIDMode="Static" runat="server" />
                        <%--      <input type="file" id="fileUploadPhotAdd" onclick="FileUp();" runat="server" />--%>
                        <%--<asp:FileUpload ID="fileUploadPhotAdd" onclick="e.preventdefault(); FileUp();" runat="server" />--%>
                        <%--<asp:AsyncFileUpload ID="fileUploadPhotoAdd" 
                        OnClientUploadComplete="uploadCompleteAdd" OnClientUploadError="uploadErrorAdd" CompleteBackColor="White" 
                runat="server" UploaderStyle="Modern" UploadingBackColor="#CCFFFF" 
                        ThrobberID="imgLoad" onuploadedcomplete="fileUploadPhotoAdd_UploadedComplete"/>
                        --%>
                        <br />
                        <asp:Label ID="lblAddMsg" runat="server" Text=""></asp:Label><br />

                    </td>

                </tr>

       
                <tr>
                    <td class="alignRight">Meeting Description:</td>
                    <td class="alignLeft">
                        <asp:TextBox ID="AddtxtInformation" TextMode="MultiLine" ClientIDMode="Static" runat="server" ></asp:TextBox><br />
                    </td>
                </tr>
                         <tr>
                    <td class="alignRight">Attachments:</td>
                    <td class="alignLeft">
                        <asp:FileUpload ID="fileUploadAttachmentsAdd" ClientIDMode="Static" runat="server" />
                        <%--       <input type="button" value="Browse..." id="fuAttach" /><span id="spnAttach">No file selected</span>--%>
                        <%--<asp:AsyncFileUpload ID="fileUploadAttachmentsAdd" OnClientUploadComplete="uploadCompleteAttachAdd" OnClientUploadError="uploadErrorAttachAdd" CompleteBackColor="White" 
                runat="server" UploaderStyle="Modern" UploadingBackColor="#CCFFFF" ThrobberID="imgLoad"/>--%>
                        <br />
                        <asp:Label ID="lblAddMsgFile" runat="server" Text=""></asp:Label><br />

                    </td>

                </tr>
                <tr>
                    <td class="alignRight">start:</td>
                    <td class="alignLeft">
                   <%--     <select id="ddlStartTime" runat="server">
                            <option value="0">00:00</option>
                            <option value="5">00:05</option>
                            <option value="10">00:10</option>
                            <option value="15">00:15</option>
                            <option value="20">00:20</option>
                            <option value="25">00:25</option>
                            <option value="30">00:30</option>
                            <option value="35">00:35</option>
                            <option value="40">00:40</option>
                            <option value="45">00:45</option>
                            <option value="50">00:50</option>
                            <option value="55">00:55</option>
                            <option value="60">01:00</option>
                            <option value="65">01:05</option>
                            <option value="70">01:10</option>
                            <option value="75">01:15</option>
                            <option value="80">01:20</option>
                            <option value="85">01:25</option>
                            <option value="90">01:30</option>
                            <option value="95">01:35</option>
                            <option value="100">01:40</option>
                            <option value="105">01:45</option>
                            <option value="110">01:50</option>
                            <option value="115">01:55</option>
                            <option value="120">02:00</option>
                            <option value="125">02:05</option>
                            <option value="130">02:10</option>
                            <option value="135">02:15</option>
                            <option value="140">02:20</option>
                            <option value="145">02:25</option>
                            <option value="150">02:30</option>
                            <option value="155">02:35</option>
                            <option value="160">02:40</option>
                            <option value="165">02:45</option>
                            <option value="170">02:50</option>
                            <option value="175">02:55</option>
                            <option value="180">03:00</option>
                            <option value="185">03:05</option>
                            <option value="190">03:10</option>
                            <option value="195">03:15</option>
                            <option value="200">03:20</option>
                            <option value="205">03:25</option>
                            <option value="210">03:30</option>
                            <option value="215">03:35</option>
                            <option value="220">03:40</option>
                            <option value="225">03:45</option>
                            <option value="230">03:50</option>
                            <option value="235">03:55</option>
                            <option value="240">04:00</option>
                            <option value="245">04:05</option>
                            <option value="250">04:10</option>
                            <option value="255">04:15</option>
                            <option value="260">04:20</option>
                            <option value="265">04:25</option>
                            <option value="270">04:30</option>
                            <option value="275">04:35</option>
                            <option value="280">04:40</option>
                            <option value="285">04:45</option>
                            <option value="290">04:50</option>
                            <option value="295">04:55</option>
                            <option value="300">05:00</option>
                            <option value="305">05:05</option>
                            <option value="310">05:10</option>
                            <option value="315">05:15</option>
                            <option value="320">05:20</option>
                            <option value="325">05:25</option>
                            <option value="330">05:30</option>
                            <option value="335">05:35</option>
                            <option value="340">05:40</option>
                            <option value="345">05:45</option>
                            <option value="350">05:50</option>
                            <option value="355">05:55</option>
                            <option value="360">06:00</option>
                            <option value="365">06:05</option>
                            <option value="370">06:10</option>
                            <option value="375">06:15</option>
                            <option value="380">06:20</option>
                            <option value="385">06:25</option>
                            <option value="390">06:30</option>
                            <option value="395">06:35</option>
                            <option value="400">06:40</option>
                            <option value="405">06:45</option>
                            <option value="410">06:50</option>
                            <option value="415">06:55</option>
                            <option value="420">07:00</option>
                            <option value="425">07:05</option>
                            <option value="430">07:10</option>
                            <option value="435">07:15</option>
                            <option value="440">07:20</option>
                            <option value="445">07:25</option>
                            <option value="450">07:30</option>
                            <option value="455">07:35</option>
                            <option value="460">07:40</option>
                            <option value="465">07:45</option>
                            <option value="470">07:50</option>
                            <option value="475">07:55</option>
                            <option value="480">08:00</option>
                            <option value="485">08:05</option>
                            <option value="490">08:10</option>
                            <option value="495">08:15</option>
                            <option value="500">08:20</option>
                            <option value="505">08:25</option>
                            <option value="510">08:30</option>
                            <option value="515">08:35</option>
                            <option value="520">08:40</option>
                            <option value="525">08:45</option>
                            <option value="530">08:50</option>
                            <option value="535">08:55</option>
                            <option value="540">09:00</option>
                            <option value="545">09:05</option>
                            <option value="550">09:10</option>
                            <option value="555">09:15</option>
                            <option value="560">09:20</option>
                            <option value="565">09:25</option>
                            <option value="570">09:30</option>
                            <option value="575">09:35</option>
                            <option value="580">09:40</option>
                            <option value="585">09:45</option>
                            <option value="590">09:50</option>
                            <option value="595">09:55</option>
                            <option value="600">10:00</option>
                            <option value="605">10:05</option>
                            <option value="610">10:10</option>
                            <option value="615">10:15</option>
                            <option value="620">10:20</option>
                            <option value="625">10:25</option>
                            <option value="630">10:30</option>
                            <option value="635">10:35</option>
                            <option value="640">10:40</option>
                            <option value="645">10:45</option>
                            <option value="650">10:50</option>
                            <option value="655">10:55</option>
                            <option value="660">11:00</option>
                            <option value="665">11:05</option>
                            <option value="670">11:10</option>
                            <option value="675">11:15</option>
                            <option value="680">11:20</option>
                            <option value="685">11:25</option>
                            <option value="690">11:30</option>
                            <option value="695">11:35</option>
                            <option value="700">11:40</option>
                            <option value="705">11:45</option>
                            <option value="710">11:50</option>
                            <option value="715">11:55</option>
                            <option value="720">12:00</option>
                            <option value="725">12:05</option>
                            <option value="730">12:10</option>
                            <option value="735">12:15</option>
                            <option value="740">12:20</option>
                            <option value="745">12:25</option>
                            <option value="750">12:30</option>
                            <option value="755">12:35</option>
                            <option value="760">12:40</option>
                            <option value="765">12:45</option>
                            <option value="770">12:50</option>
                            <option value="775">12:55</option>
                            <option value="780">13:00</option>
                            <option value="785">13:05</option>
                            <option value="790">13:10</option>
                            <option value="795">13:15</option>
                            <option value="800">13:20</option>
                            <option value="805">13:25</option>
                            <option value="810">13:30</option>
                            <option value="815">13:35</option>
                            <option value="820">13:40</option>
                            <option value="825">13:45</option>
                            <option value="830">13:50</option>
                            <option value="835">13:55</option>
                            <option value="840">14:00</option>
                            <option value="845">14:05</option>
                            <option value="850">14:10</option>
                            <option value="855">14:15</option>
                            <option value="860">14:20</option>
                            <option value="865">14:25</option>
                            <option value="870">14:30</option>
                            <option value="875">14:35</option>
                            <option value="880">14:40</option>
                            <option value="885">14:45</option>
                            <option value="890">14:50</option>
                            <option value="895">14:55</option>
                            <option value="900">15:00</option>
                            <option value="905">15:05</option>
                            <option value="910">15:10</option>
                            <option value="915">15:15</option>
                            <option value="920">15:20</option>
                            <option value="925">15:25</option>
                            <option value="930">15:30</option>
                            <option value="935">15:35</option>
                            <option value="940">15:40</option>
                            <option value="945">15:45</option>
                            <option value="950">15:50</option>
                            <option value="955">15:55</option>
                            <option value="960">16:00</option>
                            <option value="965">16:05</option>
                            <option value="970">16:10</option>
                            <option value="975">16:15</option>
                            <option value="980">16:20</option>
                            <option value="985">16:25</option>
                            <option value="990">16:30</option>
                            <option value="995">16:35</option>
                            <option value="1000">16:40</option>
                            <option value="1005">16:45</option>
                            <option value="1010">16:50</option>
                            <option value="1015">16:55</option>
                            <option value="1020">17:00</option>
                            <option value="1025">17:05</option>
                            <option value="1030">17:10</option>
                            <option value="1035">17:15</option>
                            <option value="1040">17:20</option>
                            <option value="1045">17:25</option>
                            <option value="1050">17:30</option>
                            <option value="1055">17:35</option>
                            <option value="1060">17:40</option>
                            <option value="1065">17:45</option>
                            <option value="1070">17:50</option>
                            <option value="1075">17:55</option>
                            <option value="1080">18:00</option>
                            <option value="1085">18:05</option>
                            <option value="1090">18:10</option>
                            <option value="1095">18:15</option>
                            <option value="1100">18:20</option>
                            <option value="1105">18:25</option>
                            <option value="1110">18:30</option>
                            <option value="1115">18:35</option>
                            <option value="1120">18:40</option>
                            <option value="1125">18:45</option>
                            <option value="1130">18:50</option>
                            <option value="1135">18:55</option>
                            <option value="1140">19:00</option>
                            <option value="1145">19:05</option>
                            <option value="1150">19:10</option>
                            <option value="1155">19:15</option>
                            <option value="1160">19:20</option>
                            <option value="1165">19:25</option>
                            <option value="1170">19:30</option>
                            <option value="1175">19:35</option>
                            <option value="1180">19:40</option>
                            <option value="1185">19:45</option>
                            <option value="1190">19:50</option>
                            <option value="1195">19:55</option>
                            <option value="1200">20:00</option>
                            <option value="1205">20:05</option>
                            <option value="1210">20:10</option>
                            <option value="1215">20:15</option>
                            <option value="1220">20:20</option>
                            <option value="1225">20:25</option>
                            <option value="1230">20:30</option>
                            <option value="1235">20:35</option>
                            <option value="1240">20:40</option>
                            <option value="1245">20:45</option>
                            <option value="1250">20:50</option>
                            <option value="1255">20:55</option>
                            <option value="1260">21:00</option>
                            <option value="1265">21:05</option>
                            <option value="1270">21:10</option>
                            <option value="1275">21:15</option>
                            <option value="1280">21:20</option>
                            <option value="1285">21:25</option>
                            <option value="1290">21:30</option>
                            <option value="1295">21:35</option>
                            <option value="1300">21:40</option>
                            <option value="1305">21:45</option>
                            <option value="1310">21:50</option>
                            <option value="1315">21:55</option>
                            <option value="1320">22:00</option>
                            <option value="1325">22:05</option>
                            <option value="1330">22:10</option>
                            <option value="1335">22:15</option>
                            <option value="1340">22:20</option>
                            <option value="1345">22:25</option>
                            <option value="1350">22:30</option>
                            <option value="1355">22:35</option>
                            <option value="1360">22:40</option>
                            <option value="1365">22:45</option>
                            <option value="1370">22:50</option>
                            <option value="1375">22:55</option>
                            <option value="1380">23:00</option>
                            <option value="1385">23:05</option>
                            <option value="1390">23:10</option>
                            <option value="1395">23:15</option>
                            <option value="1400">23:20</option>
                            <option value="1405">23:25</option>
                            <option value="1410">23:30</option>
                            <option value="1415">23:35</option>
                            <option value="1420">23:40</option>
                            <option value="1425">23:45</option>
                            <option value="1430">23:50</option>
                            <option value="1435">23:55</option>
                        </select>--%>
                                                <select id="ddlStartTime" runat="server">
                                                <option value="00:00:00">12:00 am</option>
                            <option value="00:05:00">12:05 am</option>
                            <option value="00:10:00">12:10 am</option>
                            <option value="00:15:00">12:15 am</option>
                            <option value="00:20:00">12:20 am</option>
                            <option value="00:25:00">12:25 am</option>
                            <option value="00:30:00">12:30 am</option>
                            <option value="00:35:00">12:35 am</option>
                            <option value="00:40:00">12:40 am</option>
                            <option value="00:45:00">12:45 am</option>
                            <option value="00:50:00">12:50 am</option>
                            <option value="00:55:00">12:55 am</option>
                            <option value="01:00:00">01:00 am</option>
                            <option value="01:05:00">01:05 am</option>
                            <option value="01:10:00">01:10 am</option>
                            <option value="01:15:00">01:15 am</option>
                            <option value="01:20:00">01:20 am</option>
                            <option value="01:25:00">01:25 am</option>
                            <option value="01:30:00">01:30 am</option>
                            <option value="01:35:00">01:35 am</option>
                            <option value="01:40:00">01:40 am</option>
                            <option value="01:45:00">01:45 am</option>
                            <option value="01:50:00">01:50 am</option>
                            <option value="01:55:00">01:55 am</option>
                            <option value="02:00:00">02:00 am</option>
                            <option value="02:05:00">02:05 am</option>
                            <option value="02:10:00">02:10 am</option>
                            <option value="02:15:00">02:15 am</option>
                            <option value="02:20:00">02:20 am</option>
                            <option value="02:25:00">02:25 am</option>
                            <option value="02:30:00">02:30 am</option>
                            <option value="02:35:00">02:35 am</option>
                            <option value="02:40:00">02:40 am</option>
                            <option value="02:45:00">02:45 am</option>
                            <option value="02:50:00">02:50 am</option>
                            <option value="02:55:00">02:55 am</option>
                            <option value="03:00:00">03:00 am</option>
                            <option value="03:05:00">03:05 am</option>
                            <option value="03:10:00">03:10 am</option>
                            <option value="03:15:00">03:15 am</option>
                            <option value="03:20:00">03:20 am</option>
                            <option value="03:25:00">03:25 am</option>
                            <option value="03:30:00">03:30 am</option>
                            <option value="03:35:00">03:35 am</option>
                            <option value="03:40:00">03:40 am</option>
                            <option value="03:45:00">03:45 am</option>
                            <option value="03:50:00">03:50 am</option>
                            <option value="03:55:00">03:55 am</option>
                            <option value="04:00:00">04:00 am</option>
                            <option value="04:05:00">04:05 am</option>
                            <option value="04:10:00">04:10 am</option>
                            <option value="04:15:00">04:15 am</option>
                            <option value="04:20:00">04:20 am</option>
                            <option value="04:25:00">04:25 am</option>
                            <option value="04:30:00">04:30 am</option>
                            <option value="04:35:00">04:35 am</option>
                            <option value="04:40:00">04:40 am</option>
                            <option value="04:45:00">04:45 am</option>
                            <option value="04:50:00">04:50 am</option>
                            <option value="04:55:00">04:55 am</option>
                            <option value="05:00:00">05:00 am</option>
                            <option value="05:05:00">05:05 am</option>
                            <option value="05:10:00">05:10 am</option>
                            <option value="05:15:00">05:15 am</option>
                            <option value="05:20:00">05:20 am</option>
                            <option value="05:25:00">05:25 am</option>
                            <option value="05:30:00">05:30 am</option>
                            <option value="05:35:00">05:35 am</option>
                            <option value="05:40:00">05:40 am</option>
                            <option value="05:45:00">05:45 am</option>
                            <option value="05:50:00">05:50 am</option>
                            <option value="05:55:00">05:55 am</option>
                            <option value="06:00:00">06:00 am</option>
                            <option value="06:05:00">06:05 am</option>
                            <option value="06:10:00">06:10 am</option>
                            <option value="06:15:00">06:15 am</option>
                            <option value="06:20:00">06:20 am</option>
                            <option value="06:25:00">06:25 am</option>
                            <option value="06:30:00">06:30 am</option>
                            <option value="06:35:00">06:35 am</option>
                            <option value="06:40:00">06:40 am</option>
                            <option value="06:45:00">06:45 am</option>
                            <option value="06:50:00">06:50 am</option>
                            <option value="06:55:00">06:55 am</option>
                            <option value="07:00:00">07:00 am</option>
                            <option value="07:05:00">07:05 am</option>
                            <option value="07:10:00">07:10 am</option>
                            <option value="07:15:00">07:15 am</option>
                            <option value="07:20:00">07:20 am</option>
                            <option value="07:25:00">07:25 am</option>
                            <option value="07:30:00">07:30 am</option>
                            <option value="07:35:00">07:35 am</option>
                            <option value="07:40:00">07:40 am</option>
                            <option value="07:45:00">07:45 am</option>
                            <option value="07:50:00">07:50 am</option>
                            <option value="07:55:00">07:55 am</option>
                            <option value="08:00:00">08:00 am</option>
                            <option value="08:05:00">08:05 am</option>
                            <option value="08:10:00">08:10 am</option>
                            <option value="08:15:00">08:15 am</option>
                            <option value="08:20:00">08:20 am</option>
                            <option value="08:25:00">08:25 am</option>
                            <option value="08:30:00">08:30 am</option>
                            <option value="08:35:00">08:35 am</option>
                            <option value="08:40:00">08:40 am</option>
                            <option value="08:45:00">08:45 am</option>
                            <option value="08:50:00">08:50 am</option>
                            <option value="08:55:00">08:55 am</option>
                            <option value="09:00:00">09:00 am</option>
                            <option value="09:05:00">09:05 am</option>
                            <option value="09:10:00">09:10 am</option>
                            <option value="09:15:00">09:15 am</option>
                            <option value="09:20:00">09:20 am</option>
                            <option value="09:25:00">09:25 am</option>
                            <option value="09:30:00">09:30 am</option>
                            <option value="09:35:00">09:35 am</option>
                            <option value="09:40:00">09:40 am</option>
                            <option value="09:45:00">09:45 am</option>
                            <option value="09:50:00">09:50 am</option>
                            <option value="09:55:00">09:55 am</option>
                            <option value="10:00:00">10:00 am</option>
                            <option value="10:05:00">10:05 am</option>
                            <option value="10:10:00">10:10 am</option>
                            <option value="10:15:00">10:15 am</option>
                            <option value="10:20:00">10:20 am</option>
                            <option value="10:25:00">10:25 am</option>
                            <option value="10:30:00">10:30 am</option>
                            <option value="10:35:00">10:35 am</option>
                            <option value="10:40:00">10:40 am</option>
                            <option value="10:45:00">10:45 am</option>
                            <option value="10:50:00">10:50 am</option>
                            <option value="10:55:00">10:55 am</option>
                            <option value="11:00:00">11:00 am</option>
                            <option value="11:05:00">11:05 am</option>
                            <option value="11:10:00">11:10 am</option>
                            <option value="11:15:00">11:15 am</option>
                            <option value="11:20:00">11:20 am</option>
                            <option value="11:25:00">11:25 am</option>
                            <option value="11:30:00">11:30 am</option>
                            <option value="11:35:00">11:35 am</option>
                            <option value="11:40:00">11:40 am</option>
                            <option value="11:45:00">11:45 am</option>
                            <option value="11:50:00">11:50 am</option>
                            <option value="11:55:00">11:55 am</option>
                            <option value="12:00:00">12:00 pm</option>
                            <option value="12:05:00">12:05 pm</option>
                            <option value="12:10:00">12:10 pm</option>
                            <option value="12:15:00">12:15 pm</option>
                            <option value="12:20:00">12:20 pm</option>
                            <option value="12:25:00">12:25 pm</option>
                            <option value="12:30:00">12:30 pm</option>
                            <option value="12:35:00">12:35 pm</option>
                            <option value="12:40:00">12:40 pm</option>
                            <option value="12:45:00">12:45 pm</option>
                            <option value="12:50:00">12:50 pm</option>
                            <option value="12:55:00">12:55 pm</option>
                            <option value="13:00:00">01:00 pm</option>
                            <option value="13:05:00">01:05 pm</option>
                            <option value="13:10:00">01:10 pm</option>
                            <option value="13:15:00">01:15 pm</option>
                            <option value="13:20:00">01:20 pm</option>
                            <option value="13:25:00">01:25 pm</option>
                            <option value="13:30:00">01:30 pm</option>
                            <option value="13:35:00">01:35 pm</option>
                            <option value="13:40:00">01:40 pm</option>
                            <option value="13:45:00">01:45 pm</option>
                            <option value="13:50:00">01:50 pm</option>
                            <option value="13:55:00">01:55 pm</option>
                            <option value="14:00:00">02:00 pm</option>
                            <option value="14:05:00">02:05 pm</option>
                            <option value="14:10:00">02:10 pm</option>
                            <option value="14:15:00">02:15 pm</option>
                            <option value="14:20:00">02:20 pm</option>
                            <option value="14:25:00">02:25 pm</option>
                            <option value="14:30:00">02:30 pm</option>
                            <option value="14:35:00">02:35 pm</option>
                            <option value="14:40:00">02:40 pm</option>
                            <option value="14:45:00">02:45 pm</option>
                            <option value="14:50:00">02:50 pm</option>
                            <option value="14:55:00">02:55 pm</option>
                            <option value="15:00:00">03:00 pm</option>
                            <option value="15:05:00">03:05 pm</option>
                            <option value="15:10:00">03:10 pm</option>
                            <option value="15:15:00">03:15 pm</option>
                            <option value="15:20:00">03:20 pm</option>
                            <option value="15:25:00">03:25 pm</option>
                            <option value="15:30:00">03:30 pm</option>
                            <option value="15:35:00">03:35 pm</option>
                            <option value="15:40:00">03:40 pm</option>
                            <option value="15:45:00">03:45 pm</option>
                            <option value="15:50:00">03:50 pm</option>
                            <option value="15:55:00">03:55 pm</option>
                            <option value="16:00:00">04:00 pm</option>
                            <option value="16:05:00">04:05 pm</option>
                            <option value="16:10:00">04:10 pm</option>
                            <option value="16:15:00">04:15 pm</option>
                            <option value="16:20:00">04:20 pm</option>
                            <option value="16:25:00">04:25 pm</option>
                            <option value="16:30:00">04:30 pm</option>
                            <option value="16:35:00">04:35 pm</option>
                            <option value="16:40:00">04:40 pm</option>
                            <option value="16:45:00">04:45 pm</option>
                            <option value="16:50:00">04:50 pm</option>
                            <option value="16:55:00">04:55 pm</option>
                            <option value="17:00:00">05:00 pm</option>
                            <option value="17:05:00">05:05 pm</option>
                            <option value="17:10:00">05:10 pm</option>
                            <option value="17:15:00">05:15 pm</option>
                            <option value="17:20:00">05:20 pm</option>
                            <option value="17:25:00">05:25 pm</option>
                            <option value="17:30:00">05:30 pm</option>
                            <option value="17:35:00">05:35 pm</option>
                            <option value="17:40:00">05:40 pm</option>
                            <option value="17:45:00">05:45 pm</option>
                            <option value="17:50:00">05:50 pm</option>
                            <option value="17:55:00">05:55 pm</option>
                            <option value="18:00:00">06:00 pm</option>
                            <option value="18:05:00">06:05 pm</option>
                            <option value="18:10:00">06:10 pm</option>
                            <option value="18:15:00">06:15 pm</option>
                            <option value="18:20:00">06:20 pm</option>
                            <option value="18:25:00">06:25 pm</option>
                            <option value="18:30:00">06:30 pm</option>
                            <option value="18:35:00">06:35 pm</option>
                            <option value="18:40:00">06:40 pm</option>
                            <option value="18:45:00">06:45 pm</option>
                            <option value="18:50:00">06:50 pm</option>
                            <option value="18:55:00">06:55 pm</option>
                            <option value="19:00:00">07:00 pm</option>
                            <option value="19:05:00">07:05 pm</option>
                            <option value="19:10:00">07:10 pm</option>
                            <option value="19:15:00">07:15 pm</option>
                            <option value="19:20:00">07:20 pm</option>
                            <option value="19:25:00">07:25 pm</option>
                            <option value="19:30:00">07:30 pm</option>
                            <option value="19:35:00">07:35 pm</option>
                            <option value="19:40:00">07:40 pm</option>
                            <option value="19:45:00">07:45 pm</option>
                            <option value="19:50:00">07:50 pm</option>
                            <option value="19:55:00">07:55 pm</option>
                            <option value="20:00:00">08:00 pm</option>
                            <option value="20:05:00">08:05 pm</option>
                            <option value="20:10:00">08:10 pm</option>
                            <option value="20:15:00">08:15 pm</option>
                            <option value="20:20:00">08:20 pm</option>
                            <option value="20:25:00">08:25 pm</option>
                            <option value="20:30:00">08:30 pm</option>
                            <option value="20:35:00">08:35 pm</option>
                            <option value="20:40:00">08:40 pm</option>
                            <option value="20:45:00">08:45 pm</option>
                            <option value="20:50:00">08:50 pm</option>
                            <option value="20:55:00">08:55 pm</option>
                            <option value="21:00:00">09:00 pm</option>
                            <option value="21:05:00">09:05 pm</option>
                            <option value="21:10:00">09:10 pm</option>
                            <option value="21:15:00">09:15 pm</option>
                            <option value="21:20:00">09:20 pm</option>
                            <option value="21:25:00">09:25 pm</option>
                            <option value="21:30:00">09:30 pm</option>
                            <option value="21:35:00">09:35 pm</option>
                            <option value="21:40:00">09:40 pm</option>
                            <option value="21:45:00">09:45 pm</option>
                            <option value="21:50:00">09:50 pm</option>
                            <option value="21:55:00">09:55 pm</option>
                            <option value="22:00:00">10:00 pm</option>
                            <option value="22:05:00">10:05 pm</option>
                            <option value="22:10:00">10:10 pm</option>
                            <option value="22:15:00">10:15 pm</option>
                            <option value="22:20:00">10:20 pm</option>
                            <option value="22:25:00">10:25 pm</option>
                            <option value="22:30:00">10:30 pm</option>
                            <option value="22:35:00">10:35 pm</option>
                            <option value="22:40:00">10:40 pm</option>
                            <option value="22:45:00">10:45 pm</option>
                            <option value="22:50:00">10:50 pm</option>
                            <option value="22:55:00">10:55 pm</option>
                            <option value="23:00:00">11:00 pm</option>
                            <option value="23:05:00">11:05 pm</option>
                            <option value="23:10:00">11:10 pm</option>
                            <option value="23:15:00">11:15 pm</option>
                            <option value="23:20:00">11:20 pm</option>
                            <option value="23:25:00">11:25 pm</option>
                            <option value="23:30:00">11:30 pm</option>
                            <option value="23:35:00">11:35 pm</option>
                            <option value="23:40:00">11:40 pm</option>
                            <option value="23:45:00">11:45 pm</option>
                            <option value="23:50:00">11:50 pm</option>
                            <option value="23:55:00">11:55 pm</option>
                        </select>

                     <%--   <select id="ddlDay" runat="server">
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                            <option value="7">7</option>
                            <option value="8">8</option>
                            <option value="9">9</option>
                            <option value="10">10</option>
                            <option value="11">11</option>
                            <option value="12">12</option>
                            <option value="13">13</option>
                            <option value="14">14</option>
                            <option value="15">15</option>
                            <option value="16">16</option>
                            <option value="17">17</option>
                            <option value="18">18</option>
                            <option value="19">19</option>
                            <option value="20">20</option>
                            <option value="21">21</option>
                            <option value="22">22</option>
                            <option value="23">23</option>
                            <option value="24">24</option>
                            <option value="25">25</option>
                            <option value="26">26</option>
                            <option value="27">27</option>
                            <option value="28">28</option>
                            <option value="29">29</option>
                            <option value="30">30</option>
                            <option value="31">31</option>
                        </select>--%>
              <%--          <select id="ddlMonth" runat="server">
                            <option value="0">January</option>
                            <option value="1">February</option>
                            <option value="2">March</option>
                            <option value="3">April</option>
                            <option value="4">May</option>
                            <option value="5">June</option>
                            <option value="6">July</option>
                            <option value="7">August</option>
                            <option value="8">September</option>
                            <option value="9">October</option>
                            <option value="10">November</option>
                            <option value="11">December</option>
                        </select>--%>
                   <%--     <select id="ddlYear" runat="server">

                            <option value="2013">2013</option>
                            <option value="2014">2014</option>
                            <option value="2015">2015</option>
                            <option value="2016">2016</option>
                            <option value="2017">2017</option>
                            <option value="2018">2018</option>
                        </select>--%>
                        <asp:HiddenField ClientIDMode="Static"  runat="server" ID="hdnEventStartDate" />
                        <label runat="server"  id="addEventStartDate"></label></td>
                </tr>
                <tr>
                    <td class="alignRight">end:</td>
                    <td class="alignLeft">
                                                <select  id="ddlEndTime" runat="server" >
                                                <option value="00:00:00">12:00 am</option>
                            <option value="00:05:00">12:05 am</option>
                            <option value="00:10:00">12:10 am</option>
                            <option value="00:15:00">12:15 am</option>
                            <option value="00:20:00">12:20 am</option>
                            <option value="00:25:00">12:25 am</option>
                            <option value="00:30:00">12:30 am</option>
                            <option value="00:35:00">12:35 am</option>
                            <option value="00:40:00">12:40 am</option>
                            <option value="00:45:00">12:45 am</option>
                            <option value="00:50:00">12:50 am</option>
                            <option value="00:55:00">12:55 am</option>
                            <option value="01:00:00">01:00 am</option>
                            <option value="01:05:00">01:05 am</option>
                            <option value="01:10:00">01:10 am</option>
                            <option value="01:15:00">01:15 am</option>
                            <option value="01:20:00">01:20 am</option>
                            <option value="01:25:00">01:25 am</option>
                            <option value="01:30:00">01:30 am</option>
                            <option value="01:35:00">01:35 am</option>
                            <option value="01:40:00">01:40 am</option>
                            <option value="01:45:00">01:45 am</option>
                            <option value="01:50:00">01:50 am</option>
                            <option value="01:55:00">01:55 am</option>
                            <option value="02:00:00">02:00 am</option>
                            <option value="02:05:00">02:05 am</option>
                            <option value="02:10:00">02:10 am</option>
                            <option value="02:15:00">02:15 am</option>
                            <option value="02:20:00">02:20 am</option>
                            <option value="02:25:00">02:25 am</option>
                            <option value="02:30:00">02:30 am</option>
                            <option value="02:35:00">02:35 am</option>
                            <option value="02:40:00">02:40 am</option>
                            <option value="02:45:00">02:45 am</option>
                            <option value="02:50:00">02:50 am</option>
                            <option value="02:55:00">02:55 am</option>
                            <option value="03:00:00">03:00 am</option>
                            <option value="03:05:00">03:05 am</option>
                            <option value="03:10:00">03:10 am</option>
                            <option value="03:15:00">03:15 am</option>
                            <option value="03:20:00">03:20 am</option>
                            <option value="03:25:00">03:25 am</option>
                            <option value="03:30:00">03:30 am</option>
                            <option value="03:35:00">03:35 am</option>
                            <option value="03:40:00">03:40 am</option>
                            <option value="03:45:00">03:45 am</option>
                            <option value="03:50:00">03:50 am</option>
                            <option value="03:55:00">03:55 am</option>
                            <option value="04:00:00">04:00 am</option>
                            <option value="04:05:00">04:05 am</option>
                            <option value="04:10:00">04:10 am</option>
                            <option value="04:15:00">04:15 am</option>
                            <option value="04:20:00">04:20 am</option>
                            <option value="04:25:00">04:25 am</option>
                            <option value="04:30:00">04:30 am</option>
                            <option value="04:35:00">04:35 am</option>
                            <option value="04:40:00">04:40 am</option>
                            <option value="04:45:00">04:45 am</option>
                            <option value="04:50:00">04:50 am</option>
                            <option value="04:55:00">04:55 am</option>
                            <option value="05:00:00">05:00 am</option>
                            <option value="05:05:00">05:05 am</option>
                            <option value="05:10:00">05:10 am</option>
                            <option value="05:15:00">05:15 am</option>
                            <option value="05:20:00">05:20 am</option>
                            <option value="05:25:00">05:25 am</option>
                            <option value="05:30:00">05:30 am</option>
                            <option value="05:35:00">05:35 am</option>
                            <option value="05:40:00">05:40 am</option>
                            <option value="05:45:00">05:45 am</option>
                            <option value="05:50:00">05:50 am</option>
                            <option value="05:55:00">05:55 am</option>
                            <option value="06:00:00">06:00 am</option>
                            <option value="06:05:00">06:05 am</option>
                            <option value="06:10:00">06:10 am</option>
                            <option value="06:15:00">06:15 am</option>
                            <option value="06:20:00">06:20 am</option>
                            <option value="06:25:00">06:25 am</option>
                            <option value="06:30:00">06:30 am</option>
                            <option value="06:35:00">06:35 am</option>
                            <option value="06:40:00">06:40 am</option>
                            <option value="06:45:00">06:45 am</option>
                            <option value="06:50:00">06:50 am</option>
                            <option value="06:55:00">06:55 am</option>
                            <option value="07:00:00">07:00 am</option>
                            <option value="07:05:00">07:05 am</option>
                            <option value="07:10:00">07:10 am</option>
                            <option value="07:15:00">07:15 am</option>
                            <option value="07:20:00">07:20 am</option>
                            <option value="07:25:00">07:25 am</option>
                            <option value="07:30:00">07:30 am</option>
                            <option value="07:35:00">07:35 am</option>
                            <option value="07:40:00">07:40 am</option>
                            <option value="07:45:00">07:45 am</option>
                            <option value="07:50:00">07:50 am</option>
                            <option value="07:55:00">07:55 am</option>
                            <option value="08:00:00">08:00 am</option>
                            <option value="08:05:00">08:05 am</option>
                            <option value="08:10:00">08:10 am</option>
                            <option value="08:15:00">08:15 am</option>
                            <option value="08:20:00">08:20 am</option>
                            <option value="08:25:00">08:25 am</option>
                            <option value="08:30:00">08:30 am</option>
                            <option value="08:35:00">08:35 am</option>
                            <option value="08:40:00">08:40 am</option>
                            <option value="08:45:00">08:45 am</option>
                            <option value="08:50:00">08:50 am</option>
                            <option value="08:55:00">08:55 am</option>
                            <option value="09:00:00">09:00 am</option>
                            <option value="09:05:00">09:05 am</option>
                            <option value="09:10:00">09:10 am</option>
                            <option value="09:15:00">09:15 am</option>
                            <option value="09:20:00">09:20 am</option>
                            <option value="09:25:00">09:25 am</option>
                            <option value="09:30:00">09:30 am</option>
                            <option value="09:35:00">09:35 am</option>
                            <option value="09:40:00">09:40 am</option>
                            <option value="09:45:00">09:45 am</option>
                            <option value="09:50:00">09:50 am</option>
                            <option value="09:55:00">09:55 am</option>
                            <option value="10:00:00">10:00 am</option>
                            <option value="10:05:00">10:05 am</option>
                            <option value="10:10:00">10:10 am</option>
                            <option value="10:15:00">10:15 am</option>
                            <option value="10:20:00">10:20 am</option>
                            <option value="10:25:00">10:25 am</option>
                            <option value="10:30:00">10:30 am</option>
                            <option value="10:35:00">10:35 am</option>
                            <option value="10:40:00">10:40 am</option>
                            <option value="10:45:00">10:45 am</option>
                            <option value="10:50:00">10:50 am</option>
                            <option value="10:55:00">10:55 am</option>
                            <option value="11:00:00">11:00 am</option>
                            <option value="11:05:00">11:05 am</option>
                            <option value="11:10:00">11:10 am</option>
                            <option value="11:15:00">11:15 am</option>
                            <option value="11:20:00">11:20 am</option>
                            <option value="11:25:00">11:25 am</option>
                            <option value="11:30:00">11:30 am</option>
                            <option value="11:35:00">11:35 am</option>
                            <option value="11:40:00">11:40 am</option>
                            <option value="11:45:00">11:45 am</option>
                            <option value="11:50:00">11:50 am</option>
                            <option value="11:55:00">11:55 am</option>
                            <option value="12:00:00">12:00 pm</option>
                            <option value="12:05:00">12:05 pm</option>
                            <option value="12:10:00">12:10 pm</option>
                            <option value="12:15:00">12:15 pm</option>
                            <option value="12:20:00">12:20 pm</option>
                            <option value="12:25:00">12:25 pm</option>
                            <option value="12:30:00">12:30 pm</option>
                            <option value="12:35:00">12:35 pm</option>
                            <option value="12:40:00">12:40 pm</option>
                            <option value="12:45:00">12:45 pm</option>
                            <option value="12:50:00">12:50 pm</option>
                            <option value="12:55:00">12:55 pm</option>
                            <option value="13:00:00">01:00 pm</option>
                            <option value="13:05:00">01:05 pm</option>
                            <option value="13:10:00">01:10 pm</option>
                            <option value="13:15:00">01:15 pm</option>
                            <option value="13:20:00">01:20 pm</option>
                            <option value="13:25:00">01:25 pm</option>
                            <option value="13:30:00">01:30 pm</option>
                            <option value="13:35:00">01:35 pm</option>
                            <option value="13:40:00">01:40 pm</option>
                            <option value="13:45:00">01:45 pm</option>
                            <option value="13:50:00">01:50 pm</option>
                            <option value="13:55:00">01:55 pm</option>
                            <option value="14:00:00">02:00 pm</option>
                            <option value="14:05:00">02:05 pm</option>
                            <option value="14:10:00">02:10 pm</option>
                            <option value="14:15:00">02:15 pm</option>
                            <option value="14:20:00">02:20 pm</option>
                            <option value="14:25:00">02:25 pm</option>
                            <option value="14:30:00">02:30 pm</option>
                            <option value="14:35:00">02:35 pm</option>
                            <option value="14:40:00">02:40 pm</option>
                            <option value="14:45:00">02:45 pm</option>
                            <option value="14:50:00">02:50 pm</option>
                            <option value="14:55:00">02:55 pm</option>
                            <option value="15:00:00">03:00 pm</option>
                            <option value="15:05:00">03:05 pm</option>
                            <option value="15:10:00">03:10 pm</option>
                            <option value="15:15:00">03:15 pm</option>
                            <option value="15:20:00">03:20 pm</option>
                            <option value="15:25:00">03:25 pm</option>
                            <option value="15:30:00">03:30 pm</option>
                            <option value="15:35:00">03:35 pm</option>
                            <option value="15:40:00">03:40 pm</option>
                            <option value="15:45:00">03:45 pm</option>
                            <option value="15:50:00">03:50 pm</option>
                            <option value="15:55:00">03:55 pm</option>
                            <option value="16:00:00">04:00 pm</option>
                            <option value="16:05:00">04:05 pm</option>
                            <option value="16:10:00">04:10 pm</option>
                            <option value="16:15:00">04:15 pm</option>
                            <option value="16:20:00">04:20 pm</option>
                            <option value="16:25:00">04:25 pm</option>
                            <option value="16:30:00">04:30 pm</option>
                            <option value="16:35:00">04:35 pm</option>
                            <option value="16:40:00">04:40 pm</option>
                            <option value="16:45:00">04:45 pm</option>
                            <option value="16:50:00">04:50 pm</option>
                            <option value="16:55:00">04:55 pm</option>
                            <option value="17:00:00">05:00 pm</option>
                            <option value="17:05:00">05:05 pm</option>
                            <option value="17:10:00">05:10 pm</option>
                            <option value="17:15:00">05:15 pm</option>
                            <option value="17:20:00">05:20 pm</option>
                            <option value="17:25:00">05:25 pm</option>
                            <option value="17:30:00">05:30 pm</option>
                            <option value="17:35:00">05:35 pm</option>
                            <option value="17:40:00">05:40 pm</option>
                            <option value="17:45:00">05:45 pm</option>
                            <option value="17:50:00">05:50 pm</option>
                            <option value="17:55:00">05:55 pm</option>
                            <option value="18:00:00">06:00 pm</option>
                            <option value="18:05:00">06:05 pm</option>
                            <option value="18:10:00">06:10 pm</option>
                            <option value="18:15:00">06:15 pm</option>
                            <option value="18:20:00">06:20 pm</option>
                            <option value="18:25:00">06:25 pm</option>
                            <option value="18:30:00">06:30 pm</option>
                            <option value="18:35:00">06:35 pm</option>
                            <option value="18:40:00">06:40 pm</option>
                            <option value="18:45:00">06:45 pm</option>
                            <option value="18:50:00">06:50 pm</option>
                            <option value="18:55:00">06:55 pm</option>
                            <option value="19:00:00">07:00 pm</option>
                            <option value="19:05:00">07:05 pm</option>
                            <option value="19:10:00">07:10 pm</option>
                            <option value="19:15:00">07:15 pm</option>
                            <option value="19:20:00">07:20 pm</option>
                            <option value="19:25:00">07:25 pm</option>
                            <option value="19:30:00">07:30 pm</option>
                            <option value="19:35:00">07:35 pm</option>
                            <option value="19:40:00">07:40 pm</option>
                            <option value="19:45:00">07:45 pm</option>
                            <option value="19:50:00">07:50 pm</option>
                            <option value="19:55:00">07:55 pm</option>
                            <option value="20:00:00">08:00 pm</option>
                            <option value="20:05:00">08:05 pm</option>
                            <option value="20:10:00">08:10 pm</option>
                            <option value="20:15:00">08:15 pm</option>
                            <option value="20:20:00">08:20 pm</option>
                            <option value="20:25:00">08:25 pm</option>
                            <option value="20:30:00">08:30 pm</option>
                            <option value="20:35:00">08:35 pm</option>
                            <option value="20:40:00">08:40 pm</option>
                            <option value="20:45:00">08:45 pm</option>
                            <option value="20:50:00">08:50 pm</option>
                            <option value="20:55:00">08:55 pm</option>
                            <option value="21:00:00">09:00 pm</option>
                            <option value="21:05:00">09:05 pm</option>
                            <option value="21:10:00">09:10 pm</option>
                            <option value="21:15:00">09:15 pm</option>
                            <option value="21:20:00">09:20 pm</option>
                            <option value="21:25:00">09:25 pm</option>
                            <option value="21:30:00">09:30 pm</option>
                            <option value="21:35:00">09:35 pm</option>
                            <option value="21:40:00">09:40 pm</option>
                            <option value="21:45:00">09:45 pm</option>
                            <option value="21:50:00">09:50 pm</option>
                            <option value="21:55:00">09:55 pm</option>
                            <option value="22:00:00">10:00 pm</option>
                            <option value="22:05:00">10:05 pm</option>
                            <option value="22:10:00">10:10 pm</option>
                            <option value="22:15:00">10:15 pm</option>
                            <option value="22:20:00">10:20 pm</option>
                            <option value="22:25:00">10:25 pm</option>
                            <option value="22:30:00">10:30 pm</option>
                            <option value="22:35:00">10:35 pm</option>
                            <option value="22:40:00">10:40 pm</option>
                            <option value="22:45:00">10:45 pm</option>
                            <option value="22:50:00">10:50 pm</option>
                            <option value="22:55:00">10:55 pm</option>
                            <option value="23:00:00">11:00 pm</option>
                            <option value="23:05:00">11:05 pm</option>
                            <option value="23:10:00">11:10 pm</option>
                            <option value="23:15:00">11:15 pm</option>
                            <option value="23:20:00">11:20 pm</option>
                            <option value="23:25:00">11:25 pm</option>
                            <option value="23:30:00">11:30 pm</option>
                            <option value="23:35:00">11:35 pm</option>
                            <option value="23:40:00">11:40 pm</option>
                            <option value="23:45:00">11:45 pm</option>
                            <option value="23:50:00">11:50 pm</option>
                            <option value="23:55:00">11:55 pm</option>
                        </select>
          <%--              <select id="ddlEndTime" runat="server">
                            <option value="0">00:00</option>
                            <option value="5">00:05</option>
                            <option value="10">00:10</option>
                            <option value="15">00:15</option>
                            <option value="20">00:20</option>
                            <option value="25">00:25</option>
                            <option value="30">00:30</option>
                            <option value="35">00:35</option>
                            <option value="40">00:40</option>
                            <option value="45">00:45</option>
                            <option value="50">00:50</option>
                            <option value="55">00:55</option>
                            <option value="60">01:00</option>
                            <option value="65">01:05</option>
                            <option value="70">01:10</option>
                            <option value="75">01:15</option>
                            <option value="80">01:20</option>
                            <option value="85">01:25</option>
                            <option value="90">01:30</option>
                            <option value="95">01:35</option>
                            <option value="100">01:40</option>
                            <option value="105">01:45</option>
                            <option value="110">01:50</option>
                            <option value="115">01:55</option>
                            <option value="120">02:00</option>
                            <option value="125">02:05</option>
                            <option value="130">02:10</option>
                            <option value="135">02:15</option>
                            <option value="140">02:20</option>
                            <option value="145">02:25</option>
                            <option value="150">02:30</option>
                            <option value="155">02:35</option>
                            <option value="160">02:40</option>
                            <option value="165">02:45</option>
                            <option value="170">02:50</option>
                            <option value="175">02:55</option>
                            <option value="180">03:00</option>
                            <option value="185">03:05</option>
                            <option value="190">03:10</option>
                            <option value="195">03:15</option>
                            <option value="200">03:20</option>
                            <option value="205">03:25</option>
                            <option value="210">03:30</option>
                            <option value="215">03:35</option>
                            <option value="220">03:40</option>
                            <option value="225">03:45</option>
                            <option value="230">03:50</option>
                            <option value="235">03:55</option>
                            <option value="240">04:00</option>
                            <option value="245">04:05</option>
                            <option value="250">04:10</option>
                            <option value="255">04:15</option>
                            <option value="260">04:20</option>
                            <option value="265">04:25</option>
                            <option value="270">04:30</option>
                            <option value="275">04:35</option>
                            <option value="280">04:40</option>
                            <option value="285">04:45</option>
                            <option value="290">04:50</option>
                            <option value="295">04:55</option>
                            <option value="300">05:00</option>
                            <option value="305">05:05</option>
                            <option value="310">05:10</option>
                            <option value="315">05:15</option>
                            <option value="320">05:20</option>
                            <option value="325">05:25</option>
                            <option value="330">05:30</option>
                            <option value="335">05:35</option>
                            <option value="340">05:40</option>
                            <option value="345">05:45</option>
                            <option value="350">05:50</option>
                            <option value="355">05:55</option>
                            <option value="360">06:00</option>
                            <option value="365">06:05</option>
                            <option value="370">06:10</option>
                            <option value="375">06:15</option>
                            <option value="380">06:20</option>
                            <option value="385">06:25</option>
                            <option value="390">06:30</option>
                            <option value="395">06:35</option>
                            <option value="400">06:40</option>
                            <option value="405">06:45</option>
                            <option value="410">06:50</option>
                            <option value="415">06:55</option>
                            <option value="420">07:00</option>
                            <option value="425">07:05</option>
                            <option value="430">07:10</option>
                            <option value="435">07:15</option>
                            <option value="440">07:20</option>
                            <option value="445">07:25</option>
                            <option value="450">07:30</option>
                            <option value="455">07:35</option>
                            <option value="460">07:40</option>
                            <option value="465">07:45</option>
                            <option value="470">07:50</option>
                            <option value="475">07:55</option>
                            <option value="480">08:00</option>
                            <option value="485">08:05</option>
                            <option value="490">08:10</option>
                            <option value="495">08:15</option>
                            <option value="500">08:20</option>
                            <option value="505">08:25</option>
                            <option value="510">08:30</option>
                            <option value="515">08:35</option>
                            <option value="520">08:40</option>
                            <option value="525">08:45</option>
                            <option value="530">08:50</option>
                            <option value="535">08:55</option>
                            <option value="540">09:00</option>
                            <option value="545">09:05</option>
                            <option value="550">09:10</option>
                            <option value="555">09:15</option>
                            <option value="560">09:20</option>
                            <option value="565">09:25</option>
                            <option value="570">09:30</option>
                            <option value="575">09:35</option>
                            <option value="580">09:40</option>
                            <option value="585">09:45</option>
                            <option value="590">09:50</option>
                            <option value="595">09:55</option>
                            <option value="600">10:00</option>
                            <option value="605">10:05</option>
                            <option value="610">10:10</option>
                            <option value="615">10:15</option>
                            <option value="620">10:20</option>
                            <option value="625">10:25</option>
                            <option value="630">10:30</option>
                            <option value="635">10:35</option>
                            <option value="640">10:40</option>
                            <option value="645">10:45</option>
                            <option value="650">10:50</option>
                            <option value="655">10:55</option>
                            <option value="660">11:00</option>
                            <option value="665">11:05</option>
                            <option value="670">11:10</option>
                            <option value="675">11:15</option>
                            <option value="680">11:20</option>
                            <option value="685">11:25</option>
                            <option value="690">11:30</option>
                            <option value="695">11:35</option>
                            <option value="700">11:40</option>
                            <option value="705">11:45</option>
                            <option value="710">11:50</option>
                            <option value="715">11:55</option>
                            <option value="720">12:00</option>
                            <option value="725">12:05</option>
                            <option value="730">12:10</option>
                            <option value="735">12:15</option>
                            <option value="740">12:20</option>
                            <option value="745">12:25</option>
                            <option value="750">12:30</option>
                            <option value="755">12:35</option>
                            <option value="760">12:40</option>
                            <option value="765">12:45</option>
                            <option value="770">12:50</option>
                            <option value="775">12:55</option>
                            <option value="780">13:00</option>
                            <option value="785">13:05</option>
                            <option value="790">13:10</option>
                            <option value="795">13:15</option>
                            <option value="800">13:20</option>
                            <option value="805">13:25</option>
                            <option value="810">13:30</option>
                            <option value="815">13:35</option>
                            <option value="820">13:40</option>
                            <option value="825">13:45</option>
                            <option value="830">13:50</option>
                            <option value="835">13:55</option>
                            <option value="840">14:00</option>
                            <option value="845">14:05</option>
                            <option value="850">14:10</option>
                            <option value="855">14:15</option>
                            <option value="860">14:20</option>
                            <option value="865">14:25</option>
                            <option value="870">14:30</option>
                            <option value="875">14:35</option>
                            <option value="880">14:40</option>
                            <option value="885">14:45</option>
                            <option value="890">14:50</option>
                            <option value="895">14:55</option>
                            <option value="900">15:00</option>
                            <option value="905">15:05</option>
                            <option value="910">15:10</option>
                            <option value="915">15:15</option>
                            <option value="920">15:20</option>
                            <option value="925">15:25</option>
                            <option value="930">15:30</option>
                            <option value="935">15:35</option>
                            <option value="940">15:40</option>
                            <option value="945">15:45</option>
                            <option value="950">15:50</option>
                            <option value="955">15:55</option>
                            <option value="960">16:00</option>
                            <option value="965">16:05</option>
                            <option value="970">16:10</option>
                            <option value="975">16:15</option>
                            <option value="980">16:20</option>
                            <option value="985">16:25</option>
                            <option value="990">16:30</option>
                            <option value="995">16:35</option>
                            <option value="1000">16:40</option>
                            <option value="1005">16:45</option>
                            <option value="1010">16:50</option>
                            <option value="1015">16:55</option>
                            <option value="1020">17:00</option>
                            <option value="1025">17:05</option>
                            <option value="1030">17:10</option>
                            <option value="1035">17:15</option>
                            <option value="1040">17:20</option>
                            <option value="1045">17:25</option>
                            <option value="1050">17:30</option>
                            <option value="1055">17:35</option>
                            <option value="1060">17:40</option>
                            <option value="1065">17:45</option>
                            <option value="1070">17:50</option>
                            <option value="1075">17:55</option>
                            <option value="1080">18:00</option>
                            <option value="1085">18:05</option>
                            <option value="1090">18:10</option>
                            <option value="1095">18:15</option>
                            <option value="1100">18:20</option>
                            <option value="1105">18:25</option>
                            <option value="1110">18:30</option>
                            <option value="1115">18:35</option>
                            <option value="1120">18:40</option>
                            <option value="1125">18:45</option>
                            <option value="1130">18:50</option>
                            <option value="1135">18:55</option>
                            <option value="1140">19:00</option>
                            <option value="1145">19:05</option>
                            <option value="1150">19:10</option>
                            <option value="1155">19:15</option>
                            <option value="1160">19:20</option>
                            <option value="1165">19:25</option>
                            <option value="1170">19:30</option>
                            <option value="1175">19:35</option>
                            <option value="1180">19:40</option>
                            <option value="1185">19:45</option>
                            <option value="1190">19:50</option>
                            <option value="1195">19:55</option>
                            <option value="1200">20:00</option>
                            <option value="1205">20:05</option>
                            <option value="1210">20:10</option>
                            <option value="1215">20:15</option>
                            <option value="1220">20:20</option>
                            <option value="1225">20:25</option>
                            <option value="1230">20:30</option>
                            <option value="1235">20:35</option>
                            <option value="1240">20:40</option>
                            <option value="1245">20:45</option>
                            <option value="1250">20:50</option>
                            <option value="1255">20:55</option>
                            <option value="1260">21:00</option>
                            <option value="1265">21:05</option>
                            <option value="1270">21:10</option>
                            <option value="1275">21:15</option>
                            <option value="1280">21:20</option>
                            <option value="1285">21:25</option>
                            <option value="1290">21:30</option>
                            <option value="1295">21:35</option>
                            <option value="1300">21:40</option>
                            <option value="1305">21:45</option>
                            <option value="1310">21:50</option>
                            <option value="1315">21:55</option>
                            <option value="1320">22:00</option>
                            <option value="1325">22:05</option>
                            <option value="1330">22:10</option>
                            <option value="1335">22:15</option>
                            <option value="1340">22:20</option>
                            <option value="1345">22:25</option>
                            <option value="1350">22:30</option>
                            <option value="1355">22:35</option>
                            <option value="1360">22:40</option>
                            <option value="1365">22:45</option>
                            <option value="1370">22:50</option>
                            <option value="1375">22:55</option>
                            <option value="1380">23:00</option>
                            <option value="1385">23:05</option>
                            <option value="1390">23:10</option>
                            <option value="1395">23:15</option>
                            <option value="1400">23:20</option>
                            <option value="1405">23:25</option>
                            <option value="1410">23:30</option>
                            <option value="1415">23:35</option>
                            <option value="1420">23:40</option>
                            <option value="1425">23:45</option>
                            <option value="1430">23:50</option>
                            <option value="1435">23:55</option>
                        </select>--%>
                        <asp:CompareValidator ID="cmpTimeAdd" ControlToCompare="ddlStartTime" ControlToValidate="ddlEndTime" 
                                                    runat="server" ErrorMessage="Start time must be greater than end time" Display="Dynamic" 
                                                    Operator="GreaterThan" ForeColor="Red" Type="String" ValidationGroup="add"></asp:CompareValidator>
                        <%--<select id="ddlEndDay" runat="server">
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                            <option value="7">7</option>
                            <option value="8">8</option>
                            <option value="9">9</option>
                            <option value="10">10</option>
                            <option value="11">11</option>
                            <option value="12">12</option>
                            <option value="13">13</option>
                            <option value="14">14</option>
                            <option value="15">15</option>
                            <option value="16">16</option>
                            <option value="17">17</option>
                            <option value="18">18</option>
                            <option value="19">19</option>
                            <option value="20">20</option>
                            <option value="21">21</option>
                            <option value="22">22</option>
                            <option value="23">23</option>
                            <option value="24">24</option>
                            <option value="25">25</option>
                            <option value="26">26</option>
                            <option value="27">27</option>
                            <option value="28">28</option>
                            <option value="29">29</option>
                            <option value="30">30</option>
                            <option value="31">31</option>
                        </select>--%>
                      <%--  <select id="ddlEndMonth" runat="server">
                            <option value="0">January</option>
                            <option value="1">February</option>
                            <option value="2">March</option>
                            <option value="3">April</option>
                            <option value="4">May</option>
                            <option value="5">June</option>
                            <option value="6">July</option>
                            <option value="7">August</option>
                            <option value="8">September</option>
                            <option value="9">October</option>
                            <option value="10">November</option>
                            <option value="11">December</option>
                        </select>--%>
                   <%--     <select id="ddlEndYear" runat="server">
                            <option value="2009">2009</option>
                            <option value="2010">2010</option>
                            <option value="2011">2011</option>
                            <option value="2012">2012</option>
                            <option value="2013">2013</option>
                            <option value="2014">2014</option>
                            <option value="2015">2015</option>
                            <option value="2016">2016</option>
                            <option value="2017">2017</option>
                            <option value="2018">2018</option>
                        </select>--%>

                        <asp:HiddenField ClientIDMode="Static"  runat="server" ID="hdnEventEndDate"/>
                        <label  id="addEventEndDate"></label></td>
                </tr>

                   <tr>
                    <td class="alignRight">Attendees:</td>
                    <td class="alignLeft">
                 
                    <div style="height:150px;overflow-y:scroll;;">
                    <asp:CheckBoxList  ID="ddlAttendees"  runat="server" ClientIDMode="Static" ></asp:CheckBoxList>
                    </div><a href="../Attendees.aspx">Add More Attendees</a>
                   
                        <asp:TextBox  style="display:none"  ID="txtAddNameDesignation" ClientIDMode="Static" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                    </td>
                </tr>
                <tr style="display:none;">
                  <td class="alignRight">Minutes:</td>
                    <td class="alignLeft">
                         <asp:TextBox  ID="txtMOM"   ClientIDMode="Static"  style="width: 265px; height: 119px;" runat="server" TextMode="MultiLine"></asp:TextBox><br />
                    </td>
                </tr>
            </table>
          

            <table id="tblUpdate" style="display: none;" cellpadding="0" class="style1">
                <tr style="display:none">
                    <td class="alignRight">Name of Visitor:</td>
                    <td class="alignLeft">
                        <asp:TextBox  ID="txtNameOfVisitorUpdate" ClientIDMode="Static" runat="server"></asp:TextBox><br />
                    </td>
                </tr>
                <tr style="display:none">
                    <td class="alignRight">Designation of Visitor:</td>
                    <td class="alignLeft">
                        <asp:TextBox   ID="txtDesignOfVisitorUpdate" ClientIDMode="Static" runat="server"></asp:TextBox><br />
                    </td>
                </tr>
                <tr style="display:none">
                    <td class="alignRight">Name of Visitor Organisation:</td>
                    <td class="alignLeft">

                        <asp:TextBox  ID="txtNameOfVisitorOrgUpdate" ClientIDMode="Static" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td class="alignRight">Meeting Title * :</td>
                    <td class="alignLeft">
                        <asp:TextBox ClientIDMode="Static" ID="txtAppointmentUpdate" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                        <asp:RequiredFieldValidator runat="server" ID="rfvAppointmentUpdate" ControlToValidate="txtAppointmentUpdate"  Display="Dynamic" ValidationGroup="update" ErrorMessage="Appointment" Text="Inavalid" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="display:none">
                    <td class="alignRight">Name/Designation:</td>
                    <td class="alignLeft">
                        <asp:TextBox  ID="txtNameDesignationUpdate" ClientIDMode="Static" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                    </td>
                </tr>
                <tr style="display:none">
                    <td class="alignRight">Upload Photo:</td>
                    <td class="alignLeft">
                        <asp:FileUpload ID="fuPhotoUp" runat="server" />
                        <%--<asp:AsyncFileUpload ID="fileUploadPhotoUpdate" OnClientUploadComplete="uploadComplete" OnClientUploadError="uploadError" CompleteBackColor="White" 
                runat="server" UploaderStyle="Modern" UploadingBackColor="#CCFFFF" ThrobberID="imgLoad"/>--%>
                        <br />
                        <asp:Label ID="lblMsg" runat="server" ClientIDMode="Static"></asp:Label><br />
                    </td>

                </tr>
                  <tr>
                    <td class="alignRight">Meeting Description:</td>
                    <td class="alignLeft">
                        <asp:TextBox ID="txtInformationUpdate" ClientIDMode="Static" runat="server" TextMode="MultiLine"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td class="alignRight">Attachments:</td>
                    <td class="alignLeft">
                        <asp:FileUpload ID="fuAttUp" runat="server" />
                        
                        <%--<asp:AsyncFileUpload ID="fileUploadAttachmentsUpdate" OnClientUploadComplete="uploadCompleteAttach" OnClientUploadError="uploadErrorAttach" CompleteBackColor="White" 
                runat="server" UploaderStyle="Modern" UploadingBackColor="#CCFFFF" ThrobberID="imgLoad"/>--%>
                        <br />
                        <asp:Label ID="lblMsgFile" runat="server" Text=""></asp:Label><br />
                        <asp:HiddenField ID="hdnFileName" runat="server" ClientIDMode="Static" />
                    </td>

                </tr>
              
                <tr>
                    <td class="alignRight">start:</td>
                    <td class="alignLeft">
                        <%--<select  id="ddlStartTimeUp" runat="server">
            <option value="0:0">00:00</option>
            <option value="0:5">00:05</option>
            <option value="0:10">00:10</option>
            <option value="0:15">00:15</option>
            <option value="0:20">00:20</option>
            <option value="0:25">00:25</option>
            <option value="30">00:30</option>
            <option value="35">00:35</option>
            <option value="40">00:40</option>
            <option value="45">00:45</option>
            <option value="50">00:50</option>
            <option value="55">00:55</option>
            <option value="60">01:00</option>
            <option value="65">01:05</option>
            <option value="70">01:10</option>
            <option value="75">01:15</option>
            <option value="80">01:20</option>
            <option value="85">01:25</option>
            <option value="90">01:30</option>
            <option value="95">01:35</option>
            <option value="100">01:40</option>
            <option value="105">01:45</option>
            <option value="110">01:50</option>
            <option value="115">01:55</option>
            <option value="120">02:00</option>
            <option value="125">02:05</option>
            <option value="130">02:10</option>
            <option value="135">02:15</option>
            <option value="140">02:20</option>
            <option value="145">02:25</option>
            <option value="150">02:30</option>
            <option value="155">02:35</option>
            <option value="160">02:40</option>
            <option value="165">02:45</option>
            <option value="170">02:50</option>
            <option value="175">02:55</option>
            <option value="180">03:00</option>
            <option value="185">03:05</option>
            <option value="190">03:10</option>
            <option value="195">03:15</option>
            <option value="200">03:20</option>
            <option value="205">03:25</option>
            <option value="210">03:30</option>
            <option value="215">03:35</option>
            <option value="220">03:40</option>
            <option value="225">03:45</option>
            <option value="230">03:50</option>
            <option value="235">03:55</option>
            <option value="240">04:00</option>
            <option value="245">04:05</option>
            <option value="250">04:10</option>
            <option value="255">04:15</option>
            <option value="260">04:20</option>
            <option value="265">04:25</option>
            <option value="270">04:30</option>
            <option value="275">04:35</option>
            <option value="280">04:40</option>
            <option value="285">04:45</option>
            <option value="290">04:50</option>
            <option value="295">04:55</option>
            <option value="300">05:00</option>
            <option value="305">05:05</option>
            <option value="310">05:10</option>
            <option value="315">05:15</option>
            <option value="320">05:20</option>
            <option value="325">05:25</option>
            <option value="330">05:30</option>
            <option value="335">05:35</option>
            <option value="340">05:40</option>
            <option value="345">05:45</option>
            <option value="350">05:50</option>
            <option value="355">05:55</option>
            <option value="360">06:00</option>
            <option value="365">06:05</option>
            <option value="370">06:10</option>
            <option value="375">06:15</option>
            <option value="380">06:20</option>
            <option value="385">06:25</option>
            <option value="390">06:30</option>
            <option value="395">06:35</option>
            <option value="400">06:40</option>
            <option value="405">06:45</option>
            <option value="410">06:50</option>
            <option value="415">06:55</option>
            <option value="420">07:00</option>
            <option value="425">07:05</option>
            <option value="430">07:10</option>
            <option value="435">07:15</option>
            <option value="440">07:20</option>
            <option value="445">07:25</option>
            <option value="450">07:30</option>
            <option value="455">07:35</option>
            <option value="460">07:40</option>
            <option value="465">07:45</option>
            <option value="470">07:50</option>
            <option value="475">07:55</option>
            <option value="480">08:00</option>
            <option value="485">08:05</option>
            <option value="490">08:10</option>
            <option value="495">08:15</option>
            <option value="500">08:20</option>
            <option value="505">08:25</option>
            <option value="510">08:30</option>
            <option value="515">08:35</option>
            <option value="520">08:40</option>
            <option value="525">08:45</option>
            <option value="530">08:50</option>
            <option value="535">08:55</option>
            <option value="540">09:00</option>
            <option value="545">09:05</option>
            <option value="550">09:10</option>
            <option value="555">09:15</option>
            <option value="560">09:20</option>
            <option value="565">09:25</option>
            <option value="570">09:30</option>
            <option value="575">09:35</option>
            <option value="580">09:40</option>
            <option value="585">09:45</option>
            <option value="590">09:50</option>
            <option value="595">09:55</option>
            <option value="600">10:00</option>
            <option value="605">10:05</option>
            <option value="610">10:10</option>
            <option value="615">10:15</option>
            <option value="620">10:20</option>
            <option value="625">10:25</option>
            <option value="630">10:30</option>
            <option value="635">10:35</option>
            <option value="640">10:40</option>
            <option value="645">10:45</option>
            <option value="650">10:50</option>
            <option value="655">10:55</option>
            <option value="660">11:00</option>
            <option value="665">11:05</option>
            <option value="670">11:10</option>
            <option value="675">11:15</option>
            <option value="680">11:20</option>
            <option value="685">11:25</option>
            <option value="690">11:30</option>
            <option value="695">11:35</option>
            <option value="700">11:40</option>
            <option value="705">11:45</option>
            <option value="710">11:50</option>
            <option value="715">11:55</option>
            <option value="720">12:00</option>
            <option value="725">12:05</option>
            <option value="730">12:10</option>
            <option value="735">12:15</option>
            <option value="740">12:20</option>
            <option value="745">12:25</option>
            <option value="750">12:30</option>
            <option value="755">12:35</option>
            <option value="760">12:40</option>
            <option value="765">12:45</option>
            <option value="770">12:50</option>
            <option value="775">12:55</option>
            <option value="780">13:00</option>
            <option value="785">13:05</option>
            <option value="790">13:10</option>
            <option value="795">13:15</option>
            <option value="800">13:20</option>
            <option value="805">13:25</option>
            <option value="810">13:30</option>
            <option value="815">13:35</option>
            <option value="820">13:40</option>
            <option value="825">13:45</option>
            <option value="830">13:50</option>
            <option value="835">13:55</option>
            <option value="840">14:00</option>
            <option value="845">14:05</option>
            <option value="850">14:10</option>
            <option value="855">14:15</option>
            <option value="860">14:20</option>
            <option value="865">14:25</option>
            <option value="870">14:30</option>
            <option value="875">14:35</option>
            <option value="880">14:40</option>
            <option value="885">14:45</option>
            <option value="890">14:50</option>
            <option value="895">14:55</option>
            <option value="900">15:00</option>
            <option value="905">15:05</option>
            <option value="910">15:10</option>
            <option value="915">15:15</option>
            <option value="920">15:20</option>
            <option value="925">15:25</option>
            <option value="930">15:30</option>
            <option value="935">15:35</option>
            <option value="940">15:40</option>
            <option value="945">15:45</option>
            <option value="950">15:50</option>
            <option value="955">15:55</option>
            <option value="960">16:00</option>
            <option value="965">16:05</option>
            <option value="970">16:10</option>
            <option value="975">16:15</option>
            <option value="980">16:20</option>
            <option value="985">16:25</option>
            <option value="990">16:30</option>
            <option value="995">16:35</option>
            <option value="1000">16:40</option>
            <option value="1005">16:45</option>
            <option value="1010">16:50</option>
            <option value="1015">16:55</option>
            <option value="1020">17:00</option>
            <option value="1025">17:05</option>
            <option value="1030">17:10</option>
            <option value="1035">17:15</option>
            <option value="1040">17:20</option>
            <option value="1045">17:25</option>
            <option value="1050">17:30</option>
            <option value="1055">17:35</option>
            <option value="1060">17:40</option>
            <option value="1065">17:45</option>
            <option value="1070">17:50</option>
            <option value="1075">17:55</option>
            <option value="1080">18:00</option>
            <option value="1085">18:05</option>
            <option value="1090">18:10</option>
            <option value="1095">18:15</option>
            <option value="1100">18:20</option>
            <option value="1105">18:25</option>
            <option value="1110">18:30</option>
            <option value="1115">18:35</option>
            <option value="1120">18:40</option>
            <option value="1125">18:45</option>
            <option value="1130">18:50</option>
            <option value="1135">18:55</option>
            <option value="1140">19:00</option>
            <option value="1145">19:05</option>
            <option value="1150">19:10</option>
            <option value="1155">19:15</option>
            <option value="1160">19:20</option>
            <option value="1165">19:25</option>
            <option value="1170">19:30</option>
            <option value="1175">19:35</option>
            <option value="1180">19:40</option>
            <option value="1185">19:45</option>
            <option value="1190">19:50</option>
            <option value="1195">19:55</option>
            <option value="1200">20:00</option>
            <option value="1205">20:05</option>
            <option value="1210">20:10</option>
            <option value="1215">20:15</option>
            <option value="1220">20:20</option>
            <option value="1225">20:25</option>
            <option value="1230">20:30</option>
            <option value="1235">20:35</option>
            <option value="1240">20:40</option>
            <option value="1245">20:45</option>
            <option value="1250">20:50</option>
            <option value="1255">20:55</option>
            <option value="1260">21:00</option>
            <option value="1265">21:05</option>
            <option value="1270">21:10</option>
            <option value="1275">21:15</option>
            <option value="1280">21:20</option>
            <option value="1285">21:25</option>
            <option value="1290">21:30</option>
            <option value="1295">21:35</option>
            <option value="1300">21:40</option>
            <option value="1305">21:45</option>
            <option value="1310">21:50</option>
            <option value="1315">21:55</option>
            <option value="1320">22:00</option>
            <option value="1325">22:05</option>
            <option value="1330">22:10</option>
            <option value="1335">22:15</option>
            <option value="1340">22:20</option>
            <option value="1345">22:25</option>
            <option value="1350">22:30</option>
            <option value="1355">22:35</option>
            <option value="1360">22:40</option>
            <option value="1365">22:45</option>
            <option value="1370">22:50</option>
            <option value="1375">22:55</option>
            <option value="1380">23:00</option>
            <option value="1385">23:05</option>
            <option value="1390">23:10</option>
            <option value="1395">23:15</option>
            <option value="1400">23:20</option>
            <option value="1405">23:25</option>
            <option value="1410">23:30</option>
            <option value="1415">23:35</option>
            <option value="1420">23:40</option>
            <option value="1425">23:45</option>
            <option value="1430">23:50</option>
            <option value="1435">23:55</option>
        </select>--%>

                        <select id="ddlStartTimeUp" runat="server" >
                        <option value="00:00:00">12:00 am</option>
                            <option value="00:05:00">12:05 am</option>
                            <option value="00:10:00">12:10 am</option>
                            <option value="00:15:00">12:15 am</option>
                            <option value="00:20:00">12:20 am</option>
                            <option value="00:25:00">12:25 am</option>
                            <option value="00:30:00">12:30 am</option>
                            <option value="00:35:00">12:35 am</option>
                            <option value="00:40:00">12:40 am</option>
                            <option value="00:45:00">12:45 am</option>
                            <option value="00:50:00">12:50 am</option>
                            <option value="00:55:00">12:55 am</option>
                            <option value="01:00:00">01:00 am</option>
                            <option value="01:05:00">01:05 am</option>
                            <option value="01:10:00">01:10 am</option>
                            <option value="01:15:00">01:15 am</option>
                            <option value="01:20:00">01:20 am</option>
                            <option value="01:25:00">01:25 am</option>
                            <option value="01:30:00">01:30 am</option>
                            <option value="01:35:00">01:35 am</option>
                            <option value="01:40:00">01:40 am</option>
                            <option value="01:45:00">01:45 am</option>
                            <option value="01:50:00">01:50 am</option>
                            <option value="01:55:00">01:55 am</option>
                            <option value="02:00:00">02:00 am</option>
                            <option value="02:05:00">02:05 am</option>
                            <option value="02:10:00">02:10 am</option>
                            <option value="02:15:00">02:15 am</option>
                            <option value="02:20:00">02:20 am</option>
                            <option value="02:25:00">02:25 am</option>
                            <option value="02:30:00">02:30 am</option>
                            <option value="02:35:00">02:35 am</option>
                            <option value="02:40:00">02:40 am</option>
                            <option value="02:45:00">02:45 am</option>
                            <option value="02:50:00">02:50 am</option>
                            <option value="02:55:00">02:55 am</option>
                            <option value="03:00:00">03:00 am</option>
                            <option value="03:05:00">03:05 am</option>
                            <option value="03:10:00">03:10 am</option>
                            <option value="03:15:00">03:15 am</option>
                            <option value="03:20:00">03:20 am</option>
                            <option value="03:25:00">03:25 am</option>
                            <option value="03:30:00">03:30 am</option>
                            <option value="03:35:00">03:35 am</option>
                            <option value="03:40:00">03:40 am</option>
                            <option value="03:45:00">03:45 am</option>
                            <option value="03:50:00">03:50 am</option>
                            <option value="03:55:00">03:55 am</option>
                            <option value="04:00:00">04:00 am</option>
                            <option value="04:05:00">04:05 am</option>
                            <option value="04:10:00">04:10 am</option>
                            <option value="04:15:00">04:15 am</option>
                            <option value="04:20:00">04:20 am</option>
                            <option value="04:25:00">04:25 am</option>
                            <option value="04:30:00">04:30 am</option>
                            <option value="04:35:00">04:35 am</option>
                            <option value="04:40:00">04:40 am</option>
                            <option value="04:45:00">04:45 am</option>
                            <option value="04:50:00">04:50 am</option>
                            <option value="04:55:00">04:55 am</option>
                            <option value="05:00:00">05:00 am</option>
                            <option value="05:05:00">05:05 am</option>
                            <option value="05:10:00">05:10 am</option>
                            <option value="05:15:00">05:15 am</option>
                            <option value="05:20:00">05:20 am</option>
                            <option value="05:25:00">05:25 am</option>
                            <option value="05:30:00">05:30 am</option>
                            <option value="05:35:00">05:35 am</option>
                            <option value="05:40:00">05:40 am</option>
                            <option value="05:45:00">05:45 am</option>
                            <option value="05:50:00">05:50 am</option>
                            <option value="05:55:00">05:55 am</option>
                            <option value="06:00:00">06:00 am</option>
                            <option value="06:05:00">06:05 am</option>
                            <option value="06:10:00">06:10 am</option>
                            <option value="06:15:00">06:15 am</option>
                            <option value="06:20:00">06:20 am</option>
                            <option value="06:25:00">06:25 am</option>
                            <option value="06:30:00">06:30 am</option>
                            <option value="06:35:00">06:35 am</option>
                            <option value="06:40:00">06:40 am</option>
                            <option value="06:45:00">06:45 am</option>
                            <option value="06:50:00">06:50 am</option>
                            <option value="06:55:00">06:55 am</option>
                            <option value="07:00:00">07:00 am</option>
                            <option value="07:05:00">07:05 am</option>
                            <option value="07:10:00">07:10 am</option>
                            <option value="07:15:00">07:15 am</option>
                            <option value="07:20:00">07:20 am</option>
                            <option value="07:25:00">07:25 am</option>
                            <option value="07:30:00">07:30 am</option>
                            <option value="07:35:00">07:35 am</option>
                            <option value="07:40:00">07:40 am</option>
                            <option value="07:45:00">07:45 am</option>
                            <option value="07:50:00">07:50 am</option>
                            <option value="07:55:00">07:55 am</option>
                            <option value="08:00:00">08:00 am</option>
                            <option value="08:05:00">08:05 am</option>
                            <option value="08:10:00">08:10 am</option>
                            <option value="08:15:00">08:15 am</option>
                            <option value="08:20:00">08:20 am</option>
                            <option value="08:25:00">08:25 am</option>
                            <option value="08:30:00">08:30 am</option>
                            <option value="08:35:00">08:35 am</option>
                            <option value="08:40:00">08:40 am</option>
                            <option value="08:45:00">08:45 am</option>
                            <option value="08:50:00">08:50 am</option>
                            <option value="08:55:00">08:55 am</option>
                            <option value="09:00:00">09:00 am</option>
                            <option value="09:05:00">09:05 am</option>
                            <option value="09:10:00">09:10 am</option>
                            <option value="09:15:00">09:15 am</option>
                            <option value="09:20:00">09:20 am</option>
                            <option value="09:25:00">09:25 am</option>
                            <option value="09:30:00">09:30 am</option>
                            <option value="09:35:00">09:35 am</option>
                            <option value="09:40:00">09:40 am</option>
                            <option value="09:45:00">09:45 am</option>
                            <option value="09:50:00">09:50 am</option>
                            <option value="09:55:00">09:55 am</option>
                            <option value="10:00:00">10:00 am</option>
                            <option value="10:05:00">10:05 am</option>
                            <option value="10:10:00">10:10 am</option>
                            <option value="10:15:00">10:15 am</option>
                            <option value="10:20:00">10:20 am</option>
                            <option value="10:25:00">10:25 am</option>
                            <option value="10:30:00">10:30 am</option>
                            <option value="10:35:00">10:35 am</option>
                            <option value="10:40:00">10:40 am</option>
                            <option value="10:45:00">10:45 am</option>
                            <option value="10:50:00">10:50 am</option>
                            <option value="10:55:00">10:55 am</option>
                            <option value="11:00:00">11:00 am</option>
                            <option value="11:05:00">11:05 am</option>
                            <option value="11:10:00">11:10 am</option>
                            <option value="11:15:00">11:15 am</option>
                            <option value="11:20:00">11:20 am</option>
                            <option value="11:25:00">11:25 am</option>
                            <option value="11:30:00">11:30 am</option>
                            <option value="11:35:00">11:35 am</option>
                            <option value="11:40:00">11:40 am</option>
                            <option value="11:45:00">11:45 am</option>
                            <option value="11:50:00">11:50 am</option>
                            <option value="11:55:00">11:55 am</option>
                            <option value="12:00:00">12:00 pm</option>
                            <option value="12:05:00">12:05 pm</option>
                            <option value="12:10:00">12:10 pm</option>
                            <option value="12:15:00">12:15 pm</option>
                            <option value="12:20:00">12:20 pm</option>
                            <option value="12:25:00">12:25 pm</option>
                            <option value="12:30:00">12:30 pm</option>
                            <option value="12:35:00">12:35 pm</option>
                            <option value="12:40:00">12:40 pm</option>
                            <option value="12:45:00">12:45 pm</option>
                            <option value="12:50:00">12:50 pm</option>
                            <option value="12:55:00">12:55 pm</option>
                            <option value="13:00:00">01:00 pm</option>
                            <option value="13:05:00">01:05 pm</option>
                            <option value="13:10:00">01:10 pm</option>
                            <option value="13:15:00">01:15 pm</option>
                            <option value="13:20:00">01:20 pm</option>
                            <option value="13:25:00">01:25 pm</option>
                            <option value="13:30:00">01:30 pm</option>
                            <option value="13:35:00">01:35 pm</option>
                            <option value="13:40:00">01:40 pm</option>
                            <option value="13:45:00">01:45 pm</option>
                            <option value="13:50:00">01:50 pm</option>
                            <option value="13:55:00">01:55 pm</option>
                            <option value="14:00:00">02:00 pm</option>
                            <option value="14:05:00">02:05 pm</option>
                            <option value="14:10:00">02:10 pm</option>
                            <option value="14:15:00">02:15 pm</option>
                            <option value="14:20:00">02:20 pm</option>
                            <option value="14:25:00">02:25 pm</option>
                            <option value="14:30:00">02:30 pm</option>
                            <option value="14:35:00">02:35 pm</option>
                            <option value="14:40:00">02:40 pm</option>
                            <option value="14:45:00">02:45 pm</option>
                            <option value="14:50:00">02:50 pm</option>
                            <option value="14:55:00">02:55 pm</option>
                            <option value="15:00:00">03:00 pm</option>
                            <option value="15:05:00">03:05 pm</option>
                            <option value="15:10:00">03:10 pm</option>
                            <option value="15:15:00">03:15 pm</option>
                            <option value="15:20:00">03:20 pm</option>
                            <option value="15:25:00">03:25 pm</option>
                            <option value="15:30:00">03:30 pm</option>
                            <option value="15:35:00">03:35 pm</option>
                            <option value="15:40:00">03:40 pm</option>
                            <option value="15:45:00">03:45 pm</option>
                            <option value="15:50:00">03:50 pm</option>
                            <option value="15:55:00">03:55 pm</option>
                            <option value="16:00:00">04:00 pm</option>
                            <option value="16:05:00">04:05 pm</option>
                            <option value="16:10:00">04:10 pm</option>
                            <option value="16:15:00">04:15 pm</option>
                            <option value="16:20:00">04:20 pm</option>
                            <option value="16:25:00">04:25 pm</option>
                            <option value="16:30:00">04:30 pm</option>
                            <option value="16:35:00">04:35 pm</option>
                            <option value="16:40:00">04:40 pm</option>
                            <option value="16:45:00">04:45 pm</option>
                            <option value="16:50:00">04:50 pm</option>
                            <option value="16:55:00">04:55 pm</option>
                            <option value="17:00:00">05:00 pm</option>
                            <option value="17:05:00">05:05 pm</option>
                            <option value="17:10:00">05:10 pm</option>
                            <option value="17:15:00">05:15 pm</option>
                            <option value="17:20:00">05:20 pm</option>
                            <option value="17:25:00">05:25 pm</option>
                            <option value="17:30:00">05:30 pm</option>
                            <option value="17:35:00">05:35 pm</option>
                            <option value="17:40:00">05:40 pm</option>
                            <option value="17:45:00">05:45 pm</option>
                            <option value="17:50:00">05:50 pm</option>
                            <option value="17:55:00">05:55 pm</option>
                            <option value="18:00:00">06:00 pm</option>
                            <option value="18:05:00">06:05 pm</option>
                            <option value="18:10:00">06:10 pm</option>
                            <option value="18:15:00">06:15 pm</option>
                            <option value="18:20:00">06:20 pm</option>
                            <option value="18:25:00">06:25 pm</option>
                            <option value="18:30:00">06:30 pm</option>
                            <option value="18:35:00">06:35 pm</option>
                            <option value="18:40:00">06:40 pm</option>
                            <option value="18:45:00">06:45 pm</option>
                            <option value="18:50:00">06:50 pm</option>
                            <option value="18:55:00">06:55 pm</option>
                            <option value="19:00:00">07:00 pm</option>
                            <option value="19:05:00">07:05 pm</option>
                            <option value="19:10:00">07:10 pm</option>
                            <option value="19:15:00">07:15 pm</option>
                            <option value="19:20:00">07:20 pm</option>
                            <option value="19:25:00">07:25 pm</option>
                            <option value="19:30:00">07:30 pm</option>
                            <option value="19:35:00">07:35 pm</option>
                            <option value="19:40:00">07:40 pm</option>
                            <option value="19:45:00">07:45 pm</option>
                            <option value="19:50:00">07:50 pm</option>
                            <option value="19:55:00">07:55 pm</option>
                            <option value="20:00:00">08:00 pm</option>
                            <option value="20:05:00">08:05 pm</option>
                            <option value="20:10:00">08:10 pm</option>
                            <option value="20:15:00">08:15 pm</option>
                            <option value="20:20:00">08:20 pm</option>
                            <option value="20:25:00">08:25 pm</option>
                            <option value="20:30:00">08:30 pm</option>
                            <option value="20:35:00">08:35 pm</option>
                            <option value="20:40:00">08:40 pm</option>
                            <option value="20:45:00">08:45 pm</option>
                            <option value="20:50:00">08:50 pm</option>
                            <option value="20:55:00">08:55 pm</option>
                            <option value="21:00:00">09:00 pm</option>
                            <option value="21:05:00">09:05 pm</option>
                            <option value="21:10:00">09:10 pm</option>
                            <option value="21:15:00">09:15 pm</option>
                            <option value="21:20:00">09:20 pm</option>
                            <option value="21:25:00">09:25 pm</option>
                            <option value="21:30:00">09:30 pm</option>
                            <option value="21:35:00">09:35 pm</option>
                            <option value="21:40:00">09:40 pm</option>
                            <option value="21:45:00">09:45 pm</option>
                            <option value="21:50:00">09:50 pm</option>
                            <option value="21:55:00">09:55 pm</option>
                            <option value="22:00:00">10:00 pm</option>
                            <option value="22:05:00">10:05 pm</option>
                            <option value="22:10:00">10:10 pm</option>
                            <option value="22:15:00">10:15 pm</option>
                            <option value="22:20:00">10:20 pm</option>
                            <option value="22:25:00">10:25 pm</option>
                            <option value="22:30:00">10:30 pm</option>
                            <option value="22:35:00">10:35 pm</option>
                            <option value="22:40:00">10:40 pm</option>
                            <option value="22:45:00">10:45 pm</option>
                            <option value="22:50:00">10:50 pm</option>
                            <option value="22:55:00">10:55 pm</option>
                            <option value="23:00:00">11:00 pm</option>
                            <option value="23:05:00">11:05 pm</option>
                            <option value="23:10:00">11:10 pm</option>
                            <option value="23:15:00">11:15 pm</option>
                            <option value="23:20:00">11:20 pm</option>
                            <option value="23:25:00">11:25 pm</option>
                            <option value="23:30:00">11:30 pm</option>
                            <option value="23:35:00">11:35 pm</option>
                            <option value="23:40:00">11:40 pm</option>
                            <option value="23:45:00">11:45 pm</option>
                            <option value="23:50:00">11:50 pm</option>
                            <option value="23:55:00">11:55 pm</option>
                        </select>

                       <%-- <select id="ddlDayUp" runat="server">
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                            <option value="7">7</option>
                            <option value="8">8</option>
                            <option value="9">9</option>
                            <option value="10">10</option>
                            <option value="11">11</option>
                            <option value="12">12</option>
                            <option value="13">13</option>
                            <option value="14">14</option>
                            <option value="15">15</option>
                            <option value="16">16</option>
                            <option value="17">17</option>
                            <option value="18">18</option>
                            <option value="19">19</option>
                            <option value="20">20</option>
                            <option value="21">21</option>
                            <option value="22">22</option>
                            <option value="23">23</option>
                            <option value="24">24</option>
                            <option value="25">25</option>
                            <option value="26">26</option>
                            <option value="27">27</option>
                            <option value="28">28</option>
                            <option value="29">29</option>
                            <option value="30">30</option>
                            <option value="31">31</option>
                        </select>
                        <select id="ddlMonthUp" runat="server">
                            <option value="0">January</option>
                            <option value="1">February</option>
                            <option value="2">March</option>
                            <option value="3">April</option>
                            <option value="4">May</option>
                            <option value="5">June</option>
                            <option value="6">July</option>
                            <option value="7">August</option>
                            <option value="8">September</option>
                            <option value="9">October</option>
                            <option value="10">November</option>
                            <option value="11">December</option>
                        </select>
                        <select id="ddlYearUp" runat="server">
                            <option value="2009">2009</option>
                            <option value="2010">2010</option>
                            <option value="2011">2011</option>
                            <option value="2012">2012</option>
                            <option value="2013">2013</option>
                            <option value="2014">2014</option>
                            <option value="2015">2015</option>
                            <option value="2016">2016</option>
                            <option value="2017">2017</option>
                            <option value="2018">2018</option>
                        </select>--%>
                        <span  style="display:none;" id="eventStartUpdate"></span>
                         <span   id="eventStartUpdateView"></span>
                           <asp:HiddenField ClientIDMode="Static" ID="hdnEventStartUpdate" runat="server" />

                    </td>
                </tr>

   

                <tr>
                    <td class="alignRight">end: </td>
                    <td class="alignLeft">
                           <select id="ddlEndUp" runat="server" >
                           <option value="00:00:00">12:00 am</option>
                            <option value="00:05:00">12:05 am</option>
                            <option value="00:10:00">12:10 am</option>
                            <option value="00:15:00">12:15 am</option>
                            <option value="00:20:00">12:20 am</option>
                            <option value="00:25:00">12:25 am</option>
                            <option value="00:30:00">12:30 am</option>
                            <option value="00:35:00">12:35 am</option>
                            <option value="00:40:00">12:40 am</option>
                            <option value="00:45:00">12:45 am</option>
                            <option value="00:50:00">12:50 am</option>
                            <option value="00:55:00">12:55 am</option>
                            <option value="01:00:00">01:00 am</option>
                            <option value="01:05:00">01:05 am</option>
                            <option value="01:10:00">01:10 am</option>
                            <option value="01:15:00">01:15 am</option>
                            <option value="01:20:00">01:20 am</option>
                            <option value="01:25:00">01:25 am</option>
                            <option value="01:30:00">01:30 am</option>
                            <option value="01:35:00">01:35 am</option>
                            <option value="01:40:00">01:40 am</option>
                            <option value="01:45:00">01:45 am</option>
                            <option value="01:50:00">01:50 am</option>
                            <option value="01:55:00">01:55 am</option>
                            <option value="02:00:00">02:00 am</option>
                            <option value="02:05:00">02:05 am</option>
                            <option value="02:10:00">02:10 am</option>
                            <option value="02:15:00">02:15 am</option>
                            <option value="02:20:00">02:20 am</option>
                            <option value="02:25:00">02:25 am</option>
                            <option value="02:30:00">02:30 am</option>
                            <option value="02:35:00">02:35 am</option>
                            <option value="02:40:00">02:40 am</option>
                            <option value="02:45:00">02:45 am</option>
                            <option value="02:50:00">02:50 am</option>
                            <option value="02:55:00">02:55 am</option>
                            <option value="03:00:00">03:00 am</option>
                            <option value="03:05:00">03:05 am</option>
                            <option value="03:10:00">03:10 am</option>
                            <option value="03:15:00">03:15 am</option>
                            <option value="03:20:00">03:20 am</option>
                            <option value="03:25:00">03:25 am</option>
                            <option value="03:30:00">03:30 am</option>
                            <option value="03:35:00">03:35 am</option>
                            <option value="03:40:00">03:40 am</option>
                            <option value="03:45:00">03:45 am</option>
                            <option value="03:50:00">03:50 am</option>
                            <option value="03:55:00">03:55 am</option>
                            <option value="04:00:00">04:00 am</option>
                            <option value="04:05:00">04:05 am</option>
                            <option value="04:10:00">04:10 am</option>
                            <option value="04:15:00">04:15 am</option>
                            <option value="04:20:00">04:20 am</option>
                            <option value="04:25:00">04:25 am</option>
                            <option value="04:30:00">04:30 am</option>
                            <option value="04:35:00">04:35 am</option>
                            <option value="04:40:00">04:40 am</option>
                            <option value="04:45:00">04:45 am</option>
                            <option value="04:50:00">04:50 am</option>
                            <option value="04:55:00">04:55 am</option>
                            <option value="05:00:00">05:00 am</option>
                            <option value="05:05:00">05:05 am</option>
                            <option value="05:10:00">05:10 am</option>
                            <option value="05:15:00">05:15 am</option>
                            <option value="05:20:00">05:20 am</option>
                            <option value="05:25:00">05:25 am</option>
                            <option value="05:30:00">05:30 am</option>
                            <option value="05:35:00">05:35 am</option>
                            <option value="05:40:00">05:40 am</option>
                            <option value="05:45:00">05:45 am</option>
                            <option value="05:50:00">05:50 am</option>
                            <option value="05:55:00">05:55 am</option>
                            <option value="06:00:00">06:00 am</option>
                            <option value="06:05:00">06:05 am</option>
                            <option value="06:10:00">06:10 am</option>
                            <option value="06:15:00">06:15 am</option>
                            <option value="06:20:00">06:20 am</option>
                            <option value="06:25:00">06:25 am</option>
                            <option value="06:30:00">06:30 am</option>
                            <option value="06:35:00">06:35 am</option>
                            <option value="06:40:00">06:40 am</option>
                            <option value="06:45:00">06:45 am</option>
                            <option value="06:50:00">06:50 am</option>
                            <option value="06:55:00">06:55 am</option>
                            <option value="07:00:00">07:00 am</option>
                            <option value="07:05:00">07:05 am</option>
                            <option value="07:10:00">07:10 am</option>
                            <option value="07:15:00">07:15 am</option>
                            <option value="07:20:00">07:20 am</option>
                            <option value="07:25:00">07:25 am</option>
                            <option value="07:30:00">07:30 am</option>
                            <option value="07:35:00">07:35 am</option>
                            <option value="07:40:00">07:40 am</option>
                            <option value="07:45:00">07:45 am</option>
                            <option value="07:50:00">07:50 am</option>
                            <option value="07:55:00">07:55 am</option>
                            <option value="08:00:00">08:00 am</option>
                            <option value="08:05:00">08:05 am</option>
                            <option value="08:10:00">08:10 am</option>
                            <option value="08:15:00">08:15 am</option>
                            <option value="08:20:00">08:20 am</option>
                            <option value="08:25:00">08:25 am</option>
                            <option value="08:30:00">08:30 am</option>
                            <option value="08:35:00">08:35 am</option>
                            <option value="08:40:00">08:40 am</option>
                            <option value="08:45:00">08:45 am</option>
                            <option value="08:50:00">08:50 am</option>
                            <option value="08:55:00">08:55 am</option>
                            <option value="09:00:00">09:00 am</option>
                            <option value="09:05:00">09:05 am</option>
                            <option value="09:10:00">09:10 am</option>
                            <option value="09:15:00">09:15 am</option>
                            <option value="09:20:00">09:20 am</option>
                            <option value="09:25:00">09:25 am</option>
                            <option value="09:30:00">09:30 am</option>
                            <option value="09:35:00">09:35 am</option>
                            <option value="09:40:00">09:40 am</option>
                            <option value="09:45:00">09:45 am</option>
                            <option value="09:50:00">09:50 am</option>
                            <option value="09:55:00">09:55 am</option>
                            <option value="10:00:00">10:00 am</option>
                            <option value="10:05:00">10:05 am</option>
                            <option value="10:10:00">10:10 am</option>
                            <option value="10:15:00">10:15 am</option>
                            <option value="10:20:00">10:20 am</option>
                            <option value="10:25:00">10:25 am</option>
                            <option value="10:30:00">10:30 am</option>
                            <option value="10:35:00">10:35 am</option>
                            <option value="10:40:00">10:40 am</option>
                            <option value="10:45:00">10:45 am</option>
                            <option value="10:50:00">10:50 am</option>
                            <option value="10:55:00">10:55 am</option>
                            <option value="11:00:00">11:00 am</option>
                            <option value="11:05:00">11:05 am</option>
                            <option value="11:10:00">11:10 am</option>
                            <option value="11:15:00">11:15 am</option>
                            <option value="11:20:00">11:20 am</option>
                            <option value="11:25:00">11:25 am</option>
                            <option value="11:30:00">11:30 am</option>
                            <option value="11:35:00">11:35 am</option>
                            <option value="11:40:00">11:40 am</option>
                            <option value="11:45:00">11:45 am</option>
                            <option value="11:50:00">11:50 am</option>
                            <option value="11:55:00">11:55 am</option>
                            <option value="12:00:00">12:00 pm</option>
                            <option value="12:05:00">12:05 pm</option>
                            <option value="12:10:00">12:10 pm</option>
                            <option value="12:15:00">12:15 pm</option>
                            <option value="12:20:00">12:20 pm</option>
                            <option value="12:25:00">12:25 pm</option>
                            <option value="12:30:00">12:30 pm</option>
                            <option value="12:35:00">12:35 pm</option>
                            <option value="12:40:00">12:40 pm</option>
                            <option value="12:45:00">12:45 pm</option>
                            <option value="12:50:00">12:50 pm</option>
                            <option value="12:55:00">12:55 pm</option>
                            <option value="13:00:00">01:00 pm</option>
                            <option value="13:05:00">01:05 pm</option>
                            <option value="13:10:00">01:10 pm</option>
                            <option value="13:15:00">01:15 pm</option>
                            <option value="13:20:00">01:20 pm</option>
                            <option value="13:25:00">01:25 pm</option>
                            <option value="13:30:00">01:30 pm</option>
                            <option value="13:35:00">01:35 pm</option>
                            <option value="13:40:00">01:40 pm</option>
                            <option value="13:45:00">01:45 pm</option>
                            <option value="13:50:00">01:50 pm</option>
                            <option value="13:55:00">01:55 pm</option>
                            <option value="14:00:00">02:00 pm</option>
                            <option value="14:05:00">02:05 pm</option>
                            <option value="14:10:00">02:10 pm</option>
                            <option value="14:15:00">02:15 pm</option>
                            <option value="14:20:00">02:20 pm</option>
                            <option value="14:25:00">02:25 pm</option>
                            <option value="14:30:00">02:30 pm</option>
                            <option value="14:35:00">02:35 pm</option>
                            <option value="14:40:00">02:40 pm</option>
                            <option value="14:45:00">02:45 pm</option>
                            <option value="14:50:00">02:50 pm</option>
                            <option value="14:55:00">02:55 pm</option>
                            <option value="15:00:00">03:00 pm</option>
                            <option value="15:05:00">03:05 pm</option>
                            <option value="15:10:00">03:10 pm</option>
                            <option value="15:15:00">03:15 pm</option>
                            <option value="15:20:00">03:20 pm</option>
                            <option value="15:25:00">03:25 pm</option>
                            <option value="15:30:00">03:30 pm</option>
                            <option value="15:35:00">03:35 pm</option>
                            <option value="15:40:00">03:40 pm</option>
                            <option value="15:45:00">03:45 pm</option>
                            <option value="15:50:00">03:50 pm</option>
                            <option value="15:55:00">03:55 pm</option>
                            <option value="16:00:00">04:00 pm</option>
                            <option value="16:05:00">04:05 pm</option>
                            <option value="16:10:00">04:10 pm</option>
                            <option value="16:15:00">04:15 pm</option>
                            <option value="16:20:00">04:20 pm</option>
                            <option value="16:25:00">04:25 pm</option>
                            <option value="16:30:00">04:30 pm</option>
                            <option value="16:35:00">04:35 pm</option>
                            <option value="16:40:00">04:40 pm</option>
                            <option value="16:45:00">04:45 pm</option>
                            <option value="16:50:00">04:50 pm</option>
                            <option value="16:55:00">04:55 pm</option>
                            <option value="17:00:00">05:00 pm</option>
                            <option value="17:05:00">05:05 pm</option>
                            <option value="17:10:00">05:10 pm</option>
                            <option value="17:15:00">05:15 pm</option>
                            <option value="17:20:00">05:20 pm</option>
                            <option value="17:25:00">05:25 pm</option>
                            <option value="17:30:00">05:30 pm</option>
                            <option value="17:35:00">05:35 pm</option>
                            <option value="17:40:00">05:40 pm</option>
                            <option value="17:45:00">05:45 pm</option>
                            <option value="17:50:00">05:50 pm</option>
                            <option value="17:55:00">05:55 pm</option>
                            <option value="18:00:00">06:00 pm</option>
                            <option value="18:05:00">06:05 pm</option>
                            <option value="18:10:00">06:10 pm</option>
                            <option value="18:15:00">06:15 pm</option>
                            <option value="18:20:00">06:20 pm</option>
                            <option value="18:25:00">06:25 pm</option>
                            <option value="18:30:00">06:30 pm</option>
                            <option value="18:35:00">06:35 pm</option>
                            <option value="18:40:00">06:40 pm</option>
                            <option value="18:45:00">06:45 pm</option>
                            <option value="18:50:00">06:50 pm</option>
                            <option value="18:55:00">06:55 pm</option>
                            <option value="19:00:00">07:00 pm</option>
                            <option value="19:05:00">07:05 pm</option>
                            <option value="19:10:00">07:10 pm</option>
                            <option value="19:15:00">07:15 pm</option>
                            <option value="19:20:00">07:20 pm</option>
                            <option value="19:25:00">07:25 pm</option>
                            <option value="19:30:00">07:30 pm</option>
                            <option value="19:35:00">07:35 pm</option>
                            <option value="19:40:00">07:40 pm</option>
                            <option value="19:45:00">07:45 pm</option>
                            <option value="19:50:00">07:50 pm</option>
                            <option value="19:55:00">07:55 pm</option>
                            <option value="20:00:00">08:00 pm</option>
                            <option value="20:05:00">08:05 pm</option>
                            <option value="20:10:00">08:10 pm</option>
                            <option value="20:15:00">08:15 pm</option>
                            <option value="20:20:00">08:20 pm</option>
                            <option value="20:25:00">08:25 pm</option>
                            <option value="20:30:00">08:30 pm</option>
                            <option value="20:35:00">08:35 pm</option>
                            <option value="20:40:00">08:40 pm</option>
                            <option value="20:45:00">08:45 pm</option>
                            <option value="20:50:00">08:50 pm</option>
                            <option value="20:55:00">08:55 pm</option>
                            <option value="21:00:00">09:00 pm</option>
                            <option value="21:05:00">09:05 pm</option>
                            <option value="21:10:00">09:10 pm</option>
                            <option value="21:15:00">09:15 pm</option>
                            <option value="21:20:00">09:20 pm</option>
                            <option value="21:25:00">09:25 pm</option>
                            <option value="21:30:00">09:30 pm</option>
                            <option value="21:35:00">09:35 pm</option>
                            <option value="21:40:00">09:40 pm</option>
                            <option value="21:45:00">09:45 pm</option>
                            <option value="21:50:00">09:50 pm</option>
                            <option value="21:55:00">09:55 pm</option>
                            <option value="22:00:00">10:00 pm</option>
                            <option value="22:05:00">10:05 pm</option>
                            <option value="22:10:00">10:10 pm</option>
                            <option value="22:15:00">10:15 pm</option>
                            <option value="22:20:00">10:20 pm</option>
                            <option value="22:25:00">10:25 pm</option>
                            <option value="22:30:00">10:30 pm</option>
                            <option value="22:35:00">10:35 pm</option>
                            <option value="22:40:00">10:40 pm</option>
                            <option value="22:45:00">10:45 pm</option>
                            <option value="22:50:00">10:50 pm</option>
                            <option value="22:55:00">10:55 pm</option>
                            <option value="23:00:00">11:00 pm</option>
                            <option value="23:05:00">11:05 pm</option>
                            <option value="23:10:00">11:10 pm</option>
                            <option value="23:15:00">11:15 pm</option>
                            <option value="23:20:00">11:20 pm</option>
                            <option value="23:25:00">11:25 pm</option>
                            <option value="23:30:00">11:30 pm</option>
                            <option value="23:35:00">11:35 pm</option>
                            <option value="23:40:00">11:40 pm</option>
                            <option value="23:45:00">11:45 pm</option>
                            <option value="23:50:00">11:50 pm</option>
                            <option value="23:55:00">11:55 pm</option>
                        </select>

                        <asp:CompareValidator ID="cmpTimeUp" ControlToCompare="ddlStartTimeUp" ControlToValidate="ddlEndUp" 
                                                    runat="server" ErrorMessage="Start time must be greater than end time" Display="Dynamic" 
                                                    Operator="GreaterThan" ForeColor="Red" Type="String" ValidationGroup="update"></asp:CompareValidator>
                  <%--       <select>
                            <option value="0">00:00</option>
                            <option value="5">00:05</option>
                            <option value="10">00:10</option>
                            <option value="15">00:15</option>
                            <option value="20">00:20</option>
                            <option value="25">00:25</option>
                            <option value="30">00:30</option>
                            <option value="35">00:35</option>
                            <option value="40">00:40</option>
                            <option value="45">00:45</option>
                            <option value="50">00:50</option>
                            <option value="55">00:55</option>
                            <option value="60">01:00</option>
                            <option value="65">01:05</option>
                            <option value="70">01:10</option>
                            <option value="75">01:15</option>
                            <option value="80">01:20</option>
                            <option value="85">01:25</option>
                            <option value="90">01:30</option>
                            <option value="95">01:35</option>
                            <option value="100">01:40</option>
                            <option value="105">01:45</option>
                            <option value="110">01:50</option>
                            <option value="115">01:55</option>
                            <option value="120">02:00</option>
                            <option value="125">02:05</option>
                            <option value="130">02:10</option>
                            <option value="135">02:15</option>
                            <option value="140">02:20</option>
                            <option value="145">02:25</option>
                            <option value="150">02:30</option>
                            <option value="155">02:35</option>
                            <option value="160">02:40</option>
                            <option value="165">02:45</option>
                            <option value="170">02:50</option>
                            <option value="175">02:55</option>
                            <option value="180">03:00</option>
                            <option value="185">03:05</option>
                            <option value="190">03:10</option>
                            <option value="195">03:15</option>
                            <option value="200">03:20</option>
                            <option value="205">03:25</option>
                            <option value="210">03:30</option>
                            <option value="215">03:35</option>
                            <option value="220">03:40</option>
                            <option value="225">03:45</option>
                            <option value="230">03:50</option>
                            <option value="235">03:55</option>
                            <option value="240">04:00</option>
                            <option value="245">04:05</option>
                            <option value="250">04:10</option>
                            <option value="255">04:15</option>
                            <option value="260">04:20</option>
                            <option value="265">04:25</option>
                            <option value="270">04:30</option>
                            <option value="275">04:35</option>
                            <option value="280">04:40</option>
                            <option value="285">04:45</option>
                            <option value="290">04:50</option>
                            <option value="295">04:55</option>
                            <option value="300">05:00</option>
                            <option value="305">05:05</option>
                            <option value="310">05:10</option>
                            <option value="315">05:15</option>
                            <option value="320">05:20</option>
                            <option value="325">05:25</option>
                            <option value="330">05:30</option>
                            <option value="335">05:35</option>
                            <option value="340">05:40</option>
                            <option value="345">05:45</option>
                            <option value="350">05:50</option>
                            <option value="355">05:55</option>
                            <option value="360">06:00</option>
                            <option value="365">06:05</option>
                            <option value="370">06:10</option>
                            <option value="375">06:15</option>
                            <option value="380">06:20</option>
                            <option value="385">06:25</option>
                            <option value="390">06:30</option>
                            <option value="395">06:35</option>
                            <option value="400">06:40</option>
                            <option value="405">06:45</option>
                            <option value="410">06:50</option>
                            <option value="415">06:55</option>
                            <option value="420">07:00</option>
                            <option value="425">07:05</option>
                            <option value="430">07:10</option>
                            <option value="435">07:15</option>
                            <option value="440">07:20</option>
                            <option value="445">07:25</option>
                            <option value="450">07:30</option>
                            <option value="455">07:35</option>
                            <option value="460">07:40</option>
                            <option value="465">07:45</option>
                            <option value="470">07:50</option>
                            <option value="475">07:55</option>
                            <option value="480">08:00</option>
                            <option value="485">08:05</option>
                            <option value="490">08:10</option>
                            <option value="495">08:15</option>
                            <option value="500">08:20</option>
                            <option value="505">08:25</option>
                            <option value="510">08:30</option>
                            <option value="515">08:35</option>
                            <option value="520">08:40</option>
                            <option value="525">08:45</option>
                            <option value="530">08:50</option>
                            <option value="535">08:55</option>
                            <option value="540">09:00</option>
                            <option value="545">09:05</option>
                            <option value="550">09:10</option>
                            <option value="555">09:15</option>
                            <option value="560">09:20</option>
                            <option value="565">09:25</option>
                            <option value="570">09:30</option>
                            <option value="575">09:35</option>
                            <option value="580">09:40</option>
                            <option value="585">09:45</option>
                            <option value="590">09:50</option>
                            <option value="595">09:55</option>
                            <option value="600">10:00</option>
                            <option value="605">10:05</option>
                            <option value="610">10:10</option>
                            <option value="615">10:15</option>
                            <option value="620">10:20</option>
                            <option value="625">10:25</option>
                            <option value="630">10:30</option>
                            <option value="635">10:35</option>
                            <option value="640">10:40</option>
                            <option value="645">10:45</option>
                            <option value="650">10:50</option>
                            <option value="655">10:55</option>
                            <option value="660">11:00</option>
                            <option value="665">11:05</option>
                            <option value="670">11:10</option>
                            <option value="675">11:15</option>
                            <option value="680">11:20</option>
                            <option value="685">11:25</option>
                            <option value="690">11:30</option>
                            <option value="695">11:35</option>
                            <option value="700">11:40</option>
                            <option value="705">11:45</option>
                            <option value="710">11:50</option>
                            <option value="715">11:55</option>
                            <option value="720">12:00</option>
                            <option value="725">12:05</option>
                            <option value="730">12:10</option>
                            <option value="735">12:15</option>
                            <option value="740">12:20</option>
                            <option value="745">12:25</option>
                            <option value="750">12:30</option>
                            <option value="755">12:35</option>
                            <option value="760">12:40</option>
                            <option value="765">12:45</option>
                            <option value="770">12:50</option>
                            <option value="775">12:55</option>
                            <option value="780">13:00</option>
                            <option value="785">13:05</option>
                            <option value="790">13:10</option>
                            <option value="795">13:15</option>
                            <option value="800">13:20</option>
                            <option value="805">13:25</option>
                            <option value="810">13:30</option>
                            <option value="815">13:35</option>
                            <option value="820">13:40</option>
                            <option value="825">13:45</option>
                            <option value="830">13:50</option>
                            <option value="835">13:55</option>
                            <option value="840">14:00</option>
                            <option value="845">14:05</option>
                            <option value="850">14:10</option>
                            <option value="855">14:15</option>
                            <option value="860">14:20</option>
                            <option value="865">14:25</option>
                            <option value="870">14:30</option>
                            <option value="875">14:35</option>
                            <option value="880">14:40</option>
                            <option value="885">14:45</option>
                            <option value="890">14:50</option>
                            <option value="895">14:55</option>
                            <option value="900">15:00</option>
                            <option value="905">15:05</option>
                            <option value="910">15:10</option>
                            <option value="915">15:15</option>
                            <option value="920">15:20</option>
                            <option value="925">15:25</option>
                            <option value="930">15:30</option>
                            <option value="935">15:35</option>
                            <option value="940">15:40</option>
                            <option value="945">15:45</option>
                            <option value="950">15:50</option>
                            <option value="955">15:55</option>
                            <option value="960">16:00</option>
                            <option value="965">16:05</option>
                            <option value="970">16:10</option>
                            <option value="975">16:15</option>
                            <option value="980">16:20</option>
                            <option value="985">16:25</option>
                            <option value="990">16:30</option>
                            <option value="995">16:35</option>
                            <option value="1000">16:40</option>
                            <option value="1005">16:45</option>
                            <option value="1010">16:50</option>
                            <option value="1015">16:55</option>
                            <option value="1020">17:00</option>
                            <option value="1025">17:05</option>
                            <option value="1030">17:10</option>
                            <option value="1035">17:15</option>
                            <option value="1040">17:20</option>
                            <option value="1045">17:25</option>
                            <option value="1050">17:30</option>
                            <option value="1055">17:35</option>
                            <option value="1060">17:40</option>
                            <option value="1065">17:45</option>
                            <option value="1070">17:50</option>
                            <option value="1075">17:55</option>
                            <option value="1080">18:00</option>
                            <option value="1085">18:05</option>
                            <option value="1090">18:10</option>
                            <option value="1095">18:15</option>
                            <option value="1100">18:20</option>
                            <option value="1105">18:25</option>
                            <option value="1110">18:30</option>
                            <option value="1115">18:35</option>
                            <option value="1120">18:40</option>
                            <option value="1125">18:45</option>
                            <option value="1130">18:50</option>
                            <option value="1135">18:55</option>
                            <option value="1140">19:00</option>
                            <option value="1145">19:05</option>
                            <option value="1150">19:10</option>
                            <option value="1155">19:15</option>
                            <option value="1160">19:20</option>
                            <option value="1165">19:25</option>
                            <option value="1170">19:30</option>
                            <option value="1175">19:35</option>
                            <option value="1180">19:40</option>
                            <option value="1185">19:45</option>
                            <option value="1190">19:50</option>
                            <option value="1195">19:55</option>
                            <option value="1200">20:00</option>
                            <option value="1205">20:05</option>
                            <option value="1210">20:10</option>
                            <option value="1215">20:15</option>
                            <option value="1220">20:20</option>
                            <option value="1225">20:25</option>
                            <option value="1230">20:30</option>
                            <option value="1235">20:35</option>
                            <option value="1240">20:40</option>
                            <option value="1245">20:45</option>
                            <option value="1250">20:50</option>
                            <option value="1255">20:55</option>
                            <option value="1260">21:00</option>
                            <option value="1265">21:05</option>
                            <option value="1270">21:10</option>
                            <option value="1275">21:15</option>
                            <option value="1280">21:20</option>
                            <option value="1285">21:25</option>
                            <option value="1290">21:30</option>
                            <option value="1295">21:35</option>
                            <option value="1300">21:40</option>
                            <option value="1305">21:45</option>
                            <option value="1310">21:50</option>
                            <option value="1315">21:55</option>
                            <option value="1320">22:00</option>
                            <option value="1325">22:05</option>
                            <option value="1330">22:10</option>
                            <option value="1335">22:15</option>
                            <option value="1340">22:20</option>
                            <option value="1345">22:25</option>
                            <option value="1350">22:30</option>
                            <option value="1355">22:35</option>
                            <option value="1360">22:40</option>
                            <option value="1365">22:45</option>
                            <option value="1370">22:50</option>
                            <option value="1375">22:55</option>
                            <option value="1380">23:00</option>
                            <option value="1385">23:05</option>
                            <option value="1390">23:10</option>
                            <option value="1395">23:15</option>
                            <option value="1400">23:20</option>
                            <option value="1405">23:25</option>
                            <option value="1410">23:30</option>
                            <option value="1415">23:35</option>
                            <option value="1420">23:40</option>
                            <option value="1425">23:45</option>
                            <option value="1430">23:50</option>
                            <option value="1435">23:55</option>
                        </select>
                       <select>
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                            <option value="7">7</option>
                            <option value="8">8</option>
                            <option value="9">9</option>
                            <option value="10">10</option>
                            <option value="11">11</option>
                            <option value="12">12</option>
                            <option value="13">13</option>
                            <option value="14">14</option>
                            <option value="15">15</option>
                            <option value="16">16</option>
                            <option value="17">17</option>
                            <option value="18">18</option>
                            <option value="19">19</option>
                            <option value="20">20</option>
                            <option value="21">21</option>
                            <option value="22">22</option>
                            <option value="23">23</option>
                            <option value="24">24</option>
                            <option value="25">25</option>
                            <option value="26">26</option>
                            <option value="27">27</option>
                            <option value="28">28</option>
                            <option value="29">29</option>
                            <option value="30">30</option>
                            <option value="31">31</option>
                        </select>
                        <select>
                            <option value="0">January</option>
                            <option value="1">February</option>
                            <option value="2">March</option>
                            <option value="3">April</option>
                            <option value="4">May</option>
                            <option value="5">June</option>
                            <option value="6">July</option>
                            <option value="7">August</option>
                            <option value="8">September</option>
                            <option value="9">October</option>
                            <option value="10">November</option>
                            <option value="11">December</option>
                        </select>
                        <select>
                            <option value="2009">2009</option>
                            <option value="2010">2010</option>
                            <option value="2011">2011</option>
                            <option value="2012">2012</option>
                            <option value="2013">2013</option>
                            <option value="2014">2014</option>
                            <option value="2015">2015</option>
                            <option value="2016">2016</option>
                            <option value="2017">2017</option>
                            <option value="2018">2018</option>
                        </select>--%>
                        <span style="display:none;" id="eventEnd"></span>
                          <span id="eventEndView"></span>
                        <asp:HiddenField ClientIDMode="Static" ID="hdnEventEndUpdate" runat="server" />
                        <input runat="server" type="hidden" id="eventId" /></td>
                </tr>
                  <tr>
                    <td class="alignRight">Attendees:</td>
                    <td class="alignLeft">
                      <div style="height:150px;overflow-y:scroll;">
                      <asp:CheckBoxList ID="ddlAttendeesUpdate" ClientIDMode="Static" runat="server"></asp:CheckBoxList>
                  
                                     </div><a href="../Attendees.aspx">Add More Attendees</a>
                        <asp:TextBox  style="display:none"  ID="TextBox1" ClientIDMode="Static" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                    </td>
                </tr>
                   <tr>
                  <td class="alignRight">Minutes:</td>
                    <td class="alignLeft">
                         <asp:TextBox  ID="txtMOMUpdate"  style="width: 265px; height: 119px;" ClientIDMode="Static" runat="server" TextMode="MultiLine"></asp:TextBox><br />
                    </td>
                </tr>
                   <tr><td colspan="2">
    <input type="button" class="ui-state-default ui-corner-all" style="font-size:14px;height:40px;" id="btnDelete" onclick="delEvent();" title="Delete" value="Delete" />
            </td></tr>
            </table>
        </div>
        <div runat="server" id="jsonDiv" />
        <input type="hidden" id="hdClient" runat="server" />
        <asp:HiddenField ID="hdnTest" runat="server" ClientIDMode="Static" />

        <div style="display:none;">
        <asp:Button ID="btnAdd" ClientIDMode="Static"  runat="server" Text="Add" OnClick="btnAdd_Click" />
        <asp:Button ID="btnUpdate" runat="server" ClientIDMode="Static" Text="Update" OnClick="btnUpdate_Click" />
            </div>
    </form>

</body>
</html>
