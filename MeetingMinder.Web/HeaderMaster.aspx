<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="HeaderMaster.aspx.cs" Inherits="MeetingMinder.Web.HeaderMaster" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Photo Gallery--%></h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#054a7f"><b>Header Master</b></font></legend>
                            <dl>
                                
                                         <div style="margin-bottom:15px">
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                </div>
                             
                               <div style="margin-bottom:15px"><asp:Label ID="lblTotalRecord" runat="server" Font-Bold="True"></asp:Label>
                               </div>  
                  <asp:UpdatePanel id="ResultsUpdatePanel" runat="server">   
                                 <ContentTemplate>
                                     <div style="margin-bottom:15px">
                                      <usercontrol:info ID="Info" runat="server" Visible="false" />
                                      <usercontrol:error ID="Error" runat="server" Visible="false" />   
                                </div>
                                    <div style="text-align:center;">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel" DynamicLayout="true">
                        <ProgressTemplate>

                           <img src="img/jquery/ajaxLoader.gif">

                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    </div>  
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
                                     <div class="box_top">
		
		
<h2 class="icon users">Headers </h2>
		
	
</div>                   
                              <div style="margin-bottom:15px">
                                   <div class="box_content">
		<div class="dataTables_wrapper">
                         <asp:GridView ID="grdHeader" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="HeaderId" OnRowDeleting="grdHeader_RowDeleting"
                                 
                                    OnPageIndexChanging="grdHeader_PageIndexChanging"
                                    onsorting="grdHeader_Sorting" onrowcommand="grdHeader_RowCommand" 
                                    onrowediting="grdHeader_RowEditing">                                  
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
                                   
                                        <asp:TemplateField HeaderText="Forum Name" SortExpression="ForumName">
                                        <ItemTemplate>
                                        <asp:Label ID="lblForum" style="width:50px;text-align:justify"  runat="server" Text ='<%#Eval("ForumName") %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                       
                                         <asp:TemplateField HeaderText="Header" SortExpression="Header">
                                        <ItemTemplate>
                                        <asp:Label ID="lblHeader" style="width:50px;text-align:justify"  runat="server" Text ='<%# Eval("Header") %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 

                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Bind("HeaderId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Bind("HeaderId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
            </div></div>
                                 </div>

                           
           
                 <div ID="divdetails" runat="server" style="margin-bottom:15px">                   
                               <dt><label>Header  <span>*</span> : </label></dt><dd >
                                <%--   <CKEditor:CKEditorControl ID="txtDescription" runat="server" Height="200px" Width="865px" ></CKEditor:CKEditorControl>--%>
                              <CKEditor:CKEditorControl ID="txtHeader" ClientIDMode="Static" runat="server" Height="200px" Width="865px" ></CKEditor:CKEditorControl>
                              
                                   <asp:RequiredFieldValidator ID="rfvHeader" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtHeader" Display="Dynamic" 
                                        ErrorMessage="Header" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
      
                               </dd>

                                  
                                        <dl>
                                     <asp:HiddenField ID="hdnHeaderId" runat="server" />
                                     
                                    </dl>
                                    <div id="divbutton" runat="server" class="fullwidth noBorder">
                                    <asp:Button ID="btnInsert" CssClass="btnSave" runat="server" Text="Save" ValidationGroup="a" onclick="btnInsert_Click" />
                                       <asp:Button ID="btnCancel" CssClass="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click"  />
                                    </div>
                                    </div>
                                         </ContentTemplate>
                                    <Triggers>
                               <%--<asp:AsyncPostBackTrigger ControlID="ddlEntity" />--%>
                               

                                <asp:PostBackTrigger ControlID="grdHeader" />
                               <asp:PostBackTrigger ControlID="btnInsert" />
                               <asp:PostBackTrigger ControlID="btnCancel" />
                               <%--<asp:PostBackTrigger ControlID="lnkView" />   --%>  

    </Triggers>
</asp:UpdatePanel>
 </dl>
                            </fieldset>
                                </div>
                                </section>
                                </article>
</asp:Content>
