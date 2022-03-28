<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ViewUserAttendance.aspx.cs" Inherits="MeetingMinder.Web.ViewUserAttendance" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
         <style>
        .radio-att label {
            display: inline;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Email Notification--%></h2>			
			</header>
			<section>  
                
                                                      
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>Attendance</b></font></legend>
                             <dl>

                                 <div style="float:right">
                                          
                                          </div>
                             <div style="width:900px;">
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
                                    Entity
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList Visible="false" ID="ddlEntity" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Entity" Text="Please Select Entity" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
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
                                     <asp:DropDownList ID="ddlForum" runat="server"  Width="50%" 
                                          onselectedindexchanged="ddlForum_SelectedIndexChanged" ClientIDMode="Static" AutoPostBack="true"   ></asp:DropDownList>
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
                                     <asp:DropDownList ID="ddlMeeting" ClientIDMode="Static" onclick="GenerateTemplates();" runat="server"  Width="50%" AutoPostBack="True" OnSelectedIndexChanged="ddlMeeting_SelectedIndexChanged"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvddlMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Meeting" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>

                                     </tr>
                                        

                                     <tr>
                                     <td>User list</td>
                                       <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                         <div class="box_top">
		
		
<h2 class="icon users">User List </h2>
		
	
</div>
                                     <asp:GridView ID="grdReport" ShowHeader="true" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" DataKeyNames ="UserId"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable">
                                  <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                  <Columns>
                                  
                                         

                                      <asp:TemplateField HeaderText="Name" SortExpression="FirstName">
                             
                                        <ItemTemplate> 
                                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("Suffix") +" "+Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                            <asp:Label Visible="false" ID="lblName" runat="server" Text='<%# Eval("Suffix") +" "+Eval("FirstName") +" "+Eval("MiddleName")+" "+ Eval("LastName") %>'></asp:Label>
                                            <asp:HiddenField ID="hdfUserId" Value='<%# Eval("UserId")%>' runat="server" />
<%--                                            <asp:HiddenField ID="hdfAttendance" Value='<%# Eval("Attendance")%>' runat="server" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>                              
                                       
                                            <asp:TemplateField  HeaderText="Attendance" >
                                      <ItemTemplate>
                              

                                          <asp:RadioButtonList Enabled="false" SelectedIndex='<%#Convert.ToInt32(Eval("IsMaker"))%>'    CssClass="radio-att" ID="rbAttendance"   runat="server" RepeatDirection="Horizontal" >
                                        <asp:ListItem Value="false" Text="Absent" ></asp:ListItem>
                                              <asp:ListItem Value="true" Text="Present" ></asp:ListItem>
                              
                                
                                 </asp:RadioButtonList>
                                        </ItemTemplate>
                                    </asp:TemplateField>    
                                                                                                                       
                                 </Columns>
                                </asp:GridView>
                                     </td>
                                     </tr>
                                     
                                   
                                        
<%--                                              <tr>
                                     <td colspan="3">
                                     <div class="fullwidth noBorder">
<asp:Button ID="btnSubmit" runat="server"  Text="Submit" CssClass="btnSave" ValidationGroup="a" onclick="btnSubmit_Click"></asp:Button>
</div>
                                    </td>
                                     </tr>    --%>        
                                 </table>
                                 </div>
                                         <div id="divbutton" class="fullwidth noBorder" runat="server" >
                                    <asp:Button CssClass="btnSave" ID="btnExportToExcel" runat="server" Text="Export To Excel" Visible="false" Width="130px"  ValidationGroup="a" onclick="btnExportToExcel_Click"  />
                                       
                                    </div>
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />
                                        <asp:PostBackTrigger ControlID="btnExportToExcel" />
                     
    </Triggers>
</asp:UpdatePanel>
                               </div>
                                   <asp:Label ID="lblAlert" runat="server" ></asp:Label>
                                 <asp:HiddenField ID="hdnPopUp" runat="server" ClientIDMode="Static" Value="0" />
                          
                               <br />
						</fieldset>
                             										
					
				</div>					
				
			</section>			
		</article>
    <div class="clearfix">
    </div>
</asp:Content>
