<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master"
    AutoEventWireup="true" CodeBehind="UserMaster.aspx.cs" Inherits="MeetingMinder.Web.UserMaster" %>

<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type='text/css'>
        .rdbChecker {
            text-align: left;
            display: inline-block;
        }

            .rdbChecker label {
                display: inline-block;
            }

        .overlay-bg {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            cursor: pointer;
            background: #000; /* fallback */
            background: rgba(0,0,0,0.75);
        }

        .overlay-content {
            background: #fff;
            padding: 1%;
            width: 700px;
            height: 400px;
            overflow: auto;
            position: relative;
            top: 15%;
            left: 30%;
            margin: 0 0 0 -10%; /* add negative left margin for half the width to center the div */
            cursor: default;
            border-radius: 4px;
            box-shadow: 0 0 5px rgba(0,0,0,0.9);
        }

        .onoffswitch {
            position: relative;
            width: 58%;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
        }

        .onoffswitch-checkbox {
            display: none;
        }

        .onoffswitch-label {
            display: block;
            overflow: hidden;
            cursor: pointer;
            border: 2px solid #999999;
            border-radius: 20px;
        }

        .onoffswitch-inner {
            display: block;
            width: 200%;
            margin-left: -100%;
            transition: margin 0.3s ease-in 0s;
        }

            .onoffswitch-inner:before, .onoffswitch-inner:after {
                display: block;
                float: left;
                width: 50%;
                height: 30px;
                padding: 0;
                line-height: 30px;
                font-size: 14px;
                color: white;
                font-family: Trebuchet, Arial, sans-serif;
                font-weight: bold;
                box-sizing: border-box;
            }

            .onoffswitch-inner:before {
                content: "Yes";
                padding-left: 10px;
                background-color: #0079C2;
                color: #FFFFFF;
            }

            .onoffswitch-inner:after {
                content: "No";
                padding-right: 10px;
                background-color: #EEEEEE;
                color: #ADA3A3;
                text-align: right;
            }

        .onoffswitch-switch {
            display: block;
            width: 17px;
            margin: 6.5px;
            background: #FFFFFF;
            position: absolute;
            top: 0;
            bottom: 0;
            right: 56px;
            border: 2px solid #999999;
            border-radius: 20px;
            transition: all 0.3s ease-in 0s;
        }

        .onoffswitch-checkbox:checked + .onoffswitch-label .onoffswitch-inner {
            margin-left: 0;
        }

        .onoffswitch-checkbox:checked + .onoffswitch-label .onoffswitch-switch {
            right: 13px;
        }

        input[disabled] {
            background: #CCC;
        }
    </style>

    <style>
        .popUp_form table td label {
            font-size: 0.9em;
            margin: 8px;
        }

        .popUp_form table td {
            font-size: 0.9em;
            padding: 3px 18px;
        }

        input[type=number] {
            font-family: Helvetica,Verdana,Arial,Helvetica,sans-serif;
            width: 230px !important;
            background-color: #f1f1f1;
            border: 1px solid #dcdcdc;
            color: #858a8e;
            padding: 5px 10px;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
            resize: vertical;
        }
    </style>

    <style type="text/css">
        .ui-widget-header {
            background-color: Blue;
        }

        .ui-widget-content a {
            color: Blue !important;
        }

        .btnSearch {
            background-color: #0F5F88;
            font-size: 13px;
            width: 90px;
            text-align: center;
            background-color: #0079c2;
            color: #fff;
            padding: 6px;
            behavior: url(css/pie/PIE.htc) !important;
            cursor: pointer;
            /* float: left; */
            text-transform: uppercase;
            border: 0;
            border-radius: 0;
            -moz-border-radius: 0;
            -webkit-border-radius: 0;
        }

            .btnSearch:hover {
                background-color: #0F5F88;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">

        function SelectCheckBox() {

            var numChecked = $("#<%= grdUser.ClientID %>  [type=checkbox]:input[id*='chkSubAdmin']:checked").length;
            var numTotal = $("#<%= grdUser.ClientID %>    [type=checkbox]:input[id*='chkSubAdmin'] ").length;
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

            if ($('#<%=hdnUserId.ClientID %>').val().length > 10) {
                var id = '<%= lblAction.ClientID %>';
                $("html, body").animate({
                    scrollTop: $('#' + id
                        ).offset().top
                }, 1500);
            }
        });

        function CheckEmailId(val) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "UserMaster.aspx/CheckEmail",
                data: "{'Email':'" + val + "'}",
                dataType: "json",
                success: function (data) {
                    $("#spnEmail").html(data.d);
                },
                error: function (result) {
                    // alert("Error");
                }
            });

        }

        function CheckEntity() {
            $('#divEntityDetails').toggle();
        }

        function CheckEmailIdTwo(val) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "UserMaster.aspx/CheckEmail",
                data: "{'Email':'" + val + "'}",
                dataType: "json",
                success: function (data) {
                    $("#spnEmailTwo").html(data.d);
                },
                error: function (result) {
                    // alert("Error");
                }
            });

        }

        function CheckUserName(val) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "UserMaster.aspx/CheckUserName",
                data: "{'UserName':'" + val + "'}",
                dataType: "json",
                success: function (data) {
                    $("#spnUser").html(data.d);
                },
                error: function (result) {
                    // alert("Error");
                }
            });

        }


        function CheckEmailId(val) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "UserMaster.aspx/CheckEmail",
                data: "{'Email':'" + val + "'}",
                dataType: "json",
                success: function (data) {
                    $("#spnEmail").html(data.d);
                },
                error: function (result) {
                    // alert("Error");
                }
            });

        }


        function CheckEmailIdTwo(val) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "UserMaster.aspx/CheckEmail",
                data: "{'Email':'" + val + "'}",
                dataType: "json",
                success: function (data) {
                    $("#spnEmailTwo").html(data.d);
                },
                error: function (result) {
                    // alert("Error");
                }
            });

        }

        function CheckUserName(val) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "UserMaster.aspx/CheckUserName",
                data: "{'UserName':'" + val + "'}",
                dataType: "json",
                success: function (data) {
                    $("#spnUser").html(data.d);
                },
                error: function (result) {
                    // alert("Error");
                }
            });

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

        function CheckCounts() {
            if ($('#<%= grdUser.ClientID %> tr:not(:first-child) td:first-child').find('input[type="checkbox"]:checked').length != 0) {
                return confirm('Are you sure you want to delete selected items?');
            }
            else {
                alert("Please Select at least one checkbox");
                return false;
            }
        }

        function showPassword() {
            if (document.getElementById('<%=chkIpad.ClientID%>').checked) {
                $("#divPassword").attr("style", "display:block");
                $("#divConfirmPassword").attr("style", "display:block");
                var validatorObject = document.getElementById('<%=rfvPassword.ClientID%>');
                validatorObject.enabled = true;
                validatorObject.isvalid = true;
                ValidatorUpdateDisplay(validatorObject);
                var validatorObjects = document.getElementById('<%=rfvConfPassword.ClientID%>');
                validatorObjects.enabled = true;
                validatorObjects.isvalid = true;
                ValidatorUpdateDisplay(validatorObjects);
            }
            else {
                $("#divPassword").attr("style", "display:none");
                $("#divConfirmPassword").attr("style", "display:none");
                var validatorObject = document.getElementById('<%=rfvPassword.ClientID%>');
                validatorObject.enabled = false;
                var validatorObjects = document.getElementById('<%=rfvConfPassword.ClientID%>');
                validatorObjects.enabled = false;
            }
        }

        function ValidateCheckBoxList(sender, args) {
            debugger;
            var checkBoxList = document.getElementById("<%=ddlEntityList.ClientID %>");
            var checkboxes = checkBoxList.getElementsByTagName("input");
            var isValid = false;
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    isValid = true;
                    break;
                }
            }
            args.IsValid = isValid;
        }

        // Department

        function DepartmentFun() {
            if (confirm('Are you sure you want to add current department?')) {
                var val = $('#<%=txtUserName.ClientID%>').val();
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "UserMaster.aspx/AddCurrentDepartment",
                    data: "{'UserName':'" + val + "'}",
                    dataType: "json",
                    success: function (data) {
                        //$("#spnUser").html(data.d);
                    },
                    error: function (result) {
                        // alert("Error");
                    }
                });
            } else {

            }
        }

        function GetUserDetailByUserName(val, e) {
            e.preventDefault();
            if (val.length > 0 && val.length < 11) {
                document.getElementById('<%= divloader.ClientID %>').style.display = "block";
                document.getElementById('<%= lnkViewUser.ClientID %>').click();
            }
        }


        function TopUp() {
            var id = '<%= lblAction.ClientID %>';
            $("html, body").animate({
                scrollTop: $('#' + id
                    ).offset().top
            }, 1500);
        }

        $(function () {
            $('#<%=btnInsert.ClientID%>').on('click', function (e) {
                if (!Page_ClientValidate("a")) {
                    e.preventDefault();
                    document.getElementById('<%= lnkViewUser.ClientID %>').click();
                }
            });

            $("#<%=txtCheckUser.ClientID%>").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                //if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                //    return false;
                //}

                var code = ('charCode' in e) ? e.charCode : e.keyCode;
                if (!(code == 32) && // space
                  !(code > 47 && code < 58) && // numeric (0-9)
                  !(code > 64 && code < 91) && // upper alpha (A-Z)
                  !(code > 96 && code < 123)) { // lower alpha (a-z)
                    e.preventDefault();
                }
            });
        })

    </script>

    <script type="text/javascript" src="js/aes.js"></script>

    <script type="text/javascript">
        function ValidateModuleList(source, args) {
            debugger;
            var chkListModules = document.getElementById('<%= ddlEntityList.ClientID %>');
            var chkListinputs = chkListModules.getElementsByTagName("input");
            for (var i = 0; i < chkListinputs.length; i++) {
                if (chkListinputs[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }

        //encrypt pan and passport
        function checkSave() {

            if (Page_ClientValidate("a")) {
                var hdnUserVal = document.getElementById('<%=hdnUserVal.ClientID%>').value;

                var value = document.getElementById('<%=txtPanNo.ClientID%>').value;
                var newval = document.getElementById('<%=txtPassportNumber.ClientID%>').value;

                var pwd = document.getElementById('<%=txtPassword.ClientID%>').value;
                var cnfPwd = document.getElementById('<%=txtConformPass.ClientID%>').value;
                //var hash =  hex_sha256(value);

                var key = CryptoJS.enc.Utf8.parse(hdnUserVal);
                var iv = CryptoJS.enc.Utf8.parse(hdnUserVal);

                var encryptpan = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(value), key,
                  {
                      keySize: 128 / 8,
                      iv: iv,
                      mode: CryptoJS.mode.CBC,
                      padding: CryptoJS.pad.Pkcs7
                  });

                var encryptpassport = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(newval), key,
                 {
                     keySize: 128 / 8,
                     iv: iv,
                     mode: CryptoJS.mode.CBC,
                     padding: CryptoJS.pad.Pkcs7
                 });

                var encPwd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(pwd), key,
                  {
                      keySize: 128 / 8,
                      iv: iv,
                      mode: CryptoJS.mode.CBC,
                      padding: CryptoJS.pad.Pkcs7
                  });

                var encCnfPwd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(cnfPwd), key,
                  {
                      keySize: 128 / 8,
                      iv: iv,
                      mode: CryptoJS.mode.CBC,
                      padding: CryptoJS.pad.Pkcs7
                  });

                document.getElementById('<%=hdnPanNo.ClientID %>').value = encryptpan;
                document.getElementById('<%=hdnPassport.ClientID %>').value = encryptpassport;

                document.getElementById('<%=txtPanNo.ClientID %>').value = "";
                document.getElementById('<%=txtPassportNumber.ClientID %>').value = "";

                document.getElementById('<%=txtPassword.ClientID %>').value = encPwd;
                document.getElementById('<%=txtConformPass.ClientID %>').value = encCnfPwd;

                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp; <%--View User--%></h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#054a7f"><b>Search User</b></font></legend>
                            <dl>
                                <div style="margin-bottom:15px"><userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                   <div style="margin-bottom:15px">
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                </div>
                                <div class="add_remove1" style="margin-bottom:15px">
                                    <asp:TextBox ID="txtSearch" runat="server" style="width: 230px!important;color: #000!important;" placeholder="Enter the Userid / Name"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqfvSearch" runat="server" ControlToValidate="txtSearch" ErrorMessage="Enter pf id or name" class="invalid-side-note" ValidationGroup="s" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:Button  ID="btnSearch" runat="server" Text="Search" CssClass="btnSearch" ValidationGroup="s" OnClick="btnSearch_Click" />
                                    <asp:Button  ID="btnReset" runat="server" Text="Reset" CssClass="btnSearch" Visible="false" OnClick="btnReset_Click" />

                                </div>
                               <div class="add_remove" style="margin-bottom:15px">
                                    <img width="16" height="16" src="img/icons/icon_list_style_cross.png" alt="" style="vertical-align: middle"/> 
                                    <asp:LinkButton runat="server" ID="lbRemoveSelected" Text="Remove All Selected Record" 
                                    OnClientClick="return CheckCounts();" onclick="lbRemoveSelected_Click"  ></asp:LinkButton>
                                   |&nbsp;<asp:LinkButton CausesValidation="false" runat="server" ID="lbExportToExcel" Text="Export To Excel" OnClick="lbExportToExcel_Click"></asp:LinkButton>
                               </div>
                               <div style="margin-bottom:15px"><asp:Label ID="lblTotalRecord" runat="server" Font-Bold="True"></asp:Label></div>  
                               <%--<div style="margin-bottom:15px"> <asp:Label  ID="lblUser" runat="server" Text="Search User" Font-Bold="true" ></asp:Label> </div>  --%>

                               <div class="grid_24">
                               <div class="box_top">
		
		<h2 class="icon users">User List</h2>
		
	</div>
    <div class="box_content">
		
		<!-- Simple Sorting Table + Pagination: Start -->
		<div class="dataTables_wrapper">
                                                      
                         
                                         <asp:GridView ID="grdUser" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    PageSize="10" DataKeyNames="UserId" OnRowDeleting="grdUser_RowDeleting"
                                    OnSelectedIndexChanged="grdUser_SelectedIndexChanged" 
                                    onpageindexchanging="grdUser_PageIndexChanging" 
                                    onsorting="grdUser_Sorting" onrowcommand="grdUser_RowCommand" 
                                    onrowediting="grdUser_RowEditing">
                                             <PagerStyle CssClass="paginate_active"   />
                                  <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                    <RowStyle CssClass="gradeA odd" />
                                  <Columns>

                                    <asp:TemplateField>
                                        <ItemStyle  HorizontalAlign="Center" />
                                        <HeaderStyle  HorizontalAlign="Center" />
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkHeader" CssClass="all" onclick="javascript: fn_select_all(this);" runat="server" />
                                         </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSubAdmin" onchange="SelectCheckBox();" CssClass="allSub" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Name" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClientName" runat="server" Text='<%# Eval("Suffix") +" "+Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                       <asp:TemplateField HeaderText="Designation" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignation" runat="server" Text='<%#Eval("Designation")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Roll Name" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRoll" runat="server" Text='<%#Eval("RollName")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField Visible="false" HeaderText="Contact Number" SortExpression="Mobile">
                                        <ItemTemplate>
                                            <asp:Label ID="lblContactNumber" runat="server" Text='<%#Eval("Mobile") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    
                                     <asp:TemplateField Visible="false" HeaderText=" Email" SortExpression="EmailId1">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClientEmail" runat="server" Text='<%#Eval("EmailID1") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:BoundField DataField="Description"  HeaderText="Description"  />--%>
                                          <asp:TemplateField HeaderText="Access Rights">
                <ItemStyle HorizontalAlign="Center" Width="10%" />
                <ItemTemplate>
                <%--<asp:LinkButton ID="lbnAccess" CommandArgument='<%# Bind("UserId") %>' CausesValidation="false" CommandName="View" Text="Add/View" runat="server" ToolTip="Add Access Rights"></asp:LinkButton>--%>
                    <a  class="AddView" href="AccessRightAdmin.aspx" title="Add or view access rights">Add/View </a>
                </ItemTemplate>
            </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>                                      
                                <%--        <asp:LinkButton ID="btnView" runat="server" Text="View" CommandName="View"  CausesValidation="false" CommandArgument='<%# Bind("UserId") %>'></asp:LinkButton>--%>

                                            <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Bind("UserId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" Visible='<%# Convert.ToBoolean(Eval("hideButton"))==true?false:true %>' />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Bind("UserID") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" Visible='<%# Convert.ToBoolean(Eval("hideButton"))==true?false:true %>' OnClientClick='return confirm("Are you sure you want to delete this entry?");' />

                                          <asp:ImageButton ID="lbtnUnlock" Visible='<%# Eval("IsLocked") %>' CommandArgument='<%# Bind("UserID") %>' ImageUrl="img/icons/actions/Unlock.png" 
                                                 CausesValidation="false" CommandName="Unlock" Text="Unlock" runat="server" ToolTip="Unlock User" />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                            </div>
                             <%--    <div style="margin-bottom:15px">--%>
                          </div>
	<!-- Box Content: End -->
	
</div>

                            </dl>
                         
           
                    <div class="frmNewuser"><!--frmNewuser start-->
<h2><asp:Label ID="lblAction" runat="server" Text="Add New User"></asp:Label> </h2>
                     <div class="frmDetails"><!--frmDetails start-->
<span> Fields marked with asterisk (*) are required</span>

                         <%--<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
                             <ContentTemplate>--%>

                             

                        



                    <h4  class="subTitle">Personal</h4>
                         <div  class="fullwidth">
                             <div class="infoPersonal">
                     <dt>
                    <label>PF Number <span>*</span> :</label>
                     </dt>
                     <dd>
                         <%--<asp:UpdatePanel ID="updatePanel" runat="server">
                             <ContentTemplate>--%>
                                 <div style="display:none;" id="divloader" runat="server">
                                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                                     <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="img/jquery/ajaxLoader.gif" AlternateText="Loading ..." ToolTip="Loading ..." style="padding: 10px;position:fixed;top:45%;left:50%;" />
                                    </div>
                                 </div>
                                  
                                 <asp:TextBox ID="txtCheckUser" runat="server" MaxLength="149" Width="214px" autocomplete="off" onblur="return GetUserDetailByUserName(this.value,event)"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revCheckUser" runat="server" ValidationGroup="a"  class="invalid-side-note"
                                        ControlToValidate="txtCheckUser" Display="Dynamic" 
                                        ErrorMessage="PF Number" 
                                        ValidationExpression="^[A-Za-z0-9]{1,10}$"> Please enter valid pf number.</asp:RegularExpressionValidator>
                                 <asp:RequiredFieldValidator ID="rfvCheckUser" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtCheckUser" Display="Dynamic" 
                                        ErrorMessage=" PF Number" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                  <span id="spnUser" runat="server"></span> 
                                 
                             <%--</ContentTemplate>
                              <Triggers>
                                  <%--<asp:AsyncPostBackTrigger EventName="Click" ControlID="lnkViewUser" />-->
                                  <asp:PostBackTrigger ControlID="lnkViewUser" />
                              </Triggers>
                          </asp:UpdatePanel>--%>
                     </dd>
                     </div>
                             </div>


                     <div  class="fullwidth">
                       <div class="infoPersonal">
                     <dt>
                    <label>   Prefix :</label>
                     </dt>
                     <dd>
                     <asp:DropDownList Width="234px" ID="ddlSuffix" runat="server"></asp:DropDownList>
                     </dd>
                     </div>
                        <div class="infoPersonal">
                                    <dt>
                                    <label>
                                    First Name <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtFirstName" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvUser" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtFirstName" Display="Dynamic" 
                                        ErrorMessage=" First Name" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>   
                        </div>
                     </div>
                     <%-- firstname and Last Name full width---%>
                     <div class="fullwidth">
                     <div class="infoPersonal">
                                <dt>
                                    <label>
                                    Middle Name <%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtMiddleName" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                   
                               </dd>                              
                                </div>

                     <div class="infoPersonal">
                                <dt>
                                    <label>
                                    Last Name <%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtLastName" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                   
                               </dd>
                              </div>
                             
                                     </div>
                                
              
              <div class="fullwidth">
              <div class="infoPersonal">
                                 <dt>
                                    <label>
                                    Designation <%--<span>*</span> --%>:
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtDesignation" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                 <%--   <asp:RequiredFieldValidator ID="rfvDesignation" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtDesignation" Display="Dynamic" 
                                        ErrorMessage="Designation" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
--%>                                </dd>
                                </div>
                  <div class="infoPersonal">
                            
                                   <dt> <label>
                                Photograph<%--<span>*</span>--%> :
                                    </label></dt>
                                    <dd>
                                    <asp:FileUpload style="width:230px" class=" marginTop100" ID="fuPhotograph" runat="server"></asp:FileUpload>
                                      <asp:LinkButton ID="lnkView" runat="server" Visible="false" Text="View" onclick="lnkView_Click"></asp:LinkButton>
                                    </dd>
                                    </div>
                                  <div class="infoPersonal" style="display:none">
                                 <dt>
                                    <label>
                                   PAN Number <%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                  <asp:TextBox ID="txtPanNo" runat="server" MaxLength="50" Width="214px" autocomplete="off"></asp:TextBox>
                                   <%-- <asp:RequiredFieldValidator ID="rfvPanNo" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtPanNo" Display="Dynamic" 
                                        ErrorMessage="PAN Number" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
--%>                                </dd>
</div>

</div>
<div class="fullwidth">
<div class="infoPersonal" style="display:none">
                                <dt>
                                    <label>
                                    DIN Number <%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                  <asp:TextBox ID="txtDinNo" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                               <%--     <asp:RequiredFieldValidator ID="rfvDinNo" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtDinNo" Display="Dynamic" 
                                        ErrorMessage="DIN Number" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
--%>                                </dd>
</div>


    <div class="infoPersonal" style="display:none">
                                <dt>
                                    <label>
                                    Passport Number <%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                  <asp:TextBox ID="txtPassportNumber" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                               <%--     <asp:RequiredFieldValidator ID="rfvDinNo" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtDinNo" Display="Dynamic" 
                                        ErrorMessage="DIN Number" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
--%>                                </dd>
</div>

                                    </div>
<div class="fullwidth" style="display:none;">


                   <%--                 <dt>
                      <label> </label>
                     </dt>--%>
     <h4  class="subTitle">Contact</h4>
                     <div class="infoPersonal">
                                 <dt>
                                    <label>
                                   Residential Address <%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtAddress" runat="server" Rows="3" TextMode="MultiLine" 
                                        Width="214px" autocomplete="off"></asp:TextBox>
                                                     </dd>
</div>
  <div class="infoPersonal">
                                 <dt>
                                    <label>
                                   Residential Contact No. <%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtContactNumber" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="rgeContactNmber" runat="server" 
                                        ControlToValidate="txtContactNumber" Display="Dynamic"  ValidationGroup="a"  class="invalid-side-note"
                                        ErrorMessage="Residential Contact No." ValidationExpression="^[0-9]{1,12}$"> Please enter valid contact number.</asp:RegularExpressionValidator>
                                  <%--  <asp:RequiredFieldValidator ID="rfvContactNumber" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtContactNumber" 
                                        Display="Dynamic" ErrorMessage="Contact Number" SetFocusOnError="True" 
                                        ValidationGroup="a">Invalid</asp:RequiredFieldValidator>--%>
                                </dd>
                                </div>
                                </div>
                                <div class="fullwidth">

<div class="infoPersonal">
                                <dt>
                                    <label>
                                    Mobile No. <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtMobileNumebr" ValidationGroup="a" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                    <div style="font-size: 12px;">
 <br> &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;<strong>Mob. No Format:</strong> Country Code + Mobile No.
  <br> &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp; <strong>Ex.: </strong>91xxxxxxxxxx </div>
                                    <%--<asp:RegularExpressionValidator ID="rgeMobileNumber" runat="server" ValidationGroup="a"  class="invalid-side-note"
                                        ControlToValidate="txtMobileNumebr" Display="Dynamic" 
                                        ErrorMessage="Mobile No." 
                                        ValidationExpression="^[0-9]{12,13}$"> Please enter valid mobile number.</asp:RegularExpressionValidator>--%>
                                    <asp:RegularExpressionValidator ID="rgeMobileNumber" runat="server" ValidationGroup="a"  class="invalid-side-note"
                                        ControlToValidate="txtMobileNumebr" Display="Dynamic" 
                                        ErrorMessage="Mobile No." 
                                        ValidationExpression="^(91|977)[0-9]{10}$"> Please enter valid mobile number.</asp:RegularExpressionValidator>
                                   <asp:RequiredFieldValidator ID="rfvMobileNumber" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtMobileNumebr" Display="Dynamic" 
                                        ErrorMessage="Mobile Number" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator> 
                                </dd>
                                </div>
                                    <div class="infoPersonal">
                                 <dt>
                                    <label>
                                    Email Id  <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtEmail" onblur="CheckEmailId(this.value)" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                    <span id="spnEmail"></span>
                                    <asp:RegularExpressionValidator ID="rgvEmail" runat="server" 
                                        ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Email Id" 
                                        SetFocusOnError="True"   ValidationGroup="a"  class="invalid-side-note"
                                        ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(['\&quot;][^\&lt;\&gt;'\&quot;]*['\&quot;]\s*\&lt;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\&gt;))(,\s*((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(['\&quot;][^\&lt;\&gt;'\&quot;]*['\&quot;]\s*\&lt;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\&gt;)))*">Please enter valid email id</asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="rfvEmailID" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtEmail" Display="Dynamic" 
                                        ErrorMessage="Email Id" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd></div>
