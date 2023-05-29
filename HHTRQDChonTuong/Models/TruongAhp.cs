using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HHTRQDChonTuong.Models
{
    public partial class TruongAhp
    {
        public string Id { get; set; }
        public double? HocPhi { get; set; }
        public string MaTruong { get; set; }
        public double? CoHoiViecLam { get; set; }
        public double? HoatDongXaHoi { get; set; }
        public double? CoSoVatChat { get; set; }
    }
}
