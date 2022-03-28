﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JdSoft.Apple.Apns.Notifications;
using System.Configuration;

namespace APNS
{
    public class PushNotification
    {
        private string deviceToken = "";
        public string DeviceToken
        {
            get { return deviceToken; }
            set { deviceToken = value; }
        }

        private bool isSandBox = true;
        public bool IsSandBox
        {
            get { return isSandBox; }
            set { isSandBox = value; }
        }

        private IList<string> receiverList = null;
        public IList<string> ReceiverList
        {
            get { return receiverList; }
            set { receiverList = value; }
        }
        private string p12FilePath = "";
        public string P12FilePath
        {
            get { return p12FilePath; }
            set { p12FilePath = value; }
        }

        private string p12FilePassword = "";
        public string P12FilePassword
        {
            get { return p12FilePassword; }
            set { p12FilePassword = value; }
        }
        private int noOfConnections = 1;
        public int NoOfConnections
        {
            get { return noOfConnections; }
            set { noOfConnections = value; }
        }

        private int badge = 1;
        public int Badge
        {
            get { return badge; }
            set { badge = value; }
        }

        private string message = "";
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        //private enum badge;
        /*public enum Badges
        {

        }*/

        public PushNotification()
        {
        }

        public PushNotification(string deviceToken, string p12FilePath, string p12FilePassword, bool isSandBox = true)
        {
            this.deviceToken = deviceToken;
            this.isSandBox = isSandBox;
            this.p12FilePath = p12FilePath;
            this.p12FilePassword = p12FilePassword;
        }

        public bool Notify()
        {
            NotificationService notificationService = null;
            bool success = false;
            if (this.p12FilePath.Trim() != "")
            {
                try
                {
                    notificationService = new NotificationService(this.isSandBox, this.p12FilePath, this.p12FilePassword, this.noOfConnections);
                    notificationService.SendRetries = 1;
                    notificationService.ReconnectDelay = 5000;

                    notificationService.Error += new NotificationService.OnError(service_Error);
                    notificationService.NotificationTooLong += new NotificationService.OnNotificationTooLong(service_NotificationTooLong);

                    notificationService.BadDeviceToken += new NotificationService.OnBadDeviceToken(service_BadDeviceToken);
                    notificationService.NotificationFailed += new NotificationService.OnNotificationFailed(service_NotificationFailed);
                    notificationService.NotificationSuccess += new NotificationService.OnNotificationSuccess(service_NotificationSuccess);
                    notificationService.Connecting += new NotificationService.OnConnecting(service_Connecting);
                    notificationService.Connected += new NotificationService.OnConnected(service_Connected);
                    notificationService.Disconnected += new NotificationService.OnDisconnected(service_Disconnected);
                    foreach (string _deviceToken in ReceiverList)
                    {
                        Notification notification = new Notification(_deviceToken);

                        notification.Payload.Alert.Body = string.Format(message);
                        notification.Payload.Sound = "default";
                        notification.Payload.Badge = this.badge;

                        //Queue the notification to be sent
                        success = notificationService.QueueNotification(notification);
                    }
                   
                    //success = true;
                }
                catch (Exception ex)
                {
                    // throw ex;
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
                finally
                {
                    if (notificationService != null)
                    {
                        notificationService.Close();
                        //notificationService.Dispose();
                    }
                }
            }
            return success;
        }
        bool success = false;
        public bool ReturnNotify(IList<PushNotification> pushList)
        {
            NotificationService notificationService = null;
              success = false;
            if (this.p12FilePath.Trim() != "")
            {
                try
                {
                    for (int i = 0; i < pushList.Count; i++)
                    {
                        notificationService = new NotificationService(pushList[i].isSandBox, pushList[i].p12FilePath, pushList[i].p12FilePassword, pushList[i].noOfConnections);
                        notificationService.SendRetries = 1;
                        notificationService.ReconnectDelay = 5000;

                        notificationService.Error += new NotificationService.OnError(service_Error);
                        notificationService.NotificationTooLong += new NotificationService.OnNotificationTooLong(service_NotificationTooLong);

                        notificationService.BadDeviceToken += new NotificationService.OnBadDeviceToken(service_BadDeviceToken);
                        notificationService.NotificationFailed += new NotificationService.OnNotificationFailed(service_NotificationFailed);
                        notificationService.NotificationSuccess += new NotificationService.OnNotificationSuccess(service_NotificationSuccess);
                        notificationService.Connecting += new NotificationService.OnConnecting(service_Connecting);
                        notificationService.Connected += new NotificationService.OnConnected(service_Connected);
                        notificationService.Disconnected += new NotificationService.OnDisconnected(service_Disconnected);

                        Notification notification = new Notification(pushList[i].DeviceToken);

                        notification.Payload.Alert.Body = string.Format(message);
                        notification.Payload.Sound = "default";
                        notification.Payload.Badge = this.badge;

                        //Queue the notification to be sent
                        notificationService.QueueNotification(notification);
                    }
                    success = true;
                }
                catch (Exception ex)
                {
                    // throw ex;
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
                finally
                {
                    if (notificationService != null)
                    {
                        //notificationService.Close();
                        //notificationService.Dispose();
                        //notificationService = null;
                    }
                }
            }
            return success;
        }

        void service_BadDeviceToken(object sender, BadDeviceTokenException ex)
        {
            success = false;
            Console.WriteLine("Bad Device Token: {0}", ex.Message);
        }

        void service_Disconnected(object sender)
        {
            Console.WriteLine("Disconnected...");
        }

        void service_Connected(object sender)
        {
            Console.WriteLine("Connected...");
        }

        void service_Connecting(object sender)
        {
            Console.WriteLine("Connecting...");
        }

        void service_NotificationTooLong(object sender, NotificationLengthException ex)
        {
            Console.WriteLine(string.Format("Notification Too Long: {0}", ex.Notification.ToString()));
        }

        static void service_NotificationSuccess(object sender, Notification notification)
        {
            Console.WriteLine(string.Format("Notification Success: {0}", notification.ToString()));
        }

        static void service_NotificationFailed(object sender, Notification notification)
        {
            Console.WriteLine(string.Format("Notification Failed: {0}", notification.ToString()));
        }

        static void service_Error(object sender, Exception ex)
        {
            Console.WriteLine(string.Format("Error: {0}", ex.Message));
        }



    }
}
