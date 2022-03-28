<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="RoleMaster.aspx.cs" Inherits="MeetingMinder.Web.RollMaster" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function SelectCheckBox() {

            var numChecked = $("#<%= grdRoll.ClientID %>  [type=checkbox]:input[id*='chkSubAdmin']:checked").length;
              var numTotal = $("#<%= grdRoll.ClientID %>    [type=checkbox]:input[id*='chkSubAdmin'] ").length;
              if (numTotal == numChecked) {

                  $("input[id*='chkHeader']").attr('checked', true);
              }
              else {

                  $("input[id*='chkHeader']").attr('checked', false);
              }
          }

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

          $(document).ready(function () {

              if ($('#<%=hdnApprovalId.ClientID %>').val().length > 0) {
            var id = '<%= divdetails.ClientID %>';
            $("html, body").animate({
                scrollTop: $('#' + id
                    ).offset().top
            }, 1500);
        }
    });
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


        function CheckCounts() {
            if ($('#<%= grdRoll.ClientID %> tr:not(:first-child) td:first-child').find('input[type="checkbox"]:checked').length != 0) {
                return confirm('Are you sure you want to delete selected items?');
            }
            else {
                alert("Please Select at least one checkbox");
                return false;
            }
        }
    </script>

    <style type="text/css">
        .gradeA.odd > td {
            min-width: 80px !important;
        }
    </style>
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Role Details--%></h2>			
			</header>
			<section >              
				<div >
                <fieldset>
							<legend><font color="#054a7f"><b>Role List</b></font></legend>
                            <dl>
                                 
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                 
                               <div class="add_remove" style="margin-bottom:15px">
                                    <img width="16" height="16" src="img/icons/icon_list_style_cross.png" alt="" style="vertical-align: middle"/> 
                                    <asp:LinkButton runat="server" ID="lbRemoveSelected" Text="Remove All Selected Record" 
                                    OnClientClick="return CheckCounts();" onclick="lbRemoveSelected_Click"  ></asp:LinkButton>
                               </div>
                               <div style="margin-bottom:15px"><asp:Label ID="lblTotalRecord" runat="server" Font-Bold="True"></asp:Label>
                               </div>  
                             <%--  <div style="margin-bottom:15px"> <asp:Label  ID="lblUser" runat="server" Text="Search User" Font-Bold="true" ></asp:Label> </div>  --%>
                                 <div class="box_top">
		
		
