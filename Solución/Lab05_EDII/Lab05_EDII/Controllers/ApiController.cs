using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab05_EDII.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDeClases.Cifrados;
using System.IO;

namespace Lab05_EDII.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        Cesar CSR = new Cesar();
        ZigZag ZZ = new ZigZag();
        Route RT = new Route();
        [HttpPost("cipher/{method}")]
        public async Task<IActionResult> Cipher(string method, IFormFile file, [FromForm] Key key) {
            CreateDirectory();
            var path = "";
            var methodToUpper = method.ToUpper();
            switch (methodToUpper)
            {
                case "ZIGZAG": //.zz
                    ZigZag.Cifrar(file, key.Levels);
                    var extZigZag = ".zz";
                    var PathZz = Environment.CurrentDirectory + "\\temp\\" + Path.GetFileNameWithoutExtension(file.FileName) + extZigZag;
                    break;
                case "CESAR": //.csr
                    Cesar.Cifrar(file, key.Word);
                    var extCesar = ".csr";
                    var PathCsr = Environment.CurrentDirectory + "\\temp\\" + Path.GetFileNameWithoutExtension(file.FileName) + extCesar;
                    path = PathCsr;
                    break;
                case "CÉSAR":
                    Cesar.Cifrar(file, key.Word);
                    var extCesar1 = ".csr";
                    var PathCsr2 = Environment.CurrentDirectory + "\\temp\\" + Path.GetFileNameWithoutExtension(file.FileName) + extCesar1;
                    path = PathCsr2;
                    break;
                case "RUTA": //.rt
                    Route.CifradoEspiral(file, key.Rows, key.Columns);
                    var extRoute = ".rt";
                    var PathRt = Environment.CurrentDirectory + "\\temp\\" + Path.GetFileNameWithoutExtension(file.FileName) + extRoute;
                    path = PathRt;
                    break;
                default:
                    break;
            }

            var Memory = new MemoryStream();
            using (var Stream = new FileStream(path,FileMode.Open))
            {
                await Stream.CopyToAsync(Memory);
            }
            Memory.Position = 0;
            var extensionFile = Path.GetExtension(path).ToLowerInvariant();
            return File(Memory, GetMimeTypes()[extensionFile], Path.GetFileName(path));
            
        }
        void CreateDirectory()
        {
            Directory.CreateDirectory($"temp");
        }
        private Dictionary<string, string> GetMimeTypes() {
            return new Dictionary<string, string> {
                {".txt","text/plain"},
                {".zz","text/plain"},
                {".csr","text/plain"},
                {".rt","text/plain"},
            };
        }
        //
    }
}
