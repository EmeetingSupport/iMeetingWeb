using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Net;
using MeetingMInder.WCFServices.Request;
using System.Web.Script.Serialization;
using MM.Domain;
using MM.Services;

namespace MeetingMInder.WCFServices.MessageInterceptor
{
    public class CustomMessageInspector : IDispatchMessageInspector
    {
        string reqMsg = String.Empty;

        DateTime reqTimeStamp = new DateTime();
        DateTime resTimeStamp = new DateTime();
        bool IsValidUser = false;
        string url = String.Empty;
        string serviceName = String.Empty;

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            return null;
        }

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            string serviceName2 = String.Empty;

            try
            {
                url = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.AbsoluteUri.ToString();
                serviceName2 = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.Segments[WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.Segments.Length - 1].ToString();

                try
                {
                    string reqMsg2 = String.Empty;
                    reqTimeStamp = System.DateTime.Now;

                    Logger objLogger = new Logger();
                    Guid objMapperId = Guid.NewGuid();
                    if (WebOperationContext.Current.IncomingRequest.Method.ToString() == "POST")
                    {
                        reqMsg2 = MessageToString(ref request);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        Dictionary<string, string> responseObject = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

                        if (serviceName2.ToLower().Equals("agendaacknowledgement"))
                        {
                            objLogger.Mapper = objMapperId;
                            objLogger.Request = reqMsg2;
                            objLogger.OperationName = serviceName2;
                            objLogger.UserIpAddress = GetUserIP();

                            if (WebOperationContext.Current.IncomingRequest.Headers["UserId"] != null)
                            {
                                objLogger.UserId = Guid.Parse(WebOperationContext.Current.IncomingRequest.Headers["UserId"].ToString());
                            }
                            return objLogger;
                        }
                        else
                        {
                            responseObject = serializer.Deserialize<Dictionary<string, string>>(reqMsg2);

                            if (responseObject != null)
                            {
                                if (responseObject.ContainsKey("UserId"))
                                {
                                    Guid userid = Guid.Empty;
                                    Guid.TryParse(responseObject["UserId"], out userid);
                                    objLogger.UserId = userid;
                                }

                                if (responseObject.ContainsKey("EntityId"))
                                {
                                    Guid entity = Guid.Empty;
                                    Guid.TryParse(responseObject["EntityId"], out entity);
                                    objLogger.EntityId = entity;
                                }

                                if (responseObject.ContainsKey("Mac"))
                                {
                                    objLogger.Mac = responseObject["Mac"];
                                }

                                if (responseObject.ContainsKey("DeviceTime"))
                                {
                                    objLogger.DeviceTime = responseObject["DeviceTime"];
                                }

                                if (responseObject.ContainsKey("UdId"))
                                {
                                    objLogger.Mac = responseObject["UdId"];
                                    //if (responseObject.ContainsKey("Password"))
                                    //{
                                    //    objLogger.UdId = responseObject["Password"];
                                    //}
                                    //else
                                    //{
                                    //    objLogger.UdId = responseObject["UdId"];
                                    //}
                                }

                                if (responseObject.ContainsKey("AppVersion"))
                                {
                                    objLogger.AppVersion = responseObject["AppVersion"];
                                }
                            }

                            objLogger.Mapper = objMapperId;
                            objLogger.Request = reqMsg2;
                            objLogger.OperationName = serviceName2;
                            objLogger.UserIpAddress = GetUserIP();
                        }

                    }
                    reqMsg2 += WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.AbsoluteUri.ToString();

                    //new MessageLoggerDb().AddMessageLog(objLogger);

                    //      return objMapperId;
                    return objLogger;
                    //new MessageLogger { Req = reqMsg2, ReqTimeStamp = System.DateTime.Now, ServiceName = serviceName2 });
                }
                catch (Exception ex)
                {
                    LogError objEr = new LogError();
                    objEr.HandleException(ex);
                    //  new MessageLoggerDb().AddMessageLog(new MessageLogger { Res = ex.Message + " --> " + ex.StackTrace + " --> " + lineNo, ResTimeStamp = System.DateTime.Now, ServiceName = serviceName2 });
                }

                return null;

            }
            catch (Exception ex)
            {
                LogError objEr = new LogError();
                objEr.HandleException(ex);

                //CommonResponse commonResponse = new CommonResponse();

                //new MessageLoggerDb().AddMessageLog(new MessageLogger { Res = ex.Message + " --> " + ex.StackTrace + " --> " + lineNo, ResTimeStamp = System.DateTime.Now, ServiceName = serviceName2 });
            }

            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            if (correlationState == null)
            {
                reply = Message.CreateMessage(MessageVersion.None, null);
                HttpResponseMessageProperty responseProp = new HttpResponseMessageProperty()
                {
                    StatusCode = HttpStatusCode.BadRequest
                };

                MemoryStream memStream = new MemoryStream();
                XmlDictionaryWriter xdw = XmlDictionaryWriter.CreateBinaryWriter(memStream);

                xdw.WriteStartElement("Response", "http://tempuri.org/");
                xdw.WriteStartElement("InnerResponse", "http://tempuri.org/");
                xdw.WriteAttributeString("type", "error");
                xdw.WriteString("Server error");
                xdw.WriteEndElement();
                xdw.WriteEndElement();
                xdw.Flush();
                memStream.Position = 0;

                XmlDictionaryReaderQuotas quotas = new XmlDictionaryReaderQuotas();
                XmlDictionaryReader xdr = XmlDictionaryReader.CreateBinaryReader(memStream, quotas);

                Message replacedMessage = Message.CreateMessage(reply.Version, null, xdr);
                replacedMessage.Headers.CopyHeadersFrom(reply.Headers);

                replacedMessage.Properties.CopyProperties(reply.Properties);
                //replacedMessage.Properties[[HttpResponseMessageProperty.Name]  = responseProp;
                reply = replacedMessage;
                reply.Properties[HttpResponseMessageProperty.Name] = responseProp;
            }
            else

