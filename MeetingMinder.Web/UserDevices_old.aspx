<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="UserDevices_old.aspx.cs" Inherits="MeetingMinder.Web.UserDevices_old" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--View Report--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>View User Devices</b></font></legend>
                             <dl>
                             <div style="width:900px;">
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
                           
                              <br /><br />
                                <div style="width:900px;">
                                 <table >

                                  
                                     <tr>
                                     <td style="width:95px;">
                                    User
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td    align="left">
                                     <asp:DropDownList ID="ddlUser" runat="server"  Width="50%" 
                                          onselectedindexchanged="ddlUser_SelectedIndexChanged" AutoPostBack="true"   ></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlUser" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Forum" Text="Please Select User" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>
                                      

                                     <tr id="trUsers" runat="server" visible="false">
                                     <td>Device list</td>
                                       <td width="20px">
                                     :
                                     </td>
                                     <td   align="left"> 
                                          <div class="box_top">
		
		
<h2 class="icon users">User Device List </h2>
		
	
</div>
                                     <asp:GridView ID="grdDevices"  ShowHeader="true" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" DataKeyNames ="UdId"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red"  EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" OnRowCommand="grdDevices_RowCommand">
                                <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                          <PagerStyle CssClass="paginate_active"   />
                                  <Columns>
                                  
                                      <asp:TemplateField HeaderText="Device Register Id">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUdid" runat="server" Text='<%# Eval("UdId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField  HeaderText="MAC Address" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblMac" runat="server" Text='<%# Eval("MAC") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                           <asp:TemplateField  HeaderText="Last Login On" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblLastLoginOn" runat="server" Text='<%# Convert.ToDateTime(Eval("Last Login On")).ToString("D")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>         
                                      
                                       <asp:TemplateField  HeaderText="Lost?" >
                                      <ItemTemplate>
                                          <asp:LinkButton ID="lnkLost" runat="server" CommandName="Lost" Text='<%# ((DataBinder.Eval(Container.DataItem,"CreatedOn")) == DBNull.Value) ? ("Blacklist") : ("Whitelist" )%>' CommandArgument='<%# Eval("UdId") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                      
                                           <asp:TemplateField  HeaderText="Blacklisted On" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblBlacklisted" runat="server" Text='<%#((DataBinder.Eval(Container.DataItem,"CreatedOn")) == DBNull.Value) ? ("") : ( Convert.ToDateTime(Eval("CreatedOn")).ToString("D") )  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>  
                                  
                                       <asp:TemplateField  HeaderText="Data Deleted?" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblBlacklistedOn" runat="server" Text='<%# ((DataBinder.Eval(Container.DataItem,"DataWipeOut")) == DBNull.Value) ? ("No") : "Yes"  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>  
                                      
                                       <asp:TemplateField  HeaderText="Data Deleted On" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblDeletedOn" runat="server" Text='<%# ((DataBinder.Eval(Container.DataItem,"UpdatedOn")) == DBNull.Value) ? ("") : ( Convert.ToDateTime(Eval("UpdatedOn")).ToString("D"))   %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                                                 
                                 </Columns>
                                </asp:GridView>
                                     </td>
                                     </tr>
                                                             
                                 </table>
                                 </div>
  <%--                                    <div>
    <span> 
      <b> Last login on: Last online login time in device</b>
    </span>
  </div>--%>
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlUser" />
                            
                         <asp:PostBackTrigger ControlID="grdDevices" />
        
    </Triggers>
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
