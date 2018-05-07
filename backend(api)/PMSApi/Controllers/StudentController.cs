using System;

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using PMSInterface;
using PMSRepository;
using PMSEntity;
using System.Threading;
using System.Threading.Tasks;
using PMSApi.Attributes;
using System.IO;
using System.Text;
using System.Web.Hosting;
namespace PMSApi.Controllers
{

    [RoutePrefix("api/students")]
    [BasicAuthentication]
    public class StudentController : ApiController
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

        public StudentController(IUserRepository usrepo, IQuestionRepository quesrepo, IEvntRepository evrepo, IAnswerRepository ansrepo, ICourseRepository currepo, IExamRepository exrepo, IOptionRepository oprepo, IRegistrationRepository regrepo, IResultRepository resrepo)
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





        
        [Route("{UserName}/evnts/running")]
        public IHttpActionResult GetEvntsRunningByStudent(string UserName)
        {
            Registration reg = new Registration();  
            List<Evnt> evn = this.evrepo.GetEvntsByStudent(UserName);
            if (evn.Count == 0)
            {
               
                return NotFound();
            }
            else
             return   Ok(evn);
        }

        [Route("{UserName}/evnts/finished")]
        public IHttpActionResult GetEvntsFinishedByStudent(string UserName)
        {
            Registration reg = new Registration();
            List<Evnt> evn = this.evrepo.GetFinishedEvntsByStudent(UserName);
            if (evn.Count == 0)
            {

                return NotFound();
            }
            else
                return Ok(evn);
        }

        [Route("{UserName}/evnts/{EvntId}")]
        public IHttpActionResult GetEvntsByStudent(string UserName,int EvntId)
        {
            Evnt ev = evrepo.Get(EvntId);
            if (ev != null)
            {
                if (ev.EvntEdt >= DateTime.Now && ev.EvntSdt<=DateTime.Now)
                {
                    if (evrepo.CheckEvnt(ev, UserName))
                        return Ok(ev);
                }
                return Unauthorized();
            }
            else
                return NotFound();
        }

        [Route("{UserName}/evnts/{EvntId}/viewresult")]
        public IHttpActionResult GetEvntsByStudentvr(string UserName, int EvntId)
        {
            Evnt ev = evrepo.Get(EvntId);
            if (ev != null)
            {
                if (ev.EvntEdt <= DateTime.Now )
                {
                    if (evrepo.CheckEvntVR(ev, UserName))
                        return Ok(ev);
                }
                return Unauthorized();
            }
            else
                return NotFound();
        }

        [Route("{UserName}/evnts/{EvntId}/questions")]
        public IHttpActionResult GetEntsByStudent(string UserName, int EvntId)
        {
            List<Exam> ex = exrepo.GetByEvnt(UserName, EvntId);
            return Ok(ex);
        }

        [Route("{UserName}/evnts/{EvntId}")]
        public IHttpActionResult PostFinishExam(string UserName, int EvntId)
        {
            regrepo.calculateResult(EvntId, UserName);
           return Ok();
        }

        [Route("{UserName}/evnts/{EvntId}/courses")]
        public IHttpActionResult GetEvntCoursesByStudent(string UserName, int EvntId)
        {
           List<Exam> exa = exrepo.GetCourses(EvntId);
           if (exa.Count == 0)
           {
               return StatusCode(HttpStatusCode.NoContent);
           }
           else
               return Ok(exa);
            
        }

        [Route("{UserName}/registrations/{EvntId}")]
        public IHttpActionResult GetResukt(string UserName, int EvntId)
        {
            return Ok(regrepo.GetEvntResult(UserName, EvntId));

        } 
        [Route("{UserName}/evnts/{EvntId}/courses/{CourseName}/questions")]
        public IHttpActionResult GetEvntQuestion(string UserName, int EvntId, string CourseName)
        {
            if (ansrepo.checkexist(UserName, EvntId))
            { }
            else
            {
                ansrepo.setAnsTable(UserName, EvntId);
            }
            List<Question> ques = quesrepo.GetQuestionsforevnt(EvntId,CourseName,UserName);

            if (ques == null)
                return StatusCode(HttpStatusCode.NoContent);
            else
                return Ok(ques);

        }

