using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testmongo.App_Start;     //for handling  error and including this page as start coz controller is everything manage
using MongoDB.Driver;
using testmongo.Models;
using MongoDB.Bson;

namespace testmongo.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Create()
        {
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var db = Client.GetDatabase("Employee");
            var categories = from c in db.GetCollection<ProductCategory>("ProductCategory").Find(new BsonDocument()).ToList() select c;
            ViewBag.ncc = new SelectList(categories, "Id", "CategoryName");
            return View();
        }

        private MongoDBContext dBContext;
        private IMongoCollection<Product> employeeCollection;

        public ProductController()
        {
            dBContext = new MongoDBContext();
            employeeCollection = dBContext.database.GetCollection<Product>("Product");
        }

        public ActionResult Detail(string id)
        {
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var db = Client.GetDatabase("Employee");
            var collection = db.GetCollection<Product>("ProductDetails").Find(new BsonDocument()).ToList();

            var productId = new ObjectId(id);
            var product = collection.AsQueryable<Product>().SingleOrDefault(x => x.Id == productId);
            return View(product);
        }

        public ActionResult Index(string searchString, string CategoryID, int minp = 0, int maxp = 0, string a = "", string b = "")
        {

            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var db = Client.GetDatabase("Employee");

            var categories = from c in db.GetCollection<ProductCategory>("ProductCategory").Find(new BsonDocument()).ToList() select c;
            ViewBag.CategoryID = new SelectList(categories, "Id", "CategoryName"); // danh sách Category
            ViewBag.SearchString = searchString;

            var model = from l in db.GetCollection<Product>("ProductDetails").Find(new BsonDocument()).ToList() // lấy toàn bộ sp
            //join c in db.GetCollection<ProductCategory>("ProductCategory").Find(new BsonDocument()).ToList() on l.CategoryID equals c.Id
                        select new { l.Id, l.ProductName, l.Price, l.ProductDescription, l.CategoryID, l.ProductImage, l.Quantity, l.CreateDate };

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.ProductName.Contains(searchString) || x.ProductName.Contains(searchString));
            }


            if (!string.IsNullOrEmpty(CategoryID))
                {
                model = model.Where(x => x.CategoryID == CategoryID);
            }

            if (!string.IsNullOrEmpty(a))
            {
                DateTime createdate1 = Convert.ToDateTime(a);
                DateTime createdate2 = Convert.ToDateTime(b);
                model = model.Where(x => x.CreateDate >= createdate1 && x.CreateDate <= createdate2);
            }

            if (minp != 0 && maxp != 0)
            {
                model = model.Where(x => x.Price >= minp && x.Price <= maxp);
            }

            if (minp != 0)
            {
                model = model.Where(x => x.Price >= minp);
            }

            if (maxp != 0)
            {
                model = model.Where(x => x.Price <= maxp);
            }

            List<Product> listLinks = new List<Product>();

            foreach (var item in model)
            {
                Product temp = new Product();
                temp.CategoryID = item.CategoryID;
                //temp.CategoryName = item.Name;
                temp.ProductDescription = item.ProductDescription;
                temp.Id = item.Id;
                temp.ProductName = item.ProductName;
                temp.Price = item.Price;
                temp.ProductImage = item.ProductImage;
                temp.Quantity = item.Quantity;
                temp.CreateDate = item.CreateDate;
                listLinks.Add(temp);
            }

            return View(listLinks);
        }

        [HttpPost]
        public ActionResult Create(Product Emp) // create
        {
            try
            {
                MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
                IMongoDatabase DB = Client.GetDatabase("Employee");
                var collection = DB.GetCollection<Product>("ProductDetails");
                collection.InsertOneAsync(Emp);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(string id)
        {
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var Db = Client.GetDatabase("Employee");
            var collection = Db.GetCollection<Product>("ProductDetails");

            var productId = new ObjectId(id);
            var product = collection.AsQueryable<Product>().SingleOrDefault(x => x.Id == productId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(string id, Product Empdet)
        {
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var Db = Client.GetDatabase("Employee");
            var collection = Db.GetCollection<Product>("ProductDetails");
            try
            {
                //TODO: Add update logic here
                var filter = Builders<Product>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<Product>.Update
                    .Set("ProductName", Empdet.ProductName)
                    .Set("Quantity", Empdet.Quantity)
                    .Set("Price", Empdet.Price)
                    .Set("ProductDescription", Empdet.ProductDescription);

                var result = collection.UpdateMany(filter, update);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult Delete(string id)
        {
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var DB = Client.GetDatabase("Employee");
            var collection = DB.GetCollection<Product>("ProductDetails");

            var productId = new ObjectId(id);
            var product = collection.AsQueryable<Product>().SingleOrDefault(x => x.Id == productId);
            return View(product);

        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
                var DB = Client.GetDatabase("Employee");
                var Collection = DB.GetCollection<Product>("ProductDetails");
                Collection.DeleteOne(Builders<Product>.Filter.Eq("_id", ObjectId.Parse(id)));
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}