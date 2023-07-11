using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Resc4you_Backend.Models;
using System.Numerics;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservicesRequest
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservicesRequest()
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
    // This method inserts a request to the request table 
    //--------------------------------------------------------------------------------------------------
    public int InsertRequest(Request request)
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

        cmd = CreateCommandWithStoredProcedureInsertRequest("spInsertRequest", con, request);             // create the command

        try
        {
            int id = Convert.ToInt32(cmd.ExecuteScalar()); // execute the command
            return id;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while opening the request, please try again later.");
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
    // This method inserts a request to the VolunteerOfrequest table 
    //--------------------------------------------------------------------------------------------------
    public int InsertToVolunteerOfRequest(string phone,int requestId)
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

        cmd = CreateCommandWithStoredProcedureInsertRequest("spInsertIntoVolunteerOfRequest", con, phone, requestId);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while post request to volunteer of request table.");
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
    // This method Get the Relevant Volunteer for the request
    //--------------------------------------------------------------------------------------------------
    public object GetRelevantVolunteer(int specialtyId)
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


        cmd = CreateCommandWithStoredProcedureRelevantVolunteer("spSelectRelevantVolunteer", con, specialtyId);             // create the command

        List<object> list = new List<object>();


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                list.Add(new
                {
                    volunteerPhone = dataReader["volunteerPhone"].ToString(),
                    volunteerAddress = dataReader["volunteerAddress"].ToString(),
                    volunteerLongitude = Convert.ToDouble(dataReader["volunteerLongitude"]),
                    volunteerLatitude = Convert.ToDouble(dataReader["volunteerLatitude"]),
                    avilabilityStatus = Convert.ToBoolean(dataReader["avilabilityStatus"]),
                    expo_push_token= dataReader["expo_push_token"].ToString(),

                }); ;
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while retriving relevant volunteers.");
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
    // This method Get the Relevant Volunteer for the request
    //--------------------------------------------------------------------------------------------------
    public object GetAllRequests()
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


        cmd = CreateCommandWithStoredProcedureGetAllRequests("spSelectRequestsForAdmin", con);             // create the command

        List<object> list = new List<object>();


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                list.Add(new
                {
                    RequestAddress = dataReader["requestAddress"].ToString(),
                    RequestType = dataReader["specialtyName"].ToString(),
                    RequestId = Convert.ToInt32(dataReader["requestId"]),
                    RequestStatus= dataReader["requestStatus"].ToString(),
                    VolunteerName=dataReader["volunteerName"].ToString(),
                    RequestDate=Convert.ToDateTime(dataReader["requestDate"]),
                    Citizen= dataReader["Citizen"].ToString(),
                    TakenRequestDate = dataReader["takenRequestDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["takenRequestDate"]),
                    CloseRequestDate = dataReader["closeRequestDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["closeRequestDate"]),
                    RequestRating = dataReader["requestRating"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataReader["requestRating"]),

                }) ;
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while retriving the data.");
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
    // This method Get the Relevant Volunteer for the request
    //--------------------------------------------------------------------------------------------------
    public object GetStatusOfSpecificRequest(int requestId)
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


        cmd = CreateCommandWithStoredProcedureSpecificRequest("spSelectStatusOfSpecificRequest", con, requestId);             // create the command

        List<object> list = new List<object>();


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                list.Add(new
                {
                    requestStatus = dataReader["requestStatus"].ToString(),
                    expo_push_token = dataReader["expo_push_token"].ToString(),
                }); ;
            }

            return list[0];
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

    public object GetRelevantRequestsToVolunteer(string VolunteerPhone)
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
            throw new Exception("An error occurred while retriving relevant requests.");
        }


        cmd = CreateCommandWithStoredProcedureGetRelvantRequestsToVolunteer("spSelectRelevantRequestsToVolunteer", con, VolunteerPhone);             // create the command

        List<object> list = new List<object>();


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                list.Add(new
                {
                    requestLongitude = Convert.ToDouble(dataReader["requestLongitude"]),
                    requestLatitude= Convert.ToDouble(dataReader["requestLatitude"]),
                    volunteerLongitude = Convert.ToDouble(dataReader["volunteerLongitude"]),
                    volunteerLatitude = Convert.ToDouble(dataReader["volunteerLatitude"]),
                    requestId= Convert.ToInt32(dataReader["requestId"])

            }); ;
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


    ////--------------------------------------------------------------------------------------------------
    //// This method reads the ush token o a volunteer handke request
    ////--------------------------------------------------------------------------------------------------
    public string GetVolunteerHandlePushTokken(int requestId)
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


        cmd = CreateCommandWithStoredProcedureGetVolunteerHandlePushTokken("spSelectVolunteerPushToken", con, requestId);             // create the command


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            Person p = new Person();

            while (dataReader.Read())
            {

                p.Expo_push_token = dataReader["expo_push_token"].ToString();
            }

            return p.Expo_push_token;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while getting volunteer push token.");
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
    //// This method check if volunteer already took the request
    ////--------------------------------------------------------------------------------------------------
    public int RequestAlreadyTaken(int requestId)
    {

        SqlConnection con;
        SqlCommand cmd;
        int num=0;


        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedureRequestAlreadyTaken("spSelectAlreadyTaken", con, requestId);             // create the command


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {

                num = Convert.ToInt32(dataReader["alreadyTaken"]);
            }

            return num;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while checking if the request handled by another volunteer.");
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
    ///method that Update Request Status To Close
    public int UpdateRequestStatusToClose(int requestId, string phone, string address, double longi, double lati)
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

        cmd = CreateCommandWithStoredProcedureUpdateRequestStatusToClose("spUpdateRequestStatusToClosed", con, requestId,phone,address,longi,lati);             // create the command
 
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while closing the request, please try again later.");
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
    ///method that update the rate and review when request closed
    public int UpdateRateOfRequest(int requestId, string review, int rating)
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

        cmd = CreateCommandWithStoredProcedureUpdateRequestRating("spUpdateRequestRate",con, requestId, review, rating);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while sending your review, please try again later.");
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
    //// This method Get Request And Volunteer Locations
    ////--------------------------------------------------------------------------------------------------
    public object GetRequestAndVolunteerLocation(int requestId, string phone)
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


        cmd = CreateCommandWithStoredProcedureGetRequestAndVolunteerLocation("spSelectRequestAndVolunteerLocation", con,requestId,phone);             // create the command

        List<object> list = new List<object>();


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                list.Add(new
                {
                    requestLongitude = Convert.ToDouble(dataReader["requestLongitude"]),
                    requestLatitude = Convert.ToDouble(dataReader["requestLatitude"]),
                    volunteerLongitude = Convert.ToDouble(dataReader["volunteerLongitude"]),
                    volunteerLatitude = Convert.ToDouble(dataReader["volunteerLatitude"]),
                }); ;
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while locating volunteer, please try again later.");
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
    //// This method Get Details Of Citizen Opened Request for updating request in admin page
    ////--------------------------------------------------------------------------------------------------
    public object GetDetailsOfCitizenOpenedRequest(int requestId)
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


        cmd = CreateCommandWithStoredProcedureGetDetailsOfCitizenOpenedRequest("spSelectDetailsOfCitizenOpenedRequest", con, requestId);             // create the command

        List<object> list = new List<object>();


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                list.Add(new
                {
                    fname= dataReader["fName"].ToString(),
                    lname= dataReader["lName"].ToString(),
                    email= dataReader["email"].ToString(),
                    phone= dataReader["Phone"].ToString(),
                    address= dataReader["requestAddress"].ToString(),
                    manufacturer= dataReader["manufacturerName"].ToString(),
                    licenseNum= dataReader["licenseNum"].ToString(),
                    specialty= dataReader["specialtyName"].ToString(),
                }); ;
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while retruvung request details.");
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
    // Create the SqlCommand using a stored procedure for insert request
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertRequest(String spName, SqlConnection con, Request request)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@requestAddress", request.RequestAddress);

        cmd.Parameters.AddWithValue("@requestLongitude", request.RequestLongitude);

        cmd.Parameters.AddWithValue("@requestLatitude", request.RequestLatitude);

        cmd.Parameters.AddWithValue("@licenseNum", request.LicenseNum);

        cmd.Parameters.AddWithValue("@requestStatus", request.RequestStatus);

        cmd.Parameters.AddWithValue("@requestDate", request.RequestDate);
        
        cmd.Parameters.AddWithValue("@requestSummary", request.RequestSummary);
        
        cmd.Parameters.AddWithValue("@citizenPhone", request.CitizenPhone);
        
        cmd.Parameters.AddWithValue("@workerPhone", request.WorkerPhone);
        
        cmd.Parameters.AddWithValue("@manufacturerId", request.ManufacturerId);
        
        cmd.Parameters.AddWithValue("@specialtyId", request.SpecialtyId);

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for select the relevant volunteers for the request
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureRelevantVolunteer(String spName, SqlConnection con, int specialtyId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@specialtyId", specialtyId);

        return cmd;
    }


    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for select all requests
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGetAllRequests(String spName, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure


        return cmd;
    }











    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for select the status of specific request
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureSpecificRequest(String spName, SqlConnection con, int requestId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@requestId", requestId);

        return cmd;
    }





    private SqlCommand CreateCommandWithStoredProcedureGetRelvantRequestsToVolunteer(String spName, SqlConnection con, string VolunteerPhone)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@VolunteerPhone", VolunteerPhone);

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for insert request to a Volunteer of request table
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertRequest(String spName, SqlConnection con, string phone, int requestId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@VolunteerPhone", phone);

        cmd.Parameters.AddWithValue("@requestId", requestId);

        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureGetVolunteerHandlePushTokken(String spName, SqlConnection con, int requestId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@requestId", requestId);

        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureRequestAlreadyTaken(String spName, SqlConnection con, int requestId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@requestId", requestId);

        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureUpdateRequestRating(string spName, SqlConnection con, int requestId, string review, int rating)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@requestid", requestId);

        cmd.Parameters.AddWithValue("@review", review);

        cmd.Parameters.AddWithValue("@rating", rating);

        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureGetRequestAndVolunteerLocation(string spName, SqlConnection con, int requestId, string phone)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@requestid", requestId);

        cmd.Parameters.AddWithValue("@phone", phone);

        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureUpdateRequestStatusToClose(string spName, SqlConnection con, int requestId,string phone, string address, double longi, double lati)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@requestid", requestId);

        cmd.Parameters.AddWithValue("@phone", phone);

        cmd.Parameters.AddWithValue("@volunteerAddress", address);

        cmd.Parameters.AddWithValue("@volunteerLongitude", longi);

        cmd.Parameters.AddWithValue("@volunteerLatitude", lati);



        return cmd;
    }
    

        private SqlCommand CreateCommandWithStoredProcedureGetDetailsOfCitizenOpenedRequest(string spName, SqlConnection con, int requestId)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@requestId", requestId);

        return cmd;
    }
}
