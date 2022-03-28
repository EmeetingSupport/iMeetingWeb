using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ScheduleEvent
/// </summary>
public class ScheduleEvent
{
    public int id { get; set; }
    public string nameOfVisitor { get; set; }
    public string designtionOfVisitor { get; set; }
    public string nameOfVisitorOrganisation { get; set; }
    public string appointment { get; set; }
    public string nameDesignation { get; set; }
    public string uploadPhoto { get; set; }
    public string attachments { get; set; }
    public string information { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public string MOM { get; set; }
}