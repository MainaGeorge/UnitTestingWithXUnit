using Library.API.Data.Models;
using LibraryApp.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LibraryApp.Controllers
{
    public class BooksController : Controller
    {
        public readonly IBookService Service;
        public BooksController(IBookService service)
        {
            Service = service;
        }

        // GET: Shopping
        public ActionResult Index()
        {
            var result = Service.GetAll();
            return View(result);
        }

        // GET: Shopping/Details/5
        public ActionResult Details(Guid id)
        {
            var result = Service.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: Shopping/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Shopping/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book item)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                Service.Add(item);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: Shopping/Delete/5
        public ActionResult Delete(Guid id)
        {
            var result = Service.GetById(id);
            return View(result);
        }

        // POST: Shopping/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                Service.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}