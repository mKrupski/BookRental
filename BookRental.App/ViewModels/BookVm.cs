using System.Collections.Generic;
using BookRental.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookRental.App.ViewModels
{
    public class BookVm
    {
        public Book Book { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
