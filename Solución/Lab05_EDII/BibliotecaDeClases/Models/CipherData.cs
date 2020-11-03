using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDeClases.Models
{
    public class CipherData : ICipherData<string[]>
    {
        public IFormFile File { get; set; }
        public string[] Key { get; set; }

        public CipherData()
        {
            Key = new string[2];
        }
    }
}