<div class="infoPersonal" style="display:none;">                              
                                 <dt>
                                    <label>
                                   Office Address <%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtOfficeAddress" runat="server" Rows="3" TextMode="MultiLine" 
                                        Width="214px" autocomplete="off"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="rfvOfficeAddress" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtOfficeAddress" Display="Dynamic" 
                                        ErrorMessage="Office Address" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>--%>
                                </dd>
                                </div>
                                </div>

                                <div class="fullwidth">

<div class="infoPersonal" style="display:none">
                                 <dt>
                                    <label>
                                     Office Contact No. <%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtOfficeContactNo" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="rgeContactNo" runat="server" 
                                        ControlToValidate="txtOfficeContactNo" Display="Dynamic"   ValidationGroup="a"  class="invalid-side-note"
                                        ErrorMessage="Office Contact No." ValidationExpression="^[0-9]{1,12}$"> Please enter valid contact number.</asp:RegularExpressionValidator>
                                    <%--<asp:RequiredFieldValidator ID="rfvOfficeContactNo" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtOfficeContactNo" 
                                        Display="Dynamic" ErrorMessage="Office Contact Number" SetFocusOnError="True" 
                                        ValidationGroup="a">Invalid</asp:RequiredFieldValidator>--%>
                                </dd>
                                </div>
                              

