<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MeetingMinder.Web.Default" %>
<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
    <div class="full_width border-bottom">
    <div class="container">  
<h1 class="heading_top"> <img src="img/home.png" /> Dashboard</h1>
</div>
        <div style="margin-bottom:15px"><userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
</div>
<div class="full_width">
    <fieldset>  
    <div class="dashboard_left_panel">

    <div class="active_user">
        <img src="img/user_icon.png" class="user_icon" />
        <div class="text">
        <h3><%= userCount %></h3>
        <h5>Active Users of SBI</h5>
        </div>
      </div>

      <div class="active_user left">
        <img src="img/group.png" class="user_icon" />
        <div class="text">
        <h3><%= directorCount %></h3>
        <h5>Board of Directors</h5>
        </div>
      </div>


      <!-- <ul>
      	<li>
      		<a href="#">
      		<div class="icon">
      			<img src="img/reports_icon_color.png" class="visible_icon">
      			<span>Reports</span>
      			</div>
      		</a>
      	</li>

      	<li>
      		<a href="#">
      		<div class="icon">
      			<img src="img/reports_icon_color.png" class="visible_icon">
      			<span>Reports</span>
      			</div>
      		</a>
      	</li>

      	<li>
      		<a href="#">
      		<div class="icon">
      			<img src="img/reports_icon_color.png" class="visible_icon">
      			<span>Reports</span>
      			</div>
      		</a>
      	</li>

      	<li>
      		<a href="#">
      		<div class="icon">
      			<img src="img/reports_icon_color.png" class="visible_icon">
      			<span>Reports</span>
      			</div>
      		</a>
      	</li>

      	<li>
      		<a href="#">
      		<div class="icon">
      			<img src="img/reports_icon_color.png" class="visible_icon">
      			<span>Reports</span>
      			</div>
      		</a>
      	</li>

      </ul> -->

    </div>
    <div class="dashboard_right_panel">
    <article class="welcome_text">
    <h2 class="welcome_heading">SBI</h2>
    <p class="welcome_text">State Bank of India welcomes you to explore the world of premier bank in India.</p>
    
<p class="welcome_text">In this section, you can access detailed information on Overview of the Bank, Technology Upgradation in the Bank, Board of Directors, Financial Results and Shareholder Info.</p>

<p class="welcome_text">The Bank is actively involved since 1973 in non-profit activity called Community Services Banking. All our branches and administrative offices throughout the country sponsor and participate in large number of welfare activities and social causes. Our business is more than banking because we touch the lives of people anywhere in many ways.</p>
    </article>
    </div>
</fieldset>
</div>
        <!-- Simple Sorting Table + Pagination: Start -->
        <%--		<div class="dataTables_wrapper" style="height:300px;">--%>
        <!--<div class="dataTables_length">Show
		<div class="selector">
		<span>10</span>
		<select size="1" style="opacity: 0;">
		<option value="10">10</option>
		<option value="25">25</option>
		<option value="50">50</option>
		<option value="100">100</option>
		</select></div>
		entries</div>
		<div class="dataTables_filter">Search: <input type="text"></div>-->

        <!-- <div class="dataTables_info">Showing 1 to 10 of 57 entries</div>
				<div class="dataTables_paginate paging_full_numbers">
				<span class="first paginate_button paginate_button_disabled">First</span>
				<span class="previous paginate_button paginate_button_disabled">Previous</span>
				<span><span class="paginate_active">1</span>
				<span class="paginate_button">2</span>
				<span class="paginate_button">3</span>
				<span class="paginate_button">4</span>
				<span class="paginate_button">5</span>
				</span><span class="next paginate_button">Next</span>
				<span class="last paginate_button">Last</span>
				</div> -->
        <%--	</div>--%>
        <!-- Sorting Table: End -->
    
</asp:Content>