<h2 class="icon users">Role Master </h2>
		
	
</div>                     
                              <div style="margin-bottom:15px">
                               <div class="box_content">	
	 
		<div class="dataTables_wrapper">
                                 <asp:GridView ID="grdRoll" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    PageSize="10" DataKeyNames="RollMasterId" OnRowDeleting="grdRoll_RowDeleting"
                                 
                                    OnPageIndexChanging="grdRoll_PageIndexChanging" 
                                    onsorting="grdRoll_Sorting" onrowcommand="grdRoll_RowCommand" 
                                    onrowediting="grdRoll_RowEditing">
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
                                            <asp:CheckBox ID="chkSubAdmin" onchange="SelectCheckBox();" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Role Name" SortExpression="RollName">
                                        <ItemTemplate>
                                        <asp:Label ID="chkRollName" runat="server" Text='<%# Eval("RollName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Master">
                                        <ItemTemplate>
                                        <asp:CheckBox ID="chkMaster" ToolTip='<%#((DataBinder.Eval(Container.DataItem,"EntityMaster")).ToString() == "True" ? ("Entity Master\n") : ("" ))+ " " +((DataBinder.Eval(Container.DataItem,"UserMaster")).ToString() == "True" ? ("User Master\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"AccessRightMaster")).ToString() == "True" ? ("AccessRight Master\n") : ("" )) + " " + ((DataBinder.Eval(Container.DataItem,"RollMaster")).ToString() == "True" ? ("Roll Master\n") : ("" ))  %>' Enabled="false" style="background-color:#F2F2F2" runat="server" Checked='<%# GetTranDetails(Convert.ToBoolean(Eval("EntityMaster")),Convert.ToBoolean(Eval("UserMaster")),Convert.ToBoolean(Eval("AccessRightMaster")),Convert.ToBoolean(Eval("RollMaster"))) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                      <asp:TemplateField HeaderText="Transaction" >
                                        <ItemTemplate>
                                        <asp:CheckBox ID="chkTransaction" ToolTip='<%#((DataBinder.Eval(Container.DataItem,"Meeting")).ToString() == "True" ? ("Meeting\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"Agenda")).ToString() == "True" ? ("Agenda\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"UploadMinutes")).ToString() == "True" ? ("Upload Minutes\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"MasterAgenda")).ToString() == "True" ? ("Master Agenda\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"BoardofDirectore")).ToString() == "True" ? ("Board of Director\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"PhotoGallery")).ToString() == "True" ? ("Photo Gallery\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"MeetingRetention")).ToString() == "True" ? ("Meeting Retention\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"GlobalSetting")).ToString() == "True" ? ("Global Setting\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"IpadNotification")).ToString() == "True" ? ("Ipad Notification\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"EmailNotification")).ToString() == "True" ? ("Email Notification\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"AboutUs")).ToString() == "True" ? ("About Us\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"DeviceManager")).ToString() == "True" ? ("Device Manager\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"PublishHistory")).ToString() == "True" ? ("Publish History\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"EmailHistory")).ToString() == "True" ? ("Email History\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"RevisedPdf")).ToString() == "True" ? ("Agenda Access\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"SectionMaster")).ToString() == "True" ? ("Section Master\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"HeaderMaster")).ToString() == "True" ? ("Header Master\n") : ("" ))+ " "+ ((DataBinder.Eval(Container.DataItem,"TabSetting")).ToString() == "True" ? ("Tabs Setting\n") : ("" )) + " "+ ((DataBinder.Eval(Container.DataItem,"SerialNoMaster")).ToString() == "True" ? ("Serial Number Master\n") : ("" ))+ " "+ ((DataBinder.Eval(Container.DataItem,"ProceedingMaster")).ToString() == "True" ? ("Proceeding\n") : ("" ))%>' Enabled="false" style="background-color:#F2F2F2" runat="server" Checked='<%#GetTranDetails(Convert.ToBoolean(Eval("Meeting")),Convert.ToBoolean(Eval("Agenda")),Convert.ToBoolean(Eval("UploadMinutes")),Convert.ToBoolean(Eval("MasterAgenda")),Convert.ToBoolean(Eval("MeetingRetention")),Convert.ToBoolean(Eval("BoardofDirectore")),Convert.ToBoolean(Eval("PhotoGallery")),Convert.ToBoolean(Eval("GlobalSetting")),Convert.ToBoolean(Eval("IpadNotification")),Convert.ToBoolean(Eval("EmailNotification")),Convert.ToBoolean(Eval("AboutUs")),Convert.ToBoolean(Eval("DeviceManager")),Convert.ToBoolean(Eval("PublishHistory")),Convert.ToBoolean(Eval("EmailHistory")),Convert.ToBoolean(Eval("RevisedPdf")),Convert.ToBoolean(Eval("ProceedingMaster")),Convert.ToBoolean(Eval("TabSetting")),Convert.ToBoolean(Eval("SerialNoMaster")),Convert.ToBoolean(Eval("HeaderMaster")),Convert.ToBoolean(Eval("SectionMaster")))%> '/>

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <%-- <asp:TemplateField HeaderText="User Master" SortExpression="UserMaster">
                                        <ItemTemplate>
                                        <asp:CheckBox ID="chkUserMaster" Enabled="false" style="background-color:#F2F2F2" runat="server" Checked='<%# Eval("UserMaster") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                   <%--       <asp:TemplateField HeaderText="Access Right Master" SortExpression="AccessRightMaster">
                                        <ItemTemplate>
                                        <asp:CheckBox ID="chkAccessMaster" Enabled="false" style="background-color:#F2F2F2" runat="server" Checked='<%# Eval("AccessRightMaster") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                   <asp:TemplateField HeaderText="Approval Master" SortExpression="ApprovalMaster">
                                        <ItemTemplate>
                                        <asp:CheckBox ID="chkApprovalMaster" Enabled="false" style="background-color:#F2F2F2" runat="server" Checked='<%# Eval("ApprovalMaster") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                     <%-- <asp:TemplateField HeaderText="Role Master" SortExpression="RollMaster">
                                        <ItemTemplate>
                                        <asp:CheckBox ID="chkRollMaster" Enabled="false" style="background-color:#F2F2F2" runat="server" Checked='<%# Eval("RollMaster") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                   <%--   <asp:TemplateField HeaderText="Transaction" SortExpression="Transaction">
                                        <ItemTemplate>
                                        <asp:CheckBox ID="chkTransactionMaster" Enabled="false" style="background-color:#F2F2F2" runat="server" Checked='<%# Eval("Transaction") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    
                                      <asp:TemplateField HeaderText="Approval" SortExpression="Approval">
                                        <ItemTemplate>
                                        <asp:CheckBox ID="chkApproval" runat="server" ToolTip='<%#((DataBinder.Eval(Container.DataItem,"Approval")).ToString() == "True" ? ("Approval\n") : ("" ))%>' Enabled="false" style="background-color:#F2F2F2" Checked='<%# Eval("Approval") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                  <%--    <asp:TemplateField HeaderText="View" SortExpression="View">
                                        <ItemTemplate>
                                        <asp:CheckBox ID="chkView" runat="server" Enabled="false" style="background-color:#F2F2F2" Checked='<%# Eval("View") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    
                                      

                                      

                                      <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                        <asp:CheckBox ID="chkView" Enabled="false" ToolTip='<%#((DataBinder.Eval(Container.DataItem,"MeetingView")).ToString() == "True" ? ("Meeting View\n") : ("" )) + " " + ((DataBinder.Eval(Container.DataItem,"NoticeView")).ToString() == "True" ? ("Notice View\n") : ("" )) + " " + ((DataBinder.Eval(Container.DataItem,"AgendaView")).ToString() == "True" ? ("Agenda View\n") : ("" )) + " " + ((DataBinder.Eval(Container.DataItem,"MinutesView")).ToString() == "True" ? ("Minutes View\n") : ("" ))%>' style="background-color:#F2F2F2" runat="server" Checked='<%# GetTranDetails(Convert.ToBoolean(Eval("MeetingView")),Convert.ToBoolean(Eval("NoticeView")),Convert.ToBoolean(Eval("AgendaView")),Convert.ToBoolean(Eval("MinutesView")),Convert.ToBoolean(Eval("ProceedingView")),Convert.ToBoolean(Eval("AttendanceView"))) %>' />

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Report" SortExpression="Report">
                                        <ItemTemplate>
                                        <asp:CheckBox ID="chkReport" Enabled="false" ToolTip='<%#((DataBinder.Eval(Container.DataItem,"Report")).ToString() == "True" ? ("Report\n") : ("" ))%>' style="background-color:#F2F2F2" runat="server" Checked='<%# Eval("Report") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Bind("RollMasterId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Bind("RollMasterId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
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
                           
           
                     <div ID="divdetails" runat="server" style="margin-bottom:15px;padding: 0 15px">
                    <div class="fullwidth">

