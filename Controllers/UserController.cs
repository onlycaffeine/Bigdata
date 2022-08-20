using MongoDB.Bson;
using MongoDB.Driver;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testmongo.Areas.Common;
using testmongo.Models;

namespace testmongo.Controllers
{
    public class UserController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }

        public int Login1(string userName, string passWord, bool isLoginAdmin = false)
        {
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var DB = Client.GetDatabase("Employee");
            var result = DB.GetCollection<User>("User").Find(Builders<User>.Filter.Where(s => s.Username == userName)).SingleOrDefault();

            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == passWord)
                        return 1;
                    else
                        return -2;
                }
            }

        }

        [HttpPost]
        public ActionResult Login(LoginModel1 model)
        {
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var DB = Client.GetDatabase("Employee");

            if (ModelState.IsValid)
            {
                var result = Login1(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = DB.GetCollection<User>("User").Find(Builders<User>.Filter.Where(s => s.Username == model.UserName)).SingleOrDefault();
                    var userSession = new UserLogin();
                    userSession.UserName = user.Username;
                    userSession.UserID = user.Id.ToString();
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return Redirect("/");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khoá.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else
                {
                    ModelState.AddModelError("", "đăng nhập không đúng.");
                }
            }
            return View(model);
        }

        public ActionResult History()
        {
            List<OrderDetail> listLinks = new List<OrderDetail>();
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var db = Client.GetDatabase("Employee");

            var session = (UserLogin)Session[testmongo.Areas.Common.CommonConstants.USER_SESSION];
            var uid = new ObjectId(session.UserID);
            var model = from l in db.GetCollection<OrderDetail>("OrderDetail").AsQueryable() // lấy toàn bộ sp
                        join c in db.GetCollection<Order>("Order").AsQueryable() on l.OrderID equals c.Id
                        join k in db.GetCollection<User>("User").AsQueryable() on c.CustomerID equals k.Id
                        //where k.Id == uid
                        select new { l.Id, l.OrderID, l.ProductID, l.Price, l.Quantity, c.CustomerID};


            model = model.Where(x => x.CustomerID == uid);
            foreach (var item in model)
            {
                OrderDetail temp = new OrderDetail();
                temp.OrderID = item.OrderID;
                temp.ProductID = item.ProductID;
                temp.Price = item.Price;
                temp.Quantity = item.Quantity;
                listLinks.Add(temp);
            }

            return View(listLinks.OrderByDescending(x => x.Price));
        }

        public ActionResult Rate(string id)
        {
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var Db = Client.GetDatabase("Employee");
            var collection = Db.GetCollection<Product>("ProductDetails");

            var productId = new ObjectId(id);
            var product = collection.AsQueryable<Product>().SingleOrDefault(x => x.Id == productId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Rate(string id, Product Empdet)
        {
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var Db = Client.GetDatabase("Employee");
            var collection = Db.GetCollection<Product>("ProductDetails");
            try
            {
                //TODO: Add update logic here
                var filter = Builders<Product>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<Product>.Update
                    .Set("StarRated", Empdet.StarRated);

                var result = collection.UpdateMany(filter, update);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }
    }
}