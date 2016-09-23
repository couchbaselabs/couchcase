using System.Collections.Generic;
using System.Web.Mvc;
using Couchbase;
using Couchcase.Models;

namespace Couchcase.Controllers
{
    public class HomeController : Controller
    {
        private readonly Repository _repo;

        public HomeController()
        {
            _repo = new Repository(ClusterHelper.GetBucket("default"));
        }

        public ActionResult Index()
        {
            var model = new MainModel
            {
                TotalDocuments = _repo.GetNumDocuments(),
                MagicTen = _repo.GetMagicTen(),
                BucketName = _repo.BucketName,
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