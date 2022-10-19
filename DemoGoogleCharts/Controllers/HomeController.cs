using DemoGoogleCharts.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DemoGoogleCharts.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]        
        public IActionResult GetDataChart()
        {
            string query = "SELECT ShipCity, COUNT(orderid) TotalOrders FROM Orders WHERE ShipCountry = 'USA' GROUP BY ShipCity";

            string constr = @"Data Source=.\sqlexpress;Initial Catalog=northwind;integrated security=true";
            
            List<object> chartData = new List<object>();
            
            chartData.Add(new object[]
                            {
                            "ShipCity", "TotalOrders"
                            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[]
                            {
                            sdr["ShipCity"], sdr["TotalOrders"]
                            });
                        }
                    }
                    con.Close();
                }
            }

            return Ok(chartData);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}