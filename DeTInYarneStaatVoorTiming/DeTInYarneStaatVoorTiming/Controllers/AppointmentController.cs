using DeTInYarneStaatVoorTiming.Data;
using DeTInYarneStaatVoorTiming.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace DeTInYarneStaatVoorTiming.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private IDataContext _data;
        private Methods methods;
        public AppointmentController(IDataContext data)
        {
            _data = data;
            methods = new Methods();
        }

        [HttpGet]
        [Authorize(Policy = "BasicAuthentication", Roles = "admin, user")]
        public ActionResult<List<Appointment>> GetAllAppointments() 
        {
            return Ok(_data.GetAllAppointments().ToList());
        }

        [HttpGet]
        [Route("/Appointment/date")]
        [Authorize(Policy = "BasicAuthentication", Roles = "admin, user")]
        public ActionResult<List<Appointment>> GetAppointmentsByDate(string date)
        {
            return Ok(_data.GetAppointmentsByDate(date));
        }

        [HttpPost]
        [Authorize(Policy = "BasicAuthentication", Roles = "admin, user")]
        public ActionResult<string> AddAppointment(Appointment appointment)
        {
            if (!methods.FilterStringInput(appointment.activityTitle)) return BadRequest("invalid activity title");
            if (!methods.FilterStringInput(appointment.location)) return BadRequest("invalid location");
            if (!methods.FilterStringInput(appointment.submitterName)) return BadRequest("invalid name");
            

            try
            {
                string result = _data.AddAppointment(appointment);
                if (result == "success") return Ok("Appointment added");
                return BadRequest(result);
            }
            catch { return BadRequest("something went wrong"); }
        }

        [HttpDelete]
        [Authorize(Policy = "BasicAuthentication", Roles = "admin, user")]
        public ActionResult<string> DeleteAppointmentByInput(string activityTitle, string submitterName)
        {
            if (!methods.FilterStringInput(activityTitle)) return BadRequest("invalid activity title");
            if (!methods.FilterStringInput(submitterName)) return BadRequest("invalid name");

            if (_data.DeleteAppointmentByInput(activityTitle, submitterName)) return Ok("activity deleted");
            return BadRequest("no activity with this type of title and name");
        }

        [HttpDelete]
        [Route("/Appointment/date")]
        [Authorize(Policy = "BasicAuthentication", Roles = "admin, user")]
        public ActionResult<string> DeleteAppointmentByDate(string date)
        {
            if (_data.DeleteAppointmentByDate(date)) return Ok("activities deleted");
            return BadRequest("no activities on this date");
        }

    }
}
