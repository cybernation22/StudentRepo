using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudentInfoEngine.CustomModels;
using StudentInfoEngine.Models;

namespace StudentInfoEngine.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly StudentsDBContext _context;

        public StudentController(StudentsDBContext context, IRepository repository)
        {
            _repository = repository;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<StudentView>>> GetStudents([FromBody] SearchCriteria criteria)
        {
            try
            {
                var dbStudents = _context.Students.Join(_context.Genders, st => st.GenderId, gend => gend.ID, (st, gend) => new StudentView()
                {
                    PrivateNumber = st.PrivateNumber,
                    FirstName = st.FirstName,
                    LastName = st.LastName,
                    BirthDate = st.BirthDate,
                    ID = st.ID,
                    GenderDesc = gend.Description
                });


                if (!string.IsNullOrWhiteSpace(criteria.PrivateNumber))
                    dbStudents = dbStudents.Where(x => x.PrivateNumber == criteria.PrivateNumber);
                if (criteria.BirthDateFrom != null)
                    dbStudents = dbStudents.Where(x => x.BirthDate > criteria.BirthDateFrom);
                if (criteria.BirthDateTo != null)
                    dbStudents = dbStudents.Where(x => x.BirthDate < criteria.BirthDateTo);

                var resultStudents = dbStudents
                        .OrderByDescending(x => x.ID).Skip(criteria.Page * criteria.PageSize).Take(criteria.PageSize);


                var studentsCount = await _context.Students.CountAsync();

                var data = new
                {
                    paginator = new
                    {
                        totalCount = studentsCount,
                        pageSize = criteria.PageSize,
                        currentPage = criteria.Page
                    },
                    students = await resultStudents.ToListAsync(),
                };

                return Ok(data);
            }

            catch(Exception ex)
            {
                return StatusCode(500, $"შეცდომა სერვერზე {ex.Message}");
            }
        }


        [HttpGet]
        public async Task<ActionResult<bool>> GetstudentByPn(string privateNumber)
        {
            try
            {
                var studentExists = await _context.Students.AnyAsync(x => x.PrivateNumber == privateNumber);

                return Ok(studentExists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"შეცდომა სერვერზე {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            try
            {
                var student = await _repository.FindById<Student>(id);

                if (student == null)
                {
                    return NotFound("სტუდენტი მოცემული პარამეტრით არ არსებობს");
                }


                return student;
            }

            catch(Exception ex)
            {
                return StatusCode(500, $"შეცდომა სერვერზე {ex.Message}");
            }


        }


        [HttpPut]
        public async Task<IActionResult> ModifyStudent(Student student)
        {
            var item  = await _context.Students.AnyAsync(x => x.PrivateNumber == student.PrivateNumber && x.ID != student.ID);
            if (item)
            {
                return BadRequest("პიროვნება მსგავსი პირადი ნომრით უკვე არსებობს");
            }
            

            try
            {
                await _repository.UpdateAsync<Student>(student);

                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"შეცდომა სერვერზე {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<ActionResult> PostStudent([FromBody] Student student)
        {
            try
            {
                await _repository.CreateAsync<Student>(student);
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"შეცდომა სერვერზე {ex.Message}");
            }

            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            try
            {
                var student = await _repository.FindById<Student>(id);

                if (student == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync<Student>(student);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"შეცდომა სერვერზე {ex.Message}");
            }


        }

        [HttpGet]
        public  async Task<ActionResult<bool>> AgeValidator(DateTime birthDate)
        {            
            return  Ok(DateTime.Now < birthDate.AddYears(16));
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gender>>> GetGender()
        {
            try
            {
                var gender = await _context.Genders.ToListAsync();

                return Ok(gender);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"შეცდომა სერვერზე {ex.Message}");
            }

        }


    }
}
