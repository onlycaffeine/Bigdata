using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testmongo.Models;
using testmongo.App_Start;
using testmongo.Areas.Common;
//using testmongo.Areas.Common;

namespace testmongo.Controllers
{

    public class HomeController : Controller
    {
        private MongoDBContext dBContext;
        private IMongoCollection<Employee> employeeCollection;

        public HomeController()
        {
            dBContext = new MongoDBContext();
            employeeCollection = dBContext.database.GetCollection<Employee>("Employee");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(string id)
        {
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var db = Client.GetDatabase("Employee");
            var collection = db.GetCollection<Employee>("EmployeeDetails").Find(new BsonDocument()).ToList();

            var productId = new ObjectId(id);
            var product = collection.AsQueryable<Employee>().SingleOrDefault(x => x.Id == productId);
            return View(product);
        }


        public ActionResult emplist(string searchString)
        {
            ViewBag.SearchString = searchString;

            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var db = Client.GetDatabase("Employee");

            if (!string.IsNullOrEmpty(searchString))
            {
                var collection = db.GetCollection<Employee>("EmployeeDetails");
                var model = collection.Find(Builders<Employee>.Filter.Where(s => s.Name.Contains(searchString))).ToList();
                return View(model);
            }

            else
            {
                var collection = db.GetCollection<Employee>("EmployeeDetails").Find(new BsonDocument()).ToList();
                return View(collection);
            }
        }

        [HttpPost]
        public ActionResult Index(Employee Emp) // create
        {
            try
            {
                MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
                IMongoDatabase DB = Client.GetDatabase("Employee");
                var collection = DB.GetCollection<Employee>("EmployeeDetails");
                collection.InsertOneAsync(Emp);
                return RedirectToAction("emplist");
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
            var collection = Db.GetCollection<Employee>("EmployeeDetails");

            var productId = new ObjectId(id);
            var product = collection.AsQueryable<Employee>().SingleOrDefault(x => x.Id == productId);
            return View(product);

        }

        [HttpPost]
        public ActionResult Edit(string id, Employee Empdet)
        {
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var Db = Client.GetDatabase("Employee");
            var collection = Db.GetCollection<Employee>("EmployeeDetails");
            try
            {
                //TODO: Add update logic here
                var filter = Builders<Employee>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<Employee>.Update
                    .Set("Name", Empdet.Name)
                    .Set("City", Empdet.City)
                    .Set("Address", Empdet.Address)
                    .Set("Department", Empdet.Department)
                    .Set("Country", Empdet.Country);

                var result = collection.UpdateMany(filter, update);
                return RedirectToAction("emplist");
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
            var collection = DB.GetCollection<Employee>("EmployeeDetails");

            var productId = new ObjectId(id);
            var product = collection.AsQueryable<Employee>().SingleOrDefault(x => x.Id == productId);
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
                var Collection = DB.GetCollection<Employee>("EmployeeDetails");
                Collection.DeleteOne(Builders<Employee>.Filter.Eq("_id", ObjectId.Parse(id)));
                return RedirectToAction("emplist");
            }
            catch
            {
                return View();
            }
        }
    }
}