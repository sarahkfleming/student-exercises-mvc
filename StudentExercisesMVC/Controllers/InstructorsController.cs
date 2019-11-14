using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StudentExercises;
using StudentExercises.Models.ViewModels;
using StudentExercisesMVC.Models;

namespace InstructorExercisesMVC.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly IConfiguration _config;
        public InstructorsController(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: Instructor
        // Index is a "Get All" type of method
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @" SELECT i.Id, i.FirstName, i.LastName, i.SlackHandle, i.CohortId,
                                                                            c.Id, c.CohortName
                                                            FROM Instructor i
                                                            LEFT JOIN Cohort c ON i.CohortId = c.Id";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Instructor> instructors = new List<Instructor>();
                    while (reader.Read())
                    {
                        Instructor instructor = new Instructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = new Cohort()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                            }
                        };

                        instructors.Add(instructor);
                    }

                    reader.Close();

                    return View(instructors);
                }
            }
        }

        // GET: Instructor/Details/5
        // Details is a "Get One" type of method
        public ActionResult Details(int id)
        {
            var instructor = GetInstructorById(id);
            return View(instructor);
        }

        // GET: Instructor/Create
        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new InstructorCreateViewModel()
            {
                Cohorts = GetAllCohorts()
            };
            //var cohorts = GetAllCohorts();
            //var selectItems = cohorts
            //    .Select(cohort => new SelectListItem
            //    {
            //        Text = cohort.CohortName,
            //        Value = cohort.Id.ToString()
            //    })
            //    .ToList();

            //selectItems.Insert(0, new SelectListItem
            //{
            //    Text = "Choose cohort...",
            //    Value = "0"
            //});
            //viewModel.Cohorts = selectItems;
            return View(viewModel);
        }

        // POST: Instructor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InstructorCreateViewModel viewModel)
        {
            try
            {
                var newInstructor = viewModel.Instructor;
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Instructor
                ( FirstName, LastName, SlackHandle, CohortId )
                VALUES
                ( @firstName, @lastName, @slackHandle, @cohortId )";
                        cmd.Parameters.Add(new SqlParameter("@firstName", newInstructor.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@lastName", newInstructor.LastName));
                        cmd.Parameters.Add(new SqlParameter("@slackHandle", newInstructor.SlackHandle));
                        cmd.Parameters.Add(new SqlParameter("@cohortId", newInstructor.CohortId));
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Instructor/Edit/5
        public ActionResult Edit(int id)
        {
            var viewModel = new InstructorEditViewModel()
            {
                Instructor = GetInstructorById(id),
                Cohorts = GetAllCohorts()
            };
            return View(viewModel);
        }

        // POST: Instructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, InstructorEditViewModel viewModel)
        {
            var updatedInstructor = viewModel.Instructor;
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Instructor 
                            SET FirstName = @FirstName,
                             LastName = @LastName,
                             SlackHandle = @SlackHandle,
                             CohortId = @CohortId
                                    WHERE Id = @Id";
                        cmd.Parameters.Add(new SqlParameter("@FirstName", updatedInstructor.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", updatedInstructor.LastName));
                        cmd.Parameters.Add(new SqlParameter("@SlackHandle", updatedInstructor.SlackHandle));
                        cmd.Parameters.Add(new SqlParameter("@CohortId", updatedInstructor.CohortId));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                viewModel = new InstructorEditViewModel()
                {
                    Instructor = updatedInstructor,
                    Cohorts = GetAllCohorts()
                };

                return View(viewModel);
            }
        }

        // GET: Instructor/Delete/5
        public ActionResult Delete(int id)
        {
            var instructor = GetInstructorById(id);
            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM Instructor WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // Create private method to get a instructor by Id so that you can use it in multiple places within the controller
        private Instructor GetInstructorById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @" SELECT i.Id, i.FirstName, i.LastName, i.SlackHandle, i.CohortId,
                                                            c.Id, c.CohortName
                                                            FROM Instructor i
                                                            LEFT JOIN Cohort c ON i.CohortId = c.Id
                                                            WHERE i.id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Instructor theInstructor = null;
                    if (reader.Read())
                    {
                        theInstructor = new Instructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = new Cohort()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                            }
                        };
                    }

                    reader.Close();
                    return theInstructor;
                }
            }
        }
        private List<Cohort> GetAllCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, CohortName FROM Cohort";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();
                    while (reader.Read())
                    {
                        cohorts.Add(new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName")),
                        });
                    }

                    reader.Close();

                    return cohorts;
                }
            }
        }
    }
}