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
            //var model = new MenuDao().ListByGroupId(1);
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var DB = Client.GetDatabase("Employee");
            var collection = DB.GetCollection<Menu>("Menu").Find(Builders<Menu>.Filter.Where(s => s.MenuTypeID == "2")).ToList();
            //collection = collection.Find(Builders<Menu>.Filter.Where(s => s.MenuTypeID == "1")).ToList();

            //var collection = db.GetCollection<Employee>("EmployeeDetails").Find(new BsonDocument()).ToList();
            return PartialView(collection);
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
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
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
            List<Product> listLinks = new List<Product>();
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var db = Client.GetDatabase("Employee");

            var model = from b in db.GetCollection<Product>("ProductDetails").AsQueryable()
                        where b.Status == true
                        select new { b.Price, b.Quantity, b.Id, b.ProductName, b.ProductImage, b.MetaTitle, b.StarRated };

            foreach (var item in model)
            {

                Product temp = new Product();
                temp.Id = item.Id;
                temp.ProductName = item.ProductName;
                temp.ProductImage = item.ProductImage;
                temp.MetaTitle = item.MetaTitle;
                temp.Price = item.Price;
                temp.Quantity = item.Quantity;
                temp.StarRated = item.StarRated;
                listLinks.Add(temp);

            }

            return PartialView(listLinks.Take(8).OrderByDescending(x => x.StarRated));
        }

        public ActionResult NewProduct()
        {
            List<Product> listLinks = new List<Product>();
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var db = Client.GetDatabase("Employee");

            var model = from b in db.GetCollection<Product>("ProductDetails").AsQueryable()
                        where b.Status == true
                        select new { b.Price, b.Quantity, b.Id, b.ProductName, b.ProductImage, b.MetaTitle, b.CreateDate};

            foreach (var item in model)
            {

                Product temp = new Product();
                temp.Id = item.Id;
                temp.ProductName = item.ProductName;
                temp.ProductImage = item.ProductImage;
                temp.MetaTitle = item.MetaTitle;
                temp.Price = item.Price;
                temp.Quantity = item.Quantity;
                temp.CreateDate = item.CreateDate;
                listLinks.Add(temp);

            }

            return PartialView(listLinks.Take(4).OrderByDescending(x => x.CreateDate));

        }

        public ActionResult RecommendProduct()
        {
            List<Product> listLinks = new List<Product>();
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var db = Client.GetDatabase("Employee");
            var model = db.GetCollection<User>("User").Find(new BsonDocument()).ToList();
            var collection = db.GetCollection<Product>("ProductDetails").Find(Builders<Product>.Filter.Where(s => s.ProductName == "")).ToList();

            var session = (UserLogin)Session[testmongo.Areas.Common.CommonConstants.USER_SESSION];
            if (session == null)
            {
                return PartialView(collection);
            }

            var uid = new ObjectId(session.UserID);

            var model1 = from l in db.GetCollection<OrderDetail>("OrderDetail").AsQueryable() // lấy toàn bộ sp
                         join b in db.GetCollection<Product>("ProductDetails").AsQueryable() on l.ProductID equals b.Id
                         join c in db.GetCollection<Order>("Order").AsQueryable() on l.OrderID equals c.Id
                         join k in db.GetCollection<User>("User").AsQueryable() on c.CustomerID equals k.Id
                         where b.Status == true && k.Status == true && b.StarRated >= 2.5
                         select new { l.OrderID, l.ProductID, l.Price, l.Quantity, k.Id, b.ProductName, b.ProductImage, b.MetaTitle };

            foreach (var item in model)
            {
                foreach (var item0 in model1.Where(x => x.Id == uid))
                {
                    bool check = false;
                    foreach (var item1 in model1.Where(x => x.Id == item.Id))
                    {
                        if (item1.ProductID == item0.ProductID)
                        {
                            check = true;
                        }
                    }
                    foreach (var item1 in model1.Where(x => x.Id == item.Id))
                    {
                        if (check == true && item1 != item0)
                        {
                            Product temp = new Product();
                            temp.Id = item1.ProductID;
                            temp.ProductName = item1.ProductName;
                            temp.ProductImage = item1.ProductImage;
                            temp.MetaTitle = item1.MetaTitle;
                            temp.Price = item1.Price;
                            temp.Quantity = item1.Quantity;
                            if (!listLinks.Contains(listLinks.Find(x => x.Id == item1.ProductID)))
                                listLinks.Add(temp);
                        }
                    }
                }
            }

            return PartialView(listLinks.Take(4).Distinct().OrderByDescending(x => x.Price));

        }
    }
}