</div>
 <div class="fullwidth" style="display:none;">
<div class="infoPersonal">
                                <dt>
                                    <label>
                                   Alternative Email Id  <%--<span>*</span>--%>:
                                    </label>
                                </dt>
                                <dd>  
                                    <asp:TextBox ID="txtEmailTwo"  onblur="CheckEmailIdTwo(this.value)" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                    <span id="spnEmailTwo"></span>
                                    <asp:RegularExpressionValidator ID="rgvEmailTwo" runat="server"   ValidationGroup="a"  class="invalid-side-note"
                                        ControlToValidate="txtEmailTwo" Display="Dynamic" ErrorMessage="Alternative Email Id" 
                                        SetFocusOnError="True" 
                                        ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(['\&quot;][^\&lt;\&gt;'\&quot;]*['\&quot;]\s*\&lt;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\&gt;))(,\s*((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(['\&quot;][^\&lt;\&gt;'\&quot;]*['\&quot;]\s*\&lt;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\&gt;)))*">Please enter valid email id</asp:RegularExpressionValidator>
                               
                               
                                </dd>
                                </div>
<div class="infoPersonal" style="display:none">
                                 <dt>
                                    <label>
                                   Food Preference :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtFoodPreference" Visible="false" runat="server" Rows="3" TextMode="MultiLine" 
                                        Width="214px" autocomplete="off"></asp:TextBox>
                                </dd></div>
                                </div>

