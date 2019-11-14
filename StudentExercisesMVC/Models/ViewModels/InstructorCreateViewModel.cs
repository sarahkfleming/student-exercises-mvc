using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesMVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace StudentExercises.Models.ViewModels
{
    public class StudentCreateViewModel
    {
        public Student Student { get; set; }
        public List<Cohort> Cohorts { get; set; } = new List<Cohort>();
        public List<SelectListItem> CohortOptions
        {
            get
            {
                if (Cohorts == null) return null;

                return Cohorts
                    .Select(cohort => new SelectListItem
                    {
                        Text = cohort.CohortName,
                        Value = cohort.Id.ToString()
                    })
                    .ToList();

            }
        }
    }
}