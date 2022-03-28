using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MeetingMinder.Web
{
    public partial class FileDecrypt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnConvert_Click(object sender, EventArgs e)
        {
            string filPath = "img/Uploads/ProfilePic/" + DateTime.Now.Ticks+".pdf";
            fuFile.SaveAs(Server.MapPath(filPath));
            string filPathNew = "img/Uploads/ProfilePic/" + DateTime.Now.Ticks + "_new.pdf";


            using (AesCryptoServiceProvider acsp = EncryptionHelper.GetProvider(Encoding.Default.GetBytes(txtKey.Text)))
            {
                ICryptoTransform ictD = acsp.CreateDecryptor();

                //RawBytes now contains original byte array, still in Encrypted state

                //Decrypt into stream
                using (FileStream msD = new FileStream(Server.MapPath(filPath), FileMode.Open))
                {
                    using (CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read))
                    {
                        using (FileStream fsOutput = new FileStream(Server.MapPath(filPathNew), FileMode.Create))
                        {
                            int data;
                            while ((data = csD.ReadByte()) != -1)
                            {
                                fsOutput.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {

                Response.ContentType = "application/octet-stream";

                Response.AddHeader("Content-Disposition", "attachment; filename=" + "Agenda.pdf");
                // Response.WriteFile(FilePath);
                Response.BinaryWrite(File.ReadAllBytes(Server.MapPath(filPathNew)).ToArray());
                Response.Flush();

                //  Response.End();
            }
        }
    }
}