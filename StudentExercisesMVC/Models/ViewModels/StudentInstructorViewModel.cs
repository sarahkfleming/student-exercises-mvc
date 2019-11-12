using StudentExercisesMVC.Models;
using System.Collections.Generic;

namespace StudentExercises.Models.ViewModels
{
    public class StudentInstructorViewModel
    {
        public List<Student> Students { get; set; }
        public List<Instructor> Instructors { get; set; }
    }
}
