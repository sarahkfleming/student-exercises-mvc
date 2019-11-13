using StudentExercisesMVC.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentExercisesMVC.Models
{
    public class Cohort
    {
        public int Id { get; set; }
        [Display(Name = "Cohort Name")]
        public string CohortName { get; set; }
        public List<Student> Students { get; set; }  = new List<Student>();
        public List<Instructor> Instructors { get; set; }  = new List<Instructor>();
    }
}