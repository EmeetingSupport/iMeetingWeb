using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MM.Services;

namespace MeetingMInder.WCFServices.Request
{
    public class MessageLoggerDb
    {

        public System.Data.DataSet AddMessageLog(Logger objLog)
        {
            UserServices objService = new UserServices();
            return objService.InsertDeviceLog(objLog.UdId, objLog.UserId, objLog.Mac, objLog.DeviceTime, objLog.OperationName, objLog.EntityId, objLog.Request, objLog.Response, objLog.Mapper, objLog.AppVersion, objLog.RequestToken, objLog.IsExpired, objLog.LoginUserId, objLog.UserIpAddress, objLog.UserName);
        }

        public void UpdateLog(Guid Mapper, string Response, string AccessToken, string IsExpired)
        {
            UserServices objService = new UserServices();
            objService.UpdateDeviceLog(Response, Mapper, AccessToken, IsExpired);
        }
    }

    public class Logger
    {
        public Guid UserId { get; set; }
        public string DeviceTime { get; set; }
        public string Mac { get; set; }
        public string UdId { get; set; }
        public Guid EntityId { get; set; }
        public string OperationName { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public Guid Mapper { get; set; }

        public string AppVersion { get; set; }

        public string IsExpired { get; set; }

        public string RequestToken { get; set; }

        public Guid LoginUserId { get; set; }

        public string UserIpAddress { get; set; }
        public string UserName { get; set; }
    }
}
