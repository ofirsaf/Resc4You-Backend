using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Resc4you_Backend.Models;
using System.Net;
using System.Numerics;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservicesVolunteer
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservicesVolunteer()
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
    public int InsertPerson(Volunteer volunteer)
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

        cmd = CreateCommandWithStoredProcedureInsertPerson("spInsertPesron", con, volunteer);             // create the command

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
    // This method inserts a volunteer to the volunteer table 
    //--------------------------------------------------------------------------------------------------
    public int InsertVolunteer(Volunteer volunteer)
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

        cmd = CreateCommandWithStoredProcedureInsertVolunteer("spInsertVolunteer", con, volunteer);             // create the command

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
    // This method inserts a spoken language of volunteer to the spoken language table 
    //--------------------------------------------------------------------------------------------------
    public int InsertVolunteerLanguage(string VolunteerPhone, int VolunteerLanguageId)
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

        cmd = CreateCommandWithStoredProcedureInsertVolunteerLanguage("spInsertVolunteerLanguage", con, VolunteerPhone, VolunteerLanguageId);             // create the command

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
    // This method inserts a specialty of volunteer to the volunteer Specialty table 
    //--------------------------------------------------------------------------------------------------
    public int InsertVolunteerSpecialty(string VolunteerPhone, int VolunteerSpecialtyId)
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

        cmd = CreateCommandWithStoredProcedureInsertVolunteerSpecialty("spInsertVolunteerSpecialty", con, VolunteerPhone, VolunteerSpecialtyId);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while saving your specialties.");
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
    // This method reads specific volunteer
    //--------------------------------------------------------------------------------------------------
    public List<Volunteer> ReadVolunteer(string phone)
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


        cmd = CreateCommandWithStoredProcedureReadVolunteer("spSelectSpecificVolunteer", con, phone);             // create the command

        List<Volunteer> list = new List<Volunteer>();


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


            while (dataReader.Read())
            {
                Volunteer v = new Volunteer();
                v.Phone = dataReader["Phone"].ToString();
                v.FName = dataReader["fname"].ToString();
                v.LName = dataReader["lname"].ToString();
                v.Email = dataReader["email"].ToString();
                v.AvilabilityStatus = Convert.ToBoolean(dataReader["avilabilityStatus"]);
                v.ExpertId = Convert.ToInt32(dataReader["ExpertGroupId"]);
                list.Add(v);
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
    // This method reads expo push token of available volunteers on specific expertise
    //--------------------------------------------------------------------------------------------------
    public object GetPushTokensForChat(int expertiseId, string phone)
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


        cmd = CreateCommandWithStoredProcedureGetPushTokensForChat("spSelectPushTokenForChat", con, expertiseId,phone);             // create the command

        List<object> list = new List<object>();


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                list.Add(new
                {
                    expo_push_token = dataReader["expo_push_token"].ToString(),

                }); ;
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while retriving push tokens.");
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
    public int UpdatePerson(Volunteer volunteer)
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

        cmd = CreateCommandWithStoredProcedureUpdatePerson("spUpdatePerson", con, volunteer);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while registering the person.");
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
    // This method update a volunteer status
    //--------------------------------------------------------------------------------------------------
    public int UpdateVolunteerStatus(int min, int hours, string phone, bool avilabilityStatus, string address, double longitude, double latitude)
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

        cmd = CreateCommandWithStoredProcedureUpdateVolunteerStatus("spUpdateVolunteerStatus", con, min, hours, phone, avilabilityStatus, address, longitude, latitude);             // create the command

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
    // This method update a volunteer status
    //--------------------------------------------------------------------------------------------------
    public int UpdateVolunteerStatus(int min, int hours, string phone, bool avilabilityStatus)
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

        cmd = CreateCommandWithStoredProcedureUpdateVolunteerStatus("spUpdateVolunteerStatusWithoutLocation", con, min, hours, phone, avilabilityStatus);             // create the command

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
    // This method update a volunteer press time
    //--------------------------------------------------------------------------------------------------
    public int updateVolunteerPressTime(DateTime pressTime, string phone)
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

        cmd = CreateCommandWithStoredProcedureupdateVolunteerPressTime("spUpdateVolunteerPressTime", con, pressTime,phone);             // create the command

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


    public int ResetVolunteerStatus(int min, int hours, string phone, bool avilabilityStatus, string address, double longitude, double latitude)
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

        cmd = CreateCommandWithStoredProcedureUpdateVolunteerStatus("spUpdateResetVolunteerStatus", con, min, hours, phone, avilabilityStatus, address, longitude, latitude);             // create the command

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
    // This method update a volunteer to the volunteer table 
    //--------------------------------------------------------------------------------------------------
    public int UpdateVolunteer(Volunteer volunteer)
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

        cmd = CreateCommandWithStoredProcedureUpdateVolunteer("spUpdateVolunteer", con, volunteer);             // create the command

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
    ////--------------------------------------------------------------------------------------------------
    //// This method  return Volunteer Relevant Request
    ////--------------------------------------------------------------------------------------------------
    public object GetVolunteerRelevantRequest(string VolunteerPhone)
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


        cmd = CreateCommandWithStoredProcedureGetVolunteerRelevantRequest("spSelectRelevantRequest", con, VolunteerPhone);             // create the command

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
                    specialtyIcon = dataReader["specialtyIcon"].ToString(),
                    citizenPhone = dataReader["citizenPhone"].ToString(),
                    citizenName = dataReader["fullName"].ToString(),
                    citizenExpo_push_token= dataReader["expo_push_token"].ToString()

                }); ;
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while retriving relevant requests.");
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
    //// This method  return Volunteer Handle Request
    ////--------------------------------------------------------------------------------------------------
    public object GetVolunteerHandleRequest(string VolunteerPhone)
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


        cmd = CreateCommandWithStoredProcedureGetVolunteerHandleRequest("spSelectHandleRequest", con, VolunteerPhone);             // create the command

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
                    specialtyIcon = dataReader["specialtyIcon"].ToString(),
                    citizenPhone = dataReader["citizenPhone"].ToString(),
                    citizenName = dataReader["fullName"].ToString(),
                    citizenExpo_push_token = dataReader["expo_push_token"].ToString()



                }); ;
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while retrivng your requests.");
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
    // This method decline request and update the status of the request of a specific volunteer in the volunteer of request table 
    //--------------------------------------------------------------------------------------------------
    public int declineRequest(string VolunteerPhone, int RequestId)
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

        cmd = CreateCommandWithStoredProcedureDeclineRequest("spUpdateRequestStatusSpecificVolunteer", con, VolunteerPhone, RequestId);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred ignoring the request please try again later");
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
    // This method Associate Request To Volunteer and update request table and volunteer of request table 
    //--------------------------------------------------------------------------------------------------
    public int AssociateRequestToVolunteer(string VolunteerPhone, int RequestId)
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

        cmd = CreateCommandWithStoredProcedureAssociateRequestToVolunteer("spUpdateAssociateRequestToVolunteer", con, VolunteerPhone, RequestId);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while taking the request, please try again later.");
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
    // This method Associate Request To Volunteer and update request table and volunteer of request table 
    //--------------------------------------------------------------------------------------------------
    public int CancelRequestFromVolunteer(int RequestId)
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

        cmd = CreateCommandWithStoredProcedureCancelRequestFromVolunteer("spUpdateHandleRequest", con, RequestId);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while canceling your arrival, please try again later.");
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
    //// This method check how many requests volunteer is handling in volunteer of request table
    ////--------------------------------------------------------------------------------------------------
    public int HandelingRequestsVolunteer(string VolunteerPhone)
    {

        SqlConnection con;
        SqlCommand cmd;
        int countHandlingRequest = 0;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedureGetHandelingRequestsVolunteer("spSelectHandelingRequestsVolunteer", con, VolunteerPhone);             // create the command


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {

                countHandlingRequest = Convert.ToInt32(dataReader["NumOfHandleRequest"]);

            }

            return countHandlingRequest;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while ckecking if you are not handling another request.");
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
    // This method reads all volunteers
    //--------------------------------------------------------------------------------------------------
    public List<string> GetAllVolunteers()
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


        cmd = CreateCommandWithStoredProcedureGetAllVolunteers("spSelectVoluneers", con);             // create the command

        List<string> list = new List<string>();


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


            while (dataReader.Read())
            {
                string str;
                str = dataReader["VolunteerName"].ToString();
                list.Add(str);
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while retriving volunteers.");
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
    // This method update volunteer location
    //--------------------------------------------------------------------------------------------------
    public int UpdateLocation(string VolunteerPhone, string address, double longitude, double latitude)
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

        cmd = CreateCommandWithStoredProcedureUpdateLocation("spUpdateVolunteerLocation", con, VolunteerPhone,address,longitude,latitude);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while upadating volunteer location.");
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
    // Create the SqlCommand using a stored procedure for reading volunteer
    //--------------------------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureReadVolunteer(string spName, SqlConnection con, string phone)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@volunteerPhone", phone);


        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for insert person
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertPerson(String spName, SqlConnection con, Volunteer volunteer)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", volunteer.Phone);

        cmd.Parameters.AddWithValue("@fName", volunteer.FName);

        cmd.Parameters.AddWithValue("@lName", volunteer.LName);

        cmd.Parameters.AddWithValue("@password", volunteer.Password);

        cmd.Parameters.AddWithValue("@email", volunteer.Email);

        cmd.Parameters.AddWithValue("@personType", volunteer.PersonType);


        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for insert volunteer
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertVolunteer(String spName, SqlConnection con, Volunteer volunteer)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@volunteerPhone", volunteer.Phone);

        cmd.Parameters.AddWithValue("@avilabilityStatus", volunteer.AvilabilityStatus);

        cmd.Parameters.AddWithValue("@hoursAvailable", volunteer.HoursAvailable);

        cmd.Parameters.AddWithValue("@minsAvailable", volunteer.MinsAvailable);

        cmd.Parameters.AddWithValue("@pressTime", volunteer.PressTime);

        cmd.Parameters.AddWithValue("@ExpertGroupId", volunteer.ExpertId);

        cmd.Parameters.AddWithValue("@volunteerAddress", volunteer.VolunteerAddress);

        cmd.Parameters.AddWithValue("@volunteerLongitude", volunteer.VolunteerLongitude);

        cmd.Parameters.AddWithValue("@volunteerLatitude", volunteer.VolunteerLatitude);

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for insert volunteer Language
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertVolunteerLanguage(String spName, SqlConnection con, string VolunteerPhone, int VolunteerLanguageId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", VolunteerPhone);

        cmd.Parameters.AddWithValue("@languageId", VolunteerLanguageId);


        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureInsertVolunteerSpecialty(String spName, SqlConnection con, string VolunteerPhone, int VolunteerSpecialtyId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@volunteerPhone", VolunteerPhone);

        cmd.Parameters.AddWithValue("@specialtyId", VolunteerSpecialtyId);


        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for update person
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureUpdatePerson(String spName, SqlConnection con, Volunteer volunteer)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", volunteer.Phone);

        cmd.Parameters.AddWithValue("@fName", volunteer.FName);

        cmd.Parameters.AddWithValue("@lName", volunteer.LName);

        cmd.Parameters.AddWithValue("@password", volunteer.Password);

        cmd.Parameters.AddWithValue("@email", volunteer.Email);

        return cmd;
    }


    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for update Volunteer Status
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureUpdateVolunteerStatus(String spName, SqlConnection con, int minsAvailable, int hoursAvailable, string volunteerPhone, bool avilabilityStatus, string address, double longitude, double latitude)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@volunteerPhone", volunteerPhone);

        cmd.Parameters.AddWithValue("@minsAvailable", minsAvailable);

        cmd.Parameters.AddWithValue("@hoursAvailable", hoursAvailable);

        cmd.Parameters.AddWithValue("@avilabilityStatus", avilabilityStatus);

        cmd.Parameters.AddWithValue("@volunteerAddress", address);

        cmd.Parameters.AddWithValue("@volunteerLongitude", longitude);

        cmd.Parameters.AddWithValue("@volunteerLatitude", latitude);


        return cmd;
    }






    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for update Volunteer Status
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureUpdateVolunteerStatus(String spName, SqlConnection con, int minsAvailable, int hoursAvailable, string volunteerPhone, bool avilabilityStatus)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@volunteerPhone", volunteerPhone);

        cmd.Parameters.AddWithValue("@minsAvailable", minsAvailable);

        cmd.Parameters.AddWithValue("@hoursAvailable", hoursAvailable);

        cmd.Parameters.AddWithValue("@avilabilityStatus", avilabilityStatus);

        return cmd;
    }





    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for update volunteer
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureUpdateVolunteer(String spName, SqlConnection con, Volunteer volunteer)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@volunteerPhone", volunteer.Phone);

        cmd.Parameters.AddWithValue("@ExpertGroupId", volunteer.ExpertId);


        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for Get Volunteer Relevant Request
    //--------------------------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGetVolunteerRelevantRequest(string spName, SqlConnection con, string VolunteerPhone)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", VolunteerPhone);


        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for Get Volunteer Handle Request
    //--------------------------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGetVolunteerHandleRequest(string spName, SqlConnection con, string VolunteerPhone)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", VolunteerPhone);


        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for decline request and update the status of the request of a specific volunteer in the volunteer of request table
    //--------------------------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureDeclineRequest(string spName, SqlConnection con, string VolunteerPhone, int requestId)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", VolunteerPhone);

        cmd.Parameters.AddWithValue("@requestId", requestId);

        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for Associate Request To Volunteer and update request table and volunteer of request table 
    //--------------------------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureAssociateRequestToVolunteer(string spName, SqlConnection con, string VolunteerPhone, int requestId)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", VolunteerPhone);

        cmd.Parameters.AddWithValue("@requestId", requestId);

        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureCancelRequestFromVolunteer(string spName, SqlConnection con, int requestId)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@requestId", requestId);

        return cmd;
    }
    private SqlCommand CreateCommandWithStoredProcedureGetHandelingRequestsVolunteer(string spName, SqlConnection con, string volunteerPhone)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", volunteerPhone);

        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureupdateVolunteerPressTime(string spName, SqlConnection con, DateTime pressTime, string phone)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@pressTime", pressTime);

        cmd.Parameters.AddWithValue("@phone", phone);

        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureGetPushTokensForChat(string spName, SqlConnection con, int expertiseId, string phone)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@expertGroupId", expertiseId);

        cmd.Parameters.AddWithValue("@phone", phone);



        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureGetAllVolunteers(string spName, SqlConnection con)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureUpdateLocation(string spName, SqlConnection con, string VolunteerPhone, string address, double longitude, double latitude)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", VolunteerPhone);

        cmd.Parameters.AddWithValue("@address", address);

        cmd.Parameters.AddWithValue("@longitude", longitude);

        cmd.Parameters.AddWithValue("@latitude", latitude);

        return cmd;
    }
}
