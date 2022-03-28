using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Data
{
    public class RevisedPdfDomain
    {
        public int RevisedId { get; set; }
        public Guid AgendaId { get; set; }
        public string Pdf_Old { get; set; }
        public string Pdf_New { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreateOn { get; set; }
        public string Pdf_NewName { get; set; }

        public string AgendaOld { get; set; }
        public string AgendaNew { get; set; }
        public string PdfOldName { get; set; }
        public Guid MeetingId { get; set; }
    }
}
