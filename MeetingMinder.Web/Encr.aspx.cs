using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EncryptWebCon;
using System.Text;
using MM.Domain;
using MM.Data;

namespace MeetingMinder.Web
{
    public partial class Encr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // < div style = "vertical-align: top;" > A - CAG - 1 & emsp; &emsp; &emsp; &emsp; &emsp; &emsp; Sanction, Approval & amp; Confirmation / CAG, Chennai & ndash; Muthoot Fincorp Limited & emsp; &emsp; &emsp; &emsp; &emsp; &emsp; (MFL) </ div >
           // if (!IsPostBack) btnExport_Click(sender, e);
            //return;
            //build the content for the dynamic Word document
            //in HTML alongwith some Office specific style properties. 
            var strBody = new StringBuilder();

            strBody.Append("<html " +
             "xmlns:o='urn:schemas-microsoft-com:office:office' " +
             "xmlns:w='urn:schemas-microsoft-com:office:word'" +
              "xmlns='http://www.w3.org/TR/REC-html40'>" +
              "<head><title>Time</title>");


            strBody.Append("<style>" +
             "<!-- /* Style Definitions */" +
             "@page Section1" +
             "   {size:8.5in 11.0in; " +
             "   margin:1.0in 1.25in 1.0in 1.25in ; " +
             "   mso-header-margin:.5in; " +
             "   mso-footer-margin:.5in; mso-paper-source:0;}" +
             " div.Section1" +
             "   {page:Section1;}" +
             "-->" +
             "</style></head>");


       
            strBody.Append("<body lang=EN-US style='tab-interval:.5in'>" +
             "<div class=Section1>");


