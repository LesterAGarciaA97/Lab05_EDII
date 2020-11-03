using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDeClases.Models
{
    public interface ICipherData <T>
    {
        IFormFile File { get; set; }
        T Key { get; set; }
    }
}
