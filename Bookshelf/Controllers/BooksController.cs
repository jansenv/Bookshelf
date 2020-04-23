using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookshelf.Data;
using Bookshelf.Models;
using Bookshelf.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bookshelf.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Books
        public async Task<ActionResult> Index()
        {
            return View();
        }

        // GET: Books/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View();
        }

        // GET: Books/Create
        public async Task<ActionResult> Create()
        {
            var genreOptions = await _context.Genres
                .Select(g => new SelectListItem() { Text = g.Name, Value = g.Id.ToString() })
                .ToListAsync();

            var viewModel = new BookFormViewModel();

            viewModel.GenreOptions = genreOptions;

            return View(viewModel);
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookFormViewModel bookViewModel)
        {
            try
            {
                var user = await GetCurrentUserAsync();

                var book = new Book
                {
                    Title = bookViewModel.Title,
                    Author = bookViewModel.Author,
                    ApplicationUserId = user.Id,
                };

                book.BookGenres = bookViewModel.SelectGenreIds.Select(genreId => new BookGenre()
                {
                    Book = book,
                    GenreId = genreId
                }).ToList();

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: Books/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            //var viewModel = new BookFormViewModel();
            //var book = await _context.BookGenres.FirstOrDefaultAsync(b => b.Id == id)
            //var genreOptions = await _context
            //    .Genres.Select(g => new SelectListItem()
            //    {
            //        Text = g.Name,
            //        Value = g.Id.ToString()
            //    }).ToListAsync();

            //_context.BookGenres.Add(book);
            //await _context.SaveChangesAsync();

            return View();
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Books/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return View();
        }

        // POST: Books/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task<ApplicationUser> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}