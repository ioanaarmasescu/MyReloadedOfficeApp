using MyReloadedOfficeApp.Models;
using MyReloadedOfficeApp.Models.DBObjects;
using MyReloadedOfficeApp.Models.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

//USEFUL LINKS!!!!
//https://stackoverflow.com/questions/546461/asp-net-mvc-redirect-to-a-different-view/546474
//https://forums.asp.net/t/2033384.aspx?redirect+from+View+to+another+view+
// https://www.c-sharpcorner.com/article/simply-create-dropdown-list-from-database-in-mvc-5-0/
// https://www.c-sharpcorner.com/UploadFile/4d9083/binding-dropdownlist-in-mvc-in-various-ways-in-mvc-with-data/
//https://docs.microsoft.com/en-us/aspnet/web-pages/overview/ui-layouts-and-themes/9-working-with-images
//https://learningprogramming.net/net/asp-net-core-razor-pages/use-images-css-and-javascript-in-asp-net-core-razor-pages/
//http://www-db.deis.unibo.it/courses/TW/DOCS/w3schools/aspnet/mvc_layout.asp.html
//https://www.youtube.com/watch?v=3jbSqmJmjkI
//https://www.youtube.com/watch?v=rct6LIyErUE

namespace MyReloadedOfficeApp.Controllers
{
    public class FloorController : Controller
    {
        private FloorRepository floorRepository = new FloorRepository();
        private OfficeBuildingRepository buildingRepository = new OfficeBuildingRepository();
        private DepartmentRepository departmentRepository = new DepartmentRepository();
        private UsersRolesRepository userRoleRepository = new UsersRolesRepository();

