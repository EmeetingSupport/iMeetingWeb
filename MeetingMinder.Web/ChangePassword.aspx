<%@ Page Title="Change Password" Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="MeetingMinder.Web.ChangePasswordaspx" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="en" class="no-js">
<head>
    <title>Change Password</title>
    <meta http-equiv="x-ua-compatible" content="IE=8" />
    <meta charset="UTF-8">
    <meta name="Generator" content="EditPlus®">
    <meta name="Author" content="">
    <meta name="Keywords" content="">
    <meta name="Description" content="">
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/aes.js"></script>

    <!--<link href="Simplpan%20-%20Admin%20Panel_files/style.css" rel="stylesheet" />-->

    <style type="text/css">
        .txtBox {
            background: no-repeat scroll 260px center rgba(0, 0, 0, 0);
        }
    </style>
    <script type="text/javascript">

        function checkLogin() {

            if (Page_ClientValidate("a")) {
                var hdnUserVal = document.getElementById('<%=hdnUserVal.ClientID%>').value;

                var value = document.getElementById('<%=txtOld.ClientID%>').value;
                var newval = document.getElementById('<%=txtNew.ClientID%>').value;
                               
                //var hash =  hex_sha256(value);
                
                var key = CryptoJS.enc.Utf8.parse(hdnUserVal);
                var iv = CryptoJS.enc.Utf8.parse(hdnUserVal);

                var encryptedpassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(value), key,
                  {
                      keySize: 128 / 8,
                      iv: iv,
                      mode: CryptoJS.mode.CBC,
                      padding: CryptoJS.pad.Pkcs7
                  });

                var encryptednewpassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(newval), key,
                 {
                     keySize: 128 / 8,
                     iv: iv,
                     mode: CryptoJS.mode.CBC,
                     padding: CryptoJS.pad.Pkcs7
                 });

                //alert(encryptedpassword + " new " + encryptednewpassword);

                document.getElementById('<%=hdnOld.ClientID %>').value = encryptedpassword;
                document.getElementById('<%=hdnNew.ClientID %>').value = encryptednewpassword;

                document.getElementById('<%=txtOld.ClientID %>').value = "";
                document.getElementById('<%=txtNew.ClientID %>').value = "";
                document.getElementById('<%=txtConfirm.ClientID %>').value = "";

                var validatorObject = document.getElementById('<%=rfvOld.ClientID%>');
                validatorObject.enabled = false;

                var validatorObject1 = document.getElementById('<%=rfvNew.ClientID%>');
                validatorObject1.enabled = false;

                var validatorObject2 = document.getElementById('<%=rfvConfirm.ClientID%>');
                validatorObject2.enabled = false;

                return true;
            }
            else {
                return false;
            }
        }


        document.onkeypress = function keypressed(e) {
            if (e.keyCode == 13) {
                if (Page_ClientValidate("a")) {

                    document.getElementById('<%= btnSearch.ClientID %>').click();
                    return false;
                }
            }
        }


        //<![CDATA[

        if (self == top) {
            var theBody = document.getElementsByTagName('body')[0]
            theBody.style.display = "block"
        } else { top.location = self.location }

        //]]>


    </script>

