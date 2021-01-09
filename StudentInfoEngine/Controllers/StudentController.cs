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

        var someval = dbStudents
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
                students = await someval.ToListAsync(),
            };

            return Ok(data);
        }


        [HttpGet]
        public async Task<ActionResult<bool>> GetstudentByPn(string privateNumber)
        {
            var studentExists = await _context.Students.AnyAsync(x => x.PrivateNumber == privateNumber);

            return Ok(studentExists);
        }

        [HttpGet]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _repository.FindById<Student>(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }


        [HttpPut]
        public async Task<IActionResult> ModifyStudent(Student student)
        {
            await _repository.UpdateAsync<Student>(student);

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult> PostStudent([FromBody] Student student)
        {
            await _repository.CreateAsync<Student>(student);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var student = await _repository.FindById<Student>(id);

            if (student == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync<Student>(student);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<bool>> AgeValidator(DateTime birthDate)
        {
            return Ok(DateTime.Now < birthDate.AddYears(16));
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gender>>> GetGender()
        {
            var gender = await _context.Genders.ToListAsync();

            return Ok(gender);
        }


    }
}
