<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ViewNotice.aspx.cs" Inherits="MeetingMinder.Web.ViewNotice" %>

<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">
    function CallPrint() {
        var prtContent = document.getElementById('<%= lblPrint.ClientID %>');
        var WinPrint = window.open('', '', 'letf=0,top=0,width=800,height=400,toolbar=0,scrollbars=0,status=0,dir=ltr');
        WinPrint.document.write(prtContent.innerHTML);
        WinPrint.document.close();
        WinPrint.focus();
        WinPrint.print();
        WinPrint.close();
        prtContent.innerHTML = strOldOne;
    }
</script>
    <style type="text/css">
        @media print {
    #page_1 {
        background-color: white;
        height: 100%;
        width: 100%;
        position: fixed;
        top: 0;
        left: 0;
        margin: 0;
        padding: 15px;
        font-size: 20px;
        line-height: 18px;
    }
}
        .full_width, fieldset b {
    top: 0px;
    position: relative;
}

    </style>
<article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--View Notice--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>View Notice</b></font></legend>
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
                                     <asp:DropDownList ID="ddlEntity" Visible="false" runat="server" AutoPostBack="true" Width="50%" 
                                             onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList>
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
                                     <asp:DropDownList ID="ddlForum" onselectedindexchanged="ddlForum_SelectedIndexChanged"  runat="server" AutoPostBack="true" Width="50%" 
                                             ></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Forum" Text="Please Select Forum" ForeColor="Red"></asp:RequiredFieldValidator>
                                     
                                 
                                     </td>
                                     </tr>

                                      <tr>
                                     <td style="width:95px;">
                                     Year 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                         <asp:DropDownList ID="ddlYear" onselectedindexchanged="ddlForum_SelectedIndexChanged" AutoPostBack="true"  runat="server" Width="50%"></asp:DropDownList>
                                     <asp:RequiredFieldValidator runat="server" ID="rfvYear" ControlToValidate="ddlYear" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Year" Text="Please Select Year" ForeColor="Red"></asp:RequiredFieldValidator>
                                 
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

                                    

                                      <tr id="trNotice" runat="server" visible="false">
                                     <td style="width:95px;">
                                    Notice 
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                           <asp:Label ID="lblView" runat="server"  Visible="false" ></asp:Label>
                                                   </td>
                                     </tr>
                                 </table>
                                 </div>
                                 
  <div class="fullwidth noBorder">
<asp:Button id="btnPrint" CssClass="btnSave" style="width:110px;" visible="false" runat="server" onclientclick="javascript:CallPrint();" text="Print Notice" xmlns:asp="#unknown" />
</div>
                                     <div style="display:none;" >
<asp:Label ID="lblPrint" runat="server"></asp:Label>
</div>
                                             </ContentTemplate>
                                    <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlMeeting" />
                               <asp:AsyncPostBackTrigger ControlID="ddlForum" />

                            

                       
                         
        
    </Triggers>
</asp:UpdatePanel>
                               </div>
                                 </dl>
                               <br />
						</fieldset>
                             										
									
				</div>					
				
			</section>			
		</article>
 

</asp:Content>

