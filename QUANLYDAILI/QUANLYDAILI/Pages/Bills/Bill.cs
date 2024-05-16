using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYDAILI.Pages.Bills
{   
    public class Bill
    {
        public int MaPhieuThu;
        public int MaDaiLy;
        public string TenDaiLy;
        public string Avatar;
        public string DiaChi; 
        public string SoDienThoai;
        public string Email;
        public decimal SoTienThu;
        public string NgayThu;
        public Bill(int mpt,int mdl,string tdl,string ava,string dc,string sdt,string em,decimal stt,string nt)
        {
            MaPhieuThu = mpt;
            MaDaiLy = mdl;
            TenDaiLy = tdl; 
            Avatar = ava;
            DiaChi = dc;
            SoDienThoai = sdt;
            Email = em;
            SoTienThu = stt;
            NgayThu = nt;
        }
    }
}
