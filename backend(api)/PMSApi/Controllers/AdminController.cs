using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PMSInterface;
using PMSRepository;
using PMSEntity;
using System.Threading;
using PMSApi.Attributes;
using System.IO;
using System.Web.Hosting;
using System.Web;
using System.Threading.Tasks;
namespace PMSApi.Controllers
{

     [RoutePrefix("api/admin")]
     [BasicAuthentication]
    public class AdminController : ApiController
    {
        
        private IUserRepository usrepo;
        private IQuestionRepository quesrepo;
        private IEvntRepository evrepo;
        private IAnswerRepository ansrepo;
        private ICourseRepository currepo;
        private IExamRepository exrepo;
        private IOptionRepository oprepo;
        private IRegistrationRepository regrepo;
        private IResultRepository resrepo;

        public AdminController(IUserRepository usrepo,IQuestionRepository quesrepo,IEvntRepository evrepo,IAnswerRepository ansrepo,ICourseRepository currepo,IExamRepository exrepo,IOptionRepository oprepo,IRegistrationRepository regrepo,IResultRepository resrepo)
        {
            this.evrepo = evrepo;
            this.usrepo = usrepo;
            this.quesrepo = quesrepo;
            this.ansrepo = ansrepo;
            this.currepo = currepo;
            this.exrepo = exrepo;
            this.oprepo = oprepo;
            this.regrepo = regrepo;
            this.resrepo = resrepo;
        }


       

       [Route("teachers")]
        public IHttpActionResult GetAllTeacher()
        {
            
               return Ok(usrepo.GetAllByType("Teacher"));
            
        }
       [Route("teachers")]
       public IHttpActionResult Postthr(User us)
       {

           if (usrepo.Get(us.UserName)==null)
           {
               
               usrepo.Insert(us);
               return Ok();
           }
           else
           {
               return Conflict();
           }

       }

       [Route("teachers/{username}")]
       public IHttpActionResult DeleteTeacher(string username)
       {
           if (usrepo.Get(username) != null)
           {
               User us = new User(); us.UserName=username;
               usrepo.Delete(us);
               return StatusCode(HttpStatusCode.NoContent);
           }
           return StatusCode(HttpStatusCode.NotFound);

       }
       
    }
}
