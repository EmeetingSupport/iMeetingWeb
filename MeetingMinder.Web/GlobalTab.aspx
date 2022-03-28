<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="GlobalTab.aspx.cs" Inherits="MeetingMinder.Web.GlobalTab" %>

<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function pageLoad(sender, args) {

            //  PdfUploadValidation();
            // ShowPopUp();
            //  showChecker();
            // ShowPopUpPublish();
            var ID = 2;
            function addRow() {

                var html =
                    '<tr>' +
                    '<td> <input type="file" name="fileUpload' + ID + '" />  ' +
                    //'<td><input type="button" class="BtnPlus" value="+" /></td>' +
                    '<input type="button" onclick="deleteRow(this);" class="BtnMinus" value="-" /></td>' +
                    '</tr>'
                $(html).appendTo($("#tdUp"))
                ID++;
            };
            // $("#tdUp").on("click", ".BtnPlus", addRow);
            $(".BtnPlus").click(addRow);

            //function deleteRow() {
            //    alert('d');
            //    debugger;
            //    var par = $(this).parent().parent();
            //    par.remove();
            //};
            //   $("#tdUp").on("click", ".BtnMinus", deleteRow);
            $("#tdUp .BtnMinus").click(deleteRow);
        }

        function deleteRow(ctrl) {
            $(ctrl.parentNode.parentNode).remove();
            // ctrl.parentNode.parentNode.remove();
            //var par = $(this).parent().parent();
            //par.remove();
        }


        function DelAgendaPdf(objName,objPdf) {
        
            if (confirm("Are you sure you want to delete this file?")) {

                document.getElementById('<%=hdnDelete.ClientID %>').value = "delete";
            }
            else {
                return;
            }

            $("#hdnDeletePdfName").val(objName);
            $("#hdnDeleteOriginalPdfName").val(objPdf);

            $("#btnDelete").click();
        }


        function ViewGlobalTabPDF(objName) {
       
            document.getElementById('<%=hdnPdfName.ClientID %>').value = objName;
            $("#btnDelete").click();
            $("#btnView").click();
      }

    </script>
    <article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Master Agenda--%></h2>			
			</header>
			<section>              
				<div >
                <fieldset>
							<legend><font color="#054a7f"><b>Global Tabs</b></font></legend>
                            <dl>
                                <div style="margin-bottom:15px">
                                      <userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                 
                      
                               <div style="margin-bottom:15px"><asp:Label ID="lblTotalRecord" runat="server" Font-Bold="True"></asp:Label>
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
                 
                                                   <div id="ContentPlaceHolder1_divdetails"  style="margin-bottom:15px">
                                      
                                         </div>
                                     <div class="box_top">
		
		
<h2 class="icon users">Global Tabs</h2>
		
	
</div>
                       
                                <div style="margin-bottom:15px">
                               <div class="box_content">
		
		<!-- Simple Sorting Table + Pagination: Start -->
		<div class="dataTables_wrapper">
                                 <asp:GridView ID="grdGlobalTab" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="GlobalTabId" OnRowDeleting="grdGlobalTab_RowDeleting"
                                 
                                    OnPageIndexChanging="grdGlobalTab_PageIndexChanging"
                                    onsorting="grdGlobalTab_Sorting" onrowcommand="grdGlobalTab_RowCommand"
                                    onrowediting="grdGlobalTab_RowEditing">
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
                                     <asp:TemplateField HeaderText="Title" SortExpression="Title">
                                        <ItemTemplate>
                                        <asp:Label style="text-align:justify"  Font-Names="Rupee" Width="600px"  ID="lblTitle" runat="server" Text='<%# Eval("Title") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Uploaded GlobalTabs">
                                          <ItemTemplate>
                                             <%# GetUploadedGlobalTabs(Eval("UploadedPDF"),Eval("GlobalTabNote"))%>
                                             <%-- <label style='<%# (((string)DataBinder.Eval(Container.DataItem,"UploadedAgendaNote")) == "") ? ("display:none") : ("display:block" )%>' >
                                                <asp:LinkButton ID="lnkUploadAgenda" CommandArgument='<%# Eval("UploadedAgendaNote") %>' CommandName="download"  Text="View"  runat="server" /> </label>--%>
                                          </ItemTemplate>
                                       </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                         <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Eval("GlobalTabId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="Edit" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Eval("GlobalTabId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                 </Columns>
                                </asp:GridView>
                                 </div>
                                   </div></div>
                           
           
                     <div ID="divdetails" runat="server" style="margin-bottom:15px">                   
                          <table width="100%">
                             <tr>
                                 <td >
                                    Title<span>*</span>
                                    <%-- <label><span>&nbsp;*</span></label>--%>
                                 </td>
                                 <td>
                                    :
                                 </td>
                                 <td  align="left">
                                   <asp:TextBox  Font-Names="Rupee"  ID="txtTitle"  runat="server" MaxLength="149" Width="400px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtTitle" Display="Dynamic" 
                                        ErrorMessage="Title" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                 </td>
                              </tr>

                          <tr>
                                 <td >
                                    Upload
                                    <span style="color:Red">(Pdf file only)</span>
                                 </td>
                                 <td>
                                    :
                                 </td>
                                 <td  id="tdUp" align="left">
                                    <asp:Label ID="lblPdfs" runat="server" ></asp:Label>
                                    <asp:FileUpload style="width:199px" name="fileUpload"  ID="fuAgenda" runat="server"  />
                                    <input type="button" class="BtnPlus" value="+" />
                                    <br /> 
                                    <span style="color:Red">(Upload small size pdf files for better performance)</span>
                                 </td>
                              </tr>
                             
                        </table>
                                     <dl>
                                     <asp:HiddenField ID="hdnGlobalTabId" runat="server" />
                                        <asp:HiddenField ID="hdnDeletePdfName" runat="server" ClientIDMode="Static" />
                                                                              <asp:HiddenField ID="hdnDeleteOriginalPdfName" runat="server" ClientIDMode="Static" />

                                       <asp:HiddenField ID="hdnDelete" runat="server" />
                                         <asp:HiddenField ID="hdnPdfName" runat="server" />
                                     
                                    </dl>
                                    <div id="divbutton" runat="server" class="fullwidth noBorder">
                                    <asp:Button ID="btnInsert" CssClass="btnSave" runat="server" Text="Save" ValidationGroup="a" onclick="btnInsert_Click" />
                                       <asp:Button ID="btnCancel" CssClass="btnCancel" CausesValidation="false" runat="server" Text="Cancel" onclick="btnCancel_Click" />
                                    </div>

                         <div style="display:none;">
                                       <%--  <a id="lnkSend" href="#" >Decline </a>--%>
                                      <asp:Button ID="btnView" runat="server" ClientIDMode="Static" Text="View" CssClass="btnSave" onclick="btnView_Click"></asp:Button>
                                       <asp:Button ID="btnDelete" runat="server" ClientIDMode="Static" Text="Cancel" CssClass="btnSave" onclick="btnDelete_Click"></asp:Button>
                                    </div>
                                    </div>
                                        </ContentTemplate>
                                    <Triggers>
                             
                                    <asp:PostBackTrigger ControlID="btnInsert" />
                               <asp:PostBackTrigger ControlID="btnCancel" />
                               <asp:PostBackTrigger ControlID="grdGlobalTab" />
                               <asp:PostBackTrigger ControlID="btnView" />
                             
                                   
    </Triggers>
</asp:UpdatePanel>
                        </dl>
                            </fieldset>         </div>
                                </section>
                                </article>
</asp:Content>
