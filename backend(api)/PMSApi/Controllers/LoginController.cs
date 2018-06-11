using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PMSInterface;
using PMSRepository;
using PMSEntity;
using PMSApi.Attributes;
using System.Threading;

namespace PMSApi.Controllers
{

     [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        private IUserRepository repo;
         
        public LoginController(IUserRepository repo)
        {
            this.repo = repo;
        }

       [Route("")]
       [BasicAuthentication]
        public IHttpActionResult Postthr(User us)
        {
            string st= Thread.CurrentPrincipal.Identity.Name;
            User usr = this.repo.Get(us.UserName);
            if (usr == null || usr.UserName != us.Password)
                return Unauthorized();
            else
            {
               return Ok(usr);
            }
        }
       [Route("CreateUser")]
       public IHttpActionResult NewUser(User us)
       {

           if (repo.Get(us.UserName) == null)
           {

               repo.Insert(us);
               return Ok();
           }
           else
           {
               return Conflict();
           }

       }
      
       
    }
}