<div class="fullwidth" style="display:none;">
      <%--<dt>
                      <label> </label>
                     </dt>--%>
    <h4  class="subTitle">Secretary</h4> 

<div class="infoPersonal">
                              

                                <dt>
                                <label>Secretary Name :</label>
                                </dt>
                                <dd>
                                 <asp:TextBox ID="txtSecretoryName" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="rfvSecretaryName" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtSecretoryName" Display="Dynamic" 
                                        ErrorMessage=" Secretary Name" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>--%>
                                </dd>
                                </div>
                                <div class="infoPersonal">
                                 <dt>
                                     <label>
                                     Secretary Residential Contact No. <%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtSecreroryResiCont" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="rgeSecretoryResiCont" runat="server" 
                                        ControlToValidate="txtSecreroryResiCont" Display="Dynamic"   ValidationGroup="a"  class="invalid-side-note"
                                        ErrorMessage="Secretary Residential Contact No." ValidationExpression="^[0-9]{1,12}$"> Please enter valid contact number.</asp:RegularExpressionValidator>
                                    <%--<asp:RequiredFieldValidator ID="rfvSecrectoryResiCont" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtSecreroryResiCont" 
                                        Display="Dynamic" ErrorMessage="Secretary Residential Contact Number" SetFocusOnError="True" 
                                        ValidationGroup="a">Invalid</asp:RequiredFieldValidator>--%>
                                </dd>
                                </div>
                                </div>

                                <div class="fullwidth" style="display:none;">


                               
