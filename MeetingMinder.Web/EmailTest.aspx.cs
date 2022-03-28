using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MeetingMinder.Web
{
    public partial class EmailTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrMsg.Text = "";
        }

        protected void btnsend_Click(object sender, EventArgs e)
        {
            try
            {
                SmtpClient mailClient = null;
                System.Net.Mail.MailMessage message = null;

                mailClient = new SmtpClient();
                message = new System.Net.Mail.MailMessage();

                //  mailClient.EnableSsl = true;

                MailAddress fromAddress = new MailAddress(txtFrom.Text);
                message.From = fromAddress;
                message.To.Add(txtToMail.Text);
                message.Subject = txtSubject.Text;
                message.Body = txtBody.Text;
                mailClient.EnableSsl = chkssl.Checked;
              
                mailClient.Host = txtDomain.Text.Trim();
                mailClient.Port = Convert.ToInt16(txtPort.Text);
            
                mailClient.Send(message);
                lblErrMsg.Text = "email sent";
            }
            catch (Exception ex)
            {
                Response.Write("<br/><br/> "+ex);
                System.Net.Mail.MailMessage messsage = null;
                SmtpClient mailClient = null;
                mailClient = new SmtpClient();
                messsage = new System.Net.Mail.MailMessage();
                string to = txtToMail.Text;
                string from = txtFrom.Text;
                MailAddress fromAddress = new MailAddress(txtFrom.Text);
                messsage.From = fromAddress;
          
                messsage.To.Add(to);
                messsage.Subject = txtSubject.Text;
                messsage.Body = txtBody.Text;
                SmtpClient client = new SmtpClient(txtDomain.Text, Convert.ToInt16(txtPort.Text));
                // Credentials are necessary if the server requires the client  
                // to authenticate before it will send e-mail on the client's behalf.
                client.UseDefaultCredentials = true;

                try
                {
                    client.Send(messsage);
                }
                catch (Exception esx)
                {
                    Response.Write("<br/><br/> "+esx);
                }              
            }
        }

        protected void btn2_Click(object sender, EventArgs e)
        {
            try
            {
                SmtpClient objSmpt = new SmtpClient(txtDomain.Text, Convert.ToInt16(txtPort.Text));
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                MailAddress fromAddress = new MailAddress(txtFrom.Text);

                message.From = fromAddress;
                message.To.Add(txtToMail.Text);
                message.Subject = txtSubject.Text;
                message.Body = txtBody.Text;
                objSmpt.EnableSsl = chkssl.Checked;
                objSmpt.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmpt.Send(message);
                lblErrMsg.Text = "sent email";
            }
            catch (Exception essx)
            {
                Response.Write("<br/><br/> "+essx);

                try
                {
                    SmtpClient objSmpt = new SmtpClient(txtDomain.Text, Convert.ToInt16(txtPort.Text));
                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                    MailAddress fromAddress = new MailAddress(txtFrom.Text);

                    message.From = fromAddress;
                    message.To.Add(txtToMail.Text);
                    message.Subject = txtSubject.Text;
                    message.Body = txtBody.Text;
                    objSmpt.EnableSsl = chkssl.Checked;
                    objSmpt.UseDefaultCredentials = true;
                    objSmpt.Send(message);
                    lblErrMsg.Text = "sent email 1";
                }
                catch (Exception ex)
                {
                    Response.Write("<br/><br/> "+"1 " + ex);

                    try
                    {
                        SmtpClient objSmpt = new SmtpClient(txtDomain.Text, Convert.ToInt16(txtPort.Text));
                        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                        MailAddress fromAddress = new MailAddress(txtFrom.Text);
                        message.From = fromAddress;
                        message.To.Add(txtToMail.Text);
                        message.Subject = txtSubject.Text;
                        message.Body = txtBody.Text;
                        objSmpt.EnableSsl = chkssl.Checked;
                        objSmpt.UseDefaultCredentials = false;
                        objSmpt.Send(message);
                        lblErrMsg.Text = "sent email to";
                    }
                    catch (Exception dex)
                    {

                        Response.Write("<br/><br/> "+"2 " + dex);

                        try
                        {
                            SmtpClient objSmpt = new SmtpClient(txtDomain.Text);
                            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                            MailAddress fromAddress = new MailAddress(txtFrom.Text);
                            message.From = fromAddress;
                            message.To.Add(txtToMail.Text);
                            message.Subject = txtSubject.Text;
                            message.Body = txtBody.Text;
                            objSmpt.EnableSsl = chkssl.Checked;
                            objSmpt.UseDefaultCredentials = false;
                            objSmpt.Send(message);
                            lblErrMsg.Text = "sent 1 email";
                        }
                        catch (Exception decx)
                        {
                            Response.Write("<br/><br/> "+"3 " + decx);

                        }
                    }
                }



            }
        }

        protected void btns_Click(object sender, EventArgs e)
        {
            try
            {
                SmtpClient objSmpt = new SmtpClient(txtDomain.Text, Convert.ToInt16(txtPort.Text));
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                MailAddress fromAddress = new MailAddress(txtFrom.Text);
                
                message.From = fromAddress;
                message.To.Add(txtToMail.Text);
                message.Subject = txtSubject.Text;
                message.Body = txtBody.Text;
                objSmpt.EnableSsl = chkssl.Checked;
                objSmpt.UseDefaultCredentials = true;
                objSmpt.Send(message);
                lblErrMsg.Text = "sent email 1";
            }
            catch (Exception ex)
            {             
                Response.Write("<br/><br/> "+"1 "+ ex);

                try
                {
                    SmtpClient objSmpt = new SmtpClient(txtDomain.Text, Convert.ToInt16(txtPort.Text));
                    System.Net.Mail.MailMessage message =  new System.Net.Mail.MailMessage();
                    MailAddress fromAddress = new MailAddress(txtFrom.Text);
                    message.From = fromAddress;
                    message.To.Add(txtToMail.Text);
                    message.Subject = txtSubject.Text;
                    message.Body = txtBody.Text;
                    objSmpt.EnableSsl = chkssl.Checked;
                    objSmpt.UseDefaultCredentials = false;
                    objSmpt.Send(message);
                    lblErrMsg.Text = "sent email to";
                }
                catch (Exception dex)
                {

                    Response.Write("<br/><br/> "+"2 " + dex);

                    try
                    {
                        SmtpClient objSmpt = new SmtpClient(txtDomain.Text);
                        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                        MailAddress fromAddress = new MailAddress(txtFrom.Text);
                        message.From = fromAddress;
                        message.To.Add(txtToMail.Text);
                        message.Subject = txtSubject.Text;
                        message.Body = txtBody.Text;
                        objSmpt.EnableSsl = chkssl.Checked;
                        objSmpt.UseDefaultCredentials = false;
                        objSmpt.Send(message);
                        lblErrMsg.Text = "sent 1 email";
                    }
                    catch (Exception decx)
                    {
                        Response.Write("<br/><br/> "+"3 " + decx);

                        try
                        {
                            SmtpClient objSmpt = new SmtpClient(txtDomain.Text);
                            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                            MailAddress fromAddress = new MailAddress(txtFrom.Text);
                            message.From = fromAddress;
                            message.To.Add(txtToMail.Text);
                            message.Subject = txtSubject.Text;
                            message.Body = txtBody.Text;
                            objSmpt.EnableSsl = chkssl.Checked;
                            objSmpt.Credentials = CredentialCache.DefaultNetworkCredentials;
                            objSmpt.Send(message);
                            lblErrMsg.Text = "sent test email";
                        }
                        catch (Exception descx)
                        {
                            Response.Write("<br/><br/> " + "3 " + descx);

                        }
                    }
                }
            }
        }

    }
}