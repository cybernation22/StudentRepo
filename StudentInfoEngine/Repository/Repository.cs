using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace StudentInfoEngine.Models
{
    public class Repository : IRepository
    {
        private readonly StudentsDBContext _context;


        public Repository(StudentsDBContext context)
        {
            _context = context;
        }

        public async Task CreateAsync<T>(T entity) where T : class
        {
            this._context.Set<T>().Add(entity);

            _ = await this._context.SaveChangesAsync();
        }

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            this._context.Set<T>().Remove(entity);

            _ = await this._context.SaveChangesAsync();
        }

        public async Task<List<T>> FindAll<T>() where T : class
        {
            return await this._context.Set<T>().ToListAsync();

        }

        public async Task<T> FindById<T>(int id) where T : class
        {
            return await this._context.Set<T>().FindAsync(id);

        }
     

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            this._context.Set<T>().Update(entity);

            _ = await this._context.SaveChangesAsync();
        }
    }
}





        //public StudentRepository(StudentsDBContext context)
        //{
        //    _context = context;
        //}

        //public async Task<Student> GetSingle(int id)
        //{
        //    return await _context.students.FirstOrDefaultAsync(x => x.ID == id);
        //}

        //public  async Task<bool> GetByPn(string pn)
        //{
        //    return await _context.students.AnyAsync(x => x.PrivateNumber == pn);
        //}

        //public void Add(Student item)
        //{
        //     _context.students.Add(item);
        //}

        //public void Delete(int id)
        //{
        //    Student student=  GetSingle(id).Result;
        //    _context.students.Remove(student);
        //}

        //public async Task<Student> Update(Student item)
        //{
        //    _context.students.Update(item);
        //    return item;
        //}

        //public async Task<IQueryable<Student>> GetAll(SearchCriteria queryParameters)
        //{
        //    IQueryable<Student> _allItems = _context.students;



        //    if (!string.IsNullOrEmpty(queryParameters.PrivateNumber))
        //        _allItems= _allItems.Where(x => x.PrivateNumber == queryParameters.PrivateNumber);
        //    if (queryParameters.BirthDateFrom != null)
        //        _allItems= _allItems.Where(x => x.BirthDate > queryParameters.BirthDateFrom);
        //    if (queryParameters.BirthDateTo != null)
        //        _allItems= _allItems.Where(x => x.BirthDate < queryParameters.BirthDateTo);


        //    return _allItems
        //        .Skip(queryParameters.PageCount * (queryParameters.Page))
        //        .Take(queryParameters.PageCount);
        //}

        //public async Task<int> Count()
        //{
        //    return  _context.students.Count();
        //}

        //public async Task<bool> Save()
        //{
        //    return (_context.SaveChanges() >= 0);
        //}
 