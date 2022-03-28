<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileDecrypt.aspx.cs" Inherits="MeetingMinder.Web.FileDecrypt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Key: <asp:TextBox ID="txtKey" runat="server" ></asp:TextBox> <br />
        File: <asp:FileUpload ID="fuFile" runat="server" />
        <asp:Button ID="btnConvert" runat="server" Text="Convert" OnClick="btnConvert_Click" /> 
    </div>
    </form>
</body>
</html>
