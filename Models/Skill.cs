using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? SkillName { get; set; }

        public List<EmployeeSkill>? EmployeeSkills { get; set; }
    }
}