<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailTest.aspx.cs" Inherits="MeetingMinder.Web.EmailTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="Label1" runat="server"></asp:Label>
     <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red" Text=""></asp:Label></div>
    <br /><br /><br />
         <div>
        From: <asp:TextBox ID="txtFrom" runat="server" Text=""></asp:TextBox>
    </div><br />
    <div>
        To: <asp:TextBox ID="txtToMail" runat="server" Text=""></asp:TextBox>
    </div><br />
         <div>
        Subject: <asp:TextBox ID="txtSubject" runat="server" Text=""></asp:TextBox>
    </div><br />
         <div>
        Body : <asp:TextBox ID="txtBody" runat="server" Text="" TextMode="MultiLine"></asp:TextBox>
    </div>

    <br />
             <div>
        Domain : <asp:TextBox ID="txtDomain" runat="server" Text="" ></asp:TextBox>
    </div><br />
             <div>
        Port : <asp:TextBox ID="txtPort" runat="server" Text="" ></asp:TextBox>
    </div><br />
        <div>
        Enable ssl : <asp:CheckBox ID="chkssl" runat="server" /> 
    </div><br />
    <div>
        <asp:Button ID="btnsend" runat="server" Text="SendMail" OnClick="btnsend_Click" />
        <asp:Button ID="btn2" runat="server" Text="send2" OnClick="btn2_Click" /> 
<asp:Button ID="btns" runat="server" Text="Send3 " OnClick="btns_Click" />

      <%--<b>2 sec Method </b>   System.Web.Mail.MailMessage Msg = new System.Web.Mail.MailMessage();
                // Sender e-mail address.
                Msg.From = txtFrom.Text;
                // Recipient e-mail address.
                Msg.To = txtToMail.Text;
                Msg.Subject = txtSubject.Text;
                Msg.Body = txtBody.Text;
                // your remote SMTP server IP.
                SmtpMail.SmtpServer = txtDomain.Text;
                SmtpMail.Send(Msg);--%>
    </div>
    </form>

</body>
</html>
