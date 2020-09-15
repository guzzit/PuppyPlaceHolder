using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PupPlaceHolder.Models;

namespace PupPlaceHolder.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostEnvironment;

        public HomeController(IHostingEnvironment HostEnvironment)
        {
            _hostEnvironment = HostEnvironment;
        }
        public IActionResult Index()
        {
            string imgSrc = "../images/defaultImage/logo.PNG";
            return View((object)imgSrc);
        }
        [ResponseCache(Duration = 3600, VaryByQueryKeys = new string[] { "widthHeight" }, Location = ResponseCacheLocation.Any)]
        [Route("/{widthHeight}")]
        public IActionResult Index(string widthHeight)
        {
            try
            {
                List<string> imageSizeString = new List<string>();
                if (widthHeight.Contains('x'))
                    imageSizeString = widthHeight.Split('x').ToList();
                else if (widthHeight.Contains('X'))
                    imageSizeString = widthHeight.Split('X').ToList();
                else if (widthHeight.Contains('×'))
                    imageSizeString = widthHeight.Split('×').ToList();

                if (imageSizeString.Count != 2)
                    return RedirectToAction("Error", new { message = "Values should not be more than two: width and height." });

                int width = 0; int height = 0;
                string widthString = imageSizeString[0];
                string heightString = imageSizeString[1];
                bool canConvertWidth = int.TryParse(widthString, out width);
                bool canConvertHeight = int.TryParse(heightString, out height);

                if (!canConvertHeight || !canConvertWidth)
                    return RedirectToAction("Error", new { message = "Values should be numerical." });

                var rand = new Random();
                var puppyImageList = Directory.GetFiles(_hostEnvironment.WebRootPath + @"\images", "*.jpg");
                Image randomPuppyImage = Image.FromFile(puppyImageList[rand.Next(puppyImageList.Length)]);
                var destRect = new Rectangle(0, 0, width, height);
                var destImage = new Bitmap(width, height);

                destImage.SetResolution(randomPuppyImage.HorizontalResolution, randomPuppyImage.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(randomPuppyImage, destRect, 0, 0, randomPuppyImage.Width, randomPuppyImage.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                var selectedImage = _hostEnvironment.WebRootPath + @"\images\selectedImage\" + widthHeight + ".jpg";
                var outputStream = new MemoryStream();
                destImage.Save(selectedImage, ImageFormat.Jpeg);
                string imgSrc = "../images/selectedImage/" + widthHeight + ".jpg";
                return View((object)imgSrc);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
                throw;
            }
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message="")
        {
            return View(new ErrorViewModel { Message = message });
        }
    }
}
