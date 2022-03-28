<%@ Page Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="CreateMeeting.aspx.cs" Inherits="MeetingMinder.Web.WebForm1" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="css/jquery.ui.timepicker.css" rel="stylesheet" type="text/css" />--%>

    <script src="js/jquery/jquery.ui.timepicker.js?v=0.3.3" type="text/javascript"></script>

    <link href="SimplpanAdminPanelfiles/theme-jquery-ui-1.12.1.css" rel="stylesheet" />

    <style type="text/css">
        .ui-widget-header {
            border: 1px solid #aaaaaa;
            background: #cccccc url(../images/ui-bg_highlight-soft_75_cccccc_1x100.png) 50% 50% repeat-x;
            color: #222222;
            font-weight: bold;
        }
    </style>

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

            var i = 0;
            $('#<%=txtEditReason.ClientID%>').on('keypress', function () {
                if (this.value.length >= 150) {
                    if (i == 0) {
                        $(this).after(" <span style='color:red'>You have exceeded the maximum character limit.  150</span>");
                        i++;
                    }
                    return false;
                } else {
                    return true
                }
            });
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
                    //CKEDITOR.instances.txtNotice.setData("No.: <br/><br<br/> Respected  Sir/Madam,<br/><br/>It is proposed to hold the next Meeting of the <b><u>{Committee}</u></b> on {Date} at <b><u>{Time}</u></b> at <b><u>{Venue}</u></b>.<br/><br/>2.  We request you to kindly make it convenient to attend the Meeting.<br/><br/>3.  The Agenda Papers for the Meeting of the <b><u>{Committee}</b></u> have been uploaded.<br/><br/><br/>Regards<br/><br/>General Manager & <br/>Secretary, Central Board");
                    CKEDITOR.instances.txtNotice.setData("No.: <b>{Meeting_Number}</b> <br/><br<br/> Respected  Sir/Madam,<br/><br/>It is proposed to hold the next Meeting of the <b><u>{Committee}</u></b> on {Date} at <b><u>{Time}</u></b> at <b><u>{Venue}</u></b>.<br/><br/>2.  We request you to kindly make it convenient to attend the Meeting.<br/><br/>3.  The Agenda Papers for the Meeting of the <b><u>{Committee}</b></u> have been uploaded.<br/><br/><br/>Regards,<br/><b>{Committee}</b>");
                    var header = document.getElementById('<%= hdnHeader.ClientID%>').value;
                    CKEDITOR.instances.txtHeader.setData(header);
                }
            }


        }

        function setNotice() {

            if ($("#hdnMeetingId").val().length == 0) {
                //CKEDITOR.instances.txtNotice.setData("No.: <br/><br/><br/> Respected  Sir/Madam,<br/><br/>It is proposed to hold the next Meeting of the <b><u>{Committee}</u></b> on {Date} at <b><u>{Time}</u></b> at <b><u>{Venue}</u></b>.<br/><br/>2.  We request you to kindly make it convenient to attend the Meeting.<br/><br/>3.  The Agenda Papers for the Meeting of the <b><u>{Committee}</b></u> have been uploaded.<br/><br/><br/>Regards<br/><br/><br/>General Manager & <br/>Secretary, Central Board");
                CKEDITOR.instances.txtNotice.setData("No.: <b>{Meeting_Number}</b><br/><br/><br/> Respected  Sir/Madam,<br/><br/>It is proposed to hold the next Meeting of the <b><u>{Committee}</u></b> on {Date} at <b><u>{Time}</u></b> at <b><u>{Venue}</u></b>.<br/><br/>2.  We request you to kindly make it convenient to attend the Meeting.<br/><br/>3.  The Agenda Papers for the Meeting of the <b><u>{Committee}</b></u> have been uploaded.<br/><br/><br/>Regards,<br/><b>{Committee}</b>");
                var header = document.getElementById('<%= hdnHeader.ClientID%>').value;
                CKEDITOR.instances.txtHeader.setData(header);
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
                $("#divEntity").attr("style", "display:table-row");
                $("#divEntity1").attr("style", "display:table-row");
                var validatorObject = document.getElementById('<%=rfvddUser.ClientID%>');
                validatorObject.enabled = true;
                validatorObject.isvalid = true;
                ValidatorUpdateDisplay(validatorObject);
            }
            else {
                $("#divEntity").attr("style", "display:none");
                $("#divEntity1").attr("style", "display:none");
                var validatorObject = document.getElementById('<%=rfvddUser.ClientID%>');
                validatorObject.enabled = false;
            }
        }

        function confirmMessage() {
            var conf = prompt("Are you sure you want to delete this record? State reason (If any) :");
            if (conf != null) {
                if (conf.length >= 150) {
                    alert('You have exceeded the maximum character limit.  150');
                    return false;
                } else {
                    $('#<%=hdnDeleteReason.ClientID%>').val(conf);
                    return true;
                }
            } else {
                return false;
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
                                            <asp:HiddenField id="hdnHeader" runat="server" />
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

                                     <br/>
                             <br/>
                          <div class="box_top">
		
		
<h2 class="icon users">Meeting</h2>
		
	
</div>

                         <div style="margin-bottom:0px">
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
                                            <%--<asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Eval("MeetingId")+","+Eval("EntityId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />--%>
                                             <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Eval("MeetingId")+","+Eval("EntityId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirmMessage()' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                            </div>
                       <div id="" style="width:100%;">
                    
<table width="100%">
<tbody>


						<tr>

                       <td style="width:115px;"> Venue <span>*</span> </td>
                       <td style="width:20px;">: </td>
                                  
                               <td align="left">
                                    <asp:TextBox ID="txtMeetingVenue" ClientIDMode="Static" onblur="setNotice();" runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnMeetingVenue" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvMeetingVenu" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtMeetingVenue" Display="Dynamic" 
                                        ErrorMessage="Venue" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </td>
                                </tr>

                                <tr>
                                <td style="width:115px;"> Date <span>*</span> </td>
                                <td style="width:20px;">: </td>
                                <td align="left">
                                    <asp:TextBox ID="txtMeetingDate" ClientIDMode="Static" runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                            
                                    <asp:RequiredFieldValidator ID="rfvMeetingDate" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtMeetingDate" Display="Dynamic" 
                                        ErrorMessage="Date" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </td>
                                	</tr>
                                    

                                	<tr>
                                   <td style="width:115px;"> Time <span>*</span></td>
                                   <td style="width:20px;">: </td>
                              
                               <td align="left">
                                    <asp:TextBox ID="txtTime" ClientIDMode="Static"  runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvMeetingTime" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtTime" Display="Dynamic" 
                                        ErrorMessage="Time" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </td>
                                </tr>

                                <tr>
                               <td style="width:115px;"> Number 
                                   
                               </td>
                                <td style="width:20px;">: </td>
                                <td align="left">
                                    <asp:TextBox ID="txtNumber" ClientIDMode="Static"  runat="server" MaxLength="10" Width="214px"></asp:TextBox>
                                  <%--  <asp:RequiredFieldValidator ID="rfvMeetingNumber" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtNumber" Display="Dynamic" 
                                        ErrorMessage="Number" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>--%>
                                </td>
                                </tr>
                                 <%--<tr>
                                    <td style="width:115px;">General Manager <span>*</span></td>
                                    <td style="width:20px;">: </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtGM" runat="server" MaxLength="149" Width="214px" Enabled="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvGM" runat="server" 
                                                                    class="invalid-side-note" ControlToValidate="txtGM" Display="Dynamic" 
                                                                    ErrorMessage="Enter General Manager Name" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                        <div class="fullwidth">
                                        <asp:Button ID="btnChangeGM" BorderStyle="Groove" runat="server" Text="Change General Manager" Visible="false" OnClick="btnChangeGM_Click" />
                                            <asp:Button Visible="false" Text="Save" ID="btnSave" runat="server" OnClick="btnSave_Click" />
                                            <asp:Button Visible="false" ID="btnCancel1" runat="server" Text="Cancel" OnClick="btnCancel1_Click"/>
                                        </div>
                                    </td>
                                </tr>--%>

     <tr>
                               <td style="width:115px;"> Convenor <%--<span>*</span>--%>
                                   
                               </td>
                                <td style="width:20px;">: </td>
                                <td align="left">
                                   <asp:TextBox ID="txtConvenorName" ClientIDMode="Static"  runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                   <asp:ListBox ID="ddlConvenor" SelectionMode="Multiple"  Width="240px" runat="server" Visible="false"></asp:ListBox>
                                  <%--<asp:RequiredFieldValidator runat="server" ID="rfvUser" ControlToValidate="ddlConvenor" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select User" InitialValue="0" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                </td>
                                </tr>

                                <tr id="trEdit" runat="server" visible="false">
                                    <td>
                                        Edit Reason
                                    </td>
                                    <td style="width:115px;">: </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtEditReason" runat="server" TextMode="MultiLine" Width="214px"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                             <td style="width:115px;">Notice <span>*</span>
                                
                             </td>
                              <td style="width:20px;">: </td>

                               <td align="left">  
                                <CKEditor:CKEditorControl ID="txtNotice" ClientIDMode="Static" runat="server" Height="200px" Width="865px" ></CKEditor:CKEditorControl>
                                    <asp:RequiredFieldValidator ID="rfvMeetingNumber" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtNotice" Display="Dynamic" 
                                        ErrorMessage="Notice" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                      <div style="float:left;font-size:12px; width:216px; margin-top:20px;">
                              <b>Place Holders</b><br />
                              1. Meeting Number : {Meeting_Number}<br />
                              2. Committee : {Committee}<br />
                              3. Entity : {Entity}<br />
                              4. Date : {Date}<br />
                              5. Time : {Time}<br />
                              6. Venue : {Venue}<br />
                              <%--7. General Manager: {General Manager}<br />--%>
                          </div>
                                        
                                 </td>
                                 </tr>
     <tr>
                             <td style="width:115px;">Header </td>
                              <td style="width:20px;">: </td>

                               <td align="left">
                                <CKEditor:CKEditorControl ID="txtHeader" ClientIDMode="Static" runat="server" Height="200px" Width="865px" ></CKEditor:CKEditorControl>
                                  
                                 </td>
                                 </tr>

      <tr style="display:none;">
                               <td style="width:115px;"> Meeting Type </td>
                                <td style="width:20px;">: </td>
                                <td align="left">
                                    <asp:DropDownList Visible="false" ID="ddlMeetingType" ClientIDMode="Static" runat="server"  Width="240px" >
                                       <asp:ListItem Value="0" Text="Select" ></asp:ListItem>
                                        <asp:ListItem Value="CB" Text="CB" ></asp:ListItem>
                                         <asp:ListItem Value="ACB" Text="ACB" ></asp:ListItem>
                                         <asp:ListItem Value="ECCB" Text="ECCB" ></asp:ListItem>
                                         <asp:ListItem Value="RMCB" Text="RMCB" ></asp:ListItem>
                                         <asp:ListItem Value="ITSC" Text="ITSC" ></asp:ListItem>
                                         <asp:ListItem Value="CSCB" Text="CSCB" ></asp:ListItem>
                                         <asp:ListItem Value="CSRC" Text="CSRC" ></asp:ListItem>
                                         <asp:ListItem Value="SCBMF" Text="SCBMF" ></asp:ListItem>
                                         <asp:ListItem Value="SRC" Text="SRC" ></asp:ListItem>
                                         
                                    </asp:DropDownList>
                                  <%--  <asp:RequiredFieldValidator ID="rfvMeetingNumber" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtNumber" Display="Dynamic" 
                                        ErrorMessage="Number" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>--%>
                                </td>
                                </tr>
                                 <tr>
                                <td style="width:214px;">Send to Checker</td>
                                 <td style="width:20px;">: </td>
                                <td align="left">
                                <asp:CheckBox onclick="showChecker();" ID="chkChecker" runat="server" ClientIDMode="Static" />
                                </td>
                                </tr>



                                 

                                <tr id="divEntity" runat="server" clientidmode="Static" style="display:none">
                                        <td style="width:115px;">Checker <span>*</span> </td>
                                        <td style="width:20px;">: </td>
                                <td align="left">
                              <asp:DropDownList Width="231px" ID="ddlUser" runat="server" ></asp:DropDownList>
                                      <asp:RequiredFieldValidator ID="rfvddUser" runat="server"  Enabled="false"
                                        class="invalid-side-note" ControlToValidate="ddlUser" Display="Dynamic" InitialValue="0"
                                        ErrorMessage="Checker" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                              
                                </td>

                                </tr>
                               


                                      <tr id="divEntity1" runat="server" clientidmode="Static"  style="display:none">
                                       <td style="width:115px;">Notify Checker</td>
                        				<td style="width:20px;">: </td>
                               <td align="left">
                                 <asp:CheckBox ID="chkNotifyChecker" runat="server"   />                     
                                </td>
                                </tr>
                                          
                                      
                                     
                                    
                                     <tr align="right">           
                                     <td align="right" colspan="5"> 
                                    <div class="fullwidth noBorder">
                                        <asp:HiddenField ID="hdnMeetingId" ClientIDMode="Static" runat="server" />
                                    <asp:Button ID="btnInsert" CssClass="btnSave" runat="server" Text="Save" ValidationGroup="a" onclick="btnInsert_Click1"  />
                                       <asp:Button ID="btnCancel" CssClass="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click1"  />
                                        <asp:HiddenField ID="hdnDeleteReason" runat="server" />
                                    </div>
                                    </td>
									</tr>


                                    </tbody>
									</table>
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


