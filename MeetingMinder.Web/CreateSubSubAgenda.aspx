<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="CreateSubSubAgenda.aspx.cs" Inherits="MeetingMinder.Web.CreateSubSubAgenda" %>
<%@ Register TagName="Error" Src="~/UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="~/UserControls/Info.ascx" TagPrefix="userControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <%--    <script src="js/jquery/jquery.min.js"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script type="text/javascript">

    
     function GetValue(obj) {
         if (obj == "Other") {
           //  document.getElementById("txtOtherSerialNo").style = "width:230px;display:block";
             $("#txtOtherSerialNo").attr("style", "width:230px;display:block");
             var validatorObjectFrom = document.getElementById('<%=rfvOtherSerialNo.ClientID%>');
             validatorObjectFrom.enabled = true;
             validatorObjectFrom.isvalid = true;
             ValidatorUpdateDisplay(validatorObjectFrom);
         }
         else {
          //   document.getElementById("txtOtherSerialNo").style = "width:230px;display:none; ";
             $("#txtOtherSerialNo").attr("style", "width:230px;display:none");
             var validatorObjectFrom = document.getElementById('<%=rfvOtherSerialNo.ClientID%>');
             validatorObjectFrom.enabled = false;
             validatorObjectFrom.isvalid = true;
             ValidatorUpdateDisplay(validatorObjectFrom);
         }
     }
     

     function ConfSub() {
        
         var file = $('#<%=fuAgenda.ClientID %>').val();
         var link = $('#<%=lnkViewPdf.ClientID %>').val();

         if (link === undefined) {
         }
         else {
             var file = $('#<%=fuAgenda.ClientID %>').val();

             if (file != '') {
                 if (confirm("Do you want to retain initial pdf ?")) {
                     document.getElementById('<%=hdnDelete.ClientID %>').value = "don't";
                 } else {
                     document.getElementById('<%=hdnDelete.ClientID %>').value = "delete";
                 }
             }

         }
        
     }


     $(document).ready(function () {
         $(document).bind('drop dragover', function (e) {
             e.preventDefault();
         });

         var ID = 2;
         function addRow() {
             var html =
                 '<tr>' +
                 '<td> <input type="file" name="fileUpload' + ID + '" />  ' +
                 //'<td><input type="button" class="BtnPlus" value="+" /></td>' +
                 '<input type="button"  onclick="deleteRow(this);"  class="BtnMinus" value="-" /></td>' +
                 '</tr>'
             $(html).appendTo($("#tdUp"))
             ID++;
         };
         //$("#tdUp").on("click", ".BtnPlus", addRow);
         //function deleteRow() {
         //    var par = $(this).parent().parent();
         //    par.remove();
         //};
         //$("#tdUp").on("click", ".BtnMinus", deleteRow);
         
         $(".BtnPlus").click(addRow);
      
     });

     function deleteRow(ctrl) {
         $(ctrl.parentNode.parentNode).remove();
         //ctrl.parentNode.parentNode.remove();
         //var par = $(this).parent().parent();
         //par.remove();
     }

     function DelAgendaPdf(objName) {
         if (confirm("Are you sure you want to delete this file?")) {

             document.getElementById('<%=hdnDelete.ClientID %>').value = "delete";
         }
         else {
             return;
         }

         $("#hdnDeletePdfName").val(objName);
         $("#btnDelete").click();
     }

     function PdfUploadValidation() {

         var retrn = true;
         var re = new RegExp('^([a-zA-Z].*|[1-9].*)\.(((p|P)(d|D)(f|F)))$');
         $(":file").each(function () {

             if (this.value != "") {
                 var m = re.exec(this.value);
                 if (m == null) {
                     retrn = false;
                 }
             }
         });
         if (!retrn) {
             alert("Please Select Pdf file for upload");
         }
         return retrn;
     }


     function ViewAgendaPdf(objName) {
         $("#hdnDeletePdfName").val(objName);

         // $('[id$=lnkViewPdf]').click();

         //      $("#btnView").click();   
         document.getElementById('<%= lnkViewPdf.ClientID %>').click();
         // $("#lnkViewPdf").click();
         //var obj = $("#lnkViewPdf");
         //alert(obj.href);
         //$("#lnkSend").attr("href", obj.href);
         //$("#lnkSend").click();
         }
  function GetAgenda() {
            var values =$('#<%=ddlMaster.ClientID %>').val();
            var txt = $('#<%=ddlMaster.ClientID %> option:selected').text();

            if (values != "0") {
                $('#<%=txtSubAgenda.ClientID %>').val(txt);
            }
  }
     $(document).ready(function () {

         if ($('#<%=hdnAgendaId.ClientID %>').val().length > 10) {
                  var id = 'divData';
                  $("html, body").animate({
                      scrollTop: $('#' + id
                          ).offset().top
                  }, 1500);
              }
          });
      
        </script>