<div class="infoPersonal">
                                 <dt>
                                    <label>
                                     Secretary Office Contact No. <%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtSecreroryOffiCont" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="rgeSecreroryOffiCont" runat="server" 
                                        ControlToValidate="txtSecreroryOffiCont" Display="Dynamic"   ValidationGroup="a"  class="invalid-side-note"
                                        ErrorMessage="Secretary Office Contact No." ValidationExpression="^[0-9]{1,12}$"> Please enter valid contact number.</asp:RegularExpressionValidator>
                                  
                                </dd>
                                </div>
                                <div class="infoPersonal">
                                 <dt>
                                    <label>
                                     Secretary Mobile No. <%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtSecretoryMobile" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="rgeSecretoryMobile" runat="server" 
                                        ControlToValidate="txtSecretoryMobile" Display="Dynamic" 
                                        ErrorMessage="Secretary Mobile No."   ValidationGroup="a"  class="invalid-side-note"
                                        ValidationExpression="^[0-9]{1,12}$"> Please enter valid mobile number.</asp:RegularExpressionValidator>
                                    <%--<asp:RequiredFieldValidator ID="rfvSecretoryMobile" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtSecretoryMobile" Display="Dynamic" 
                                        ErrorMessage="Secretary Mobile Number" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>--%>
                                </dd>
                                </div>

                                </div>

                                <div class="fullwidth" style="display:none;">



                            