<div class="infoPersonal">
                       <dt>
                                    <label>
                                    Role Name <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtRollName" runat="server" MaxLength="100" Width="214px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvUser" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtRollName" Display="Dynamic" 
                                        ErrorMessage="Role Name" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>
                                </div>
                                </div>
                                <div class="fullwidth">

<div class="masters">
                               <dt><label class="datalabel">Masters   : </label></dt><dd >
                                <asp:CheckBox CssClass="btnCheck" ID="chkEntityMaster" Text="&nbsp;&nbsp;Department" runat="server" />
                                   <asp:CheckBox CssClass="btnCheck" ID="chkUserMaster" Text="&nbsp;&nbsp;User" runat="server" />

                                    <asp:CheckBox CssClass="btnCheck" ID="chkAccessRight" Text="&nbsp;&nbsp;Access Rights" runat="server" />

                                       <asp:CheckBox CssClass="btnCheck" ID="chkApprovalMaster"  Visible="false" Text="&nbsp;&nbsp;Approval" runat="server" />

                                            <asp:CheckBox CssClass="btnCheck" ID="chkRollMaster" Text="&nbsp;&nbsp;Role" runat="server" />
                               </dd>
                             </div>
                             </div>
                                <%--   <dt>
                                    <label>
                                  Entity Master<span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                  
                                </dd>

                                   <dt>
                                    <label>
                                    User Master <span>*</span> :
                                    </label>
                                </dt>
                                <dd >
                              
                                  
                                </dd>

                                   <dt>
                                    <label>
                                  Access Right Master <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                               
                              
                                </dd>

                                             <dt>
                                    <label>
                                    Approval Master <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                            
                                </dd>--%>
                                <div class="fullwidth">

