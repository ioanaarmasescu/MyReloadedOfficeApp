using MyReloadedOfficeApp.Models.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyReloadedOfficeApp.Models.Repository
{
    public class DepartmentRepository
    {

        private Models.DBObjects.officeplanningDataContext dbContext;

        public DepartmentRepository()
        {
            this.dbContext = new Models.DBObjects.officeplanningDataContext();
        }

        public DepartmentRepository(Models.DBObjects.officeplanningDataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<DepartmentsModel> GetAllDepartments()
        {
            List<DepartmentsModel> departmentList = InitializeDepartmentsCollection();
            foreach (Department dbDepartment in dbContext.Departments)
            {
                AddDbObjectTo(departmentList, dbDepartment);
            }
            return departmentList;
        }

        public List<string> GetAllDepartmentNames()
        {
            List<string> departmentNamesList = new List<string>();
            List<DepartmentsModel> departmentList = InitializeDepartmentsCollection();
            foreach (Department dbDepartment in dbContext.Departments)
            {
                departmentNamesList.Add(dbDepartment.Name);
            }
            return departmentNamesList;
        }

        public Guid GetDepartmentIdByName(string name)
        {
            var department = dbContext.Departments.FirstOrDefault(a => a.Name == name);

            return department.IdDepartment;
        }

        public DepartmentsModel GetDepartmentById(Guid ID)
        {
            var department = dbContext.Departments.FirstOrDefault(a => a.IdDepartment == ID);

            return MapDbObjectToModel(department);
        }

        public DepartmentsModel GetDepartmentByName(string name)
        {
            var department = dbContext.Departments.FirstOrDefault(a => a.Name == name);

            return MapDbObjectToModel(department);
        }

        public bool IsDuplicateDepartment(DepartmentsModel department)
        {
            if (GetDepartmentByName(department.Name) == null)
                return false;
            else
                return true;
        }

        public void InsertDepartment(DepartmentsModel department)
        {
           
                department.IdDepartment = Guid.NewGuid();
                dbContext.Departments.InsertOnSubmit(MapModelToDbObject(department));
                dbContext.SubmitChanges();
  
        }

        public void UpdateDepartment(DepartmentsModel department)
        {
            Department departmentDb = dbContext.Departments.FirstOrDefault(x => x.IdDepartment == department.IdDepartment);
            if (departmentDb != null)
            {
                departmentDb.IdDepartment = department.IdDepartment;
                departmentDb.Name = department.Name;
                departmentDb.MaximumSeatsPerDepartment = department.MaximumSeatsPerDepartment;
                departmentDb.DepartmentDescription = department.DepartmentDescription;
                dbContext.SubmitChanges();
            }
        }

        public void DeleteDepartment(Guid ID)
        {
            Department departmentDb = dbContext.Departments.FirstOrDefault(x => x.IdDepartment == ID);

            if (departmentDb != null)
            {
                dbContext.Departments.DeleteOnSubmit(departmentDb);
                dbContext.SubmitChanges();
            }

        }

        private Department MapModelToDbObject(DepartmentsModel department)
        {
            Department departmentDb = new Department();

            if (department != null)
            {
                departmentDb.IdDepartment = department.IdDepartment;
                departmentDb.Name = department.Name;
                departmentDb.MaximumSeatsPerDepartment = department.MaximumSeatsPerDepartment;
                departmentDb.DepartmentDescription = department.DepartmentDescription;

                return departmentDb;
            }

            return null;
        }

        private DepartmentsModel MapDbObjectToModel(Department dbDepartment)
        {
            DepartmentsModel department = new DepartmentsModel();

            if (dbDepartment != null)
            {
                department.IdDepartment = dbDepartment.IdDepartment;
                department.Name = dbDepartment.Name;
                department.MaximumSeatsPerDepartment = dbDepartment.MaximumSeatsPerDepartment;
                department.DepartmentDescription = dbDepartment.DepartmentDescription;
                return department;
            }

            return null;

        }

        private static List<DepartmentsModel> InitializeDepartmentsCollection()
        {
            return new List<DepartmentsModel>();
        }

        private void AddDbObjectTo(List<DepartmentsModel> departmentsList, Department dbDepartment)
        {
            departmentsList.Add(MapDbObjectToModel(dbDepartment));
        }








    }
}