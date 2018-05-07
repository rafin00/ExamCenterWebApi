using PMSInterface;
using PMSRepository;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace PMSApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IQuestionRepository, QuestionRepository>();
            container.RegisterType<IEvntRepository, EvntRepository>();
            container.RegisterType<IAnswerRepository, AnswerRepository>();
            container.RegisterType<ICourseRepository, CourseRepository>();
            container.RegisterType<IExamRepository, ExamRepository>();
            container.RegisterType<IOptionRepository, OptionRepository>();
            container.RegisterType<IRegistrationRepository, RegistrationRepository>();
            container.RegisterType<IResultRepository, ResultRepository>();
            

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}