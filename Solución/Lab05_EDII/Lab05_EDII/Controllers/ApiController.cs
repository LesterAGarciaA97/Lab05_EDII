using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Metodo para cifrar, esto se realizara segun el valor que se ingrese desde la ruta
        /// </summary>
        /// <param name="method">Metodo de cifrado</param>
        /// <param name="file">Archivo a cifrar</param>
        /// <param name="key">Dependiendo del metodo se enviara el valor necesario</param>
        /// <returns></returns>
        [HttpPost("cipher/{method}")]
        public async Task<IActionResult> Cipher(string method, IFormFile file, [FromForm] Key key) {
            CreateDirectory();
            var path = "";
            var methodToUpper = method.ToUpper();
            switch (methodToUpper)
            {
                case "ZIGZAG": //.zz
                    ZigZag.Cifrar(file, key.levels);
                    var extZigZag = ".zz";
                    var PathZz = Environment.CurrentDirectory + "\\temp\\" + Path.GetFileNameWithoutExtension(file.FileName) + extZigZag;
                    path = PathZz;
                    break;
                case "CESAR": //.csr
                    Cesar.Cifrar(file, key.word);
                    var extCesar = ".csr";
                    var PathCsr = Environment.CurrentDirectory + "\\temp\\" + Path.GetFileNameWithoutExtension(file.FileName) + extCesar;
                    path = PathCsr;
                    break;
                case "CÉSAR":
                    Cesar.Cifrar(file, key.word);
                    var extCesar1 = ".csr";
                    var PathCsr2 = Environment.CurrentDirectory + "\\temp\\" + Path.GetFileNameWithoutExtension(file.FileName) + extCesar1;
                    path = PathCsr2;
                    break;
                case "RUTA": //.rt
                    Route.CifradoEspiral(file, key.columns, key.rows);
                    var extRoute = ".rt";
                    var PathRt = Environment.CurrentDirectory + "\\temp\\" + Path.GetFileNameWithoutExtension(file.FileName) + extRoute;
                    path = PathRt;
                    break;
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
                    break;
            }

            var Memory = new MemoryStream();
            using (var Stream = new FileStream(path, FileMode.Open))
            {
                await Stream.CopyToAsync(Memory);
            }
            Memory.Position = 0;
            var extensionFile = Path.GetExtension(path).ToLowerInvariant();
            DeleteDirectoy();
            return File(Memory, GetMimeTypes()[extensionFile], Path.GetFileName(path));

        }

        /// <summary>
        /// Metodo para decifrar, se realizara el decifrado segun la ruta que tenga el archivo 
        /// </summary>
        /// <param name="file">Archivo a Decifrar</param>
        /// <param name="key">Dependiendo del metodo se enviara el valor necesario</param>
        /// <returns></returns>
        [HttpPost("decipher")]
        public async Task<IActionResult> Decipher(IFormFile file ,[FromForm] Key key) {
            
            CreateDirectory();
            var ExtensionFile = "";
            ExtensionFile =  Path.GetExtension(file.FileName);
            switch (ExtensionFile)
            {
                case ".zz":
                    ZigZag.Decifrar(file, key.levels);
                    break;
                case ".csr":
                    Cesar.Decifrar(file, key.word);
                    break;
                case ".rt":
                    Route.DecifradoEspiral(file, key.columns, key.rows);
                    break;
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
                    break;
            }
            var path = Environment.CurrentDirectory + "\\temp\\" + Path.GetFileNameWithoutExtension(file.FileName) + ".txt";
            var Memory = new MemoryStream();
            using (var Stream = new FileStream(path, FileMode.Open))
            {
                await Stream.CopyToAsync(Memory);
            }
            Memory.Position = 0;
            var extensionFile = Path.GetExtension(path).ToLowerInvariant();
            DeleteDirectoy();
            return File(Memory, GetMimeTypes()[extensionFile], Path.GetFileName(path));
        }
        /// <summary>
        /// Procedimiento para eliminar la carpeta temporal
        /// </summary>
        void DeleteDirectoy() {
            Directory.Delete($"temp", true);
        }
        /// <summary>
        /// Procedimiento para crear la carpeta temporal
        /// </summary>
        void CreateDirectory()
        {
            Directory.CreateDirectory($"temp");
        }
        /// <summary>
        /// Metodo donde crea el diccionario con las diferentes extension, y con tipo de contenido de la extension
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetMimeTypes() {
            return new Dictionary<string, string> {
                {".txt","text/plain"},
                {".zz","text/plain"},
                {".csr","text/plain"},
                {".rt","text/plain"},
            };
        }
    }
}