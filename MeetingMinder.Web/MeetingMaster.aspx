﻿<%@ Page Title="Meeting Master" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="MeetingMaster.aspx.cs" Inherits="MeetingMinder.Web.MeetingMaster" %>
<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">

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
    </script>
    <article class="content-box minimizer">
			<header>			
				<h2>Meeitng Details</h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#AE432E"><b>Meeting list</b></font></legend>
                            <dl>
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                 
                               <div style="margin-bottom:15px">
                                    <img width="16" height="16" src="img/icons/icon_list_style_cross.png" alt="" style="vertical-align: middle"/> 
                                    <asp:LinkButton runat="server" ID="lbRemoveSelected" Text="Remove All Selected Record" 
                                    OnClientClick="return confirm('Are you sure you want to delete selected items?');" onclick="lbRemoveSelected_Click"  ></asp:LinkButton>
                               </div>
                               <div style="margin-bottom:15px"><asp:Label ID="lblTotalRecord" runat="server" Font-Bold="True"></asp:Label></div>  
                             <%--  <div style="margin-bottom:15px"> <asp:Label  ID="lblMeeting" runat="server" Text="Search User" Font-Bold="true" ></asp:Label> </div>  --%>
                                                      
                              <div style="margin-bottom:15px">
                                         <asp:GridView ID="grdMeeting" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="MeetingId" OnRowDeleting="grdMeeting_RowDeleting"
                                 
                                    OnPageIndexChanging="grdMeeting_PageIndexChanging" 
                                    onsorting="grdMeeting_Sorting" onrowcommand="grdMeetingRowCommand" 
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

                                    <asp:TemplateField HeaderText="Meeting Venue" SortExpression="MeetingVenue">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeetingVenu" runat="server" Text='<%# Eval("MeetingVenue") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Meeting Date" SortExpression="MeetingDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeetingDate" runat="server" Text='<%# Eval("MeetingDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Meeting Time" SortExpression="MeetingTime">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeetingTime" runat="server" Text='<%# Eval("MeetingTime") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                                              <asp:TemplateField HeaderText="Agenda">
                <ItemStyle HorizontalAlign="Center" Width="10%" />
                <ItemTemplate>
                <asp:LinkButton ID="lbnState" CommandArgument='<%# Bind("MeetingId") %>' CausesValidation="false" CommandName="View" Text="Add/View" runat="server" ToolTip="Add OR View Agenda"></asp:LinkButton>
                    
                </ItemTemplate>
            </asp:TemplateField>
<%--

                                    
                                     <asp:TemplateField HeaderText=" Email" SortExpression="EmailId1">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClientEmail" runat="server" Text='<%#Eval("EmailID1") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <%--<asp:BoundField DataField="Description"  HeaderText="Description"  />--%>

                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                            <%--<asp:ImageButton ID="imgbtnView" CommandArgument='<%# Bind("MeetingId") %>' ImageUrl="~/img/icons/actions/page_white_find.png" 
                                                    CausesValidation="false" CommandName="View" Text="Edit" runat="server" ToolTip="Edit Item" />                                     --%>
                                            <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Bind("MeetingId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Bind("MeetingId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                            </div>

                            </dl>
                            </fieldset>
           
                     <div ID="divdetails" runat="server" style="margin-bottom:15px">
                    
                       <dt>
                                    <label>
                                   Meeting Venue <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtMeetingVenue" runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvMeetingVenu" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtMeetingVenue" Display="Dynamic" 
                                        ErrorMessage="Meeting Venue" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>

                                 <dt>
                                    <label>
                                   Meeting Date <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtMeetingDate" ClientIDMode="Static" runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvMeetingDate" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtMeetingDate" Display="Dynamic" 
                                        ErrorMessage="Meeting Date" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>

                                    <dt>
                                    <label>
                                   Meeting Time <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                                    <asp:TextBox ID="txtTime" ClientIDMode="Static" runat="server" MaxLength="149" Width="214px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvMeetingTime" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtTime" Display="Dynamic" 
                                        ErrorMessage="Meeting Time" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                </dd>
                                           <dt>
                                    <label>
                                    Meeting Checker <span>*</span> :
                                    </label>
                                </dt>
                                <dd>
                              <asp:DropDownList ID="ddlUser" runat="server" ></asp:DropDownList>
                                      <asp:RequiredFieldValidator ID="rfvddUser" runat="server" 
                                        class="invalid-side-note" ControlToValidate="ddlUser" Display="Dynamic" InitialValue="0"
                                        ErrorMessage="Select Checker" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                              
                                </dd>

                                     <dl>
                                     <asp:HiddenField ID="hdnMeetingId" runat="server" />
                                    </dl>
                                    <div id="divbutton" runat="server" style="margin-bottom:15px;margin-top:15px">
                                    <asp:Button ID="btnInsert" runat="server" Text="Save" ValidationGroup="a" onclick="btnInsert_Click1"  />
                                       <asp:Button ID="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click1"  />
                                    </div>
                                    </div>
                                </div>
                                </section>
                                </article>
</asp:Content>
