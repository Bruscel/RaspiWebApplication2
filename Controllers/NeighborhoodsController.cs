using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using RaspiWebApplication2.Models.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
            com.Parameters.Clear();
            con.Close();

            return RedirectToAction("getNeighborhoods");
        }

        [HttpPost]
        public IActionResult fileLoaderAsync(IFormFile file, Hoods h)
        {

            if (file != null)
            {
                com.Connection = con;
                com.CommandText = "CALL ADD_NEIGHBORHOOD(@Neighborhood_name, @City, @Zip, @State, @Latitude, @Longitude, @Feet_radius, @Insertion_use)";
                List<Hoods> ResultsList = new List<Hoods>();
                string[] fileContents = { " " };
                using (var stream = file.OpenReadStream())
                using (var reader = new StreamReader(stream))
                {
                    fileContents = reader.ReadToEnd().Split(new[] { "\r\n" }, StringSplitOptions.None);
                }

                foreach (var x in fileContents)
                {
                    var read = x.Split(",");
                    if (x.Length > 0)
                    {
                        Hoods Result = new Hoods();
                        Result.neighborhood_name = read[0];
                        Result.latitude = read[1];
                        Result.longitude = read[2];
                        Result.city = read[3];
                        Result.state = read[4];
                        Result.zip = read[5];
                        Result.feet_radius = read[6];
                        ResultsList.Add(Result);
                    }
                }
                con.Open();
                foreach (var i in ResultsList)
                {
                    com.Parameters.Add("@Neighborhood_name", MySqlDbType.String, 25).Value = i.neighborhood_name;
                    com.Parameters.Add("@City", MySqlDbType.String, 8).Value = i.city;
                    com.Parameters.Add("@Zip", MySqlDbType.String, 12).Value = i.zip;
                    com.Parameters.Add("@State", MySqlDbType.String, 35).Value = i.state;
                    com.Parameters.Add("@Latitude", MySqlDbType.String, 8).Value = i.latitude;
                    com.Parameters.Add("@Longitude", MySqlDbType.String, 8).Value = i.longitude;
                    com.Parameters.Add("@Feet_radius", MySqlDbType.String, 8).Value = i.feet_radius;
                    com.Parameters.Add("@Insertion_use", MySqlDbType.String, 8).Value = "User1";

                    com.ExecuteNonQuery();
                    com.Parameters.Clear();
                }
                con.Close();
            }
            else
            {

            }
            //return BadRequest(fileContents);
            return RedirectToAction("getNeighborhoods");
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
