using MyReloadedOfficeApp.Models.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyReloadedOfficeApp.Models.Repository
{
    public class RawUserTypesRepository
    {

        private Models.DBObjects.officeplanningDataContext dbContext;


        public RawUserTypesRepository()
        {
            this.dbContext = new Models.DBObjects.officeplanningDataContext();
        }

        public RawUserTypesRepository(Models.DBObjects.officeplanningDataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<RawUserTypesModel> GetAllRawUserTypes()
        {
            List<RawUserTypesModel> rolesList = InitializeRolesCollection();
            foreach (RawUserType dbRawRole in dbContext.RawUserTypes)
            {
                AddDbObjectTo(rolesList, dbRawRole);
            }
            return rolesList;
        }

        public RawUserTypesModel GetRawRoleById(Guid ID)
        {
            var role = dbContext.RawUserTypes.FirstOrDefault(a => a.IdUserTypes == ID);

            return MapDbObjectToModel(role);
        }

        private static List<RawUserTypesModel> InitializeRolesCollection()
        {
            return new List<RawUserTypesModel>();
        }

        private void AddDbObjectTo(List<RawUserTypesModel> rolesList, RawUserType dbRole)
        {
            rolesList.Add(MapDbObjectToModel(dbRole));
        }


        public RawUserTypesModel GetRawRoleByName(string userRawRoleName)
        {
            var role = dbContext.RawUserTypes.FirstOrDefault(a => a.Name == userRawRoleName);

            return MapDbObjectToModel(role);
        }

        private RawUserTypesModel MapDbObjectToModel(RawUserType dbRole)
        {
            RawUserTypesModel role = new RawUserTypesModel();

            if (dbRole != null)
            {
                role.IdUserTypes = dbRole.IdUserTypes;
                role.Name = dbRole.Name;
                return role;
            }

            return null;

        }


        public bool IsDuplicateRawRole(RawUserTypesModel role)
        {
            if (GetRawRoleByName(role.Name) == null)
            {
                return false;
            }
            else
            {
                return true;
             }
          

        }

        public void InsertRawRole(RawUserTypesModel role)
        {

            role.IdUserTypes = Guid.NewGuid();
            dbContext.RawUserTypes.InsertOnSubmit(MapModelToDbObject(role));
            dbContext.SubmitChanges();

        }


        public void UpdateRawRole(RawUserTypesModel roles)
        {
            RawUserType roleDb = dbContext.RawUserTypes.FirstOrDefault(x => x.IdUserTypes == roles.IdUserTypes);
            if (roleDb != null)
            {
                roleDb.IdUserTypes = roles.IdUserTypes;
                roleDb.Name = roles.Name;
                dbContext.SubmitChanges();
            }
        }

        public void DeleteRawRole(Guid id)
        {
            RawUserType roleDb = dbContext.RawUserTypes.FirstOrDefault(x => x.IdUserTypes == id);

            if (roleDb != null)
            {
                dbContext.RawUserTypes.DeleteOnSubmit(roleDb);
                dbContext.SubmitChanges();
            }

        }

        private RawUserType MapModelToDbObject(RawUserTypesModel role)
        {
            RawUserType roleDb = new RawUserType();

            if (role != null)
            {
                roleDb.IdUserTypes = role.IdUserTypes;
                roleDb.Name = role.Name;
                return roleDb;
            }

            return null;
        }


    }
}