            strBody.Append(@" <div style='overflow: hidden; width: 800px;'>
    <span style='width:100px;'>La&ensp;bel 1</span>
    <span style='display: block;overflow: hidden;'>Value 1</span>
</div>");
            strBody.Append(@"      te <br /> swewe <br /> dssadsa /n/r tdssaddsa \r yyyyy  
  <div style='width:100%'>
       

<div  style='float: left;width: 100px;height: 20px;margin-right: 8px;' >
  testst
</div>
<div style='margin-left: 108px;'>
  For Sanction, Approval &amp; Confirmation/ CAG, Chennai &ndash; Muthoot Fincorp Limited (MFL)
</div>


<table cellspacing='0' cellpadding='0' border='0' style='border-width:0;border-collapse: collapse;border: 1px white solid;width:100%'>
            <tr style='border: 1px white solid;'>
                <td style='border: 1px white solid;width:50%;vertical-align:top;text-align:left;line-height: 1;'> 
<div>Central Board Secretariat</div><div>Corporate Centre </div><div style='margin-top: 12px; margin-left: -8px;'><b>MUMBAI, (date)</b> </div></td>
                <td style='border: 1px white solid;width:50%;vertical-align:top;text-align:right;' > <b>     General Manager & <br />  Secretary, Central Board</b></td>
            </tr>
        </table>");
            strBody.Append("</div></body></html>");

            //Force this content to be downloaded 
            //as a Word document with the name of your choice
            Response.AppendHeader("Content-Type", "application/msword");
            Response.AppendHeader("Content-disposition", "attachment; filename=myword.doc");

            Response.Write(strBody.ToString());
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {

            try
            {
                Session["UserId"] = "4bb72b6d-0d4b-41d3-87d4-b5a6544d02de";
                lblList.Text = "";
                lblList.Visible = true;
                string strMeetingId = "3e38d51f-1b2f-462f-9480-529651ed513a";//ddlMeeting.SelectedValue;
                IList<AgendaDomain> objAgentList = new List<AgendaDomain>();

                if (strMeetingId != "0")
                {
                    Guid meetingId;
                    if (Guid.TryParse(strMeetingId, out meetingId))
                    {
                        ViewState["MeetingId"] = meetingId;

                        StringBuilder strMenu = new StringBuilder("");
                        StringBuilder strAgendaNames = new StringBuilder();
                        StringBuilder strAllAgenda = new StringBuilder();
                        if (Session["obj"] != null)
                        {
                            objAgentList = (List<AgendaDomain>)(Session["obj"]);
                        }
                        else
                        {
                            objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(meetingId);

                            Session["obj"] = objAgentList;
                        }

                        if (objAgentList.Count > 0)
                        {
                            //get only parent agenda
                            var objParentAgenda = from agend in objAgentList
                                                  where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                                  select agend;



                            //get only sub agenda
                            var subAgendaList = from agend in objAgentList
                                                where (agend.ParentAgendaId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                                select agend;

                            //get agenda name/note and agenda id
                            var agendaName = (from agend in objParentAgenda

                                              select (new
                                              {
                                                  AgendaName = agend.AgendaName,
                                                  AgendaId = agend.AgendaId,
                                                  UplaodedAgenda = agend.UploadedAgendaNote,
                                                  AgendaNote = agend.AgendaNote,
                                                  DeletedAgenda = agend.DeletedAgenda,
                                                  Classifications = agend.Classification,
                                                  SerialNumber = agend.SerialNumber,
                                                  Presenter = agend.Presenter,
                                                  SerialNumberType = agend.SerialNumberType,
                                                  SerialTitle = agend.SerialTitle,
                                                  SerialText = agend.SerialText
                                              })).Distinct().ToList();

                            if (agendaName.Count() > 0)
                            {
                                string ClassificationOld = "";
                                string ClassificationNew = "";


                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
                                    strMenu.Append("<b>" + agendaName[i].SerialNumber + ".      " + agendaName[i].AgendaName + "</b>  <br /> ");
                                }

                                strMenu.Append(@" </div>  <br />  <div style='width:100%;'>");

                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
                                    Guid agendaId = agendaName[i].AgendaId;
                                    ClassificationNew = agendaName[i].Classifications;
                                    //get sub agenda for parent agenda 
                                    var subAgendaName = (from subAgenda in subAgendaList
                                                         where (subAgenda.ParentAgendaId == agendaId)
                                                         select (new
                                                         {
                                                             AgendaName = subAgenda.AgendaName,
                                                             AgendaId = subAgenda.AgendaId,
                                                             UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                             AgendaNote = subAgenda.AgendaNote,
                                                             DeletedAgenda = subAgenda.DeletedAgenda,
                                                             Presenter = subAgenda.Presenter,
                                                             SerialNumber = subAgenda.SerialNumber,
                                                             SerialNumberType = subAgenda.SerialNumberType,
                                                             SerialTitle = subAgenda.SerialTitle,
                                                             SerialText = subAgenda.SerialText
                                                         })).ToList();
                                    if (ClassificationOld != ClassificationNew)
                                    {
                                        ClassificationOld = ClassificationNew;
                                        if (agendaName[i].Classifications != "")
                                        {
                                            // strMenu.Append(" < h2><b>" + agendaName[i].Classifications + "</b></h2>");
                                        }

                                    }

                                    string Presenter = "";

                                    if (agendaName[i].Presenter.Trim().Length > 0)
                                    {
                                        Presenter = "(Presenter : " + agendaName[i].Presenter + ")";
                                    }

                                    string SerialNumber = "";
                                    if (agendaName[i].SerialNumber.Length > 0)
                                    {
                                        SerialNumber = agendaName[i].SerialNumber + " : ";
                                    }



                                    if (agendaName[i].UplaodedAgenda.Length > 0)
                                    {
                                        string[] strDeletedAgenda = agendaName[i].DeletedAgenda.Split(',');
                                        string[] agendaNames = agendaName[i].UplaodedAgenda.Split(',');
                                        string[] agendaPdfs = agendaName[i].AgendaNote.Split(',');

                                        strMenu.Append("<b>" + agendaName[i].SerialNumber + ".  <u>" + agendaName[i].AgendaName + "</u></b> <br /> ");
                                        //strMenu.Append("<li id=" + agendaId + " class='group' ><h3>  " + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");



                                        bool SendPdfFile = false;
                                        foreach (string str in agendaNames)
                                        {
                                            if (!strDeletedAgenda.Contains(str))
                                            {
                                                SendPdfFile = true;
                                            }
                                        }
                                        if (agendaNames.Count() > 0 && SendPdfFile)
                                        {
                                            string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                            strMenu.Append(" <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a></br> ");
                                        }
                                    }
                                    else
                                    {
                                        strMenu.Append("<b>" + agendaName[i].SerialNumber + ". <u>" + agendaName[i].AgendaName + "</u></b></br>");
                                        //strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");
                                    }

                                    strMenu.Append(" <br /> ");

                                    //attach sub agenda to parent agenda list element
                                    if (subAgendaName.Count() > 0)
                                    {
                                        Dictionary<string, int> objSerial = new Dictionary<string, int>();
                                        // strMenu.Append("<div><ol  style='list-style:none;margin-left:-30px;' class=ddrag>");
                                        // strMenu.Append("<table>");
                                        for (int j = 0; j <= subAgendaName.Count() - 1; j++)
                                        {
                                            Presenter = "";
                                            string serialNo = "";

                                            if (subAgendaName[j].Presenter.Trim().Length > 0)
                                            {
                                                Presenter = "(Presenter : " + subAgendaName[j].Presenter + ")";
                                            }

                                            if (subAgendaName[j].SerialNumber.Trim().Length > 0)
                                            {
                                                if (objSerial.ContainsKey(subAgendaName[j].SerialNumber))
                                                {
                                                    if (subAgendaName[j].SerialNumberType != "Other")
                                                    {
                                                        objSerial[subAgendaName[j].SerialNumber] += 1;
                                                        serialNo = subAgendaName[j].SerialNumber + " - " + objSerial[subAgendaName[j].SerialNumber] + " : ";
                                                    }
                                                    else
                                                    {
                                                        serialNo = subAgendaName[j].SerialNumber + " : ";
                                                    }
                                                }
                                                else
                                                {
                                                    if (subAgendaName[j].SerialNumberType != "Other")
                                                    {
                                                        objSerial.Add(subAgendaName[j].SerialNumber, 1);
                                                        serialNo = subAgendaName[j].SerialNumber + " - " + objSerial[subAgendaName[j].SerialNumber] + " : ";
                                                    }
                                                    else
                                                    {
                                                        serialNo = subAgendaName[j].SerialNumber + " : ";
                                                    }
                                                }
                                            }

                                            if (subAgendaName[j].UplaodedAgenda.Length > 0)
                                            {
                                                string[] strDeletedAgenda = subAgendaName[j].DeletedAgenda.Split(',');
                                                string[] agendaNames = subAgendaName[j].UplaodedAgenda.Split(',');
                                                string[] agendaPdfs = subAgendaName[j].AgendaNote.Split(',');

                                                bool SendPdfFile = false;
                                                foreach (string str in agendaNames)
                                                {
                                                    if (!strDeletedAgenda.Contains(str))
                                                    {
                                                        SendPdfFile = true;
                                                    }
                                                }

                                                strMenu.Append(@" <div style='width:12%;float: left;width: 100px; vertical-align: top;'>" + CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + "</div> <div style='margin-left: 108px;width:87%;'> " + subAgendaName[j].AgendaName);
                                                //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> <span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span> " + CreateSubAgenda.GetTitles(subAgendaName[j].SerialText, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + subAgendaName[j].AgendaName + Presenter + "");

                                                if (agendaNames.Count() > 0 && SendPdfFile)
                                                {
                                                    string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                                    strMenu.Append(" <br />  <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> </br>");
                                                }
                                            }
                                            else
                                            {
                                                strMenu.Append(@" <div style='width:12%; vertical-align: top;float: left;width: 100px;height: 20px;'>" + CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + "</div> <div style='margin-left: 108px;width:87%;'> " + subAgendaName[j].AgendaName);
                                                //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><span style='display:none'> " + (i + 1) + "." + (j + 1) + "</span> " + CreateSubAgenda.GetTitles(subAgendaName[j].SerialText, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + subAgendaName[j].AgendaName + Presenter);// + "</li>");
                                            }

                                            Guid subAgendaId = subAgendaName[j].AgendaId;
                                            //Get sub sub agenda
                                            var subSubAgendaName = (from subAgenda in subAgendaList
                                                                    where (subAgenda.ParentAgendaId == subAgendaId)
                                                                    select (new
                                                                    {
                                                                        AgendaName = subAgenda.AgendaName,
                                                                        AgendaId = subAgenda.AgendaId,
                                                                        UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                                        AgendaNote = subAgenda.AgendaNote,
                                                                        DeletedAgenda = subAgenda.DeletedAgenda,
                                                                        Presenter = subAgenda.Presenter,
                                                                        SerialNumber = subAgenda.SerialNumber,
                                                                        SerialNumberType = subAgenda.SerialNumberType,
                                                                        SerialTitle = subAgenda.SerialTitle,
                                                                        SerialText = subAgenda.SerialText
                                                                    })).ToList();
                                            if (subSubAgendaName.Count() > 0)
                                            {
                                                Dictionary<string, int> objSerialSub = new Dictionary<string, int>();
                                                //attach sub sub agenda to parent agenda list element
                                                //   strMenu.Append("<ol class=inddrag  style='list-style: lower-alpha outside none;margin-left:20px;padding-left:10px;'>");
                                                strMenu.Append(" <div class='subAgenda' cellspacing='0' >");
                                                for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
                                                {
                                                    Presenter = "";

                                                    if (subSubAgendaName[y].Presenter.Trim().Length > 0)
                                                    {
                                                        Presenter = "(Presenter : " + subSubAgendaName[y].Presenter + ")";
                                                    }

                                                    string serialNoSub = "";



                                                    //if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                                                    //{
                                                    //    string[] strDeletedAgenda = subSubAgendaName[y].DeletedAgenda.Split(',');
                                                    //    string[] agendaNames = subSubAgendaName[y].UplaodedAgenda.Split(',');
                                                    //    string[] agendaPdfs = subSubAgendaName[y].AgendaNote.Split(',');

                                                    //    // strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "> " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialText, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + subSubAgendaName[y].AgendaName + Presenter + "");
                                                    //    strMenu.Append("<tr> <td style='width:8%;padding:0;margin:0;vertical-align:top;'>  " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + " </td> <td style='width:3%;'></td>  <td style='width:60%;vertical-align: top;'>" + subSubAgendaName[y].AgendaName);


                                                    //    bool SendPdfFile = false;
                                                    //    foreach (string str in agendaNames)
                                                    //    {
                                                    //        if (!strDeletedAgenda.Contains(str))
                                                    //        {
                                                    //            SendPdfFile = true;
                                                    //        }
                                                    //    }
                                                    //    if (agendaNames.Count() > 0 && SendPdfFile)
                                                    //    {
                                                    //        string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                                    //        strMenu.Append(" <br />  <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> ");
                                                    //    }
                                                    //}
                                                    //else
                                                    //{
                                                        //strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialText, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + subSubAgendaName[y].AgendaName + Presenter);
                                                        strMenu.Append("<div style='float: left;width: 100px;padding:0;margin:0;vertical-align:top;'>  " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + " </div>  <div style='margin-left: 108px;vertical-align: top;'>" + subSubAgendaName[y].AgendaName);
                                                    //}
                                                    strMenu.Append(" </div>");
                                                }
                                                // strMenu.Append("</ol>");
                                                strMenu.Append("</div>");

                                            }
                                            else
                                            {
                                                strMenu.Append(" </div>  ");
                                            }
                                            //strMenu.Append("</li>");
                                         //   strMenu.Append(@"</td></tr>");
                                        }
                                        //strMenu.Append("</ol></div></li>");
                                        strMenu.Append("<table> <br />  <br /> ");

                                    }
                                    else
                                    {
                                        // strMenu.Append("</li>");
                                    }

                                }
                                //     strMenu.Append("</ol></div>");
                                strMenu.Append(@" </div> <div> <br /> Any other item with the permission of the Chair. <br />  <br /> </div>
        <table style='width:100%'>
            <tr>
                <td style='width:50%;vertical-align:top;text-align:left;line-height: 1;'> 
<div>Central Board Secretariat</div><div>Corporate Centre </div><div style='margin-top: 12px; margin-left: -8px;'><b>MUMBAI, (date)</b> </div></td>
                <td style='width:50%;vertical-align:top;text-align:right;' > <b>     General Manager & <br />  Secretary, Central Board</b></td>
            </tr>
        </table></div>");

                            }
                            lblList.Text = strMenu.ToString();

                            //var strBody = new StringBuilder();

                            //strBody.Append("<html " +
                            // "xmlns:o='urn:schemas-microsoft-com:office:office' " +
                            // "xmlns:w='urn:schemas-microsoft-com:office:word'" +
                            //  "xmlns='http://www.w3.org/TR/REC-html40'>" +
                            //  "<head><title>Time</title>");


                            //strBody.Append("<style>" +
                            // "<!-- /* Style Definitions */" +
                            // "@page Section1" +
                            // "   {size:8.5in 11.0in; " +
                            // "   margin:1.0in 1.25in 1.0in 1.25in ; " +
                            // "   mso-header-margin:.5in; " +
                            // "   mso-footer-margin:.5in; mso-paper-source:0;}" +
                            // " div.Section1" +
                            // "   {page:Section1;}" +
                            // "-->" +
                            // "</style></head>");

                            //strBody.Append("<body lang=EN-US style='tab-interval:.5in'>" +
                            // "<div class=Section1>");
                            //strBody.Append("<style type='text/css'> .divPrint b {font-size: 14pt !important;}  .divPrint,.divPrint table,.subAgenda td{color:#000!important;font-family:times new roman}#accordion li{margin-left:20px;list-style:none}table tr td,table tr th{border:none!important}.tab tr:last-child>td{border:1px solid #000!important}.divPrint,.divPrint table{font-size:14pt}.subAgenda td{font-size:11pt!important}b{font-weight:700!important}</style>");
                            //strBody.Append(strMenu.ToString());
                            //strBody.Append("</div></body></html>");

                            ////Force this content to be downloaded 
                            ////as a Word document with the name of your choice
                            //Response.AppendHeader("Content-Type", "application/msword");
                            //Response.AppendHeader("Content-disposition", "attachment; filename=myword.doc");

                            //Response.Write(strBody.ToString());
                            //Response.End();
                        }


                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnExportd_Click(object sender, EventArgs e)
        {
            try
            {
                Session["UserId"] = "4bb72b6d-0d4b-41d3-87d4-b5a6544d02de";
                lblList.Text = "";
                lblList.Visible = true;
                string strMeetingId = "3e38d51f-1b2f-462f-9480-529651ed513a";//ddlMeeting.SelectedValue;
                if (strMeetingId != "0")
                {
                    Guid meetingId;
                    if (Guid.TryParse(strMeetingId, out meetingId))
                    {
                        ViewState["MeetingId"] = meetingId;

                        StringBuilder strMenu = new StringBuilder("");
                        StringBuilder strAgendaNames = new StringBuilder();
                        StringBuilder strAllAgenda = new StringBuilder();
                        IList<AgendaDomain> objAgentList = new List<AgendaDomain>();
                        if (Session["obj"] != null)
                        {
                            objAgentList = (List<AgendaDomain>)(Session["obj"]);
                        }
                        else
                        {
                            objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(meetingId);

                            Session["obj"] = objAgentList;
                        }
                        if (objAgentList.Count > 0)
                        {
                            //get only parent agenda
                            var objParentAgenda = from agend in objAgentList
                                                  where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                                  select agend;



                            //get only sub agenda
                            var subAgendaList = from agend in objAgentList
                                                where (agend.ParentAgendaId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                                select agend;

                            //get agenda name/note and agenda id
                            var agendaName = (from agend in objParentAgenda

                                              select (new
                                              {
                                                  AgendaName = agend.AgendaName,
                                                  AgendaId = agend.AgendaId,
                                                  UplaodedAgenda = agend.UploadedAgendaNote,
                                                  AgendaNote = agend.AgendaNote,
                                                  DeletedAgenda = agend.DeletedAgenda,
                                                  Classifications = agend.Classification,
                                                  SerialNumber = agend.SerialNumber,
                                                  Presenter = agend.Presenter,
                                                  SerialNumberType = agend.SerialNumberType,
                                                  SerialTitle = agend.SerialTitle,
                                                  SerialText = agend.SerialText
                                              })).Distinct().ToList();

                            if (agendaName.Count() > 0)
                            {
                                string ClassificationOld = "";
                                string ClassificationNew = "";

                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
                                    strMenu.Append("<b>" + agendaName[i].SerialNumber + ".      " + agendaName[i].AgendaName + "</b>  <br /> ");
                                }

                                strMenu.Append(@" </div>  <br />  <div style='width:100%;'>");

                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
                                    Guid agendaId = agendaName[i].AgendaId;
                                    ClassificationNew = agendaName[i].Classifications;
                                    //get sub agenda for parent agenda 
                                    var subAgendaName = (from subAgenda in subAgendaList
                                                         where (subAgenda.ParentAgendaId == agendaId)
                                                         select (new
                                                         {
                                                             AgendaName = subAgenda.AgendaName,
                                                             AgendaId = subAgenda.AgendaId,
                                                             UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                             AgendaNote = subAgenda.AgendaNote,
                                                             DeletedAgenda = subAgenda.DeletedAgenda,
                                                             Presenter = subAgenda.Presenter,
                                                             SerialNumber = subAgenda.SerialNumber,
                                                             SerialNumberType = subAgenda.SerialNumberType,
                                                             SerialTitle = subAgenda.SerialTitle,
                                                             SerialText = subAgenda.SerialText
                                                         })).ToList();
                                    if (ClassificationOld != ClassificationNew)
                                    {
                                        ClassificationOld = ClassificationNew;
                                        if (agendaName[i].Classifications != "")
                                        {
                                            // strMenu.Append(" < h2><b>" + agendaName[i].Classifications + "</b></h2>");
                                        }

                                    }

                                    string Presenter = "";

                                    if (agendaName[i].Presenter.Trim().Length > 0)
                                    {
                                        Presenter = "(Presenter : " + agendaName[i].Presenter + ")";
                                    }

                                    string SerialNumber = "";
                                    if (agendaName[i].SerialNumber.Length > 0)
                                    {
                                        SerialNumber = agendaName[i].SerialNumber + " : ";
                                    }


                                    {
                                        strMenu.Append("<b>" + agendaName[i].SerialNumber + ". <u>" + agendaName[i].AgendaName + "</u></b></br>");
                                        //strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");
                                    }

                                    strMenu.Append(" <br /> ");

                                    //attach sub agenda to parent agenda list element
                                    if (subAgendaName.Count() > 0)
                                    {
                                        Dictionary<string, int> objSerial = new Dictionary<string, int>();
                                        // strMenu.Append("<div><ol  style='list-style:none;margin-left:-30px;' class=ddrag>");
                                        // strMenu.Append("<table>");
                                        for (int j = 0; j <= subAgendaName.Count() - 1; j++)
                                        {
                                            Presenter = "";
                                            string serialNo = "";

                                            if (subAgendaName[j].Presenter.Trim().Length > 0)
                                            {
                                                Presenter = "(Presenter : " + subAgendaName[j].Presenter + ")";
                                            }

                                            
                                                strMenu.Append(@" <div style='vertical-align: top;'>" + CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + "  <pre style='width:10px;display:inline;' >  </pre>   " + subAgendaName[j].AgendaName);
                                                //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><span style='display:none'> " + (i + 1) + "." + (j + 1) + "</span> " + CreateSubAgenda.GetTitles(subAgendaName[j].SerialText, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + subAgendaName[j].AgendaName + Presenter);// + "</li>");
                                             

                                            Guid subAgendaId = subAgendaName[j].AgendaId;
                                            //Get sub sub agenda
                                            var subSubAgendaName = (from subAgenda in subAgendaList
                                                                    where (subAgenda.ParentAgendaId == subAgendaId)
                                                                    select (new
                                                                    {
                                                                        AgendaName = subAgenda.AgendaName,
                                                                        AgendaId = subAgenda.AgendaId,
                                                                        UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                                        AgendaNote = subAgenda.AgendaNote,
                                                                        DeletedAgenda = subAgenda.DeletedAgenda,
                                                                        Presenter = subAgenda.Presenter,
                                                                        SerialNumber = subAgenda.SerialNumber,
                                                                        SerialNumberType = subAgenda.SerialNumberType,
                                                                        SerialTitle = subAgenda.SerialTitle,
                                                                        SerialText = subAgenda.SerialText
                                                                    })).ToList();
                                            if (subSubAgendaName.Count() > 0)
                                            {
                                                Dictionary<string, int> objSerialSub = new Dictionary<string, int>();
                                                //attach sub sub agenda to parent agenda list element
                                                //   strMenu.Append("<ol class=inddrag  style='list-style: lower-alpha outside none;margin-left:20px;padding-left:10px;'>");
                                                strMenu.Append(" <div class='subAgenda' cellspacing='0' >");
                                                for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
                                                {
                                                    Presenter = "";

                                                    if (subSubAgendaName[y].Presenter.Trim().Length > 0)
                                                    {
                                                        Presenter = "(Presenter : " + subSubAgendaName[y].Presenter + ")";
                                                    }

                                                    string serialNoSub = "";



                                                    {
                                                        //strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialText, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + subSubAgendaName[y].AgendaName + Presenter);
                                                        strMenu.Append("  <div style='float: left;width: 100px;padding:0;margin:0;vertical-align:top;'>  " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + " </div>   <div style='margin-left: 108px;vertical-align: top;'>" + subSubAgendaName[y].AgendaName);
                                                    }
                                                    strMenu.Append(" </div>");
                                                }
                                                // strMenu.Append("</ol>");
                                              //  strMenu.Append("</table>");

                                            }
                                            else
                                            {
                                                strMenu.Append(" </div>  ");
                                            }
                                            //strMenu.Append("</li>");
                                         //   strMenu.Append(@"</td></tr>");
                                        }
                                        //strMenu.Append("</ol></div></li>");
                                      //  strMenu.Append("<table> <br />  <br /> ");

                                    }
                                    else
                                    {
                                        // strMenu.Append("</li>");
                                    }

                                }
                                //     strMenu.Append("</ol></div>");
                                strMenu.Append(@" </div> <div> <br /> Any other item with the permission of the Chair. <br />  <br /> </div>
        <table style='width:100%'>
            <tr>
                <td style='width:50%;vertical-align:top;text-align:left;line-height: 1;'> 
<div>Central Board Secretariat</div><div>Corporate Centre </div><div style='margin-top: 12px; margin-left: -8px;'><b>MUMBAI, (date)</b> </div></td>
                <td style='width:50%;vertical-align:top;text-align:right;' > <b>     General Manager & <br />  Secretary, Central Board</b></td>
            </tr>
        </table></div>");

                            }
                            lblList.Text = strMenu.ToString();

                            var strBody = new StringBuilder();

                            strBody.Append("<html " +
                             "xmlns:o='urn:schemas-microsoft-com:office:office' " +
                             "xmlns:w='urn:schemas-microsoft-com:office:word'" +
                              "xmlns='http://www.w3.org/TR/REC-html40'>" +
                              "<head><title>Time</title>");


                            strBody.Append("<style>" +
                             "<!-- /* Style Definitions */" +
                             "@page Section1" +
                             "   {size:8.5in 11.0in; " +
                             "   margin:1.0in 1.25in 1.0in 1.25in ; " +
                             "   mso-header-margin:.5in; " +
                             "   mso-footer-margin:.5in; mso-paper-source:0;}" +
                             " div.Section1" +
                             "   {page:Section1;}" +
                             "-->" +
                             "</style></head>");

                            strBody.Append("<body lang=EN-US style='tab-interval:.5in'>" +
                             "<div class=Section1>");
                            strBody.Append("<style type='text/css'> .divPrint b {font-size: 14pt !important;}  .divPrint,.divPrint table,.subAgenda td{color:#000!important;font-family:times new roman}#accordion li{margin-left:20px;list-style:none}table tr td,table tr th{border:none!important}.tab tr:last-child>td{border:1px solid #000!important}.divPrint,.divPrint table{font-size:14pt}.subAgenda td{font-size:11pt!important}b{font-weight:700!important}</style>");
                            strBody.Append(strMenu.ToString());
                            strBody.Append("</div></body></html>");

                            //Force this content to be downloaded 
                            //as a Word document with the name of your choice
                            Response.AppendHeader("Content-Type", "application/msword");
                            Response.AppendHeader("Content-disposition", "attachment; filename=myword.doc");

                            Response.Write(strBody.ToString());
                            Response.End();
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }

        }

        //public void test()
        //{
        //    Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
        //    Microsoft.Office.Interop.Word.Document wordDoc = new Microsoft.Office.Interop.Word.Document();
        //    Object oMissing = System.Reflection.Missing.Value;
        //    wordDoc = word.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        //    word.Visible = false;
        //    Object filepath = "c:\\page.html";
        //    Object confirmconversion = System.Reflection.Missing.Value;
        //    Object readOnly = false;
        //    Object saveto = "c:\\doc.pdf";
        //    Object oallowsubstitution = System.Reflection.Missing.Value;

        //    wordDoc = word.Documents.Open(ref filepath, ref confirmconversion, ref readOnly, ref oMissing,
        //                                  ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //                                  ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //                                  ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        //    object fileFormat = WdSaveFormat.wdFormatPDF;
        //    wordDoc.SaveAs(ref saveto, ref fileFormat, ref oMissing, ref oMissing, ref oMissing,
        //                   ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //                   ref oMissing, ref oMissing, ref oMissing, ref oallowsubstitution, ref oMissing,
        //                   ref oMissing);
        //}
    }
}