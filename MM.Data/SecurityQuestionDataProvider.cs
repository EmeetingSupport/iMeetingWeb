using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Domain;
using System.Data;
using MM.Core;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using MySql.Data.MySqlClient;

namespace MM.Data
{
    public class SecurityQuestionDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile SecurityQuestionDataProvider _instance;
        private static object _synRoot = new Object();

        private SecurityQuestionDataProvider()
        {
        }

        public static SecurityQuestionDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SecurityQuestionDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// Fill Security Questions
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private SecurityQuestionDomain ToSecurityQuestionDomain(IDataReader dr)
        {
            return new SecurityQuestionDomain
            {
                SecurityQuestionId = Null.SetNullGuid(dr["SecurityQuestionId"]),
                SecurityQuestion = Null.SetNullString(dr["SecurityQuestion"])
            };
        }

        /// <summary>
        /// Get List of Security Question
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<SecurityQuestionDomain> ToSecurityQuestionDomains(IDataReader dr)
        {
            IList<SecurityQuestionDomain> objSecurityQuestionDomain = new List<SecurityQuestionDomain>();
            while (dr.Read())
            {
                objSecurityQuestionDomain.Add(ToSecurityQuestionDomain(dr));
            }
            return objSecurityQuestionDomain;
        }

        #endregion

        #region "Get"
        /// <summary>
        /// Get list of Sequrity Questions 
        /// </summary>
        /// <returns></returns>
        public IList<SecurityQuestionDomain> Get()
        {
            IList<SecurityQuestionDomain> objSecurityQuestionDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllSecurityQuestion", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //  using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllSecurityQuestion"))
            {
                objSecurityQuestionDomain = ToSecurityQuestionDomains(dataReader);
            }
            conn.Close();
            return objSecurityQuestionDomain;

        }


        ///// <summary>
        ///// Get User SecurityQuestion With Anser 
        ///// </summary>
        ///// <param name="UserId">Guid specifying UserId</param>
        ///// <returns></returns>
        //public SecurityQuestionDomain GetSecurityQuestion(Guid UserId)
        //{
        //    //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetSecurityQuestionByUserId", UserId);
        //    string constr = ConnectionString;
        //    MySqlConnection conn = new MySqlConnection(constr);
        //    MySqlCommand cmd = new MySqlCommand("stp_GetSecurityQuestionByUserId", conn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("p_UserId", UserId);
        //    conn.Open();
        //    MySqlDataReader dataReader = cmd.ExecuteReader();

        //    //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetSecurityQuestionByUserId", UserId))


        //    if (!dataReader.Read())
        //        return null;

        //    conn.Close();
        //    return new SecurityQuestionDomain()
        //    {
        //        SecurityQuestionId = Null.SetNullGuid(dataReader["SecurityQuestionId"]),
        //        SecurityQuestion = Null.SetNullString(dataReader["SecurityQuestion"]),
        //        Answer = Encryptor.DecryptString(Null.SetNullString(dataReader["Answer"])),
        //        UserId = Null.SetNullGuid(dataReader["UserId"])

        //};

        //}
        #endregion


        /// <summary>
        /// Get User SecurityQuestion With Anser 
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public SecurityQuestionDomain GetSecurityQuestion(Guid UserId)
        {
            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetSecurityQuestionByUserId", UserId);
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetSecurityQuestionByUserId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetSecurityQuestionByUserId", UserId))


            if (!dataReader.Read())
            {
                conn.Close();
                return null;
            }

            SecurityQuestionDomain objSec = new SecurityQuestionDomain()
            {
                SecurityQuestionId = Null.SetNullGuid(dataReader["SecurityQuestionId"]),
                SecurityQuestion = Null.SetNullString(dataReader["SecurityQuestion"]),
                Answer = Encryptor.DecryptString(Null.SetNullString(dataReader["Answer"])),
                UserId = Null.SetNullGuid(dataReader["UserId"])

            };
            conn.Close();
            return objSec;
        }
        #region "Modify"
        /// <summary>
        /// insert or update security qusetion
        /// </summary>
        /// <param name="Answer">string sepcifing Answer</param>
        /// <param name="QuestionId">Guid specifying QuestionId</param>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public bool InsertSecurityQuestion(string Answer, Guid QuestionId, Guid UserId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertSecurityQuestion", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_QuestionId", QuestionId);
            cmd.Parameters.AddWithValue("p_Answer", Encryptor.EncryptString(Answer));
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //  return Convert.ToInt32((SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertSecurityQuestion", QuestionId,Encryptor.EncryptString(Answer), UserId))) > 0;
        }
        #endregion
    }
}
