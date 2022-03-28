<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="KeyInformation.aspx.cs" Inherits="MeetingMinder.Web.KeyInformation" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            if ($('#<%=hdnKeyInfoId.ClientID %>').val().length > 0) {
                var id = '<%=divdetails.ClientID %>';
                $("html, body").animate({
                    scrollTop: $('#' + id
                        ).offset().top
                }, 1500);
            }            
        });
                
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Board of Directors--%></h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#054a7f"><b>Board of Directors</b></font></legend>
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
                                    Department <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                   <asp:DropDownList ID="ddlEntity" Visible="false" runat="server" AutoPostBack="true" Width="231px"
                                    onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList> 
                                     <asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Department" Text="Please Select Department" ForeColor="Red"></asp:RequiredFieldValidator>  
                                 
                                </dd>    
                                </div>
                                   <div  style="margin-bottom:15px">
                                                
<asp:HyperLink ID="hlChangeOrder" Visible="false" Text="Change Order" runat="server" ></asp:HyperLink>
  </div>
                                     <div class="box_top">
		
		
<h2 class="icon users">Board Of Directors </h2>
		
	
</div>
                                  <div style="margin-bottom:15px">
                                       <div class="box_content">
		
		<!-- Simple Sorting Table + Pagination: Start -->
		<div class="dataTables_wrapper">
                                    <asp:GridView ID="grdKeyInfo" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="KeyInformationId" OnRowDeleting="grdKeyInfo_RowDeleting"
                                 
                                    OnPageIndexChanging="grdKeyInfo_PageIndexChanging" 
                                    onsorting="grdKeyInfo_Sorting" onrowcommand="grdKeyInfo_RowCommand" 
                                    onrowediting="grdKeyInfo_RowEditing">
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
                                   
                                    <asp:TemplateField HeaderText="Photo" >
                                        <ItemTemplate>
                                   <img alt="Image Not found" height="50px" width="100px" src="img/Uploads/KeyInfo/<%# Eval("Photograph") %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                        <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Suffix") +" "+ Eval("Name") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Designation" SortExpression="Designation">
                                        <ItemTemplate>
                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Description" SortExpression="Description">
                                        <ItemTemplate>
                                        <asp:Label ID="lblDescription" style="width:50px;text-align:justify"  runat="server" Text ='<%# GetDesc(Eval("Description")) %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 

                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Bind("KeyInformationId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Bind("KeyInformationId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
            </div></div>
                                 </div>

                                   
                 <div ID="divdetails" runat="server" style="margin-bottom:15px">
                    <dt>
                    <label>Prefix :</label>
                    </dt>
                    <dd>
                    <asp:DropDownList ID="ddlSuffix" runat="server"></asp:DropDownList>
                    </dd>
                      <dt>
                                    <label>
                                   Name <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtName" runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtName" Display="Dynamic" 
                                        ErrorMessage="Name" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>
                       <dt>
                                    <label>
                                   Designation <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtDesignation" runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvUser" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtDesignation" Display="Dynamic" 
                                        ErrorMessage="Designation" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>
                               <dt><label>Description  : </label></dt><dd >
                                   <CKEditor:CKEditorControl ID="txtDescription" runat="server" Height="200px" Width="865px" name="DSC" CssClass="editor"></CKEditor:CKEditorControl>
                              <%--<asp:TextBox TextMode="MultiLine" ID="txtDescription" runat="server" Height="200px" Width="865px" ></asp:TextBox>--%>
                               </dd>

                                   <dt><label>Photograph <span>*</span> : </label></dt><dd >
                                  <asp:FileUpload ID="fuPhoto" runat="server"></asp:FileUpload>
                                          <asp:RequiredFieldValidator ID="rfvPhoto" runat="server" 
                                        class="invalid-side-note" ControlToValidate="fuPhoto" Display="Dynamic" 
                                        ErrorMessage="Photograph" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
          
                               </dd>
                                        <dl>
                                     <asp:HiddenField ID="hdnKeyInfoId" runat="server" />
                                     
                                    </dl>
                                    <div id="divbutton" runat="server" class="fullwidth noBorder">
                                    <asp:Button ID="btnInsert" CssClass="btnSave" runat="server" Text="Save" ValidationGroup="a" onclick="btnInsert_Click1"  />
                                       <asp:Button ID="btnCancel" CssClass="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click1"  />
                                    </div>
                                    </div>
                                     </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                               

                                <asp:PostBackTrigger ControlID="grdKeyInfo" />
                               <asp:PostBackTrigger ControlID="btnInsert" />
                               <asp:PostBackTrigger ControlID="btnCancel" />
                                   
    </Triggers>
</asp:UpdatePanel>
                                        </dl>
                            </fieldset>
                                </div>
                                </section>
                                </article>
                             

</asp:Content>
