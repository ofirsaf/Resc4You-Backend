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
public class DBservicesManufacture
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservicesManufacture()
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
    // This method reads all the Manufacturer
    //--------------------------------------------------------------------------------------------------
    public List<Manufacturer> ReadManufacturer()
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


        cmd = CreateCommandWithStoredProcedureReadManufacturer("spSelectManufacturer", con);             // create the command


        List<Manufacturer> list = new List<Manufacturer>();

        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {

                Manufacturer m = new Manufacturer();
                m.ManufacturerId = Convert.ToInt32(dataReader["manufacturerId"]);
                m.ManufacturerName = dataReader["manufacturerName"].ToString();
                list.Add(m);
            }

            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while retriving Manufacturer list.");
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
    // This method inserts a manufacturer to the manufacturer table 
    //--------------------------------------------------------------------------------------------------
    public int insertNewManufacturer(Manufacturer manufacturer)
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

        cmd = CreateCommandWithStoredProcedureInsertNewManufacturer("spInsertManufacturer", con, manufacturer);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw new Exception("An error occurred while adding new manufacturer, please try again later.");
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
    // Create the SqlCommand using a stored procedure for read Manufacturer
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureReadManufacturer(String spName, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for insert new Manufacturer
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertNewManufacturer(String spName, SqlConnection con,Manufacturer manufacturer)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

        cmd.Parameters.AddWithValue("@manufacturerName", manufacturer.ManufacturerName);


        return cmd;
    }
}
