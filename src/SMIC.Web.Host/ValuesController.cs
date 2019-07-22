using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMIC.Controllers;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SMIC.Web.Host
{
    [Route("api/[controller]")]
    public class ValuesController : SMICControllerBase
    {
        // GET: api/<controller>
        [HttpGet]
        [IgnoreAntiforgeryToken]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [IgnoreAntiforgeryToken]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Post([FromBody]string value1)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [IgnoreAntiforgeryToken]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [IgnoreAntiforgeryToken]
        public void Delete(int id)
        {
        }
    }
}
