using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using ResistanceV2.Models;
using ResistanceV2.DataContext;
using System.Web.Script.Serialization;




namespace ResistanceV2.Controllers
{
    public class APIController : ApiController
    {
        private ResistanceDBContext db = new ResistanceDBContext();
        //
        // GET: /API/
        [ResponseType(typeof(List<Message>))]
        [Route("http://localhost:53348/API/GetFeed/")]
        public IHttpActionResult GetFeed()
        {

            return Ok(db.Message.ToList());






        }
	}
}