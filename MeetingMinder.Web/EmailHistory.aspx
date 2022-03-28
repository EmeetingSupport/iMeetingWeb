<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="EmailHistory.aspx.cs" Inherits="MeetingMinder.Web.EmailHistory" %>
<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>
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
							<legend><font color="#054a7f"><b>Email History</b></font></legend>
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

                                    <tr style="display:none">
                                     <td style="width:95px;">
                                    Department
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlEntity" Visible="false" runat="server"  Width="50%" 
                                            ></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Department" Text="Please Select Department" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
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
                                     <asp:DropDownList ID="ddlForum" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlForum_SelectedIndexChanged"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Forum" Text="Please Select Forum" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
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
                                     <asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Meeting" Text="Please Select Meeting" ForeColor="Red"></asp:RequiredFieldValidator>
                                 
                                                   </td>
                                     </tr>
                               <tr>
                                   <td colspan="3">
                                      <div class="box_top">
		
		
<h2 class="icon users">Email History </h2>
		
	
</div>                   
                              <div style="margin-bottom:15px">
                                   <div class="box_content">
		<div class="dataTables_wrapper">
                         <asp:GridView ID="grdHistory" runat="server" AutoGenerateColumns="False"  
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" 
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable"   
                                   >                                  
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
                                   
                                        <asp:TemplateField HeaderText="Subject" >
                                        <ItemTemplate>
                                          <asp:Label ID="lblName" style="width:250px;text-align:justify"  runat="server" Text ='<%# Eval("Subject")   %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                                            
                                         <asp:TemplateField HeaderText="To"  >
                                        <ItemTemplate>
                                        <asp:Label ID="lblNotice" style="width:50px;text-align:justify"  runat="server" Text ='<%#Eval("Receiver")%>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CC"  >
                                        <ItemTemplate>
                                        <asp:Label ID="lblCC" style="width:50px;text-align:justify"  runat="server" Text ='<%#Eval("CC")%>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Body"  >
                                        <ItemTemplate>
                                        <asp:Label ID="lblAgenda" style="width:500px;text-align:justify"  runat="server" Text ='<%#Eval("Body")%>'  />
                             
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                                          <asp:TemplateField HeaderText="Sender" >
                                        <ItemTemplate>
                                          <asp:Label ID="lblSenderName" style="width:50px;text-align:justify"  runat="server" Text ='<%#  MM.Core.Encryptor.DecryptString(Eval("Suffix").ToString()) +" "+  MM.Core.Encryptor.DecryptString(Eval("FirstName").ToString())  +" "+   MM.Core.Encryptor.DecryptString(Eval("LastName").ToString())   %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                                                     <asp:TemplateField HeaderText="Sent On" >
                                        <ItemTemplate>
                                          <asp:Label ID="lblSentOn" style="width:50px;text-align:justify"  runat="server" Text ='<%# Convert.ToDateTime(Eval("CreatedOn").ToString()).ToString("D") %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 </Columns>
                             
                                </asp:GridView>
            </div></div>
                                 </div>
                                   </td>
                               </tr>
                                 </table>
                                 </div>
                                 
  
 
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlMeeting" />
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />

                            

                       
                         
        
    </Triggers>
</asp:UpdatePanel>
                               </div>
                                 </dl>
                               <br />
						</fieldset>
                             										
									
				</div>					
				
			</section>			
		</article>
</asp:Content>
