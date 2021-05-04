using MyReloadedOfficeApp.Models.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyReloadedOfficeApp.Models.Repository
{
    public class OfficeBuildingRepository
    {

        private Models.DBObjects.officeplanningDataContext dbContext;

        public OfficeBuildingRepository()
         {
            this.dbContext = new Models.DBObjects.officeplanningDataContext();
         }

        public OfficeBuildingRepository(Models.DBObjects.officeplanningDataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<OfficeBuildingsModel> GetAllOfficeBuildings()
        {
            List<OfficeBuildingsModel> officeBuilingsList = InitializeBuildingsCollection();
            foreach (OfficeBuilding dbBuilding in dbContext.OfficeBuildings)
            {
                AddDbObjectTo(officeBuilingsList, dbBuilding);
            }
            return officeBuilingsList;
        }

        

        public OfficeBuildingsModel GetBuildingById(Guid ID)
        {
            var building = dbContext.OfficeBuildings.FirstOrDefault(a => a.IdBuilding == ID);

            return MapDbObjectToModel(building);
        }

   
        public OfficeBuildingsModel GetBuildingByName(string name)
        {
            var building = dbContext.OfficeBuildings.FirstOrDefault(a => a.Name == name);

            return MapDbObjectToModel(building);
        }

        public OfficeBuildingsModel GetBuildingByRentalContract(string contract)
        {
            var building = dbContext.OfficeBuildings.FirstOrDefault(a => a.RentalContract == contract);

            return MapDbObjectToModel(building);
        }

        public bool IsDuplicateOfficeBuilding(OfficeBuildingsModel building)
        {
            if (GetBuildingByName(building.Name) == null)
            {
                return false;
            }
            else
                return true;
        }

        public void InsertOfficeBuilding(OfficeBuildingsModel building)
        {
           
                building.IdBuilding = Guid.NewGuid();
                dbContext.OfficeBuildings.InsertOnSubmit(MapModelToDbObject(building));
                dbContext.SubmitChanges();

        }

        public void UpdateOfficeBuilding(OfficeBuildingsModel building)
        {
            OfficeBuilding buildingDb = dbContext.OfficeBuildings.FirstOrDefault(x => x.IdBuilding == building.IdBuilding);
            if (buildingDb != null)
            {
                buildingDb.IdBuilding = building.IdBuilding;
                buildingDb.Name = building.Name;
                buildingDb.StreetHouseNumber = building.StreetHouseNumber;
                buildingDb.City = building.City;
                buildingDb.PostalCode = building.PostalCode;
                buildingDb.RentalContract = building.RentalContract;
                buildingDb.BuildingDescription = building.BuildingDescription;
                dbContext.SubmitChanges();
            }
        }

        public void DeleteBuilding(Guid ID)
        {
            OfficeBuilding buildingDb = dbContext.OfficeBuildings.FirstOrDefault(x => x.IdBuilding == ID);

            if (buildingDb != null)
            {
                dbContext.OfficeBuildings.DeleteOnSubmit(buildingDb);
                dbContext.SubmitChanges();
            }

        }

        private OfficeBuilding MapModelToDbObject(OfficeBuildingsModel building)
        {
            OfficeBuilding buildingDb = new OfficeBuilding();

            if (building != null)
            {
                buildingDb.IdBuilding = building.IdBuilding;
                buildingDb.Name = building.Name;
                buildingDb.StreetHouseNumber = building.StreetHouseNumber;
                buildingDb.City = building.City;
                buildingDb.PostalCode = building.PostalCode;
                buildingDb.RentalContract = building.RentalContract;
                buildingDb.BuildingDescription = building.BuildingDescription;

                return buildingDb;
            }

            return null;
        }

        private OfficeBuildingsModel MapDbObjectToModel(OfficeBuilding dbBuilding)
        {
            OfficeBuildingsModel building = new OfficeBuildingsModel();

            if (dbBuilding != null)
            {
                building.BuildingDescription = dbBuilding.BuildingDescription;
                building.City = dbBuilding.City;
                building.IdBuilding = dbBuilding.IdBuilding;
                building.PostalCode = dbBuilding.PostalCode;
                building.RentalContract = dbBuilding.RentalContract;
                building.StreetHouseNumber = dbBuilding.StreetHouseNumber;
                building.Name = dbBuilding.Name;
                return building;
            }

            return null;

        }

        private static List<OfficeBuildingsModel> InitializeBuildingsCollection()
        {
            return new List<OfficeBuildingsModel>();
        }

        private void AddDbObjectTo(List<OfficeBuildingsModel> officeBuildingsList, OfficeBuilding dbBuilding)
        {
            officeBuildingsList.Add(MapDbObjectToModel(dbBuilding));
        }






    }
}