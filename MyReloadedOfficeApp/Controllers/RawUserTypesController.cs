using MyReloadedOfficeApp.Models;
using MyReloadedOfficeApp.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MyReloadedOfficeApp.Controllers
{
    public class RawUserTypesController : Controller
    {
        private RawUserTypesRepository rawRolesRepository = new RawUserTypesRepository();
        private UsersRolesRepository userRoleRepository = new UsersRolesRepository();

        // GET: RawUserTypes
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            { 
                List<RawUserTypesModel> roles = rawRolesRepository.GetAllRawUserTypes();
                return View("Index", roles);
            }
            else
                return RedirectToAction("Contact", "Home");


        }

        [Authorize]
        public ActionResult IndexError()
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                ViewBag.Message = String.Format("You have attempted to create a user role type which already exists");
                List<RawUserTypesModel> roles = rawRolesRepository.GetAllRawUserTypes();
                return View("IndexError", roles);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        [Authorize]
        // GET: RawUserTypes/Details/5
        public ActionResult Details(Guid id)
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                RawUserTypesModel rolesModel = rawRolesRepository.GetRawRoleById(id);
                return View("DetailsRawRoles", rolesModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        [Authorize]
        // GET: RawUserTypes/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                return View("CreateRawUserType");
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // POST: RawUserTypes/Create
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
                    RawUserTypesModel rawRolesModel = new RawUserTypesModel();

                UpdateModel(rawRolesModel);

                if (rawRolesRepository.IsDuplicateRawRole(rawRolesModel) == false)
                {
                    rawRolesRepository.InsertRawRole(rawRolesModel);

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
                return View("CreateRawUserType");
            }
        }

        // GET: RawUserTypes/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                RawUserTypesModel rawRolesModel = rawRolesRepository.GetRawRoleById(id);
                return View("EditRawRoles", rawRolesModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // POST: RawUserTypes/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(Guid id, FormCollection collection)
        {
            try
            {
                var userId = User.Identity.GetUserName();
                if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
                {
                    RawUserTypesModel rawRolesModel = new RawUserTypesModel();

                    UpdateModel(rawRolesModel);

                    rawRolesRepository.UpdateRawRole(rawRolesModel);

                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Contact", "Home");
            }
            catch
            {
                return View("EditRawRoles");
            }
        }

        // GET: RawUserTypes/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                RawUserTypesModel rawRolesModel = rawRolesRepository.GetRawRoleById(id);
                return View("DeleteRawRole", rawRolesModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // POST: RawUserTypes/Delete/5
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


                    rawRolesRepository.DeleteRawRole(id);

                return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Contact", "Home");
            }
            catch
            {
                ViewBag.Message_Delete = String.Format("The deletion of this role type is not possible, as it is currently in use. Please remove the role from all users and then reattempt the deletion operation of the rolet type.");
                return View("DeleteRawRole");
            }
        }
    }
}
