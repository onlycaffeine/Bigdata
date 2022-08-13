using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testmongo.Areas.Common;
using testmongo.Models;

namespace testmongo.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult TopMenu()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {

            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var DB = Client.GetDatabase("Employee");
            var collection = DB.GetCollection<Footer>("Footer").Find(new BsonDocument()).ToList();
            return PartialView(collection);
        }

        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            //var model = new MenuDao().ListByGroupId(1);
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var DB = Client.GetDatabase("Employee");
            var collection = DB.GetCollection<Menu>("Menu").Find(Builders<Menu>.Filter.Where(s => s.MenuTypeID == "1")).ToList();
            //collection = collection.Find(Builders<Menu>.Filter.Where(s => s.MenuTypeID == "1")).ToList();

            //var collection = db.GetCollection<Employee>("EmployeeDetails").Find(new BsonDocument()).ToList();
            return PartialView(collection);
        }

        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            //var cart = Session[CartSession];
            var cart = Session[CommonConstants.CartSession];
            var list = new List<Cart>();
            if (cart != null)
            {
                list = (List<Cart>)cart;
            }
            return PartialView(list);
        }

        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            //var model = new ProductCategoryDao().ListAll();
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var db = Client.GetDatabase("Employee");
            var collection = db.GetCollection<ProductCategory>("ProductCategory").Find(new BsonDocument()).ToList();

            return PartialView(collection);
        }

        public ActionResult FeatureProduct()
        {

            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var db = Client.GetDatabase("Employee");
            var collection = db.GetCollection<Product>("ProductDetails").Find(new BsonDocument()).ToList();

            //var model = new ProductDao().ListFeatureProduct(4);
            return PartialView(collection);
        }

        public ActionResult NewProduct()
        {
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var DB = Client.GetDatabase("Employee");
            var collection = DB.GetCollection<Product>("ProductDetails").Find(new BsonDocument()).ToList();

            //var model = new ProductDao().ListNewProduct(4);
            return PartialView(collection);
        }
    }
}