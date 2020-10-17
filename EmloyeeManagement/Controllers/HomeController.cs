using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment hostEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IWebHostEnvironment hostEnvironment)
        {
            this._employeeRepository = employeeRepository;
            this.hostEnvironment = hostEnvironment;
        }

        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployees();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            Employee model = _employeeRepository.GetEmployeeById(id);
            if (model == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound");
            }
            ViewBag.PageTitle = "View from Details Page";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
           return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photos!=null && model.Photos.Count>0)
                {
                    uniqueFileName = ProcessUploadFiles(model);

                }
                var employee = new Employee()
                {
                    Name = model.Name,
                    Department = model.Department,
                    Email = model.Email,
                    PhotoPath = uniqueFileName
                };
                _employeeRepository.Add(employee);
                return RedirectToAction("details", new { id = employee.ID });
            }
            return View();
        }

        private string ProcessUploadFiles(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            foreach (var photo in model.Photos)
            {
                string uploadFolder = Path.Combine(hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(filestream);
                }
            }

            return uniqueFileName;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var employee = _employeeRepository.GetEmployeeById(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                ID = employee.ID,
                Name = employee.Name,
                Department = employee.Department,
                Email = employee.Email,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = _employeeRepository.GetEmployeeById(model.ID);
                employee.Name = model.Name;
                employee.Department = model.Department;

                if (model.Photos != null && model.Photos.Count > 0)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        System.IO.File.Delete(Path.Combine(hostEnvironment.WebRootPath, "images", model.ExistingPhotoPath));
                    }
                    employee.PhotoPath = ProcessUploadFiles(model);
                }
                Employee updatedEmployee = _employeeRepository.Update(employee);
                return RedirectToAction("index");
            }
            return View(model);
        }
    }
}
