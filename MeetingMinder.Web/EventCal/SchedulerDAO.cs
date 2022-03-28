using System;
using System.Collections.Generic;
using System.Linq;

using System.Data;
using System.Data.SqlClient;

using System.Configuration;

/// <summary>
/// Summary description for SchedulerDAO
/// </summary>
public class SchedulerDAO
{


    private static string objConnection = MM.Core.Config.GetConnectionString("RelianceConnectionString");
        //ConfigurationManager.ConnectionStrings["RelianceConnectionString"].ConnectionString.ToString();

    //private static SqlConnection objConnection = null;
    private static SqlCommand cmd = null;
    public static List<ScheduleEvent> getEvents(DateTime start, DateTime end)
    {

        List<ScheduleEvent> events = new List<ScheduleEvent>();

        SqlConnection connection = new SqlConnection(objConnection);


        cmd = new SqlCommand("sp_GetSchedulerEvent", connection);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Event_Start", start);
        cmd.Parameters.AddWithValue("@Event_End", end);
        cmd.Parameters.AddWithValue("@UserId", System.Web.HttpContext.Current.Session["UserId"]);
        using (connection)
        {
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ScheduleEvent sevent = new ScheduleEvent();
                sevent.id = int.Parse(reader["Event_Id"].ToString());
                sevent.nameOfVisitor = (string)reader["Name_Of_Visitor"];
                sevent.designtionOfVisitor = (reader["Designation_Of_Visitor"] == DBNull.Value) ? string.Empty : (string)reader["Designation_Of_Visitor"];
                   
                sevent.nameOfVisitorOrganisation = (string)reader["Name_Of_Visitor_Organisation"];
                sevent.appointment = (string)reader["Appointment"];
                sevent.nameDesignation = (string)reader["Name_Designation"];
                sevent.uploadPhoto = (string)reader["Upload_Photo"];
                sevent.attachments = (string)reader["Attachment"];
                sevent.information = (string)reader["Information"];
                sevent.start = (DateTime)reader["Event_Start"];
                sevent.end = (DateTime)reader["Event_End"];
                sevent.MOM = (reader["MOM"] == DBNull.Value) ? string.Empty : (string)reader["MOM"];
                    //(string)reader["MOM"];
                events.Add(sevent);
            }
        }
        return events;
        //side note: if you want to show events only related to particular users,
        //if user id of that user is stored in session as Session["userid"]
        //the event table also contains a extra field named 'user_id' to mark the event for that particular user
        //then you can modify the SQL as:
        //SELECT event_id, description, title, event_start, event_end FROM event where user_id=@user_id AND event_start>=@start AND event_end<=@end
        //then add paramter as:cmd.Parameters.AddWithValue("@user_id", HttpContext.Current.Session["userid"]);
    }



    //this method updates the event title and description
    public static void updateEvent(int id, String nameOfVisitor, String designtionOfVisitor,String nameOfVisitorOrganisation,String appointment,String nameDesignation,String uploadPhoto,String attachments,String information, string start, string end, string MOM)
    {
        SqlConnection connection = new SqlConnection(objConnection);

        SqlCommand cmd = new SqlCommand("sp_updateSchedulerEvent", connection);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Name_Of_Visitor", nameOfVisitor);
        cmd.Parameters.AddWithValue("@Designation_Of_Visitor", designtionOfVisitor);
        cmd.Parameters.AddWithValue("@Name_Of_Visitor_Organisation", nameOfVisitorOrganisation);
        cmd.Parameters.AddWithValue("@Appointment", appointment);
        cmd.Parameters.AddWithValue("@Name_Designation", nameDesignation);
        cmd.Parameters.AddWithValue("@Upload_Photo", uploadPhoto);
        cmd.Parameters.AddWithValue("@Attachment", attachments);
        cmd.Parameters.AddWithValue("@Information", information);
        cmd.Parameters.AddWithValue("@event_id", id);
        cmd.Parameters.AddWithValue("@Event_Start", start);
        cmd.Parameters.AddWithValue("@Event_End", end);
        cmd.Parameters.AddWithValue("@MOM", MOM);
        using (connection)
        {
            connection.Open();
            cmd.ExecuteNonQuery();
        }


    }

    public static void updateEventTime(int id, DateTime start, DateTime end)
    {
        SqlConnection connection = new SqlConnection(objConnection);
        SqlCommand cmd = new SqlCommand("sp_updateScheduleTimeEvent", connection);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Event_Start", start);
        cmd.Parameters.AddWithValue("@Event_End", end);
        cmd.Parameters.AddWithValue("@Event_Id", id);
        using (connection)
        {
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }


    public static void deleteEvent(int id)
    {
        SqlConnection connection = new SqlConnection(objConnection);
        SqlCommand cmd = new SqlCommand("sp_deleteScheduleEvent", connection);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@event_id", id);
        using (connection)
        {
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }

 

    public static int addEvent(ScheduleEvent sevent)
    {
        //add event to the database and return the primary key of the added event row

        //insert
        SqlConnection connection = new SqlConnection(objConnection);
        SqlCommand cmd = new SqlCommand("sp_insertScheduleEvent", connection);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Name_Of_Visitor", sevent.nameOfVisitor);
        cmd.Parameters.AddWithValue("@Designation_Of_Visitor", sevent.designtionOfVisitor);
        cmd.Parameters.AddWithValue("@Name_Of_Visitor_Organisation", sevent.nameOfVisitorOrganisation);
        cmd.Parameters.AddWithValue("@Appointment", sevent.appointment);
        cmd.Parameters.AddWithValue("@Name_Designation", sevent.nameDesignation);
        cmd.Parameters.AddWithValue("@Upload_Photo", sevent.uploadPhoto); //sevent.uploadPhoto
        cmd.Parameters.AddWithValue("@Attachment", sevent.attachments); // sevent.attachments
        cmd.Parameters.AddWithValue("@Information", sevent.information);
        cmd.Parameters.AddWithValue("@Event_Start", sevent.start);
        cmd.Parameters.AddWithValue("@Event_End", sevent.end);
        cmd.Parameters.AddWithValue("@CreatedBy", Guid.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString()));
        cmd.Parameters.AddWithValue("@MOM", sevent.MOM);
       int key = 0;
        using (connection)
        {
            connection.Open();
            //cmd.ExecuteNonQuery();

            ////get primary key of inserted row
            //cmd = new SqlCommand("sp_GetMaxinsertScheduleEvent", connection);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@Name_Of_Visitor", sevent.nameOfVisitor);
            //cmd.Parameters.AddWithValue("@Designation_Of_Visitor",sevent.designtionOfVisitor);
            //cmd.Parameters.AddWithValue("@Name_Of_Visitor_Organisation", sevent.nameOfVisitorOrganisation);
            //cmd.Parameters.AddWithValue("@Appointment", sevent.appointment);
            //cmd.Parameters.AddWithValue("@Name_Designation", sevent.nameDesignation);
            //cmd.Parameters.AddWithValue("@Upload_Photo", sevent.uploadPhoto);
            //cmd.Parameters.AddWithValue("@Attachment", sevent.attachments);
            //cmd.Parameters.AddWithValue("@Information",sevent.information);
            //cmd.Parameters.AddWithValue("@Event_Start",sevent.start);
            //cmd.Parameters.AddWithValue("@Event_End", sevent.end);

                
            key = (int)cmd.ExecuteScalar();
        }

        return key;

    }
        
}