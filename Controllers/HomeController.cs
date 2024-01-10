using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using EmployeeManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers;

[Authorize]
public class HomeController : Controller
{
     private readonly ApplicationDbContext _dbContext;

    public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        }

    public IActionResult Index()
    {
        var employees = _dbContext.Employees.Include(e => e.EmployeeSkills)
                    .ThenInclude(es => es.Skill).ToList(); 
        return View(employees);
    }

    public IActionResult Search(string searchTerm, string searchCriteria)
        {
            try
            {
                // Your search logic here
                var employees = _dbContext.Employees
                    .Include(e => e.EmployeeSkills)
                    .ThenInclude(es => es.Skill)
                    .Where(e =>
                        (string.IsNullOrEmpty(searchCriteria) || searchCriteria == "Name") &&
                        (EF.Functions.Like(e.FirstName, $"%{searchTerm}%") || EF.Functions.Like(e.LastName, $"%{searchTerm}%")) ||
                        (searchCriteria == "Skill" && e.EmployeeSkills.Any(es => EF.Functions.Like(es.Skill.SkillName, $"%{searchTerm}%"))) ||
                        (searchCriteria == "Designation" && EF.Functions.Like(e.Designation, $"%{searchTerm}%"))
                    )
                    .ToList();

                return PartialView("_SearchResults", employees);
            }
            catch (Exception ex)
            {
                // Log the exception for further investigation (you might want to use a logging framework)
                Console.WriteLine(ex.Message);

                // Return a 500 status code with an error message
                return StatusCode(500, "Internal Server Error");
            }
        }

   
}
