using StudentExercisesMVC.Models;
using System.Collections.Generic;


namespace StudentExercises
{
    public class Cohort
    {
        public int Id { get; set; }
        public string CohortName { get; set; }
        public List<Student> Students { get; set; }  = new List<Student>();
        public List<Instructor> Instructors { get; set; }  = new List<Instructor>();
    }
}