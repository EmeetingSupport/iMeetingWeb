<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Redirect.aspx.cs" Inherits="MeetingMinder.Web.Redirect" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="en" class="no-js">
<head>
    <meta charset="UTF-8">
    <meta name="Generator" content="EditPlus®">
    <meta name="Author" content="">
    <meta name="Keywords" content="">
    <meta name="Description" content="">
   
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/aes.js"></script>


    <title>iMeeting</title>
</head>
<body>
     	<div class="wrapper loginmain">
			<div class="login-left">
				<div class="leftlogo">
			 <img src="images/logo.png"    alt="sbi small logo" /><%--</a>--%>
				 
				</div>
			</div>
			<div class="login-right">
				<h1 class="logo"> <img src="images/logo1.png"alt="sbi Logo" /> </h1>
				<div class="login-main">
					<div class="login-head">
                        <br />
						<h2>Department Selection</h2>
						<%--<p>Login with your username & password.</p>--%>
					</div>

					<form method="post"  runat="server" autocomplete="off" name="formRedirect" action="Redirect.aspx">
                         <%--<%= System.Web.Helpers.AntiForgery.GetHtml() %>--%>
                     <div style="margin-bottom:0px" class="validation">
                      <%--  <asp:ValidationSummary  ID="vsLogin" ValidationGroup="check" runat="server" CssClass="notification error" HeaderText="Error notification" />     --%>                   
                   </div> 

                     <div style="margin-bottom:0px">             	
                         <asp:Label runat="server" style="font-size: 12px;" ID="lblmsg" cssclass="notification error invalid-error" Visible="false" ></asp:Label>
			          </div> 
						<div class="user-info">
							 <br />
							<asp:DropDownList Width="300px" ID="ddlEntity" runat="server" ></asp:DropDownList>
                                <asp:RequiredFieldValidator ValidationGroup="check"  ID="rfvLogin" class="user-error" runat="server"
                ControlToValidate="ddlEntity" ErrorMessage="Select Department" SetFocusOnError="True" InitialValue="0"
                Display="Dynamic">Select Department</asp:RequiredFieldValidator>        
                           
                              <asp:Button ID="btnGo"  Text="Go" runat="server" ValidationGroup="check" 
                                    onclick="btnGo_Click" class="login-btn"  />
                    
							 
						</div>
						<div class="login-bottom">
						 
						 
						</div>
					</form>
				</div>
			 
			</div>
		</div>


   
    
</body>
</html>
