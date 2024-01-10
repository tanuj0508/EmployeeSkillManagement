using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class EmployeeSkill
    {
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public int SkillId { get; set; }
        public Skill? Skill { get; set; }

        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10.")]
        public int Rating { get; set; }

        [Range(1, 20, ErrorMessage = "Years of Experience must be between 1 and 20.")]
        public int YearsOfExperience { get; set; }

        public List<Skill>? AvailableSkills { get; set; }
    }
}