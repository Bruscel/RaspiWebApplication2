using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using RaspiWebApplication2.Models.Dto;
using System.IO;

namespace RaspiWebApplication2.Controllers
{
    public class NeighborhoodsController : Controller
    {
        //nodeTestData Model conection 
        List<Hoods> neighborhoods = new List<Hoods>();

        //********mySql Server connection***********
        MySqlCommand com = new MySqlCommand();
        MySqlDataReader dr;
        MySqlConnection con = new MySqlConnection();

        private readonly ILogger<NeighborhoodsController> _logger;

        public NeighborhoodsController(ILogger<NeighborhoodsController> logger)
        {
            _logger = logger;
            con.ConnectionString = RaspiWebApplication2.Properties.Resources.ConnectionStringMariadbOnline;

        }
        [HttpGet]
        public IActionResult getNeighborhoods()
        {
            return View();
        }

        [HttpPost]
        public IActionResult getNeighborhoods(Hoods h)
        {
            //string hood_name, string latitude, string longitude, string city, string state, string zip, string radius
            com.Connection = con;
            com.CommandText = "CALL ADD_NEIGHBORHOOD(@Neighborhood_name, @City, @Zip, @State, @Latitude, @Longitude, @Feet_radius, @Insertion_use)";

            com.Parameters.Add("@Neighborhood_name", MySqlDbType.String, 25).Value = h.neighborhood_name;
            com.Parameters.Add("@City", MySqlDbType.String, 8).Value = h.city;
            com.Parameters.Add("@Zip", MySqlDbType.String, 12).Value = h.zip;
            com.Parameters.Add("@State", MySqlDbType.String, 35).Value = h.state;
            com.Parameters.Add("@Latitude", MySqlDbType.String, 8).Value = h.latitude;
            com.Parameters.Add("@Longitude", MySqlDbType.String, 8).Value = h.longitude;
            com.Parameters.Add("@Feet_radius", MySqlDbType.String, 8).Value = h.feet_radius;
            com.Parameters.Add("@Insertion_use", MySqlDbType.String, 8).Value = "User1";


            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("getNeighborhoods");
        }


        public void fileLoader(object sender, EventArgs e)
        {
            

        }
        public IActionResult FetchData()
        {
            if (neighborhoods.Count > 0)
            {
                neighborhoods.Clear();

            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "select * from neighborhoods";

                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    neighborhoods.Add(new Hoods()
                    {
                        neighborhood_name = dr["neighborhood_name"].ToString()
                        ,
                        latitude = dr["latitude"].ToString()
                        ,
                        longitude = dr["longitude"].ToString()
                        ,
                        city = dr["city"].ToString()
                        ,
                        state = dr["state"].ToString()
                        ,
                        zip = dr["zip"].ToString()
                        ,
                        feet_radius = dr["feet_radius"].ToString()
                    });
                }
                con.Close();

                return new JsonResult(new { neighborhoods })
                {
                    StatusCode = StatusCodes.Status200OK // Status code here 
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
        public void Save(string hood_name, string latitude, string longitude, string city, string state, string zip, string radius)
        {

            com.Connection = con;
            com.CommandText = "CALL ADD_NEIGHBORHOOD(@Neighborhood_name, @City, @Zip, @State, @Latitude, @Longitude, @Feet_radius, @Insertion_use)";



            com.Parameters.Add("@Neighborhood_name", MySqlDbType.String, 25).Value = hood_name;
            com.Parameters.Add("@City", MySqlDbType.String, 8).Value = city;
            com.Parameters.Add("@Zip", MySqlDbType.String, 12).Value = zip;
            com.Parameters.Add("@State", MySqlDbType.String, 35).Value = state;
            com.Parameters.Add("@Latitude", MySqlDbType.String, 8).Value = latitude;
            com.Parameters.Add("@Longitude", MySqlDbType.String, 8).Value = longitude;
            com.Parameters.Add("@Feet_radius", MySqlDbType.String, 8).Value = radius;
            com.Parameters.Add("@Insertion_use", MySqlDbType.String, 8).Value = "User1";


            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }
        */
    }
}
