<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="MeetingMinder.Web.Error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
  <title>iMeeting:Error</title>
    <style type="text/css">
        .style1
        {
            font-size: xx-large;
            font-weight: bold;
            color: #FF3300;
        }
    </style>
    <script type="text/javascript">
        //<![CDATA[

        window.history.forward();
        function noBack() { window.history.forward(); }

        //]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <span class="style1">Sorry for Inconvenience<br />
        </span>
        <br />
        <asp:Label ID="lblMessage" runat="server" Text="InVaild Page, Please Enter Page Name Properly."
            Font-Bold="True" Font-Size="Large"></asp:Label>
        <br />
        <br />
        <asp:LinkButton ID="lnkPreviousPage" runat="server" OnClick="lnkPreviousPage_Click">Click Here For Login Page.</asp:LinkButton>
    </div>
    </form>
</body>
</html>
