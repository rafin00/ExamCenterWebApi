using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMSRepository;
using PMSInterface;
using PMSEntity;

namespace PMSApp.Controllers
{
    public class ProductController : Controller
    {
        IProductRepository repo = new ProductRepository();
        public ActionResult Index()
        {
            
            return View();
        }
    }
}