<div class="infoPersonal">
                                  <dt>
                                    <label>
                                  Secretary Email Id<%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtSecretoryEmail" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="rgeSecretoryEmail" runat="server"   ValidationGroup="a"  class="invalid-side-note"
                                        ControlToValidate="txtSecretoryEmail" Display="Dynamic" ErrorMessage="Secretary Email Id" 
                                        SetFocusOnError="True" 
                                        ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(['\&quot;][^\&lt;\&gt;'\&quot;]*['\&quot;]\s*\&lt;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\&gt;))(,\s*((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(['\&quot;][^\&lt;\&gt;'\&quot;]*['\&quot;]\s*\&lt;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\&gt;)))*">
                                        Please enter valid email id
                                    </asp:RegularExpressionValidator>
                                    <%--<asp:RequiredFieldValidator ID="rfvSecretoryEmail" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtSecretoryEmail" Display="Dynamic" 
                                        ErrorMessage="Secretary EMail ID 1" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>--%>
                                </dd>
                                </div>
                                <div class="infoPersonal">
                                  <dt>
                                    <label>
                                 Secretary Alternative Email Id<%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtSecretoryEmailTwo" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="rgeSecretoryEmailTwo" runat="server"   ValidationGroup="a"  class="invalid-side-note"
                                        ControlToValidate="txtSecretoryEmailTwo" Display="Dynamic" ErrorMessage="Secretary Alternative Email Id" 
                                        SetFocusOnError="True" 
                                        ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(['\&quot;][^\&lt;\&gt;'\&quot;]*['\&quot;]\s*\&lt;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\&gt;))(,\s*((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(['\&quot;][^\&lt;\&gt;'\&quot;]*['\&quot;]\s*\&lt;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\&gt;)))*">Please enter valid email id</asp:RegularExpressionValidator>
                                    <%--<asp:RequiredFieldValidator ID="rfvSecretoryEmailTwo" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtSecretoryEmailTwo" Display="Dynamic" 
                                        ErrorMessage="Secretary EMail ID 2" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>--%>
                                </dd>
                                </div>
                                </div>


                          <div class="fullwidth">
                                 <%--<dt>
                      <label> </label>
                     </dt>--%>

                    <h4 class="subTitle">Account</h4>                                                              
                            <div class="infoPersonal">
                                <dt>
                                   <label>
                                Username<span>*</span> :
                                </label>
                                </dt>
                                <dd>
                               <asp:TextBox ID="txtUserName" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>

                               <%--<asp:TextBox ID="txtUserName" AutoPostBack="true" OnTextChanged="txtUserName_TextChanged" runat="server" MaxLength="149" Width="214px"></asp:TextBox>--%>
                                         <%-- <span id="spnUser" runat="server"></span> --%>
                                    <%--onblur="CheckUserName(this.value)"--%>
                                    <%--<div style="font-size:12px; margin-left:315px"><strong><a style="color:#3f79c1;" href="javascript:void(0)" onclick="DepartmentFun()">Add department</a></strong></div>     --%>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtUserName" Display="Dynamic" 
                                        ErrorMessage="Username" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>
                                        <br />
                                        
                                </div>
                                      <div class="infoPersonal" runat="server" id="divDepartment">
                      
    <dt>
                                   <label>
                                Department<span>*</span> :
                                </label>
                                </dt>
                                <dd>
                                 <%--  <asp:ListBox  SelectionMode="Multiple" ID="ddlEntityList" Width="234px" runat="server" />--%>
                                       <asp:CheckboxList    SetFocusOnError="True" ValidationGroup="a"  SelectionMode="Multiple" ID="ddlEntityList" Width="234px" runat="server" />
                                    <asp:CustomValidator     class="invalid-side-note"  runat="server" ID="cvmodulelist"  ValidationGroup="a" 
  ClientValidationFunction="ValidateModuleList"
  ErrorMessage="Department" >Invalid</asp:CustomValidator>

                                      <%-- <asp:RequiredFieldValidator ID="rfvEntityList" runat="server" 
                                        class="invalid-side-note" ControlToValidate="ddlEntityList" Display="Dynamic" 
                                        ErrorMessage="Entity" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>--%>
                                </dd>
