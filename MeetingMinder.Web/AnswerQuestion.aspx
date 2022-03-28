<%@ Page Title="Change Password" Language="C#"  AutoEventWireup="true" CodeBehind="AnswerQuestion.aspx.cs" Inherits="MeetingMinder.Web.AnswerQuestion" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="en" class="no-js">
  <head>
  <meta charset="UTF-8">
  <meta name="Generator" content="EditPlus®">
  <meta name="Author" content="">
  <meta name="Keywords" content="">
  <meta name="Description" content="">
  <link href="css/style.css" rel="stylesheet" type="text/css" />
      <!--<link href="Simplpan%20-%20Admin%20Panel_files/style.css" rel="stylesheet" />-->
<title>Change Password</title>
 </head>
<body class="login">
<style type="text/css">
.txtBox
{
    background:  no-repeat scroll 260px center rgba(0, 0, 0, 0);
}
</style>

     <script type="text/javascript">
         //<![CDATA[		

         if (self == top) {
             var theBody = document.getElementsByTagName('body')[0]
             theBody.style.display = "block"
         } else { top.location = self.location }

         //]]>		

         
        document.onkeypress = function keypressed(e) {
            if (e.keyCode == 13) {
                if (Page_ClientValidate("a")) {
                      document.getElementById('<%= btnSubmit.ClientID %>').click();
                }
            }
        }

	    </script>
<form runat="server"  AUTOCOMPLETE="OFF" name="formAnswerQuestion" action="AnswerQuestion.aspx" method="post">
    <%--<%= System.Web.Helpers.AntiForgery.GetHtml() %>--%>
<asp:Panel ID="aspPanel" runat="server" >
 
<div class="wrapper" >
	<div class="login_section login-left">
	<div class="leftlogo login-left-section" >
					 <img alt="yes bank small logo" src="images/logo.png"/> 
				<%--	<div class="left-info">
						<span class="info-top">say yes to</span>
						<span class="info-bottom">growth</span>
					</div>--%>
				</div>
   <div class="login_screen login-right">
					<div class="logo">
						<%--<a href="Login.aspx">--%>
							<img src="images/logo1.png" border="0" alt="logo" />
						<%--</a>--%>
					</div>
					<%--<div class="middle_bg"><img src="images/middle_bg.png" border="0" alt="" /></div>--%>

<div class="form-wrapper">
<div class="form-entry">

	   <h3>CHANGE PASSWORD</h3>
						<%--<fieldset>--%>
						<%--	<legend><font color="#AE432E"><b>Security Question</b></font></legend>--%>
                             <dl>
                                <div style="margin-bottom:15px"><userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                <div style="margin-bottom:15px">
                                <%--    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />--%>
                                </div>
                                <div>
                                 <dd style="margin-bottom:10px"><span class="lbltopquest">Question
                                     :</span>
                                     <asp:Label ID="lblQuest" Visible="false" runat="server"></asp:Label>
                                     <asp:DropDownList ID="ddlSecurityQuestion" Width="73%" runat="server"></asp:DropDownList></dd>
                                  <asp:RequiredFieldValidator runat="server" ID="rfvQuestion" ControlToValidate="ddlSecurityQuestion" Display="Dynamic"
                                       ValidationGroup="a" ErrorMessage="Security Question" InitialValue="0" Text="Security Question" CssClass="ErrorMsg"></asp:RequiredFieldValidator>
                                    
                                     <asp:TextBox ID="txtSecurityAnswer" runat="server" TextMode="Password" CssClass="txtBox" placeholder="Answer" autocomplete="off"></asp:TextBox>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvNew" ControlToValidate="txtSecurityAnswer" 
                                         Display="Dynamic" ValidationGroup="a" ErrorMessage="Answer" CssClass="ErrorMsg">Answer</asp:RequiredFieldValidator>
                                    
                                      <div class="fullwidth noBorder">
                                        <asp:Button ID="btnSubmit" CssClass="btnSave" CausesValidation="true" ValidationGroup="a" runat="server" Text="Submit" onclick="btnSubmit_Click"></asp:Button> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" CssClass="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click"></asp:Button>
                                                </div>
                               </div>
                               
						<%--</fieldset>--%>
                             										
						</div></div>
				</div>					
		</div>
    <div class="clearfix">
    </div>
    </div>
    </asp:Panel>
    </form>
    </body>
    </html>