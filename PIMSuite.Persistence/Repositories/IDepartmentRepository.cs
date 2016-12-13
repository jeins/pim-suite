using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;

namespace PIMSuite.Persistence.Repositories
{
    public interface IDepartmentRepository : IDisposable
    {
        IEnumerable<Department> GetDepartments();
        Department GetDepartmentByName(string departmentName);
        void InsertDepartment(Department department);
        void DeleteDepartment(string departmentName);
        void UpdateDepartment(Department department);
        void Save();
    }
}