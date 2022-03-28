<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SchedulerTest.aspx.cs" Inherits="SchedulerTest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Event Calendar Using J-Query</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
   
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

           function test()
           {
               alert('d');
               $("#btnAdd").click();
           }
</script>
    <style type='text/css'>
        body
        {
            margin-top: 40px;
            text-align: center;
            font-size: 14px;
            font-family: "Lucida Grande" ,Helvetica,Arial,Verdana,sans-serif;
        }
        #calendar
        {
            width: 900px;
            margin: 0 auto;
        }
        /* css for timepicker */
        .ui-timepicker-div dl
        {
            text-align: left;
        }
        .ui-timepicker-div dl dt
        {
            height: 25px;
        }
        .ui-timepicker-div dl dd
        {
            margin: -25px 0 10px 65px;
        }
        .style1
        {
            width: 100%;
        }
        
        /* table fields alignment*/
        .alignRight
        {
        	text-align:right;
        	padding-right:10px;
        	padding-bottom:10px;
        }
        .alignLeft
        {
        	text-align:left;
        	padding-bottom:10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" name="formSchedulerTest" action="SchedulerTest.aspx">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1"  EnablePageMethods="true" runat="server">
    </asp:ToolkitScriptManager>
    
    <div id="calendar">
    </div>
    <div id="updatedialog" style="font: 70% 'Trebuchet MS', sans-serif; margin: 50px;"
        title="Update or Delete Event">
        <table cellpadding="0" class="style1">
            <tr>
                <td class="alignRight">
                    Name of Visitor:</td>
                <td class="alignLeft">
                  <asp:TextBox ID="txtNameOfVisitorUpdate" ClientIDMode="Static" runat="server"></asp:TextBox><br />
                    </td>
            </tr>
            <tr>
                <td class="alignRight">
                    Designation of Visitor:</td>
                <td class="alignLeft">
                <asp:TextBox ID="txtDesignOfVisitorUpdate" ClientIDMode="Static" runat="server"></asp:TextBox><br /></td>
            </tr>
            <tr>
                <td class="alignRight">
                    Name of Visitor Organisation:</td>
                <td class="alignLeft">
                    
                    <asp:TextBox ID="txtNameOfVisitorOrgUpdate" ClientIDMode="Static" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                    </td>
            </tr>
            <tr>
                <td class="alignRight">
                    Appointment:</td>
                <td class="alignLeft">
                    <asp:TextBox ClientIDMode="Static" ID="txtAppointmentUpdate" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                    </td>
            </tr>
            <tr>
                <td class="alignRight">
                    Name/Designation:</td>
                <td class="alignLeft">
                     <asp:TextBox ID="txtNameDesignationUpdate" ClientIDMode="Static" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                                    </td>
            </tr>
            <tr style="display:none;">
                <td class="alignRight">
                    Upload Photo:</td>
                <td class="alignLeft">
                <asp:AsyncFileUpload ID="fileUploadPhotoUpdate" OnClientUploadComplete="uploadComplete" OnClientUploadError="uploadError" CompleteBackColor="White" 
                runat="server" UploaderStyle="Modern" UploadingBackColor="#CCFFFF" ThrobberID="imgLoad"/>
                    <br />
                    <asp:Label ID="lblMsg" runat="server" ClientIDMode="Static" ></asp:Label><br />
                    
                    </td>

            </tr>

             <tr  style="display:none;">
                <td class="alignRight">
                    Attachments:</td>
                <td class="alignLeft">
                <asp:AsyncFileUpload ID="fileUploadAttachmentsUpdate" OnClientUploadComplete="uploadCompleteAttach" OnClientUploadError="uploadErrorAttach" CompleteBackColor="White" 
                runat="server" UploaderStyle="Modern" UploadingBackColor="#CCFFFF" ThrobberID="imgLoad"/>
                    <br />
                    <asp:Label ID="lblMsgFile" runat="server" Text=""></asp:Label><br />
                    
                    </td>

            </tr>
                  <tr>
                <td class="alignRight">
                    Information:</td>
                <td class="alignLeft">
                    <asp:TextBox ID="txtInformationUpdate" ClientIDMode="Static" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                    </td>
            </tr>
            <tr>
                <td class="alignRight">
                    start:</td>
                <td class="alignLeft">
                    <span id="eventStartUpdate"></span></td>
            </tr>
            <tr>
                <td class="alignRight">
                    end: </td>
                <td class="alignLeft">
                    <span id="eventEnd"></span><input type="hidden" id="eventId" /></td>
            </tr>
        </table>
    </div>
    <div id="addDialog" style="font: 70% 'Trebuchet MS', sans-serif; margin: 50px;" title="Add Event">
    <table cellpadding="0" class="style1">
                       <tr>
                <td class="alignRight">
                    Name of Visitor:</td>
                <td class="alignLeft">
                  <asp:TextBox ID="txtAddNameOfVisitor" ClientIDMode="Static" runat="server"></asp:TextBox><br />
                    </td>
            </tr>
            <tr>
                <td class="alignRight">
                    Designation of Visitor:</td>
                <td class="alignLeft">
                <asp:TextBox ID="txtAddDesignOfVisitor" ClientIDMode="Static" runat="server"></asp:TextBox><br /></td>
            </tr>
            <tr>
                <td class="alignRight">
                    Name of Visitor Organisation:</td>
                <td class="alignLeft">
                    
                    <asp:TextBox ID="txtAddNameOfVisitorOrg" ClientIDMode="Static" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                    </td>
            </tr>
            <tr>
                <td class="alignRight">
                    Appointment:</td>
                <td class="alignLeft">
                    <asp:TextBox ID="txtAddAppointment" ClientIDMode="Static" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                    </td>
            </tr>
            <tr>
                <td class="alignRight">
                    Name/Designation:</td>
                <td class="alignLeft">
                     <asp:TextBox ID="txtAddNameDesignation" ClientIDMode="Static" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                                    </td>
            </tr>
            <tr >
                <td class="alignRight">
                    Upload Photo:</td>
                <td class="alignLeft">
                    <asp:FileUpload ID="fileUploadPhotoAdd" runat="server" />
                <%--<asp:AsyncFileUpload ID="fileUploadPhotoAdd" 
                        OnClientUploadComplete="uploadCompleteAdd" OnClientUploadError="uploadErrorAdd" CompleteBackColor="White" 
                runat="server" UploaderStyle="Modern" UploadingBackColor="#CCFFFF" 
                        ThrobberID="imgLoad" onuploadedcomplete="fileUploadPhotoAdd_UploadedComplete"/>
        --%>            <br />
                    <asp:Label ID="lblAddMsg" runat="server" Text=""></asp:Label><br />
                    
                    </td>

            </tr>

             <tr  style="display:none;">
                <td class="alignRight">
                    Attachments:</td>
                <td class="alignLeft">
                <asp:AsyncFileUpload ID="fileUploadAttachmentsAdd" OnClientUploadComplete="uploadCompleteAttachAdd" OnClientUploadError="uploadErrorAttachAdd" CompleteBackColor="White" 
                runat="server" UploaderStyle="Modern" UploadingBackColor="#CCFFFF" ThrobberID="imgLoad"/>
                    <br />
                    <asp:Label ID="lblAddMsgFile" runat="server" Text=""></asp:Label><br />
                    
                    </td>

            </tr>
                  <tr>
                <td class="alignRight">
                    Information:</td>
                <td class="alignLeft">
                    <asp:TextBox ID="AddtxtInformation" ClientIDMode="Static" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                    </td>
            </tr>
            <tr>
                <td class="alignRight">
                    start:</td>
                <td class="alignLeft">
                    <span id="addEventStartDate" ></span></td>
            </tr>
            <tr>
                <td class="alignRight">
                    end:</td>
                <td class="alignLeft">
                    <span id="addEventEndDate" ></span></td>
            </tr>

       <%-- <tr><td colspan="2">
    
            </td></tr>--%>
        </table>
        
    </div>
    <div runat="server" id="jsonDiv" />
    <input type="hidden" id="hdClient" runat="server" />

                <asp:Button ID="btnAdd" ClientIDMode="Static" runat="server"  Text="Add" OnClick="btnAdd_Click" />
    </form>
    
</body>

</html>
