<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="SectionMaster.aspx.cs" Inherits="MeetingMinder.Web.SectionMaster" %>

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
							<legend><font color="#054a7f"><b>Section Master</b></font></legend>
                            <dl>
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                 
                       <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                           CssClass="notification error" 
                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                           ValidationGroup="a" />
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
                    <%--<div style="display:none">
                     <dt>
                                    <label>
                                    Entity <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                   <asp:DropDownList ID="ddlEntity" Visible="false" runat="server" AutoPostBack="true" Width="231px"
                                    onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList> 
                                     <asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Entity" Text="Please Select Entity" ForeColor="Red"></asp:RequiredFieldValidator>  
                                 
                                </dd>  
                                </div> --%>
                                                   <div id="ContentPlaceHolder1_divdetails"  style="margin-bottom:15px">
                                       <dt>
                                    <label>
                                     Forum <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:DropDownList ID="ddlForum" Width="231px" runat="server" AutoPostBack="true"
                                        onselectedindexchanged="ddlForum_SelectedIndexChanged" ></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Forum" Text="Invalid"  ForeColor="Red"></asp:RequiredFieldValidator>                                     
                                     </dd>
                                         </div>
                                     <div>

                                         <asp:LinkButton Visible="false" ID="lnkChangeOrder" runat="server" OnClick="lnkChangeOrder_Click" Text="Change Order" ></asp:LinkButton>
                                     </div>
                                     <div class="box_top">
		
		
<h2 class="icon users">Section Master</h2>
		
	
</div>
                       
                                <div style="margin-bottom:15px">
                               <div class="box_content">
		
		<!-- Simple Sorting Table + Pagination: Start -->
		<div class="dataTables_wrapper">
                                 <asp:GridView ID="grdSection" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" OnRowDeleting="grdSection_RowDeleting"
                                 
                                    OnPageIndexChanging="grdSection_PageIndexChanging" 
                                    onsorting="grdSection_Sorting" onrowcommand="grdSection_RowCommand" 
                                    onrowediting="grdSection_RowEditing">
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

                                        <asp:TemplateField HeaderText="Serial Number" >
                                        <ItemTemplate>
                                        <asp:Label style="text-align:justify"  Font-Names="Rupee"  ID="lblSerialNumber" runat="server" Text='<%# Eval("SerialNumber") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                     <asp:TemplateField HeaderText="Title" >
                                        <ItemTemplate>
                                        <asp:Label style="text-align:justify"  Font-Names="Rupee"  ID="lblTitle" runat="server" Text='<%# Eval("Title") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    


                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Eval("SectionId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Eval("SectionId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                                 </div>
                                   </div></div>
                           
           
                     <div ID="divdetails" runat="server" style="margin-bottom:15px">
                    

                          <dt >
                                    <label>
                                    Serial Number <span>*</span> :
                                    </label>
                                </dt>
                                <dd  >
                                    <asp:TextBox  Font-Names="Rupee"  ID="txtSerialNumber"  runat="server" MaxLength="149" Width="400px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvSerialNumber" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtSerialNumber" Display="Dynamic" 
                                        ErrorMessage="Serial Number" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>

                          <br>
                          <br>
                          <br>
                          <dt>
                              <label>
                              Title <span>*</span> :
                              </label>
                          </dt>
                          <dd>
                              <asp:TextBox ID="txtTitle" runat="server" Font-Names="Rupee" MaxLength="149" TextMode="MultiLine" Width="400px"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="rfvTitle" runat="server" class="invalid-side-note" ControlToValidate="txtTitle" Display="Dynamic" ErrorMessage="Title" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                          </dd>
                          <dl>
                              <asp:HiddenField ID="hdnSectionId" runat="server" />
                          </dl>
                          <div id="divbutton" runat="server" class="fullwidth noBorder">
                              <asp:Button ID="btnInsert" runat="server" CssClass="btnSave" onclick="btnInsert_Click1" Text="Save" ValidationGroup="a" />
                              <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="btnCancel" OnClick="btnCancel_Click" Text="Cancel" />
                          </div>

                          <br></br>

                          </br>
                          
                        
                                    </div>
                                        </ContentTemplate>
                                    <Triggers>
                               <%--<asp:AsyncPostBackTrigger ControlID="ddlEntity" />--%>
                                        <asp:PostBackTrigger ControlID="ddlForum" />
                                    <asp:PostBackTrigger ControlID="btnInsert" />
                               <asp:PostBackTrigger ControlID="btnCancel" />
                               <asp:PostBackTrigger ControlID="grdSection" />
                             
                                   
    </Triggers>
</asp:UpdatePanel>
                        </dl>
                            </fieldset>         </div>
                                </section>
                                </article>


</asp:Content>