        // GET: Floor
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null)
            {
                List<FloorsModel> floors = floorRepository.GetAllFloors();
                return View("Index", floors);
            }
            else
                return RedirectToAction("Contact", "Home");
        }


        [Authorize]
        public ActionResult IndexError()
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null)
            {
                ViewBag.Message = String.Format("You have attempted to create a floor space - same office building and department - the operation has therefore been cancelled");
                List<FloorsModel> floors = floorRepository.GetAllFloors();
                return View("IndexError", floors);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        [Authorize]
        public ActionResult IndexError_MaximumSurpassed()
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null)
            {
                ViewBag.Message = String.Format("You have attempted to assign floor space to a department, but this surpasses the maximum allowed overall at department level");
                List<FloorsModel> floors = floorRepository.GetAllFloors();
                return View("IndexError_MaximumSurpassed", floors);
            }
            else
                return RedirectToAction("Contact", "Home");
    }

        // GET: Floor/Details/5
        [Authorize]
        public ActionResult Details(Guid id)
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null)
            {
                FloorsModel floorModel = floorRepository.GetFloorById(id);
                return View("DetailsFloors", floorModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // GET: Floor/Create
        [Authorize]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserName();
                if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
                {
                var departmentItems = departmentRepository.GetAllDepartments();
                    if (departmentItems != null && departmentItems.Count !=0)
                    {
                    ViewBag.datadepartments = departmentItems;
                    }

                var buildingItems = buildingRepository.GetAllOfficeBuildings();
                    if (buildingItems != null && buildingItems.Count != 0)
                    {
                    ViewBag.databuildings = buildingItems;
                    }

                return View("CreateFloor");
                }
                else
                return RedirectToAction("Contact", "Home");

        }

        // POST: Floor/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var userId = User.Identity.GetUserName();
                    if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
                    {
                        var alreadySetFloorLimit = 0;
                        var departmentItems = departmentRepository.GetAllDepartments();
                        if (departmentItems != null && departmentItems.Count != 0)
                        {
                        ViewBag.datadepartments = departmentItems;
                        }

                        var buildingItems = buildingRepository.GetAllOfficeBuildings();
                        if (buildingItems != null && buildingItems.Count != 0)
                        {
                        ViewBag.databuildings = buildingItems;
                        }

                        FloorsModel floorModel = new FloorsModel();

                        floorModel.IdDepartment = Guid.Parse(Request.Form["Department"]);
                        floorModel.IdBuilding = Guid.Parse(Request.Form["Building"]);


                        UpdateModel(floorModel);

                        //Check if allowed seats department limit is surpassed
                        //First calculate how many seats you would have with this additional entry
                        var floors = floorRepository.GetFloorByDepartmentId(floorModel.IdDepartment);

                        foreach (FloorsModel floor in floors)
                        {
                        alreadySetFloorLimit = floor.BookableSeats + alreadySetFloorLimit;
                        }

                        alreadySetFloorLimit = alreadySetFloorLimit + floorModel.BookableSeats;

                        //Check which is the maximum allowed overall space at department level
                        if (alreadySetFloorLimit <= departmentRepository.GetDepartmentById(floorModel.IdDepartment).MaximumSeatsPerDepartment)
                            {

                                if (floorRepository.IsDuplicateFloor(floorModel) == false)
                                {

                                floorRepository.InsertFloorBuilding(floorModel);

                                return RedirectToAction("Index");
                                }
                        else
                        {
                            return RedirectToAction("IndexError");
                        }

                    }
                    else
                        {
                        return RedirectToAction("IndexError_MaximumSurpassed");
                        }

                    }
                    else
                    return RedirectToAction("Contact", "Home");

            }
            catch
            {
                return View("CreateFloor");
            }
        }

        // GET: Floor/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var userId = User.Identity.GetUserName();
                if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
                {
                var departmentItems = departmentRepository.GetAllDepartments();
                    if (departmentItems != null && departmentItems.Count != 0)
                    {
                    ViewBag.datadepartments = departmentItems;
                    }

                var buildingItems = buildingRepository.GetAllOfficeBuildings();
                    if (buildingItems != null && buildingItems.Count != 0)
                    {
                    ViewBag.databuildings = buildingItems;
                    }
                FloorsModel floorsModel = floorRepository.GetFloorById(id);
                return View("EditFloor", floorsModel);
                }
                else
                return RedirectToAction("Contact", "Home");
        }

        // POST: Floor/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(Guid id, FormCollection collection)
        {
            try
            {
                var userId = User.Identity.GetUserName();
                    if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
                        {
                         // TODO: Add update logic here
                        var alreadySetFloorLimit = 0;

                        var departmentItems = departmentRepository.GetAllDepartments();
                            if (departmentItems != null && departmentItems.Count != 0)
                            {
                            ViewBag.datadepartments = departmentItems;
                            }

                        var buildingItems = buildingRepository.GetAllOfficeBuildings();
                            if (buildingItems != null && buildingItems.Count != 0)
                            {
                            ViewBag.databuildings = buildingItems;
                            }

                    FloorsModel floorsModel = new FloorsModel();

                    floorsModel.IdDepartment = Guid.Parse(Request.Form["Department"]);
                    floorsModel.IdBuilding = Guid.Parse(Request.Form["Building"]);


                    UpdateModel(floorsModel);

                    var floors = floorRepository.GetFloorByDepartmentId(floorsModel.IdDepartment);

                        foreach (FloorsModel floor in floors)
                        {
                        alreadySetFloorLimit = floor.BookableSeats + alreadySetFloorLimit;
                        }

                    alreadySetFloorLimit = alreadySetFloorLimit + floorsModel.BookableSeats;

                        if (alreadySetFloorLimit <= departmentRepository.GetDepartmentById(floorsModel.IdDepartment).MaximumSeatsPerDepartment)
                        {
                        floorRepository.UpdateFloor(floorsModel);
                        return RedirectToAction("Index");
                        }
                        else
                        {
                        return RedirectToAction("IndexError_MaximumSurpassed");
                        }
                    }
                else
                    return RedirectToAction("Contact", "Home");
            }
            catch
            {
                return View("EditFloor");
            }
        }

        // GET: Floor/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                FloorsModel floorModel = floorRepository.GetFloorById(id);

                return View("DeleteFloor", floorModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // POST: Floor/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            try
            {
                var userId = User.Identity.GetUserName();
                if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
                {
                    // TODO: Add delete logic here
                    floorRepository.DeleteFloor(id);
                return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Contact", "Home");
            }
            catch
            {
                ViewBag.Message_Delete = String.Format("The deletion of this floor is not possible, as it is currently in use. Please cancel all bookings from this floor first and then reattempt the deletion operation of the floor.");
                return View("DeleteFloor");
            }
        }
    }
}
