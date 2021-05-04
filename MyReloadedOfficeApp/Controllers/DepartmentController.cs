using MyReloadedOfficeApp.Models;
using MyReloadedOfficeApp.Models.DBObjects;
using MyReloadedOfficeApp.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MyReloadedOfficeApp.Controllers
{
    public class DepartmentController : Controller
    {
        private DepartmentRepository departmentRepository = new DepartmentRepository();
        private UsersRolesRepository userRoleRepository = new UsersRolesRepository();

        // GET: Department
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null)
                {
                List<DepartmentsModel> departments = departmentRepository.GetAllDepartments();
                return View("Index", departments);
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
                ViewBag.Message = String.Format("You have attempted to create a department which already exists - same name - the operation has therefore been cancelled");
                List<DepartmentsModel> departments = departmentRepository.GetAllDepartments();
                return View("IndexError", departments);
            }
            else
                return RedirectToAction("Contact", "Home");
        }


        // GET: Department/Details/5
        [Authorize]
        public ActionResult Details(Guid id)
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null)
            {
                DepartmentsModel departmentModel = departmentRepository.GetDepartmentById(id);
                return View("DetailsDepartment", departmentModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // GET: Department/Create
        [Authorize]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserName();
                if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin" )
                    {
                       return View("CreateDepartment");
                    }
                else
                    return RedirectToAction("Contact", "Home");
        }

        // POST: Department/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var userId = User.Identity.GetUserName();
                if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
                {
                    // TODO: Add insert logic here

                    DepartmentsModel departmentModel = new DepartmentsModel();

                    UpdateModel(departmentModel);

                    if (departmentRepository.IsDuplicateDepartment(departmentModel) == false)
                    {

                    departmentRepository.InsertDepartment(departmentModel);

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
            catch
            {
                return View("CreateDepartment");
            }
        }

        // GET: Department/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null && (userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin" || userRoleRepository.GetRoleByUserName(userId).IdUserType == "Manager"))
            {
                DepartmentsModel departmentsModel = departmentRepository.GetDepartmentById(id);
                return View("EditDepartment", departmentsModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // POST: Department/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(Guid id, FormCollection collection)
        {
            try
            {
                var userId = User.Identity.GetUserName();
                if (userRoleRepository.GetRoleByUserName(userId) != null && (userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin" || userRoleRepository.GetRoleByUserName(userId).IdUserType == "Manager"))
                {
                    // TODO: Add update logic here
                    DepartmentsModel departmentModel = new DepartmentsModel();

                    UpdateModel(departmentModel);

                    departmentRepository.UpdateDepartment(departmentModel);

                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Contact", "Home");
            }
            catch
            {
                return View("EditDepartment");
            }
        }

        // GET: Department/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null && userRoleRepository.GetRoleByUserName(userId).IdUserType == "Admin")
            {
                DepartmentsModel departmentModel = departmentRepository.GetDepartmentById(id);
                return View("DeleteDepartment", departmentModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // POST: Department/Delete/5
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
                    if (userRoleRepository.GetRoleByDepartmentName(departmentRepository.GetDepartmentById(id).Name) == null)
                    {
                        departmentRepository.DeleteDepartment(id);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Message_Delete = String.Format("The deletion of this department is not possible, as it is currently in use. Please remove first the associated department from the assigned user roles and then reattempt the deletion operation of the department.");
                        return View("DeleteDepartment");
                    }
                }
                else
                    return RedirectToAction("Contact", "Home");
            }
            catch
            {
                ViewBag.Message_Delete = String.Format("The deletion of this department is not possible, as it is currently in use. Please remove first the associated department space on the assigned floor and then reattempt the deletion operation of the department.");
                return View("DeleteDepartment");
            }
        }
    }
}
