using MyReloadedOfficeApp.Models;
using MyReloadedOfficeApp.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MyReloadedOfficeApp.Controllers
{
    public class OfficeBuildingController : Controller
    {
        private OfficeBuildingRepository officeRepository = new OfficeBuildingRepository();
        private UsersRolesRepository userRoleRepository = new UsersRolesRepository();


        // GET: OfficeBuilding
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null)
            {

                List<OfficeBuildingsModel> officeBuildings = officeRepository.GetAllOfficeBuildings();

                return View("Index", officeBuildings);
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
                ViewBag.Message = String.Format("You have attempted to create a building which already exists - same name and rental contract - the operation has therefore been cancelled");
                List<OfficeBuildingsModel> buildings = officeRepository.GetAllOfficeBuildings();
                return View("IndexError", buildings);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // GET: OfficeBuilding/Details/5
        [Authorize]
        public ActionResult Details(Guid id)
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null)
            {
                OfficeBuildingsModel officeBuildingModel = officeRepository.GetBuildingById(id);
                return View("OfficeBuildingsDetails", officeBuildingModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }


        // GET: OfficeBuilding/Create
        [Authorize]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                return View("CreateOfficeBuilding");
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // POST: OfficeBuilding/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var userId = User.Identity.GetUserName();
                    if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
                    {
                    //instantiem modelul

                    OfficeBuildingsModel officeBuildingModel = new OfficeBuildingsModel();

                    UpdateModel(officeBuildingModel);

                        if (officeRepository.IsDuplicateOfficeBuilding(officeBuildingModel) == false)
                        {
                        officeRepository.InsertOfficeBuilding(officeBuildingModel);

                         return RedirectToAction("Index");
                        }
                        else
                        return RedirectToAction("IndexError");
                    }
                    else
                    return RedirectToAction("Contact", "Home");
            }
            catch
            {
                return View("CreateOfficeBuilding");
            }
        }

        // GET: OfficeBuilding/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                OfficeBuildingsModel officeBuildingModel = officeRepository.GetBuildingById(id);
                return View("EditOfficeBuilding", officeBuildingModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // POST: OfficeBuilding/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Guid id, FormCollection collection)
        {
            try
            {
                var userId = User.Identity.GetUserName();
                if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
                {
                    // TODO: Add update logic here

                    OfficeBuildingsModel officeBuildingModel = new OfficeBuildingsModel();

                UpdateModel(officeBuildingModel);

                officeRepository.UpdateOfficeBuilding(officeBuildingModel);

                return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Contact", "Home");
            }
            catch
            {
                return View("EditOfficeBuilding");
            }
        }

        // GET: OfficeBuilding/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                OfficeBuildingsModel officeBuildingModel = officeRepository.GetBuildingById(id);
                return View("DeleteOfficeBuilding", officeBuildingModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // POST: OfficeBuilding/Delete/5
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

                    officeRepository.DeleteBuilding(id);

                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Contact", "Home");
            }
            catch
            {
                ViewBag.Message_Delete = String.Format("The deletion of this office building is not possible, as it is currently in use. Please cancel all bookings and delete all associated floors for this building first and then reattempt the deletion operation of the office building.");
                return View("DeleteOfficeBuilding");
            }
        }
    }
}
