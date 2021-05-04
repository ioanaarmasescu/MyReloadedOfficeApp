using MyReloadedOfficeApp.Models.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace MyReloadedOfficeApp.Models.Repository
{
    public class UsersRolesRepository
    {
        private Models.DBObjects.officeplanningDataContext dbContext;

        public UsersRolesRepository()
        {
            this.dbContext = new Models.DBObjects.officeplanningDataContext();
        }

        public UsersRolesRepository(Models.DBObjects.officeplanningDataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<UsersClustersModel> GetAllUserRoles()
        {
            List<UsersClustersModel> usersRolesList = InitializeUsersRolesCollection();
            foreach (AspNetRole dbUserRole in dbContext.AspNetRoles)
            {
                AddDbObjectTo(usersRolesList, dbUserRole);
            }
            return usersRolesList;
        }

        public UsersClustersModel GetRoleById(string Id)
        {
            var userRole = dbContext.AspNetRoles.FirstOrDefault(a => a.Id == Id);

            return MapDbObjectToModel(userRole);
        }

        public UsersClustersModel GetRoleByDepartmentName(string departmentName)
        {
            var userRole = dbContext.AspNetRoles.FirstOrDefault(a => a.IdDepartment == departmentName);

            return MapDbObjectToModel(userRole);
        }

        public UsersClustersModel GetRoleByUserName(string userName)
        {
            var userRole = dbContext.AspNetRoles.FirstOrDefault(a => a.Name == userName);

            return MapDbObjectToModel(userRole);
        }


        public bool IsDuplicateUserName(UsersClustersModel userRole)
        {
            if (GetRoleByUserName(userRole.Name) == null)
                return false;
            else
                return true;
        }

        public void InsertUserRole(UsersClustersModel userRole)
        {

            dbContext.AspNetRoles.InsertOnSubmit(MapModelToDbObject(userRole));
            dbContext.SubmitChanges();

        }

        public void UpdateUserRole(UsersClustersModel userRole)
        {
            AspNetRole userRoleDb = dbContext.AspNetRoles.FirstOrDefault(x => x.Id == userRole.Id);
            if (userRoleDb != null)
            {
                userRoleDb.Name = userRole.Name;
                userRoleDb.IdDepartment = userRole.IdDepartment;
                userRoleDb.IdUserType = userRole.IdUserType;
                userRoleDb.Id = userRole.Id;
                userRoleDb.Discriminator = userRole.Discriminator;
                dbContext.SubmitChanges();
            }
        }

        public void DeleteUserRole(string Id)
        {
            AspNetRole userNameDb = dbContext.AspNetRoles.FirstOrDefault(x => x.Id == Id);

            if (userNameDb != null)
            {
                dbContext.AspNetRoles.DeleteOnSubmit(userNameDb);
                dbContext.SubmitChanges();
            }

        }

        private AspNetRole MapModelToDbObject(UsersClustersModel role)
        {
            AspNetRole roleDb = new AspNetRole();

            if (role != null)
            {
                roleDb.Id = role.Id;
                roleDb.Name = role.Name;
                roleDb.IdDepartment = role.IdDepartment;
                roleDb.IdUserType = role.IdUserType;
                roleDb.Discriminator = role.Discriminator;
                return roleDb;
            }

            return null;
        }

        private UsersClustersModel MapDbObjectToModel(AspNetRole dbRole)
        {
            UsersClustersModel role = new UsersClustersModel();

            if (dbRole != null)
            {
                role.Name = dbRole.Name;
                role.IdDepartment = dbRole.IdDepartment;
                role.IdUserType = dbRole.IdUserType;
                role.Id = dbRole.Id;
                role.Discriminator = dbRole.Discriminator;

                return role;
            }

            return null;

        }

        private static List<UsersClustersModel> InitializeUsersRolesCollection()
        {
            return new List<UsersClustersModel>();
        }

        private void AddDbObjectTo(List<UsersClustersModel> usersRolesList, AspNetRole dbRole)
        {
            usersRolesList.Add(MapDbObjectToModel(dbRole));
        }


    }
}