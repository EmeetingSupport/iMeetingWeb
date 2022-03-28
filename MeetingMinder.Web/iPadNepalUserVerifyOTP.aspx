<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="iPadNepalUserVerifyOTP.aspx.cs" Inherits="MeetingMinder.Web.iPadNepalUserVerifyOTP" %>

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
            height: 450px !important;
        }

        /*.login-right {
            padding: 2px 60px !important;
        }*/
        .resend{
            text-decoration:none !important;
            font-weight:bold !important;
            color:#0079C2;
        }
    </style>

    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script type="text/javascript" src="js/aes.js"></script>

    <script type="text/javascript">

        window.onload = function () {

            var myInput = document.getElementById('txtOtp');
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
                        document.getElementById('<%= btnOTP.ClientID %>').click();
                        return false;
                    }
                }
            }
        });

        $('form[name=yourformname]').submit(function () {

        });
    </script>
    <title>Verify OTP</title>
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
        <div class="login-right" style="margin-left: 300px; height: 450px">
            <h1 class="logo">
                <img src="images/logo1.png" alt="sbi Logo" />
            </h1>
            <div class="login-main">
                <form method="post" runat="server" autocomplete="off" action="iPadNepalUserVerifyOTP.aspx">
                    <div class="login-head">
                        <h2>
                            <asp:Label ID="lblLogin" runat="server" Text="OTP"></asp:Label></h2>
                        <p>
                            <asp:Label ID="lblloginMsg" runat="server" Text="Login with your username & password."></asp:Label>
                            <a id="aResendOTP" onserverclick="btnResendOTP_Click" runat="server" class="resend">Resend OTP</a>                            
                        </p>
                        <p>
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </p>
                    </div>

                    <asp:ScriptManager ID="scrManager" runat="server"></asp:ScriptManager>
                    <div style="margin-bottom: 0px" class="validation">
                        <%-- <asp:ValidationSummary  ID="vsLogin" ValidationGroup="check" runat="server" CssClass="notification error" HeaderText="Error notification" />                        --%>
                    </div>

                    <div style="margin-bottom: 0px">
                        <asp:Label runat="server" Style="font-size: 12px;" ID="lblmsg" CssClass="notification error invalid-error" Visible="false"></asp:Label>
                    </div>

                    <div class="user-info">
                        <asp:TextBox ID="txtOtp" runat="server" autocomplete="off" placeholder="OTP" CssClass="user-name"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="otp" ID="rqfOtp" class="user-error" runat="server"
                            ControlToValidate="txtOtp" ErrorMessage="Enter OTP" SetFocusOnError="True"
                            Display="Dynamic">Enter OTP</asp:RequiredFieldValidator>
                        <br />

                        <asp:UpdatePanel ID="updatePanelOTP" runat="server" UpdateMode="Conditional" Visible="false">
                            <ContentTemplate>
                                <cc1:CaptchaControl ID="CaptchaControl2" CssClass="captcha1" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="5"
                                    CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                    FontColor="#D20B0C" NoiseColor="#B1B1B1" />

                                <asp:ImageButton ImageUrl="~/images/refresh_icon.jpg" runat="server" CausesValidation="false" />
                                <asp:TextBox ID="txtCaptcha2" autocomplete="off" MaxLength="8" placeholder="captcha*" class="user-name" runat="server"
                                    CssClass="user-name upper" Style="text-transform: uppercase;"></asp:TextBox>
                                <asp:RequiredFieldValidator Enabled="true" ValidationGroup="otp1" ID="reqfCaptch2" runat="server"
                                    class="invalid-side-note pass-error" ForeColor="Red" ControlToValidate="txtCaptcha2" ErrorMessage="Enter Captcha"
                                    SetFocusOnError="True" Display="Dynamic">Captcha</asp:RequiredFieldValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <br />
                        <asp:Button ID="btnOTP" runat="server" Text="Submit" ValidationGroup="otp" CssClass="login-btn" OnClick="btnOTP_Click" />
                        <asp:Button ID="btnBackTologin" runat="server" Text="Back to login" CssClass="login-btn" OnClick="btnBackTologin_Click" Visible="false" />
                    </div>

                    <div class="login-bottom" style="display: none;">
                        <asp:HiddenField ID="hdnPassword" runat="server" />
                        <asp:HiddenField ID="hdnUserVal" runat="server" />
                        <div id="divForgotPassword" runat="server" class="bottom-right">
                            <a href="ForgotPassword.aspx">Forgot Password?</a>
                        </div>
                    </div>
                </form>
            </div>

        </div>
    </div>

    <script type="text/javascript">        
        var seconds = 120;                
        setInterval(function () {
            seconds--;
            //console.log(seconds);            
            if (seconds == 0) {                
                window.location = "/iPadUserLogin.aspx";
            }
        }, 1000);
    </script>
   
</body>
</html>