</head>
<body class="login">
    <form id="Form1" autocomplete="off" runat="server" name="formChangePassword" action="ChangePassword.aspx" method="post">
        <asp:Panel ID="aspPanel" runat="server">


            <div class="wrapper">
                <div class="login_section login-left">
                    <div class="leftlogo login-left-section">
                        <img alt="yes bank small logo" src="images/logo.png" />
                        <%--	<div class="left-info">
						<span class="info-top">say yes to</span>
						<span class="info-bottom">growth</span>
					</div>--%>
                    </div>
                    <div class="login_screen login-right">
                        <div class="logo">
                            <%--<a href="Login.aspx">--%>
                            <img src="images/logo1.png" border="0" alt="logo" />
                            <%--	</a>--%>
                        </div>
                        <%--	<div class="middle_bg"><img src="images/middle_bg.png" border="0" alt="" /></div>--%>

                        <div class="form-wrapper">
                            <div class="form-entry">


                                <h3>Change Password</h3>


                                <div>


                                    <div style="margin-bottom: 15px">
                                        <userControl:Info ID="Info" runat="server" Visible="false" />
                                        <userControl:Error ID="Error" runat="server" Visible="false" />
                                    </div>
                                    <div style="margin-bottom: 15px">
                                        <%--    <asp:ValidationSummary ID="ValidationSummary" runat="server"
                                            CssClass="notification error"
                                            HeaderText="<p>You must enter a valid value in the following fields:</p>"
                                            ValidationGroup="a" />--%>
                                    </div>
                                    <dl>
                                        <div>

                                            <dd>Old Password :
                                    
                                     <asp:TextBox ID="txtOld" CssClass="txtBox" TextMode="Password" runat="server" autocomplete="off"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvOld" CssClass="errorpassword" ControlToValidate="txtOld" Display="Dynamic" ValidationGroup="a" ErrorMessage="Enter New Password">Enter Password</asp:RequiredFieldValidator>
                                                <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ControlToValidate="txtNew" Display="Dynamic" ValidationGroup="a" ErrorMessage="Password must contain: Minimum 8 and Maximum 15 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character"
                                            SetFocusOnError="True"  
                                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$"></asp:RegularExpressionValidator>--%>
                                            </dd>

                                            <dd>New Password :
                                    
                                     <asp:TextBox ID="txtNew" CssClass="txtBox" TextMode="Password" runat="server" autocomplete="off"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvNew" CssClass="errorpassword" ControlToValidate="txtNew" Display="Dynamic" ValidationGroup="a" ErrorMessage="Enter New Password">Enter Password</asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="rgePassword" runat="server" CssClass="ErrorMsg"
                                                    ControlToValidate="txtNew" Display="Dynamic" ValidationGroup="a" ErrorMessage="Password must contain: Minimum 8 and Maximum 15 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character"
                                                    SetFocusOnError="True"
                                                    ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$"></asp:RegularExpressionValidator>
                                            </dd>


                                            <dd>Confirm Password :
                                   
                                     <asp:TextBox ID="txtConfirm" CssClass="txtBox" TextMode="Password" runat="server" autocomplete="off"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvConfirm" ValidationGroup="a" Display="Dynamic" ControlToValidate="txtConfirm" ErrorMessage=" Enter Confirm Password"
                                                    CssClass="ErrorMsg">Enter Confirm Password</asp:RequiredFieldValidator>

                                                <asp:CompareValidator ID="cfvConfirm" runat="server" ControlToCompare="txtNew" ControlToValidate="txtConfirm" ErrorMessage="Confirm Password"
                                                    Text="Password not match" CssClass="ErrorMsg" ValidationGroup="a" Display="Dynamic"></asp:CompareValidator>

                                                <asp:RegularExpressionValidator ID="rexConf" runat="server" CssClass="ErrorMsg"
                                                    ControlToValidate="txtConfirm" Display="Dynamic" ValidationGroup="a" ErrorMessage="Password must contain: Minimum 8 and Maximum 15 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character"
                                                    SetFocusOnError="True"
                                                    ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$"></asp:RegularExpressionValidator>
                                            </dd>
                                            <div class="fullwidth noBorder">

                                                <asp:Button ID="btnSearch" CssClass="btnSave" CausesValidation="true" ValidationGroup="a" runat="server" OnClientClick="checkLogin()" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button CssClass="btnCancel" ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
                                            </div>
                                        </div>
                                </div>



                                <div class="clearfix"></div>
                                <asp:HiddenField ID="hdnUserVal" runat="server" />
                                <asp:HiddenField ID="hdnOld" runat="server" />
                                <asp:HiddenField ID="hdnNew" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </form>
</body>
</html>
