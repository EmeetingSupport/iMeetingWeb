<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="UploadMinutes.aspx.cs" Inherits="MeetingMinder.Web.UploadMinutes" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        $(document).bind('drop dragover', function (e) {
            e.preventDefault();
        });

        if ($('#<%=hdnUploadId.ClientID %>').val().length > 0) {
            var id = 'divData';
             $("html, body").animate({
                 scrollTop: $('#' + id
                     ).offset().top
             }, 1500);
         }
    });

    $(function () {
        $("#txtMeetingDate").datepicker({ minDate: 0, maxDate: "+2M +10D", dateFormat: "mm/dd/yy" });
    });

    function fn_select_all(chkSelectAll) {
        var IsChecked = chkSelectAll.checked;
        var items = document.getElementsByTagName('input');
        for (i = 0; i < items.length; i++) {
            if (items[i] != chkSelectAll && items[i].type == "checkbox") {
                if (items[i].checked != IsChecked) {
                    items[i].click();
                }
            }
        }
    }

    function showChecker() {
        if (document.getElementById('chkChecker').checked) {
            $("#divEntity").attr("style", "display:table-row");
            var validatorObject = document.getElementById('<%=rfvddUser.ClientID%>');
            validatorObject.enabled = true;
            validatorObject.isvalid = true;
            ValidatorUpdateDisplay(validatorObject);

            $("#divCheckerEntity").attr("style", "display:table-row");
        }
        else {
            $("#divCheckerEntity").attr("style", "display:none");
            $("#divEntity").attr("style", "display:none");
            var validatorObject = document.getElementById('<%=rfvddUser.ClientID%>');
            validatorObject.enabled = false;
        }
    }
</script>

<article class="content-box minimizer">
			<header>			
				<h2> &nbsp; <%--Upload Minutes--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font  color="#054a7f"><b>Upload Minutes</b></font></legend>
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
                              <div>
                                  <div class="box_top">
		
		
