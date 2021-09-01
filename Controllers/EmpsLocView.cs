using locationRecordeapi.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace locationRecordeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpsLocView : ControllerBase
    {
        private readonly locationRecordeapiContext _context;

        public EmpsLocView(locationRecordeapiContext context)
        {
            _context = context;
        }
        // GET: api/<EmpsLocView>
        [HttpGet]
        public IEnumerable<Emps_Locs_View> Get()
        {
            return _context.emps_Locs_View.ToList();
        }
        // GET api/<EmpsLocView>/1244

        [HttpGet("{phone}")]
        public IEnumerable<Emps_Locs_View> Get(string phone)
        {
            return _context.emps_Locs_View.Where(elv=>elv.empPhone == phone).ToList();
        }

        [HttpGet("[Action]/{code}")]
        public IEnumerable<Emps_Locs_View> GetByCode(string code)
        {
            return _context.emps_Locs_View.Where(elv => elv.empCode == code).ToList();
        }
    }
}
