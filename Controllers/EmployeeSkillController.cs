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
    public class EmployeeSkillController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeSkillController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult AssignSkill(int employeeId)
        {
            var employee = _dbContext.Employees
                .Include(e => e.EmployeeSkills)
                .FirstOrDefault(e => e.Id == employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            // Retrieve skills from the database
            var availableSkills = _dbContext.Skills.ToList();

            var viewModel = new EmployeeSkill
            {
                EmployeeId = employeeId,
                Employee = employee,
                Rating = 1,
                YearsOfExperience = 1,
                AvailableSkills = availableSkills ?? new List<Skill>(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AssignSkill(EmployeeSkill employeeSkill)
        {
            List<Skill> availableSkills;

            if (ModelState.IsValid)
            {
                // Set Skill property based on the selected value
                employeeSkill.Skill = _dbContext.Skills.Find(employeeSkill.SkillId);

                // Ensure Skill is not null
                if (employeeSkill.Skill == null)
                {
                    ModelState.AddModelError("SkillId", "Invalid Skill selected.");
                    availableSkills = _dbContext.Skills.ToList();
                    employeeSkill.AvailableSkills = availableSkills ?? new List<Skill>();
                    return View(employeeSkill);
                }

                // Set Employee based on the EmployeeId
                employeeSkill.Employee = _dbContext.Employees.Find(employeeSkill.EmployeeId);

                // Ensure Employee is not null and set the EmployeeId
                if (employeeSkill.Employee != null)
                {
                    employeeSkill.EmployeeId = employeeSkill.Employee.Id;
                }
                else
                {
                    // Handle the case where the Employee is not found
                    ModelState.AddModelError("EmployeeId", $"Invalid EmployeeId selected: {employeeSkill.EmployeeId}");
                    availableSkills = _dbContext.Skills.ToList();
                    employeeSkill.AvailableSkills = availableSkills ?? new List<Skill>();
                    return View(employeeSkill);
                }

                // Add the employeeSkill to the context
                _dbContext.EmployeeSkills.Add(employeeSkill);
                _dbContext.SaveChanges();
                TempData["Success"] = "Skill Assigned successfully";

                return RedirectToAction("Index", "Employee");
            }

            // If the model state is not valid, redisplay the form with errors
            availableSkills = _dbContext.Skills.ToList();
            employeeSkill.AvailableSkills = availableSkills ?? new List<Skill>();

            return View(employeeSkill);
        }


        // public IActionResult ViewSkills(int employeeId)
        // {
        //     // Implement the logic to retrieve and pass the assigned skills for the employee
        //     var skills = _dbContext.EmployeeSkills
        //         .Where(es => es.EmployeeId == employeeId)
        //         .Select(es => es.Skill)
        //         .ToList();

        //     return View(skills);
        // }

        public IActionResult ViewSkills(int employeeId)
        {
            // Retrieve the skills for the specified employeeId
            var skills = _dbContext.EmployeeSkills
                .Include(es => es.Skill)  // Include the Skill navigation property
                .Where(es => es.EmployeeId == employeeId)
                .ToList();

            return View(skills);
        }


    }
}