<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AboutUs.aspx.cs" Inherits="MeetingMinder.Web.AboutUs" %>

<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">
            $(document).ready(function () {
                if ($('#<%=hdnPhotoId.ClientID %>').val().length > 0) {
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
				<h2>&nbsp;<%--About Us--%></h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#054a7f"><b>About Us</b></font></legend>
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
                                   <asp:DropDownList Visible="false" ID="ddlEntity" runat="server" AutoPostBack="true" Width="231px"
                                    onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList> 
                                     <asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Department" Text="Please Select Department" ForeColor="Red"></asp:RequiredFieldValidator>  
                                 
                                </dd>           
                                </div>     
                                     <div class="box_top">
		
		
<h2 class="icon users">About Us</h2>
		
	
</div>                   
                              <div style="margin-bottom:15px">
                         <asp:GridView ID="grdPhoto" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="Id" OnRowDeleting="grdPhoto_RowDeleting"
                                 
                                    OnPageIndexChanging="grdPhoto_PageIndexChanging" 
                                    onsorting="grdPhoto_Sorting" onrowcommand="grdPhoto_RowCommand" 
                                    onrowediting="grdPhoto_RowEditing">                                  
 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                              <PagerStyle CssClass="paginate_active"   />
                                  <RowStyle CssClass="gradeA odd" />
                                  <Columns>

                                                                    
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="About Us" >
                                        <ItemTemplate>
                                         <asp:Label ID="lblEncrptionkey" runat="server" Visible="false" Text='<%#Eval("EncryptionKey") %>'></asp:Label>
                                      <asp:LinkButton CssClass="AddView" ID="lbnApprove" CommandArgument='<%# Bind("Image") %>' CausesValidation="false" CommandName="view" Text="View" runat="server" ToolTip="View PDF"></asp:LinkButton>
                                       </ItemTemplate>
                                    </asp:TemplateField>

                                       
<%--                                         <asp:TemplateField HeaderText="Description" SortExpression="Description">
                                        <ItemTemplate>
                                        <asp:Label ID="lblDescription" style="width:50px;text-align:justify"  runat="server" Text ='<%# GetDesc(Eval("Text")) %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                 

                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Bind("Id") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton Visible="true" ID="lbtnDelete" CommandArgument='<%# Bind("Id") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                                 </div>

                           
           
                 <div ID="divdetails" runat="server" style="margin-bottom:15px" visible="false">
                    
                    <div style="display:none">
                               <dt ><label>Description  <span>*</span> : </label></dt><dd >
                                   <asp:TextBox Visible="false" ID="txtDescription" runat="server" Height="200px" Width="865px" ></asp:TextBox>
                              <asp:RequiredFieldValidator ID="rfvDesc" runat="server"  
                                Enabled="false"  class="invalid-side-note" ControlToValidate="txtDescription" Display="Dynamic" 
                                        ErrorMessage="Description" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
      
                               </dd>
                     </div>
                                   <dt><label>About Us (PDF only) <span>*</span> : </label></dt><dd >
                                  <asp:FileUpload ID="fuPhoto" runat="server"></asp:FileUpload>
                                   <asp:RequiredFieldValidator ID="rfvPhoto" runat="server" 
                                        class="invalid-side-note" ControlToValidate="fuPhoto" Display="Dynamic" 
                                        ErrorMessage="About Us" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                         <asp:LinkButton ID="lnkView" runat="server" Visible="false" Text="View" 
                                             onclick="lnkView_Click"></asp:LinkButton>
                               </dd>
                                        <dl>
                                  
                                     
                                    </dl>
                                    <div id="divbutton" runat="server" class="fullwidth noBorder">
                                    <asp:Button ID="btnInsert" CssClass="btnSave" runat="server" Text="Save" ValidationGroup="a" onclick="btnInsert_Click1"  />
                                       <asp:Button ID="btnCancel" CssClass="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click1"  />
                                    </div>
                                    </div>
                                        <asp:HiddenField ID="hdnPhotoId" runat="server" />
                                         </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                               
                                <asp:PostBackTrigger ControlID="lnkView" />
                                <asp:PostBackTrigger ControlID="grdPhoto" />
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
