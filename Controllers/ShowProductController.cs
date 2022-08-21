using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using testmongo.Models;

namespace testmongo.Controllers
{
    public class ShowProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        //[ChildActionOnly]
        //public PartialViewResult ProductCategory()
        //{
        //    var model = new ProductCategoryDao().ListAll();
        //    return PartialView(model);
        //}

        public ActionResult Category(string cateId, int page = 1, int pageSize = 1)
        {
            var cateID = new ObjectId(cateId);

            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var DB = Client.GetDatabase("Employee");
            var Cate = DB.GetCollection<ProductCategory>("ProductCategory").Find(Builders<ProductCategory>.Filter.Where(s => s.Id == cateID)).SingleOrDefault();

            var collection = DB.GetCollection<Product>("ProductDetails").Find(Builders<Product>.Filter.Where(s => s.CategoryID == cateId)).ToList();
            ViewBag.Category = Cate;
            int totalRecord = 0;
            var model = collection.OrderByDescending(x => x.CreateDate).Skip((1 - 1) * 2).Take(2);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(collection);
        }

        public List<ProductViewModel> Search1(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            List<Product> listLinks = new List<Product>();
            MongoClient Client = new MongoClient("mongodb+srv://sa:sa@cluster0.pxuvg.mongodb.net/?retryWrites=true&w=majority");
            var db = Client.GetDatabase("Employee");
            var collectionpr = db.GetCollection<Product>("ProductDetails").Find(new BsonDocument()).ToList();
            totalRecord = db.GetCollection<Product>("ProductDetails").Find(Builders<Product>.Filter.Where(s => s.ProductName == keyword)).ToList().Count();

            //totalRecord = db.Products.Where(x => x.ProductName == keyword).Count();
            var model = (from b in db.GetCollection<ProductCategory>("ProductCategory").AsQueryable() // lấy toàn bộ sp
                         join a in db.GetCollection<Product>("ProductDetails").AsQueryable() on b.CategoryId equals a.CategoryID
                         where a.ProductName.Contains(keyword)
                         select new
                         {
                             CateMetaTitle = b.MetaTitle,
                             CateName = b.CategoryName,
                             CreateDate = a.CreateDate,
                             ID = a.Id,
                             Images = a.ProductImage,
                             Name = a.ProductName,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreateDate = x.CreateDate,
                             ID = x.ID.ToString(),
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price
                         });
            model.OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }

        public ActionResult Search(string keyword, int page = 1, int pageSize = 1)
        {
            int totalRecord = 0;
            var model = Search1(keyword, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            ViewBag.Keyword = keyword;
            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }

        //public ActionResult Detail(long id)
        //{
        //    var product = new ProductDao().ViewDetail(id);
        //    ViewBag.Category = new ProductCategoryDao().ViewDetail(product.CategoryID.Value);
        //    ViewBag.RelatedProducts = new ProductDao().ListRelatedProducts(id);
        //    return View(product);
        //}

    }
}