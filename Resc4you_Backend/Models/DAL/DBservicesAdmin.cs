using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Resc4you_Backend.Models;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservicesAdmin
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservicesAdmin()
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


    ////--------------------------------------------------------------------------------------------------
    //// This method check if user already exist
    ////--------------------------------------------------------------------------------------------------
    public bool GetUser(string phone)
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


        cmd = CreateCommandWithStoredProcedureGetUser("spSelectUserDetails", con, phone);             // create the command

        List<string> list = new List<string>();


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (dataReader.HasRows == true)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while checking if user exists.");
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
    //// This method get the name and push token of volunteer handle request for cancel from web
    ////--------------------------------------------------------------------------------------------------
    public object SendCancelNotificationsForVolunteerFromWeb(int requestid)
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


        cmd = CreateCommandWithStoredProcedureSendCancelNotificationsForVolunteerFromWeb("spSelectVolunteerHandleRequestDetails", con, requestid);             // create the command

        List<object> list = new List<object>();


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
                list.Add(new
                {
                    fName = dataReader["fName"].ToString(),
                    lName = dataReader["lName"].ToString(),
                    expo_push_token = dataReader["expo_push_token"].ToString(),
                }); ;
            }

            return list[0];

        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while getting the data of volunteer to send him push notification.");
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
    //// This method get volunteer data for volunteers page admin web
    ////--------------------------------------------------------------------------------------------------
    public object GetVolunteerData()
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


        cmd = CreateCommandWithStoredProcedureGetVolunteerData("spSelectVolunteerDataForWeb", con);             // create the command

        List<object> list = new List<object>();


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
                list.Add(new
                {
                    phone = dataReader["volunteerPhone"].ToString(),
                    fName = dataReader["fName"].ToString(),
                    lName = dataReader["lName"].ToString(),
                    email = dataReader["email"].ToString(),
                    avilablity = dataReader["availabilityStatus"].ToString(),
                    expert = dataReader["ExpertGroupName"].ToString(),
                    specialties = dataReader["specialties"].ToString(),
                    isActive= Convert.ToBoolean(dataReader["isActive"])

                }); ;
            }

            return list;

        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while getting the data of volunteers.");
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
    // This method update Specialty is Active
    //--------------------------------------------------------------------------------------------------
    public int UpdateSpecialtyActive(int specialtyId, bool isActive)
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

        cmd = CreateCommandWithStoredProcedureUpdateSpecialtyActive("spUpdateSpecialtyActive", con, specialtyId,isActive);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while updating specialty activity status");
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
    // This method update worker is Active
    //--------------------------------------------------------------------------------------------------
    public int UpdateWorkerActive(string phone, bool isActive)
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

        cmd = CreateCommandWithStoredProcedureUpdateWorkerActive("spUpdateWorkerActive", con, phone, isActive);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while updating worker activity status");
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
    // Create the SqlCommand using a stored procedure check if user already exist
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGetUser(String spName, SqlConnection con, string phone)
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
    // Create the SqlCommand using a stored procedure get the name and push token of volunteer handle request for cancel from web
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureSendCancelNotificationsForVolunteerFromWeb(String spName, SqlConnection con, int requestid)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@requestid", requestid);

        return cmd;
    }


    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure get volunteer data for volunteers page admin web
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGetVolunteerData(String spName, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure update Specialty is Active
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureUpdateSpecialtyActive(String spName, SqlConnection con, int specialtyId, bool isActive)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@specaltyId", specialtyId);

        cmd.Parameters.AddWithValue("@specialtyIsActive", isActive);

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure update worker is Active
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureUpdateWorkerActive(String spName, SqlConnection con, string phone, bool isActive)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", phone);

        cmd.Parameters.AddWithValue("@workerIsActive", isActive);

        return cmd;
    }
}
