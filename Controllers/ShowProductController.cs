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

            //var category = new CategoryDao().ViewDetail(cateId);
            ViewBag.Category = Cate;
            int totalRecord = 0;
            //var model = new ProductDao().ListByCategoryId(cateId, ref totalRecord, page, pageSize);
            var model = collection.OrderByDescending(x => x.CreateDate).Skip((1 - 1) * 2).Take(2);

            //totalRecord = collection.Count();
            //var model = (from a in DB.GetCollection<Product>("ProductDetails").Find(Builders<Product>.Filter.Where(s => s.CategoryID == cateId)).ToList()
            //             where a.CategoryID == cateId
            //             select new
            //             {
            //                 CateMetaTitle = a.MetaTitle,
            //                 //CateName = a.ProductName,
            //                 CreateDate = a.CreateDate,
            //                 //ID = a.Id,
            //                 Images = a.ProductImage,
            //                 Name = a.ProductName,
            //                 MetaTitle = a.MetaTitle,
            //                 Price = a.Price
            //             }).AsEnumerable().Select(x => new ProductViewModel()
            //             {
            //                 CateMetaTitle = x.MetaTitle,
            //                 CateName = x.Name,
            //                 CreateDate = x.CreateDate,
            //                 //ID = x.ID,
            //                 Images = x.Images,
            //                 Name = x.Name,
            //                 MetaTitle = x.MetaTitle,
            //                 Price = x.Price
            //             });
            //model.OrderByDescending(x => x.CreateDate).Skip((1 - 1) * pageSize).Take(pageSize);
            //var ok = model.ToList();




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

            //return View(ok);
            return View(collection);
        }

        //public ActionResult Search(string keyword, int page = 1, int pageSize = 1)
        //{
        //    int totalRecord = 0;
        //    var model = new ProductDao().Search(keyword, ref totalRecord, page, pageSize);

        //    ViewBag.Total = totalRecord;
        //    ViewBag.Page = page;
        //    ViewBag.Keyword = keyword;
        //    int maxPage = 5;
        //    int totalPage = 0;

        //    totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
        //    ViewBag.TotalPage = totalPage;
        //    ViewBag.MaxPage = maxPage;
        //    ViewBag.First = 1;
        //    ViewBag.Last = totalPage;
        //    ViewBag.Next = page + 1;
        //    ViewBag.Prev = page - 1;

        //    return View(model);
        //}

        ////[OutputCache(CacheProfile = "Cache1DayForProduct")]
        //public ActionResult Detail(long id)
        //{
        //    var product = new ProductDao().ViewDetail(id);
        //    ViewBag.Category = new ProductCategoryDao().ViewDetail(product.CategoryID.Value);
        //    ViewBag.RelatedProducts = new ProductDao().ListRelatedProducts(id);
        //    return View(product);
        //}
    }
}