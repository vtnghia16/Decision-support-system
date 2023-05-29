using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HHTRQDChonTuong.Models
{
    public partial class Nganh
    {
        public Nganh()
        {
            TruongNganh = new HashSet<TruongNganh>();
        }

        public string MaNganh { get; set; }
        public string TenNganh { get; set; }

        public virtual ICollection<TruongNganh> TruongNganh { get; set; }
    }
}
