using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(100)]
        public string? Designation { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public List<EmployeeSkill>? EmployeeSkills { get; set; }
    }
}