using Microsoft.AspNetCore.Mvc;
using MyAspNetApp.Services;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

/// <summary>
/// Controller for the web application.
/// </summary>
namespace MyAspNetApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ParseRequestHandler _requestHandler;

        public HomeController()
        {
            _requestHandler = new ParseRequestHandler();
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Main method for validating KerMLv2 and SysMLv2.
        /// </summary>
        [HttpPost]
        public IActionResult Parse([FromBody] JObject data)
        {
            Debug.WriteLine("Received Data: " + data.ToString());
            var (result, message) = _requestHandler.HandleRequest(data);
            var statusCode = result ? 201 : 400;

            return StatusCode(statusCode, new { success = result, result = message });
        }
    }
}
