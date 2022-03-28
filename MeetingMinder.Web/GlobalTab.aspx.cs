using iTextSharp.text.pdf;
using MM.Data;
using MM.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Core;
using System.Data;
using iTextSharp.text;

namespace MeetingMinder.Web
{
    public partial class GlobalTab : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)

        {
            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;
            if (!Page.IsPostBack)
            {

                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

                    BindGlobalTab();
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }

            }
        }
        protected string GetUploadedGlobalTabs(object GlobalTabFiles, object GlobalTabPdf)
        {
            try
            {
                string GlobalTabFileName = Convert.ToString(GlobalTabFiles);
                string GlobalTabPdfName = Convert.ToString(GlobalTabPdf);

                StringBuilder sbGlobalTab = new StringBuilder("");
                string[] GlobalTabNames = GlobalTabFileName.Split(',');
                string[] GlobalTabPdfs = GlobalTabPdfName.Split(',');
                if (!string.IsNullOrEmpty(GlobalTabFileName))
                {
                    for (int i = 0; i <= GlobalTabNames.Count() - 1; i++)
                    {
                        sbGlobalTab.Append(" <a class='AddView' href='javascript:void(0)' onclick=ViewGlobalTabPDF('" + GlobalTabNames[i].Trim() + "')  >" + GlobalTabPdfs[i].Trim() + " </a> <br/>");
                    }
                }
                return sbGlobalTab.ToString();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
                return "Attached PDF files corrupted or unreadable";
            }
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {

                string UploadOn = "";
                string GlobalTabFile = "";
                string GlobalTabFileInsert = "";
                UserAcess objUser = new UserAcess();

                StringBuilder sbGlobalTabNames = new StringBuilder("");
                StringBuilder sbPdfNames = new StringBuilder("");
                string GlobalTabName;
                bool isUplaoded = false;
                string EncryptionKey = Session["EncryptionKey"].ToString();
                
                if (txtTitle.Text != "")
                {
                    if (!objUser.isValidChar(txtTitle.Text))
                    {
                        txtTitle.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid title.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtTitle.Text))
                    {
                        txtTitle.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter email subject";
                    Error.Visible = true;
                    return;
                }

                GlobalTabDomain objGlobalTabDomain = new GlobalTabDomain();
                objGlobalTabDomain.Title = txtTitle.Text.Trim();
                objGlobalTabDomain.EntityId = Guid.Parse(Session["EntityId"].ToString());


                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Files.Count - 1; i++)
                    {
                        string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["KeyInfo"]);
                        string contentType = Request.Files[i].ContentType;
                        if (Request.Files[i].ContentLength > 0)
                        {
                            if (!objUser.isValidChar_old(Request.Files[i].FileName))
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                                Error.Visible = true;
                                return;
                            }

                            if (contentType.ToLower().Contains("application/pdf"))
                            {
                                string ext = Path.GetExtension(Request.Files[i].FileName);
                                if (ext.ToLower().Equals(".pdf"))
                                {

                                    System.IO.BinaryReader r = new System.IO.BinaryReader(Request.Files[i].InputStream);
                                    string fileclass = "";
                                    byte buffer;
                                    try
                                    {
                                        buffer = r.ReadByte();
                                        fileclass = buffer.ToString();
                                        buffer = r.ReadByte();
                                        fileclass += buffer.ToString();
                                    }
                                    catch (Exception ex)
                                    {
                                        ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                                        Error.Visible = true;

                                        LogError objEr = new LogError();
                                        objEr.HandleException(ex);
                                    }

                                    if (fileclass != "3780")
                                    {
                                        ((Label)Error.FindControl("lblError")).Text = "Invalid uploaded file";
                                        Error.Visible = true;
                                        return;
                                    }

                                    isUplaoded = true;
                                    GlobalTabName = Guid.NewGuid() + ext;

                                    Request.Files[i].SaveAs(Server.MapPath(savePath + GlobalTabName));

                                    EncryptionHelper.EncryptString(Server.MapPath(savePath + GlobalTabName), EncryptionKey);

                                    GlobalTabFileInsert = Request.Files[i].FileName;
                                    GlobalTabFile = GlobalTabFileInsert;
                                    sbGlobalTabNames.Append("");
                                    sbGlobalTabNames.Append(GlobalTabName + ",");
                                    sbPdfNames.Append(GlobalTabFileInsert + ",");
                                }

                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Invalid file Uploaded";
                                    Error.Visible = true;
                                    return;
                                }
                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = "Invalid file Uploaded";
                                Error.Visible = true;
                                return;
                            }
                        }
                    }


                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = "Please upload file";
                    Error.Visible = true;
                    return;
                }
                if (isUplaoded)
                {
                    sbGlobalTabNames.Remove(sbGlobalTabNames.ToString().LastIndexOf(','), 1);
                    sbPdfNames.Remove(sbPdfNames.ToString().LastIndexOf(','), 1);

                    if (ViewState["UploadedFile"] != null && ViewState["pdf"] != null)
                    {
                        sbPdfNames.Insert(0, Convert.ToString(ViewState["pdf"]) + ",");

                        sbGlobalTabNames.Insert(0, Convert.ToString(ViewState["UploadedFile"]) + ",");
                    }

                    objGlobalTabDomain.GlobalTabNote = sbPdfNames.ToString();
                    objGlobalTabDomain.UploadedPDF = sbGlobalTabNames.ToString();
                    UploadOn = DateTime.Now.ToString();
                }
                else
                {
                    objGlobalTabDomain.UploadedPDF = ViewState["UploadedFile"].ToString();
                    objGlobalTabDomain.GlobalTabNote = ViewState["pdf"].ToString();
                }

                objGlobalTabDomain.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                objGlobalTabDomain.CreatedBy = Guid.Parse(Session["UserId"].ToString());



                if (hdnGlobalTabId.Value == "")
                {

                    objGlobalTabDomain = GlobalTabDataProvider.Instance.Insert(objGlobalTabDomain);
                    if (objGlobalTabDomain.GlobalTabId > 0)
                    {
                        var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objGlobalTabDomain);
                        var encrypt = Encryptor.EncryptString(json);

                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objGlobalTabDomain.UpdatedBy), "Global Tab", "Success", Convert.ToString(objGlobalTabDomain.GlobalTabId), "GlobalTab inserted successfully :- " + encrypt + "");

                        ((Label)Info.FindControl("lblName")).Text = "GlobalTab inserted successfully";
                        BindGlobalTab();
                        ClearData();

                        Info.Visible = true;
                    }

                }
                else
                {

                    objGlobalTabDomain.GlobalTabId = Convert.ToInt32(hdnGlobalTabId.Value);
                    bool status = GlobalTabDataProvider.Instance.Update(objGlobalTabDomain);

                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objGlobalTabDomain);
                    var encrypt = Encryptor.EncryptString(json);

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objGlobalTabDomain.UpdatedBy), "Global Tab", "Success", Convert.ToString(objGlobalTabDomain.GlobalTabId), "GlobalTab updated successfully :- " + encrypt + "");

                    ((Label)Info.FindControl("lblName")).Text = "GlobalTab updated successfully";
                    BindGlobalTab();
                    ClearData();
                    Info.Visible = true;
                }


                if (isUplaoded)
                {
                    List<string> strPdfsList = new List<string>();
                    string Deletedpdf = "";
                    string[] arrGlobalTabFiles = objGlobalTabDomain.UploadedPDF.Split(',');

                    if (ViewState["delFile"] != null)
                    {
                        Deletedpdf = (hdnDeletePdfName.Value).ToString();
                    }

                    string[] arrDeletedpdf = Deletedpdf.Split(',');
                    for (int pdfUp = 0; pdfUp <= arrGlobalTabFiles.Count() - 1; pdfUp++)
                    {
                        if (!string.IsNullOrWhiteSpace(arrGlobalTabFiles[pdfUp]) && !arrDeletedpdf.Contains(arrGlobalTabFiles[pdfUp].Trim()))
                        {
                            strPdfsList.Add(arrGlobalTabFiles[pdfUp].Trim());
                        }
                    }

                    bool status = MergePdf(strPdfsList, arrGlobalTabFiles[0].Trim());
                    if (!status)
                    {
                        return;
                    }

                }


            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }



        public void CombineMultiplePDFs(string[] fileNames, string outFile)
        {
            // step 1: creation of a document-object
            Document document = new Document();
            string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["KeyInfo"]);

            // step 2: we create a writer that listens to the document
            PdfCopy writer = new PdfCopy(document, new FileStream(Server.MapPath(savePath + "" + outFile), FileMode.Create));
            if (writer == null)
            {
                return;
            }

            // step 3: we open the document
            document.Open();

            foreach (string fileName in fileNames)
            {
                // we create a reader for a certain document
                PdfReader reader = new PdfReader(Server.MapPath(savePath + "/" + fileName));
                reader.ConsolidateNamedDestinations();

                // step 4: we add content
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    writer.AddPage(page);
                }

                PRAcroForm form = reader.AcroForm;
                if (form != null)
                {
                    //writer.CopyAcroForm(reader);
                    writer.AddDocument(reader);
                }

                reader.Close();
            }

            // step 5: we close the document and writer
            writer.Close();
            document.Close();
        }


        /// <summary>
        /// Merge pdf files
        /// </summary>
        /// <param name="files"></param>
        /// <param name="strFileName"></param>
        public bool MergePdf(List<string> files, string strFileName)
        {
            try
            {
                //// Open the output document
                //PdfDocument outputDocument = new PdfDocument();
                if (files.Count == 0)
                {
                    return true;
                }

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["KeyInfo"]);
                //// Iterate files
                //foreach (string file in files)
                //{
                //    //   System.Web.Hosting.HostingEnvironment.MapPath();
                //    string paths = Server.MapPath(savePath + "/" + file);
                //    //System.Web.Hosting.HostingEnvironment.MapPath("~/" + savePath + "/" + file);//System.Web.HttpContext.Current.Server.MapPath(savePath + "/" + file);
                //    // Open the document to import pages from it.
                //    PdfDocument inputDocument = PdfReader.Open(paths, PdfDocumentOpenMode.Import);

                //    // Iterate pages
                //    int count = inputDocument.PageCount;
                //    for (int idx = 0; idx < count; idx++)
                //    {
                //        // Get the page from the external document...
                //        PdfPage page = inputDocument.Pages[idx];
                //        // ...and add it to the output document.
                //        outputDocument.AddPage(page);
                //    }
                //}

                //// Save the document...

                //outputDocument.Save(Server.MapPath(savePath + "/" + strFileName.Split('.')[0] + "_merge.pdf"));
                ////System.Web.Hosting.HostingEnvironment.MapPath("~/" + savePath + "/" + strFileName.Split('.')[0] + "_merge.pdf"));


                iTextSharp.text.Document document = new iTextSharp.text.Document();

                string File_Name = Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge.pdf");

                if (File.Exists(File_Name))
                {
                    File.Delete(File_Name);
                }

                File_Name = Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf");

                if (File.Exists(File_Name))
                {
                    File.Delete(File_Name);
                }

                // step 2: we create a writer that listens to the document
                PdfCopy writer = new PdfCopy(document, new FileStream(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf"), FileMode.Create));

                if (writer == null)
                {
                    return false;
                }

                // step 3: we open the document
                document.Open();
                Dictionary<string, string> dictKeys = null;
                Dictionary<string, string> dictEncrKeys = null;
                string AgendaEncrKey = "";
                AgendaEncrKey = Session["EncryptionKey"].ToString();

                bool pdfCreate = false;
                foreach (string fileName in files)
                {
                    string AgendaKey = "";

                    if (dictKeys != null)
                    {
                        if (dictKeys.ContainsKey(fileName))
                        {
                            AgendaKey = dictKeys[fileName];
                            AgendaEncrKey = MM.Core.Encryptor.DecryptString(dictEncrKeys[fileName]);
                        }
                    }

                    PdfReader reader = null;

                    //string PdfPassword = EncryptionHelper.GetPassword(fileName, AgendaKey);
                    //if (PdfPassword != "")
                    //{
                    //    byte[] password = System.Text.ASCIIEncoding.ASCII.GetBytes(PdfPassword);

                    #region aes 128 bit decryption
                    // we create a reader for a certain document
                    //using (AesCryptoServiceProvider acsp = EncryptionHelper.GetProvider(Encoding.Default.GetBytes(AgendaEncrKey)))
                    //{
                    //    ICryptoTransform ictD = acsp.CreateDecryptor();

                    //    //RawBytes now contains original byte array, still in Encrypted state

                    //    //Decrypt into stream                            
                    //    using (FileStream msD = new FileStream(Server.MapPath(savePath + "/" + fileName), FileMode.Open))
                    //    {
                    //        using (CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read))
                    //        {
                    //            using (FileStream fsOutput = new FileStream(Server.MapPath(savePath + "/" + "oo" + fileName), FileMode.Create))
                    //            {
                    //                int data;
                    //                while ((data = csD.ReadByte()) != -1)
                    //                {
                    //                    fsOutput.WriteByte((byte)data);
                    //                }

                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                    byte[] decryptedBytes = null;
                    byte[] bytesToBeEncrypted = File.ReadAllBytes(Server.MapPath(savePath + "/" + fileName));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (RijndaelManaged rijndael = new RijndaelManaged())
                        {
                            byte[] ba = Encoding.Default.GetBytes(AgendaEncrKey);
                            var hexString = BitConverter.ToString(ba);
                            hexString = hexString.Replace("-", "");

                            byte[] bsa = Encoding.Default.GetBytes("0000000000000000");
                            var hexStrings = BitConverter.ToString(bsa);
                            hexStrings = hexStrings.Replace("-", "");

                            rijndael.Mode = CipherMode.CBC;
                            rijndael.Padding = PaddingMode.PKCS7;
                            rijndael.KeySize = 256;
                            rijndael.BlockSize = 128;
                            rijndael.Key = EncryptionHelper.StringToByteArray(hexString);//"3030303030303030303030303030303030303030303030303030303030303030"); //prekey2; //key;

                            rijndael.IV = EncryptionHelper.StringToByteArray(hexStrings);//"30303030303030303030303030303030");//iv;


                            using (var cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                                cs.Close();
                            }
                            decryptedBytes = ms.ToArray();
                        }
                    }
                    File.WriteAllBytes(Server.MapPath(savePath + "/" + "oo" + fileName), decryptedBytes);
                    reader = new PdfReader(Server.MapPath(savePath + "/" + "oo" + fileName));
                    // }
                    //else
                    //{
                    //      reader = new PdfReader(Server.MapPath(savePath + "/" + fileName));
                    //}
                    reader.ConsolidateNamedDestinations();

                    // step 4: we add content
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                        pdfCreate = true;
                    }

                    PRAcroForm form = reader.AcroForm;
                    if (form != null)
                    {
                        // writer.CopyAcroForm(reader);
                        writer.AddDocument(reader);
                    }

                    reader.Close();
                    if (File.Exists(Server.MapPath(savePath + "/" + "oo" + fileName)))
                    {
                        File.Delete(Server.MapPath(savePath + "/" + "oo" + fileName));
                    }
                }

                // step 5: we close the document and writer
                writer.Close();
                document.Close();

                if (pdfCreate)
                {
                    string AgendaKey = "";

                    if (dictKeys != null)
                    {
                        if (dictKeys.ContainsKey(strFileName))
                        {
                            AgendaKey = dictKeys[strFileName];
                            AgendaEncrKey = MM.Core.Encryptor.DecryptString(dictEncrKeys[strFileName]);
                        }
                    }
                    string pdfPass = EncryptionHelper.GetPassword(strFileName, AgendaKey);
                    if (pdfPass != "")
                    {
                        using (Stream input = new FileStream(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf"), FileMode.Open))//new FileStream(InputFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            using (Stream output = new FileStream(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge.pdf"), FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                            {
                                PdfReader reader = new PdfReader(input);

                                PdfEncryptor.Encrypt(reader, output, true, pdfPass, pdfPass, PdfWriter.ALLOW_MODIFY_ANNOTATIONS);

                            }
                        }
                    }
                    else
                    {
                        System.IO.File.Move(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf"), Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge.pdf"));
                    }

                    if (File.Exists(File_Name))
                    {
                        File.Delete(File_Name);
                    }


                    EncryptionHelper.EncryptString(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge.pdf"), AgendaEncrKey);

                }
                return true;
            }

            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Uploaded Pdf Error: " + ex.Message;//"Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);

                return false;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        /// <summary>
        /// Bind Global Tab to grid
        /// </summary>
        private void BindGlobalTab()
        {
            try
            {
                IList<GlobalTabDomain> objGlobalTab = GlobalTabDataProvider.Instance.Get();
                grdGlobalTab.DataSource = objGlobalTab;
                grdGlobalTab.DataBind();

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }


        /// <summary>
        /// clear form data
        /// </summary>
        private void ClearData()
        {

            ViewState["UploadedFile"] = null;
            ViewState["pdf"] = null;
            txtTitle.Text = "";
            hdnGlobalTabId.Value = "";
            btnInsert.Text = "Save";
            lblPdfs.Text = "";
        }

        protected void grdGlobalTab_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grdGlobalTab_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                ClearData();
                grdGlobalTab.PageIndex = e.NewPageIndex;
                BindGlobalTab();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void grdGlobalTab_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {


                DataTable dt = (DataTable)GlobalTabDataProvider.Instance.Get().AsDataTable();
                DataView dv = new DataView(dt);
                if (Convert.ToString(ViewState["sortDirection"]) == "asc")
                {
                    ViewState["sortDirection"] = "dsc";
                    dv.Sort = e.SortExpression + " DESC";
                }
                else
                {
                    ViewState["sortDirection"] = "asc";
                    dv.Sort = e.SortExpression + " ASC";
                }

                grdGlobalTab.DataSource = dv;
                grdGlobalTab.DataBind();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }



        /// <summary>
        /// Delete pdf agenda event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalTabDomain objGlobalTabDomain = new GlobalTabDomain();
                string deletePdf = hdnDeletePdfName.Value;
                string deleteOriginalPdfName = hdnDeleteOriginalPdfName.Value;

                if (ViewState["delFile"] != null)
                {
                    deletePdf = Convert.ToString(ViewState["delFile"]) + "," + hdnDeletePdfName.Value;
                }
                string GlobalTabFilePdf = "";
                string GlobalTabFileTimeName = "";

                string[] fileNames = Convert.ToString(ViewState["UploadedFile"]).Replace(" ", "").Split(',');
                string[] pdfNames = Convert.ToString(ViewState["pdf"]).Split(',');

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["KeyInfo"]);

                if (hdnDelete.Value == "delete")
                {
                    //  File.Delete(Server.MapPath(savePath + hdnDeletePdfName.Value));
                }
                if (hdnDelete.Value == "don't")
                {
                    int pos = Array.IndexOf(fileNames, hdnDeletePdfName.Value.Trim());
                    if (pos > -1)
                    {
                        GlobalTabFilePdf = pdfNames[pos];
                        GlobalTabFileTimeName = fileNames[pos];
                    }
                }


                //comma sepated string
                string joinedPdfNames = String.Join(", ", pdfNames);
                string joinedFileNames = String.Join(", ", fileNames);


                objGlobalTabDomain.UploadedPDF = joinedFileNames;
                objGlobalTabDomain.GlobalTabNote = joinedPdfNames;

                //create Merged pdf

                // List<string> strPdfsList = new List<string>();

                string strPdfsList = "";

                string[] arrDeletedpdf = deletePdf.Split(',');
                string[] arrGlobalTabFiles = objGlobalTabDomain.UploadedPDF.Split(',');

                int pdfIndex = -1;
                for (int pdfUp = 0; pdfUp <= arrGlobalTabFiles.Count() - 1; pdfUp++)
                {
                    if (!string.IsNullOrWhiteSpace(arrGlobalTabFiles[pdfUp]) && !arrDeletedpdf.Contains(arrGlobalTabFiles[pdfUp].Trim()))
                    {
                        //strPdfsList.Add(arrGlobalTabFiles[pdfUp].Trim());
                        strPdfsList = strPdfsList + arrGlobalTabFiles[pdfUp].Trim() + ",";
                        pdfIndex = pdfUp;
                    }
                }


                string strOriginalPdfsList = "";

                string[] arrDeletedOriginalpdf = deleteOriginalPdfName.Split(',');
                string[] arrGlobalTabNotes = objGlobalTabDomain.GlobalTabNote.Split(',');

                if (arrGlobalTabNotes.Count() > 0)
                {
                    List<string> tmp = new List<string>(arrGlobalTabNotes);
                    tmp.RemoveAt(pdfIndex);
                    arrGlobalTabNotes = tmp.ToArray();
                }

                //for (int pdfUp = 0; pdfUp <= arrGlobalTabFiles.Count() - 1; pdfUp++)
                //{
                //    if (!string.IsNullOrWhiteSpace(arrGlobalTabNotes[pdfUp]) && !arrDeletedOriginalpdf.Contains(arrGlobalTabNotes[pdfUp].Trim()))
                //    {                        
                //        strOriginalPdfsList = strOriginalPdfsList + arrGlobalTabNotes[pdfUp].Trim() + ",";
                //    }
                //}

                strPdfsList = strPdfsList.TrimEnd(',');
                objGlobalTabDomain.GlobalTabId = Convert.ToInt32(hdnGlobalTabId.Value);
                objGlobalTabDomain.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                objGlobalTabDomain.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                objGlobalTabDomain.Title = txtTitle.Text.Trim();
                objGlobalTabDomain.UploadedPDF = strPdfsList.ToString();
                objGlobalTabDomain.GlobalTabNote = String.Join(", ", arrGlobalTabNotes);//strOriginalPdfsList.ToString();
                bool status = GlobalTabDataProvider.Instance.Update(objGlobalTabDomain);

                //MergePdf(strPdfsList, arrGlobalTabFiles[0].Trim());


                objGlobalTabDomain.GlobalTabId = Convert.ToInt32(hdnGlobalTabId.Value);
                BindGlobalTab();
                EditGlobalTab();





                Info.Visible = true;
                // EditAgenda(Guid.Parse(hdnAgendaId.Value), ddlAgendaMeeting.SelectedValue, sender, e);
                // ClearData();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
                Info.Visible = false;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }



        protected void grdGlobalTab_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {

                if (e.CommandName.ToLower().Equals("edit"))
                {

                    GlobalTabDomain objGlobalTabDomain = GlobalTabDataProvider.Instance.Get(Convert.ToInt32(Convert.ToString(e.CommandArgument)));
                    txtTitle.Text = objGlobalTabDomain.Title.ToString();
                    hdnGlobalTabId.Value = objGlobalTabDomain.GlobalTabId.ToString();

                    if (objGlobalTabDomain.UploadedPDF != null)
                    {
                        if (objGlobalTabDomain.UploadedPDF.Length > 0)
                        {
                            string[] strDeletedGlobalTabs = objGlobalTabDomain.DeletedPDF.Split(',');
                            StringBuilder sbGlobalTab = new StringBuilder("");
                            string[] globalTabsNames = objGlobalTabDomain.UploadedPDF.Split(',');
                            string[] globalTabsPdfs = objGlobalTabDomain.GlobalTabNote.Split(',');
                            if (objGlobalTabDomain.DeletedPDF != "")
                            {
                                ViewState["file"] = objGlobalTabDomain.DeletedPDF;
                            }

                            if (objGlobalTabDomain.GlobalTabNote != "")
                            {
                                ViewState["pdf"] = objGlobalTabDomain.GlobalTabNote;
                            }

                            if (objGlobalTabDomain.DeletedPDF != "")
                            {
                                ViewState["delFile"] = objGlobalTabDomain.DeletedPDF;
                            }
                            if (objGlobalTabDomain.UploadedPDF != "")
                            {
                                ViewState["UploadedFile"] = objGlobalTabDomain.UploadedPDF;
                            }
                            for (int i = 0; i <= globalTabsNames.Count() - 1; i++)
                            {
                                if (!strDeletedGlobalTabs.Contains(globalTabsNames[i].Trim()))
                                {
                                    // sbAgenda.Append(" <a href='javascript:void(0)' onclick=ViewAgendaPdf('" + agendaNames[i].Trim() + "') >" + agendaPdfs[i].Trim() + " </a>&nbsp;&nbsp; <a href='javascript:void(0)' onclick=DelAgendaPdf('" + agendaNames[i].Trim() + "') >Delete </a><br/> ");
                                    sbGlobalTab.Append(" <a href='javascript:void(0)' onclick=ViewGlobalTabPDF('" + globalTabsNames[i].Trim() + "')  >" + globalTabsPdfs[i].Trim() + " </a>&nbsp;&nbsp; <a href='javascript:void(0)' onclick=DelAgendaPdf('" + globalTabsNames[i].Trim() + "','" + globalTabsPdfs[i].Trim() + "') >Delete </a><br/> ");
                                }

                            }

                            lblPdfs.Text = sbGlobalTab.ToString();
                            // lnkViewPdf.Visible = true;
                        }
                        else
                        {
                            lblPdfs.Text = "";
                        }
                    }

                    btnInsert.Text = "Update";

                }
                //delete 
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    ClearData();
                    bool status = GlobalTabDataProvider.Instance.Delete(Convert.ToInt32(Convert.ToString(e.CommandArgument)));
                    ((Label)Info.FindControl("lblName")).Text = "GlobalTab deleted successfully";
                    Info.Visible = true;

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Global Tab", "Success", Convert.ToString(e.CommandArgument), "GlobalTab deleted successfully");

                    BindGlobalTab();
                }

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }


        protected void EditGlobalTab()
        {
            try
            {
                GlobalTabDomain objGlobalTabDomain = GlobalTabDataProvider.Instance.Get(Convert.ToInt32(hdnGlobalTabId.Value));
                txtTitle.Text = objGlobalTabDomain.Title.ToString();
                hdnGlobalTabId.Value = objGlobalTabDomain.GlobalTabId.ToString();

                if (objGlobalTabDomain.UploadedPDF != null)
                {
                    if (objGlobalTabDomain.UploadedPDF.Length > 0)
                    {
                        string[] strDeletedGlobalTabs = objGlobalTabDomain.DeletedPDF.Split(',');
                        StringBuilder sbGlobalTab = new StringBuilder("");
                        string[] globalTabsNames = objGlobalTabDomain.UploadedPDF.Split(',');
                        string[] globalTabsPdfs = objGlobalTabDomain.GlobalTabNote.Split(',');
                        if (objGlobalTabDomain.DeletedPDF != "")
                        {
                            ViewState["file"] = objGlobalTabDomain.DeletedPDF;
                        }

                        if (objGlobalTabDomain.GlobalTabNote != "")
                        {
                            ViewState["pdf"] = objGlobalTabDomain.GlobalTabNote;
                        }

                        if (objGlobalTabDomain.DeletedPDF != "")
                        {
                            ViewState["delFile"] = objGlobalTabDomain.DeletedPDF;
                        }
                        if (objGlobalTabDomain.UploadedPDF != "")
                        {
                            ViewState["UploadedFile"] = objGlobalTabDomain.UploadedPDF;
                        }
                        for (int i = 0; i <= globalTabsNames.Count() - 1; i++)
                        {
                            if (!strDeletedGlobalTabs.Contains(globalTabsNames[i].Trim()))
                            {
                                // sbAgenda.Append(" <a href='javascript:void(0)' onclick=ViewAgendaPdf('" + agendaNames[i].Trim() + "') >" + agendaPdfs[i].Trim() + " </a>&nbsp;&nbsp; <a href='javascript:void(0)' onclick=DelAgendaPdf('" + agendaNames[i].Trim() + "') >Delete </a><br/> ");
                                sbGlobalTab.Append(" <a href='javascript:void(0)' onclick=ViewGlobalTabPDF('" + globalTabsNames[i].Trim() + "')  >" + globalTabsPdfs[i].Trim() + " </a>&nbsp;&nbsp; <a href='javascript:void(0)' onclick=DelAgendaPdf('" + globalTabsNames[i].Trim() + "','" + globalTabsPdfs[i].Trim() + "') >Delete </a><br/> ");

                            }

                        }

                        lblPdfs.Text = sbGlobalTab.ToString();
                        // lnkViewPdf.Visible = true;
                    }
                    else
                    {
                        lblPdfs.Text = "";
                    }
                }


                btnInsert.Text = "Update";
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void grdGlobalTab_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = hdnPdfName.Value;

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["KeyInfo"]);
                string FilePath = Server.MapPath(savePath + fileName);

                byte[] decryptedBytes = null;
                byte[] bytesToBeEncrypted = File.ReadAllBytes(FilePath);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged rijndael = new RijndaelManaged())
                    {
                        byte[] ba = Encoding.Default.GetBytes(Session["EncryptionKey"].ToString());
                        var hexString = BitConverter.ToString(ba);
                        hexString = hexString.Replace("-", "");

                        byte[] bsa = Encoding.Default.GetBytes("0000000000000000");
                        var hexStrings = BitConverter.ToString(bsa);
                        hexStrings = hexStrings.Replace("-", "");

                        rijndael.Mode = CipherMode.CBC;
                        rijndael.Padding = PaddingMode.PKCS7;
                        rijndael.KeySize = 256;
                        rijndael.BlockSize = 128;
                        rijndael.Key = EncryptionHelper.StringToByteArray(hexString);//"3030303030303030303030303030303030303030303030303030303030303030"); //prekey2; //key;

                        rijndael.IV = EncryptionHelper.StringToByteArray(hexStrings);//"30303030303030303030303030303030");//iv;

                        using (var cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                            cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                    }
                    //Set the appropriate ContentType.
                    Response.ContentType = "Application/pdf";
                    //Get the physical path to the file.
                    // string FilePath = Server.MapPath(savePath + fileName);

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + "GlobalTab.pdf");
                    // Response.WriteFile(FilePath);
                    Response.BinaryWrite(ms.ToArray());
                    Response.Flush();
                }

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }


    }
}