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

        public void DeleteDepartament(string DepartmentName)
        {
            context.Departments.Remove(context.Departments.Find(DepartmentName));
        }

        public Department GetDepartamentByName(string DepartmentName)
        {
            return context.Departments.Find(DepartmentName);
        }

        public IEnumerable<Department> GetDepartments()
        {
            return context.Departments.ToList();
        }

        public void InsertDepartament(Department Departament)
        {
            context.Departments.Add(Departament);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateDepartament(Department Departament)
        {
            context.Entry(Departament).State = EntityState.Modified;
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
