using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using testmongo.Models;

namespace testmongo.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(string id)
        {
            var cateID = new ObjectId(id);
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.Id == cateID);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Update(string cartModel)
        {
            var cateID = new ObjectId(cartModel);

            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.Id == item.Product.Id);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public ActionResult AddItem(string productId, int quantity)
        {
            var cateID = new ObjectId(productId);
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var DB = Client.GetDatabase("Employee");
            var product = DB.GetCollection<Product>("ProductDetails").Find(Builders<Product>.Filter.Where(s => s.Id == cateID)).SingleOrDefault();

            //var product = new ProductDetailDao().ViewDetail(productId);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.Id == cateID))
                {

                    foreach (var item in list)
                    {
                        if (item.Product.Id == cateID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                //Gán vào session
                Session[CartSession] = list;
            }
            else
            {
                //tạo mới đối tượng cart item
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gán vào session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public ObjectId Insertod(Order od)
        {
            //try
            //{
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            IMongoDatabase DB = Client.GetDatabase("Employee");
            var collection = DB.GetCollection<Order>("Order");
            collection.InsertOneAsync(od);
            return od.Id;
            //}
            //catch
            //{
            //    return null;
            //}
        }

        public bool Insertoddt(OrderDetail oddt)
        {
            try
            {
                MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
                IMongoDatabase DB = Client.GetDatabase("Employee");
                var collection = DB.GetCollection<OrderDetail>("OrderDetail");
                collection.InsertOneAsync(oddt);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email)
        {
            var order = new Order();
            order.CreateDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = mobile;
            order.ShipName = shipName;
            order.ShipEmail = email;

            try
            {
                var id = Insertod(order);
                var cart = (List<CartItem>)Session[CartSession];
                //var detailDao = new Model.Dao.OrderDetailDao();
                decimal total = 0;
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.Id;
                    orderDetail.OrderID = id;
                    orderDetail.Price = item.Product.Price;
                    orderDetail.Quantity = item.Quantity;
                    //detailDao.Insert(orderDetail);
                    Insertoddt(orderDetail);

                    total += (item.Product.Price * item.Quantity);
                }
            }
            catch (Exception ex)
            {
                //ghi log
                return Redirect("/loi-thanh-toan");
            }
            return Redirect("/hoan-thanh");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}