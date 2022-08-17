using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RaspiWebApplication2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using RaspiWebApplication2.Models.Dto;

namespace RaspiWebApplication2.Controllers
{
    public class HomeController : Controller
    {
        //********mySql Server connection***********
        MySqlCommand com = new MySqlCommand();
        MySqlDataReader dr;
        MySqlConnection con = new MySqlConnection();

        //*********Sql Server**************** 
        //SqlCommand com = new SqlCommand();
        //SqlDataReader dr;
        //SqlConnection con = new SqlConnection();


        //nodeTestData Model conection 
        List<nodeTestData> nodetestdata = new List<nodeTestData>();

        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            con.ConnectionString = RaspiWebApplication2.Properties.Resources.ConnectionStringMariadbOnline;
           
        }

        public IActionResult Index()
        {
           
            return View();
        }

       
        public IActionResult FetchDataAll()
        {
            if (nodetestdata.Count > 0)
            {
                nodetestdata.Clear();

            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "select * from node_test_data";

                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    nodetestdata.Add(new nodeTestData()
                    {
                        mls_id = dr["mls_id"].ToString()
                        ,
                        insertion_id = dr["insertion_id"].ToString()
                        ,
                        address = dr["address"].ToString()
                        ,
                        city = dr["city"].ToString()
                        ,
                        zip = dr["zip"].ToString()
                        ,
                        state = dr["state"].ToString()
                        ,
                        beds = dr["beds"].ToString()
                        ,
                        baths = dr["baths"].ToString()
                        ,
                        built_on = dr["built_on"].ToString()
                        ,
                        price = dr["price"].ToString()
                        ,
                        status = dr["status"].ToString()
                        ,
                        sqrt_feet = dr["sqrt_feet"].ToString()
                        ,
                        lot_size = dr["lot_size"].ToString()
                        ,
                        source = dr["source"].ToString()
                        ,
                        latitude = dr["latitude"].ToString()
                        ,
                        longitude = dr["longitude"].ToString()
                        ,
                        neighborhood = dr["neighborhood"].ToString()
                        ,
                        date_time = dr["date_time"].ToString()

                        
                    });
                }
                con.Close();
                return new JsonResult(new { nodetestdata })
                {
                    StatusCode = StatusCodes.Status200OK // Status code here 
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult FetchDataActive()
        {
            if (nodetestdata.Count > 0)
            {
                nodetestdata.Clear();

            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "select * from node_test_data n where n.status = 'Active' ";

                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    nodetestdata.Add(new nodeTestData()
                    {
                        mls_id = dr["mls_id"].ToString()
                        ,
                        insertion_id = dr["insertion_id"].ToString()
                        ,
                        address = dr["address"].ToString()
                        ,
                        city = dr["city"].ToString()
                        ,
                        zip = dr["zip"].ToString()
                        ,
                        state = dr["state"].ToString()
                        ,
                        beds = dr["beds"].ToString()
                        ,
                        baths = dr["baths"].ToString()
                        ,
                        built_on = dr["built_on"].ToString()
                        ,
                        price = dr["price"].ToString()
                        ,
                        status = dr["status"].ToString()
                        ,
                        sqrt_feet = dr["sqrt_feet"].ToString()
                        ,
                        lot_size = dr["lot_size"].ToString()
                        ,
                        source = dr["source"].ToString()
                        ,
                        latitude = dr["latitude"].ToString()
                        ,
                        longitude = dr["longitude"].ToString()
                        ,
                        neighborhood = dr["neighborhood"].ToString()
                        ,
                        date_time = dr["date_time"].ToString()


                    });
                }
                con.Close();
               
                return new JsonResult(new { nodetestdata })
                {
                  StatusCode = StatusCodes.Status200OK // Status code here 
                };
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /*
        [HttpGet]
        public IActionResult Neighborhoods()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Neighborhoods(Hoods h)
        {
            //string hood_name, string latitude, string longitude, string city, string state, string zip, string radius
            com.Connection = con;
            com.CommandText = "CALL ADD_NEIGHBORHOOD(@Neighborhood_name, @City, @Zip, @State, @Latitude, @Longitude, @Feet_radius, @Insertion_use)";
             
            com.Parameters.Add("@Neighborhood_name", MySqlDbType.String, 25).Value = h.hood_name;
            com.Parameters.Add("@City", MySqlDbType.String, 8).Value = h.city;
            com.Parameters.Add("@Zip", MySqlDbType.String, 12).Value = h.zip;
            com.Parameters.Add("@State", MySqlDbType.String, 35).Value = h.state;
            com.Parameters.Add("@Latitude", MySqlDbType.String, 8).Value = h.latitude;
            com.Parameters.Add("@Longitude", MySqlDbType.String, 8).Value = h.longitude;
            com.Parameters.Add("@Feet_radius", MySqlDbType.String, 8).Value = h.radius;
            com.Parameters.Add("@Insertion_use", MySqlDbType.String, 8).Value = "User1";


            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Neighborhoods");
        }
        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
