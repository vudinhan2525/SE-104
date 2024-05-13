using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYDAILI.Pages
{
    public class YourDataModel : INotifyPropertyChanged
    {
        private string _maMatHang;
        public string MaMatHang
        {
            get { return _maMatHang; }
            set
            {
                _maMatHang = value;
                OnPropertyChanged(nameof(MaMatHang));
            }
        }

        private decimal _donGia;
        public decimal DonGia
        {
            get { return _donGia; }
            set
            {
                _donGia = value;
                OnPropertyChanged(nameof(DonGia));
                CalculateTotal();
            }
        }

        private int _soLuong;
        public int SoLuong
        {
            get { return _soLuong; }
            set
            {
                _soLuong = value;
                OnPropertyChanged(nameof(SoLuong));
                CalculateTotal();
            }
        }

        private decimal _thanhTien;
        public decimal ThanhTien
        {
            get { return _thanhTien; }
            set
            {
                _thanhTien = value;
                OnPropertyChanged(nameof(ThanhTien));
            }
        }

        // Phương thức tính toán tự động cho Thành tiền
        public void CalculateTotal()
        {
            // Chỉ tính toán Thành tiền nếu cả hai SoLuong và DonGia có giá trị
            if (SoLuong != 0 && DonGia != 0)
            {
                ThanhTien = DonGia * SoLuong;
            }
            else
            {
                ThanhTien = 0; // Thiết lập Thành tiền thành 0 nếu một trong hai SoLuong hoặc DonGia không có giá trị
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
