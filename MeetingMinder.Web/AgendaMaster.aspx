﻿<%@ Page Title=""  Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AgendaMaster.aspx.cs" EnableEventValidation="false"  Inherits="MeetingMinder.Web.AgendaMaster" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<link rel="stylesheet" type="text/css" href="css/jquery-ui.css" />
    <style>
        .ui-widget {
            font-family : Rupee;
        }

        li {
            list-style:none;
        }
        .ui-accordion-content.ui-helper-reset.ui-widget-content.ui-corner-bottom.ui-accordion-content-active {
    height: auto !important;
}
    </style>
  <script type="text/javascript">
     function AddValue() {
         var vals = "";
         $(".group").each(function () {
             vals += this.id + ",";
             id = this.id;
             $("#" + id + " > div ol").find("li").each(function () {
                 vals += this.id + ",";
             });
         });
         $("#hdnListItems").val(vals);
        // alert(vals);
         return true;
     }

     $(function () {
         $("#accordion")
.accordion({
    header: "> ol > li > h3",
    
});
 $("#sort").sortable({
    axis: "y",
    handle: "h3",
    stop: function (event, ui) {
        // IE doesn't register the blur when sorting
        // so trigger focusout handlers to remove .ui-state-focus
        ui.item.children("h3").triggerHandler("focusout");
        $("#divOrder").attr("style", "display:block");
    }
});

$(".inddrag").sortable({
 stop: function (event, ui) {
 $("#divOrder").attr("style", "display:block");
 }});

$(".ddrag").sortable({
 stop: function (event, ui) {
 $("#divOrder").attr("style", "display:block");
 }});
     });

      //Disable sorting
//     $(document).ready(function () {
//    
//     $("#sort").sortable( "disable" );
//      $(".inddrag").sortable( "disable" );
//       $(".ddrag").sortable( "disable" );
//     });

 </script>
<article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Agenda--%></h2>			
			</header>
			<section>              
				<div >
                  <div class="table-form">
						<fieldset>
							<legend><font color="#054a7f"><b>Agenda List</b></font></legend>
                             <dl>
                                <div style="margin-bottom:15px"><userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                               
                                <div style="margin-bottom:15px">
                            
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                              <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="b" />
                                </div>
                                <div>
                               <%--   <asp:GridView ID="grdAgenda" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="MeetingId" OnRowDeleting="grdAgenda_RowDeleting"
                                 
                                    OnPageIndexChanging="grdAgenda_PageIndexChanging" 
                                    onsorting="grdAgenda_Sorting" onrowcommand="grdAgendaRowCommand" 
                                    onrowediting="grdMeeting_RowEditing">
                                  <HeaderStyle HorizontalAlign="Center" BackColor="#AE432E" ForeColor="#F2DEDA" Font-Bold="true" />
                                  <Columns>

                                    <asp:TemplateField>
                                        <ItemStyle  HorizontalAlign="Center" />
                                        <HeaderStyle  HorizontalAlign="Center" />
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkHeader" onclick="javascript: fn_select_all(this);" runat="server" />
                                         </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSubAdmin" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Agenda Name" SortExpression="AgendaName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAgendaName" runat="server" Text='<%# Eval("AgendaName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Sub Agenda" SortExpression="SubAgenda">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubAgenda" runat="server" Text='<%# Eval("SubAgenda") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                                              <asp:TemplateField HeaderText="Agenda">
                <ItemStyle HorizontalAlign="Center" Width="10%" />
                <ItemTemplate>
                <asp:LinkButton ID="lbnState" CommandArgument='<%# Bind("AgendaId") %>' CausesValidation="false" CommandName="View" Text="Add/View" runat="server" ToolTip="Add OR View Agenda"></asp:LinkButton>
                    
                </ItemTemplate>
            </asp:TemplateField>
