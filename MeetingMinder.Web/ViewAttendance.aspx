<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ViewAttendance.aspx.cs" Inherits="MeetingMinder.Web.ViewAttendance" %>

<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <article class="content-box minimizer">
			<header>			
				<h2>Attendance Details</h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#AE432E"><b>Attendance list</b></font></legend>
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
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                 
                                    <div style="width:900px;">
                                 <table width="75%">

                                    <tr style="display:none;">
                                     <td style="width:95px;">
                                    Department
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlEntity" runat="server" Visible="false" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList>
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Department" Text="Please Select Department" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                     
                                 
                                     </td>
                                     </tr>
                                     <tr>
                                     <td style="width:95px;">
                                    Forum
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlForum" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlForum_SelectedIndexChanged"></asp:DropDownList>
                                   <%--  <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Forum" Text="Please Select Forum" ForeColor="Red"></asp:RequiredFieldValidator>
                                --%>     
                                 
                                     </td>
                                     </tr>
                                     <tr>
                                     <td style="width:95px;">
                                    Meeting 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                         <asp:DropDownList ID="ddlMeeting" Width="50%" AutoPostBack="true" runat="server" 
                                    onselectedindexchanged="ddlMeeting_SelectedIndexChanged"></asp:DropDownList> 
                                                   </td>
                                     </tr>
                                     
                                 </table>
                                 </div>         
                              <div style="margin-bottom:15px">
                               <div> </div> 
                                    <br />
                                     <div class="box_top">
		
		
<h2 class="icon users">Meeting </h2>
		
	
</div>
                                     <div class="box_content">
		
		<!-- Simple Sorting Table + Pagination: Start -->
		<div class="dataTables_wrapper">
                                         <asp:GridView ID="grdAttendance" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="false"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable"  DataKeyNames="AttendanceId" 
                                 
                                   >
                                  <HeaderStyle HorizontalAlign="Center" BackColor="#AE432E" ForeColor="#F2DEDA" Font-Bold="true" />
                                  <Columns>

                                  

                                  
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Meeting">
                                        <ItemTemplate>
                                              <asp:Label style="text-align:justify" Width="200px"  ID="lblTe" runat="server" Text='<%#Eval("MeetingDate" ) +" "+ Eval("MeetingTime") +" "+ Eval("MeetingVenue") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                
                                    <asp:TemplateField HeaderText="Name">
                                  
                                        <ItemTemplate>
                                      
                                            <asp:Label ID="lblUserName" style="text-align:justify" Width="100px" runat="server" Text='<%#Eval("Suffix") +" "+Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

              <asp:TemplateField HeaderText="Attendance" >
                                  
                                        <ItemTemplate>
                                      
                                            <asp:Label ID="lblAttendance" style="text-align:justify" Width="100px" runat="server" Text='<%# Eval("Attending")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

              <asp:TemplateField HeaderText="Reason">                                  
                                        <ItemTemplate>                                      
                                            <asp:Label ID="lblReason" style="text-align:justify" Width="100px" runat="server" Text='<%# Eval("Reason")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                          
                                 </Columns>
                                </asp:GridView>
            </div>
                                         </div>
  <div id="divbutton" class="fullwidth noBorder" runat="server" >
                                    <asp:Button CssClass="btnSave" ID="btnExportToExcel" runat="server" Text="Export To Excel" Width="130px" Visible="false" ValidationGroup="a" onclick="btnExportToExcel_Click"  />
                                       
                                    </div>
                            </div>
                                                                         </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlMeeting" />
                                        <asp:PostBackTrigger ControlID="btnExportToExcel" />
                           
                                   
    </Triggers>
</asp:UpdatePanel>
                               </div>
                            </dl>
                            </fieldset>
                           </div>
                                </section>
                                </article>


</asp:Content>
