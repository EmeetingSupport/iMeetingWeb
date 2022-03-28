<%@ Page Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="CreateMeeting.aspx.cs" Inherits="MeetingMinder.Web.WebForm1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <link href="css/jquery.ui.timepicker.css" rel="stylesheet"      type="text/css" />
     
    <script src="js/jquery/jquery.ui.timepicker.js?v=0.3.3" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
    <script>
        $(function () {
            var MeetVenue = $("#hdnMeetingVenue").val().split(',');
            //"hdnMeetingVenue"

            $("#txtMeetingVenue").autocomplete({
                source: MeetVenue
            });
        });

        $(document).ready(function () {

            if ($('#<%=hdnMeetingId.ClientID %>').val().length > 10) {
                var id = '<%= divdetails.ClientID %>';
                $("html, body").animate({
                    scrollTop: $('#' + id
                        ).offset().top
                }, 1500);
            }
        });
	</script>


    <script language="javascript" type="text/javascript">

        function pageLoad(sender, args) {
            $(function () {
                $("#txtMeetingDate").datepicker({ minDate: 0, maxDate: "+11M +30D", dateFormat: "dd.mm.yy" });

                var MeetVenue = $("#hdnMeetingVenue").val().split(',');
                //"hdnMeetingVenue"

                $("#txtMeetingVenue").autocomplete({
                    source: MeetVenue
                });
            });

            $('#txtTime').timepicker({
                showPeriod: true,
                showLeadingZero: true,
                timeSeparator: '.',
               //  timeFormat: 'hh mm p' 
            });

            function setNotice() {
                if ($("#hdnMeetingId").val().length == 0) {
                    CKEDITOR.instances.txtNotice.setData("Notice is hereby given that the {Meeting_Number} meeting of the <u>{Committee}</u> of <u>{Entity}</u> will be held on <u>{Date}</u> at <u>{Time}</u> in <u>{Venue}.</u> <br/><br/>Kindly make it convenient to attend the meeting.");
                }
            }

            
        }

        function setNotice() {
            if ($("#hdnMeetingId").val().length == 0) {
                CKEDITOR.instances.txtNotice.setData("Notice is hereby given that the {Meeting_Number} meeting of the <u>{Committee}</u> of <u>{Entity}</u> will be held on <u>{Date}</u> at <u>{Time}</u> in <u>{Venue}.</u><br/><br/> Kindly make it convenient to attend the meeting.");
            }
        }

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

        
    </script>
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Meeitng Details--%></h2>			
			</header>
			<section>              
				<div >
                <fieldset>  
                            
							<legend><font color="#054a7f"><b>Meeting List</b></font></legend>
                            <dl>
                             <div >
                                  <asp:UpdatePanel id="ResultsUpdatePanel" runat="server" >   
                                 <ContentTemplate>

                                    <div style="text-align:center;">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel" DynamicLayout="true">
                        <ProgressTemplate>

                           <img src="img/jquery/ajaxLoader.gif" alt="Loading.." />

                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    </div>
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                   <div style="margin-bottom:15px">
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                </div>
                               <div style="display:none;margin-bottom:15px">
                                    <img width="16" height="16" src="img/icons/icon_list_style_cross.png" alt="" style="vertical-align: middle"/> 
                                    <asp:LinkButton runat="server" ID="lbRemoveSelected" Text="Remove All Selected Record" 
                                    OnClientClick="return confirm('Are you sure you want to delete selected items?');" onclick="lbRemoveSelected_Click"  ></asp:LinkButton>
                               </div>
                               <div style="margin-bottom:15px"><asp:Label ID="lblTotalRecord" runat="server" Font-Bold="True"></asp:Label></div>  
                       
                      <div ID="divdetails" runat="server" style="margin-bottom:15px">
      <div style="display:none">
                       <dt>
                                    <label>
                                    Entity <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                   <asp:DropDownList Visible="false" ID="ddlEntity" runat="server" AutoPostBack="true" Width="231px"
                                             onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList>
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Entity" Text="Please Select Entity" ForeColor="Red"></asp:RequiredFieldValidator>--%>  
                                 
                                </dd>
                                </div>

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
                          <div class="box_top">
		
		
