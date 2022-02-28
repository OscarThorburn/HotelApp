using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelAppWeb.Pages
{
    public class RoomSearchModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]     
        public DateTime StartDate { get; set; } = DateTime.Now;

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);

        [BindProperty(SupportsGet = true)]
        public bool SearchEnabled { get; set; } = false;

        public List<RoomTypeModel> AvailableRoomTypes { get; set; }
        private readonly IDatabaseData _db;

        public RoomSearchModel(IDatabaseData db)
        {
            _db = db;
        }
        public void OnGet()
        {
            if(SearchEnabled == true && EndDate > StartDate)
            {
                AvailableRoomTypes = _db.GetAvailableRoomTypes(StartDate, EndDate);
            }
        }


        public IActionResult OnPost()
        {
            return RedirectToPage(new 
            { 
                SearchEnabled = true, 
                StartDate = StartDate.ToString("yyyy-MM-dd"), 
                EndDate = EndDate.ToString("yyyy-MM-dd"),
            });
        }
    }
}