                if (correlationState.GetType().ToString().ToLower().Equals("system.string"))
            {
                string head = (string)correlationState;
                if (head == "null header")
                {
                    reply = Message.CreateMessage(MessageVersion.None, null);
                    HttpResponseMessageProperty responseProp = new HttpResponseMessageProperty()
                    {
                        StatusCode = HttpStatusCode.Unauthorized
                    };

                    // Set response header
                    responseProp.Headers.Add("WWW-Authenticate", String.Format("Basic realm=\"{0}\"", "ravi"));
                    reply.Properties[HttpResponseMessageProperty.Name] = responseProp;
                    // requestContext.Reply(reply);

                    // set the request context to null to terminate processing of this request
                    // requestContext = null;
                }
            }
            else
            {
                string serviceName2 = String.Empty;
                //try
                //{
                //int RequesterUserId = 0;
                Logger objLogger = (Logger)correlationState;

                string responseMsg = this.MessageToString(ref reply);

                resTimeStamp = System.DateTime.Now;

                string url2 = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.AbsoluteUri.ToString();


                serviceName2 = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.Segments[WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.Segments.Length - 1].ToString();

                string strAccessToken = "";

                if (WebOperationContext.Current.IncomingRequest.Headers["AccessToken"] != null)
                {
                    strAccessToken = WebOperationContext.Current.IncomingRequest.Headers["AccessToken"].ToString();
                    objLogger.RequestToken = strAccessToken;
                }

                string IsExpired = "false";
                if (WebOperationContext.Current.IncomingRequest.Headers["TokenExpired"] != null)
                {
                    IsExpired = WebOperationContext.Current.IncomingRequest.Headers["TokenExpired"].ToString();
                    objLogger.IsExpired = "true";
                }

                if (serviceName2.ToLower().Equals("login"))
                {

                    if (WebOperationContext.Current.OutgoingResponse.Headers["AccessToken"] != null)
                    {
                        strAccessToken = WebOperationContext.Current.OutgoingResponse.Headers["AccessToken"].ToString();
                        objLogger.RequestToken = strAccessToken;
                    }
                    string userId = getBetween(responseMsg);
                    if (userId != "")
                    {
                        objLogger.LoginUserId = Guid.Parse(userId);
                    }

                }
                objLogger.Response = responseMsg;
                if (serviceName2.ToLower().Equals("getdata"))
                {
                    return;
                }
                if (serviceName2.ToLower().Equals("userlock"))
                {
                    return;
                }
                if (serviceName2.ToLower().Equals("getdbserverstatus"))
                {
                    return;
                }
                if (serviceName2.ToLower().Equals("getappserverstatus"))
                {
                    return;
                }
                //if (serviceName2.ToLower().Equals("forum"))
                //{
                //    return;
                //}
                UserServices objService = new UserServices();

                if (objLogger.UserId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    UserDomain objUser = objService.Get(objLogger.UserId);
                    objLogger.UserName = objUser.FirstName + " " + objUser.LastName;
                }
                else
                {
                    if (objLogger.LoginUserId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    {
                        UserDomain objUser = objService.Get(objLogger.LoginUserId);
                        objLogger.UserName = objUser.FirstName + " " + objUser.LastName;
                    }
                }


                System.Data.DataSet objVal = new MessageLoggerDb().AddMessageLog(objLogger);

                if (objVal != null && objVal.Tables.Count > 0)
                {
                    if (objVal.Tables[0].Rows.Count > 0)
                    {
                        System.Data.DataColumnCollection columns = objVal.Tables[0].Columns;
                        if (serviceName2.ToLower().Equals("forgotpassword"))
                        {
                            return;
                        }
                        if (serviceName2.ToLower().Equals("keyinformation"))
                        {
                            return;
                        }
                        if (serviceName2.ToLower().Equals("getauthenticatedata"))
                        {
                            return;
                        }
                        if (columns.Contains("IsExpired"))
                        {
                            reply = Message.CreateMessage(MessageVersion.None, null);
                            HttpResponseMessageProperty responseProp = new HttpResponseMessageProperty()
                            {
                                StatusCode = HttpStatusCode.BadRequest
                            };
                            responseProp.Headers.Add("TokenExpired", "true");

                            MemoryStream memStream = new MemoryStream();
                            XmlDictionaryWriter xdw = XmlDictionaryWriter.CreateBinaryWriter(memStream);

                            xdw.WriteStartElement("Response", "http://tempuri.org/");
                            xdw.WriteStartElement("InnerResponse", "http://tempuri.org/");
                            xdw.WriteAttributeString("type", "error");
                            xdw.WriteString("Server error");
                            xdw.WriteEndElement();
                            xdw.WriteEndElement();
                            xdw.Flush();
                            memStream.Position = 0;

                            XmlDictionaryReaderQuotas quotas = new XmlDictionaryReaderQuotas();
                            XmlDictionaryReader xdr = XmlDictionaryReader.CreateBinaryReader(memStream, quotas);

                            Message replacedMessage = Message.CreateMessage(reply.Version, null, xdr);
                            replacedMessage.Headers.CopyHeadersFrom(reply.Headers);

                            replacedMessage.Properties.CopyProperties(reply.Properties);
                            //replacedMessage.Properties[[HttpResponseMessageProperty.Name]  = responseProp;
                            reply = replacedMessage;
                            reply.Properties[HttpResponseMessageProperty.Name] = responseProp;

                            // Set response header
                            //responseProp.Headers.Add("TokenExpired", "true");
                            //reply.Properties[HttpResponseMessageProperty.Name] = responseProp;
                        }
                        if (columns.Contains("AlreadyLogin"))
                        {

                            HttpResponseMessageProperty responseProp = new HttpResponseMessageProperty();
                            string strProperty = " ";
                            if (WebOperationContext.Current.OutgoingResponse.Headers["Property"] != null)
                            {
                                strProperty = WebOperationContext.Current.OutgoingResponse.Headers["Property"].ToString();
                            }

                            //  objLogger.RequestToken = strAccessToken;
                            responseProp.Headers.Add("AccessToken", strAccessToken);
                            responseProp.Headers.Add("Login", "true");
                            responseProp.Headers.Add("Property", strProperty);
                            reply.Properties[HttpResponseMessageProperty.Name] = responseProp;

                        }

                    }
                }



                // new MessageLoggerDb().AddMessageLog(new MessageLogger { Res = responseMsg, ResTimeStamp = System.DateTime.Now, ServiceName = serviceName2 });
                //  Guid objMapper = (Guid)correlationState;
                //  new MessageLoggerDb().UpdateLog(objMapper, responseMsg, strAccessToken, IsExpired);
                //}
                //catch (Exception ex)
                //{
                //    //string responseMsg = this.MessageToString(ref reply);
                //    //    new MessageLoggerDb().AddMessageLog(new MessageLogger { Res = ex.Message + " --> " + ex.StackTrace + " --> " + lineNo, ResTimeStamp = System.DateTime.Now, ServiceName = serviceName2 });
                //}
            }
        }

