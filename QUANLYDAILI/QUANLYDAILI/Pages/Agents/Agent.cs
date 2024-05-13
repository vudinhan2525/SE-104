using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYDAILI.Pages.Agents
{
    public class Agent
    {
        public int MaDaiLy;
        public string TenDaiLy;
        public string SoDienThoai;
        public string Quan;
        public string Avatar;
        public string DiaChi;
        public int Loai;
        public string NgayTiepNhan;
        public decimal KhoanNo;
        public string Email;
        public Agent(int maDaiLy, string tenDaiLy, string soDienThoai, string quan, string avatar, string diaChi, int loai, string ngayTiepNhan, decimal khoanNo, string email)
        {
            MaDaiLy = maDaiLy;
            TenDaiLy = tenDaiLy;
            SoDienThoai = soDienThoai;
            Quan = quan;
            Avatar = avatar;
            DiaChi = diaChi;
            Loai = loai;
            NgayTiepNhan = ngayTiepNhan;
            KhoanNo = khoanNo;
            Email = email;
        }
    }
}
