using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestPlayground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IHostingEnvironment host;
        public FileUploadController(IHostingEnvironment host)
        {
            this.host = host;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }      

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file) {
            Console.WriteLine("***" + file);
            if(file == null) return BadRequest("NULL FILE");
            if(file.Length == 0) return BadRequest("Empty File");
            Console.WriteLine("***" + host.WebRootPath);
            if (string.IsNullOrWhiteSpace(host.WebRootPath))
            {
                host.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
            var uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath)) Directory.CreateDirectory(uploadsFolderPath);
            var fileName = "Master" + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
               await file.CopyToAsync(stream);
            }
            return Ok("Okay");
        }      
    }
}
