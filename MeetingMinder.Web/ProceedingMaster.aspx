<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ProceedingMaster.aspx.cs" Inherits="MeetingMinder.Web.Proceding" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <article class="content-box minimizer">
			<header>			
				<h2> &nbsp; <%--Upload Minutes--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font  color="#054a7f"><b>Proceeding</b></font></legend>
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

                                <div style="margin-bottom:15px"><userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                <div style="margin-bottom:15px">
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                </div>


                                      <div class="box_top">
		
		
<h2 class="icon users">Proceeding</h2>
		
	
</div>
                                   <div class="box_content">
		<div class="dataTables_wrapper">
                                   <asp:GridView ID="grdProceding" runat="server" AutoGenerateColumns="False" 
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="ProcedingId"                                
                                    onsorting="grdProceding_Sorting" onrowcommand="grdProceding_RowCommand" 
                                    onpageindexchanging="grdProceding_PageIndexChanging" 
                                       onrowediting="grdProceding_RowEditing" 
                                       onrowdeleting="grdProceding_RowDeleting">
                                 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                        <PagerStyle CssClass="paginate_active"   />
                                  <Columns>
                                     <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Eval("RowNumber") %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                       </asp:TemplateField>

                                    <%--<asp:TemplateField HeaderText="Agenda" SortExpression="MeetingVenue" >
                                        <ItemTemplate>
                                          <asp:Label ID="lblAgenda"  style="text-align:justify"  runat="server"
                                               Text='<%# Eval("AgendaName") %>' ></asp:Label>--%>
                                            <%--Text='<%# Convert.ToDateTime(Eval("MeetingDate")).ToString("MMM d yyyy") +" "+ Eval("MeetingTime") +" "+ Eval("MeetingVenue") %>'--%>
                                       <%-- </ItemTemplate>
                                    </asp:TemplateField>--%>

                                         <asp:TemplateField HeaderText="Forum" SortExpression="ForumName" >
                                        <ItemTemplate>
                                          <asp:Label ID="lblForum"  style="text-align:justify" runat="server" Text='<%#MM.Core.Encryptor.DecryptString(Eval("ForumName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Meeting"  >
                                        <ItemTemplate>
                                          <asp:Label ID="lblMeeting"  style="text-align:justify" runat="server" Text='<%#Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("MMM d yyyy") +" "+ MM.Core.Encryptor.DecryptString(Eval("MeetingTime").ToString()) +" "+ MM.Core.Encryptor.DecryptString(Eval("MeetingVenue").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                     

                                     <asp:TemplateField HeaderText="Proceeding" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncrptionkey" runat="server" Visible="false" Text='<%#Eval("EncryptionKey") %>'></asp:Label>
                                           <asp:LinkButton ID="lbnProceding" CommandArgument='<%# Bind("ProcedingName") %>' CausesValidation="false" CommandName="view" Text="View" runat="server" ToolTip="Proceding"></asp:LinkButton>
                                           <%--&nbsp; &nbsp;  <asp:LinkButton ID="lbnReject" CommandArgument='<%# Bind("EntityId") %>' CausesValidation="false" CommandName="Decline" Text="Decline" runat="server" ToolTip="Decline"></asp:LinkButton>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                          <asp:ImageButton ID="lbtnEdit" CommandArgument='<%#  Eval("ProcedingId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Eval("ProcedingId")%>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                                                
                                 </Columns>
                                </asp:GridView>

             <asp:Repeater ID="rptPager" runat="server">
                 <HeaderTemplate>
                     <table>
                     <tr class="paginate_active">
				<td colspan="5"><table>
					<tbody><tr>
                 </HeaderTemplate>
<ItemTemplate>
    <td>
    <asp:LinkButton ID="lnkPage" runat="server" Text = '<%#Eval("Text") %>' CommandArgument = '<%# Eval("Value") %>' Enabled = '<%# Eval("Enabled") %>' OnClick = "Page_Changed"></asp:LinkButton></td>
</ItemTemplate>
                 <FooterTemplate>
                     </tr>
				</tbody></table></td>
			</tr></table>
                 </FooterTemplate>
</asp:Repeater>
            </div>
                                       </div>

                                     <br />
                              <div id="divData" style="width:900px;">
                                 <table width="75%">
                                  
                                  <tr>
                                     <td>
                                    Forum<span>&nbsp;*</span>
                                     </td>
                                     <td >
                                     :
                                     </td>
                                     <td   align="left">
                                     <asp:DropDownList ID="ddlForum" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlForum_SelectedIndexChanged"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Forum" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>

                                  <tr>
                                     <td>
                                    Meeting<span>&nbsp;*</span>
                                     </td>
                                     <td >
                                     :
                                     </td>
                                     <td   align="left">
                                         <asp:DropDownList ID="ddlMeeting" runat="server" Width="50%"  ></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Meeting" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                 
                                    <%-- <asp:TextBox ID="txtDate" ClientIDMode="Static" runat="server" Width="70%"></asp:TextBox>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvNew" ControlToValidate="txtDate" Display="Dynamic" ValidationGroup="a" ErrorMessage="Answer" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
     --%>                                </td>
                                     </tr>
                                       
                                    
                                       <tr>
                                     <td>
                                   Proceeding 
                                         <span>&nbsp;* </span> (<span style="color:red;">Pdf file only</span>)
                                     </td>
                                     <td >
                                     :
                                     </td>
                                     <td   align="left">
                                     <asp:FileUpload ID="fuProceding" style="width:270px" runat="server"  />
                                   <%-- <br /> <label><span>&nbsp;Pdf file only</span></label>--%>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvProceding" ControlToValidate="fuProceding" Display="Dynamic" ValidationGroup="a" ErrorMessage="Upload Proceding" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     <%--<asp:LinkButton ID="lnkView" runat="server" Visible="false" Text="View" 
                                             onclick="lnkView_Click"></asp:LinkButton>--%>
                                     </td>
                                     </tr>
                                 
                                 
                                              

                                     <tr align="right">
                                        <td colspan="5" align="right"> 
                                        <div class="fullwidth noBorder">
                                        <asp:HiddenField ID="hdnUploadId" runat="server" /> 
                                        <asp:Button ID="btnSubmit" CssClass="btnSave" CausesValidation="true" ValidationGroup="a" runat="server" Text="Submit" onclick="btnSubmit_Click"></asp:Button> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" CssClass="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click"></asp:Button>
                                        </div>
                                 </td>
                                 </tr>
                                 </table>
                                 </div >

                                 
                              </div>
                              <br /><br />
                                       
                                             </ContentTemplate>
                                    <Triggers>
                               <%--<asp:AsyncPostBackTrigger ControlID="ddlEntity" />--%>
                               <asp:PostBackTrigger ControlID="ddlForum" />

                                      <%--   <asp:PostBackTrigger ControlID="ddlMeeting" />--%>
                                         <%--<asp:PostBackTrigger ControlID="ddlAgenda" />
                                          <asp:PostBackTrigger ControlID="ddlSubAgenda" />--%>
                                <asp:PostBackTrigger ControlID="grdProceding" />
                               <asp:PostBackTrigger ControlID="btnSubmit" />
                               <asp:PostBackTrigger ControlID="btnCancel" />
                              <%-- <asp:PostBackTrigger ControlID="lnkView" />--%>
                             
                         
        
    </Triggers>
</asp:UpdatePanel>
                               </div>
                               <br />
						</fieldset>
                             										
									
				</div>					
				
			</section>			
		</article>

</asp:Content>
