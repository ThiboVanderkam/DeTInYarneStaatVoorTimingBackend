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
    public class AccountController : ControllerBase
    {
        private IDataContext _data;
        public AccountController(IDataContext data)
        {
            _data = data;
        }

        [HttpGet]
        [Authorize(Policy = "BasicAuthentication", Roles = "admin")]
        public ActionResult<List<Account>> GetAllAccounts()
        {
            return _data.GetAllAccounts().ToList();
        }

        [HttpPost]
        [Authorize(Policy = "BasicAuthentication", Roles = "admin")]
        public ActionResult<string> AddAccount(Account account)
        {
            _data.AddAccount(account);
            return Ok("account added");
            
        }

        [HttpDelete]
        [Authorize(Policy = "BasicAuthentication", Roles = "admin")]
        public ActionResult DeleteAccount(int id)
        {
            if (_data.DeleteAccount(id)) return Ok("account deleted");
            return BadRequest("account not found");
        }
    }
}
