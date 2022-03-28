using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MM.Core;
using MM.Data;
using MM.Domain;

namespace MM.Services
{
  public class CheckerServices
    {
      /// <summary>
      /// get All Unapproved Meeting
      /// </summary>
      /// <returns></returns>
      public IList<CheckerDomain> GetAllUnApprovedMeeting()
      {
          return CheckerDataProvider.Instance.GetAllUnApprovedMeeting();
      }
    }
}
