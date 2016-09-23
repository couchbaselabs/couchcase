using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Couchbase;
using Couchcase.Models;

namespace Couchcase.Controllers
{
    public class HomeController : Controller
    {
        private readonly Repository _repo;
        private readonly string _bucketName;

        public HomeController()
        {
            _bucketName = ConfigurationManager.AppSettings["CouchbaseBucketName"];
            _repo = new Repository(ClusterHelper.GetBucket(_bucketName));
        }

        public ActionResult Index()
        {
            var model = new MainModel
            {
                TotalDocuments = _repo.GetNumDocuments(),
                MagicTen = _repo.GetMagicTen(),
                BucketName = _bucketName,
                Errors = (IDictionary<string,string>)TempData["FlashMessage"]
            };
            return View(model);
        }

        public RedirectToRouteResult CreateMagic10()
        {
            _repo.CreateMagic10();
            return RedirectToAction("Index");
        }

        public RedirectToRouteResult CreateArbitrary10()
        {
            _repo.CreateArbitrary(10);
            return RedirectToAction("Index");
        }

        public RedirectToRouteResult DeleteAllArbitrary()
        {
            _repo.DeleteAllArbitrary();
            return RedirectToAction("Index");
        }

        public RedirectToRouteResult UpdateMagic10()
        {
            var errors = _repo.UpdateMagic10();
            TempData["FlashMessage"] = errors;
            return RedirectToAction("Index");
        }

        public RedirectToRouteResult DeleteDocument(string id)
        {
            _repo.DeleteDocument(id);
            return RedirectToAction("Index");
        }
    }
}