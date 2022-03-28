<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ApprovalMaster.aspx.cs" Inherits="MeetingMinder.Web.ApprovalMaster" %>

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
            z-index: 10000;
        }

        #footer {
            position: inherit;
            z-index: -1;
        }
        table tr td {
    padding: 6px 6px;}
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

        li {
            list-style:none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .ui-widget-header {
            background-color: #e31f26;
        }

        .ui-widget-content a {
            color: Blue !important;
        }

          .box_content {		
	    overflow: auto;		
	    width: 93%;		
	}
    </style>

    <script type="text/javascript">
        var lnkId = "";
        function ShowPopUp(obj) {

            $('.overlay-bg').show();
            lnkId = obj.href;
            $("#lnkSend").attr("href", lnkId)
            return false;
        }

        function Add() {

            $('.overlay-bg').hide();
            $("#lnkSend").click();
            var ans = confirm("This action will delete selected items?");
            if (ans == true) {
                return true;
            }
            else {
                //alert("no");
                return false;
            }
        }

        $('.close-btn').click(function () {
            $('.overlay-bg').hide(); // hide the overlay
        });

        function viewMom() {
           
            $("#lnkViewMom").click();
        }
    </script>
    <article class="content-box minimizer">
            <header>        
                <h2>&nbsp; </h2>                
            <%--    <h2>Pending Details</h2>    --%>
                        </header>
        <section>
                   <div class='overlay-bg' style="z-index:8"> <div class='overlay-content'>
                        <h2><label id="lblAgendaTitle">Decline Reason</label></h2>
                    <h3><b>Note: You have selected "Decline" option. This action will completely delete the selected item including its linked items, such as user, meeting, forum, notice, agenda, PDFs, minutes as applicable. The Maker will have to create the entire item again afresh. If you still wish to go ahead, please provide the reason for decline in the box below and then click on Decline. Else click on Close.</b></h3>
           <p> <asp:TextBox ID="txtDecline" Width="630px" Height="170px" TextMode="MultiLine" runat="server" ClientIDMode="Static"></asp:TextBox> 
 </p>
     <p> <a id="lnkSend" href="#" >Decline </a> &nbsp; &nbsp; &nbsp;
 <%--           <asp:Button Text="Save" OnClientClick="Add();" ID="btnSave" runat="server" />--%>
         <a id="lnkClose" class="close-btn" href="javascript:void(0)"  onclick="$('.overlay-bg').hide();"  >Close </a>
        <%--   <asp:Button ID="btnClose" runat="server" CssClass="close-btn" Text="Close" /> --%> </p>
   
       </div>
    </div>
<div style="z-index:5">
    <fieldset>
            <legend><b>Pending list</b></legend>
                             <dl>
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                 
                               <div style="margin-bottom:15px">
                                    <%--<img width="16" height="16" src="img/icons/icon_list_style_cross.png" alt="" style="vertical-align: middle"/> --%>
                                  
                               </div>
                               <div style="margin-bottom:15px">
                               <asp:Label ID="lblTotalRecord" runat="server" Font-Bold="True"></asp:Label>
                               </div>  
                             <%--  <div style="margin-bottom:15px"> <asp:Label  ID="lblUser" runat="server" Text="Search User" Font-Bold="true" ></asp:Label> </div>  --%>
                             <div>
                               <div>
                                  <asp:UpdatePanel id="ResultsUpdatePanel" runat="server">   
                                 <ContentTemplate>
                                    <div style="text-align:center;">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel" DynamicLayout="true">
                        <ProgressTemplate>

                           <img src="../img/jquery/ajaxLoader.gif">

                        </ProgressTemplate>
                    </asp:UpdateProgress>
                                <%-- <table width="75%">
                                  <tr>
                                     <td style="width:95px;">
                                    Select Master
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                  <asp:DropDownList AutoPostBack="true"  ID="ddlMaster" runat="server" 
                                             onselectedindexchanged="ddlMaster_SelectedIndexChanged">--%>
                                            <%-- <asp:ListItem Text="Select Master" Value="0"></asp:ListItem>--%>
                                            <%-- <asp:ListItem Text="Entity" Value="Entity"></asp:ListItem>--%>
                                          <%--   <asp:ListItem Text="Forum" Value="Forum"></asp:ListItem>

                                             <asp:ListItem Text="Meeting" Value="Meeting"></asp:ListItem>
                                                 <asp:ListItem Text="User" Value="User"></asp:ListItem>
                                                 <asp:ListItem Text="Notice" Value="Notice"></asp:ListItem>
                                                 <asp:ListItem Text="Agenda" Value="Agenda"></asp:ListItem>
                                               <asp:ListItem Text="Upload Minutes" Value="Minutes"></asp:ListItem> 
                                             </asp:DropDownList>
                                       
                                     </td>--%>
                                   <%--  <td  style="width:250px;" align="left">
                                  <asp:Button ID="btnGetApproval" runat="server" Text="Show Approval" />
                                     </td>--%>
                                    <%-- </tr>
                                     </table>--%>

                             </div> 
                                     
<span> <b>View :</b> &nbsp;  <asp:RadioButtonList ID="rdbCheckers" CssClass="rdbChecker" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true" runat="server" OnSelectedIndexChanged="rdbCheckers_SelectedIndexChanged">
    <asp:ListItem Selected="True" Text="Dashbord" Value="dashbord"></asp:ListItem>
    <asp:ListItem  Text="History" Value="history"></asp:ListItem>
</asp:RadioButtonList>   </span>                      
                                  <br />
                                     
                            <div class="holder">
                            <div>
                                 <div class="box_top">
        
        
<h2 >Approval </h2>
        
    
</div>
                                 <div class="box_content">  
<div class="dataTables_wrapper">
                             <asp:GridView ID="grdApprove" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="EntityId"                                
                                    onsorting="grdApprove_Sorting" onrowcommand="grdApproveRowCommand" 
                                    onpageindexchanging="grdApprove_PageIndexChanging">
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

                                    <asp:TemplateField HeaderText="Type" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="Department"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Department" SortExpression="EntityName" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblEntity" runat="server" Text='<%# Eval("EntityName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Created By" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnView" CommandArgument='<%# Bind("EntityId") %>' CausesValidation="false" CommandName="view" Text="View Details" runat="server" ToolTip="View Details"></asp:LinkButton>
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Enable in Ipad" >
                                        <ItemTemplate>
                                       <asp:CheckBox ID="chkEnable"  Checked="true" runat="server" />
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- <asp:TemplateField HeaderText="Status" >
                                        <ItemTemplate>
                                       <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("IsApproved") %>'></asp:Label>
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                     <asp:TemplateField HeaderText="Action" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnApprove" CommandArgument='<%# Bind("EntityId") %>' CausesValidation="false" CommandName="Approve" Text="Approve" runat="server" ToolTip="Approve"></asp:LinkButton>
                                           &nbsp; &nbsp;  <asp:LinkButton CssClass="AddView" ID="lbnReject" CommandArgument='<%# Bind("EntityId") %>' OnClientClick="return ShowPopUp(this);" CausesValidation="false" CommandName="Decline" Text="Decline" runat="server" ToolTip="Decline"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                                
                                 </Columns>
                                </asp:GridView>
    </div></div>

                            </div>

<div class="left_spacer">                               <div><h4 class="title">Forums for approval</h4></div>
                              <div>
                                   <div class="box_content">    
<div class="dataTables_wrapper">
                             <asp:GridView ID="grdForum" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="ForumId"                                
                                    onsorting="grdForum_Sorting" onrowcommand="grdForumRowCommand" 
                                      onpageindexchanging="grdForum_PageIndexChanging">
<HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <PagerStyle CssClass="paginate_active"   />
                                  <RowStyle CssClass="gradeA odd" />
                                  <Columns>
                                     <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Type" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="Forum"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Department" SortExpression="EntityName" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblEntity" runat="server" Text='<%# Bind("EntityName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Forum" SortExpression="ForumName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblForum" runat="server" Text='<%# Bind("ForumName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Created By" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                           <asp:LinkButton   CssClass="AddView" ID="lbnView" CommandArgument='<%# Bind("ForumId") %>' CausesValidation="false" CommandName="View" Text="View Details" runat="server" ToolTip="View Details"></asp:LinkButton>
                                            <asp:HiddenField ID="hdnMaker" runat="server" Value='<%# Eval("UpdatedBy") %>' />
                                       
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Enable in Ipad" >
                                        <ItemTemplate>
                                       <asp:CheckBox ID="chkEnable" Checked="true" runat="server" />
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Maker Notification" >
                                        <ItemTemplate>
                                       Send <asp:CheckBox ID="chkNotify"   runat="server" />                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Action" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnApprove" CommandArgument='<%# Bind("ForumId") %>' CausesValidation="false" CommandName="Approve" Text="Approve" runat="server" ToolTip="Approve"></asp:LinkButton>

                                           <br /> <asp:LinkButton CssClass="AddView" ID="lbnReject" CommandArgument='<%# Bind("ForumId") %>'  OnClientClick="return ShowPopUp(this);"  CausesValidation="false" CommandName="Decline" Text="Decline" runat="server" ToolTip="Decline"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                                   
                                 </Columns>
                                </asp:GridView>
  

                                          <asp:GridView ID="grdForumPending" runat="server" AutoGenerateColumns="False" 
                                 AllowPaging="true" PageSize="10"   CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" 
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable"  DataKeyNames="ForumId"                                
                                    OnPageIndexChanging="grdForumPending_PageIndexChanging" onrowcommand="grdForumRowCommand" 
                        >
<HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <PagerStyle CssClass="paginate_active"   />
                                  <RowStyle CssClass="gradeA odd" />
                                  <Columns>
                                     <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Type" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="Forum"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                <%--     <asp:TemplateField HeaderText="Entity" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblEntity" runat="server" Text='<%# MM.Core.Encryptor.DecryptString(Eval("EntityName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                      <asp:TemplateField HeaderText="Forum"  >
                                        <ItemTemplate>
                                            <asp:Label ID="lblForum" runat="server" Text='<%# MM.Core.Encryptor.DecryptString(Eval("ForumName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Created By"  >
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# MM.Core.Encryptor.DecryptString(Eval("FirstName").ToString()) +" "+ MM.Core.Encryptor.DecryptString(Eval("LastName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnView" CommandArgument='<%# Bind("ForumId") %>' CausesValidation="false" CommandName="View" Text="View Details" runat="server" ToolTip="View Details"></asp:LinkButton>                                       
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         
                                     <asp:TemplateField HeaderText="Status" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblSataus" runat="server" Text='<%#Eval("IsApproved") %>'></asp:Label>                                           
                                             </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Reason" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblReason" runat="server" Text='<%#Eval("DeclineReason") %>'></asp:Label>                                           
                                             </ItemTemplate>
                                    </asp:TemplateField>
                                      
                                              <asp:TemplateField HeaderText="Checker" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblCheckerName" runat="server" Text='<%#  MM.Core.Encryptor.DecryptString(Eval("CheckerFirstName").ToString()) +" "+  MM.Core.Encryptor.DecryptString(Eval("CheckerLastName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>   
                                 </Columns>
                                </asp:GridView>
    </div>
                                       </div>
                            </div>
                                     <div><h4 class="title">Meetings for approval</h4></div>
                                <div>
                                     <div class="box_content">  
<div class="dataTables_wrapper">
                             <asp:GridView ID="grdMeeting" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="MeetingId"                                
                                    onsorting="grdMeeting_Sorting" onrowcommand="grdMeetingCommand" 
                                        onpageindexchanging="grdMeeting_PageIndexChanging">
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

                                    <asp:TemplateField HeaderText="Type" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="Meeting"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Department" SortExpression="EntityName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTpe" runat="server" Text='<%# Bind("EntityName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 <%--    <asp:TemplateField HeaderText="Forum" SortExpression="ForumName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblype" runat="server" Text='<%# Bind("ForumName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                      <asp:TemplateField HeaderText="Meeting" SortExpression="MeetingVenue">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTe" runat="server" Text='<%# Eval("MeetingNumber") +" "+ Eval("ForumName")+" "+ Convert.ToDateTime(Eval("MeetingDate")).ToString("MMMM dd, yyyy") %>'></asp:Label>
                                         <%--Text='<%# Convert.ToDateTime(Eval("MeetingDate")).ToString("MMM d yyyy") +" "+ Eval("MeetingTime") +" "+ Eval("MeetingVenue") %>'--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                               <%--       <asp:TemplateField HeaderText="Meeting Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeetingType" runat="server" Text='<%# Eval("MeetingType")  %>'></asp:Label>
                                               </ItemTemplate>
                                    </asp:TemplateField>--%>


                                    <asp:TemplateField HeaderText="Created By" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnView" CommandArgument='<%# Eval("MeetingId")+","+Eval("EntityName")+","+Eval("ForumName") %>' CausesValidation="false" CommandName="view" Text="View Details" runat="server" ToolTip="View Details"></asp:LinkButton>
                                       
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Enable in Ipad" >
                                        <ItemTemplate>
                                       <asp:CheckBox ID="chkEnable" Checked="true" runat="server" />                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Maker Notification" >
                                        <ItemTemplate>
                                    Send   <asp:CheckBox ID="chkNotify"   runat="server" /> 
                                             <asp:HiddenField ID="hdnMaker" runat="server" Value='<%# Eval("UpdatedBy") %>' />                                         
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Action" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnApprove" CommandArgument='<%# Bind("MeetingId") %>' CausesValidation="false" CommandName="Approve" Text="Approve" runat="server" ToolTip="Approve"></asp:LinkButton>
                                           &nbsp; &nbsp; <asp:LinkButton CssClass="AddView"  OnClientClick="return ShowPopUp(this);"  ID="lbnReject" CommandArgument='<%# Bind("MeetingId") %>' CausesValidation="false" CommandName="Decline" Text="Decline" runat="server" ToolTip="Decline"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                             
                                 </Columns>
                                </asp:GridView>

                                       <asp:GridView ID="grdMeetingPending" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found"  
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable"  DataKeyNames="MeetingId"                                
                                   onrowcommand="grdMeetingCommand" 
                                        onpageindexchanging="grdMeetingPending_PageIndexChanging">
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

                                    <asp:TemplateField HeaderText="Type" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="Meeting"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  <%--   <asp:TemplateField HeaderText="Entity"  >
                                        <ItemTemplate>
                                            <asp:Label ID="lblTpe" runat="server" Text='<%#   MM.Core.Encryptor.DecryptString(Eval("EntityName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                  <%--   <asp:TemplateField HeaderText="Forum" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblype" runat="server" Text='<%#  MM.Core.Encryptor.DecryptString(Eval("ForumName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                      <asp:TemplateField HeaderText="Meeting" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblTe" runat="server" Text='<%# MM.Core.Encryptor.DecryptString(Eval("MeetingNumber").ToString()) +" "+ MM.Core.Encryptor.DecryptString(Eval("ForumName").ToString())+" "+ Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("MMMM dd, yyyy") %>'></asp:Label>
                                            <%--Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("MMM d yyyy") +" "+   MM.Core.Encryptor.DecryptString(Eval("MeetingTime").ToString()) +" "+   MM.Core.Encryptor.DecryptString(Eval("MeetingVenue").ToString()) %>'></asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <%--   <asp:TemplateField HeaderText="Meeting Type">
                                        <ItemTemplate>
                                         <asp:Label ID="lblMeetingType" runat="server" Text='<%# Eval("MeetingType")  %>'></asp:Label> 
                                   Text='<%# Convert.ToDateTime(Eval("MeetingDate")).ToString("MMM d yyyy") +" "+ Eval("MeetingTime") +" "+ Eval("MeetingVenue") %>'
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>


                                    <asp:TemplateField HeaderText="Created By"  >
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#   MM.Core.Encryptor.DecryptString(Eval("FirstName").ToString()) +" "+   MM.Core.Encryptor.DecryptString(Eval("LastName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnView" CommandArgument='<%#   Eval("MeetingId").ToString()+","+  MM.Core.Encryptor.DecryptString(Eval("EntityName").ToString())+","+  MM.Core.Encryptor.DecryptString(Eval("ForumName").ToString()) %>' CausesValidation="false" CommandName="view" Text="View Details" runat="server" ToolTip="View Details"></asp:LinkButton>
                                       
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 

                                       <asp:TemplateField HeaderText="Status" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblSataus" runat="server" Text='<%#Eval("IsApproved") %>'></asp:Label>                                           
                                             </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Reason" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblReason" runat="server" Text='<%#Eval("DeclineReason") %>'></asp:Label>                                           
                                             </ItemTemplate>
                                    </asp:TemplateField>
                                      
                                              <asp:TemplateField HeaderText="Checker" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblCheckerName" runat="server" Text='<%#  MM.Core.Encryptor.DecryptString(Eval("CheckerFirstName").ToString()) +" "+  MM.Core.Encryptor.DecryptString(Eval("CheckerLastName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>   
                                                             
                                 </Columns>
                                </asp:GridView>
    </div>
                                         </div>
                            </div>
                                     <div><h4 class="title">Users for approval</h4></div>
                                  <div>
                                       <div class="box_content scroll">    
<div class="dataTables_wrapper">
                             <asp:GridView ID="grdUser" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red"  EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="UserId"  Width="100%"                               
                                    onsorting="grdUserSorting" onrowcommand="grdUserCommand" 
                                        onpageindexchanging="grdUser_PageIndexChanging">
                                 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd"  />
                                  <PagerStyle CssClass="paginate_active"   />
                                 
                                  <Columns>
                                     <asp:TemplateField  HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Type" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="User"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Image" >
                                        <ItemTemplate>
                                         <img id="UserImg" height="50px" width="100px" src="img/Uploads/ProfilePic/<%# Eval("Photograph") %>" alt="Image Not Available" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField  HeaderText="Name" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTpe"  runat="server" Text='<%#Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Designation" SortExpression="Designation">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignation" runat="server" Text='<%# Bind("Designation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  
                                    <asp:TemplateField HeaderText="Created By" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("CreaterFirstName") +" "+ Eval("CreaterLastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnView" CommandArgument='<%# Bind("UserId") %>' CausesValidation="false" CommandName="view" Text="View Details" runat="server" ToolTip="View Details"></asp:LinkButton>
                                         
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Send Email" >
                                        <ItemTemplate>
                                       <asp:CheckBox ID="chkEnable"  runat="server" />
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Maker Notification" >
                                        <ItemTemplate>
                                       Send <asp:CheckBox ID="chkNotify"   runat="server" />    
                                               <asp:HiddenField ID="hdnMaker" runat="server" Value='<%# Eval("UpdatedBy") %>' />                                                                               
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Action" >
                                        <ItemTemplate >
                                           
                                           <asp:LinkButton CssClass="AddView" ID="lbnApprove" CommandArgument='<%# Bind("UserId") %>' CausesValidation="false" CommandName="Approve" Text="Approve" runat="server" ToolTip="Approve"></asp:LinkButton>
                                             <asp:LinkButton CssClass="AddView" ID="lbnReject"  OnClientClick="return ShowPopUp(this);"  CommandArgument='<%# Bind("UserId") %>' CausesValidation="false" CommandName="Decline" Text="Decline" runat="server" ToolTip="Decline"></asp:LinkButton>
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                             
                                 </Columns>
                                </asp:GridView>

                                              <asp:GridView ID="grdUserPending" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found"  
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red"  EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" DataKeyNames="UserId"  Width="100%"                               
                                    onrowcommand="grdUserCommand" 
                                        onpageindexchanging="grdUserPending_PageIndexChanging">
                                 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd"  />
                                  <PagerStyle CssClass="paginate_active"   />
                                 
                                  <Columns>
                                     <asp:TemplateField  HeaderText="Sr. No.">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Type" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="User"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Image" >
                                        <ItemTemplate>
                                         <img id="UserImg" height="50px" width="100px" src="img/Uploads/ProfilePic/<%#  MM.Core.Encryptor.DecryptString(Eval("Photograph").ToString()) %>" alt="Image Not Available" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField  HeaderText="Name" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblTpe"  runat="server" Text='<%#  MM.Core.Encryptor.DecryptString(Eval("FirstName").ToString()) +" "+  MM.Core.Encryptor.DecryptString(Eval("LastName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Designation" SortExpression="Designation">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignation" runat="server" Text='<%#  MM.Core.Encryptor.DecryptString(Eval("Designation").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  
                                    <asp:TemplateField HeaderText="Created By" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#  MM.Core.Encryptor.DecryptString(Eval("CreaterFirstName").ToString()) +" "+  MM.Core.Encryptor.DecryptString(Eval("CreaterLastName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnView" CommandArgument='<%# Bind("UserId") %>' CausesValidation="false" CommandName="view" Text="View Details" runat="server" ToolTip="View Details"></asp:LinkButton>
                                         
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  
                                                
                                     <asp:TemplateField HeaderText="Status" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblSataus" runat="server" Text='<%#Eval("IsApproved") %>'></asp:Label>                                           
                                             </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Reason" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblReason" runat="server" Text='<%#Eval("DeclineReason") %>'></asp:Label>                                           
                                             </ItemTemplate>
                                    </asp:TemplateField>
                                      
                                              <asp:TemplateField HeaderText="Checker" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblCheckerName" runat="server" Text='<%#  MM.Core.Encryptor.DecryptString(Eval("CheckerFirstName").ToString()) +" "+  MM.Core.Encryptor.DecryptString(Eval("CheckerLastName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                             
                                 </Columns>
                                </asp:GridView>

    </div>
                                           </div>
                            </div>
                                     <div><h4 class="title">Notice for approval</h4></div>
                             <div>
                                  <div class="box_content"> 
<div class="dataTables_wrapper">
                             <asp:GridView ID="grdNotice" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="NoticeId"                                
                                    onsorting="grdNotice_Sorting" onrowcommand="grdNoticeRowCommand" 
                                    onpageindexchanging="grdNotice_PageIndexChanging">
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

                                    <asp:TemplateField HeaderText="Type" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="Notice"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Meeting" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeeting"  style="text-align:justify" Width="200px"  runat="server" Text='<%# Eval("MeetingNumber") +" "+ Eval("ForumName")+" "+ Convert.ToDateTime(Eval("MeetingDate")).ToString("MMMM dd, yyyy") %>'></asp:Label>
                                                <%--<%# Convert.ToDateTime(Eval("MeetingDate")).ToString("MMM d yyyy") +" "+ Eval("MeetingTime") +" "+ Eval("MeetingVenue") %>'--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Created By" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnView" CommandArgument='<%# Eval("NoticeId")+","+Eval("MeetingDate")+","+ Eval("MeetingTime") +","+ Eval("MeetingVenue")  %>' CausesValidation="false" CommandName="view" Text="View Details" runat="server" ToolTip="View Details"></asp:LinkButton>                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Enable in Ipad" Visible="false" >
                                        <ItemTemplate>
                                       <asp:CheckBox ID="chkEnable" Checked="true" runat="server" />
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Maker Notification" >
                                        <ItemTemplate>
                                       Send <asp:CheckBox ID="chkNotify"   runat="server" />  
                                               <asp:HiddenField ID="hdnMaker" runat="server" Value='<%# Eval("UpdatedBy") %>' />                                                                                 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Action" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnApprove" CommandArgument='<%# Bind("NoticeId") %>' CausesValidation="false" CommandName="Approve" Text="Approve" runat="server" ToolTip="Approve"></asp:LinkButton>
                                           &nbsp; &nbsp;  <asp:LinkButton CssClass="AddView"  OnClientClick="return ShowPopUp(this);"  ID="lbnReject" CommandArgument='<%# Bind("NoticeId") %>' CausesValidation="false" CommandName="Decline" Text="Decline" runat="server" ToolTip="Decline"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                                
                                 </Columns>
                                </asp:GridView>

                                   <asp:GridView ID="grdNoticePending" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found"  
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable"  DataKeyNames="NoticeId" onrowcommand="grdNoticeRowCommand" 
                                    onpageindexchanging="grdNoticePending_PageIndexChanging">
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

                                    <asp:TemplateField HeaderText="Type" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="Notice"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Meeting" SortExpression="MeetingVenue" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeeting"  style="text-align:justify" Width="200px"  runat="server" Text='<%# MM.Core.Encryptor.DecryptString(Eval("MeetingNumber").ToString()) +" "+ MM.Core.Encryptor.DecryptString(Eval("ForumName").ToString())+" "+ Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("MMMM dd, yyyy") %>'></asp:Label>
                                                <%--<%# Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("MMM d yyyy") +" "+  MM.Core.Encryptor.DecryptString(Eval("MeetingTime").ToString()) +" "+  MM.Core.Encryptor.DecryptString(Eval("MeetingVenue").ToString()) %>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnView" CommandArgument='<%# Eval("NoticeId")+","+ MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())+","+  MM.Core.Encryptor.DecryptString(Eval("MeetingTime").ToString()) +","+  MM.Core.Encryptor.DecryptString(Eval("MeetingVenue").ToString())  %>' CausesValidation="false" CommandName="view" Text="View Details" runat="server" ToolTip="View Details"></asp:LinkButton>                                          
                                        </ItemTemplate>
                                    </asp:TemplateField> 

        <asp:TemplateField HeaderText="Created By" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# MM.Core.Encryptor.DecryptString(Eval("FirstName").ToString()) +" "+ MM.Core.Encryptor.DecryptString(Eval("LastName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 <%--    <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                           <asp:LinkButton ID="lbnView" CommandArgument='<%# Bind("ForumId") %>' CausesValidation="false" CommandName="View" Text="View Details" runat="server" ToolTip="View Details"></asp:LinkButton>                                       
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                         
                                     <asp:TemplateField HeaderText="Status" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblSataus" runat="server" Text='<%#Eval("IsApproved") %>'></asp:Label>                                           
                                             </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Reason" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblReason" runat="server" Text='<%#Eval("DeclineReason") %>'></asp:Label>                                           
                                             </ItemTemplate>
                                    </asp:TemplateField>
                                      
                                              <asp:TemplateField HeaderText="Checker" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblCheckerName" runat="server" Text='<%#  MM.Core.Encryptor.DecryptString(Eval("CheckerFirstName").ToString()) +" "+  MM.Core.Encryptor.DecryptString(Eval("CheckerLastName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                                                
                                 </Columns>
                                </asp:GridView>
    </div>
                                      </div>
                            </div>
                                      <div><h4 class="title">Agenda for approval</h4></div>
                              <div>
                                   <div class="box_content">    
<div class="dataTables_wrapper">
                             <asp:GridView ID="grdAgenda" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="MeetingId"                                
                                    onsorting="grdAgenda_Sorting" onrowcommand="grdAgendaCommand" 
                                        onpageindexchanging="grdAgenda_PageIndexChanging">
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

                                    <asp:TemplateField HeaderText="Type" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="Agenda"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Department" SortExpression="EntityName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTpe" runat="server" Text='<%# Bind("EntityName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <%--  <asp:TemplateField HeaderText="Forum" SortExpression="ForumName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblype" runat="server" Text='<%# Bind("ForumName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                      <asp:TemplateField HeaderText="Meeting" SortExpression="MeetingVenue">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTe" runat="server" Text='<%# Eval("MeetingNumber") +" "+ Eval("ForumName")+" "+ Convert.ToDateTime(Eval("MeetingDate")).ToString("MMMM dd, yyyy") %>'></asp:Label>
                                                <%--<%# Convert.ToDateTime(Eval("MeetingDate")).ToString("MMM d yyyy") +" "+ Eval("MeetingTime") +" "+ Eval("MeetingVenue") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Created By" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnView" CommandArgument='<%# Eval("MeetingId") %>' CausesValidation="false" CommandName="view" Text="View Details" runat="server" ToolTip="View Details"></asp:LinkButton>
                                              <asp:HiddenField ID="hdnChekerUserId" runat="server" Value='<%# Session["UserId"].ToString() %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                          <asp:TemplateField Visible="false" HeaderText="Enable in Ipad" >
                                        <ItemTemplate>
                                       <asp:CheckBox ID="chkEnable" Checked="true" runat="server" />
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Maker Notification" >
                                        <ItemTemplate>
                                       Send <asp:CheckBox ID="chkNotify"   runat="server" />                                             
                                               <asp:HiddenField ID="hdnMaker" runat="server" Value='<%# Eval("UpdatedBy") %>' />                                             
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Action" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnApprove" CommandArgument='<%# Bind("MeetingId") %>' CausesValidation="false" CommandName="Approve" Text="Approve" runat="server" ToolTip="Approve"></asp:LinkButton>
                                           &nbsp; &nbsp; <asp:LinkButton CssClass="AddView" OnClientClick="return ShowPopUp(this);"  ID="lbnReject" CommandArgument='<%# Bind("MeetingId") %>' CausesValidation="false" CommandName="Decline" Text="Decline" runat="server" ToolTip="Decline"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                             
                                 </Columns>
                                </asp:GridView>

                                     <asp:GridView ID="grdAgendaPending" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found"  
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable"   DataKeyNames="MeetingId"                                
                                     onrowcommand="grdAgendaCommand" 
                                        onpageindexchanging="grdAgendaPending_PageIndexChanging">
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

                                    <asp:TemplateField HeaderText="Type" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="Agenda"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Department" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblTpe" runat="server" Text='<%#  MM.Core.Encryptor.DecryptString(Eval("EntityName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <%--  <asp:TemplateField HeaderText="Forum">
                                        <ItemTemplate>
                                            <asp:Label ID="lblype" runat="server" Text='<%# MM.Core.Encryptor.DecryptString(Eval("ForumName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                      <asp:TemplateField HeaderText="Meeting" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblTe" runat="server" Text='<%# MM.Core.Encryptor.DecryptString(Eval("MeetingNumber").ToString()) +" "+ MM.Core.Encryptor.DecryptString(Eval("ForumName").ToString())+" "+ Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("MMMM dd, yyyy") %>'></asp:Label>
                                                <%--<%#  Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("MMM d yyyy") +" "+  MM.Core.Encryptor.DecryptString(Eval("MeetingTime").ToString()) +" "+  MM.Core.Encryptor.DecryptString(Eval("MeetingVenue").ToString()) %>'></asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                                       

                                      <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                           <asp:LinkButton ID="lbnView" CommandArgument='<%# Eval("MeetingId") %>' CausesValidation="false" CommandName="view" Text="View Details" runat="server" ToolTip="View Details"></asp:LinkButton>
                                       
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  
                                    <%--<asp:TemplateField HeaderText="Created By" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# MM.Core.Encryptor.DecryptString(Eval("FirstName").ToString()) +" "+ MM.Core.Encryptor.DecryptString(Eval("LastName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                                                             
                                     <asp:TemplateField HeaderText="Status" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblSataus" runat="server" Text='<%#Eval("IsApproved") %>'></asp:Label>                                           
                                             </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Reason" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblReason" runat="server" Text='<%#Eval("DeclineReason") %>'></asp:Label>                                           
                                             </ItemTemplate>
                                    </asp:TemplateField>
                                      
                                              <asp:TemplateField HeaderText="Checker" >
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnChekerUserId" runat="server" Value='<%#Eval("checkerId") %>' />
                                            <asp:Label ID="lblCheckerName" runat="server" Text='<%#  MM.Core.Encryptor.DecryptString(Eval("CheckerFirstName").ToString()) +" "+  MM.Core.Encryptor.DecryptString(Eval("CheckerLastName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>  
                                 </Columns>
                                </asp:GridView>
    </div>
                                       </div>
                            </div>
                                    <div><h4 class="title">Minutes for approval</h4></div>
                              <div>
                                   <div class="box_content">    
<div class="dataTables_wrapper">
                             <asp:GridView ID="grdMinutes" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="UploadMinuteId"                                
                                    onsorting="grdMinutes_Sorting" onrowcommand="grdMinutesRowCommand" 
                                    onpageindexchanging="grdMinutes_PageIndexChanging">
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

                                    <asp:TemplateField HeaderText="Type" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="Upload Minutes"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Meeting" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblEntity" runat="server" Text='<%# Eval("MeetingNumber") +" "+ Eval("ForumName")+" "+ Convert.ToDateTime(Eval("MeetingDate")).ToString("MMMM dd, yyyy") %>'></asp:Label>
                                            <%--Convert.ToDateTime(Eval("MeetingDate")).ToString("MMM d yyyy") +" "+ Eval("MeetingTime") +" "+ Eval("MeetingVenue") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Created By" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnView" CommandArgument='<%# Eval("UploadMinuteId") +","+Eval("MeetingDate") +","+ Eval("MeetingTime") +","+ Eval("MeetingVenue") %>' CausesValidation="false" CommandName="view" Text="View Details" runat="server" ToolTip="View Details"></asp:LinkButton>
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Enable in Ipad" >
                                        <ItemTemplate>
                                       <asp:CheckBox ID="chkEnable" Checked="true" runat="server" />
                                             <asp:HiddenField ID="hdnMaker" runat="server" Value='<%# Eval("UpdatedBy") %>' />                                         
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Maker Notification" >
                                        <ItemTemplate>
                                       Send <asp:CheckBox ID="chkNotify"   runat="server" />                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Action" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnApprove" CommandArgument='<%# Bind("UploadMinuteId") %>' CausesValidation="false" CommandName="Approve" Text="Approve" runat="server" ToolTip="Approve"></asp:LinkButton>
                                           &nbsp; &nbsp;  <asp:LinkButton CssClass="AddView" OnClientClick="return ShowPopUp(this);"  ID="lbnReject" CommandArgument='<%# Bind("UploadMinuteId") %>' CausesValidation="false" CommandName="Decline" Text="Decline" runat="server" ToolTip="Decline"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                                
                                 </Columns>
                                </asp:GridView>

                                    <asp:GridView ID="grdMinutesPending" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found"  
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable"  DataKeyNames="UploadMinuteId"                                
                                    onrowcommand="grdMinutesRowCommand" 
                                    onpageindexchanging="grdMinutesPending_PageIndexChanging">
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

                                    <asp:TemplateField HeaderText="Type" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="Upload Minutes"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Meeting"   >
                                        <ItemTemplate>
                                            <asp:Label ID="lblEntity" runat="server" Text='<%# MM.Core.Encryptor.DecryptString(Eval("MeetingNumber").ToString()) +" "+ MM.Core.Encryptor.DecryptString(Eval("ForumName").ToString())+" "+ Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("MMMM dd, yyyy") %>'></asp:Label> 
                                            <%--Convert.ToDateTime( MM.Core.Encryptor.DecryptString(Eval("MeetingDate"  ).ToString())).ToString("MMM d yyyy") +" "+  MM.Core.Encryptor.DecryptString(Eval("MeetingTime").ToString()) +" "+  MM.Core.Encryptor.DecryptString(Eval("MeetingVenue").ToString()) %>'></asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                                        
                                     <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                           <asp:LinkButton CssClass="AddView" ID="lbnView" CommandArgument='<%# Eval("UploadMinuteId") +","+ MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString()) +","+  MM.Core.Encryptor.DecryptString(Eval("MeetingTime").ToString()) +","+  MM.Core.Encryptor.DecryptString(Eval("MeetingVenue").ToString()) %>' CausesValidation="false" CommandName="view" Text="View Details" runat="server" ToolTip="View Details"></asp:LinkButton>
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      
                                    <asp:TemplateField HeaderText="Created By" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# MM.Core.Encryptor.DecryptString(Eval("FirstName").ToString()) +" "+ MM.Core.Encryptor.DecryptString(Eval("LastName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                                                             
                                     <asp:TemplateField HeaderText="Status" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblSataus" runat="server" Text='<%#Eval("IsApproved") %>'></asp:Label>                                           
                                             </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Reason" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblReason" runat="server" Text='<%#Eval("DeclineReason") %>'></asp:Label>                                           
                                             </ItemTemplate>
                                    </asp:TemplateField>
                                      
                                              <asp:TemplateField HeaderText="Checker" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblCheckerName" runat="server" Text='<%#  MM.Core.Encryptor.DecryptString(Eval("CheckerFirstName").ToString()) +" "+  MM.Core.Encryptor.DecryptString(Eval("CheckerLastName").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>  
                                                                
                                 </Columns>
                                </asp:GridView>
    </div>
                                       </div>
                                       </div>
                            </div>
                            </div>
                            </ContentTemplate>
                                    <Triggers>
                               <%--<asp:AsyncPostBackTrigger ControlID="ddlMaster" />--%>
                               <asp:PostBackTrigger ControlID="grdMeeting" />
                               <asp:PostBackTrigger ControlID="grdForum" />
                               <asp:PostBackTrigger ControlID="grdApprove" />
                               <asp:PostBackTrigger ControlID="grdUser" />
                               <asp:PostBackTrigger ControlID="grdNotice" />
                                 <asp:PostBackTrigger ControlID="grdAgenda" />
                                 <asp:PostBackTrigger ControlID="grdMinutes"/>

                               <asp:PostBackTrigger ControlID="grdForumPending" />
                               <asp:PostBackTrigger ControlID="grdMeetingPending" />
                               <asp:PostBackTrigger ControlID="grdUserPending" />
                               <asp:PostBackTrigger ControlID="grdNoticePending" />
                                 <asp:PostBackTrigger ControlID="grdAgendaPending" />
                                 <asp:PostBackTrigger ControlID="grdMinutesPending"/>
                                  <asp:PostBackTrigger ControlID="lnkViewMom" />
   <asp:PostBackTrigger ControlID="rdbCheckers"/>
                                        
    </Triggers>
</asp:UpdatePanel>
</div>
</div>
                            </dl>

               <div ID="divdetails" runat="server" style="margin-bottom:15px">
                                        
                     </div>
                              
                                <div runat="server" clientidmode="Static" style="display:none;height:200px !important; overflow:scroll;width:700px !important;" id="dialog">
<asp:Label ID="lblDetails"  Font-Names="Rupee"  runat="server" class="popUp_form"></asp:Label>
</div>
                          
            </div>
   <asp:Button ID="lnkViewMom" style="display:none;" ClientIDMode="Static" runat="server" Text="View" OnClick="lnkViewMom_Click"></asp:Button>
        </section>
       
   </article>
</asp:Content>
