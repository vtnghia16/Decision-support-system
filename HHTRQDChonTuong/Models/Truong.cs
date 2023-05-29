using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HHTRQDChonTuong.Models
{
    public partial class Truong
    {
        public Truong()
        {
            TruongNganh = new HashSet<TruongNganh>();
        }

        public string MaTruong { get; set; }
        public string TenTruong { get; set; }
        public string DiaChi { get; set; }
        public string Sdt { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string MoTa { get; set; }
        public double? CoHoiViecLam { get; set; }
        public int? HoatDongXaHoi { get; set; }
        public int? CoSoVatChat { get; set; }

        public virtual ICollection<TruongNganh> TruongNganh { get; set; }
    }
}