<div class="masters">
                                     <dt>
                                    <label>
                                   Transactions  <%--<span>*</span> --%>:
                                    </label>
                                </dt>
                                <dd>
                               <asp:CheckBox CssClass="btnCheck" Visible="false" ID="chkTransaction" Text="  Transaction"  runat="server" />
                                 <asp:CheckBox CssClass="btnCheck" ID="chkMeeting" Text="&nbsp;&nbsp;Meeting" runat="server" />
                                   <asp:CheckBox CssClass="btnCheck" ID="chkAgenda" Text="&nbsp;&nbsp;Agenda" runat="server" />

                                    <asp:CheckBox CssClass="btnCheck" ID="chkUploadMin" Text="&nbsp;&nbsp;Upload Minutes" runat="server" />

                                       <asp:CheckBox CssClass="btnCheck" ID="chkMaster" Text="&nbsp;&nbsp;Master Agenda" runat="server" />
                                        <asp:CheckBox Visible="false" CssClass="btnCheck" ID="chkAgendaMarker" Text="&nbsp;&nbsp;Agenda Marker" runat="server" />
                                            <asp:CheckBox CssClass="btnCheck" ID="chkBod" Text="&nbsp;&nbsp;Board Of Directors" runat="server" />
                                                <asp:CheckBox CssClass="btnCheck" ID="chkPhoto" Text="&nbsp;&nbsp;Photo Gallery" runat="server" />
                                   <asp:CheckBox CssClass="btnCheck" ID="chkMeetingRetention" Text="&nbsp;&nbsp;Meeting Retention" runat="server" />
                                   <asp:CheckBox CssClass="btnCheck" ID="chkGlobal" Text="&nbsp;&nbsp;Global Setting" runat="server" />

                                    <asp:CheckBox CssClass="btnCheck" ID="chkIpad" Text="&nbsp;&nbsp;Ipad Notification" runat="server" />

                                       <asp:CheckBox CssClass="btnCheck" ID="chkEmail" Text="&nbsp;&nbsp;Email Notification" runat="server" />

                                      <asp:CheckBox CssClass="btnCheck" ID="chkMeetingScedule" Visible="false" Text="&nbsp;&nbsp;Meeting Schedule" runat="server" />

                                       <asp:CheckBox CssClass="btnCheck" ID="chkAboutUs" Text="&nbsp;&nbsp;About Us" runat="server" />
                                           <asp:CheckBox Visible="false"  CssClass="btnCheck" ID="chkClassification" Text="&nbsp;&nbsp;Classification" runat="server" />
                                           <asp:CheckBox CssClass="btnCheck" ID="chkNoticeTemplate" Visible="false" Text="&nbsp;&nbsp;Notice Template" runat="server" />
                                     <asp:CheckBox Visible="false" CssClass="btnCheck" ID="chkDaftMOM" Text="&nbsp;&nbsp;Draft Minutes" runat="server" />
                                     <asp:CheckBox   CssClass="btnCheck" ID="chkDeviceManager" Text="&nbsp;&nbsp;Device Manager" runat="server" />
                                        <asp:CheckBox   CssClass="btnCheck" ID="chkRevisedPdf" runat="server" Text="&nbsp;&nbsp;Agenda Access" />
                                       <asp:CheckBox   CssClass="btnCheck" ID="chkSection" Text="&nbsp;&nbsp;Section Master" runat="server" />
                                        <asp:CheckBox   CssClass="btnCheck" ID="chkHeaderMaster" Text="&nbsp;&nbsp;Header Master" runat="server" />
                                             <asp:CheckBox   CssClass="btnCheck" ID="chkEmailHistory" Text="&nbsp;&nbsp;Email History" runat="server" />
                                  <asp:CheckBox   CssClass="btnCheck" ID="chkPublishHistory" Text="&nbsp;&nbsp;Publish History" runat="server" />
                                
                                        <asp:CheckBox   CssClass="btnCheck" ID="chkSerailNoMaster" Text="&nbsp;&nbsp;Serial No Master" runat="server" />

                                             <asp:CheckBox   CssClass="btnCheck" ID="chkTabSetting" Text="&nbsp;&nbsp;Tabs Setting" runat="server" />
              

                                             <asp:CheckBox   CssClass="btnCheck" ID="chkProceedingMaster" Text="&nbsp;&nbsp;Proceeding Master" runat="server" />
                                
                                </dd>
                                </div>
                                </div>
                                <div class="fullwidth">