<article class="content-box minimizer">
			<header>			
				<h2>&nbsp;<%--Create Sub Subagenda--%></h2>			
			</header>
			<section>              
				<div >
                 
						<fieldset>
							<legend><font color="#054a7f"><b>Create Sub Subagenda </b></font></legend>
                             <dl>
                             <div >
                      

                                <div style="margin-bottom:15px"><userControl:Info ID="Info" runat="server" Visible="false" />
                                      <userControl:Error ID="Error" runat="server" Visible="false" />   
                                </div>
                                <div style="margin-bottom:15px">
                                    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                                           CssClass="notification error" 
                                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                                           ValidationGroup="a" />
                                </div>
                                <div>
                                    <div  style="float:right;margin-top:-40px;">
                                          <div class="fullwidth noBorder">
            <asp:Button ID="btnBack" CssClass="btnSave"  runat="server" Text="Back" onclick="btnBack_Click"></asp:Button>
                                      			
            </div> 
                                    </div>
                                         <div style="display:none;"><asp:HyperLink style="float:right" ID="hylnkAgenda" runat="server" NavigateUrl="#" >Change Order</asp:HyperLink></div>
                                      <dt>
                                    <label>
                                 Sub Agenda  :
                                    </label>
                                </dt>
                                <dd>
                             <asp:Label   Font-Names="Rupee"   ID="lblAgenda" runat="server"></asp:Label>
                                </dd>

                                    <dt>
                                   <label>
                                    Meeting :
                                       </label>
                                    </dt>
                                    
                      
                                     <dd>
                                        
                                 <asp:Label ID="lblMeeting" Font-Names="Rupee"   runat="server"></asp:Label>
                                   
                                   </dd>

                                    <dt>
                                   <label>
                                    Forum :
                                    </label>
                                    </dt>
                                     
                                    <dd>
                                     
                                 <asp:Label ID="lblForum" Font-Names="Rupee"   runat="server"></asp:Label>

                                     </dd>
                                </div>
                              <div>
                                  <div class="box_top">
		
		
