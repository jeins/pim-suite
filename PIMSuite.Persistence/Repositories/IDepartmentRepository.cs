using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Repositories
{
    public interface IDepartmentRepository : IDisposable
    {
        IEnumerable<Department> GetDepartments();
        Department GetDepartmentByName(string DepartmentName);
        void InsertDepartment(Department Department);
        void DeleteDepartment(string DepartmentName);
        void UpdateDepartment(Department Department);
        void Save();
    }
}
