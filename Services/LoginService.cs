using System;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using VLTP.Models;

namespace VLTP.Services
{
    public class LoginService: ILoginService
    {
        private readonly IConfiguration _configuration;

        public LoginService(IConfiguration config)
        {
            _configuration = config;
        }

        public LoginInfo RetreiveLoginInfo()
        {
            string logSnippet = "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][LoginService][RetreiveLoginInfo] => ";

            OracleConnection conn = null;
            OracleCommand cmd = null;

            try
            {
                ///////////////////////////////////////////////////////////////////////////////////////
                // Get Oracle Connection String	
                ///////////////////////////////////////////////////////////////////////////////////////
                string connString = _configuration.GetSection("ConnectionStrings")["OracleConnection"];
                //Console.WriteLine(logSnippet + $"Oracle Connection String: '{connString}'");

                ///////////////////////////////////////////////////////////////////////////////////////
                // Instantiate Oracle Connection
                ///////////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine(logSnippet + "Instantiating OracleConnection ...");
                conn = new OracleConnection(connString);
                //Console.WriteLine(logSnippet + "OracleConnection.State: '" + conn.State + "'");
                
                ///////////////////////////////////////////////////////////////////////////////////////
                // Open Oracle Connection
                ///////////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine(logSnippet + "Opening OracleConnection ...");
                conn.Open();
                //Console.WriteLine(logSnippet + "OracleConnection.State: '" + conn.State + "'");
                //Console.WriteLine(logSnippet + "OracleConnection.ConnectionTimeout: '" + conn.ConnectionTimeout + "'");

                ///////////////////////////////////////////////////////////////////////////////////////
                // Create Oracle Command
                ///////////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine(logSnippet + "Invoking OracleConnection.CreateCommand()...");
                cmd = conn.CreateCommand();
                //Console.WriteLine(logSnippet + $"(OracleCommand == null): {cmd == null}");

                ///////////////////////////////////////////////////////////////////////////////////////
                // Set Oracle Command Text for VLTP (app_num = '002' AND lock_type = '00001')
                ///////////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine(logSnippet + "Setting OracleCommand.CommandText for VLTP (app_num = '002' AND lock_type = '00001')");
                cmd.CommandText = "SELECT active_ind, message FROM eex_mgr.sys_app_control WHERE app_num = '002' AND lock_type = '00001' AND active_ind = '1'";
                //Console.WriteLine(logSnippet + $"cmd.CommandText: \"{cmd.CommandText}\"");

                ///////////////////////////////////////////////////////////////////////////////////////
                // OracleCommand.ExecuteReader() for VLTP (app_num = '002' AND lock_type = '00001')
                ///////////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine(logSnippet + "Invoking OracleCommand.ExecuteReader() for VLTP (app_num = '002' AND lock_type = '00001')");
                OracleDataReader reader = cmd.ExecuteReader();
 
                //Console.WriteLine(logSnippet + $"OracleDataReader.HasRows(): '{reader.HasRows}'");  
                if (reader.HasRows)
                {
                    bool readSuccess = reader.Read();

                    //Console.WriteLine(logSnippet + $"OracleDataReader.Read(): '{readSuccess}'"); 
                    if (readSuccess)
                    {
                        var activeIndicator = reader.GetString(0);
                        var message = reader.GetString(1);

                        //Console.WriteLine( logSnippet + $"OracleCommand.CommandText: \"{cmd.CommandText}\"");
                        //Console.WriteLine( logSnippet + $"(activeIndicator): '{activeIndicator}'");
                        //Console.WriteLine( logSnippet + $"(message): {message}");
 
                        if ( activeIndicator.Equals("1"))
                        {
                            return new LoginInfo(false, message);
                        }
                    }
                }   

                ///////////////////////////////////////////////////////////////////////////////////////
                //// Set Oracle Command Text for PAR (app_num = '001' AND lock_type = '00001')
                ///////////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine(logSnippet + "Setting OracleCommand.CommandText for PAR (app_num = '001' AND lock_type = '00001')");
                cmd.CommandText = "SELECT active_ind, message FROM eex_mgr.sys_app_control WHERE app_num = '001' AND lock_type = '00001' AND active_ind = '1'";
                //Console.WriteLine(logSnippet + $"cmd.CommandText: \"{cmd.CommandText}\"");

                ///////////////////////////////////////////////////////////////////////////////////////
                // OracleCommand.ExecuteReader() for PAR (app_num = '001' AND lock_type = '00001')
                ///////////////////////////////////////////////////////////////////////////////////////
                //Console.WriteLine(logSnippet + "Invoking OracleCommand.ExecuteReader() for PAR (app_num = '001' AND lock_type = '00001')");
                reader = cmd.ExecuteReader();
 
                //Console.WriteLine(logSnippet + $"OracleDataReader.HasRows(): '{reader.HasRows}'");  
                if (reader.HasRows)
                {
                    bool readSuccess = reader.Read();

                    //Console.WriteLine(logSnippet + $"OracleDataReader.Read(): '{readSuccess}'"); 
                    if (readSuccess)
                    {
                        var activeIndicator = reader.GetString(0);
                        var message = reader.GetString(1);

                        //Console.WriteLine( logSnippet + $"OracleCommand.CommandText: \"{cmd.CommandText}\"");
                        //Console.WriteLine( logSnippet + $"(activeIndicator): '{activeIndicator}'");
                        //Console.WriteLine( logSnippet + $"(message): {message}");
 
                        if ( activeIndicator.Equals("1"))
                        {
                            return new LoginInfo(false, message);
                        }
                    }
                } 

                return new LoginInfo(true, null);

            }
            catch(Exception exc)
            {
                Console.WriteLine("");
                Console.WriteLine( logSnippet + "BEGIN: Exception Message:");
                Console.WriteLine(exc.Message);
                Console.WriteLine( logSnippet + "END: Exception Message:");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine( logSnippet + "BEGIN: Exception StackTrace:");
                Console.WriteLine(exc.StackTrace);
                Console.WriteLine( logSnippet + "END: Exception StackTrace:");
                Console.WriteLine("");

                return new LoginInfo(false, "Something went wrong.");
            }
            finally
            {
                if ( cmd != null ) 
                {
                    cmd.Dispose();
                }
                if (conn != null ) 
                {
                    conn.Close();
                }                
            }
        }
    }
}