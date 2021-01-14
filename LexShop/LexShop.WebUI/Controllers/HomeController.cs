using LexShop.Core.Contracts;
using LexShop.Core.Models;
using LexShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LexShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> context;
        
        IRepository<ProductCategory> ProductCategories;
        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            
            context = productContext;
            ProductCategories = productCategoryContext;

        }
        public ActionResult Index(string Category=null)
        {
            /*List<Product> products = context.Collection().ToList();
            return View(products);*/
            List<Product> products;
            List<ProductCategory> categories = ProductCategories.Collection().ToList();
            if(Category==null)
            {
                products = context.Collection().ToList();
            }
            else
            {
                products = context.Collection().Where(p => p.Category == Category).ToList();
            }
            ProductListViewModel model=new ProductListViewModel();
            model.ProductCategories = categories;
            model.Products = products;

            return View(model);


        }
        public ActionResult Details(string Id)
        {
            Product product = context.Find(Id);
            if(product==null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}