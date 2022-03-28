<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AdMaster.aspx.cs" Inherits="MeetingMinder.Web.AdMaster" %>
<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <article class="content-box minimizer">
			<header>			
				<h2>Ad Details</h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#AE432E"><b>Ad Details</b></font></legend>
                            <dl>
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                 
                                         <div style="margin-bottom:15px">
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                </div>
                            
                               <div style="margin-bottom:15px"><asp:Label ID="lblTotalRecord" runat="server" Font-Bold="True"></asp:Label>
                               </div>  
                              <div style="margin-bottom:15px">
<asp:GridView ID="grdAdDetails" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="AdMasterId" OnRowDeleting="grdAdDetails_RowDeleting"
                                 
                                    OnPageIndexChanging="grdAdDetails_PageIndexChanging" 
                                    onsorting="grdAdDetails_Sorting" onrowcommand="grdAdDetails_RowCommand" 
                                    onrowediting="grdAdDetails_RowEditing">
                                  <HeaderStyle HorizontalAlign="Center" BackColor="#AE432E" ForeColor="#F2DEDA" Font-Bold="true" />
                                  <Columns>

                                                                    
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="First Name" SortExpression="FirstName" >
                                        <ItemTemplate>
                   <asp:Label ID="lblFirst" style="width:50px;text-align:justify"  runat="server" Text ='<%# Eval("FirstName") %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                       
                                         <asp:TemplateField HeaderText="Last" SortExpression="LastName">
                                        <ItemTemplate>
                                        <asp:Label ID="lblDescription" style="width:50px;text-align:justify"  runat="server" Text ='<%# Eval("LastName") %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 
                                         <asp:TemplateField HeaderText="UserId" SortExpression="UserId">
                                        <ItemTemplate>
                                        <asp:Label ID="lblUserId" style="width:50px;text-align:justify"  runat="server" Text ='<%# Eval("UserId") %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Bind("AdMasterId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Bind("AdMasterId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                                 </div>

                            </dl>
                            </fieldset>
           
                 <div ID="divdetails" runat="server" style="margin-bottom:15px">
                    
                    
                               <dt><label>First Name  <span>*</span> : </label></dt><dd >
                                 <asp:TextBox ID="txtFirstName" runat="server" MaxLength="75"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="rfvDesc" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtFirstName" Display="Dynamic" 
                                        ErrorMessage="First Name" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
      
                               </dd>

                                   <dt><label>Last Name  <span>*</span> : </label></dt><dd >
                                   <asp:TextBox ID="txtLastName" runat="server" MaxLength="75"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="rfvPhoto" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtLastName" Display="Dynamic" 
                                        ErrorMessage="Last Name" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                               </dd>

                                   <dt><label>UserId  <span>*</span> : </label></dt><dd >
                                   <asp:TextBox ID="txtUserId" runat="server" MaxLength="75"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="rfvUserId" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtUserId" Display="Dynamic" 
                                        ErrorMessage="User Id" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                               </dd>

                                        <dl>
                                     <asp:HiddenField ID="hdnAdMasterId" runat="server" />
                                     
                                    </dl>
                                    <div id="divbutton" runat="server" style="margin-bottom:15px;margin-top:15px">
                                    <asp:Button ID="btnInsert" runat="server" Text="Save" ValidationGroup="a" onclick="btnInsert_Click1"  />
                                       <asp:Button ID="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click1"  />
                                    </div>
                                    </div>
                                </div>
                                </section>
                                </article>
</asp:Content>
