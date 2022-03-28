<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="MasterAgenda.aspx.cs" Inherits="MeetingMinder.Web.MasterAgenda" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <script type="text/javascript">
        function ValidateCheckBox(sender, args) {
            var count = 0;
            var items = document.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].type == "checkbox") {
                    if (items[i].checked) {
                        count++;
                    }
                }
            }

            if (count == 0) {
                args.IsValid = false;
            }
            if (count > 0) {
                args.IsValid = true;
            }

        }

        function toggle(toggeldivid, toggeltext) {
            var divelement = document.getElementById(toggeldivid);
            var lbltext = document.getElementById(toggeltext);
            if (divelement.style.display == "block") {
                divelement.style.display = "none";
                lbltext.innerHTML = "+ Show Orders";
            }
            else {
                divelement.style.display = "block";
                lbltext.innerHTML = "- Hide Orders";
            }
        }
    </script>
    <script language="javascript" type="text/javascript">

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

        function testCheckbox() {
            Parent = document.getElementById('cpMain_grdOrder');
            var headchk = document.getElementById('cpMain_grdOrder_chkHeader');
            var items = Parent.getElementsByTagName('input');
            var flg = false;
            for (i = 0; i < items.length; i++) {

                if (items[i] != headchk && items[i].type == "checkbox") {

                    if (items[i].checked) {
                        flg = true;
                    }
                }
            }
            if (flg) {
                var ans = confirm("Are you sure you want to delete selected items?");
                if (ans == true) {
                    return true;
                }
                else {
                    //alert("no");
                    return false;
                }
            }
            else {
                alert("Select item(s) to delete");
                return false;
            }
        }

        $(document).ready(function () {

            if ($('#<%=hdnAgendaId.ClientID %>').val().length > 0) {
                var id = '<%=divdetails.ClientID %>';
                 $("html, body").animate({
                     scrollTop: $('#' + id
                         ).offset().top
                 }, 1500);
             }
         });

    </script>
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Master Agenda--%></h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#054a7f"><b>Master Agenda</b></font></legend>
                            <dl>
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
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
                                                   <div id="ContentPlaceHolder1_divdetails"  style="margin-bottom:15px">
                                       <dt>
                                    <label>
                                     Forum <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:DropDownList ID="ddlForum" Width="231px" runat="server" AutoPostBack="true"
                                        onselectedindexchanged="ddlForum_SelectedIndexChanged" ></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Forum" Text="Invalid"  ForeColor="Red"></asp:RequiredFieldValidator>                                     
                                     </dd>
                                         </div>
                                     <div class="box_top">
		
		
<h2 class="icon users">Master Agenda</h2>
		
	
</div>
                       
                                <div style="margin-bottom:15px">
                               <div class="box_content">
		
		<!-- Simple Sorting Table + Pagination: Start -->
		<div class="dataTables_wrapper">
                                 <asp:GridView ID="grdAgenda" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="AgendaId" OnRowDeleting="grdRoll_RowDeleting"
                                 
                                    OnPageIndexChanging="grdAgenda_PageIndexChanging" 
                                    onsorting="grdAgenda_Sorting" onrowcommand="grdAgenda_RowCommand" 
                                    onrowediting="grdAgenda_RowEditing">
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
                                     <asp:TemplateField HeaderText="Agenda" SortExpression="AgendaName">
                                        <ItemTemplate>
                                        <asp:Label style="text-align:justify"  Font-Names="Rupee" Width="600px"  ID="lblAgenda" runat="server" Text='<%# Eval("AgendaName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Eval("AgendaId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Eval("AgendaId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                                 </div>
                                   </div></div>
                           
           
                     <div ID="divdetails" runat="server" style="margin-bottom:15px">
                    
                       <dt>
                                    <label>
                                    Agenda <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox TextMode="MultiLine"  Font-Names="Rupee"  ID="txtAgenda"  runat="server" MaxLength="149" Width="400px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvUser" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtAgenda" Display="Dynamic" 
                                        ErrorMessage="Agenda" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>
                             
                        
                                     <dl>
                                     <asp:HiddenField ID="hdnAgendaId" runat="server" />
                                     
                                    </dl>
                                    <div id="divbutton" runat="server" class="fullwidth noBorder">
                                    <asp:Button ID="btnInsert" CssClass="btnSave" runat="server" Text="Save" ValidationGroup="a" onclick="btnInsert_Click1"  />
                                       <asp:Button ID="btnCancel" CssClass="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click1"  />
                                    </div>
                                    </div>
                                        </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                                        <asp:PostBackTrigger ControlID="ddlForum" />
                                    <asp:PostBackTrigger ControlID="btnInsert" />
                               <asp:PostBackTrigger ControlID="btnCancel" />
                               <asp:PostBackTrigger ControlID="grdAgenda" />
                             
                                   
    </Triggers>
</asp:UpdatePanel>
                        </dl>
                            </fieldset>         </div>
                                </section>
                                </article>
</asp:Content>