<h2 class="icon users">Meeting</h2>
		
	
</div>

                         <div style="margin-bottom:15px">
                                         <asp:GridView ID="grdMeeting" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="MeetingId" OnRowDeleting="grdMeeting_RowDeleting"
                                 
                                    OnPageIndexChanging="grdMeeting_PageIndexChanging" 
                                    onsorting="grdMeeting_Sorting" onrowcommand="grdMeetingRowCommand" 
                                    onrowediting="grdMeeting_RowEditing">
                                              <PagerStyle CssClass="paginate_active"   />
                                 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                  <Columns>

                                    <asp:TemplateField Visible="false">
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

                                       <asp:TemplateField HeaderText="Number"  >
                                        <ItemTemplate>
                                            <asp:Label  style="text-align:justify" Width="100px"  ID="lblNumber" runat="server" Text='<%# Eval("MeetingNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Venue"  >
                                        <ItemTemplate>
                                            <asp:Label  style="text-align:justify" Width="200px"  ID="lblMeetingVenu" runat="server" Text='<%# Eval("MeetingVenue") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText=" Date" >
                                        <ItemTemplate>
                                            <asp:Label  style="text-align:justify" Width="100px"  ID="lblMeetingDate" runat="server"  Text='<%# Convert.ToDateTime(Eval("MeetingDate")).ToString("D") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Time" SortExpression="MeetingTime">
                                        <ItemTemplate>
                                            <asp:Label  style="text-align:justify" Width="150px"  ID="lblMeetingTime" runat="server" Text='<%# Eval("MeetingTime") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                                          

                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                                <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Eval("MeetingId")+","+Eval("ForumId")+","+Eval("EntityId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Eval("MeetingId")+","+Eval("EntityId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                            </div>
                       <dt>
                                    <label>
                                     Venue <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtMeetingVenue" ClientIDMode="Static" onblur="setNotice();" runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnMeetingVenue" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvMeetingVenu" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtMeetingVenue" Display="Dynamic" 
                                        ErrorMessage="Venue" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>

                                 <dt>
                                    <label>
                                     Date <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtMeetingDate" ClientIDMode="Static" runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                            
                                    <asp:RequiredFieldValidator ID="rfvMeetingDate" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtMeetingDate" Display="Dynamic" 
                                        ErrorMessage="Date" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>

                                    <dt>
                                    <label>
                                     Time <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtTime" ClientIDMode="Static"  runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvMeetingTime" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtTime" Display="Dynamic" 
                                        ErrorMessage="Time" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>

                               <dt>
                                    <label>
                                     Number  :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtNumber" ClientIDMode="Static"  runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                  <%--  <asp:RequiredFieldValidator ID="rfvMeetingNumber" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtNumber" Display="Dynamic" 
                                        ErrorMessage="Number" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>--%>
                                </dd>

                            <dt>
                                    <label>
                                     Notice <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                <CKEditor:CKEditorControl ID="txtNotice" ClientIDMode="Static" runat="server" Height="200px" Width="865px" ></CKEditor:CKEditorControl>
                                    <asp:RequiredFieldValidator ID="rfvMeetingNumber" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtNotice" Display="Dynamic" 
                                        ErrorMessage="Notice" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                 </dd>

                                     <dt>
                                    <label>
                                    Send to Checker  :
                                    </label>
                                </dt>
                                <dd>
                                <asp:CheckBox onclick="showChecker();" ID="chkChecker" runat="server" ClientIDMode="Static" />
                                </dd>
                                <div id="divEntity" style="display:none">
                                           <dt>
                                    <label>
                                      Checker <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                              <asp:DropDownList Width="231px" ID="ddlUser" runat="server" ></asp:DropDownList>
                                      <asp:RequiredFieldValidator ID="rfvddUser" runat="server"  Enabled="false"
                                        class="invalid-side-note" ControlToValidate="ddlUser" Display="Dynamic" InitialValue="0"
                                        ErrorMessage="Checker" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                              
                                </dd>
                                      <div>
                                        <dt>
                                    <label>
                                    Notify Checker :
                                    </label>
                                </dt>
                                <dd>
                                 <asp:CheckBox ID="chkNotifyChecker" runat="server"   />                     
                                </dd>
                                         
                                </div>
                                      </div>
                                     
                                    
                                                      
                                    <div class="fullwidth noBorder">
                                        <asp:HiddenField ID="hdnMeetingId" ClientIDMode="Static" runat="server" />
                                    <asp:Button ID="btnInsert" CssClass="btnSave" runat="server" Text="Save" ValidationGroup="a" onclick="btnInsert_Click1"  />
                                       <asp:Button ID="btnCancel" CssClass="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click1"  />
                                    </div>
                                    </div>
                                  </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />
                               

                               <%-- <asp:PostBackTrigger ControlID="grdUploadMinutes" />--%>
                                         <asp:PostBackTrigger ControlID="grdMeeting" />
                               <asp:PostBackTrigger ControlID="btnInsert" />
                               <asp:PostBackTrigger ControlID="btnCancel" />
                                   
    </Triggers>
</asp:UpdatePanel>
                               </div>
                               </dl>
                               </div>
                           
         </fieldset>
                                </section>
                                </article>
</asp:Content>


