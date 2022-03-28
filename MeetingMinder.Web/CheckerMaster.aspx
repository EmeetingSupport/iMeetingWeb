﻿<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="CheckerMaster.aspx.cs" Inherits="MeetingMinder.Web.CheckerMaster" %>
<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
  <script language="javascript" type="text/javascript">

      function fn_select_all(chkSelectAll) {
          var IsChecked = chkSelectAll.checked;
          var items = document.getElementsByTagName('input');
          for (i = 0; i < items.length; i++) {
              if (items[i] != chkSelectAll && items[i].type == "checkbox") {
                  if (items[i].checked != IsChecked) {
                      items[i].click();
                  }
              }
          }
      }

      function testCheckbox() {
          Parent = document.getElementById('cpMain_grdOrder');
          var headchk = document.getElementById('cpMain_grdOrder_chkHeader');
          var items = Parent.getElementsByTagName('input');
          var flg = false;
          for (i = 0; i < items.length; i++) {

              if (items[i] != headchk && items[i].type == "checkbox") {

                  if (items[i].checked) {
                      flg = true;
                  }
              }
          }
          if (flg) {
              var ans = confirm("Are you sure you want to delete selected items?");
              if (ans == true) {
                  return true;
              }
              else {
                  //alert("no");
                  return false;
              }
          }
          else {
              alert("Select item(s) to delete");
              return false;
          }
      }
    </script>
    <article class="content-box minimizer">
			<header>			
				<h2>Pending Details</h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#AE432E"><b>Pending meeting list</b></font></legend>
                            <dl>
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                 
                               <div style="margin-bottom:15px">
                                    <%--<img width="16" height="16" src="img/icons/icon_list_style_cross.png" alt="" style="vertical-align: middle"/> --%>
                                  
                               </div>
                               <div style="margin-bottom:15px">
                               <asp:Label ID="lblTotalRecord" runat="server" Font-Bold="True"></asp:Label></div>  
                             <%--  <div style="margin-bottom:15px"> <asp:Label  ID="lblUser" runat="server" Text="Search User" Font-Bold="true" ></asp:Label> </div>  --%>
                                                      
                              <div style="margin-bottom:15px">
                                           <asp:GridView ID="grdMeeting" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="MeetingId" 
                                    OnPageIndexChanging="grdMeeting_PageIndexChanging" 
                                    onsorting="grdMeeting_Sorting" onrowcommand="grdMeetingRowCommand" 
                                    >
                                  <HeaderStyle HorizontalAlign="Center" BackColor="#AE432E" ForeColor="#F2DEDA" Font-Bold="true" />
                                <PagerStyle CssClass="paginate_active"   />
                                                  <Columns>

                                 

                                  
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Entity" SortExpression="EntityName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEntity" runat="server" Text='<%# Eval("EntityName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Forum" SortExpression="ForumName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblForum" runat="server" Text='<%#Eval("ForumName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Meeting Venue" SortExpression="MeetingVenue">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeetingVenu" runat="server" Text='<%#  Eval("MeetingVenue") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Meeting Date" SortExpression="MeetingDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeetingDate" runat="server" Text='<%#  Eval("MeetingDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Meeting Time" SortExpression="MeetingTime">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeetingTime" runat="server" Text='<%#  Eval("MeetingTime") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Agenda">
                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemTemplate>
                                    <asp:LinkButton ID="lbnState" CommandArgument='<%# Bind("MeetingId") %>' CausesValidation="false" CommandName="View" Text="Add/View" runat="server" ToolTip="View Agenda"></asp:LinkButton>
                    
                                    </ItemTemplate>
                                </asp:TemplateField>      
                                
                                  <%--<asp:TemplateField HeaderText="Decline All">
                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnDec" CommandArgument='<%# Bind("MeetingId") %>' CausesValidation="false" CommandName="View" Text="Add/View" runat="server" ToolTip="Decline all"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>                                 
                                 </Columns>
                                </asp:GridView>
                            </div>
                            <div>
                             <asp:GridView ID="grdDocument" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="MeetingId"                                
                                    onsorting="grdDocument_Sorting" onrowcommand="grdDocumentRowCommand">
                                  <HeaderStyle HorizontalAlign="Center" BackColor="#AE432E" ForeColor="#F2DEDA" Font-Bold="true" />
                                  <PagerStyle CssClass="paginate_active"   /> 
                                 <Columns>
                                     <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Document Name" SortExpression="MeetingVenue">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDoc" runat="server" Text='<%# Eval("UploadedAgendaNote") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Approve" >
                                        <ItemTemplate>
                                           <asp:LinkButton ID="lbnApprove" CommandArgument='<%# Bind("AgendaId") %>' CausesValidation="false" CommandName="Approve" Text="Approve" runat="server" ToolTip="Approve"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Decline">
                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemTemplate>
                                    <asp:LinkButton ID="lbnReject" CommandArgument='<%# Bind("AgendaId") %>' CausesValidation="false" CommandName="Decline" Text="Decline" runat="server" ToolTip="Decline"></asp:LinkButton>
                    
                                    </ItemTemplate>
                                </asp:TemplateField>                                 
                                 </Columns>
                                </asp:GridView>
                            </div>
                            </dl>
                            </fieldset>
           
                     <div ID="divdetails" runat="server" style="margin-bottom:15px">
                                        
                     </div>
                                </div>
                                </section>
                                </article>
</asp:Content>