        #region JSON Helpers

        private static string GetUserIP()
        {
            string visitorsIPAddress = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                visitorsIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                string[] splitIP = visitorsIPAddress.Split(',');
                if (splitIP[0] != "")
                {
                    visitorsIPAddress = splitIP[0].ToString();
                }
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                visitorsIPAddress = HttpContext.Current.Request.UserHostAddress;
            }
            return visitorsIPAddress;
        }

        private string MessageToStringOnReply(ref Message message)
        {
            WebContentFormat messageFormat = this.GetMessageContentFormat(message);
            MemoryStream ms = new MemoryStream();
            XmlDictionaryWriter writer = null;
            switch (messageFormat)
            {
                case WebContentFormat.Default:
                case WebContentFormat.Xml:
                    writer = XmlDictionaryWriter.CreateTextWriter(ms);
                    break;
                case WebContentFormat.Json:
                    writer = JsonReaderWriterFactory.CreateJsonWriter(ms);

                    break;
                case WebContentFormat.Raw:
                    // special case for raw, easier implemented separately
                    return this.ReadRawBody(ref message);
            }

            message.WriteMessage(writer);

            writer.Flush();
            string messageBody = Encoding.UTF8.GetString(ms.ToArray());

            //Html decode message
            messageBody = WebUtility.HtmlDecode(messageBody);

            //byte[] jsonReplyBytes = Encoding.UTF8.GetBytes(messageBody);

            // Here would be a good place to change the message body, if so desired.

            // now that the message was read, it needs to be recreated.
            ms.Position = 0;

            // if the message body was modified, needs to reencode it, as show below
            ms = new MemoryStream(Encoding.UTF8.GetBytes(messageBody));

            XmlDictionaryReader reader;
            if (messageFormat == WebContentFormat.Json)
            {
                reader =// JsonReaderWriterFactory.CreateJsonReader(jsonReplyBytes, XmlDictionaryReaderQuotas.Max);
                   JsonReaderWriterFactory.CreateJsonReader(ms, XmlDictionaryReaderQuotas.Max);
            }
            else
            {
                reader = XmlDictionaryReader.CreateTextReader(ms, XmlDictionaryReaderQuotas.Max);
            }

            Message newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);
            newMessage.Properties.CopyProperties(message.Properties);
            message = newMessage;

            return messageBody;
        }

        private WebContentFormat GetMessageContentFormat(Message message)
        {
            WebContentFormat format = WebContentFormat.Default;
            if (message.Properties.ContainsKey(WebBodyFormatMessageProperty.Name))
            {
                WebBodyFormatMessageProperty bodyFormat;
                bodyFormat = (WebBodyFormatMessageProperty)message.Properties[WebBodyFormatMessageProperty.Name];
                format = bodyFormat.Format;
            }

            return format;
        }

        private string ReadRawBody(ref Message message)
        {
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
            bodyReader.ReadStartElement("Binary");
            byte[] bodyBytes = bodyReader.ReadContentAsBase64();
            string messageBody = Encoding.UTF8.GetString(bodyBytes);

            // Now to recreate the message
            MemoryStream ms = new MemoryStream();
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateBinaryWriter(ms);
            writer.WriteStartElement("Binary");
            writer.WriteBase64(bodyBytes, 0, bodyBytes.Length);
            writer.WriteEndElement();
            writer.Flush();
            ms.Position = 0;
            XmlDictionaryReader reader = XmlDictionaryReader.CreateBinaryReader(ms, XmlDictionaryReaderQuotas.Max);
            Message newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);
            newMessage.Properties.CopyProperties(message.Properties);
            message = newMessage;

            return messageBody;
        }

        public void GenerateErrorResponse(RequestContext requestContext, HttpStatusCode statusCode, string errorMessage)
        {
            Message reply = Message.CreateMessage(MessageVersion.None, null);
            HttpResponseMessageProperty responseProp = new HttpResponseMessageProperty()
            {
                StatusCode = statusCode
            };

            // Set response header
            responseProp.Headers.Add("WWW-Authenticate", String.Format("Basic realm=\"{0}\"", "ravi"));
            reply.Properties[HttpResponseMessageProperty.Name] = responseProp;
            requestContext.Reply(reply);

            // set the request context to null to terminate processing of this request
            requestContext = null;
        }

        private string MessageToString(ref Message message)
        {
            WebContentFormat messageFormat = this.GetMessageContentFormat(message);
            MemoryStream ms = new MemoryStream();
            XmlDictionaryWriter writer = null;
            switch (messageFormat)
            {
                case WebContentFormat.Default:
                case WebContentFormat.Xml:
                    writer = XmlDictionaryWriter.CreateTextWriter(ms);
                    break;
                case WebContentFormat.Json:
                    writer = JsonReaderWriterFactory.CreateJsonWriter(ms);

                    break;
                case WebContentFormat.Raw:
                    // special case for raw, easier implemented separately
                    return this.ReadRawBody(ref message);
            }

            message.WriteMessage(writer);

            writer.Flush();
            string messageBody = Encoding.UTF8.GetString(ms.ToArray());

            //Html decode message
            //messageBody = WebUtility.HtmlDecode(messageBody);

            //byte[] jsonReplyBytes = Encoding.UTF8.GetBytes(messageBody);

            // Here would be a good place to change the message body, if so desired.

            // now that the message was read, it needs to be recreated.
            ms.Position = 0;

            // if the message body was modified, needs to reencode it, as show below
            // ms = new MemoryStream(Encoding.UTF8.GetBytes(messageBody));

            XmlDictionaryReader reader;
            if (messageFormat == WebContentFormat.Json)
            {
                reader =// JsonReaderWriterFactory.CreateJsonReader(jsonReplyBytes, XmlDictionaryReaderQuotas.Max);
                   JsonReaderWriterFactory.CreateJsonReader(ms, XmlDictionaryReaderQuotas.Max);
            }
            else
            {
                reader = XmlDictionaryReader.CreateTextReader(ms, XmlDictionaryReaderQuotas.Max);
            }

            Message newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);
            newMessage.Properties.CopyProperties(message.Properties);
            message = newMessage;

            return messageBody;
        }

        public static string getBetween(string strSource, string strStart = "UserId")
        {
            int Start;
            if (strSource.Contains(strStart))// && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                //  End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start + 3, 36);
            }
            else
            {
                return "";
            }
        }
        #endregion
    }

    public static class MyExtensions
    {
        public static string EncodeHtml(this string txtBox)
        {
            return WebUtility.HtmlEncode(txtBox);
            //System.Uri.EscapeDataString(txtBox);
        }

        public static string DecodeHtml(this string txtBox)
        {
            return WebUtility.HtmlDecode(txtBox);
            //System.Uri.EscapeDataString(txtBox);
        }
    }
}
