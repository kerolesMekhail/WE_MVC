using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WE_Test.Business.Abstract;
using WE_Test.Data.Model;
using WE_Test.Helper;
using System.Linq.Dynamic.Core;
namespace WE_Test.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly IEmployee _repository;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployee repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            ViewBag.newEmployees = _repository.GetAll().Where(x => x.Type == "1").ToList().Count();
            ViewBag.probationaryStaff = _repository.GetAll().Where(x => x.Type == "2").ToList().Count();
            ViewBag.existingEmployees = _repository.GetAll().Where(x => x.Type == "3").ToList().Count();
            ViewBag.localEmployeesForPension = _repository.GetAll().Where(x => x.Type == "4").ToList().Count();
            return View();
        }
        public IActionResult EmployeeCount()
        {
            ViewBag.newEmployees = _repository.GetAll().Where(x=>x.Type=="1").ToList().Count();
            ViewBag.probationaryStaff = _repository.GetAll().Where(x=>x.Type=="2").ToList().Count();
            ViewBag.existingEmployees = _repository.GetAll().Where(x=>x.Type=="3").ToList().Count();
            ViewBag.localEmployeesForPension = _repository.GetAll().Where(x=>x.Type=="4").ToList().Count();
            return PartialView();
        }
        public IActionResult Add()
        {
            ViewBag.TypesDDL = TypeDDL();
            return View();
        }
        [HttpPost]
        public IActionResult Add(emp_data emp_Data)
        {
            try
            {
                _repository.Add(emp_Data);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(emp_Data);
            }
        } 
        public IActionResult Edit(int Id)
        {
            try
            {
                var Emp = _repository.GetById(Id);
                ViewBag.TypesDDL = TypeDDL();
                return View(Emp);

            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit(emp_data emp_Data)
        {
            try
            {
                _repository.Update(emp_Data);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(emp_Data);
            }
        }
        [HttpPost]
        public IActionResult GetEmployees(string type ,string someValue)
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];
            IQueryable<emp_data> Employees = _repository.GetAllEmps().Where(x=>x.Type==type).Where(m => string.IsNullOrEmpty(searchValue)
                ? true
                : (m.Type.Contains(searchValue) || m.Emp_name.Contains(searchValue) || m.Emp_job.Contains(searchValue)));
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                Employees = Employees.OrderBy(string.Concat(sortColumn, " ", sortColumnDirection));
            var data = Employees.Skip(skip).Take(pageSize).ToList();
            var recordsTotal = Employees.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Json(jsonData);
        }

        public IActionResult Delete(int Id)
        {
            try
            {
                var Emp = _repository.GetById(Id);
                if (Emp != null)
                {
                    _repository.Delete(Emp);

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
