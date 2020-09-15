using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using PupPlaceHolder.Controllers;
using System;
using Xunit;

namespace PupPlaceHolderUnitTest
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexWidthHeightTest()
        {
            IHostingEnvironment hostingEnv  = new HostingEnvironment() { WebRootPath = "C:\\Users\\Chigozie Joshua\\source\\repos\\PupPlaceHolder\\PupPlaceHolder\\wwwroot" };
            
            HomeController homeController = new HomeController(hostingEnv);
            string widthHeight = "450x50";
            var result = homeController.Index(widthHeight) as ViewResult;
            string imageSrc = "../images/selectedImage/" + widthHeight + ".jpg";
            Assert.Equal(imageSrc, result.ViewData.Model.ToString()); 
        }
    }
}
