using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace VLTP.Services
{
    public class OracleService : IOracleService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OracleService> _logger;

        public OracleService(ILogger<OracleService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void InsertRow(string sessionIndex, string maxEmail)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            try
            {
                ///////////////////////////////////////////////////////////////////////////////////////
                // Write sessionId and email address that were passed into this method to the log.
                ///////////////////////////////////////////////////////////////////////////////////////
                Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => sessionId: '"
                                        + sessionIndex + "'");
                Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][InsertRow] => email....: '"
                                        + maxEmail + "'");

                ///////////////////////////////////////////////////////////////////////////////////////
                // Get Oracle Connection String	
                ///////////////////////////////////////////////////////////////////////////////////////
                string connString = _configuration.GetSection("ConnectionStrings")["OracleConnection"];
                // Console.WriteLine("[" +  DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => BEGIN: connString:");
                // Console.WriteLine(connString);
                // Console.WriteLine("[" +  DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => END: connString:");

                ///////////////////////////////////////////////////////////////////////////////////////
                // Instantiate Oracle Connection
                ///////////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => Instantiating OracleConnection ...");
                conn = new OracleConnection(connString);
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => OracleConnection.State: '" + conn.State + "'");

                ///////////////////////////////////////////////////////////////////////////////////////
                // Open Oracle Connection
                ///////////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => Opening OracleConnection ...");
                conn.Open();
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => OracleConnection.State '" + conn.State + "'");
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => OracleConnection.ConnectionTimeout '" + conn.ConnectionTimeout + "'");
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => OracleConnection.ConnectionString '" + conn.ConnectionString + "'");
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => OracleConnection.DatabaseName '" + conn.DatabaseName + "'");
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => OracleConnection.DataSource '" + conn.DataSource + "'");

                ///////////////////////////////////////////////////////////////////////////////////////
                // Create Oracle SQL Parameters
                ///////////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => Creating OracleParameter(s) for sessionId and emailAddress...");
                OracleParameter sessionIdParam = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = sessionIndex
                };

                OracleParameter emailAddressParam = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = maxEmail
                };

                ///////////////////////////////////////////////////////////////////////////////////////
                // Create Oracle Command
                ///////////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => Invoking OracleConnection.CreateCommand()...");
                cmd = conn.CreateCommand();

                ///////////////////////////////////////////////////////////////////////////////////////
                // Set Oracle SQL Parameters
                ///////////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => Adding OracleParameter(s) to OracleCommand...");
                cmd.Parameters.Add(sessionIdParam);
                cmd.Parameters.Add(emailAddressParam);

                ///////////////////////////////////////////////////////////////////////////////////////
                // **************************INSERT INTO VLTP_AUTH***********************************//
                ///////////////////////////////////////////////////////////////////////////////////////
                // Assign INSERT INTO VLTP_AUTH SQL to OracleCommand
                ///////////////////////////////////////////////////////////////////////////////////////
                cmd.CommandText = "INSERT INTO VLTA.VLTP_AUTH(SESSION_ID, EMAIL_ADDRESS) VALUES(:1, :2)";
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => SQL: '" + cmd.CommandText + "'");
                ///////////////////////////////////////////////////////////////////////////////////////
                // Invoke OracleCommand.ExecuteNonQuery() FOR INSERT INTO VLTP_AUTH SQL
                ///////////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")
                //                      + "][OracleService][InsertIntoVltpAuth] => Calling cmd.ExecuteNonQuery() FOR INSERT INTO VLTP_AUTH ...");
                int execResult = cmd.ExecuteNonQuery();
                ///////////////////////////////////////////////////////////////////////////////////////
                // Write # of rows inserted to the log FOR INSERT INTO VLTP_AUTH SQL
                ///////////////////////////////////////////////////////////////////////////////////////      
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")
                //                      + "][OracleService][InsertIntoVltpAuth] => cmd.ExecuteNonQuery() # of rows inserted FOR INSERT INTO VLTP_AUTH SQL: '"
                //                      + execResult + "'");

                ///////////////////////////////////////////////////////////////////////////////////////
                // **************************INSERT INTO VLTP_AUTH_HIST******************************//              
                ///////////////////////////////////////////////////////////////////////////////////////
                // Assign INSERT INTO VLTP_AUTH_HIST SQL to OracleCommand
                ///////////////////////////////////////////////////////////////////////////////////////
                cmd.CommandText = "INSERT INTO VLTA.VLTP_AUTH_HIST(SESSION_ID, EMAIL_ADDRESS) VALUES(:1, :2)";
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => SQL: '" + cmd.CommandText + "'");
                ///////////////////////////////////////////////////////////////////////////////////////
                // Invoke OracleCommand.ExecuteNonQuery() FOR INSERT INTO VLTP_AUTH_HIST SQL
                ///////////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")
                //                      + "][OracleService][InsertIntoVltpAuth] => Calling cmd.ExecuteNonQuery() FOR INSERT INTO VLTP_AUTH_HIST ...");
                int execResultHist = cmd.ExecuteNonQuery();
                ///////////////////////////////////////////////////////////////////////////////////////
                // Write # of rows inserted to the log FOR INSERT INTO VLTP_AUTH_HIST SQL
                ///////////////////////////////////////////////////////////////////////////////////////      
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")
                //                   + "][OracleService][InsertIntoVltpAuth] => cmd.ExecuteNonQuery() # of rows inserted FOR INSERT INTO VLTP_AUTH_HIST SQL: '"
                //                   + execResultHist + "'");

                ///////////////////////////////////////////////////////////////////////////////////////
                // Dispose of OracleCommand
                ///////////////////////////////////////////////////////////////////////////////////////   
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => Invoking OracleCommand.Dispose() ...");
                cmd.Dispose();

                ///////////////////////////////////////////////////////////////////////////////////////
                // Close OracleConnection
                /////////////////////////////////////////////////////////////////////////////////////// 
                //Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => Invoking OracleConnection.Close() ...");
                conn.Close();
            }
            catch (Exception ex)
            {
                ///////////////////////////////////////////////////////////////////////////////////////
                // Write Exception Details to the Log
                ///////////////////////////////////////////////////////////////////////////////////////
                Console.WriteLine("********************************************************************************");
                Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => BEGIN: Exception Message:");
                Console.WriteLine(ex.Message);
                Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => END: Exception Message:");
                Console.WriteLine("********************************************************************************");
                Console.WriteLine("");
                Console.WriteLine("********************************************************************************");
                Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => BEGIN: Exception StackTrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][OracleService][InsertRow] => END: Exception StackTrace:");
                Console.WriteLine("********************************************************************************");

                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}