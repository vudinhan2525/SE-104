using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYDAILI.Class
{
    public class Goods : INotifyPropertyChanged
    {
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }



        public string MaMatHang { get; set; }
        public string TenMatHang { get; set; }
        public int Gia { get; set; }
        public int SoLuong { get; set; }
        public string DonViTinh { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
 