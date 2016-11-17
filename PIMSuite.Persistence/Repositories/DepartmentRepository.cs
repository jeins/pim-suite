using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence;
using System.Data.Entity;

namespace PIMSuite.Persistence.Repositories
{
    public class DepartmentRepository : IDepartmentRepository, IDisposable
    {
        private DataContext context;
        private bool disposed = false;

        public DepartmentRepository(DataContext context)
        {
            this.context = context;
        }

        public void DeleteDepartment(string DepartmentName)
        {
            context.Departments.Remove(context.Departments.Find(DepartmentName));
        }

        public Department GetDepartmentByName(string DepartmentName)
        {
            return context.Departments.Find(DepartmentName);
        }

        public IEnumerable<Department> GetDepartments()
        {
            return context.Departments.ToList();
        }

        public void InsertDepartment(Department Department)
        {
            context.Departments.Add(Department);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateDepartment(Department Department)
        {
            context.Entry(Department).State = EntityState.Modified;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
