using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LexShop.Core.Models;
using LexShop.DataAccess.InMemory;
using LexShop.Core.ViewModels;
using LexShop.Core.Contracts;

namespace LexShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        //ProductRepository context;
        //InMemoryRepository<Product> context;
        IRepository<Product> context;
        //ProductCatogoryRepository ProductCategories;
        //InMemoryRepository<ProductCategory> ProductCategories;
        IRepository<ProductCategory> ProductCategories;
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            /*context = new InMemoryRepository<Product>();
            ProductCategories = new InMemoryRepository<ProductCategory>();*/
            context = productContext;
            ProductCategories = productCategoryContext;

        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }
        public ActionResult Create()
        {
            /* Product product = new Product();
             return View(product);*/
            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.ProductCategories = ProductCategories.Collection();
            return View(viewModel);
        }
        [HttpPost]
        
        public ActionResult Create(Product product)
        {
            if(ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                context.Insert(product);
                context.commit();
                return RedirectToAction("Index");

            }
        }

        public ActionResult Edit(String Id)
        {
            Product product = context.Find(Id);   
            if (product==null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = ProductCategories.Collection();
                return View(viewModel);
            }
        }
        [HttpPost]

        public ActionResult Edit(Product product, string Id)
        {
            Product productToEdit = context.Find(Id);

            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    return View(product);
                }
                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.image = product.image;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;

                context.commit();
                return RedirectToAction("Index");

            }
        }
        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if(productToDelete==null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.commit();
                return RedirectToAction("Index");
            }
        }

    }
}