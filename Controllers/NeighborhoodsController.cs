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

namespace RaspiWebApplication2.Controllers
{
    public class NeighborhoodsController : Controller
    {
        //********mySql Server connection***********
        MySqlCommand com = new MySqlCommand();
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
            return RedirectToAction("getNeighborhoods");
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
