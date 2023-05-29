using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HHTRQDChonTuong.Models
{
    public partial class LsketQua
    {
        public int Id { get; set; }
        public string Ipmac { get; set; }
        public DateTime? Ngay { get; set; }
        public string MaTruong { get; set; }
        public double? KetQua { get; set; }
        public int? Rank { get; set; }
    }
}
