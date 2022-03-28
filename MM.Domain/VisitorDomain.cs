using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    public class VisitorDomain
    {
        public Int64 VisiterId { get; set; }
        public string Designation_Of_Visitor { get; set; }
        public string Name_Of_Visitor_Organisation { get; set; }
        public string Upload_Photo { get; set; }
        public string LinkedInUrl { get; set; }
        public string Name_Of_Visitor { get; set; }

        public string Email  { get; set; }
        public string Phone { get; set; }
        public Guid CreatedBy { get; set; }
        public Boolean IsActive { get; set; }
        //EventId

    }
}
