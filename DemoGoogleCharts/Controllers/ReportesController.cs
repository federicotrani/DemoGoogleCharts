using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace DemoGoogleCharts.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IConfiguration _config;
        public ReportesController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ReporteEjemplo()
        {
            return View();
        }

        public IActionResult SexoAnimalesEstablecimiento()
        {
            return View();
        }

        public IActionResult DesinfeccionOmbligoEstablecimiento()
        {
            return View();
        }

        [HttpGet(Name = "GetDataReporteEjemplo")]
        public IActionResult GetDataReporteEjemplo()
        {
            string query = "select sexo, count(idanimal) as num from Animales where Activo=1 and IdEstablecimiento=50 group by Sexo ";

            List<object> chartData = new List<object>();

            chartData.Add(new object[]
                            {
                            "ShipCity", "TotalOrders"
                            });
            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("GuacherasDb")))
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
                            sdr["sexo"], sdr["num"]
                            });
                        }
                    }
                    con.Close();
                }
            }

            return Ok(chartData);
        }

        [HttpGet(Name = "GetDataDesinfeccionOmbligoEstablecimiento")]
        public IActionResult GetDataDesinfeccionOmbligoEstablecimiento()
        {
            string query = "(select 'desinfectados' as categoria, count(idanimal) as num from animales where DesinfeccionOmbligo=1 and Activo=1 and IdEstablecimiento=50) UNION (select 'sin desinfectar' as categoria, count(idanimal) as num from animales where DesinfeccionOmbligo=0 and Activo=1 and IdEstablecimiento=50)";

            List<object> chartData = new List<object>();

            chartData.Add(new object[]
                            {
                            "categoria", "num"
                            });
            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("GuacherasDb")))
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
                            sdr["categoria"], sdr["num"]
                            });
                        }
                    }
                    con.Close();
                }
            }

            return Ok(chartData);
        }
    }
}
