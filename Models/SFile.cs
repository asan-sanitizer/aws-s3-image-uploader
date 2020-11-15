using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Lab03.Models
{
    public class SFile
    {
        public IFormFile myFile { get; set; }
        public string comments { get; set; }

    }
}