using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Lab03.Amazon.DynamoDB;
using Lab03.Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lab03.Models;

namespace Lab03.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private List<SFile> filesLists = new List<SFile>();
        private S3Adapter _s3_adapter;
        private DynamoDBAdapter _db_adapter;

        public HomeController(ILogger<HomeController> logger )
        {
            _logger = logger;
            _s3_adapter = new S3Adapter();
            _db_adapter = new DynamoDBAdapter();
        }

        // [HttpPut]
        public IActionResult Login()
        {
            //authorize here 
            return View();
        }

        // [HttpPost]
        // public IActionResult Login(User user)
        // {
        //     if (_db_adapter.isAuthenticated(user.username, user.password))
        //     {
        //         return View("UploadFile");
        //     }
        //
        //     return View("Login");
        // }

        [HttpGet]
        public IActionResult Upload()
        {
            //display the list of all uploaded files using dynamodb 
            return View("UploadFile");
        }

        [HttpPost]
        public IActionResult Upload(SFile file)
        {
            //lets find out if the file exist 
            if (file.myFile.Name != null)
            {
                var fullPath = Path.GetFullPath(file.myFile.FileName);
                Debug.WriteLine("file : " + fullPath);
                
                filesLists.Add(file);
                
                // upload the file to amazon s3 
                _s3_adapter.uploadFile(fullPath);
                
                // save the link of the file to dynamodb 
                _db_adapter.insertFile( file.myFile.FileName, file.comments);
                
            }
            
            return View("Home",filesLists);
        }
    }
}