<div class="masters">
                                     <dt>
                                    <label>
                                    Approval  <%-- <span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                               <asp:CheckBox CssClass="btnCheck" ID="chkApproval" Text="  Approval" runat="server" />
                                </dd>
                                </div>
                                </div>
                                <div class="fullwidth">

<div class="masters">
                                     <dt>
                                    <label>
                                   View<%--<span>*</span> --%>:
                                    </label>
                                </dt>
                                <dd>
                                
                               <asp:CheckBox CssClass="btnCheck" ID="chkView" Visible="false" Text="  View" runat="server" />
                                 <asp:CheckBox CssClass="btnCheck" ID="chkMeetingSch" Text="&nbsp;&nbsp;Meeting" runat="server" />
                                     <asp:CheckBox CssClass="btnCheck" ID="chkNotice" Text="&nbsp;&nbsp;Notice" runat="server" />
                                      <asp:CheckBox CssClass="btnCheck" ID="chkAgendaView" Text="&nbsp;&nbsp;Agenda" runat="server" />
                                       <asp:CheckBox CssClass="btnCheck" ID="chkMinutesView" Text="&nbsp;&nbsp;Minutes" runat="server" />
                      
                                         <asp:CheckBox CssClass="btnCheck" ID="chkProceedingView" Text="&nbsp;&nbsp;Proceeding" runat="server" />
                                       <asp:CheckBox CssClass="btnCheck" ID="chkAttendanceView" Text="&nbsp;&nbsp;Attendance" runat="server" />
                           
                                </dd>
                                </div>
                                </div>
                                <div class="fullwidth">

<div class="masters">
                                     <dt>
                                    <label>
                                     Reports  <%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                               <asp:CheckBox CssClass="btnCheck" ID="chkReport" Text="  Report" runat="server" />
                                </dd>
                                </div>
                                </div>

                                     <dl>
                                     <asp:HiddenField ID="hdnApprovalId" runat="server" />
                                     
                                    </dl>
                                    <div id="divbutton" runat="server" class="fullwidth noBorder">
                                    <asp:Button ID="btnInsert" CssClass="btnSave" runat="server" Text="Save" ValidationGroup="a" onclick="btnInsert_Click1"  />
                                       <asp:Button ID="btnCancel" CssClass="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click1"  />
                                    </div>
                                    </div>
                     </fieldset>
                                </div>
                                </section>
                                </article>
</asp:Content>