</div>
                                </div>
                                <div class="fullwidth">
                                    
<div id ="divPassword" runat="server" clientidmode="Static" class="infoPersonal">
                                 <dt>
                                    <label id="lblPassword" runat="server">
                                 Password<span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                    <br />
                                    <%--<asp:RegularExpressionValidator ID="rgePassword" runat="server"  class="invalid-side-note" 
                                        ControlToValidate="txtPassword" ValidationGroup="a" Display="Static" ErrorMessage="Password" 
                                        SetFocusOnError="True"  
                                        ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$">Password must contain: Minimum 8 and Maximum 15 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character</asp:RegularExpressionValidator>--%>
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                                       class="invalid-side-note" ControlToValidate="txtPassword" Display="Static" 
                                        ErrorMessage="Password" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>
                                </div>

<div  id ="divConfirmPassword" runat="server"   clientidmode="Static" class="infoPersonal">
 <dt>
                                    <label  id="lblConfPassword" runat="server">
                                 Confirm Password<span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtConformPass" TextMode="Password" runat="server" MaxLength="149" Width="214px" autocomplete="off"></asp:TextBox>
                                   <br />
                                     <%--<asp:CompareValidator class="invalid-side-note" ID="cfvConformPass" runat="server" ValidationGroup="a" Display="Static" SetFocusOnError="true" ControlToCompare="txtPassword"   ControlToValidate="txtConformPass" ErrorMessage="Password not match"></asp:CompareValidator>--%>
                                        <asp:RequiredFieldValidator ID="rfvConfPassword" runat="server"  
                                        class="invalid-side-note" ControlToValidate="txtConformPass" Display="Static" 
                                        ErrorMessage="Confirm Password" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                         
                                          <%--<asp:RegularExpressionValidator ID="revConfPas" ValidationGroup="a" runat="server"  class="invalid-side-note" 
                                        ControlToValidate="txtConformPass" Display="Dynamic" ErrorMessage="Confirm Password" 
                                        SetFocusOnError="True"
                                        ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$">Password must contain: Minimum 8 and Maximum 15 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character</asp:RegularExpressionValidator>--%>
                                </dd>
                                </div>
                                </div>
                                
                                <div class="fullwidth">

