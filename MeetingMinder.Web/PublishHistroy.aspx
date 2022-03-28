<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="PublishHistroy.aspx.cs" Inherits="MeetingMinder.Web.PublishHistroy" %>

<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        h3 {
            margin-bottom: 0;
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
            width: 75%;
            height: 75%;
            overflow: auto;
            position: relative;
            top: 15%;
            left: 22%;
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

        .close-btn {
            position: absolute;
            right: 25px;
            top: 10px;
            cursor: pointer;
        }

        .floatDiv {
            float: right;
            display: flex;
            font-size: 15px;
        }

        .no-js .floatDiv {
            margin: 30px 0 0 10px\0/;
        }

            .no-js .floatDiv > div {
                float: right\0/;
            }

        @media screen and (-ms-high-contrast: active), (-ms-high-contrast: none) {
            .floatDiv {
                float: right;
                display: flex;
                margin-top: 40px;
            }
        }

        .overlay-content fieldset {
            border: none;
        }
    </style>
    <script type="text/javascript">
        function HidePopUp() {

            $("#hdnPopUp").val("0");

            $("#divAgenda").hide();

        }

        function ShowPopUp() {
            
            if ($("#hdnPopUp").val() == "1") {

                $("#divAgenda").show();
            }

            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--View Notice--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>Agenda Acknowledgement</b></font></legend>
                             <dl>
                             <div >
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
                           
                              <br /><br />
                                <div style="width:900px;">
                                 <table width="75%">

                                    <tr style="display:none">
                                     <td style="width:95px;">
                                    Department
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlEntity" Visible="false" runat="server"  Width="50%" 
                                            ></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Department" Text="Please Select Department" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>
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
                                     <td style="width:95px;">
                                    Meeting 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                         <asp:DropDownList ID="ddlMeeting" onselectedindexchanged="ddlMeeting_SelectedIndexChanged" AutoPostBack="true"  runat="server" Width="50%"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Meeting" Text="Please Select Meeting" ForeColor="Red"></asp:RequiredFieldValidator>
                                 
                                                   </td>
                                     </tr>
                               <tr>
                                   <td colspan="3">
                                      <div class="box_top">
		
		
<h2 class="icon users">Agenda Acknowledgement </h2>
		
	
</div>                   
                              <div style="margin-bottom:15px">
                                   <div class="box_content">
		<div class="dataTables_wrapper">
            <br />
         <b>   Publish On: <asp:Label ID="lblPublishOn" runat="server"></asp:Label></b>
            <br /><br />
            
                         <asp:GridView ID="grdAgendaVersions" runat="server" AutoGenerateColumns="False"  
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" 
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable"   OnRowCommand="grdAgendaVersions_RowCommand"
                                   >                                  
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
                                   
                                        <asp:TemplateField HeaderText="Publish On" >
                                        <ItemTemplate>
                                          <asp:Label ID="lblPublish" style="width:250px;text-align:justify"  runat="server" Text ='<%#  Convert.ToDateTime((Eval("PublishDate"))).ToString("F")   %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                                            
                                      

                                    <asp:TemplateField HeaderText="View"  >
                                        <ItemTemplate>
                                        
                                        <asp:LinkButton ID="lblAgenda" style="width:50px;text-align:justify"  runat="server" CommandArgument='<%# Eval("PublishVersion") %>' CommandName="view"  Text="View"  />
                             
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                            <asp:TemplateField HeaderText="View Ack"  >
                                        <ItemTemplate>
                                        
                                        <asp:LinkButton ID="lblAgendaAck" style="width:50px;text-align:justify"  runat="server" CommandArgument='<%# Eval("MeetingId")+","+Eval("PublishVersion") %>' CommandName="viewAck"  Text="View Acknowledgement"  />
                             
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 </Columns>
                             
                                </asp:GridView>
            <br /> <br />

                         <asp:GridView ID="grdHistory" runat="server" AutoGenerateColumns="False"  
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" 
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable"   
                                   >                                  
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
                                   
                                        <asp:TemplateField HeaderText="Name" >
                                        <ItemTemplate>
                                              <asp:Label ID="lblName" style="width:250px;text-align:justify"  runat="server" Text ='<%# Eval("Suffix") +" "+ Eval("Name") %>'  />
                                        <%--  <asp:Label ID="lblName" style="width:250px;text-align:justify"  runat="server" Text ='<%#  MM.Core.Encryptor.DecryptString(Eval("AckSuffix").ToString()) +" "+  MM.Core.Encryptor.DecryptString(Eval("AckFirstName").ToString())  +" "+   MM.Core.Encryptor.DecryptString(Eval("AckLastName").ToString())   %>'  />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                                            
                                         <asp:TemplateField HeaderText="Notice Read"  >
                                        <ItemTemplate>
                                        <asp:Label ID="lblNotice" style="width:50px;text-align:justify"  runat="server" Text ='<%#((DataBinder.Eval(Container.DataItem,"IsNoticeRead")).ToString() == "True" ? ("Yes") : ("No" ))%>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Agenda Read"  >
                                        <ItemTemplate>
                                        
                                        <asp:Label ID="lblAgenda" style="width:50px;text-align:justify"  runat="server" Text ='<%#((DataBinder.Eval(Container.DataItem,"AgendaRead")).ToString() == "True" ? ("Yes") : ("No" )) %>'  />
                             
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Notice Read On"  >
                                        <ItemTemplate>
                                        
                                        <asp:Label ID="lblAgendaRead" style="width:50px;text-align:justify"  runat="server" Text ='<%# Eval("NoticeReadOn") != null ? ((Convert.ToDateTime(Eval("NoticeReadOn")).ToString("f"))):"" %>'  />
                             
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                          
                                   <asp:TemplateField HeaderText="Agenda Read On"  >
                                        <ItemTemplate>
                                        
                                        <asp:Label ID="lblAgendaRead" style="width:50px;text-align:justify"  runat="server" Text ='<%# Eval("AgendaReadOn") != null   ? ((Convert.ToDateTime(Eval("AgendaReadOn")).ToString("f"))):"" %>'  />
                             
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Version Read"  >
                                        <ItemTemplate>
                                        <asp:Label ID="lblNotice" style="width:50px;text-align:justify"  runat="server" Text ='<%#  Convert.ToDateTime((Eval("PublishDate"))).ToString("F")   %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 </Columns>
                             
                                </asp:GridView>
            </div></div>
                                 </div>
                                   </td>
                               </tr>
                                 </table>
                                 </div>
                                     
  
 
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlMeeting" />
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />
<asp:PostBackTrigger ControlID="grdAgendaVersions" />
                            

                       
                         
        
    </Triggers>
</asp:UpdatePanel>
                               </div>

                                     <div id="divAgenda" class='overlay-bg'> <div class='overlay-content'>
                      <fieldset style="height:90%;width:900px;">
                        
						 
                               
                                                            <div class="box_top">	
<h2 class="icon users">Agenda </h2>	
</div>
                                        <asp:Label Font-Names="Rupee"  CssClass="agenda" ID="lblDetails" runat="server"></asp:Label>
                          <img src="img/icons/icon_list_style_cross.png"  onclick="HidePopUp();"  alt="Close" class="close-btn" />

    
                      

                      
                          </fieldset>
       </div>
    </div>

                                 </dl>
                               <br />
						</fieldset>
                             		<asp:HiddenField ID="hdnPopUp" Value="0" ClientIDMode="Static" runat="server" />										
						<asp:Label ID="lblAlert" runat="server" ></asp:Label>			
				</div>					
				
			</section>			
		</article>
</asp:Content>
