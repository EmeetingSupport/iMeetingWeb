<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Encrypt.aspx.cs" Inherits="MeetingMinder.Web.en" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <table><tr><td>Text</td><td><asp:TextBox ID="txtDomain" runat="server"></asp:TextBox></td></tr>
     <tr><td><asp:Button ID="btnTest" runat="server" Text="Encrypt" 
            onclick="btnTest_Click" /> </td><td><asp:Button ID="Button1" runat="server" Text="Decrypt" 
            onclick="btnTest_Click1" /></td></tr>
    </table>
    <asp:Label ID="lblMsg" runat="server"></asp:Label>

  
    </div>
    </form>
</body>
</html>
