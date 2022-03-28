using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Core;
using MM.Data;
using MM.Domain;

namespace MM.Services
{
  public class SecurityQuestionService
    {
      /// <summary>
      /// Get All Security Question
      /// </summary>
      /// <returns></returns>
      public IList<SecurityQuestionDomain> GetAllQuestion()
      {
          return SecurityQuestionDataProvider.Instance.Get();
      }

      /// <summary>
      /// Get Security question with answer
      /// </summary>
      /// <param name="UserId">Guid specifying UserId</param>
      /// <returns></returns>
      public SecurityQuestionDomain GetSecurityQuestion(Guid UserId)
      {
          return SecurityQuestionDataProvider.Instance.GetSecurityQuestion(UserId);
      }

      /// <summary>
      /// Insert Security Question
      /// </summary>
      /// <param name="Answer">string specifying Answer</param>
      /// <param name="QuestionId">Guid specifying QuestionId</param>
      /// <param name="UserId">Guid specifying UserId</param>
      /// <returns></returns>
      public bool InsertSecurityQuestion(string Answer, Guid QuestionId, Guid UserId)
      {
          return SecurityQuestionDataProvider.Instance.InsertSecurityQuestion(Answer,QuestionId,UserId);
      }
    }
}
