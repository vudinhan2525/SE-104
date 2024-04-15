using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QUANLYDAILI.Pages.GoodsImport;

namespace QUANLYDAILI.Pages
{
    public class GoodsViewModel : INotifyPropertyChanged
    {
        private int _maPhieuNhap;
        public int MaPhieuNhap
        {
            get { return _maPhieuNhap; }
            set
            {
                _maPhieuNhap = value;
                OnPropertyChanged(nameof(MaPhieuNhap));
            }
        }

        private DateTime _ngayLapPhieu;
        public DateTime NgayLapPhieu
        {
            get { return _ngayLapPhieu; }
            set
            {
                _ngayLapPhieu = value;
                OnPropertyChanged(nameof(NgayLapPhieu));
            }
        }

        private ObservableCollection<YourDataModel> _yourDataItems;
        public ObservableCollection<YourDataModel> YourDataItems
        {
            get { return _yourDataItems; }
            set
            {
                _yourDataItems = value;
                OnPropertyChanged(nameof(YourDataItems));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
