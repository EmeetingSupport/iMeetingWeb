<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Error.ascx.cs" Inherits="MeetingMinder.Web.UserControl.Error" %>


						<div class="notification error">
							<%--<a href="#" class="close-notification tooltip" title="Hide Notification">x</a>--%>
							<h4>Error notification</h4>
                            <div><asp:Label ID="lblError" runat="server"></asp:Label> </div>													
						</div>