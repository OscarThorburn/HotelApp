using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotelAppWeb.Pages
{
    public class BookRoomModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int RoomTypeId { get; set; }

        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public RoomTypeModel RoomType { get; set; }
        private IDatabaseData _db { get; }

        public BookRoomModel(IDatabaseData db)
        {
            _db = db;
        }
        public void OnGet()
        {
            if (RoomTypeId > 0)
            {
                RoomType = _db.GetRoomTypeById(RoomTypeId);
            }
        }

        public IActionResult OnPost()
        {
            _db.BookRoom(StartDate, EndDate, FirstName, LastName, RoomTypeId);
            return RedirectToPage("/Index");
        }
    }
}
