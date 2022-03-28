﻿<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="YearlyMeeting.aspx.cs" Inherits="MeetingMinder.Web.YearlyMeeting" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <article class="content-box minimizer">
			<header>			
				<h2>Meeting Shedule</h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#054a7f"><b>Global Setting</b></font></legend>
                            <dl>
                                   
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                 
                               <div style="margin-bottom:15px"><asp:Label ID="lblTotalRecord" runat="server" Font-Bold="True"></asp:Label>
                               </div>  
                                      <asp:UpdatePanel id="ResultsUpdatePanel" runat="server">   
                                 <ContentTemplate>
                                    <div style="text-align:center;">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel" DynamicLayout="true">
                        <ProgressTemplate>

                           <img src="img/jquery/ajaxLoader.gif">

                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    </div>   
                     <div style="display:none">   
                     <dt>
                                    <label>
                                    Entity <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                   <asp:DropDownList Visible="false" ID="ddlEntity" runat="server" AutoPostBack="true" Width="231px"
                                    onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList> 
                                     <asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Entity" Text="Please Select Entity" ForeColor="Red"></asp:RequiredFieldValidator>  
                                 
                                </dd>    
                              </div>       
                                     <div class="box_top">
		
		
<h2 class="icon users">Gloabal Setting </h2>
		
	
</div>
                              <div style="margin-bottom:15px">
                              
                                 <asp:GridView ID="grdGlobal" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="Id" 
                                 
                                    OnPageIndexChanging="grdGlobal_PageIndexChanging" 
                                    onsorting="grdGlobal_Sorting" onrowcommand="grdGlobal_RowCommand" 
                                    onrowediting="grdGlobal_RowEditing" 
                                      onrowcancelingedit="grdGlobal_RowCancelingEdit" 
                                      onrowupdating="grdGlobal_RowUpdating">                                  
 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                  <Columns>

                              

                                  
                                    <asp:TemplateField HeaderText="Sr. No." HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Key" SortExpression="Key" >
                                        <ItemTemplate>
                                        <asp:Label ID="lblKey" runat="server" Text='<%# Eval("Key")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Value" SortExpression="Value">
                                        <ItemTemplate>
                                        <asp:Label ID="lblVal" runat="server" Text='<%# Eval("Value") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        <asp:TextBox ID="txtVal" runat="server" Text='<%# Eval("Value") %>' />
                                         <asp:RequiredFieldValidator runat="server" ID="rfvNotice" ControlToValidate="txtVal" Display="Dynamic" ValidationGroup="a" ErrorMessage="Value" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>


                                    
                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Eval("Id") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                         <%--   <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Bind("NoticeId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />--%>
                                         </ItemTemplate>
                                         <EditItemTemplate>
                                         <asp:LinkButton ID="lbnUpdate" runat="server" Text="Update"  ValidationGroup="a" CommandName="update" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                         <asp:LinkButton ID="lbnCancel" runat="server" Text="Cancel" CommandName="cancel" ></asp:LinkButton>
                                         &nbsp;&nbsp; 
                                         </EditItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                                 </div>

                                 </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                               

                            
                               <asp:PostBackTrigger ControlID="grdGlobal" />
                             
                                   
    </Triggers>
</asp:UpdatePanel>
                                 </dl>
                                 
                                 </fieldset>
                                 </div>
                                 </section>
                                 </article>
</asp:Content>
