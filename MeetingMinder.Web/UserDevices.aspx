<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="UserDevices.aspx.cs" Inherits="MeetingMinder.Web.UserDevices" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
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
        function HidePopUp() {
            $("#hdnPopUp").val("0");
            $("#divEmail").hide();
        }

        function ShowPopUp() {
            if ($("#hdnPopUp").val() == "1") {
                $("#divEmail").show();
            }
            return false;
        }

        function ShowPopUpPublish() {

            if ($("#hdnPopUpPublish").val() == "1") {
                // $('.overlay-bg').show();
                $("#divAgendaPublish").show();
            }
            //lnkId = obj.href;
            //$("#lnkSend").attr("href", lnkId)
            return false;
        }

        function HidePopUpPublish() {
            //$('.close-btn').click(function () {

            $("#hdnPopUpPublish").val("0");
            //  $("#hdnAgenda").val("");
            //$('.overlay-bg').hide(); // hide the overlay
            $("#divAgendaPublish").hide();
            // });
        }

                function CheckCounts() {
            if ($('#<%= grdDevices.ClientID %> tr:not(:first-child) td:first-child').find('input[type="checkbox"]:checked').length != 0) {
                return confirm('Are you sure you want to delete selected items?');
            }
            else {
                alert("Please Select at least one checkbox");
                return false;
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
    </script>
    <style type="text/css">
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
            z-index: 10;
        }

        .overlay-content {
            background: #fff;
            padding: 1%;
            width: 75%;
            height: 400px;
            overflow: auto;
            max-height: 70%;
            position: relative;
            top: 15%;
            bottom: 10%;
            left: 22%;
            margin: 0 0 0 -10%; /* add negative left margin for half the width to center the div */
            cursor: default;
            border-radius: 4px;
            box-shadow: 0 0 5px rgba(0,0,0,0.9);
            z-index: 1000;
        }

        @media screen {
            .grid_24 {
                position: inherit !important;
            }
        }

        .close-btn {
            position: absolute;
            right: 25px;
            top: 10px;
            cursor: pointer;
        }

        .floatDiv {
            float: right;
            display: flex;
        }

        @media screen and (-ms-high-contrast: active), (-ms-high-contrast: none) {
            .floatDiv {
                float: right;
                display: flex;
                margin-top: 40px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--View Report--%></h2>			
			</header>
			<section> 

                    <div id="divAgendaPublish" class='overlay-bg'>
            <div class='overlay-content'>
               <fieldset  style="height:90%;width:800px !important;">
                   <legend><font color="#054a7f"><b>Change Password</b></font></legend>
                                   
                          <img src="img/icons/icon_list_style_cross.png"  onclick="HidePopUpPublish();"  alt="Close" class="close-btn" />
                    <table>
                        <tr>
                                        <td style="width:115px;">New Password :
                                    </td><td>
                                     <asp:TextBox ID="txtNew" CssClass="txtBox" TextMode="Password" runat="server" ></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvNew"  CssClass="errorpassword" ControlToValidate="txtNew" Display="Dynamic" ValidationGroup="a" ErrorMessage="Enter New Password"  >Enter Password</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="rgePassword" runat="server"
                                            ControlToValidate="txtNew" Display="Dynamic" ValidationGroup="a" ErrorMessage="Password must contain: Minimum 8 and Maximum 15 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character"
                                            SetFocusOnError="True"  
                                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$"></asp:RegularExpressionValidator>
                                        </td>

                            </tr><tr>
                                        <td style="width:115px;">Confirm Password :
                                    </td><td>
                                     <asp:TextBox ID="txtConfirm" CssClass="txtBox" TextMode="Password" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvConfirm" ValidationGroup="a" Display="Dynamic" ControlToValidate="txtConfirm" ErrorMessage=" Enter Confirm Password"
                                               CssClass="ErrorMsg">Enter Confirm Password</asp:RequiredFieldValidator>

                                        <asp:CompareValidator ID="cfvConfirm" runat="server" ControlToCompare="txtNew" ControlToValidate="txtConfirm" ErrorMessage="Confirm Password"
                                            Text="Password not match" CssClass="ErrorMsg" ValidationGroup="a" Display="Dynamic"></asp:CompareValidator>

                                        <asp:RegularExpressionValidator ID="rexConf" runat="server" CssClass="ErrorMsg"
                                            ControlToValidate="txtConfirm" Display="Dynamic" ValidationGroup="a" ErrorMessage="Password must contain: Minimum 8 and Maximum 15 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character"
                                            SetFocusOnError="True"  
                                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$"></asp:RegularExpressionValidator>
                                            </td>
                                 </tr><tr>
                                     <td style="width:115px;">
                                        <div class="fullwidth noBorder">

                                        <asp:Button ID="btnSearch"  CssClass="btnSave"  CausesValidation="true" ValidationGroup="a" runat="server" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                                         </div>
                                         </td>
                            </tr>
                      </table>
               </fieldset>
            </div>
         </div>

                      <div id="divEmail" class='overlay-bg'> <div class='overlay-content'>
                      <fieldset style="height:90%;width:900px;">
                        
							<legend><font color="#054a7f"><b>Send Email</b></font></legend>

                            
                                                      
                                   
                          <img src="img/icons/icon_list_style_cross.png"  onclick="HidePopUp();"  alt="Close" class="close-btn" />

                         
                                <table width="75%">
                                   <tr>
                                     <td style="width:115px;">
                                   To Email <label style="display:inline;margin:-2px;"><span>&nbsp;*</span></label>
                                     </td>
                                     <td style="width:20px;">
                                     :
                                     </td>
                                     <td  align="left">
                                      <asp:TextBox Width="450px" ID="txtSenderEmail" runat="server"  MaxLength="250"></asp:TextBox>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvSender" ControlToValidate="txtSenderEmail"   Display="Dynamic" ValidationGroup="ema" ErrorMessage="To Email"  ForeColor="Red">Invalid</asp:RequiredFieldValidator>
                                               </td>
                                     </tr>
                                    <tr>
                                     <td >
                                    CC
                                     </td>
                                     <td>
                                     :
                                     </td>
                                     <td  align="left">
                                         <asp:TextBox  Width="450px"  ID="txtcc" runat="server"  MaxLength="250"></asp:TextBox>
                                     </td>
                                     </tr>
                                     <tr>
                                     <td>Subject<label  style="display:inline;margin:-2px;"><span>&nbsp;*</span></label></td>
                                       <td>
                                     :
                                     </td>
                                     <td  align="left">
                                      <asp:TextBox  Width="450px" ID="txtSubject" runat="server"  MaxLength="250"></asp:TextBox>
                                 <asp:RequiredFieldValidator runat="server" ID="rfvSubject" ControlToValidate="txtSubject"   Display="Dynamic" ValidationGroup="ema" ErrorMessage="Subject"  ForeColor="Red">Invalid</asp:RequiredFieldValidator>
                              
                                     </td>
                                     </tr>
                                     <tr>
                                     <td >
                                    Body<label  style="display:inline;margin:-2px;"><span>&nbsp;*</span></label>
                                     </td>
                                     <td>
                                     :
                                     </td>
                                     <td  align="left">
                                    
                                            <asp:TextBox Height="150px"  Width="450px" ID="txtBody" TextMode="MultiLine" runat="server"  MaxLength="250"></asp:TextBox>
                                 <asp:RequiredFieldValidator runat="server" ID="rfvBody" ControlToValidate="txtBody"   Display="Dynamic" ValidationGroup="ema" ErrorMessage="Body"  ForeColor="Red">Invalid</asp:RequiredFieldValidator>
                                         
                                     </td>
                                     </tr>
                                                                          
                                     <tr align="right">
                                        <td colspan="5" align="right"> 
                                        <div class="fullwidth noBorder"> 
                                                 <asp:Button  ID="btnEmailSubmit"  CssClass="btnSave"   ValidationGroup="ema" runat="server" Text="Submit" onclick="btnEmailSubmit_Click"></asp:Button>
                                        </div>
                                           
                                 </td>
                                 </tr>
                                 </table>
                          </fieldset>
       </div>
    </div>             
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>View User Devices</b></font></legend>
                             <dl>
                                   

                             <div style="width:900px;">
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
                           
                           

                                <div >
                                 <table style="display:none" >

                                  
                                     <tr>
                                     <td style="width:95px;">
                                    User
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td    align="left">
                                     <asp:DropDownList Visible="false" ID="ddlUser" runat="server"  Width="50%" 
                                          onselectedindexchanged="ddlUser_SelectedIndexChanged" AutoPostBack="true"   ></asp:DropDownList>
                                     <%--<asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlUser" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Forum" Text="Please Select User" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                     
                                 
                                     </td>
                                     </tr>
                                      

                                     <tr id="trUsers" runat="server" visible="false">
                                     <td>Device list</td>
                                       <td width="20px">
                                     :
                                     </td>
                                     <td   align="left"> 
                                          <div class="box_top">
		
		
<h2 class="icon users">User Device List </h2>
		
	
</div>
                                     <asp:GridView Visible="false" ID="grdDevices"  ShowHeader="true" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" DataKeyNames ="UdId"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red"  EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" OnRowCommand="grdDevices_RowCommand">
                                <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                  <RowStyle CssClass="gradeA odd" />
                                          <PagerStyle CssClass="paginate_active"   />
                                  <Columns>
                                  
                                      <asp:TemplateField HeaderText="Device Register Id">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUdid" runat="server" Text='<%# Eval("UdId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField  HeaderText="MAC Address" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblMac" runat="server" Text='<%# Eval("MAC") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                           <asp:TemplateField  HeaderText="Last Login On" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblLastLoginOn" runat="server" Text='<%# Convert.ToDateTime(Eval("Last Login On")).ToString("D")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>         
                                      
                                       <asp:TemplateField  HeaderText="Lost?" >
                                      <ItemTemplate>
                                          <asp:LinkButton ID="lnkLost" runat="server" CommandName="Lost" Text='<%# ((DataBinder.Eval(Container.DataItem,"CreatedOn")) == DBNull.Value) ? ("Blacklist") : ("Whitelist" )%>' CommandArgument='<%# Eval("UdId") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                      
                                           <asp:TemplateField  HeaderText="Blacklisted On" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblBlacklisted" runat="server" Text='<%#((DataBinder.Eval(Container.DataItem,"CreatedOn")) == DBNull.Value) ? ("") : ( Convert.ToDateTime(Eval("CreatedOn")).ToString("D") )  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>  
                                  
                                       <asp:TemplateField  HeaderText="Data Deleted?" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblBlacklistedOn" runat="server" Text='<%# ((DataBinder.Eval(Container.DataItem,"DataWipeOut")) == DBNull.Value) ? ("No") : "Yes"  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>  
                                      
                                       <asp:TemplateField  HeaderText="Data Deleted On" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblDeletedOn" runat="server" Text='<%# ((DataBinder.Eval(Container.DataItem,"UpdatedOn")) == DBNull.Value) ? ("") : ( Convert.ToDateTime(Eval("UpdatedOn")).ToString("D"))   %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                                                 
                                 </Columns>
                                </asp:GridView>
                                     </td>
                                     </tr>
                                                             
                                 </table>
                                 </div>
   
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlUser" />
                            
                         <asp:PostBackTrigger ControlID="grdDevices" />
        
    </Triggers>
</asp:UpdatePanel>
                                    <br />
                              
                               
					  <div class="grid_24">
                               <div class="box_top">
		
		<h2 class="icon users">User </h2>
		
	</div>			
				   <div class="box_content">
		
		<!-- Simple Sorting Table + Pagination: Start -->
		<div class="dataTables_wrapper">
                                                      
                         
                                         <asp:GridView ID="grdUser" runat="server" AutoGenerateColumns="False" AllowSorting="false"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="UserId"  onpageindexchanging="grdUser_PageIndexChanging" 
                                   onrowcommand="grdUser_RowCommand" >
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

                                  
                                    <asp:TemplateField HeaderText="Sr. No." HeaderStyle-Width="30px">
                                        <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Name" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblClientName" runat="server" Text='<%# Eval("Suffix") +" "+Eval("FirstName") +" "+ Eval("LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                     <asp:TemplateField Visible="false" HeaderText="Contact Number" SortExpression="Mobile">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName") %>'></asp:Label>
                                              <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("EmailID1") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    
                                     

                           
                                          <asp:TemplateField HeaderText="Device Lost?">
                <ItemStyle HorizontalAlign="Center" Width="10%" />
                <ItemTemplate>
              
                    <asp:LinkButton ID="lnkFlushData" runat="server" CommandArgument='<%#Eval("UserId") %>' CommandName="flush" Text='<%# Eval("IsMaker").ToString().ToLower().Equals("true") ? "Stop Flushing":"Flush data " %>'  ToolTip="Add or view access rights">  </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
                                   
                                 </Columns>
                                </asp:GridView>
            
            <asp:HiddenField ID="hdnPopUpPublish" Value="0" ClientIDMode="Static" runat="server" />
            	<asp:HiddenField ID="hdnPopUp" Value="0" ClientIDMode="Static" runat="server" />
                            </div>
                          </div>
                          </div>
                                 
                                  </div></dl>
                                 </fieldset>                            										
					
				</div>			
			</section>			
		</article>
    <div class="clearfix">
    </div>
</asp:Content>
