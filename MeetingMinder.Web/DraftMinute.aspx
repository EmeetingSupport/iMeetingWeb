<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="DraftMinute.aspx.cs" Inherits="MeetingMinder.Web.DraftMinute" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--View Notice--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>Draft Minutes</b></font></legend>
                             <dl>
                             <div >
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
                                 <table width="75%">

                             <%--       <tr style="display:none">
                                     <td style="width:95px;">
                                    Entity
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlEntity" Visible="false" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Entity" Text="Please Select Entity" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>--%>
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
                                     <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Forum" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
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
                                         <asp:DropDownList ID="ddlMeeting" onselectedindexchanged="ddlMeeting_SelectedIndexChanged" AutoPostBack="true"  runat="server" Width="50%"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Meeting" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                 
                                                   </td>
                                     </tr>
                                    
                                 </table>

                                       <div class="box_top">
		
		
<h2 class="icon users">Draft Minute </h2>
		
	
</div>
                                   <div class="box_content">
		<div class="dataTables_wrapper">
                             <asp:GridView ID="grdDraftMOM" runat="server" AutoGenerateColumns="False" 
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found"  
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                 EnableSortingAndPagingCallbacks = "true"   class="datatable"  DataKeyNames="DraftMinutesId" OnRowDeleting="grdDraftMOM_RowDeleting"
                                 
                                     
                                   onrowcommand="grdDraftMOM_RowCommand">
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

                                    <asp:TemplateField HeaderText="MOM Name" SortExpression="DraftMOMName">
                                        <ItemTemplate>
                                                <asp:LinkButton CssClass="AddView" ID="lnkUploadAgenda" CommandArgument='<%# Eval("DraftMinute") %>' CommandName="download"  Text='<%# Eval("DraftMinuteName") %>'  runat="server" />                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Created On">
                                        <ItemTemplate>
                                                <%# Convert.ToDateTime(Eval("CreatedOn")).ToString("f") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   
                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                            <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Eval("DraftMinutesId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="EditAgenda" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Eval("DraftMinutesId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>

                                 </Columns>
                                </asp:GridView>
            </div>
                                       </div>
                                 </div>
 
                                               <div ID="divdetails" runat="server" style="margin-bottom:15px">
                    
                    
                              

                                   <dt><label>Draft MOM (PDF only) <span>*</span> : </label></dt><dd >
                                  <asp:FileUpload ID="fuMOM" runat="server"></asp:FileUpload>
                                   <asp:RequiredFieldValidator ID="rfvPhoto" runat="server" 
                                        class="invalid-side-note" ControlToValidate="fuMOM" Display="Dynamic" 
                                        ErrorMessage="Draft MOM" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                         <asp:LinkButton ID="lnkView" runat="server" Visible="false" Text="View" 
                                             onclick="lnkView_Click"></asp:LinkButton>
                               </dd>
                                        <dl>
                                  
                                     
                                    </dl>
                                    <div id="divbutton" runat="server" class="fullwidth noBorder">
                                    <asp:Button ID="btnInsert" CssClass="btnSave" runat="server" Text="Save" ValidationGroup="a" onclick="btnInsert_Click"  />
                                       <asp:Button ID="btnCancel" CssClass="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click"  />
                                    </div>
                                    </div>
                                        <asp:HiddenField ID="hdnMOMId" runat="server" />
                                             </ContentTemplate>
                                    <Triggers>
                       
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />
                                              <asp:AsyncPostBackTrigger ControlID="ddlMeeting" />
                                        
                              <asp:PostBackTrigger ControlID="btnInsert" />
                               <asp:PostBackTrigger ControlID="btnCancel" />
                                          <asp:PostBackTrigger ControlID="lnkView" />
                                        
    </Triggers>
</asp:UpdatePanel>
                               </div>
                               <br />
						</fieldset>
                             										
									
				</div>					
				
			</section>			
		</article>
 
</asp:Content>
