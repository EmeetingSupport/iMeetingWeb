<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="MeetingMinder.Web.ResetPassword" %>

<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Password</title>
    <meta http-equiv="x-ua-compatible" content="IE=8" />
    <meta charset="UTF-8">
    <meta name="Generator" content="EditPlus®">
    <meta name="Author" content="">
    <meta name="Keywords" content="">
    <meta name="Description" content="">

    <style type="text/css">
        .CustError {
            font-size: 11px !important;
        }

        .captcha {
            float: left;
            margin-right: 5px;
        }

        .errorclass {
            font-size: 18px;
            color: darkred;
            padding: 18px;
        }

        .success h4 {
            background-color: #16ad22;
            color: #fff;
            padding: 3px 8px;
            margin: 0;
            font-size: 14px;
        }
    </style>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="SimplpanAdminPanelfiles/style.css" rel="stylesheet" />

    <script type="text/javascript">

             document.onkeypress = function keypressed(e) {
            if (e.keyCode == 13) {
                if (Page_ClientValidate("check")) {
                      document.getElementById('<%= btnSubmit.ClientID %>').click();
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
<body>
    <form id="form1" runat="server" autocomplete="off" name="formResetPassword" action="ResetPassword.aspx">
       <%-- <%= System.Web.Helpers.AntiForgery.GetHtml() %>--%>
        <div>
            <asp:Panel ID="aspPanel" runat="server" DefaultButton="btnSubmit">
                <div class="wrapper">
                    <div class="login_section login-left">
                        <div class="leftlogo login-left-section">
                            <img alt="sbi bank small logo" src="images/logo.png">
                        </div>
                        <div class="login_screen login-right">
                            <div class="logo">

                                <img src="images/logo1.png" border="0" alt="logo" />

                            </div>
                            <br />
                            <div class="form-wrapper">
                                <div class="form-entry" style="margin-top: 0px;">
                                    <h3>Reset Password</h3>


                                    <div>
                                        <asp:Label ID="lblError" runat="server" CssClass="errorclass"></asp:Label>
                                    </div>
                                    <userControl:Info ID="Info" runat="server" Visible="false" />
                                    <br />

                                    <dl>
                                        <div id="divName" runat="server">
                                            <div id="LoginDetails" runat="server">
                                                <dt>New Password</dt>
                                                <dd>
                                                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="txtBox" placeholder="Password*"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="rgePassword" runat="server" class="invalid-side-note CustError"
                                                        ControlToValidate="txtPassword" ValidationGroup="check" Display="Dynamic" ErrorMessage="Password"
                                                        SetFocusOnError="True"
                                                        ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$">Password must contain: Minimum 8 and Maximum 15 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character</asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ValidationGroup="check" ID="rfvPassword" runat="server" CssClass="ErrorMsg"
                                                        ControlToValidate="txtPassword" ErrorMessage="Enter Password" SetFocusOnError="True" Display="Dynamic">Enter Password</asp:RequiredFieldValidator>
                                                </dd>

                                                <dt>Retype Password </dt>
                                                <dd>
                                                    <asp:TextBox ID="txtConfirmPassword"  TextMode="Password" runat="server" CssClass="txtBox" placeholder="ConfirmPassword*"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" class="invalid-side-note CustError"
                                                        ControlToValidate="txtConfirmPassword" ValidationGroup="check" Display="Dynamic" ErrorMessage="Password"
                                                        SetFocusOnError="True"
                                                        ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$">Password must contain: Minimum 8 and Maximum 15 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character</asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ValidationGroup="check" ID="rfvConfirmPassword" runat="server" CssClass="ErrorMsg"
                                                        ControlToValidate="txtConfirmPassword" ErrorMessage="Enter Confirm Password" SetFocusOnError="True" Display="Dynamic">Enter Confirm Password</asp:RequiredFieldValidator>
                                                </dd>
                                                <dd>
                                                    <asp:CompareValidator ID="comparePasswords"
                                                        runat="server"
                                                        ControlToCompare="txtPassword"
                                                        ControlToValidate="txtConfirmPassword"
                                                        ErrorMessage="Your passwords do not match up!"
                                                        Display="Dynamic" ValidationGroup="check" CssClass="ErrorMsg" />
                                                </dd>
                                            </div>
                                           
                                            <div class="fullwidth noBorder">
                                                <asp:Button ID="btnSubmit" CssClass="btnSave" Text="Save" runat="server" ValidationGroup="check" OnClick="btnSubmit_Click" />
                                                &nbsp;&nbsp; 
                                             <asp:Button ID="btnCancel" CssClass="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click" />
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
        </div>
    </form>
</body>
</html>