<h2 class="icon users">Sub Sub Agenda </h2>
		
	
</div>
                                          <div class="box_content">
		<div class="dataTables_wrapper">
                                   <asp:GridView ID="grdSubAgenda" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    class="datatable" PageSize="10" DataKeyNames="AgendaId" 
                                       onpageindexchanging="grdSubAgenda_PageIndexChanging" onrowcommand="grdSubAgenda_RowCommand"                              
                                     
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

                                            <asp:TemplateField HeaderText="Serial No." >
                                          <ItemTemplate>
                                             <asp:Label  style="text-align:justify;font-family:Rupee" Width="100px"  ID="lblSerial" runat="server" Text='<%# MeetingMinder.Web.CreateSubAgenda.GetTitles(Eval("SerialTitle").ToString(), Eval("SerialNumber").ToString(), Eval("SerialText").ToString())  %>'></asp:Label>
                                          </ItemTemplate>
                                       </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Sub sub Agenda" >
                                        <ItemTemplate>
                                          <asp:Label ID="lblTe"  Font-Names="Rupee"   runat="server" Text='<%#  Eval("AgendaName").ToString().Replace(Environment.NewLine, "<BR/>") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Note" Visible="false" >
                                        <ItemTemplate>
                                          <asp:Label ID="lblNote" runat="server" Text='<%# Eval("AgendaNote") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Uploaded Sub Sub Agenda">
                                        <ItemTemplate>
                                        <%# GetUploadedAgenda(Eval("UploadedAgendaNote"),Eval("AgendaNote"),Eval("DeletedAgenda"))%>
                                    <%-- <label style='<%# (((string)DataBinder.Eval(Container.DataItem,"UploadedAgendaNote")) == "") ? ("display:none") : ("display:block" )%>' >
                                     <asp:LinkButton ID="lnkUploadAgenda" CommandArgument='<%# Eval("UploadedAgendaNote") %>' CommandName="download"  Text="View"  runat="server" /> </label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                         <ItemTemplate>
                                            <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Eval("AgendaId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                    CausesValidation="false" CommandName="EditAgenda" Text="Edit" runat="server" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Bind("AgendaId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                 CausesValidation="false" CommandName="Del" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                         </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                   </asp:TemplateField>                        
                                 </Columns>
                                </asp:GridView>
                       </div>
                                       </div>
            
                    </div>

                              <br /><br />
                                <div id="divData" style="width:900px;">
                                 <table width="75%">

                                     
                                     <tr >
                                     <td >
                                    Serial Title 
                                   
                                     </td>
                                     <td>
                                     :
                                     </td>
                                     <td  align="left">
                                     <asp:DropDownList   ID="ddlSerialTitle" Width="250px" runat="server" MaxLength="250"></asp:DropDownList>
                                     
                                 
                                    
                                  </td>
                                     </tr>

                                        <tr >
                                     <td >
                                   Serial Number  <span>*</span> 
                                        
                                     </td>
                                     <td>
                                     :
                                     </td>
                                     <td  align="left">
                                   <asp:DropDownList onchange="GetValue(this.value)" ID="ddlSerialNumber" ClientIDMode="Static" runat="server"  Width="250px" >
                                    <%--   <asp:ListItem Value="0" Text="Select" ></asp:ListItem>
                                         <asp:ListItem Value="" Text="" ></asp:ListItem>
                                        <asp:ListItem Value="ABS" Text="ABS" ></asp:ListItem>
                                         <asp:ListItem Value="C&R" Text="C&R" ></asp:ListItem>
                                         <asp:ListItem Value="CAG" Text="CAG" ></asp:ListItem>
                                         <asp:ListItem Value="CBG" Text="CBG" ></asp:ListItem>
                                         <asp:ListItem Value="CCO" Text="CCO" ></asp:ListItem>
                                         <asp:ListItem Value="CDO" Text="CDO" ></asp:ListItem>
                                         <asp:ListItem Value="CFO" Text="CFO" ></asp:ListItem>
                                         <asp:ListItem Value="CIO" Text="CIO" ></asp:ListItem>
                                         <asp:ListItem Value="COO" Text="COO" ></asp:ListItem>
                                       <asp:ListItem Value="CRO" Text="CRO" ></asp:ListItem>
                                       <asp:ListItem Value="CS&NB" Text="CS&NB" ></asp:ListItem>
                                       <asp:ListItem Value="I&MA" Text="I&MA" ></asp:ListItem>
                                       <asp:ListItem Value="IBG" Text="IBG" ></asp:ListItem>
                                       <asp:ListItem Value="MCG" Text="MCG" ></asp:ListItem>
                                       <asp:ListItem Value="NBG" Text="NBG" ></asp:ListItem>
                                       <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                                       <asp:ListItem Value="SAMG" Text="SAMG" ></asp:ListItem>

                                          <asp:ListItem Value="A&S" Text="A&S" ></asp:ListItem>
                                           <asp:ListItem Value="DB&NB" Text="DB&NB"></asp:ListItem>
                                       <asp:ListItem Value="GM" Text="GM" ></asp:ListItem>--%>
                                    </asp:DropDownList>
                                         <br /><br />
                                         <asp:TextBox ID="txtOtherSerialNo" runat="server" ClientIDMode="Static" style="display:none;"></asp:TextBox>
                                             <asp:RequiredFieldValidator runat="server" Enabled="false"  ID="rfvOtherSerialNo" ControlToValidate="txtOtherSerialNo" Display="Dynamic"
                                ValidationGroup="a"   ErrorMessage="Other Serial Number" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                  
                                         <asp:RequiredFieldValidator runat="server" ID="rfvNew" ControlToValidate="ddlSerialNumber" Display="Dynamic"
                                              ValidationGroup="a" InitialValue="0" ErrorMessage="Serial Number" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </tr>
                                      <tr >
                                     <td >
                                    Serial Text 
                                   
                                     </td>
                                     <td>
                                     :
                                     </td>
                                     <td  align="left">
                                     <asp:TextBox   ID="txtSerialText" Width="230px" runat="server" MaxLength="250"></asp:TextBox>
                                  </td>
                                     </tr>

                                     <tr>
                                     <td  style="width:95px;">Master Agenda</td>
                                       <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="left">
                                     <asp:DropDownList ID="ddlMaster"  Font-Names="Rupee"  Width="250px" onclick="GetAgenda()" runat="server"></asp:DropDownList>
                                     </td>
                                     </tr>
                                   <tr>
                                     <td style="width:95px;">
                                    Sub Agenda
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="center">
                                    <asp:TextBox ID="txtSubAgenda"   Font-Names="Rupee"   TextMode="MultiLine" runat="server"  Width="230px" Font-Size="12px"></asp:TextBox>
                                      <asp:RequiredFieldValidator ID="rfvSub" runat="server" 
                                        class="invalid-side-note" ControlToValidate="txtSubAgenda" Display="Dynamic" 
                                        ErrorMessage="Agenda Name" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>

                                     </td>
                                     </tr>

                                     <tr>
                                     <td >
                                    Presenter 
                                    <%-- <label><span>&nbsp;*</span></label>--%>
                                     </td>
                                     <td>
                                     :
                                     </td>
                                     <td  align="left">
                                     <asp:TextBox  ID="txtPresenter" Width="230px" runat="server" MaxLength="250"></asp:TextBox>
                                     
                                 
                                    
                                  </td>
                                     </tr>

                                    <tr style="display:none">
                                     <td style="width:95px;">
                                   Agenda Note
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  style="width:250px;" align="center">
                                    <asp:TextBox  Font-Names="Rupee"    ID="txtAgendaNote" runat="server"  Width="50%"></asp:TextBox>                                   
                                    </td>
                                     </tr>

                                            <tr>
                                     <td  style="width:95px;">
                                   Upload <span style="color:Red">(Pdf file only)</span>
                                     </td>
                                     <td width="20px">
                                     :
                                     </td>
                                     <td  id="tdUp" style="width:250px;" align="left">
                                             <asp:Label ID="lblPdfs" runat="server" ></asp:Label>
                                  <asp:FileUpload style="width:265px" name="fileUpload"  ID="fuAgenda" runat="server"  />  
                                      <input type="button" class="BtnPlus" value="+" />
                               <%--   <asp:FileUpload ID="fuAgenda" runat="server"  />--%>
                           <br />
                                                                       <span style="color:Red">(Upload small size pdf files for better performance)</span>
                                   </td>
                                     </tr>


                                     <tr align="right">
                                        <td colspan="5" align="right"> 
                                        <asp:HiddenField ID="hdnAgendaId" runat="server" />
                                        <asp:HiddenField ID="hdnDelete" runat="server" />	
                                        <div class="fullwidth noBorder">
                                        <asp:Button CssClass="btnSave" ID="btnSubmit"  CausesValidation="true" ValidationGroup="a" runat="server" Text="Submit" onclick="btnSubmit_Click"></asp:Button> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button CssClass="btnCancel" ID="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click"></asp:Button>
                                        </div>
                                 </td>
                                 </tr>
                                 </table>
                                 </div>
                                 <br />
                               <asp:HiddenField ID="hdnDeletePdfName" runat="server" ClientIDMode="Static" />      
                                  <div style="display:none;">
                                           
                                        <%--<asp:Button ID="btnView" runat="server" ClientIDMode="Static" Text="View" CssClass="btnSave" onclick="btnView_Click"></asp:Button>--%>

                                               <asp:LinkButton ID="lnkViewPdf" runat="server"  Text="View" onclick="lnkView_Click"></asp:LinkButton>
                                        <asp:Button ID="btnDelete" runat="server" ClientIDMode="Static" Text="Cancel" CssClass="btnSave" onclick="btnDelete_Click"></asp:Button>
                                            </div>
                               </div>
                               <br />
						</fieldset>
				</div>					
			</section>			
		</article>
    <div class="clearfix">
    </div>
</asp:Content>
