using System.Collections.Generic;
using Couchbase;
using Couchcase.Models;
using Microsoft.AspNetCore.Mvc;

namespace Couchcase.Controllers
{
    public class HomeController : Controller
    {
        private readonly Repository _repo;
        private readonly string _bucketName;

        public HomeController()
        {
            _bucketName = "default";
            _repo = new Repository(ClusterHelper.GetBucket("default"));
        }

        public ActionResult Index()
        {
            var model = new MainModel
            {
                //TotalDocuments = _repo.GetNumDocuments(),
                MagicTen = _repo.GetMagicTen(),
                BucketName = _bucketName //,
                //Errors = (IDictionary<string,string>)TempData["FlashMessage"]
            };
            return View(model);
        }

        public RedirectToActionResult CreateMagic10()
        {
            _repo.CreateMagic10();
            return RedirectToAction("Index");
        }

        public RedirectToActionResult CreateArbitrary10()
        {
            var errors = _repo.CreateArbitrary(10);
            //TempData["FlashMessage"] = errors;
            return RedirectToAction("Index");
        }

        public RedirectToActionResult DeleteAllArbitrary()
        {
            _repo.DeleteAllArbitrary();
            return RedirectToAction("Index");
        }

        public RedirectToActionResult UpdateMagic10()
        {
            var errors = _repo.UpdateMagic10();
            //TempData["FlashMessage"] = errors;
            return RedirectToAction("Index");
        }

        public RedirectToActionResult DeleteDocument(string id)
        {
            _repo.DeleteDocument(id);
            return RedirectToAction("Index");
        }
    }
}