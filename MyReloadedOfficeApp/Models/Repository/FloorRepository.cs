using MyReloadedOfficeApp.Models.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyReloadedOfficeApp.Models.Repository
{
    public class FloorRepository
    {

        private Models.DBObjects.officeplanningDataContext dbContext;


        public FloorRepository()
        {
            this.dbContext = new Models.DBObjects.officeplanningDataContext();
        }

        public FloorRepository(Models.DBObjects.officeplanningDataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<FloorsModel> GetAllFloors()
        {
            List<FloorsModel> floorList = InitializeFloorsCollection();
            foreach (Floor dbFloor in dbContext.Floors)
            {
                AddDbObjectTo(floorList, dbFloor);
            }
            return floorList;
        }
        public FloorsModel GetFloorById(Guid ID)
        {
            var floor = dbContext.Floors.FirstOrDefault(a => a.IdFloor == ID);

            return MapDbObjectToModel(floor);
        }

        public FloorsModel GetFloorByFloorIdandBuildingIdandDepartmentId(Guid floorID, Guid buildingID, Guid departmentId)
        {
            var floor = dbContext.Floors.FirstOrDefault(a => a.IdFloor == floorID && a.IdBuilding == buildingID && a.IdDepartment == departmentId);

            return MapDbObjectToModel(floor);
        }

        public List<FloorsModel> GetFloorByDepartmentId(Guid ID)
        {
            List<FloorsModel> floorList = InitializeFloorsCollection();
            foreach (Floor dbFloor in dbContext.Floors)
                if (dbFloor.IdDepartment == ID)
                { 
                    AddDbObjectTo(floorList, dbFloor);
                }
            return floorList;
        }

        public FloorsModel GetFloorByBuildingId(Guid ID)
        {
            var floor = dbContext.Floors.FirstOrDefault(a => a.IdBuilding == ID);

            return MapDbObjectToModel(floor);
        }

        public FloorsModel GetFloorByBookableSeats(int seats)
        {
            var floor = dbContext.Floors.FirstOrDefault(a => a.BookableSeats == seats);

            return MapDbObjectToModel(floor);
        }

        public bool IsDuplicateFloor(FloorsModel floor)
        {
            var floorByBuildingId = GetFloorByBuildingId(floor.IdBuilding);
            if (floorByBuildingId == null)
            {
                return false;
            }
            else
            {
                if (floorByBuildingId.IdDepartment == floor.IdDepartment)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
            




            
        }

        public void InsertFloorBuilding(FloorsModel floor)
        {

            floor.IdFloor = Guid.NewGuid();
            dbContext.Floors.InsertOnSubmit(MapModelToDbObject(floor));
            dbContext.SubmitChanges();

        }


        public void UpdateFloor(FloorsModel floors)
        {
            Floor floorDb = dbContext.Floors.FirstOrDefault(x => x.IdFloor == floors.IdFloor);
            if (floorDb != null)
            {
                floorDb.IdFloor = floors.IdFloor;
                floorDb.Name = floors.Name;
                floorDb.IdDepartment = floors.IdDepartment;
                floorDb.IdBuilding = floors.IdBuilding;
                floorDb.BookableSeats = floors.BookableSeats;
                floorDb.FloorDescription = floors.FloorDescription;
                dbContext.SubmitChanges();
            }
        }

        public void DeleteFloor(Guid ID)
        {
            Floor floorDb = dbContext.Floors.FirstOrDefault(x => x.IdFloor == ID);

            if (floorDb != null)
            {
                dbContext.Floors.DeleteOnSubmit(floorDb);
                dbContext.SubmitChanges();
            }

        }




        private Floor MapModelToDbObject(FloorsModel floor)
        {
            Floor floorDb = new Floor();

            if (floor != null)
            {
                floorDb.IdFloor = floor.IdFloor;
                floorDb.Name = floor.Name;
                floorDb.IdDepartment = floor.IdDepartment;
                floorDb.IdBuilding = floor.IdBuilding;
                floorDb.BookableSeats = floor.BookableSeats;
                floorDb.FloorDescription = floor.FloorDescription;
                return floorDb;
            }

            return null;
        }

        private FloorsModel MapDbObjectToModel(Floor dbFloor)
        {
            FloorsModel floor = new FloorsModel();

            if (dbFloor != null)
            {
                floor.IdFloor = dbFloor.IdFloor;
                floor.Name = dbFloor.Name;
                floor.IdDepartment = dbFloor.IdDepartment;
                floor.IdBuilding = dbFloor.IdBuilding;
                floor.BookableSeats = dbFloor.BookableSeats;
                floor.FloorDescription = dbFloor.FloorDescription;
                return floor;
            }

            return null;

        }

        private static List<FloorsModel> InitializeFloorsCollection()
        {
            return new List<FloorsModel>();
        }

        private void AddDbObjectTo(List<FloorsModel> floorList, Floor dbFloor)
        {
            floorList.Add(MapDbObjectToModel(dbFloor));
        }

    }
}