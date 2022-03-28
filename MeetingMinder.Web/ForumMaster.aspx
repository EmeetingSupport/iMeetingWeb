<%@ Page Title="Forum Master" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ForumMaster.aspx.cs" Inherits="MeetingMinder.Web.ForumMaster" %>
<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <script language="javascript" type="text/javascript">
        function SelectCheckBox() {
          
            var numChecked = $("#<%= grdForum.ClientID %>  [type=checkbox]:input[id*='chkSubAdmin']:checked").length;
            var numTotal = $("#<%= grdForum.ClientID %>    [type=checkbox]:input[id*='chkSubAdmin'] ").length;
            if (numTotal == numChecked) {
                  
                  $("input[id*='chkHeader']").attr('checked', true);                
            }
            else {
                
                $("input[id*='chkHeader']").attr('checked', false);              
            }
        }

     $(document).ready(function () {
         $(document).bind('drop dragover', function (e) {
             e.preventDefault();
         });

         if ($('#<%=hdnForumId.ClientID %>').val().length > 10) {
             var id = '<%= divdetails.ClientID %>';
                 $("html, body").animate({
                     scrollTop: $('#' + id
                         ).offset().top
                 }, 1500);
             }
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

     function showChecker() {
         if (document.getElementById('chkChecker').checked) {
             $("#divEntity").attr("style", "display:block");
             var validatorObject = document.getElementById('<%=rfvddUser.ClientID%>');
             validatorObject.enabled = true;
             validatorObject.isvalid = true;
             ValidatorUpdateDisplay(validatorObject);
         }
         else {
             $("#divEntity").attr("style", "display:none");
             var validatorObject = document.getElementById('<%=rfvddUser.ClientID%>');
             validatorObject.enabled = false;
         }
     }

             function CheckCounts() {

            if ($('#<%= grdForum.ClientID %> tr:not(:first-child) td:first-child').find('input[type="checkbox"]:checked').length != 0) {
                return confirm('Are you sure you want to delete selected items?');
            }
            else {
                alert("Please Select at least one checkbox");
                return false;
            }
        }
    </script>
    <article class="content-box minimizer">
			<header>			
				<h2>Forum Details</h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#054a7f"><b>Forum list</b></font></legend>
                            <dl>
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                 
                               <div style="margin-bottom:15px">
                                    <img width="16" height="16" src="img/icons/icon_list_style_cross.png" alt="" style="vertical-align: middle"/> 
                                    <asp:LinkButton runat="server" ID="lbRemoveSelected" Text="Remove All Selected Record" 
                                    OnClientClick="return CheckCounts();" onclick="lbRemoveSelected_Click"  ></asp:LinkButton>
                                  |&nbsp;<div style="display:none"> <asp:HyperLink ID="hlChangeOrder" Visible="false" Text="Change Order" runat="server" ></asp:HyperLink>
<%--<a href="Reorder.aspx?for=1" >Change Order</a>--%></div>
                               </div>
                               <div style="margin-bottom:15px"><asp:Label ID="lblTotalRecord" runat="server" Font-Bold="True"></asp:Label></div>  
                               <%--<div style="margin-bottom:15px"> <asp:Label  ID="lblUser" runat="server" Text="Search User" Font-Bold="true" ></asp:Label> </div>  --%>
                                  
                                
                                   <div class="box_top">
		
		
<h2 class="icon users">Forum </h2>
		
	
</div>                    
                              <div style="margin-bottom:15px">
                                     <div class="box_content">
		<div class="dataTables_wrapper">
                                         <asp:GridView ID="grdForum" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="ForumId" OnRowDeleting="grdForum_RowDeleting"
                                 
                                    OnPageIndexChanging="grdForum_PageIndexChanging" 
                                    onsorting="grdForum_Sorting" onrowcommand="grdForum_RowCommand" 
                                    onrowediting="grdForum_RowEditing">
                                 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                              <PagerStyle CssClass="paginate_active"   />
                                  <Columns>

                                    <asp:TemplateField>
                                        <ItemStyle  HorizontalAlign="Center" />
                                        <HeaderStyle  HorizontalAlign="Center" />
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkHeader" onclick="javascript: fn_select_all(this);" runat="server" />
                                         </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox onchange="SelectCheckBox();" ID="chkSubAdmin" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Forum Name" SortExpression="ForumName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblForumName" runat="server" Text='<%# Eval("ForumName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Meeting" Visible="false">
                <ItemStyle HorizontalAlign="Center" Width="10%" />
                <ItemTemplate>
                <asp:LinkButton ID="lbnState" CommandArgument='<%# Bind("ForumId") %>' CausesValidation="false" CommandName="View" Text="Add/View" runat="server" ToolTip="Add OR View Meetings"></asp:LinkButton>
                    
                </ItemTemplate>
            </asp:TemplateField>
<%--

                                     <asp:TemplateField HeaderText="Contact Number" SortExpression="Mobile">
                                        <ItemTemplate>
                                            <asp:Label ID="lblContactNumber" runat="server" Text='<%#Eval("Mobile") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    
                                     <asp:TemplateField HeaderText=" Email" SortExpression="EmailId1">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClientEmail" runat="server" Text='<%#Eval("EmailID1") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <%--<asp:BoundField DataField="Description"  HeaderText="Description"  />--%>

                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                          <%--  <asp:ImageButton ID="imgbtnView" CommandArgument='<%# Bind("ForumId") %>' ImageUrl="~/img/icons/actions/page_white_find.png" 
                                                    CausesValidation="false" CommandName="View" Text="Edit" runat="server" ToolTip="Edit Item" />                             --%>        
                                            <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Eval("ForumId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Eval("ForumId")+","+Eval("MembersInfo")%>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
            </div>
                                         </div>
                            </div>

                            </dl>
                          
           
                     <div ID="divdetails" runat="server" style="margin-bottom:15px">
                    
                       <dt>
                                    <label>
                                   Forum Name <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtForumName" runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvForumName" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtForumName" Display="Dynamic" 
                                        ErrorMessage="Forum Name" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>
                                   <dt>
                                    <label>
                                   Forum Short Name<%-- <span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtShortName" runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvShortName" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtShortName" Display="Dynamic"  Enabled="false"
                                        ErrorMessage="Forum Name" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>

                                    <dt style="display:none">
                                    <label>
                                    Enable <span>*</span> :
                                    </label>
                                </dt>
                                <dd style="display:none">
                                 <asp:CheckBox ID="chkEnable"  runat="server" Checked="false" />
                                  
                                </dd>

                                    <dt  style="display:none">
                                    <label>
                                    Members Info :
                                    </label>
                                </dt>
                                <dd  style="display:none">
                                 <asp:FileUpload ID="fuMemberInfo"  runat="server" />     
                                   <asp:LinkButton ID="lnkView" runat="server" Visible="false" Text="View" onclick="lnkView_Click"></asp:LinkButton>
                                    &nbsp; &nbsp;   <asp:LinkButton ID="lnkDelete" runat="server" Visible="false" Text="Delete" onclick="lnkDelete_Click"></asp:LinkButton>                            
                                   <br /> <label><span>&nbsp;Pdf file only</span></label>
                                </dd>

                                 <dt>
                                    <label>
                                    Send to Checker  :
                                    </label>
                                </dt>
                                <dd>
                                <asp:CheckBox onclick="showChecker();" ID="chkChecker" runat="server" ClientIDMode="Static" />
                                </dd>
                                 <div id="divEntity" runat="server" clientidmode="Static" style="display:none">
                                      <dt>
                                    <label>
                                    Forum Checker <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                              <asp:DropDownList ID="ddlUser" runat="server" ></asp:DropDownList>
                                      <asp:RequiredFieldValidator ID="rfvddUser" runat="server" Enabled="false"
                                        class="invalid-side-note" ControlToValidate="ddlUser" Display="Dynamic" InitialValue="0"
                                        ErrorMessage="Checker" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                              
                                </dd>
                                        <dt>
                                    <label>
                                    Notify Checker   :
                                    </label>
                                </dt>
                                <dd>
                                 <asp:CheckBox ID="chkNotifyChecker" runat="server"   />                     
                                </dd>
                                </div>
                                     <dl>
                                     <asp:HiddenField ID="hdnForumId" runat="server" />
                                    </dl>
                                    <div id="divbutton" runat="server" class="fullwidth noBorder" style="margin-bottom:15px;margin-top:15px">
                                    <asp:Button CssClass="btnSave" ID="btnInsert" runat="server" Text="Save" ValidationGroup="a" onclick="btnInsert_Click1"  />
                                       <asp:Button CssClass="btnCancel" ID="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click1"  />
                                    </div>
                                    </div>
                      </fieldset>
                                </div>
                                </section>
                                </article>

</asp:Content>
