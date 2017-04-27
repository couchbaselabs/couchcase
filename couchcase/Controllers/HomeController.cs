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
                MagicTen = _repo.GetMagicTen()
            };
            return View(model);
        }

        public RedirectToActionResult CreateMagic10()
        {
            _repo.CreateMagic10();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateMagic10()
        {
            var errors = _repo.UpdateMagic10();
            var model = new MainModel
            {
                MagicTen = _repo.GetMagicTen(),
                Errors = errors
            };
            return View("Index", model);
        }

        public RedirectToActionResult DeleteDocument(string id)
        {
            _repo.DeleteDocument(id);
            return RedirectToAction("Index");
        }

        public RedirectToActionResult TouchDocument(string id)
        {
            _repo.TouchDocument(id);
            return RedirectToAction("Index");
        }

    }
}