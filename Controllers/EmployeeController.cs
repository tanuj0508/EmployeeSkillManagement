using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
   
   [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Employee
        public IActionResult Index()
        {
            var employees = _dbContext.Employees.ToList();
            return View(employees);
        }


        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,DateOfJoining,Designation,Email")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(employee);
                await _dbContext.SaveChangesAsync();
                TempData["Success"] = "Employee Created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }



        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _dbContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,DOJ,Designation,Email")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(employee);
                    await _dbContext.SaveChangesAsync();
                    TempData["Success"] = "Employee Updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index"); // Use action name as a string
            }

            return View(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _dbContext.Employees.Any(e => e.Id == id);
        }

        // Get: Employee/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Retrieve the employee by ID
            var employee = _dbContext.Employees.Find(id);

            if (employee == null)
            {
                // Handle the case where the employee is not found
                return NotFound();
            }

            return View(employee);
        }
        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var employee = _dbContext.Employees.Find(id);

            if (employee == null)
            {
                // Handle the case where the employee is not found
                return NotFound();
            }

            _dbContext.Employees.Remove(employee);
            _dbContext.SaveChanges();
            TempData["Success"] = "Employee Deleted successfully";

            return RedirectToAction("Index"); // Redirect to the index page after deletion
        }


    }
}