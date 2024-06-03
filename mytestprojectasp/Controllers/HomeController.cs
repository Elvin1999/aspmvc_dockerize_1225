using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using mytestprojectasp.Models;
using System.Diagnostics;

namespace mytestprojectasp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileProvider _fileProvider;

        public HomeController(ILogger<HomeController> logger, IFileProvider fileProvider)
        {
            _logger = logger;
            _fileProvider = fileProvider;
        }

        public IActionResult Index()
        {
            var images=_fileProvider.GetDirectoryContents("wwwroot/images").Select(x=>x.Name);
            return View(images);
        }

        public IActionResult ImageSave()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImageSave(IFormFile imageFile)
        {
            if(imageFile!=null && imageFile.Length > 0)
            {
                var fileName=Guid.NewGuid().ToString()+Path.GetExtension(imageFile.FileName);
                var path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/images", fileName);

                using (var stream =new FileStream(path,FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
