﻿using BethanysPieShopAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BethanysPieShopAdmin.ViewModels
{
    public class PieAddViewModel
    {
        public IEnumerable<SelectListItem>? Categories { get; set; } = default!;
        public Pie? Pie { get; set; }
    }
}
