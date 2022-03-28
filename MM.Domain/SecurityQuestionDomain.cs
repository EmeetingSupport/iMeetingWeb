using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    #region "Property of SecurityQuestionEntity"
    /// <summary>
    /// Property of SecurityQuestionEntity table
    /// </summary>
    public  class SecurityQuestionDomain
    {
        public Guid SecurityQuestionId
        {
            get;
            set;
        }

        public string SecurityQuestion
        {
            get;
            set;
        }


        public Guid UserId
        {
            get;
            set;
        }

        public string Answer
        {
            get;
            set;
        }

    }
    #endregion
}
