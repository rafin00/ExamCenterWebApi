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

     [RoutePrefix("api/teachers")]
     [BasicAuthentication]
    public class TeacherController : ApiController
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

        public TeacherController(IUserRepository usrepo,IQuestionRepository quesrepo,IEvntRepository evrepo,IAnswerRepository ansrepo,ICourseRepository currepo,IExamRepository exrepo,IOptionRepository oprepo,IRegistrationRepository regrepo,IResultRepository resrepo)
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


        [Route("fileUpload/courses/{CourseName}")]

         //upload for questions
        public Task<HttpResponseMessage> Post( string CourseName)
        {
            string fileRelativePath = ""; string error = "";
            List<string> savedFilePath = new List<string>();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string rootPath = HttpContext.Current.Server.MapPath("~/uploadFiles");
            var provider = new MultipartFileStreamProvider(rootPath);
            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsCanceled || t.IsFaulted)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }
                    foreach (MultipartFileData item in provider.FileData)
                    {
                        try
                        {
                            string name = item.Headers.ContentDisposition.FileName.Replace("\"", "");
                            string newFileName = Guid.NewGuid() + Path.GetExtension(name);
                            File.Move(item.LocalFileName, Path.Combine(rootPath, newFileName));

                            Uri baseuri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, string.Empty));
                            fileRelativePath = "~/uploadFiles/" + newFileName;
                            Uri fileFullPath = new Uri(baseuri, VirtualPathUtility.ToAbsolute(fileRelativePath));
                            savedFilePath.Add(fileFullPath.ToString());
                            error = myfunc(fileRelativePath, CourseName);

                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.Created, error);
                });


            return task;
        }

         //upload for events
          [Route("fileUpload/evnts/{EvntId}/courses/{CourseName}")]
        public Task<HttpResponseMessage> Post(int EvntId,string CourseName)
        {
            string fileRelativePath = ""; string error = "";
            List<string> savedFilePath = new List<string>();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string rootPath = HttpContext.Current.Server.MapPath("~/uploadFiles");
            var provider = new MultipartFileStreamProvider(rootPath);
            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsCanceled || t.IsFaulted)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }
                    foreach (MultipartFileData item in provider.FileData)
                    {
                        try
                        {
                            string name = item.Headers.ContentDisposition.FileName.Replace("\"", "");
                            string newFileName = Guid.NewGuid() + Path.GetExtension(name);
                            File.Move(item.LocalFileName, Path.Combine(rootPath, newFileName));

                            Uri baseuri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, string.Empty));
                            fileRelativePath = "~/uploadFiles/" + newFileName;
                            Uri fileFullPath = new Uri(baseuri, VirtualPathUtility.ToAbsolute(fileRelativePath));
                            savedFilePath.Add(fileFullPath.ToString());
                              error = myfunc(fileRelativePath,EvntId,CourseName);
                            
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.Created, error);
                });


            return task;
        }
         class OptionClass
        {
            public List<Option> options { get; set; }
        }
        //load file for events
         string myfunc(string path,int evntid,string CourseName)
         {
             int totaladded = 0;
            string mpath = HostingEnvironment.MapPath(path);
            StreamReader sre = new StreamReader(mpath);
            string line;
            List<Question> questions = new List<Question>();
            List<OptionClass> optionclass = new List<OptionClass>();
            OptionClass opclsobj = new OptionClass();
            List<Option> options = new List<Option>();
            string err = ""; 
            string[] tags = { "@question", "@answer", "@option" }; int index = 0, lc = 0, count = 0;
            Question ques = new Question();
            Option op = new Option();
            while ((line = sre.ReadLine()) != null)
            {

                if (index == 2)
                {
                    int at = -1;
                    at = line.IndexOf(tags[index]);
                    if (at >= 0)
                    {

                        string[] arr = line.Split(new char[] { ':' });
                        op.OptionText = arr[1].Trim();
                        if (String.IsNullOrWhiteSpace(op.OptionText))
                        {
                            return err = "Option error, at line " + lc;

                        }
                        if (op.OptionText == "NA")
                        {
                            return "Option/Answer can not be NA, at line " + lc;
                        }
                        options.Add(op);
                        op = new Option();
                        count = 3;


                    }
                    at = line.IndexOf(tags[0]);
                    if (at >= 0)
                    {
                        index = 0;
                        opclsobj.options = options;
                        options = new List<Option>();
                        optionclass.Add(opclsobj);
                        opclsobj = new OptionClass();
                      
                    }
                   
                }
              
                string[] arr2 = line.Split(new char[] { ':' });
                if (arr2.Length > 1)
                {
                    string tmptag = arr2[0].Trim();
                    if (!(String.IsNullOrWhiteSpace(tmptag)))
                    {
                        
                        if (tmptag != tags[index])
                        {
                            return tags[index] + " expected, at line " + lc;
                        }
                    }
                }
               
           
                if (index == 0)
                {

                    int at = -1;
                    at = line.IndexOf(tags[index]);
                    if (at >= 0)
                    {

                        string[] arr = line.Split(new char[] { ':' });
                        ques.QuestionText = arr[1].Trim();
                        if (String.IsNullOrWhiteSpace(ques.QuestionText))
                        {
                            return err = "Question error, at line " + lc;

                        }
                        count = 1;
                        index++;

                    }
                }
                if (index == 1)
                {
                    int at = -1;
                    at = line.IndexOf(tags[index]);
                    if (at >= 0)
                    {

                        string[] arr = line.Split(new char[] { ':' });
                        ques.Answer = arr[1].Trim();
                        if (String.IsNullOrWhiteSpace(ques.Answer))
                        {
                            return err = "Answer error, at line " + lc;

                        }
                        ques.CourseName = CourseName;
                        ques.TeacherUserName=Thread.CurrentPrincipal.Identity.Name;
                        questions.Add(ques);
                        ques = new Question();
                        count = 2;
                        index++;

                    }
                }
                


                lc++;
            }
            sre.Close();
            if (count < 3)
            {
                return "Incomplete file";
            }
            else
            {
                if(optionclass==null);
                if(questions == null);
                opclsobj.options = options;
                options = new List<Option>();
                optionclass.Add(opclsobj);
                opclsobj = new OptionClass();
                Question questoadd = new Question();
                List<Option> optoadd = new List<Option>();
                Course cc = new Course();
                Exam ex = new Exam();
                int qid = -1;
                for (int k = 0; k < questions.Count; k++)
                {
                    if (!quesrepo.CheckValidQuestion(questions[k], optionclass[k].options))
                    {
                        return "Failed!Incorrects options for Question no. " + (k + 1);
                    }
                }
                    for (int i = 0; i < questions.Count; i++)
                    {
                        questoadd = questions[i];
                        optoadd = optionclass[i].options;

                        qid = quesrepo.CheckIdenticalQuestion(questoadd, optoadd);
                        if (qid == (-1))
                        {

                            quesrepo.Insert(questoadd);
                            int quesid = quesrepo.GetMaxId();

                            foreach (Option oop in optoadd)
                            {
                                oop.QuestionId = quesid;
                                oprepo.Insert(oop);
                            }
                            ex.CourseName = questoadd.CourseName;
                            ex.EvntId = evntid;
                            ex.QuestionId = quesid;
                            exrepo.Insert(ex);
                            totaladded++;

                        }
                        else
                        {
                            questoadd.QuestionId = qid;
                            if (!exrepo.CheckIdenticalQuestion(questoadd, evntid))
                            {
                                ex.EvntId = evntid;
                                ex.QuestionId = qid;
                                ex.CourseName = questoadd.CourseName;
                                exrepo.Insert(ex);
                                ex = new Exam();
                                totaladded++;
                            }

                        }
                    }

                return "Questions Added : "+totaladded.ToString();
            }


           
        }
        //load file for courses
         string myfunc(string path, string CourseName)
         {
             int totaladded = 0;
             string mpath = HostingEnvironment.MapPath(path);
             StreamReader sre = new StreamReader(mpath);
             string line;
             List<Question> questions = new List<Question>();
             List<OptionClass> optionclass = new List<OptionClass>();
             OptionClass opclsobj = new OptionClass();
             List<Option> options = new List<Option>();
             string err = "";
             string[] tags = { "@question", "@answer", "@option" }; int index = 0, lc = 0, count = 0;
             Question ques = new Question();
             Option op = new Option();
             while ((line = sre.ReadLine()) != null)
             {

                 if (index == 2)
                 {
                     int at = -1;
                     at = line.IndexOf(tags[index]);
                     if (at >= 0)
                     {

                         string[] arr = line.Split(new char[] { ':' });
                         op.OptionText = arr[1].Trim();
                         if (String.IsNullOrWhiteSpace(op.OptionText))
                         {
                             return err = "Option error, at line " + lc;

                         }
                         if (op.OptionText == "NA")
                         {
                             return "Option/Answer can not be NA, at line " + lc;
                         }
                         options.Add(op);
                         op = new Option();
                         count = 3;


                     }
                     at = line.IndexOf(tags[0]);
                     if (at >= 0)
                     {
                         index = 0;
                         opclsobj.options = options;
                         options = new List<Option>();
                         optionclass.Add(opclsobj);
                         opclsobj = new OptionClass();

                     }

                 }

                 string[] arr2 = line.Split(new char[] { ':' });
                 if (arr2.Length > 1)
                 {
                     string tmptag = arr2[0].Trim();
                     if (!(String.IsNullOrWhiteSpace(tmptag)))
                     {

                         if (tmptag != tags[index])
                         {
                             return tags[index] + " expected, at line " + lc;
                         }
                     }
                 }


                 if (index == 0)
                 {

                     int at = -1;
                     at = line.IndexOf(tags[index]);
                     if (at >= 0)
                     {

                         string[] arr = line.Split(new char[] { ':' });
                         ques.QuestionText = arr[1].Trim();
                         if (String.IsNullOrWhiteSpace(ques.QuestionText))
                         {
                             return err = "Question error, at line " + lc;

                         }
                         count = 1;
                         index++;

                     }
                 }
                 if (index == 1)
                 {
                     int at = -1;
                     at = line.IndexOf(tags[index]);
                     if (at >= 0)
                     {

                         string[] arr = line.Split(new char[] { ':' });
                         ques.Answer = arr[1].Trim();
                         if (String.IsNullOrWhiteSpace(ques.Answer))
                         {
                             return err = "Answer error, at line " + lc;

                         }
                         ques.CourseName = CourseName;
                         ques.TeacherUserName = Thread.CurrentPrincipal.Identity.Name;
                         questions.Add(ques);
                         ques = new Question();
                         count = 2;
                         index++;

                     }
                 }



                 lc++;
             }
             sre.Close();
             if (count < 3)
             {
                 return "Incomplete file";
             }
             else
             {
                 if (optionclass == null) ;
                 if (questions == null) ;
                 opclsobj.options = options;
                 options = new List<Option>();
                 optionclass.Add(opclsobj);
                 opclsobj = new OptionClass();
                 Question questoadd = new Question();
                 List<Option> optoadd = new List<Option>();
                 Course cc = new Course();
                
                 int qid = -1;
                 for (int k = 0; k < questions.Count; k++)
                 {
                     if (!quesrepo.CheckValidQuestion(questions[k], optionclass[k].options))
                     {
                         return "Failed!Incorrects options for Question no. " + (k + 1);
                     }
                 }
                 for (int i = 0; i < questions.Count; i++)
                 {
                     questoadd = questions[i];
                     optoadd = optionclass[i].options; 

                     qid = quesrepo.CheckIdenticalQuestion(questoadd, optoadd);
                     if (qid == (-1))
                     {

                         quesrepo.Insert(questoadd);
                         int quesid = quesrepo.GetMaxId();

                         foreach (Option oop in optoadd)
                         {
                             oop.QuestionId = quesid;
                             oprepo.Insert(oop);
                         }
                         
                         totaladded++;

                     }
                     
                 }

                 return "Questions Added : " + totaladded.ToString();
             }



         }

        [Route("{UserName}/evnts/running")]
        public IHttpActionResult GetCurrentevntsbyteacher(string UserName)
        {

            List<Evnt> evnts = this.evrepo.GetAllByTeacher(Thread.CurrentPrincipal.Identity.Name);
            evnts.RemoveAll(ev => ev.EvntEdt < DateTime.Now);
            if (evnts.Count == 0)
                return StatusCode(HttpStatusCode.NoContent);
           
            return Ok(evnts);

        }

        [Route("{UserName}/evnts/finished")]
        public IHttpActionResult GetEvntsFinishedByStudent(string UserName)
        {
            Registration reg = new Registration();
            List<Evnt> evn = this.evrepo.GetFinishedEvntsByTeacher(UserName);
            if (evn.Count == 0)
            {

                return NotFound();
            }
            else
                return Ok(evn);
        }


        [Route("{UserName}/evnts")]
        public IHttpActionResult GetAllevntsbyteacher(string UserName)
        {

            List<Evnt> evnts = this.evrepo.GetAllByTeacher(Thread.CurrentPrincipal.Identity.Name);
            if (evnts.Count == 0)
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(evnts);

        }

        [Route("{UserName}/evnts")]
        public IHttpActionResult PostCreateEvent(string UserName,Evnt ev)
        {

            if (ev.EvntEdt < DateTime.Now)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                Request.CreateErrorResponse(
                    (HttpStatusCode)412,
                    new HttpError("End Time Cant be less than current time")
                )
            );
            }
            else if (ev.EvntEdt < ev.EvntSdt)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                Request.CreateErrorResponse(
                    (HttpStatusCode)412,
                    new HttpError("Event Can't end before it starts")
                ));
            }
            evrepo.Insert(ev);
           

            return Ok(ev);

        }

        [Route("{UserName}/evnts/{EvntId}/courses/{CourseName}/questions")]
        public IHttpActionResult GetEvntQuestion(string UserName, int EvntId, string CourseName)
        {
           
            List<Question> ques = quesrepo.GetQuestionsforevntTV(EvntId, CourseName, UserName);

            if (ques == null)
                return StatusCode(HttpStatusCode.NoContent);
            else
                return Ok(ques);

        }
        
        [Route("{UserName}/evnts/{EvntId}/students/{StudentUsername}/questions/{QuestionId}/answers")]
        public IHttpActionResult GetQuestionAnswer(string UserName, string StudentUserName, int EvntId, int QuestionId)
        {

            Answer ans = new Answer();
            ans.QuestionId = QuestionId; ans.EvntId = EvntId; ans.StudentUserName = StudentUserName;
            return Ok(ansrepo.GetMyAnswer(ans));

        }
        [Route("{UserName}/courses/average")]
        public IHttpActionResult Getaveragemarks(string UserName)
        {
            List<Result> results = resrepo.GetTeacherAverageMarksByCourse(UserName);
            if (results.Count == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
                return Ok(results);
        }

        [Route("{UserName}/evnts/{EvntId}")]
        public IHttpActionResult Getevntsbyteacher(string UserName, int EvntId)
        {

            Evnt evnts = this.evrepo.Get(EvntId);

            if (evnts == null)
                return StatusCode(HttpStatusCode.NoContent);
            else if (evnts.TeacherUserName != UserName)
                return Unauthorized();
            else
                return Ok(evnts);

        }

        [Route("{UserName}/registrations/{EvntId}/results")]
        public IHttpActionResult GetRegistrationsbyEvnt(string UserName, int EvntId)
        {
             List<Registration> regs= regrepo.GetRegistrationsByEvnts(EvntId);
            

            if (regs.Count == 0)
                return StatusCode(HttpStatusCode.NoContent);
           
            else
                return Ok(regs);

        }

       

       
        [Route("{UserName}/evnts/{EvntId}")]
        public IHttpActionResult Putevntsbyteacher(Evnt ev)
        {

            evrepo.Update(ev);
            return Ok();

        }
        [Route("{UserName}/evnts/{EvntId}")]
        public IHttpActionResult Deleteevntbyteacher(string UserName, int EvntId)
        {
            evrepo.DeleteTotalEvnt(EvntId);
            Evnt evnts = this.evrepo.Get(EvntId);


            return StatusCode(HttpStatusCode.NoContent);

        }

         //get all students for assign
        [Route("students")]
        public IHttpActionResult GetAllTeacher()
        {

            return Ok(usrepo.GetAllByType("Students"));

        }

         //check assigned
        [Route("{UserName}/evnts/{EvntId}/students/{StudentUserName}")]
        public IHttpActionResult GetCheckStudentAssigned(string UserName, int EvntId,string StudentUserName)
        {
            if (regrepo.CheckRegistration(EvntId, StudentUserName))
            {
                return Ok(); 
            }
            return StatusCode(HttpStatusCode.NoContent);

        }
         //Assign All
        [Route("{UserName}/evnts/{EvntId}/students")]
        public IHttpActionResult PostAllStudentsAssign(string UserName, int EvntId)
        {
            regrepo.AssignAll(EvntId);
            return Ok();

        }
         //unassign all
        [Route("{UserName}/evnts/{EvntId}/students")]
        public IHttpActionResult DeleteAllStudentsAssign(string UserName, int EvntId)
        {
            regrepo.UnAssignAll(EvntId);
            return Ok();

        }

         //assign/unassign one
        [Route("{UserName}/evnts/{EvntId}/students/{StudentUserName}")]
        public IHttpActionResult PostStudentAssignUnasign(string UserName, int EvntId,string StudentUserName)
        {
            regrepo.AddRemoveRegistration(EvntId,StudentUserName);
            return Ok();

        }

        [Route("{UserName}/evnts/{EvntId}/courses")]
        public IHttpActionResult GetAllevntcourses(string UserName, int EvntId)
        {

            List<Exam> ex = exrepo.GetCourses(EvntId);

            if (ex.Count==0)
                return StatusCode(HttpStatusCode.NoContent);
            else
                return Ok(ex);

        }

        [Route("{UserName}/evnts/{EvntId}/courses/{CourseName}")]
        public IHttpActionResult DeleteEvntCourses(string UserName, int EvntId,string CourseName)
        {

            exrepo.DeleteEvntCourse(EvntId, CourseName);
           
                return StatusCode(HttpStatusCode.NoContent);
            

        }

        

        [Route("{UserName}/evnts/{EvntId}/courses/{CourseName}/questions/{QuestionId}")]
        public IHttpActionResult DeleteEvntCoursesQuestion(string UserName, int EvntId, string CourseName,int QuestionId)
        {

            exrepo.DeleteEvntQuestion(EvntId, QuestionId);
           
            return StatusCode(HttpStatusCode.NoContent);

        }

        [Route("{UserName}/evnts/{EvntId}/courses/{CourseName}/questions")]
        public IHttpActionResult PostEvntCoursesQuestion(Exam exam)
        {   Exam ex =exrepo.GetEvntQuestion(exam.EvntId, exam.QuestionId);
        if (ex == null)
        {
            exrepo.Insert(exam);
            return Ok(exam);
        }
        else
        {
            exrepo.DeleteEvntQuestion(ex.EvntId,ex.QuestionId);
            return StatusCode(HttpStatusCode.NoContent);
        }
        }

        [Route("{UserName}/evnts/{EvntId}/courses/{CourseName}/questions/{QuestionId}")]
        public IHttpActionResult GetEvntCoursesQuestion(string UserName, int EvntId, string CourseName, int QuestionId)
        {
            Exam ex = exrepo.GetEvntQuestion(EvntId, QuestionId);
            if (ex == null)
                return NotFound();
            else
                return Ok(ex);
        }

        [Route("{UserName}/evnts/{EvntId}/courses/{CourseName}/questions/{QuestionId}/options")]
        public IHttpActionResult GetQuestionOptions(string UserName,int EvntId,string CourseName, int QuestionId)
        {

            List<Option> op = oprepo.GetOptionsByQuestionId(QuestionId);

            if (op.Count == 0)
                return StatusCode(HttpStatusCode.NoContent);
            else
                return Ok(op);

        }
         //course questions

        [Route("{UserName}/courses/{CourseName}/questions")]
        public IHttpActionResult GetQuestionOptions(string UserName, string CourseName)
        {
            List<Question> questons = quesrepo.GetCourseQuestions(CourseName, UserName);

            
            return Ok(questons);


        }
         //Question option
        [Route("{UserName}/options/{QuestionId}")]
        public IHttpActionResult GetQuestionOptions(string UserName,int QuestionId)
        {
            return Ok(oprepo.GetOptionsByQuestionId(QuestionId));
            

        }


        [Route("{UserName}/courses")]
        public IHttpActionResult GetCoursesByTeacher(string UserName)
        {
            List<Course> curr = currepo.GetAll().FindAll(cur => cur.TeacherUserName == UserName);
            return Ok(curr);

        }
        [Route("{UserName}/courses")]
        public IHttpActionResult PostCoursesByTeacher(Course course)
        {
            Course cr = currepo.Get(course.CourseName);
            if (cr != null)
            {
                return Conflict();
            }
            else 
            {
                currepo.Insert(course);
                return StatusCode(HttpStatusCode.Created);
            }
           
           

        }
        

       


         //old
       [Route("evnts")]
         
       public IHttpActionResult Post(Evnt ev)
       {
           string username = Thread.CurrentPrincipal.Identity.Name;
           ev.TeacherUserName = username;
           
               this.evrepo.Insert(ev);
               int evntid = evrepo.GetMaxId();

               List<User> us = usrepo.GetAllByType("Student");
               for (int i = 0; i < us.Count; i++)
               {
                   Registration reg = new Registration();
                   reg.EvntId = evntid;
                   reg.Result = -1;
                   reg.StudentUserName = us[i].UserName;
                   regrepo.Insert(reg);

                   //this.ansrepo.setAnsTable(us[i].UserName, evntid);
               }



               return Ok();
           
          

       }
         [Route("answers")]
       public IHttpActionResult Postanswers()
     {
         List<User> us =usrepo.GetAllByType("Student");
         for (int i = 0; i < us.Count; i++)
               {

                   int evntid = evrepo.GetMaxId();
                   this.ansrepo.setAnsTable(us[i].UserName, evntid);
               }
         return Ok();
     }

         

       [Route("GetAllEvntsByTeacher")]

       public IHttpActionResult Get()
       {


           return Ok(this.evrepo.GetAllByTeacher(Thread.CurrentPrincipal.Identity.Name));

       }
       [Route("evnts/{id}")]
       public IHttpActionResult Get(int id)
       {
           Evnt ev =this.evrepo.Get(id);
           if (ev != null)
               return Ok(ev);
           else
               return NotFound();

       }
       [Route("evntsByTeacher/{id}")]
       public IHttpActionResult GetEvntsby(string id)
       {
           return Ok(evrepo.GetAllByTeacherName(id));
       }
       [Route("coursesbyevnt/{id}")]
       public IHttpActionResult Getbyevnt(int id)
       {
           List<Exam> crs = this.exrepo.GetCourses(id);
           if (crs != null)
               return Ok(crs);
           else
               return NotFound();

       }
       [Route("courses")]
       public IHttpActionResult GetCourses()
       {
           List<Course> crs = this.currepo.GetAll();
           if (crs != null)
               return Ok(crs);
           else
               return NotFound();

       }
         [Route("courses/question/{id}")]
       public IHttpActionResult GetAddQuesRand(string id)
       {
           string[] arr = id.Split(new char[] { ',' });
           int evntId = Convert.ToInt32(arr[0]);
           int courseId = Convert.ToInt32(arr[1]);
           int totalques = Convert.ToInt32(arr[2]);
           List<Course> crs = this.currepo.GetAll();
           if (crs != null)
               return Ok(crs);
           else
               return NotFound();

       }

         [Route("GetAllStudents")]
         public IHttpActionResult GetAllStudents()
         {
            return Ok(usrepo.GetAllByType("Student"));
         }

            [Route("AssignStudents")]
         public IHttpActionResult GetAssignStudents(Evnt evnt)
         {
             string thrusername = Thread.CurrentPrincipal.Identity.Name;
            
                 return Ok();
         }
            
            public class MyViewModel
            {
                public Evnt evnt { get; set; }
                public Course crs { get; set; }
            }
         [Route("evnts/questionsbyevntcourse/{id}")]
            public IHttpActionResult GetQuesionsbycourse(string id)
            {
                string[] arr = id.Split(new char[] { ',' });
                int evntid = Convert.ToInt32(arr[0]);
                string courseName = arr[1];
              

                return Ok(this.evrepo.GetQuestionsByEvntByCourse( evntid, courseName));
            }
         [Route("students")]
         public IHttpActionResult Getstudents()
         {
            

             return Ok(usrepo.GetAllByType("Student"));
         }

         
         [Route("evnts/students/{id}")]
         public IHttpActionResult GetCheckStudentEvnt(string id)
         {

             string[] arr = id.Split(new char[] { ',' });
             int evntid = Convert.ToInt32(arr[1]);
             string username = arr[0];

             return Ok(this.regrepo.CheckRegistration(evntid,username));
         }
         
          [Route("evnts/register/student/{id}")]
         public IHttpActionResult Getchekstd(string id)
         {
           
             string[] arr = id.Split(new char[] { ',' });
             int evntid = Convert.ToInt32(arr[1]);
             string username = arr[0];
             this.regrepo.AddRemoveRegistration(evntid, username);
             return Ok();
         }
     [Route("registrations/{id}")]
         public IHttpActionResult GetRegistrartionsByEvnts(int id)
     {
         return Ok(regrepo.GetRegistrationsByEvnts(id));
     }
       
    }
}
