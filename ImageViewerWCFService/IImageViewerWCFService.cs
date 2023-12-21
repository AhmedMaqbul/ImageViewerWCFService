using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Windows.Media.Imaging;

namespace ImageViewerWCFService
{
    [ServiceContract]
    public interface IImageViewerWCFService
    {
        [OperationContract]
        ObservableCollection<ImageData> GetImagesFromFolder(string folderPath);

        [OperationContract]
        byte[] GetImage(string imagePath, int pageNumber);
    }

    [DataContract]
    public class ImageData
    {
        private string _name;
        private string _path;
        private ObservableCollection<PageData> _pages = new ObservableCollection<PageData>();

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public string Path
        {
            get => _path;
            set => _path = value;
        }

        [DataMember]
        public ObservableCollection<PageData> Pages
        {
            get => _pages;
            set => _pages = value;
        }
    }

    [DataContract]
    public class PageData
    {
        private string _documentName;
        private string _pageName;
        private string _path;
        private int _pageIndex;

        [DataMember]
        public string DocumentName
        {
            get => _documentName;
            set => _documentName = value;
        }

        [DataMember]
        public string PageName
        {
            get => _pageName;
            set => _pageName = value;
        }

        [DataMember]
        public string Path
        {
            get => _path;
            set => _path = value;
        }

        [DataMember]
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value;
        }
    }

    [DataContract]
    public class PageMetadata
    {
        private int _pixelWidth;
        private int _pixelHeight;
        private double _dpiX;
        private double _dpiY;
        private string _pixelFormat;
        private BitmapPalette _palette;
        public int _stride;

        [DataMember]
        public int PixelWidth
        {
            get => _pixelWidth;
            set => _pixelWidth = value;
        }

        [DataMember]
        public int PixelHeight
        {
            get => _pixelHeight;
            set => _pixelHeight = value;
        }

        [DataMember]
        public double DpiX
        {
            get => _dpiX;
            set => _dpiX = value;
        }

        [DataMember]
        public double DpiY
        {
            get => _dpiY;
            set => _dpiY = value;
        }

        [DataMember]
        public string PixelFormat
        {
            get => _pixelFormat;
            set => _pixelFormat = value;
        }

        [DataMember]
        public BitmapPalette Palette
        {
            get => _palette;
            set => _palette = value;
        }

        [DataMember]
        public int Stride
        {
            get => _stride;
            set => _stride = value;
        }
    }
}
