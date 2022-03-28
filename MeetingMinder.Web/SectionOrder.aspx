<%@ Page Title="" EnableEventValidation="false"  Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="SectionOrder.aspx.cs" Inherits="MeetingMinder.Web.SectionOrder" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<link rel="stylesheet" type="text/css" href="css/jquery-ui.css" />
<script type="text/javascript">
     function AddValue() {
         var vals = "";
         $(".group").each(function () {
             vals += this.id + ",";
             id = this.id;
//             $("#" + id + " > div ol").find("li").each(function () {
//                 vals += this.id + ",";
//             });
         });
         $("#hdnListItems").val(vals);
       //alert(vals);
         return true;
     }

     $(function () {
         $("#accordion")
.accordion({
    header: "> ol > li > h3",
    
});
 $("#sort").sortable({
    axis: "y",
    handle: "h3",
    stop: function (event, ui) {
        // IE doesn't register the blur when sorting
        // so trigger focusout handlers to remove .ui-state-focus
        ui.item.children("h3").triggerHandler("focusout");
        $("#divOrder").attr("style", "display:block");
    }
});

$(".inddrag").sortable({
 stop: function (event, ui) {
 $("#divOrder").attr("style", "display:block");
 }});

$(".ddrag").sortable({
 stop: function (event, ui) {
 $("#divOrder").attr("style", "display:block");
 }});
     });

     

 </script>

 <article class="content-box minimizer">
			<header>			
				<h2>&nbsp; <asp:Label ID="lblItemName" Visible="false"  runat="server"></asp:Label></h2>			
			</header>
			<section>              
				<div >
                  <div class="table-form">
						<fieldset>
							<legend><font color="#054a7f"><b><asp:Label ID="lblListName" runat="server"></asp:Label></b></font></legend>
                             <dl>
                                <div style="margin-bottom:15px"><userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                               
                                <div style="margin-bottom:15px">
                            
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                              <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="b" />
                                </div>
                                <div>

                                 </div>
                                  <div style="margin-bottom:15px">
                                 <asp:Label ID="lblList" runat="server"></asp:Label>
                                 </div>
                                   <br />
                                   <asp:HiddenField ClientIDMode="Static" ID="hdnListItems" runat="server" />
                                    <div id="divOrder" style="display:none">
                                        <div class="fullwidth noBorder">
            <asp:Button ID="btnSave"  runat="server"  CssClass="btnSave" style="width:110px;"  OnClientClick="AddValue();"   Text="Save Order" onclick="btnSave_Click"></asp:Button> 

                                            </div>
                          
                </div> 			      <div class="fullwidth noBorder">

                                 <asp:Button ID="btnBack"  runat="server"  CssClass="btnSave" style="width:110px;"    Text="Back" onclick="btnBack_Click"></asp:Button> 
                    </div>
                               <br />
						</fieldset>
                         							
					</div>									
				</div>					
				
			</section>			
		</article>
    <div class="clearfix">
    </div>
</asp:Content>

