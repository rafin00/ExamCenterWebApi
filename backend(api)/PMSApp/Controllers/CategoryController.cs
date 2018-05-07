using PMSInterface;
using PMSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMSApp.Controllers
{
    public class CategoryController : BaseController
    {
        private ICategoryRepository repo;
        public CategoryController(ICategoryRepository repo)
        {
            this.repo = repo;
        }
        public ActionResult Index()
        {
            
            return View(this.repo.GetAll());
        }
    }
}