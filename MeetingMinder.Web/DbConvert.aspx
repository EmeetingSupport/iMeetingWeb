<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DbConvert.aspx.cs" Inherits="MeetingMinder.Web.DbConvert" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <%--<asp:Button ID="btnConvert" runat="server" Text="Convert" OnClick="btnConvert_Click" />--%>
        <asp:Button ID="btnConvertAboutUs" runat="server" Text="Convert AboutUs" OnClick="btnConvertAboutUs_Click" /> <br /><br />
        <asp:Button ID="btnConvertAgendaDetails" runat="server" Text="Convert AgendaDetails" OnClick="btnConvertAgendaDetails_Click" /> <br /><br />
        <asp:Button ID="btnConvertEntity" runat="server" Text="Convert Entity" OnClick="btnConvertEntity_Click" /> <br /><br />
        <asp:Button ID="btnConvertForum" runat="server" Text="Convert Forum" OnClick="btnConvertForum_Click" /> <br /><br />
        <asp:Button ID="btnConvertGlobalSetting" runat="server" Text="Convert GlobalSetting" OnClick="btnConvertGlobalSetting_Click" /> <br /><br />
        <asp:Button ID="btnConvertMeeting" runat="server" Text="Convert Meeting" OnClick="btnConvertMeeting_Click" /> <br /><br />
        <asp:Button ID="btnConvertRollMaster" runat="server" Text="Convert RollMaster" OnClick="btnConvertRollMaster_Click" /> <br /><br />
        <asp:Button ID="btnConvertUploadMinute" runat="server" Text="Convert UploadMinute" OnClick="btnConvertUploadMinute_Click" /> <br /><br />
        <asp:Button ID="btnConvertUser" runat="server" Text="Convert User" OnClick="btnConvertUser_Click" />

    </div>
    </form>
</body>
</html>
