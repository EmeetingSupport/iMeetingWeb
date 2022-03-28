<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="MeetingRetention.aspx.cs" Inherits="MeetingMinder.Web.MeetingRetention" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(function () {
                $("#txtMeetingDate").datepicker({ minDate: 0, maxDate: "+11M +30D", dateFormat: "dd.mm.yy" });
                $('#txtMeetingDate').on('keydown', function (e) {
                    e.preventDefault();
                })
            })

            $('#txtTime').timepicker({
                showPeriod: true,
                showLeadingZero: true,
                timeSeparator: '.',
                //  timeFormat: 'hh mm p' 
            });

            $('#txtTime').on('keydown', function (e) {
                e.preventDefault();
            })
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;></h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#054a7f"><b>Meeting Retention</b></font></legend>
                            <dl>
                                   <br />
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
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
                     <div style="width:100%;">
                                 <table width="75%">
                                     <tr>
                                     <td style="width:95px;">
                                    Forum
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlForum" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlForum_SelectedIndexChanged"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Forum" Text="Please Select Forum" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>
                                      <tr>
                                        <td colspan="3">


                                        </td>
                                       </tr>
                                     </table>
                         </div>

                                     
                                         
                                     <div class="box_top">
		
		
<h2 class="icon users">Meeting Details</h2>
		
	
</div>
                              <div style="margin-bottom:15px">
                              
                                 <asp:GridView ID="grdMeeting" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="MeetingId"                                  
                                    OnPageIndexChanging="grdMeeting_PageIndexChanging" 
                                    onrowcommand="grdMeeting_RowCommand" 
                                    onrowediting="grdMeeting_RowEditing" 
                                      onrowcancelingedit="grdMeeting_RowCancelingEdit" 
                                      onrowupdating="grdMeeting_RowUpdating"
                                     onsorting="grdMeeting_Sorting">                                  
 <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                      <PagerStyle CssClass="paginate_active"   />
                                  <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Meeting" SortExpression="MeetingDate" >
                                        <ItemTemplate>
                                        <%--<asp:Label ID="Label1" runat="server" Text='<%# Eval("MeetingNumber").ToString() + " " + Eval("ForumName") +" "+  Convert.ToDateTime(Eval("MeetingDate")).ToString("D") +" "+ Eval("MeetingTime") %>' />--%>
                                        <asp:Label ID="lblKey" runat="server" Text='<%# Eval("MeetingNumber").ToString() + " " + Eval("ForumName") + ", " + Eval("MeetingVenue") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Meeting Date">
                                          <ItemTemplate>
                                              <asp:Label ID="lblMeetindDate" runat="server" Text='<%# Convert.ToDateTime(Eval("MeetingDate")).ToString("D")  %>'></asp:Label>
                                          </ItemTemplate>
                                          <EditItemTemplate>
                                              <asp:TextBox ID="txtMeetingDate" runat="server" ClientIDMode="Static" Text='<%# Convert.ToDateTime(Eval("MeetingDate")).ToString("dd.MM.yyyy") %>' Width="140px"></asp:TextBox>
                                              <asp:RequiredFieldValidator ID="rfvMeetingDate" runat="server" ControlToValidate="txtMeetingDate" Display="Dynamic" ValidationGroup="a" ErrorMessage="Value" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                          </EditItemTemplate>
                                      </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Meeting Time">
                                          <ItemTemplate>
                                              <asp:Label ID="lblMeetingTime" runat="server" Text='<%# Eval("MeetingTime") %>'></asp:Label>
                                          </ItemTemplate>
                                          <EditItemTemplate>
                                              <asp:TextBox ID="txtTime" runat="server" ClientIDMode="Static" Text='<%# Eval("MeetingTime") %>' Width="100px"></asp:TextBox>
                                              <asp:RequiredFieldValidator ID="rfvTime" runat="server" ControlToValidate="txtTime" Display="Dynamic" ValidationGroup="a" ErrorMessage="Value" Text="*Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                          </EditItemTemplate>
                                      </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Retention Period (Days)" SortExpression="PastMeeting">
                                        <ItemTemplate>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblVal" runat="server" Text='<%#Eval("PastMeeting").ToString()==""?"0":Eval("PastMeeting") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        <asp:TextBox ID="txtVal" runat="server" MaxLength="2" minimum="1" Text='<%#Eval("PastMeeting").ToString()==""?"0":Eval("PastMeeting") %>' style="width:100px" />
                                        <asp:HiddenField ID="hdnMeetingTime" runat="server" Value='<%#Eval("MeetingTime") %>' />
                                        <asp:HiddenField ID="hdnMeetingDate" runat="server" Value='<%#Eval("MeetingDate","{0:MM/dd/yy}") %>' />
                                         <asp:RegularExpressionValidator ID="regvalidation" runat="server" ControlToValidate="txtVal" ErrorMessage="Only numeric allowed." ForeColor="Red" ValidationExpression="^([2-9]|1[0-9])*$" ValidationGroup="a" Text="*Invalid"></asp:RegularExpressionValidator>                                                                                

                                         <asp:RequiredFieldValidator runat="server" ID="rfvNotice" ControlToValidate="txtVal" Display="Dynamic" ValidationGroup="a" ErrorMessage="Value" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>


                                    
                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Eval("MeetingId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" Visible='<%# Eval("PastMeetingExpiry").ToString()=="0"?false:true %>' />
                                            </ItemTemplate>
                                         <EditItemTemplate>
                                         <asp:LinkButton ID="lbnUpdate" runat="server" Text="Update"  ValidationGroup="a" CommandName="update" CommandArgument='<%# Eval("MeetingId") %>'></asp:LinkButton>
                                             <span>/</span>                                             
                                         <asp:LinkButton ID="lbnCancel" runat="server" Text="Cancel" CommandName="cancel" ></asp:LinkButton>
                                         &nbsp;&nbsp; 
                                         </EditItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                                 </div>
                            
                                        
                                 </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />                               
                               <asp:PostBackTrigger ControlID="grdMeeting" />
                             
                                   
    </Triggers>
</asp:UpdatePanel>
                                 </dl>
                                 
                                 </fieldset>
                                 </div>
                                 </section>
                                 </article>
</asp:Content>
