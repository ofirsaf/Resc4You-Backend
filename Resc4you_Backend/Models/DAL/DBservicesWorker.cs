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
public class DBservicesWorker
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservicesWorker()
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
    public int InsertPerson(Worker worker)
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

        cmd = CreateCommandWithStoredProcedureInsertPerson("spInsertPesron", con, worker);             // create the command

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
    // This method inserts a worker to the worker table 
    //--------------------------------------------------------------------------------------------------
    public int InsertWorker(Worker worker)
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

        cmd = CreateCommandWithStoredProcedureInsertWorker("spInsertWorker", con, worker);             // create the command

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
    //// This method  return object of person
    ////--------------------------------------------------------------------------------------------------
    public Worker loginWorker(string phone)
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


        cmd = CreateCommandWithStoredProcedureLoginWorker("spSelcetLoginWorker", con, phone);             // create the command

        Worker w = new Worker();


        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {

                w.Phone = dataReader["phone"].ToString();
                w.FName = dataReader["fName"].ToString();
                w.LName = dataReader["lName"].ToString();
                w.Password = dataReader["password"].ToString();
                w.Email = dataReader["email"].ToString();
                w.WorkerType = dataReader["workerType"].ToString();
                w.IsActive= Convert.ToBoolean(dataReader["isActive"]);

            }

            return w;
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
    //// This method  return workers details
    ////--------------------------------------------------------------------------------------------------
    public object GetWorkersDetails()
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

        List<object> list = new List<object>();
        cmd = CreateCommandWithStoredProcedureGetWorkersDetails("spSelectWorkers", con);             // create the command

        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                list.Add(new
                {
                    phone = dataReader["phone"].ToString(),
                    fName = dataReader["fName"].ToString(),
                    lName = dataReader["lName"].ToString(),
                    email = dataReader["email"].ToString(),
                    isActive= Convert.ToBoolean(dataReader["isActive"]),

            });
        }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while retriving worker details.");
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
    public int UpdatePerson(Worker worker)
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

        cmd = CreateCommandWithStoredProcedureUpdatePerson("spUpdatePerson", con, worker);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while update worker details.");
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
    // Create the SqlCommand using a stored procedure for login worker
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureLoginWorker(String spName, SqlConnection con, string phone)
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
    // Create the SqlCommand using a stored procedure for insert person
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertPerson(String spName, SqlConnection con, Worker worker)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", worker.Phone);

        cmd.Parameters.AddWithValue("@fName", worker.FName);

        cmd.Parameters.AddWithValue("@lName", worker.LName);

        cmd.Parameters.AddWithValue("@password", worker.Password);

        cmd.Parameters.AddWithValue("@email", worker.Email);

        cmd.Parameters.AddWithValue("@personType", worker.PersonType);

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for insert worker
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertWorker(String spName, SqlConnection con, Worker worker)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", worker.Phone);

        cmd.Parameters.AddWithValue("@workerType", worker.WorkerType);


        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for get worker details
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGetWorkersDetails(String spName, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for update person
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureUpdatePerson(String spName, SqlConnection con, Worker worker)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", worker.Phone);

        cmd.Parameters.AddWithValue("@fName", worker.FName);

        cmd.Parameters.AddWithValue("@lName", worker.LName);

        cmd.Parameters.AddWithValue("@password", worker.Password);

        cmd.Parameters.AddWithValue("@email", worker.Email);

        return cmd;
    }
}
