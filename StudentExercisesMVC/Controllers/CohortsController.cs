using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StudentExercises;
using StudentExercisesMVC.Models;
using StudentExercises.Models.ViewModels;


namespace StudentExercisesMVC.Controllers
{
    public class CohortsController : Controller
    {
        private readonly IConfiguration _config;
        public CohortsController(IConfiguration config)
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

        // GET: Cohorts
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, CohortName
                                                            FROM Cohort";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();
                    while (reader.Read())
                    {
                        Cohort cohort = new Cohort()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                        };

                        cohorts.Add(cohort);
                    }
                    reader.Close();
                    return View(cohorts);
                }
            }
        }

        // GET: Cohorts/Details/5
        public ActionResult Details(int id)
        {
            var cohort = GetById(id);
            return View(cohort);
        }

        // GET: Cohorts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cohorts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Cohorts/Edit/5
        public ActionResult Edit(int id)
        {
            var cohort = GetById(id);
            return View(cohort);
        }

        // POST: Cohorts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Cohorts/Delete/5
        public ActionResult Delete(int id)
        {
            var cohort = GetById(id);
            return View(cohort);
        }

        // POST: Cohorts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private Cohort GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @" SELECT c.Id, c.CohortName
                                                            FROM Cohort c
                                                            WHERE id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Cohort theCohort = null;
                    if (reader.Read())
                    {
                        theCohort = new Cohort
                        {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                        };
                    }

                    reader.Close();
                    return theCohort;
                }
            }
        }
    }
}