using StudentExercisesMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentExercisesMVC.Models
{
    public class Instructor 
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Slack Handle")]
        public string SlackHandle { get; set; }

        [Required]
        public int CohortId { get; set; }
        public Cohort Cohort { get; set; }
        public void AssignExercise(Student student, Exercise exercise)
       {
           student.Exercises.Add(exercise);
       }
    }
}