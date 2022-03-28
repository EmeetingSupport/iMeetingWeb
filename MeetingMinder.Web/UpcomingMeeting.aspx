<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="UpcomingMeeting.aspx.cs" Inherits="MeetingMinder.Web.UpcomingMeeting" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--View Meeting--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>Upcoming Meeting</b></font></legend>
                             <dl>
                             <div>
                                  <asp:UpdatePanel id="ResultsUpdatePanel" runat="server">   
                                 <ContentTemplate>

                                    <div style="text-align:center;">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel" DynamicLayout="true">
                        <ProgressTemplate>

                           <img src="img/jquery/ajaxLoader.gif">

                        </ProgressTemplate>
                    </asp:UpdateProgress>
                               
                             </div>                 

                                <div style="margin-bottom:15px"><userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                <div style="margin-bottom:15px">
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                </div>
                           
                              <br />
                                <div >
                                 
                                     <div class="box_top">
		
		
<h2 class="icon users">Meeting Details</h2>
		
	
</div>
                                     <div class="box_content">
		
		<!-- Simple Sorting Table + Pagination: Start -->
		<div class="dataTables_wrapper">

                                       <asp:GridView ID="grdMeeting" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="MeetingId" OnPageIndexChanging="grdMeeting_PageIndexChanging" OnSorting="grdMeeting_Sorting">
                                <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                            <PagerStyle CssClass="paginate_active"   />
                                  <Columns>
                                                                    
                                  
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Department Name" SortExpression="EntityId">
                                          <ItemTemplate>
                                              <asp:Label ID="lblDepartment" runat="server" Text='<%# Eval("EntityName") %>'></asp:Label>
                                          </ItemTemplate>
                                      </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Forum Name">
                                          <ItemTemplate>
                                              <asp:Label ID="lblForum" runat="server" Text='<%# Eval("ForumName") %>'></asp:Label>
                                          </ItemTemplate>
                                      </asp:TemplateField>
                                      
                                    <asp:TemplateField HeaderText="Meeting Details" SortExpression="MeetingDate">
                                        <ItemTemplate>
                                            <asp:Label style="text-align:justify" Width="300px" ID="lblMeetingVenu" runat="server" Text='<%# Eval("MeetingNumber") + " " + Eval("MeetingVenue") + ", "+ Convert.ToDateTime(Eval("MeetingDate")).ToString("D") + " " + Eval("MeetingTime") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   
                               
                                 </Columns>
                                </asp:GridView>
                             </div>
                             <%--    <div style="margin-bottom:15px">--%>
                          </div>
                                 </div>
                                             </ContentTemplate>
                 
</asp:UpdatePanel>
                               </div>
                               <br />
						</fieldset>
                             										
					
				</div>					
				
			</section>			
		</article>
    <div class="clearfix">
    </div>
</asp:Content>
