using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImageViewerWCFService
{
    public class ImageViewerWCFService : IImageViewerWCFService
    {
        public ObservableCollection<ImageData> GetImagesFromFolder(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                throw new ArgumentException("Folder path cannot be null or empty.", nameof(folderPath));
            }

            var images = new ObservableCollection<ImageData>();

            try
            {
                string[] allFiles = Directory.GetFiles(folderPath);

                foreach (var filePath in allFiles)
                {
                    string extension = Path.GetExtension(filePath).ToLower();

                    if (IsImageFile(extension))
                    {
                        ImageData imageDatas = LoadImageInfo(filePath);
                        images.Add(imageDatas);
                    }
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception("Invalid directory: " + ex.Message);
            }
            catch (IOException ex)
            {
                throw new Exception("An error occurred while accessing the folder: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new Exception("Permission denied: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred: " + ex.Message);
            }

            return images;
        }

        public byte[] GetImage(string imagePath, int selectedPageNumber)
        {
            byte[] imageArray = null;

            try
            {
                if (imagePath == null || File.Exists(imagePath) == false)
                    return null;

                var fileExtension = Path.GetExtension(imagePath).ToLower();

                if (fileExtension == ".tiff" || fileExtension == ".tif")
                    imageArray = GetTiffImageFrameBytes(imagePath, selectedPageNumber);
                else if (fileExtension == ".jfif")
                    imageArray = GetJfifImageFrameBytes(imagePath, selectedPageNumber);
                else
                    imageArray = File.ReadAllBytes(imagePath);

                GC.Collect();
                GC.WaitForPendingFinalizers();

                return imageArray;
            }
            catch (UriFormatException ex)
            {
                throw new UriFormatException("Invalid URI format: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred: " + ex.Message);
            }
            finally
            {
                imageArray = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private byte[] GetJfifImageFrameBytes(string imagePath, int selectedPageNumber)
        {
            byte[] imageArray;

            var bitmapDecoder = JpegBitmapDecoder.Create(new Uri(imagePath), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

            if (selectedPageNumber < 0 || selectedPageNumber >= bitmapDecoder.Frames.Count)
                return null;

            var bitmapFrame = bitmapDecoder.Frames[selectedPageNumber];

            using (MemoryStream ms = new MemoryStream())
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(bitmapFrame);
                encoder.Save(ms);
                imageArray = ms.ToArray();
                encoder = null;
            }

            return imageArray;
        }

        private byte[] GetTiffImageFrameBytes(string imagePath, int selectedPageNumber)
        {
            byte[] imageArray;

            var bitmapDecoder = TiffBitmapDecoder.Create(new Uri(imagePath), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

            if (selectedPageNumber < 0 || selectedPageNumber >= bitmapDecoder.Frames.Count)
                return null;

            var bitmapFrame = bitmapDecoder.Frames[selectedPageNumber];

            using (MemoryStream ms = new MemoryStream())
            {
                var encoder = new TiffBitmapEncoder();
                encoder.Frames.Add(bitmapFrame);
                encoder.Save(ms);
                imageArray = ms.ToArray();
                encoder = null;
            }

            return imageArray;
        }

        public ImageData LoadImageInfo(string imagePath)
        {
            ImageData imageData = new ImageData
            {
                Path = imagePath,
                Name = Path.GetFileName(imagePath)
            };

            var noOfPages = GetPageCount(imagePath);

            for (int pageNumber = 0; pageNumber < noOfPages; pageNumber++)
            {
                PageData page = new PageData
                {
                    PageName = $"Page {pageNumber + 1}",
                    DocumentName = imageData.Name,
                    PageIndex = pageNumber,
                    Path = imageData.Path,
                };

                imageData.Pages.Add(page);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return imageData;
        }

        public int GetPageCount(string filePath)
        {
            var fileExtension = Path.GetExtension(filePath).ToLower();
            if (fileExtension == ".tiff" || fileExtension == ".tif")
            {
                var tiffBitmapDecoder = new TiffBitmapDecoder(new Uri(filePath), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                return tiffBitmapDecoder.Frames.Count;
            }
            else if (fileExtension == ".jfif")
            {
                var jpegBitmapDecoder = new JpegBitmapDecoder(new Uri(filePath), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                return jpegBitmapDecoder.Frames.Count;
            }
            else
            {
                return 1;
            }
        }

        public bool IsImageFile(string filePath)
        {
            HashSet<string> validImageFileExtensions = InitializeImageFileExtension();

            string fileExtension = Path.GetExtension(filePath);

            if (validImageFileExtensions.Contains(fileExtension.ToLower()))
                return true;

            return false;
        }

        public HashSet<string> InitializeImageFileExtension()
        {
            HashSet<string> fileExtensions = new HashSet<string>
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".gif",
                ".bmp",
                ".tiff",
                ".tif",
                ".jfif"
            };

            return fileExtensions;
        }
    }
}