<div class="infoPersonal">
                                <dt>
                                    <label>
                                 Checker<%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                <asp:RadioButton CssClass="btnRadio" ID="rdbCheker" runat="server" Text="Yes" GroupName="check" /> &nbsp; &nbsp; 
                                  <asp:RadioButton CssClass="btnRadio" ID="rdbCheckerNo" runat="server" Text="No"  Checked="true"  GroupName="check" />
                                </dd>
                             </div>
                              
 

<div class="infoPersonal">
                               <dt>
                                    <label>
                                 Maker<%--<span>*</span>--%> :
                                    </label>
                                </dt>
                                <dd>
                                <asp:RadioButton CssClass="btnRadio" ID="rdbMaker" runat="server" Text="Yes" GroupName="make" /> &nbsp; &nbsp; 
                                  <asp:RadioButton CssClass="btnRadio" ID="rdbMakerNo" runat="server" Text="No" Checked="true" GroupName="make" />
                                </dd>

                                </div>
                                </div>
                                <div class="fullwidth">

<div class="infoPersonal">
                                
                                   <dt>
                                    <label >
                                Enabled On :
                                    </label>
                                </dt>
                                <dd>
                                   <asp:CheckBox CssClass="btnCheck" ID="chkWebApp" runat="server" Text="Web App" />  &nbsp; &nbsp; 
                                <asp:CheckBox CssClass="btnCheck" ID="chkIpad" runat="server" Text="Ipad" />
                                </dd>
                                </div>
                                


                                </div>
                         <div class="fullwidth">
                             <div class="infoPersonal" runat="server" id="divIsSuperAdmin">
                                 <dt>
                                     <label>
                                         IsSuperAdmin  :
                                     </label>
                                 </dt>
                                 <dd>
                                    <asp:CheckBox ID="chkIsSuperAdmin" runat="server" ClientIDMode="Static" />
                                 </dd>
                             </div>
                         </div>
                              <div class="fullwidth">
                             <div class="infoPersonal">
                                  <dt>
                                    <label>
                                    Send to Checker  :
                                    </label>
                                </dt>
                                <dd>
                                <asp:CheckBox onclick="showChecker();" ID="chkChecker" runat="server" ClientIDMode="Static" />
                                </dd>
                                </div>
                         <div style="display:none;"  class="infoPersonal">
   <dt>
                                    <label >
                                Default User :
                                    </label>
                                </dt>
                                <dd>
                                   <asp:CheckBox CssClass="btnCheck" ID="chkDefaultUser" runat="server"  />  
                                </dd>

                         </div>
                                  </div>
                          <div class="fullwidth noBorder">
                      

  <div  id="divEntity"  runat="server" clientidmode="Static" style="display:none">
<div class="infoPersonal" >
  
                                  <dt>
                                    <label>
                                Checker <span>*</span>:
                                    </label>
                                </dt>
                                <dd>
                                   <asp:DropDownList Width="232px" ID="ddlUser"  runat="server" ></asp:DropDownList>
                                      <asp:RequiredFieldValidator ID="rfvddUser" runat="server" Enabled="false"
                                        class="invalid-side-note" ControlToValidate="ddlUser" Display="Dynamic" InitialValue="0"
                                        ErrorMessage="Checker" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator> 
                                </dd>
        
                       
     </div>
    <div   class="infoPersonal">
        <dt>
                                    <label>
                                   Notify Checker   :
                                    </label>
                                </dt>
                                <dd>
                                 <asp:CheckBox ID="chkNotifyChecker" runat="server"   />                     
                                </dd>
    </div>
                               
        </div>
                                
                        
                                


                                 <%--   <dl>--%><asp:HiddenField ID="hdnUserId" runat="server" />
                                    <asp:HiddenField ID="hdnPhoto" runat="server" />
                                    <%--</dl>--%>
                                    </div>
                                    <div id="divbutton" class="fullwidth noBorder" runat="server" >
                                    <asp:Button CssClass="btnSave" ID="btnInsert" runat="server" Text="Save" ValidationGroup="a" OnClientClick="javascript:return checkSave()" onclick="btnInsert_Click1"  />
                                       <asp:Button CssClass="btnCancel" ID="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click1"  />
                                    </div>
                                    </div>
                                            </div>

                        <div runat="server" clientidmode="Static" style="display:none;height:200px !important; overflow:scroll;width:700px !important;" id="dialog">
<asp:Label ID="lblDetails" runat="server" class="popUp_form"></asp:Label>
</div>

                   <%-- </ContentTemplate>
                             <Triggers>
                                 <asp:AsyncPostBackTrigger ControlID="txtUserName" EventName="TextChanged" />
                             </Triggers>
                 </asp:UpdatePanel>--%>
                    <asp:HiddenField ID="hdnPassport" runat="server" />
                    <asp:HiddenField ID="hdnPanNo" runat="server" />
                    <asp:HiddenField ID="hdnUserVal" runat="server" />
                    <div style="display:none;">
                                    <asp:LinkButton ID="lnkViewUser" runat="server"  Text="View" OnClick="lnkViewUser_Click"></asp:LinkButton>
                                </div>
                    
                       </fieldset>
                                </div>
                               
                                </section>
                                </article>
</asp:Content>
