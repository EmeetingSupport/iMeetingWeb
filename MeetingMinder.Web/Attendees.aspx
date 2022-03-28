<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="Attendees.aspx.cs" Inherits="MeetingMinder.Web.EventCal.Attendis" %>

<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Add Attendees--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>Add Attendees</b></font></legend>
                             <dl>
                             <div style="width:900px;">
       <%--                           <asp:UpdatePanel id="ResultsUpdatePanel" runat="server">   
                                 <ContentTemplate>

                                    <div style="text-align:center;">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel" DynamicLayout="true">
                        <ProgressTemplate>

                           <img src="img/jquery/ajaxLoader.gif">

                        </ProgressTemplate>
                    </asp:UpdateProgress>
                               
                             </div>      --%>           

                                <div style="margin-bottom:15px"><userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                <div style="margin-bottom:15px">
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                </div>
                                     <div>

                             <div class="box_top">
		
		
<h2 class="icon users">Add Attendees</h2>
		
	
</div>

                          <asp:GridView ID="grdAttendis" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    PageSize="10" DataKeyNames="VisiterId"
                                   
                                      OnRowDeleting="grdAttendis_RowDeleting"
                                      onrowcommand="grdAttendis_RowCommand"    
                                      
                                       onpageindexchanging="grdAttendis_PageIndexChanging"   >
                                    
                                     
                                   <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="access odd" />
                               <PagerStyle CssClass="paginate_active"   />
                                  <Columns>

                                  <%--  <asp:TemplateField>
                                        <ItemStyle  HorizontalAlign="Center" />
                                        <HeaderStyle  HorizontalAlign="Center" />
                                        <HeaderTemplate>
                                            <asp:CheckBox  ID="chkHeader" onclick="javascript:fn_select_all(this);" runat="server" />
                                         </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox  ID="chkSubAdmin" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                  
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Name" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("Name_Of_Visitor") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Photo" >
                                        <ItemTemplate>
                                         <img id="UserImg" height="50px" width="100px" src="/img/Uploads/EntityLogo/<%# Eval("Upload_Photo") %>" alt="Image Not Available" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                               
                                <asp:TemplateField HeaderText="Designation" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeisgantion" runat="server" Text='<%# Eval("Designation_Of_Visitor") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   
                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         
                                            <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Bind("VisiterId") %>' ImageUrl="~/img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edits" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Bind("VisiterId") %>' ImageUrl="~/img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                               </div>
                              <br /><br />
                                <div style="width:900px;">
                                     <table width="75%">

                                    <tr >
                                     <td style="width:95px;">
                                    Name<span>&nbsp;*</span>
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                   <asp:TextBox ID="txtName" runat="server" ></asp:TextBox>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"  Display="Dynamic" ValidationGroup="a" ErrorMessage="Name" Text="Inavalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </td>
                                     </tr>
                                     <tr>
                                     <td style="width:95px;">
                                    Designation
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                         <asp:TextBox ID="txtDesignation" runat="server" ></asp:TextBox>
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="txtDesignation"  Display="Dynamic" ValidationGroup="a" ErrorMessage="Designation" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                     
                                 
                                     </td>
                                     </tr>
                                     <tr>
                                     <td style="width:95px;">
                                    Organization 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                        <asp:TextBox ID="txtOrgnaization" runat="server"></asp:TextBox>
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Meeting" Text="Please Select Meeting" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                 
                                                   </td>
                                     </tr>
                                  <tr>
                                     <td style="width:95px;">
                                    Url 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                        <asp:TextBox ID="txtLinkedInUrl" runat="server"></asp:TextBox>
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Meeting" Text="Please Select Meeting" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                 
                                                   </td>
                                     </tr>   <tr>
                                     <td style="width:95px;">
                                    Email 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                      <asp:TextBox ID="txtEmail" runat="server" ></asp:TextBox>
                                         <asp:RegularExpressionValidator ID="rgvEmail" runat="server" 
                                        ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Invalid Format" 
                                        SetFocusOnError="True" ValidationGroup="a"
                                        ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(['\&quot;][^\&lt;\&gt;'\&quot;]*['\&quot;]\s*\&lt;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\&gt;))(,\s*((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(['\&quot;][^\&lt;\&gt;'\&quot;]*['\&quot;]\s*\&lt;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\&gt;)))*"></asp:RegularExpressionValidator> 
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Meeting" Text="Please Select Meeting" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                 
                                                   </td>
                                     </tr>

                                      <tr>
                                     <td style="width:95px;">
                                    Phone No 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                      <asp:TextBox ID="txtPhoneNo" runat="server" ></asp:TextBox> 
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Meeting" Text="Please Select Meeting" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                 
                                                   </td>
                                     </tr>
                                     <tr>
                                     <td style="width:95px;">
                                    Profile Pic 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                        <asp:FileUpload ID="fuProfile" runat="server" />
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Meeting" Text="Please Select Meeting" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                 
                                                   </td>
                                     </tr>

                                     <tr align="right">
                                        <td colspan="5" align="right"> 
                                        <div class="fullwidth noBorder"> 
                                        <asp:HiddenField ID="hdnVisitorId" runat="server" />
                                           <%-- OnClientClick="return PdfUploadValidation();"--%>
                                        <asp:Button  ID="btnSubmit"  CssClass="btnSave" CausesValidation="true" ValidationGroup="a" runat="server" Text="Submit" onclick="btnSubmit_Click"></asp:Button> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnSave" onclick="btnCancel_Click"></asp:Button>
                                        </div>
                                       </td>
                                       </tr>
                                 </table>
                                 <br />
                                   
                                 </div>
                          <%--                   </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />--%>

                         


                       
                         
<%--        
    </Triggers>
</asp:UpdatePanel>--%>
                               </div>
                               <br />
						</fieldset>
                             										
									
				</div>					
				
			</section>			
		</article>
    <div class="clearfix">
    </div>
</asp:Content>

