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
public class DBservicesSpecialty
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservicesSpecialty()
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
    //// This method inserts a user to the user table 
    ////--------------------------------------------------------------------------------------------------
    //public int Insert(User user)
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    cmd = CreateCommandWithStoredProcedure("spInsertUser", con, user);             // create the command

    //    try
    //    {
    //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
    //        return numEffected;
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}


    //--------------------------------------------------------------------------------------------------
    // This method reads all the specialties
    //--------------------------------------------------------------------------------------------------
    public List<Specialty> ReadSpecialty()
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


        cmd = CreateCommandWithStoredProcedureReadSpecialty("spSelectSpecialty", con);             // create the command


        List<Specialty> list = new List<Specialty>();

        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {

                Specialty s = new Specialty();
                s.SpecialtyId = Convert.ToInt32(dataReader["specialtyId"]);
                s.SpecialtyName = dataReader["specialtyName"].ToString();
                s.SpecialtyIcon= dataReader["specialtyIcon"].ToString();
                s.IsActive = Convert.ToBoolean(dataReader["isActive"]);
                list.Add(s);
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while retrivng specialties.");
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
    // This method reads all the volunteer specialty
    //--------------------------------------------------------------------------------------------------
    public List<Specialty> ReadVolunteerSpecialty(string volunteerPhone)
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


        cmd = CreateCommandWithStoredProcedureReadSelectVolunteerSpecialty("spSelectVolunteerSpecialty", con, volunteerPhone);             // create the command


        List<Specialty> list = new List<Specialty>();

        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {

                Specialty s = new Specialty();
                s.SpecialtyId = Convert.ToInt32(dataReader["specialtyId"]);
                s.SpecialtyName = dataReader["specialtyName"].ToString();
                list.Add(s);
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while retrivng volunterr specialties.");
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
    // This method inserts a specialty to the specialty table 
    //--------------------------------------------------------------------------------------------------
    public int insertNewSpecialty(Specialty specialty)
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

        cmd = CreateCommandWithStoredProcedureinsertNewSpecialty("spInsertSpecialty", con, specialty);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while adding new specialty, please try again later.");
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
    // This method delete the specialties of volunteer from the volunteer specialty table 
    //--------------------------------------------------------------------------------------------------
    public int DeletePersonSpecialty(string volunteerPhone)
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

        cmd = CreateCommandWithStoredProcedureDeleteVolunteerSpecialty("spDeleteVolunteerSpecialty", con, volunteerPhone);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while updating your specialties.");
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
    // Create the SqlCommand using a stored procedure for read specialty
    //--------------------------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureReadSpecialty(string spName, SqlConnection con)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for read volunteer specialty
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureReadSelectVolunteerSpecialty(String spName, SqlConnection con, string volunteerPhone)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@volunteerPhone", volunteerPhone);


        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureDeleteVolunteerSpecialty(String spName, SqlConnection con, string volunteerPhone)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@phone", volunteerPhone);


        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureinsertNewSpecialty(String spName, SqlConnection con, Specialty specialty)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@specialtyName", specialty.SpecialtyName);

        return cmd;
    }
}
