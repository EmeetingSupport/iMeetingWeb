<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ad.aspx.cs" Inherits="MeetingMinder.Web.ad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
    <table><tr><td>Domain</td><td><asp:TextBox ID="txtDomain" Text="LDAP://" runat="server"></asp:TextBox></td></tr>
    <tr><td>UserName</td><td><asp:TextBox ID="txtUserName" runat="server"></asp:TextBox></td></tr>
    <tr><td>Password</td><td><asp:TextBox ID="txtPassword"  runat="server" ></asp:TextBox></td></tr>
    <tr><td colspan="2"><asp:Button ID="btnTest" runat="server" Text="Test1" 
            onclick="btnTest_Click" /> </td></tr>
    </table>
    <asp:Button ID="Button1" runat="server" Text="Test2" 
            onclick="btnTest_Click1" />

            <asp:Button ID="Button2" runat="server" Text="Test3" 
            onclick="btnTest_Click2" />

    

        <%--    <asp:Button ID="Button4" runat="server" Text="Test4" 
            onclick="btnTest_Click4" />--%>

            
            <asp:Button ID="Button5" runat="server" Text="Test4 only Username" 
            onclick="btnTest_Click5" />

            
            <asp:Button ID="Button6" runat="server" Text="Test5 only Username" 
            onclick="btnTest_Click6" />         

           <asp:Button ID="Button9" runat="server" Text="Test4 hdfc" 
            onclick="Button9_Click" />

                <asp:Button ID="Button3" runat="server" Text="Test extra " 
            onclick="btnTest_Click3" />
        <br /><br />

           <asp:Button ID="btnSMSTest" runat="server" Text="OTP Check" 
            onclick="btnSMSTest_Click" />

       
    </div>

         <table><tr><td>Version</td><td><asp:TextBox ID="txtVersion" Text="LDAP://" runat="server"></asp:TextBox></td></tr>
    <tr><td>Key</td><td><asp:TextBox ID="txtKey" runat="server"></asp:TextBox></td></tr>
    <tr><td>Value</td><td><asp:TextBox ID="txtVal"  runat="server" ></asp:TextBox></td></tr>
             <tr><td>Userid</td><td><asp:TextBox ID="txtUserid"  runat="server" ></asp:TextBox></td></tr>
    <tr><td colspan="2"> <asp:Button ID="btnOTPVerify" runat="server" Text="OTP Verify" 
            onclick="btnOTPVerify_Click" /> </td></tr>
    </table>
    </form>

</body>
</html>
