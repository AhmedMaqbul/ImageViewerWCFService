using ImageViewerWCFService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageViewerServiceUnitTest
{
    [TestClass]
    public class ImageViewerServiceUnitTest
    {
        public string EnvironmentVariableName = "CommonDataTestPath";
        public string EnvironmentVariableValue = @"C:\Users\maqbul.ahmed\source\repos\ImageViewerWCFService\ImageViewerServiceUnitTest\DataTest\";

        public string EnvironmentVariable()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableName, EnvironmentVariableValue);
            string commonDataTestPath = Environment.GetEnvironmentVariable(EnvironmentVariableName);
            return commonDataTestPath;
        }

        public dynamic Bitmapsource()
        {
            var pixelFormat = PixelFormats.Bgr24;
            int width = 100;
            int height = 50;
            byte[] pixels = new byte[width * height * (pixelFormat.BitsPerPixel / 8)];
            new Random().NextBytes(pixels);
            var validBitmapSource = BitmapSource.Create(width, height, 96, 96, pixelFormat, null, pixels, width * (pixelFormat.BitsPerPixel / 8));

            return validBitmapSource;
        }

        [TestMethod]
        [DataRow(@"C:\Ahmed\ImageViewerWpfApp\ImageViewerUnitTest\DataTest\Multipage Document\Combined_Document.tiff")]
        public void LoadImageInfo_Returns_GivenImageData(string imagePath)
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            var actualImageData = viewerService.LoadImageInfo(imagePath);
            var imageInfo = actualImageData;

            Assert.AreEqual("Combined_Document.tiff", imageInfo.Name);
            Assert.AreEqual(imagePath, imageInfo.Path);
            Assert.AreEqual(5, imageInfo.Pages.Count);
        }

        [TestMethod]
        public void LoadImageInfo_ValidImage_PathAndPageCount()
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document 2.tiff";

            var imageData = viewerService.LoadImageInfo(imagePath);

            Assert.AreEqual(imagePath, imageData.Path);
            Assert.AreEqual("Combined_Document 2.tiff", imageData.Name);
            Assert.AreEqual(3, imageData.Pages.Count);
        }

        [TestMethod]
        [DataRow("\\Mixed\\366.jpeg")]
        public void LoadImageInfo_ValidImagePath_ReturnsImageDataWithCorrectProperties(string randomImagePath)
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();
            string validImagePath = EnvironmentVariable() + randomImagePath;

            var imageData = viewerService.LoadImageInfo(validImagePath);

            Assert.AreEqual(validImagePath, imageData.Path);
            Assert.AreEqual("366.jpeg", imageData.Name);

            Assert.IsNotNull(imageData.Pages);
            Assert.AreEqual("Page 1", imageData.Pages[0].PageName);
            Assert.AreEqual("366.jpeg", imageData.Pages[0].DocumentName);
            Assert.AreEqual(0, imageData.Pages[0].PageIndex);
            Assert.AreEqual(1, imageData.Pages.Count);
            Assert.AreEqual(validImagePath, imageData.Pages[0].Path);
        }

        [TestMethod]
        [DataRow(".jpg")]
        [DataRow(".jpeg")]
        [DataRow(".png")]
        [DataRow(".gif")]
        [DataRow(".bmp")]
        [DataRow(".tiff")]
        [DataRow(".tif")]
        [DataRow(".jfif")]
        public void InitializeImageFileExtensionHashSet_OnlyContainsAndReturnsHashSetCollections_ImageFileExtensions(string randomFileExtension)
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            Assert.IsTrue(viewerService.InitializeImageFileExtension().Contains(randomFileExtension));
        }

        [TestMethod]
        [DataRow(".cs")]
        [DataRow(".txt")]
        [DataRow(".pdf")]
        public void InitializeImageFileExtensionHashSet_OnlyContainsAndReturnsHashSetCollections_ImageFilesExtensionNotOtherFileExtensions(string randomFileExtension)
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            Assert.IsFalse(viewerService.InitializeImageFileExtension().Contains(randomFileExtension));
        }

        [TestMethod]
        [DataRow(".jpg")]
        [DataRow(".jpeg")]
        [DataRow(".png")]
        [DataRow(".gif")]
        [DataRow(".bmp")]
        [DataRow(".tiff")]
        [DataRow(".tif")]
        [DataRow(".jfif")]
        public void IsImageFile_ReturnTrue_OfValidExtension(string randomFile)
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            var actualResult = viewerService.IsImageFile(randomFile);

            Assert.AreEqual(true, actualResult);
        }

        [TestMethod]

        [DataRow(".cs")]
        [DataRow(".txt")]
        [DataRow(".pdf")]
        public void IsImageFile_ReturnFalse_OfValidExtension(string randomFile)
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            var actualResult = viewerService.IsImageFile(randomFile);

            Assert.AreEqual(false, actualResult);
        }

        [TestMethod]
        [DataRow("Pictures\\")]
        [DataRow("NoPictures\\")]
        [DataRow("Empty\\")]
        public void GetImagesFromFolder_AlwaysPrepareNewImageCollection_WhetherImageCollectionContainsImagesOrNotOrEmpty(string randomFolderName)
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();


            string folderPath = EnvironmentVariable() + randomFolderName;

            var images = viewerService.GetImagesFromFolder(folderPath);
            int imagesCount = images.Count;

            Assert.AreEqual(imagesCount, images.Count);
        }

        [TestMethod]
        public void GetImagesFromFolder_FilteredAndPrepareOnlyImageCollection_WhetherFolderContainsMixedFiles()
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            string folderPath = EnvironmentVariable() + @"Mixed\";
            var images = viewerService.GetImagesFromFolder(folderPath);

            Assert.AreEqual(3, images.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid directory: The specified path is invalid.")]
        public void GetImagesFromFolder_InvalidDirectory_Exception()
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            var actualResult = viewerService.GetImagesFromFolder("InvalidPath");

            Assert.AreEqual("Invalid directory: The specified path is invalid.", actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "An error occurred while accessing the folder: Access to the path 'InvalidPath' is denied.")]
        public void GetImagesFromFolder_IOError_Exception()
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            var actualResult = viewerService.GetImagesFromFolder("InvalidPath");

            Assert.AreEqual("An error occurred while accessing the folder: Access to the path 'InvalidPath' is denied.", actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Permission denied: Access to the path 'InvalidPath' is denied.")]
        public void GetImagesFromFolder_UnauthorizedAccessException_Exception()
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            var actualResult = viewerService.GetImagesFromFolder("InvalidPath");

            Assert.AreEqual("Permission denied: Access to the path 'InvalidPath' is denied.", actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "An error occurred: Something went wrong.")]
        public void GetImagesFromFolder_GeneralException_Exception()
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            var actualResult = viewerService.GetImagesFromFolder("InvalidPath");

            Assert.AreEqual("An error occurred: Something went wrong.", actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Folder path cannot be null or empty.")]
        public void GetImagesFromFolder_NullFolderPath_Exception()
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            var actualResult = viewerService.GetImagesFromFolder(null);

            Assert.AreEqual("Folder path cannot be null or empty.", actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Folder path cannot be null or empty.")]
        public void GetImagesFromFolder_StringEmptyFolderPath_Exception()
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            var actualResult = viewerService.GetImagesFromFolder(string.Empty);

            Assert.AreEqual("Folder path cannot be null or empty.", actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Folder path cannot be null or empty.")]
        public void GetImagesFromFolder_EmptyStringFolderPath_Exception()
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            var actualResult = viewerService.GetImagesFromFolder("");

            Assert.AreEqual("Folder path cannot be null or empty.", actualResult);
        }

        [TestMethod]
        [DataRow(" ")]
        [DataRow("___")]
        public void GetImagesFromFolder_ShouldThrowException_InvalidPath(string randomPath)
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            Assert.ThrowsException<Exception>(() => viewerService.GetImagesFromFolder(randomPath));
        }


        [TestMethod]
        [DataRow("c:\\abc\\")]
        [DataRow("_\\_\\_")]
        public void GetImagesFromFolder_ShouldThrowException_InvalidDirectory(string randomPath)
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            var exception = Assert.ThrowsException<Exception>(() => viewerService.GetImagesFromFolder(randomPath));

            StringAssert.Contains(exception.Message, "Invalid directory:");
        }

        //[TestMethod]
        //public void BitmapSourceToArray_ValidBitmapSource_ReturnsCorrectByteArray()
        //{
        //    var viewerService = new ImageViewerWCFService.ImageViewerWCFService();


        //    var pixelFormat = PixelFormats.Bgr24;
        //    int width = 100;
        //    int height = 50;
        //    byte[] pixels = new byte[width * height * (pixelFormat.BitsPerPixel / 8)];
        //    new Random().NextBytes(pixels);
        //    var validBitmapSource = BitmapSource.Create(width, height, 96, 96, pixelFormat, null, pixels, width * (pixelFormat.BitsPerPixel / 8));

        //    byte[] resultPixels = viewerService.BitmapSourceToArray(validBitmapSource);

        //    Assert.IsNotNull(resultPixels);
        //    Assert.AreEqual((width * height * (pixelFormat.BitsPerPixel / 8)), resultPixels.Length);
        //    CollectionAssert.AreEqual(pixels, resultPixels);
        //}

        //[TestMethod]
        //public void GetPagesFromImage_ValidImagePathAndPageNumber_ReturnsCorrectByteArray()
        //{
        //    var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

        //    var pixelFormat = PixelFormats.Bgr24;
        //    int width = 100;
        //    int height = 50;
        //    byte[] pixels = new byte[width * height * (pixelFormat.BitsPerPixel / 8)];
        //    new Random().NextBytes(pixels);
        //    var validBitmapSource = BitmapSource.Create(width, height, 96, 96, pixelFormat, null, pixels, width * (pixelFormat.BitsPerPixel / 8));

        //    string tempFilePath = Path.GetTempFileName();
        //    using (var stream = new FileStream(tempFilePath, FileMode.Create))
        //    {
        //        var encoder = new BmpBitmapEncoder();
        //        encoder.Frames.Add(BitmapFrame.Create(validBitmapSource));
        //        encoder.Save(stream);
        //    }

        //    try
        //    {
        //        var resultStream = viewerService.GetImage(tempFilePath, 0);

        //        CollectionAssert.AreEqual(pixels, resultStream);
        //    }
        //    finally
        //    {
        //        File.Delete(tempFilePath);
        //    }
        //}

        [TestMethod]
        public void GetPagesFromImage_ReturnsNull_InavlidSelectedPageNumber()
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            string validImagePath = EnvironmentVariable() + @"\Mixed\\366.jpeg";

            var actualResult = viewerService.GetImage(validImagePath, -1);

            Assert.AreEqual(null, actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException), "Invalid URI format: The URI format is not valid.")]
        public void GetPagesFromImage_InvalidUriFormat_Exception()
        {
            var viewerService = new ImageViewerWCFService.ImageViewerWCFService();

            var actualResult = viewerService.GetImage("validImagePath.jpg", 0);

            Assert.AreEqual("Invalid URI format: The URI format is not valid.", actualResult);
        }
    }

}