        [Route("{UserName}/evnts/{EvntId}/questions/{QuestionId}/answers")]
        public IHttpActionResult PutQuestionAnswer(Answer ans)
        {

            ansrepo.answerupdate(ans);
                return Ok();

        }

        [Route("{UserName}/evnts/{EvntId}/questions/{QuestionId}/answers")]
        public IHttpActionResult GetQuestionAnswer(string UserName,int EvntId,int QuestionId)
        {

            Answer ans = new Answer();
            ans.QuestionId = QuestionId; ans.EvntId = EvntId; ans.StudentUserName = UserName;
            return Ok(ansrepo.GetMyAnswer(ans));

        }

        [Route("{UserName}/evnts/{EvntId}/courses/{CourseName}/questions/{QuestionId}/options")]
        public IHttpActionResult GetQuestionOptions(string UserName, int EvntId, string CourseName, int QuestionId)
        {

            List<Option> op = oprepo.GetOptionsByQuestionId(QuestionId);

            if (op.Count == 0)
                return StatusCode(HttpStatusCode.NoContent);
            else
                return Ok(op);

        }
        //
        [Route("{UserName}/courses/average")]
        public IHttpActionResult Getaveragemarks(string UserName)
        {
            List<Result> results = resrepo.GetStudentAverageMarksByCourse(UserName);
            if (results.Count == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
                return Ok(results);
        }



        //
        [Route("evnts/check/{id}")]
        public IHttpActionResult GetcheckEvnt(String id)
        {
            string[] arr = id.Split(new char[] { ',' });
            int evntid =Convert.ToInt32(arr[1]);
            string username = arr[0];
            Evnt ev = evrepo.Get(evntid);
            if (ev.EvntEdt < DateTime.Now)
            { return Unauthorized(); }
            else if (regrepo.GetEvntResult(username,evntid)!=null)
            {
                return Unauthorized();
            }
            else return Ok(ev);
        }
        [Route("evntscourse/{id}")]
        public IHttpActionResult GetevntCourse(int id)
        {
            return Ok(exrepo.GetCourses(id));
        }
        [Route("evntcoursequestions/{id}")]
        public IHttpActionResult GetEvntCourseQuestions(string id)    
        {
            string[] arr = id.Split(new char[] { ',' });
            int evntid =Convert.ToInt32(arr[0]);
            string coursename = arr[1];

            return Ok(evrepo.GetQuestionsByEvntByCourse(evntid,coursename));

        }
        [Route("options/{id}")]
        public IHttpActionResult GetQuestionOptions(int id)
        {
            return Ok(oprepo.GetOptionsByQuestionId(id));
        }
        [Route("answers/{id}")]
        public IHttpActionResult PutAns(string id)
        {
            string[] arr =id.Split(new char[] { ',' });
            string userName = arr[0];
            int evntid =Convert.ToInt32(arr[1]);
            int questionid = Convert.ToInt32(arr[2]);
            string myans = arr[3];
            Answer ans = new Answer();
            ans.EvntId = evntid;
            ans.QuestionId = questionid;
            ans.MyAnswer = myans;
            ans.StudentUserName = userName;

            ansrepo.answerupdate(ans);
           
            return Ok();
        }

        [Route("evnts/{id}")]
        public IHttpActionResult PostGenResult(string id)
        {
            string[] arr = id.Split(new char[] { ',' });
            string username = arr[0];
            int evntid =Convert.ToInt32(arr[1]);
            regrepo.calculateResult(evntid,username);
            return Ok();
        }

        [Route("evnts/{id}")]
        public IHttpActionResult GetGenResult(int id)
        {
            
            return Ok(evrepo.Get(id));
        }
        

        [Route("registration/{id}")]
        public IHttpActionResult GetResuts(string id)
    {
        List<Registration> reg = regrepo.GetResult(id);
        if (reg == null)
            return NotFound();
       else return Ok (reg);
    }

    }

       
}
