using System;
using System.Collections.Generic;
using System.Linq;
using PIMSuite.Persistence.Entities;
using System.Data.Entity;

namespace PIMSuite.Persistence.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        // Constructors

        public DepartmentRepository(DataContext context)
        {
            _context = context;
        }

        // Fields

        private readonly DataContext _context;
        private bool _disposed = false;

        // Methods

        public void DeleteDepartment(string departmentName)
        {
            _context.Departments.Remove(_context.Departments.Find(departmentName));
        }

        public Department GetDepartmentByName(string departmentName)
        {
            return _context.Departments.Find(departmentName);
        }

        public IEnumerable<Department> GetDepartments()
        {
            return _context.Departments.ToList();
        }

        public void InsertDepartment(Department department)
        {
            _context.Departments.Add(department);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateDepartment(Department department)
        {
            _context.Entry(department).State = EntityState.Modified;
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}