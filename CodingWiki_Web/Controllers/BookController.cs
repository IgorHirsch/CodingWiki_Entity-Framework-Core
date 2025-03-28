﻿using CodingWiki_DataAccess.Data;
using CodingWiki_Model.Models;
using CodingWiki_Model.ViewModel;
using CodingWiki_Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.Arm;

namespace CodingWiki_Web.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            //List<Book> objList = _db.Books.Include(u => u.Publisher).ToList();

            // Eager Loading mit Include, um alle benötigten Daten auf einmal zu laden.
            List<Book> objList = _db.Books.Include(u => u.Publisher)
                                                                .Include(u => u.BookAuthorMap)
                                                                .ThenInclude(u => u.Author)
                                                                .ToList();



            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            BookVM obj = new();

            obj.PublisherList = _db.Publishers.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Publisher_Id.ToString()
            });

            if (id == null || id == 0)
            {
                //create
                return View(obj);
            }
            //edit
            obj.Book = _db.Books.FirstOrDefault(u => u.BookId == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(BookVM obj)
        {
            if (obj.Book.BookId == 0)
            {
                //create
                await _db.Books.AddAsync(obj.Book);
            }
            else
            {
                //update
                _db.Books.Update(obj.Book);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            BookDetail obj = new();

            //edit
            obj = _db.BookDetails.Include(u => u.Book).FirstOrDefault(u => u.Book_Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(BookDetail obj)
        {
            if (obj.BookDetail_Id == 0)
            {
                //create
                await _db.BookDetails.AddAsync(obj);
            }
            else
            {
                //update
                _db.BookDetails.Update(obj);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            Book obj = new();
            obj = _db.Books.FirstOrDefault(u => u.BookId == id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Books.Remove(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Playground()
        {
            // Holt ALLE Bücher aus der Datenbank und filtert dann in der Anwendung
            IEnumerable<Book> BookList1 = _db.Books; // Lädt alle Bücher in den Speicher(RAM)
            var FilteredBook1 = BookList1.Where(b => b.Price > 50).ToList(); // Filtert erst nach dem Laden (in Memory)


            // Erstellt eine Datenbankabfrage mit dem Filter
            IQueryable<Book> BookList2 = _db.Books; // Erstellt eine Abfrage, aber lädt noch nichts
            var FilteredBook2 = BookList2.Where(b => b.Price > 50).ToList(); // Filtert direkt in der Datenbank (SQL)




            return RedirectToAction(nameof(Index));
        }


        public IActionResult ManageAuthors(int id)
        {
            BookAuthorVM obj = new()
            {
                BookAuthorList = _db.BookAuthorMaps.Include(u => u.Author)
                                                                                    .Include(u => u.Book)
                                                                                    .Where(u => u.BookId == id).ToList(),
                BookAuthor = new()
                {
                    BookId = id
                },
                Book = _db.Books.FirstOrDefault(u => u.BookId == id)
            };


            List<int> tempListOfAssignedAuthor = obj.BookAuthorList.Select(u => u.Author_Id).ToList();

            //NOT IN clause
            //get all the authors whos id is not in tempListOfAssignedAuthors

            var tempList = _db.Authors.Where(u => !tempListOfAssignedAuthor.Contains(u.Author_Id)).ToList();


            obj.AuthorList = tempList.Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.Author_Id.ToString()
            });

            return View(obj);
        }

        [HttpPost]
        public IActionResult ManageAuthors(BookAuthorVM bookAuthorVM)
        {
            if (bookAuthorVM.BookAuthor.BookId != 0 && bookAuthorVM.BookAuthor.Author_Id != 0)
            {
                _db.BookAuthorMaps.Add(bookAuthorVM.BookAuthor);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookAuthorVM.BookAuthor.BookId });
        }




        [HttpPost]
        public IActionResult RemoveAuthors(int authorId, BookAuthorVM bookAuthorVM)
        {
            int bookId = bookAuthorVM.Book.BookId;
            BookAuthorMap bookAuthorMap = _db.BookAuthorMaps.FirstOrDefault(u => u.Author_Id == authorId && u.BookId == bookId);


            _db.BookAuthorMaps.Remove(bookAuthorMap);
            _db.SaveChanges();
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookId });
        }
    }
}