<%--

                                    
                                     <asp:TemplateField HeaderText=" Email" SortExpression="EmailId1">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClientEmail" runat="server" Text='<%#Eval("EmailID1") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <%--<asp:BoundField DataField="Description"  HeaderText="Description"  />--%>

                               <%--    <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>--%>
                                            <%--<asp:ImageButton ID="imgbtnView" CommandArgument='<%# Bind("MeetingId") %>' ImageUrl="~/img/icons/actions/page_white_find.png" 
                                                    CausesValidation="false" CommandName="View" Text="Edit" runat="server" ToolTip="Edit Item" />                                     --%>
                            <%--                <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Bind("AgendaId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Bind("AgendaId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>--%>
                                </div>
                                  <div style="margin-bottom:15px">
                                 <asp:Label ID="lblList" style="font-family:Arial;" runat="server"></asp:Label>
                                 </div>
                            
                              <div  class="fullwidth noBorder" id="divOrder" style="display:none">
                                
        <asp:Button ID="btnSave" style="width:110px;" CssClass="btnSave" runat="server" Text="Save Order" OnClientClick="AddValue();" 
            OnClick="btnSave_Click"  />

            </div>
                                <div style="display:none;" style="width:900px;">
                                    <asp:HiddenField ID="hdnListItems" ClientIDMode="Static" runat="server" />
                                 <table width="75%">
                                     <tr>
                                     <td style="width:95px;">
                                 Agenda
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                       <asp:TextBox ID="txtAgenda" TextMode="MultiLine" runat="server" Width="70%"></asp:TextBox>
                                  <asp:RequiredFieldValidator runat="server" ID="rfvAgenda" ControlToValidate="txtAgenda" Display="Dynamic" ValidationGroup="a" ErrorMessage="Agenda" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </td>
                                     </tr>                               
                             
                                     <tr align="right">
                                        <td colspan="5" align="right"> 
                                        <asp:Button ID="btnSubmit"  CausesValidation="true" ValidationGroup="a" runat="server" Text="Add Agenda" onclick="btnSubmit_Click"></asp:Button> 
                                      
                                 </td>
                                 </tr>
                                 <tr>
                                 <td  style="width:95px;" colspan="5"><b>Or</b></td>
                                 </tr>

                                  <tr align="right">
                                     <td style="width:95px;">
                                 Agenda
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                      <asp:DropDownList Width="73%" ID="ddlAgenda" runat="server"></asp:DropDownList>
                                  <asp:RequiredFieldValidator runat="server" ID="rfvddlAgenda" ControlToValidate="ddlAgenda" Display="Dynamic" ValidationGroup="b" ErrorMessage="Select Agenda" InitialValue="0" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </td>
                                     </tr>                       
                                      <tr>
                                     <td style="width:95px;">
                                  Sub Agenda
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:TextBox TextMode="MultiLine" ID="txtSubAgenda" runat="server" Width="70%"></asp:TextBox>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvSubAgenda" ControlToValidate="txtSubAgenda" Display="Dynamic" ValidationGroup="b" ErrorMessage="Sub Agenda" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </td>
                                     </tr>                                 
                                     <tr align="right">
                                        <td colspan="5" align="right"> 
                                        <asp:HiddenField ID="hdnAgendaId" runat="server" />
                                        <asp:HiddenField ID="hdnSubAgenda" runat="server" />
                                        <asp:Button ID="btnSubAgenda" CausesValidation="true" ValidationGroup="b" runat="server" Text="Add Subagenda" onclick="btnSubAgendaSubmit_Click"></asp:Button> 
                                        </td></tr>

                                            <tr>
                                 <td  style="width:95px;" colspan="5"><b>Or</b></td>
                                 </tr>
                                  <tr align="right">
                                     <td style="width:95px;">
                                 Upload Agenda
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                    <asp:FileUpload runat="server" ID="fuAgenda"/>
                                  <asp:RequiredFieldValidator runat="server" ID="rfvUploadAgenda" ControlToValidate="ddlAgenda" Display="Dynamic" ValidationGroup="c" ErrorMessage="Select Agenda"  Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </td>
                                     </tr>     
                                       <tr align="right">
                                        <td colspan="5" align="right"> 
                                        <div class="fullwidth noBorder">
                                        <asp:Button CssClass="btnSave" ID="btnUpAgenda" CausesValidation="true" ValidationGroup="c" runat="server" Text="Upload" onclick="btnUpAgendaSubmit_Click"></asp:Button> 
                                        </div>
                                        </td></tr>  
                                 </table>
                               </div>
                                   <br />
                                   <div class="fullwidth noBorder">
            <asp:Button ID="btnBack" CssClass="btnCancel"  runat="server" Text="Back" onclick="btnBack_Click"></asp:Button>  
            </div>			
                               <br />
						</fieldset>
                         							
					</div>									
				</div>					
				
			</section>			
		</article>
    <div class="clearfix">
    </div>
   
</asp:Content>

