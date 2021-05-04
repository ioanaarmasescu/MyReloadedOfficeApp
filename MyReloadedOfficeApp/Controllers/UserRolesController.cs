using Microsoft.AspNet.Identity;
using MyReloadedOfficeApp.Models;
using MyReloadedOfficeApp.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyReloadedOfficeApp.Controllers
{
    public class UserRolesController : Controller
    {
        private UsersRolesRepository userRolesRepository = new UsersRolesRepository();

        // GET: UserRoles
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserName();
            if (userRolesRepository.GetRoleByUserName(userId) != null && userRolesRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                List<UsersClustersModel> userRoles = userRolesRepository.GetAllUserRoles();
                return View("Index", userRoles);
            }
            else
                return RedirectToAction("Contact", "Home");

        }

        [Authorize]
        public ActionResult IndexError()
        {
            var userId = User.Identity.GetUserName();
            if (userRolesRepository.GetRoleByUserName(userId) != null && userRolesRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                ViewBag.Message = String.Format("You have attempted to assign roles to a user already handled - please use the Edit function instead of Create New.");
                List<UsersClustersModel> userRoles = userRolesRepository.GetAllUserRoles();
                return View("IndexError", userRoles);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // GET: UserRoles/Details/5
        [Authorize]
        public ActionResult Details(string Id)
        {
            var userId = User.Identity.GetUserName();
            if (userRolesRepository.GetRoleByUserName(userId) != null && userRolesRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                UsersClustersModel UserRolesModel = userRolesRepository.GetRoleById(Id);
                return View("DetailsUserRoles", UserRolesModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // GET: UserRoles/Create
        [Authorize]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserName();
            if (userRolesRepository.GetRoleByUserName(userId) != null && userRolesRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                return View("CreateUserRolesAssignment");
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // POST: UserRoles/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var userId = User.Identity.GetUserName();
                if (userRolesRepository.GetRoleByUserName(userId) != null && userRolesRepository.GetRoleByUserName(userId).IdUserType == "Admin")
                {
                    UsersClustersModel usersModel = new UsersClustersModel();

                    usersModel.Name = Request.Form["UserName"];
                    usersModel.IdDepartment = Request.Form["Department"];
                    usersModel.IdUserType = Request.Form["UserType"];
                    usersModel.Id = Guid.NewGuid().ToString();
                    usersModel.Discriminator = "";

                    UpdateModel(usersModel);


                       if (userRolesRepository.IsDuplicateUserName(usersModel) == false)
                        {

                        userRolesRepository.InsertUserRole(usersModel);

                        return RedirectToAction("Index");
                        }
                        else
                        {
                        return RedirectToAction("IndexError");
                        }
                }
                else
                    return RedirectToAction("Contact", "Home");

            }
            catch (Exception e)
            {
                
                ViewBag.Message_Delete = String.Format(e.Message);
                return View("CreateUserRolesAssignment");
            }
        }

        // GET: UserRoles/Edit/5
        [Authorize]
        public ActionResult Edit(string Id)
        {
            var userId = User.Identity.GetUserName();
            if (userRolesRepository.GetRoleByUserName(userId) != null && userRolesRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                UsersClustersModel usersModel = userRolesRepository.GetRoleById(Id);
                return View("EditUserRole", usersModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // POST: UserRoles/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(string name, FormCollection collection)
        {
            try
            {
                var userId = User.Identity.GetUserName();
                if (userRolesRepository.GetRoleByUserName(userId) != null && userRolesRepository.GetRoleByUserName(userId).IdUserType == "Admin")
                {
                    // TODO: Add update logic here

                    UsersClustersModel usersModel = new UsersClustersModel();
                usersModel.Name = Request.Form["UserName"];
                usersModel.IdDepartment = Request.Form["Department"];
                usersModel.IdUserType = Request.Form["UserType"];
                usersModel.Id = Guid.NewGuid().ToString();
                usersModel.Discriminator = "";
                UpdateModel(usersModel);

                userRolesRepository.UpdateUserRole(usersModel);

                return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Contact", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRoles/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            var userId = User.Identity.GetUserName();
            if (userRolesRepository.GetRoleByUserName(userId) != null && userRolesRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                UsersClustersModel usersModel = userRolesRepository.GetRoleById(id);

            return View("DeleteBooking", usersModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // POST: UserRoles/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                var userId = User.Identity.GetUserName();
                if (userRolesRepository.GetRoleByUserName(userId) != null && userRolesRepository.GetRoleByUserName(userId).IdUserType == "Admin")
                {
                    // TODO: Add delete logic here

                    userRolesRepository.DeleteUserRole(id);
                return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Contact", "Home");
            }
            catch
            {
                return View("DeleteBooking");
            }
        }
    }
}