<h2 class="icon users">Upload Minutes </h2>
		
	
</div>
                                   <div class="box_content">
		<div class="dataTables_wrapper">
                                   <asp:GridView ID="grdUploadMinutes" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="UploadMinuteId"                                
                                    onsorting="grdUploadMinutes_Sorting" onrowcommand="grdUploadMinutesRowCommand" 
                                    onpageindexchanging="grdUploadMinutes_PageIndexChanging" 
                                       onrowediting="grdUploadMinutes_RowEditing" 
                                       onrowdeleting="grdUploadMinutes_RowDeleting">
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

                                    <asp:TemplateField HeaderText="Meeting" SortExpression="MeetingVenue" >
                                        <ItemTemplate>
                                          <asp:Label ID="lblTe"  style="text-align:justify"  runat="server"
                                               Text='<%# Eval("MeetingNumber") +" "+ Eval("ForumName")+" "+ Convert.ToDateTime(Eval("MeetingDate")).ToString("MMMM dd, yyyy") %>' ></asp:Label>
                                            <%--Text='<%# Convert.ToDateTime(Eval("MeetingDate")).ToString("MMM d yyyy") +" "+ Eval("MeetingTime") +" "+ Eval("MeetingVenue") %>'--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <%--      <asp:TemplateField HeaderText="Forum" SortExpression="ForumName" >
                                        <ItemTemplate>
                                          <asp:Label ID="lblForum"  style="text-align:justify" runat="server" Text='<%#Eval("ForumName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="Created By" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncrptionkey" runat="server" Visible="false" Text='<%#Eval("EncryptionKey") %>'></asp:Label>
                                            <asp:Label ID="lblName"  style="text-align:justify" runat="server" Text='<%#Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Minutes" >
                                        <ItemTemplate>
                                           <asp:LinkButton ID="lbnApprove" CommandArgument='<%# Bind("UploadFile") %>' CausesValidation="false" CommandName="view" Text="View" runat="server" ToolTip="Approve"></asp:LinkButton>
                                           <%--&nbsp; &nbsp;  <asp:LinkButton ID="lbnReject" CommandArgument='<%# Bind("EntityId") %>' CausesValidation="false" CommandName="Decline" Text="Decline" runat="server" ToolTip="Decline"></asp:LinkButton>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                          <asp:ImageButton ID="lbtnEdit" CommandArgument='<%#  Eval("UploadMinuteId")+","+Eval("MeetingDate", "{0:dd/MM/yyyy}" ) +" "+ Eval("MeetingTime") +" "+ Eval("MeetingVenue")+","+Eval("ForumName")+","+Eval("EntityId")+","+ Eval("MeetingId")+","+ Eval("ForumId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Eval("UploadMinuteId")+","+Eval("EntityId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                                                
                                 </Columns>
                                </asp:GridView>
            </div>
                                       </div>
                              </div>
                              <br /><br />
                                       <div id="divData" style="width:900px;">
                                 <table width="75%">

                                    <tr style="display:none;" >
                                     <td  style="width:115px;">
                                    Department<span>&nbsp;*</span>
                                     </td>
                                     <td  style="width:20px;">
                                     :
                                     </td>
                                     <td   align="left">
                                     <asp:DropDownList Visible="false" ID="ddlEntity" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Department" Text="Please Select Department" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>
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
                                         <asp:DropDownList ID="ddlMeeting" runat="server" Width="50%"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Meeting" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                 
                                    <%-- <asp:TextBox ID="txtDate" ClientIDMode="Static" runat="server" Width="70%"></asp:TextBox>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvNew" ControlToValidate="txtDate" Display="Dynamic" ValidationGroup="a" ErrorMessage="Answer" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
     --%>                                </td>
                                     </tr>

                                       <tr>
                                     <td>
                                   Upload 
                                         <label style="display:inline;margin:-2px;"><span>&nbsp;* </span></label>(<span style="color:red;">Pdf file only</span>)
                                     </td>
                                     <td >
                                     :
                                     </td>
                                     <td   align="left">
                                     <asp:FileUpload ID="fuMinutes" style="width:270px" runat="server"  />
                                   <%-- <br /> <label><span>&nbsp;Pdf file only</span></label>--%>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvMinutes" ControlToValidate="fuMinutes" Display="Dynamic" ValidationGroup="a" ErrorMessage="Upload Minutes" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     <asp:LinkButton ID="lnkView" runat="server" Visible="false" Text="View" 
                                             onclick="lnkView_Click"></asp:LinkButton>
                                     </td>
                                     </tr>
                                 <tr>
                                     <td>
                                    Send to Checker  :
                                    </td>
                                <td >
                                     :
                                     </td>
                                     <td   align="left">
                                <asp:CheckBox onclick="showChecker();" ID="chkChecker" runat="server" ClientIDMode="Static" />
                                </td>
                                </tr>
                                 <tr id="divEntity" runat="server" clientidmode="Static" style="display:none">
                                     <td>
                                    Checker <span>&nbsp;*</span>
                                     </td>
                                     <td >
                                     :
                                     </td>
                                     <td   align="center">
                                    <asp:DropDownList ID="ddlUser" runat="server"  Width="50%"></asp:DropDownList>
                                      <asp:RequiredFieldValidator ID="rfvddUser" runat="server" Enabled="false"
                                     ControlToValidate="ddlUser" Display="Dynamic" InitialValue="0" ForeColor="Red"
                                        ErrorMessage="Checker" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                     </td>
                                     </tr>
                                              <tr id="divCheckerEntity" runat="server" clientidmode="Static" style="display:none">
                                     <td >
                                    Notify Checker 
                                     </td>
                                     <td>
                                     :
                                     </td>
                                     <td align="center">
                                      <asp:CheckBox ID="chkNotifyChecker" runat="server"   />      

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
                                 </div>
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />

                                <asp:PostBackTrigger ControlID="grdUploadMinutes" />
                               <asp:PostBackTrigger ControlID="btnSubmit" />
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
    <div class="clearfix">
    </div>

</asp:Content>
