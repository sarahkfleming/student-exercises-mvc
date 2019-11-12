﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentExercisesMVC.Models;

namespace StudentExercisesMVC.Controllers
{
    public class HomeController : Controller //inherits from ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Student> students = new List<Student>()
            {
                new Student()
                {
                    FirstName = "Lisa",
                    LastName = "Jones"
                },
                new Student()
                {
                    FirstName = "Roy",
                    LastName = "Batty"
                }
            };
            // View method comes from the parent class "Controller"
            return View(students);
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
