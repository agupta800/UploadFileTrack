using File.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace File.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Retrieve all file models
            var files = _context.Files.ToList();

            // Pass the list of files to the view
            return View(files);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }


        //[HttpPost]
        //public IActionResult Upload(IFormFile file)
        //{
        //    if (file != null && file.Length > 0)
        //    {
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            file.CopyTo(memoryStream);

        //            var newFile = new FileModel
        //            {
        //                FileName = file.FileName,
        //                FileContent = memoryStream.ToArray()
        //            };

        //            _context.Files.Add(newFile);
        //            _context.SaveChanges();
        //        }
        //    }

        //    return RedirectToAction("Index"); // Redirect to file list or any other page
        //}

        //[HttpPost]
        //public IActionResult Upload([FromForm] FileModel model, IFormFile file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (file != null && file.Length > 0)
        //        {
        //            using (var memoryStream = new MemoryStream())
        //            {
        //                file.CopyTo(memoryStream);

        //                // Set the file properties in the model
        //                //model.FileName = file.FileName;
        //                model.FileContent = memoryStream.ToArray();

        //                _context.Files.Add(model);
        //                _context.SaveChanges();

        //                return RedirectToAction("Index");
        //            }
        //        }

        //        // Handle other properties of YourViewModel
        //        // Save, update, or process the data as needed

        //        return RedirectToAction("Success"); // Redirect to a success page or take appropriate action
        //    }

        //    // If ModelState is not valid, return to the form view with validation errors
        //    return View("Index", model);
        //}

        [HttpPost]
        public IActionResult Upload(FileModel model, IFormFile image)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Save the image to the server
            if (image != null && image.Length > 0)
            {
                // Sanitize the file name by replacing spaces with underscores
                var fileName = Path.GetFileNameWithoutExtension(image.FileName);
                var fileExtension = Path.GetExtension(image.FileName);
                var sanitizedFileName = $"{fileName.Replace(" ", "_")}_{DateTime.Now.Ticks}{fileExtension}";

                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sanitizedFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                // Set the Image property of the model to the correct image path
                model.Image = "/images/" + sanitizedFileName;
            }

            // Generate a tracking ID with 4 digits
            Random random = new Random();
            model.TrackingId = random.Next(1000, 9999).ToString();

            // Add the model to the context and save changes
            _context.Files.Add(model);
            _context.SaveChanges();

            // Redirect to Index action with the tracking ID as a route parameter
            return RedirectToAction("NewTrack", new { trackingId = model.TrackingId });
        }



        public IActionResult NewTrack(string trackingId)
        {
            // Retrieve the file model based on the tracking ID
            var model = _context.Files.SingleOrDefault(f => f.TrackingId == trackingId);

            if (model == null)
            {
                // Handle the case where the model is not found
                return NotFound();
            }

            // Pass the model to the view
            return View(model);
        }



        [HttpGet("Download/{id}")]
        public IActionResult Download(int id)
        {
            var fileModel = _context.Files.Find(id);

            if (fileModel != null)
            {
                // Determine content type based on file extension
                string contentType = GetContentType(fileModel.Image);

                if (contentType != null)
                {
                    // Return the file for download
                    return File(fileModel.Image, contentType, fileModel.Image);
                }
            }

            return NotFound();
        }

        [HttpGet("View/{id}")]
        public IActionResult View(int id)
        {
            var fileModel = _context.Files.Find(id);

            if (fileModel != null)
            {
                // Determine content type based on file extension
                string contentType = GetContentType(fileModel.Image);

                if (contentType != null)
                {
                    // Return the file for viewing in the browser
                    return File(fileModel.Image, contentType);
                }
            }

            return NotFound();
        }

        // Helper method to determine content type based on file extension
        private string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (provider.TryGetContentType(fileName, out var contentType))
            {
                return contentType;
            }

            return "application/octet-stream"; // If content type cannot be determined
        }


        //public IActionResult Download(int id)
        //{
        //    var file = _context.Files.Find(id);

        //    if (file == null)
        //        return NotFound();

        //    return File(file.Image, "application/octet-stream");
        //}
        //[HttpGet("View/{id}")]
        //public IActionResult View(int id)
        //{
        //    var fileModel = _context.Files.Find(id);

        //    if (fileModel != null)
        //    {
        //        // Determine the content type based on the file extension
        //        string contentType = GetContentType(fileModel.Image);

        //        if (contentType != null)
        //        {
        //            // Return the file with the determined content type
        //            return File(fileModel.Image, contentType);
        //        }
        //    }

        //    return NotFound();
        //}

        //// Helper method to determine content type based on file extension
        //private string GetContentType(string fileName)
        //{
        //    // You might need to implement a more comprehensive method to determine content type
        //    // This is a simple example, you may want to use a library or a more complex logic
        //    // to handle various file types.
        //    var provider = new FileExtensionContentTypeProvider();
        //    if (provider.TryGetContentType(fileName, out var contentType))
        //    {
        //        return contentType;
        //    }

        //    return null; // If content type cannot be determined
        //}

        [HttpPost]
        public IActionResult Approve(int id)
        {
            var model = _context.Files.Find(id);

            if (model != null)
            {
                model.IsApproved = true;
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { trackingId = model.TrackingId });
        }

        [HttpPost]
        public IActionResult Disapprove(int id)
        {
            var model = _context.Files.Find(id);

            if (model != null)
            {
                model.IsApproved = false;
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { trackingId = model.TrackingId });
        }
        //public IActionResult Track(string trackingId)
        //{
        //    // Retrieve the file model based on the tracking ID
        //    var model = _context.Files.SingleOrDefault(f => f.TrackingId == trackingId);

        //    if (model == null)
        //    {
        //        // Handle the case where the model is not found
        //        return NotFound("File not found.");
        //    }

        //    // Pass the model to the view
        //    return View(model);
        //}

        public IActionResult Track()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetStatus(string trackingId)
        {
            // Retrieve the file model based on the tracking ID
            var model = _context.Files.SingleOrDefault(f => f.TrackingId == trackingId);

            if (model == null)
            {
                // Handle the case where the model is not found
                ViewBag.ErrorMessage = "File not found. Please check the tracking ID.";
                return View("Track");
            }

            // Pass the model to the view
            return View("FileStatus", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
