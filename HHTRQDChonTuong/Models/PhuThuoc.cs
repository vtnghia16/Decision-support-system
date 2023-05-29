using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HHTRQDChonTuong.Models
{
    public partial class PhuThuoc
    {
        public string MaTruong { get; set; }
        public string MaNganh { get; set; }
        public string MaNam { get; set; }
        public double? Diem { get; set; }
        public int? HocPhi { get; set; }

        public virtual Nam MaNamNavigation { get; set; }
        public virtual Nganh MaNganhNavigation { get; set; }
        public virtual Truong MaTruongNavigation { get; set; }
    }
}
