using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Resc4you_Backend.Models;
using System.Numerics;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservicesCitizen
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservicesCitizen()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    //--------------------------------------------------------------------------------------------------
    // This method inserts a person to the person table 
    //--------------------------------------------------------------------------------------------------
    public int InsertPerson(Citizen citizen)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureInsertPerson("spInsertPesron", con, citizen);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627) // Check for unique constraint violation error code
            {
                // Handle the unique constraint violation by showing a friendly message to the user
                throw new Exception("A person with the same Phone already exists.");
            }
            else
            {
                // Handle other types of SQL exceptions
                throw new Exception("An error occurred while registering the person.");
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method inserts a citizen to the citizen table 
    //--------------------------------------------------------------------------------------------------
    public int InsertCitizen(Citizen citizen)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureInsertCitizen("spInsertCitizen", con, citizen);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method inserts a spoken language of citizen to the spoken language table 
    //--------------------------------------------------------------------------------------------------
    public int InsertCitizenLanguage(string CitizenPhone, int CitizenLanguageId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureInsertCitizenLanguage("spInsertCitizenLanguage", con, CitizenPhone, CitizenLanguageId);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while saving your languages.");
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
   

    //--------------------------------------------------------------------------------------------------
    // This method update a person to the person table 
    //--------------------------------------------------------------------------------------------------
    public int UpdatePerson(Citizen citizen)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureUpdatePerson("spUpdatePerson", con, citizen);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while update citizen details.");
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    ////--------------------------------------------------------------------------------------------------
    //// This method  return citizen requests
    ////--------------------------------------------------------------------------------------------------
    public object getCitizenReqests(string phone)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedureGetCitizenRequests("spSelectCitizenRequests", con, phone);             // create the command

        List<object> list = new List<object>();

        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                list.Add(new
                {
                    RequestAddress = dataReader["requestAddress"].ToString(),
                    RequestStatus = dataReader["requestStatus"].ToString(),
                    specialtyName = dataReader["specialtyName"].ToString(),
                    requestDate = Convert.ToDateTime(dataReader["requestDate"]),
                    requestId = Convert.ToInt32(dataReader["requestId"]),
                    specialtyIcon= dataReader["specialtyIcon"].ToString(),
                    isRevewed= Convert.ToInt32(dataReader["isReviewed"]),
                    volunteerHandle = dataReader["VolunterHandle"].ToString()

                }); ;
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred retrieving your requests.");
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    ////--------------------------------------------------------------------------------------------------
    //// This method  return citizen open request
    ////--------------------------------------------------------------------------------------------------
    public object GetCitizenOpenRequest(string phone)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedureGetCitizenOpenRequests("spSelectCitizenOpenRequest", con, phone);             // create the command


        List<object> list = new List<object>();

        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                list.Add(new
                {
                    requestId = Convert.ToInt32(dataReader["requestId"]),
                    RequestStatus = dataReader["requestStatus"].ToString(),

                }); ;
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while retriving opened requests.");
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    ////--------------------------------------------------------------------------------------------------
    //// This method  return citizen request details
    ////--------------------------------------------------------------------------------------------------
    public object GetDetailsOfReportedRequest(string phone)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedureGetDetailsOfReportedRequest("spSelectReportedRequestDetails", con, phone);             // create the command

        List<object> list = new List<object>();

        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                list.Add(new
                {
                    requestDate = Convert.ToDateTime(dataReader["requestDate"]),
                    NumOfRelevantVolunteer = Convert.ToInt32(dataReader["NumOfRelevantVolunteer"]),
                }); ;
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while loading your request details.");
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    ////--------------------------------------------------------------------------------------------------
    //// This method delete a request from the request table and from volunteer of request table
    ////--------------------------------------------------------------------------------------------------
    public int DeleteRequest(int requestId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureDeleteCitizenRequest("spDeleteCitizenRequest", con, requestId);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while canceling your request, please try again later.");
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method reads specific citizen
    //--------------------------------------------------------------------------------------------------
    public List<Citizen> ReadCitizen(string phone)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedureReadCitizen("spSelectSpecificCitizen", con, phone);             // create the command

        List<Citizen> list = new List<Citizen>();


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


            while (dataReader.Read())
            {
                Citizen c = new Citizen();
                c.Phone = dataReader["Phone"].ToString();
                c.FName = dataReader["fname"].ToString();
                c.LName = dataReader["lname"].ToString();
                c.Email = dataReader["email"].ToString();
                list.Add(c);
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method reads the number of open request of citizen
    //--------------------------------------------------------------------------------------------------
    public object GetNumberCitizenOpenRequest(string phone)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedureNumberOpenRequest("spSelectOpenRequest", con, phone);             // create the command
        List<object> list = new List<object>();

        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                list.Add(new
                {
                    NumberOfOpened = Convert.ToInt32(dataReader["NumberOfOpened"]),
                });
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while checking number of opened requests.");
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for insert person
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertPerson(String spName, SqlConnection con, Citizen citizen)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", citizen.Phone);

        cmd.Parameters.AddWithValue("@fName", citizen.FName);

        cmd.Parameters.AddWithValue("@lName", citizen.LName);

        cmd.Parameters.AddWithValue("@password", citizen.Password);

        cmd.Parameters.AddWithValue("@email", citizen.Email);

        cmd.Parameters.AddWithValue("@personType", citizen.PersonType);


        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for insert citizen
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertCitizen(String spName, SqlConnection con, Citizen citizen)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", citizen.Phone);

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for insert citizen Language
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertCitizenLanguage(String spName, SqlConnection con, string CitizenPhone, int CitizenLanguageId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", CitizenPhone);

        cmd.Parameters.AddWithValue("@languageId", CitizenLanguageId);


        return cmd;
    }


    

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for update person
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureUpdatePerson(String spName, SqlConnection con, Citizen citizen)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", citizen.Phone);

        cmd.Parameters.AddWithValue("@fName", citizen.FName);

        cmd.Parameters.AddWithValue("@lName", citizen.LName);

        cmd.Parameters.AddWithValue("@password", citizen.Password);

        cmd.Parameters.AddWithValue("@email", citizen.Email);

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for get citizen requests
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGetCitizenRequests(String spName, SqlConnection con, string phone)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", phone);

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for get citizen open request
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGetCitizenOpenRequests(String spName, SqlConnection con, string phone)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", phone);

        return cmd;
    }

    ////--------------------------------------------------------------------------------------------------
    //// Create the SqlCommand using a stored procedure for delete citizen request
    ////--------------------------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureDeleteCitizenRequest(string spName, SqlConnection con,int requestId)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@requestId", requestId);
        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for reading volunteer
    //--------------------------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureReadCitizen(string spName, SqlConnection con, string phone)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@Phone", phone);

        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureNumberOpenRequest(String spName, SqlConnection con, string phone)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", phone);

        return cmd;
    }


    private SqlCommand CreateCommandWithStoredProcedureGetDetailsOfReportedRequest(String spName, SqlConnection con, string phone)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", phone);

        return cmd;
    }

}
