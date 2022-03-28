<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ViewProceeding.aspx.cs" Inherits="MeetingMinder.Web.ViewProceding" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--View Meeting--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>View Proceeding</b></font></legend>
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
                                <div >
                                 <table width="75%">
                           <div class="box_top">
        
        
                               <caption>
                                   <h2 class="icon users">Proceeding </h2>
                               </caption>
        
    
</div>
                                    <tr style="display:none;">
                                     <td style="width:95px;">
                                    Department
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList Visible="false" ID="ddlEntity" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList>
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Department" Text="Please Select Department" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                     
                                 
                                     </td>
                                     </tr>
                                     <tr>
                                     <td style="width:95px;">
                                    Forum
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlForum"  runat="server"  Width="50%" onselectedindexchanged="ddlForum_SelectedIndexChanged"
                                           AutoPostBack="true"   ></asp:DropDownList>
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Forum" Text="Please Select Forum" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                     
                                 
                                     </td>
                                     </tr>
                                                                           <tr>
                                     <td style="width:95px;">
                                     Year 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                         <asp:DropDownList ID="ddlYear" onselectedindexchanged="ddlForum_SelectedIndexChanged" AutoPostBack="true"  runat="server" Width="50%"></asp:DropDownList>
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvYear" ControlToValidate="ddlYear" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Year" Text="Please Select Year" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                 
                                                   </td>
                                     </tr>


                                     <tr>
                                     <td>
                                    Meeting
                                     </td>
                                     <td >
                                     :
                                     </td>
                                     <td   align="left">
                                         <asp:DropDownList ID="ddlMeeting" runat="server" Width="50%" OnSelectedIndexChanged="ddlMeeting_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Meeting" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                 
                                    <%-- <asp:TextBox ID="txtDate" ClientIDMode="Static" runat="server" Width="70%"></asp:TextBox>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvNew" ControlToValidate="txtDate" Display="Dynamic" ValidationGroup="a" ErrorMessage="Answer" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
     --%>                                </td>
                                     </tr>

                                      

                                     

                                       <%--<tr>
                                     <td style="width:95px;">
                                    Scheduled Meeting 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                             </td>
                                     </tr>    --%>                             
                                 </table>
                                 <br />
          
                                     <div class="box_content">
		
		<!-- Simple Sorting Table + Pagination: Start -->
		<div class="dataTables_wrapper">

                                       <asp:GridView ID="grdProceding" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="ProcedingId"
                                 
                                    OnPageIndexChanging="grdProceding_PageIndexChanging"
                                    onsorting="grdProceding_Sorting" OnRowCommand="grdProceding_RowCommand"           >
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
                                      
                                     <%--  <asp:TemplateField  HeaderText="Meeting" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblMeeting" runat="server" Text='<%# Eval("MeetingNumber").ToString()+" "+ MM.Core.Encryptor.DecryptString(Eval("ForumName").ToString() )+" "+Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("MMMM dd, yyyy") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                       <asp:TemplateField  HeaderText="Forum" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblForumName" runat="server" Text='<%# MM.Core.Encryptor.DecryptString(Eval("ForumName").ToString() )%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 

                                      <asp:TemplateField  HeaderText="Agenda" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblAgendaName" runat="server" Text='<%# Eval("AgendaName").ToString() %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>  

                                      <asp:TemplateField  HeaderText="Sub Agenda" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblSubAgendaName" runat="server" Text='<%# Eval("SubAgendaName").ToString() %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>  --%>

                                      <asp:TemplateField HeaderText="Proceeding" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncrptionkey" runat="server" Visible="false" Text='<%#Eval("EncryptionKey") %>'></asp:Label>
                                           <asp:LinkButton ID="lbnProceding" CommandArgument='<%# Bind("ProcedingName") %>' CausesValidation="false" CommandName="view" Text="View" runat="server" ToolTip="Proceding"></asp:LinkButton>
                                           <%--&nbsp; &nbsp;  <asp:LinkButton ID="lbnReject" CommandArgument='<%# Bind("EntityId") %>' CausesValidation="false" CommandName="Decline" Text="Decline" runat="server" ToolTip="Decline"></asp:LinkButton>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                             </div>
                             <%--    <div style="margin-bottom:15px">--%>
                          </div>
                                 </div>
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />

                                <asp:PostBackTrigger ControlID="grdProceding" />
                             
                              <%-- <asp:PostBackTrigger ControlID="btnSubmit" />
                               <asp:PostBackTrigger ControlID="btnCancel" />
                             --%>
                         
        
    </Triggers>
</asp:UpdatePanel>
                               </div>
                               <br />
						</fieldset>
                             										
					
				</div>					
				
			</section>			
		</article>

</asp:Content>
