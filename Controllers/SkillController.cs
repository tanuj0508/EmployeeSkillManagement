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
    public class SkillController : Controller
    {
      
       private readonly ApplicationDbContext _dbContext;

        public SkillController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Skill
        public IActionResult Index()
        {
            var skills = _dbContext.Skills.ToList();
            return View(skills);
        }

        // GET: Skill/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Skill/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SkillName")] Skill skill)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(skill);
                await _dbContext.SaveChangesAsync();
                TempData["Success"] = "Skill Created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(skill);
        }

        // GET: Skill/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skill = await _dbContext.Skills.FindAsync(id);

            if (skill == null)
            {
                return NotFound();
            }

            return View(skill);
        }

        // POST: Skill/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SkillName")] Skill skill)
        {
            if (id != skill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(skill);
                    await _dbContext.SaveChangesAsync();
                    TempData["Success"] = "Skill Updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkillExists(skill.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(skill);
        }

        // GET: Skill/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skill = await _dbContext.Skills
                .FirstOrDefaultAsync(m => m.Id == id);

            if (skill == null)
            {
                return NotFound();
            }

            return View(skill);
        }

        // POST: Skill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skill = await _dbContext.Skills.FindAsync(id);

            if (skill == null)
            {
                return NotFound();
            }

            _dbContext.Skills.Remove(skill);
            await _dbContext.SaveChangesAsync();
            TempData["Success"] = "Skill Deleted successfully";

            return RedirectToAction(nameof(Index));
        }

        private bool SkillExists(int id)
        {
            return _dbContext.Skills.Any(s => s.Id == id);
        }

       
    }
}