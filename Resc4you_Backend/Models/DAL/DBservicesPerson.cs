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
public class DBservicesPerson
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservicesPerson()
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
    //// This method  return object of person
    ////--------------------------------------------------------------------------------------------------
    public Person loginUser(string phone)
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


        cmd = CreateCommandWithStoredProcedureLoginUser("spSelcetLoginUser", con, phone);             // create the command

        Person p = new Person();


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {

                p.Phone = dataReader["phone"].ToString();
                p.FName = dataReader["fName"].ToString();
                p.LName = dataReader["lName"].ToString();
                p.Password = dataReader["password"].ToString();
                p.Email = dataReader["email"].ToString();
                p.PersonType = dataReader["personType"].ToString();
                p.Expo_push_token = dataReader["expo_push_token"].ToString();
                p.IsActive = Convert.ToBoolean(dataReader["isActive"]);


            }

            return p;
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

    ////--------------------------------------------------------------------------------------------------
    //// This method  return object with User Details
    ////--------------------------------------------------------------------------------------------------
    public object GetUserDetails(string phone)
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


        cmd = CreateCommandWithStoredProcedureGetUserDetails("spSelectUserDetails", con, phone);             // create the command

        List<object> list = new List<object>();


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                list.Add(new
                {
                    Email = dataReader["Email"].ToString(),
                    FName = dataReader["FName"].ToString(),
                    LName = dataReader["LName"].ToString(),
                    PersonType= dataReader["PersonType"].ToString(),
                    Phone = dataReader["Phone"].ToString(),
                    expo_push_token= dataReader["expo_push_token"].ToString(),
                    IsActive= Convert.ToBoolean(dataReader["isActive"])
            }); ;
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while loading your data please log in again.");
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
    //// This method update a User Push Notification Token in person table
    ////--------------------------------------------------------------------------------------------------
    public int UpdateUserPushNotificationToken(string phone, string pushNotificationToken)
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

        cmd = CreateCommandWithStoredProcedureUpdateUserPushNotificationToken("spUpdateUserPushNotificationToken", con, phone,pushNotificationToken);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while updating user push token.");
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
    //// This method update is active status of volunteer
    ////--------------------------------------------------------------------------------------------------
    public int UpdateIsActive(string phone, bool isActive)
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

        cmd = CreateCommandWithStoredProcedureUpdateIsActive("spUpdateIsActive", con, phone, isActive);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while updating volunteer activity status.");
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
    // Create the SqlCommand using a stored procedure for login citizen
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureLoginUser(String spName, SqlConnection con, string phone)
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
    // Create the SqlCommand using a stored procedure for UpdateUserPushNotificationToken
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureUpdateUserPushNotificationToken(String spName, SqlConnection con, string phone, string pushNotificationToken)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", phone);

        cmd.Parameters.AddWithValue("@expo_push_token", pushNotificationToken);

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for GetUserDetails
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGetUserDetails(String spName, SqlConnection con, string phone)
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
    // Create the SqlCommand using a stored procedure for update is active status of volunteer
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureUpdateIsActive(String spName, SqlConnection con, string phone, bool isActive)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", phone);

        cmd.Parameters.AddWithValue("@isActive", isActive);

        return cmd;
    }
}
