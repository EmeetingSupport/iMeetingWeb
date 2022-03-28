<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="SerialNumberMaster.aspx.cs" Inherits="MeetingMinder.Web.SerialNumberMaster" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
   
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Master Agenda--%></h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#054a7f"><b>Serial Number</b></font></legend>
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
                 
                                                   <div id="ContentPlaceHolder1_divdetails"  style="margin-bottom:15px">
                                      
                                         </div>
                                     <div class="box_top">
		
		
<h2 class="icon users">Serial Number</h2>
		
	
</div>
                       
                                <div style="margin-bottom:15px">
                               <div class="box_content">
		
		<!-- Simple Sorting Table + Pagination: Start -->
		<div class="dataTables_wrapper">
                                 <asp:GridView ID="grdSerialNumber" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="SerialNumberId" OnRowDeleting="grdSerialNumber_RowDeleting"
                                 
                                    OnPageIndexChanging="grdSerialNumber_PageIndexChanging"
                                    onsorting="grdSerialNumber_Sorting" onrowcommand="grdSerialNumber_RowCommand"
                                    onrowediting="grdSerialNumber_RowEditing">
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
                                     <asp:TemplateField HeaderText="SerialNumber" SortExpression="SerialNumber">
                                        <ItemTemplate>
                                        <asp:Label style="text-align:justify"  Font-Names="Rupee" Width="600px"  ID="lblSerialNumber" runat="server" Text='<%# Eval("SerialNumber") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Eval("SerialNumberId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Eval("SerialNumberId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                                 </div>
                                   </div></div>
                           
           
                     <div ID="divdetails" runat="server" style="margin-bottom:15px">
                    
                       <dt>
                                    <label>
                                    Serial Number <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox  Font-Names="Rupee"  ID="txtSerialNumber"  runat="server" MaxLength="149" Width="400px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvSerialNumber" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtSerialNumber" Display="Dynamic" 
                                        ErrorMessage="Agenda" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>
                             
                        
                                     <dl>
                                     <asp:HiddenField ID="hdnSerialNumberId" runat="server" />
                                     
                                    </dl>
                                    <div id="divbutton" runat="server" class="fullwidth noBorder">
                                    <asp:Button ID="btnInsert" CssClass="btnSave" runat="server" Text="Save" ValidationGroup="a" onclick="btnInsert_Click" />
                                       <asp:Button ID="btnCancel" CssClass="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click" />
                                    </div>
                                    </div>
                                        </ContentTemplate>
                                    <Triggers>
                             
                                    <asp:PostBackTrigger ControlID="btnInsert" />
                               <asp:PostBackTrigger ControlID="btnCancel" />
                               <asp:PostBackTrigger ControlID="grdSerialNumber" />
                             
                                   
    </Triggers>
</asp:UpdatePanel>
                        </dl>
                            </fieldset>         </div>
                                </section>
                                </article>
</asp:Content>

