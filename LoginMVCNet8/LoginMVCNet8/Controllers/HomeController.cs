using LoginMVCNet8.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoginMVCNet8.Controllers
{
    [Authorize]
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Ejemplo()
        {
            string connectionString = "User Id=sys;Password=root;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));DBA Privilege=SYSDBA;";
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                using OracleCommand cmd = con.CreateCommand();
                try
                {
                    con.Open();

                    //Use the command to display employee names from EMPLOYEES table
                    cmd.CommandText = "select * from sys";

                    // Assign id to the department number 50 
                    cmd.BindByName = true;
                    OracleParameter id = new OracleParameter("id", 50);
                    cmd.Parameters.Add(id);

                    OracleDataReader reader = cmd.ExecuteReader();
                    id.Dispose();
                    reader.Dispose();
                }
                catch (Exception ex)
                {

                }
            }
            return BadRequest(new { message = "hola" });
        }
    }
}
