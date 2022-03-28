

<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="CreateAgenda.aspx.cs"  Inherits="MeetingMinder.Web.CreateAgenda" %>
<%@ Register TagName="Error" Src="UserControls/Error.ascx" TagPrefix="userControl" %>
<%@ Register TagName="Info" Src="UserControls/Info.ascx" TagPrefix="userControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%-- <script src="js/jquery/jquery.min.js"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
      border:none;
      }
      div#accordion {
    padding-top: 18px !important;
}

       li {
           list-style:none;
       }
   </style>
   <script type="text/javascript">
      function pageLoad(sender, args) {
      
          //  PdfUploadValidation();
          ShowPopUp();
        //  showChecker();
          ShowPopUpPublish();
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
      
      function ConfSub() {
          var values = $('#<%=ddlUser.ClientID %>').val();
          var file = $('#<%=fuAgenda.ClientID %>').val();
          var link = $('#<%=lnkViewPdf.ClientID %>').val();
          if (Page_ClientValidate("a")) {
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
      }
      
      function GetAgenda() {
          var values = $('#<%=ddlMaster.ClientID %>').val();
          var txt = $('#<%=ddlMaster.ClientID %> option:selected').text();
      
          if (values != "0") {
              $('#<%=txtTitle.ClientID %>').val(txt);
          }
      }
      
      
      $(document).ready(function () {
      
      //            SearchAgendaTitle();
          $(document).bind('drop dragover', function (e) {
              e.preventDefault();
          });
      
      });
      
      function SearchAgendaTitle() {
          $("#txtTitle").autocomplete({
              source: function (request, response) {
                  $.ajax({
                      type: "POST",
                      contentType: "application/json; charset=utf-8",
                      url: "CreateAgenda.aspx/GetAgendaTitles",
                      data: "{'strSearch':'" + document.getElementById('txtTitle').value + "'}",
                      dataType: "json",
                      success: function (data) {
                          response(data.d);
                      },
                      error: function (result) {
                          alert("Error");
                      }
                  });
              }
          });
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
      
      function DelAgendaPdf(objName) {
          debugger;
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
          // var re = new RegExp('^([a-zA-Z].*|[1-9].*)\.(((p|P)(d|D)(f|F)))$');
          $(":file").each(function () {
              //alert(this.value);
              if (this.value != "") {
                  var m = this.value;
                  var ext = m.split('.').pop();
                  if (m == "pdf" || m == "PDF") {
                  }
                  else {
                      retrn = false;
                  }
              }
              else {
                  retrn = false;
              }
          });
          if (!retrn) {
              alert("Please Select Pdf file for upload");
          }
          return retrn;
      }
      
      function HidePopUp() {
          //$('.close-btn').click(function () {
      
          $("#hdnPopUp").val("0");
          $("#hdnAgenda").val("");
          //$('.overlay-bg').hide(); // hide the overlay
          $("#divAgendaApproval").hide();
          // });
      }
      
      function ShowPopUp() {
      
          if ($("#hdnPopUp").val() == "1") {
              // $('.overlay-bg').show();
              $("#divAgendaApproval").show();
          }
          //lnkId = obj.href;
          //$("#lnkSend").attr("href", lnkId)
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
      
      function SaveAccess(obj) {
          var agendaIds = [];
          if ($("#hdnAgenda").val() != "") {
              agendaIds = $("#hdnAgenda").val().split(",");
          }
          if (obj.checked) {
              $('#' + obj.id + ' input[type="checkbox"]').each(function (i, el) {
                  agendaIds.push(el.id);
              });
              //   agendaIds.push(obj.id);
          }
          else {
              $('#' + obj.id + ' input[type="checkbox"]').each(function (i, el) {
                  agendaIds.splice(agendaIds.indexOf(el.id), 1);
              });
              // agendaIds.splice(agendaIds.indexOf(obj.id), 1);
          }
      
          $("#hdnAgenda").val(agendaIds);
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
         <h2>&nbsp;<%--Create Agenda--%></h2>
      </header>
      <section>
     

         <div >
            <fieldset>
               <legend><font color="#054a7f"><b>Agenda Details</b></font></legend>
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
                        <div style="margin-bottom:15px">
                           <userControl:Info ID="Info" runat="server" Visible="false" />
                           <userControl:Error ID="Error" runat="server" Visible="false" />
                        </div>
                        <%--  <div style="margin-bottom:15px">--%>
                        <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                           CssClass="notification error" 
                           HeaderText="<p>You must enter a valid value in the following fields:</p>" 
                           ValidationGroup="a" />
                        <%-- </div>--%>
                        <div>
                           <div>
                              <asp:DropDownList ID="ddlAgendaMeeting" ClientIDMode="Static"  AutoPostBack="true" runat="server" Width="200px"
                                 onselectedindexchanged="ddlAgendaMeeting_SelectedIndexChanged"></asp:DropDownList>

                               <dt>
                                   <label>
                                    Forum : 
                                    </label>
                                   
                                    </dt>
                                     
                                    <dd>
                                     
                                 <asp:Label ID="lblForum1" Font-Names="Rupee"   runat="server"></asp:Label>

                                     </dd>

                              <asp:LinkButton  Visible="false" style="float:right" ID="lnkSendApproval" runat="server"
                                 Text="| &nbsp;Send To Checker" OnClick="lnkSendApproval_Click"></asp:LinkButton>
                              <asp:LinkButton Visible="false"   style="float:right" ID="lnkPublish" runat="server"
                                 Text="| &nbsp;Publish&nbsp;" OnClick="lnkPublish_Click"></asp:LinkButton>
                              &nbsp;      
                              <asp:LinkButton style="float:right"  Visible="false" ID="hylnkAgenda"  OnClick="hylnkAgenda_Click" runat="server">Change Order  &nbsp;  </asp:LinkButton>
                           </div>
                           <br />
                           <div class="box_top">
                              <h2 >Agenda </h2>
                             <%-- <div class="text_box_top">Fields marked with asterisk (*) are required</div>--%>
                           </div>
                           <div class="box_content">
                              <div class="dataTables_wrapper">
                                 <asp:GridView ID="grdAgenda" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                    CellPadding="0" CellSpacing="1" EmptyDataText="No Records Found" AllowPaging="True"
                                    GridLines="None" EmptyDataRowStyle-ForeColor="red" Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EnableSortingAndPagingCallbacks = "true"   class="datatable" PageSize="10" DataKeyNames="AgendaId" OnRowDeleting="grdAgenda_RowDeleting"
                                    OnPageIndexChanging="grdAgenda_PageIndexChanging" 
                                    onsorting="grdAgenda_Sorting" onrowcommand="grdAgendaRowCommand">
                                    <HeaderStyle HorizontalAlign="Center" CssClass="sorting"  Font-Bold="true" />
                                    <RowStyle CssClass="gradeA odd" />
                                    <PagerStyle CssClass="paginate_active"   />
                                    <Columns>
                                       <%--   <asp:TemplateField>
                                          <ItemStyle  HorizontalAlign="Center" />
                                          <HeaderStyle  HorizontalAlign="Center" />
                                          <HeaderTemplate>
                                              <asp:CheckBox ID="chkHeader" onclick="javascript: fn_select_all(this);" runat="server" />
                                           </HeaderTemplate>
                                          <ItemTemplate>
                                              <asp:CheckBox ID="chkSubAdmin" runat="server" />
                                          </ItemTemplate>
                                          </asp:TemplateField>--%>
                                       <asp:TemplateField HeaderText="Sr. No.">
                                          <ItemTemplate>
                                             <%# Container.DataItemIndex + 1 %>
                                          </ItemTemplate>
                                          <ItemStyle  HorizontalAlign="Center" />
                                       </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Serial No." >
                                          <ItemTemplate>
                                             <asp:Label  style="text-align:justify;font-family:Rupee" Width="100px"  ID="lblSerial" runat="server" Text='<%#  Eval("SerialNumber") %>'></asp:Label>
                                          </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Agenda Name" SortExpression="AgendaName">
                                          <ItemTemplate>
                                             <asp:Label  style="text-align:justify;font-family:Rupee" Width="300px"  ID="lblAgendaName" runat="server" Text='<%#   Eval("HasChild") + ""+Eval("AgendaName").ToString().Replace(Environment.NewLine, "<BR/>") %>'></asp:Label>
                                          </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Uploaded Agenda">
                                          <ItemTemplate>
                                             <%# GetUploadedAgenda(Eval("UploadedAgendaNote"),Eval("AgendaNote"),Eval("DeletedAgenda"))%>
                                             <%-- <label style='<%# (((string)DataBinder.Eval(Container.DataItem,"UploadedAgendaNote")) == "") ? ("display:none") : ("display:block" )%>' >
                                                <asp:LinkButton ID="lnkUploadAgenda" CommandArgument='<%# Eval("UploadedAgendaNote") %>' CommandName="download"  Text="View"  runat="server" /> </label>--%>
                                          </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Sub Agenda">
                                          <ItemTemplate>
                                             <asp:LinkButton ID="lnkAddView" CommandArgument='<%# Eval("AgendaId") %>' CommandName="add"  Text="Add / View"  runat="server" />
                                             <%--<a class="AddView" href="CreateSubAgenda.aspx?id=<%# Eval("AgendaId") %>" >Add / View</a>--%>
                                             <%--  <asp:Label ID="lblSubAgenda" runat="server" Text='<%# Eval("SubAgenda") %>'></asp:Label>--%>
                                          </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Action" ControlStyle-CssClass="table-actions">
                                          <ItemTemplate>
                                             <asp:ImageButton ID="lbtnEdit" CommandArgument='<%# Eval("AgendaId")+","+Eval("MeetingId")+","+Eval("ForumId")+","+Eval("EntityId") %>' ImageUrl="img/icons/actions/page_white_edit.png" 
                                                CausesValidation="false" CommandName="EditAgenda" Text="Edit" runat="server" ToolTip="Edit Item" />
                                             <asp:ImageButton ID="lbtnDelete" CommandArgument='<%# Eval("AgendaId")+","+Eval("EntityId")+","+Eval("EntityId") %>' ImageUrl="img/icons/actions/page_white_delete.png" 
                                                CausesValidation="false" CommandName="Delete" Text="Delete" runat="server" ToolTip="Delete Item" OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                          </ItemTemplate>
                                          <ItemStyle HorizontalAlign="Center" />
                                       </asp:TemplateField>
                                    </Columns>
                                 </asp:GridView>
                              </div>
                           </div>
                        </div>
                        <div id="divData" style="width:100%;">
                           <table width="100%">
                              <tr>
                                 <td style="width:115px;">
                                    Meeting <span>*</span>
                                    <%--             <label><span>&nbsp;*</span></label>--%>
                                 </td>
                                 <td style="width:20px;">
                                    :
                                 </td>
                                 <td  align="left">
                                    <asp:DropDownList ID="ddlMeeting" ClientIDMode="Static"  AutoPostBack="true"  onselectedindexchanged="ddlMeeting_SelectedIndexChanged"   runat="server" Width="25%"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvMeet" ControlToValidate="ddlMeeting" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Meeting" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <%-- <asp:TextBox ID="txtDate" ClientIDMode="Static" runat="server" Width="70%"></asp:TextBox>
                                       <asp:RequiredFieldValidator runat="server" ID="rfvNew" ControlToValidate="txtDate" Display="Dynamic" ValidationGroup="a" ErrorMessage="Answer" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                       --%>                                
                                 </td>
                              </tr>
                              <tr style="display:none;">
                                 <td >
                                    Entity
                                 </td>
                                 <td>
                                    :
                                 </td>
                                 <td  align="left">
                                    <asp:Label ID="lblEntity" Visible="false" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlEntity" Visible="false" runat="server" AutoPostBack="true" Width="25%" 
                                       onselectedindexchanged="ddlEntity_SelectedIndexChanged"></asp:DropDownList>
                                    <%--<asp:RequiredFieldValidator runat="server" ID="rfvEntity" ControlToValidate="ddlEntity" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Entity" Text="Please Select Entity" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                 </td>
                              </tr>
                              <tr>
                                 <td >
                                    Forum
                                 </td>
                                 <td>
                                    :
                                 </td>
                                 <td  align="left">
                                    <asp:Label ID="lblForum" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlForum" Visible="false" runat="server"  Width="25%" 
                                       onselectedindexchanged="ddlForum_SelectedIndexChanged" AutoPostBack="true"   ></asp:DropDownList>
                                    <%--<asp:RequiredFieldValidator runat="server" ID="rfvForum" ControlToValidate="ddlForum" InitialValue="0" Display="Dynamic" ValidationGroup="a" ErrorMessage="Select Forum" Text="Please Select Forum" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                 </td>
                              </tr>
                              <tr style="display:none;">
                                 <td>Classification</td>
                                 <td>
                                    :
                                 </td>
                                 <td  align="left">
                                    <asp:DropDownList  ID="ddlClassification" Width="25%" runat="server"></asp:DropDownList>
                                 </td>
                              </tr>
                              <tr>
                                 <td>Master Agenda</td>
                                 <td>
                                    :
                                 </td>
                                 <td  align="left">
                                    <asp:DropDownList ID="ddlMaster" Font-Names="Rupee" Width="25%"   onclick="GetAgenda()" runat="server"></asp:DropDownList>
                                 </td>
                              </tr>
                              <tr>
                                 <td >
                                    Agenda Title <span>*</span>
                                    <%-- <label><span>&nbsp;*</span></label>--%>
                                 </td>
                                 <td>
                                    :
                                 </td>
                                 <td  align="left">
                                    <asp:TextBox ID="txtTitle" TextMode="MultiLine"  ClientIDMode="Static" style=" Width:199px !important ; font-size:12px;font-family:Rupee" runat="server" MaxLength="250"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvMeeting" ControlToValidate="txtTitle"  Display="Dynamic" ValidationGroup="a" ErrorMessage="Agenda Title" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <%-- <asp:TextBox ID="txtDate" ClientIDMode="Static" runat="server" Width="70%"></asp:TextBox>
                                       <asp:RequiredFieldValidator runat="server" ID="rfvNew" ControlToValidate="txtDate" Display="Dynamic" ValidationGroup="a" ErrorMessage="Answer" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                       --%>                                
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
                                     <asp:TextBox ID="txtPresenter" Width="199px" runat="server" MaxLength="250"></asp:TextBox>
                                     
                                 
                                    
                                  </td>
                                     </tr>

                              <tr style="display:none">
                                 <td >
                                    Agenda Note
                                 </td>
                                 <td>
                                    :
                                 </td>
                                 <td  align="left">
                                    <asp:TextBox ID="txtAgendaNote" Width="215px" runat="server" MaxLength="250"></asp:TextBox>
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
                                         <asp:TextBox ID="txtSerialNumber" Width="215px" runat="server" MaxLength="250"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvNew" ControlToValidate="txtSerialNumber" Display="Dynamic" ValidationGroup="a" ErrorMessage="Serial Number" Text="Invalid" ForeColor="Red"></asp:RequiredFieldValidator>
                                  <%-- <asp:DropDownList ID="ddlSerialNumber" ClientIDMode="Static" runat="server"  Width="25%" >
                                       <asp:ListItem Value="0" Text="Select" ></asp:ListItem>
                                        <asp:ListItem Value="ABS :" Text="ABS" ></asp:ListItem>
                                         <asp:ListItem Value="C&R :" Text="C&R" ></asp:ListItem>
                                         <asp:ListItem Value="CAG :" Text="CAG" ></asp:ListItem>
                                         <asp:ListItem Value="CBG :" Text="CBG" ></asp:ListItem>
                                         <asp:ListItem Value="CCO :" Text="CCO" ></asp:ListItem>
                                         <asp:ListItem Value="CDO :" Text="CDO" ></asp:ListItem>
                                         <asp:ListItem Value="CFO :" Text="CFO" ></asp:ListItem>
                                         <asp:ListItem Value="CIO :" Text="CIO" ></asp:ListItem>
                                         <asp:ListItem Value="COO :" Text="COO" ></asp:ListItem>
                                       <asp:ListItem Value="CRO :" Text="CRO" ></asp:ListItem>
                                       <asp:ListItem Value="CS&NB :" Text="CS&NB" ></asp:ListItem>
                                       <asp:ListItem Value="I&MA :" Text="I&MA" ></asp:ListItem>
                                       <asp:ListItem Value="IBG :" Text="IBG" ></asp:ListItem>
                                       <asp:ListItem Value="MCG.MBG :" Text="MCG" ></asp:ListItem>
                                       <asp:ListItem Value="MBG :" Text="MBG" ></asp:ListItem>
                                       <asp:ListItem Value="SAMG :" Text="SAMG" ></asp:ListItem>
                                         
                                    </asp:DropDownList>--%>
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
                              <tr style="display:none;">
                                 <td>
                                    Send to Checker  :
                                 </td>
                                 <td >
                                    :
                                 </td>
                                 <td   align="left">
                                    <asp:CheckBox   onclick="showChecker();" ID="chkChecker" runat="server" ClientIDMode="Static" />
                                 </td>
                              </tr>
                              <tr id="divEntity" style="display:none">
                                 <td  style="display:none;">
                                    Checker<label><span>&nbsp;*</span></label>
                                 </td>
                                 <td  style="display:none;">
                                    :
                                 </td>
                                 <td  style="display:none;"  align="center">
                                    <%--       <asp:DropDownList ID="ddlUser" runat="server"  Width="50%"></asp:DropDownList>
                                       <asp:RequiredFieldValidator ID="rfvddUser" runat="server" Enabled="false"
                                         class="invalid-side-note" ControlToValidate="ddlUser" Display="Dynamic" InitialValue="0"
                                         ErrorMessage="Select Checker" SetFocusOnError="True" ValidationGroup="a">Invalid</asp:RequiredFieldValidator>
                                       --%>               
                                 </td>
                              </tr>
                              <tr align="right">
                                 <td colspan="5" align="right">
                                    <div class="fullwidth noBorder">
                                       <%-- OnClientClick="return PdfUploadValidation();"--%>
                                       <asp:Button  ID="btnSubmit"  CssClass="btnSave" CausesValidation="true" ValidationGroup="a" runat="server" Text="Submit" onclick="btnSubmit_Click"></asp:Button>
                                       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                       <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnCancel" onclick="btnCancel_Click"></asp:Button>
                                    </div>
                                    <div style="display:none;">
                                       <%--  <a id="lnkSend" href="#" >Decline </a>--%>
                                       <asp:Button ID="btnView" runat="server" ClientIDMode="Static" Text="View" CssClass="btnSave" onclick="btnView_Click"></asp:Button>
                                       <asp:LinkButton ID="lnkViewPdf"   ClientIDMode="Static" runat="server"  Text="View" onclick="lnkView_Click"></asp:LinkButton>
                                       <asp:Button ID="btnDelete" runat="server" ClientIDMode="Static" Text="Cancel" CssClass="btnSave" onclick="btnDelete_Click"></asp:Button>
                                    </div>
                                 </td>
                              </tr>
                           </table>
                        </div>
                     </ContentTemplate>
                     <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlEntity" />
                        <asp:AsyncPostBackTrigger ControlID="ddlForum" />
                        <asp:PostBackTrigger ControlID="lnkViewPdf" />
                        <asp:PostBackTrigger ControlID="grdAgenda" />
                        <asp:PostBackTrigger ControlID="btnSubmit" />
                        <asp:PostBackTrigger ControlID="btnCancel" />
                        <asp:PostBackTrigger ControlID="lnkSendApproval" />
                        <asp:PostBackTrigger ControlID="lnkPublish" />
                        <asp:PostBackTrigger ControlID="ddlMeeting" />
                     </Triggers>
                  </asp:UpdatePanel>
               </div>
               <br />
            </fieldset>
            <asp:HiddenField ID="hdnDeletePdfName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnAgendaId" runat="server" />
            <asp:HiddenField ID="hdnDelete" runat="server" />
            <asp:HiddenField ID="hdnAgenda" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnPopUpPublish" Value="0" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnPopUp" Value="0" ClientIDMode="Static" runat="server" />
         </div>
                  <div id="divAgendaPublish" class='overlay-bg'>
            <div class='overlay-content'>
               <fieldset  style="height:90%;width:800px !important;">
                  <div class="floatDiv">
                     <div>&nbsp; Published&nbsp; </div>
                     <div style="border: 1px solid; margin-top:6px; height: 10px; width: 10px; background-color: green;"> 
                     </div>
                     <div>&nbsp;Not Published&nbsp; </div>
                     <div style="border: 1px solid; height: 10px; width: 10px; margin-top:6px; background-color: red;"> </div>
                     <div>&nbsp; Pending &nbsp;</div>
                     <div style="border: 1px solid; height: 10px; width: 10px; margin-top:6px; background-color: orchid; "> </div>
                  </div>
                  <legend style="line-height: 30px !important;"><div class="box_top">
                     <h2 >Agenda </h2>
                     <div class="text_box_top" style="padding: 0px 0 0;">Fields marked with asterisk (*) are required</div>
                  </div></legend>
                  
                  <asp:Label Font-Names="Rupee"  CssClass="agenda" ID="lblAgndaPublish" runat="server"></asp:Label>
                  <img src="img/icons/icon_list_style_cross.png"  onclick="HidePopUpPublish();"  alt="Close" class="close-btn" />
                  <%-- <a id="lnkClose1" class="close-btn" href="javascript:void(0)"  onclick="HidePopUpPublish();"  >Close </a>--%>
                  <div class="fullwidth noBorder">
                     <asp:Button ID="btnPublishAgenda"  CssClass="btnSave" runat="server" Text="Publish" OnClick="btnPublishAgenda_Click"/>
                  </div>
               </fieldset>
            </div>
         </div>

              <div id="divAgendaApproval" class='overlay-bg'>
            <div class='overlay-content'>
               <fieldset  style="height:90%;width:800px !important;">
                         <legend><font color="#054a7f"><b>Agenda Approval</b></font></legend>
                  <div class="floatDiv">
                     <div>  &nbsp; Sent to checker&nbsp; </div>
                     <div style="border: 1px solid; height: 10px; width: 10px; background-color: red; margin-top:6px;"> 
                     </div>
                     <div>  &nbsp; Not Sent&nbsp;  </div>
                     <div style="border: 1px solid; height: 10px; width: 10px; margin-top:6px; background-color: #656b6d;"> 
                     </div>
                    <div>   &nbsp; Approved &nbsp;  </div> 
                     <div style="border: 1px solid; height: 10px; width: 10px;  margin-top:6px; background-color: #4800ff;"> 
                     </div>
                     <div> &nbsp; Published &nbsp;  </div> 
                     <div style="border: 1px solid; height: 10px; width: 10px;  margin-top:6px; background-color: green;"> 
                     </div>
                   </div>
                 
            
                  <label>
                     Checker : &nbsp; 
                     <asp:DropDownList ID="ddlUser" runat="server"  Width="50%"></asp:DropDownList>
                     <asp:RequiredFieldValidator ID="rfvddUser" runat="server" Enabled="true"
                        class="invalid-side-note" ControlToValidate="ddlUser" Display="Dynamic" InitialValue="0"
                        ErrorMessage="Checker" SetFocusOnError="True" ValidationGroup="checkers">Invalid</asp:RequiredFieldValidator>
                  </label>
                  <label>
                     Notify Checker :
                     <asp:CheckBox ID="chkNotifyChecker" runat="server"   />
                  </label>
                  <div class="box_top">
                     <h2>Agenda </h2>
                     <div class="text_box_top">Fields marked with asterisk (*) are required</div>
                  </div>
                  <asp:Label Font-Names="Rupee"  CssClass="agenda" ID="lblList" runat="server"></asp:Label>
                  <img src="img/icons/icon_list_style_cross.png"  onclick="HidePopUp();"  alt="Close" class="close-btn" />
                  <%--  <a id="lnkClose" class="close-btn" href="javascript:void(0)"  onclick="HidePopUp();"  >Close </a>--%>
                  <div class="fullwidth noBorder">
                     <asp:Button ID="btnSubmitAgenda" CssClass="btnSave" ValidationGroup="checkers" runat="server" Text="Submit" OnClick="btnSubmitAgenda_Click"/>
                  </div>
                  <table>
                     <tr id="trCheckerMessage" runat="server" visible="false">
                        <td colspan="3" >
                           <asp:Label ID="lblCheckerMessage" Font-Bold="true" Text="This agenda has been assigned to checker. Checker approval required to view in i-pad. Click here to"
                              ForeColor="Red" runat="server">
                              <asp:LinkButton ID="lnkRemoveChecker" OnClick="lnkRemoveChecker_Click" runat="server" Text=" Remove Checker and Approve"></asp:LinkButton>
                           </asp:Label>
                        </td>
                     </tr>
                  </table>
               </fieldset>
            </div>
         </div>
         <asp:Label ID="lblAlert" runat="server" ></asp:Label>
      </section>
   </article>
   <div class="clearfix">
   </div>
</asp:Content>

