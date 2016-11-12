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
        Department GetDepartamentByName(string DepartmentName);
        void InsertDepartament(Department Departament);
        void DeleteDepartament(string DepartmentName);
        void UpdateDepartament(Department Departament);
        void Save();
    }
}
