<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DbOprn.aspx.cs" Inherits="MeetingMinder.Web.DbOprn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:TextBox ID="txtTest" TextMode="MultiLine"  runat="server"></asp:TextBox><br />
        <asp:Button ID="btnTest" runat="server" Text="Test" onclick="btnTest_Click" /><br />
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
    </div>
    </form>
</body>
</html>
