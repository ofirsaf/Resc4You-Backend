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
public class DBservicesLanguage
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservicesLanguage()
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
    // This method reads all the languages
    //--------------------------------------------------------------------------------------------------
    public List<Language> ReadLanguage()
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


        cmd = CreateCommandWithStoredProcedureReadSelectLanguage("spSelectLanguage", con);             // create the command


        List<Language> list = new List<Language>();

        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {

                Language l = new Language();
                l.LanguageId = Convert.ToInt32(dataReader["languageId"]);
                l.LanguageName = dataReader["languageName"].ToString();
                list.Add(l);
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while retrivng languages.");
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
    // This method reads all the person languages
    //--------------------------------------------------------------------------------------------------
    public List<Language> ReadPersonLanguage(string personPhone)
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


        cmd = CreateCommandWithStoredProcedureReadSelectPersonLanguage("spSelectPersonLanguage", con, personPhone);             // create the command


        List<Language> list = new List<Language>();
            
        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {

                Language l = new Language();
                l.LanguageId = Convert.ToInt32(dataReader["languageId"]);
                l.LanguageName = dataReader["languageName"].ToString();
                list.Add(l);
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while retrivng your languages.");
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
    //// This method update a user to the user table 
    ////--------------------------------------------------------------------------------------------------
    //public int Update(User user)
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

    //    cmd = CreateCommandWithStoredProcedure("spUpdateUser", con, user);             // create the command

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
    // This method delete the languages of volunteer from the spoken language table 
    //--------------------------------------------------------------------------------------------------
    public int DeletePersonLanguge(string personPhone)
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

        cmd = CreateCommandWithStoredProcedureDeletePersonLanguage("spDeletePersonLanguage", con, personPhone);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while updating your languages.");
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
    // Create the SqlCommand using a stored procedure for select language
    //--------------------------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureReadSelectLanguage(string spName, SqlConnection con)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for select person language
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureReadSelectPersonLanguage(String spName, SqlConnection con, string personPhone)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@Phone", personPhone);

        return cmd;
    }

    private SqlCommand CreateCommandWithStoredProcedureDeletePersonLanguage(String spName, SqlConnection con, string personPhone)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@Phone", personPhone);

        return cmd;
    }
}
