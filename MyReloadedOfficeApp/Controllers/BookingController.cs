using Microsoft.Ajax.Utilities;
using MyReloadedOfficeApp.Models;
using MyReloadedOfficeApp.Models.DBObjects;
using MyReloadedOfficeApp.Models.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MyReloadedOfficeApp.Controllers
{
    public class BookingController : Controller
    {
        private BookingRepository bookingRepository = new BookingRepository();
        private FloorRepository floorRepository = new FloorRepository();
        private OfficeBuildingRepository buildingRepository = new OfficeBuildingRepository();
        private UsersRolesRepository userRoleRepository = new UsersRolesRepository();
        private DepartmentRepository departmentRepository = new DepartmentRepository();


        // GET: Booking

        [Authorize]
        public ActionResult Index()
        {
            
            var userId = User.Identity.GetUserName();
            if ( userRoleRepository.GetRoleByUserName(userId) != null)
            {
                var userRole = userRoleRepository.GetRoleByUserName(userId).IdUserType;
                var userDepartment = userRoleRepository.GetRoleByUserName(userId).IdDepartment;

                if (userRole != "Manager" && userRole != "Admin")
                {

                    List<BookingsModel> bookings = bookingRepository.GetAllBookingsbyUserName(userId);
                    return View("Index", bookings);
                }
                else
                {
                    if (userRole != "Admin")
                    {
                        List<BookingsModel> bookings = bookingRepository.GetAllBookingsbyUserDepartment(userDepartment);
                        return View("Index", bookings);
                    }

                    else
                    {
                        List<BookingsModel> bookings = bookingRepository.GetAllBookings();
                        return View("Index", bookings);
                    }

                        
                }
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
                var userRole = userRoleRepository.GetRoleByUserName(userId).IdUserType;
                var userDepartment = userRoleRepository.GetRoleByUserName(userId).IdDepartment;

                if (userRole != "Manager" && userRole != "Admin")
                {
                    ViewBag.Message = String.Format("You have attempted to create a booking - same booking dates, booked seats, building and floor or you have attempted to book more seats than allowed for you/your department on this floor and office building - the operation has therefore been cancelled");
                    List<BookingsModel> bookings = bookingRepository.GetAllBookingsbyUserName(userId);
                    return View("IndexError", bookings);
                }

                else
                {
                    if (userRole != "Admin")
                    {
                        ViewBag.Message = String.Format("You have attempted to create a booking - same booking dates, booked seats, building and floor or you have attempted to book more seats than allowed for you/your department on this floor and office building - the operation has therefore been cancelled");
                        List<BookingsModel> bookings = bookingRepository.GetAllBookingsbyUserDepartment(userDepartment);
                        return View("IndexError", bookings);
                    }

                    else
                    {
                        ViewBag.Message = String.Format("You have attempted to create a booking - same booking dates, booked seats, building and floor or you have attempted to book more seats than allowed for you/your department on this floor and office building - the operation has therefore been cancelled");
                        List<BookingsModel> bookings = bookingRepository.GetAllBookings();
                        return View("IndexError", bookings);
                    }


                }

            }
            else
                return RedirectToAction("Contact", "Home");
        }

        [Authorize]
        public ActionResult IndexFilterErrorBooking()
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null)
            {
                var userRole = userRoleRepository.GetRoleByUserName(userId).IdUserType;
                var userDepartment = userRoleRepository.GetRoleByUserName(userId).IdDepartment;

                if (userRole != "Manager" && userRole != "Admin")
                {
                    ViewBag.Message = String.Format("You have attempted to create a booking - however no office space is alloted for your department yet. Contact admin.");
                    List<BookingsModel> bookings = bookingRepository.GetAllBookingsbyUserName(userId);
                    return View("IndexFilterErrorBooking", bookings);
                }

                else
                {
                    if (userRole != "Admin")
                    {
                        ViewBag.Message = String.Format("You have attempted to create a booking - however no office space is alloted for your department yet. Contact admin.");
                        List<BookingsModel> bookings = bookingRepository.GetAllBookingsbyUserDepartment(userDepartment);
                        return View("IndexFilterErrorBooking", bookings);
                    }

                    else
                    {
                        ViewBag.Message = String.Format("You have attempted to create a booking - however no office space is alloted for your department yet. Contact admin.");
                        List<BookingsModel> bookings = bookingRepository.GetAllBookings();
                        return View("IndexFilterErrorBooking", bookings);
                    }


                }


            }
            else
                return RedirectToAction("Contact", "Home");
        }


        [Authorize]
        // GET: Booking/Details/5
        public ActionResult Details(Guid id)
        {
            var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null)
            {
                BookingsModel bookingModel = bookingRepository.GetBookingById(id);
            return View("DetailsBooking", bookingModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        [Authorize]
        // GET: Booking/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserName();
            var floorItems = new List<FloorsModel>();
            if (userRoleRepository.GetRoleByUserName(userId) != null)
            {
                var userRole = userRoleRepository.GetRoleByUserName(userId).IdUserType;
                var userDepartment = userRoleRepository.GetRoleByUserName(userId).IdDepartment;
                var filterDepartmentId = departmentRepository.GetDepartmentIdByName(userDepartment);
                floorItems = floorRepository.GetFloorByDepartmentId(filterDepartmentId);
                

                if (floorItems != null && floorItems.Count!=0 )
                    {
                    ViewBag.datafloors = floorItems;
                    }
                else
                    {
                    ViewBag.Message = "No floor space has been allocated for your department. Please contact your admin.";

                    }

           
                if (ViewBag.Message != "No floor space has been allocated for your department. Please contact your admin.")
                    {
                    return View("CreateBooking");
                    }
                else
                    {
                    return RedirectToAction("IndexFilterErrorBooking");
                    }
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // POST: Booking/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(FormCollection collection)
        {

            try
            {
                var userId = User.Identity.GetUserName();
                var floorItems = new List<FloorsModel>();
                if (userRoleRepository.GetRoleByUserName(userId) != null)
                    {
                    var userRole = userRoleRepository.GetRoleByUserName(userId).IdUserType;
                    var userDepartment = userRoleRepository.GetRoleByUserName(userId).IdDepartment;
                    var filterDepartmentId = departmentRepository.GetDepartmentIdByName(userDepartment);
                    var spaceAlreadyBooked = 0;
                    //Get relevant floors for department

                        floorItems = floorRepository.GetFloorByDepartmentId(filterDepartmentId);


                    if (floorItems != null && floorItems.Count != 0 )
                        {
                        ViewBag.datafloors = floorItems;
                        }
                    else
                        {
                        ViewBag.Message = "No floor space has been allocated for your department. Please contact your admin.";
                        }


                // TODO: Add insert logic here

                    if (ViewBag.Message != "No floor space has been allocated for your department. Please contact your admin.")

                        {
                            BookingsModel bookingModel = new BookingsModel();
                            bookingModel.IdFloor = Guid.Parse(Request.Form["Floor"]);
                            bookingModel.IdBuilding = floorRepository.GetFloorById(Guid.Parse(Request.Form["Floor"])).IdBuilding;
                            bookingModel.UserName = userId;
                            bookingModel.UserDepartment = userDepartment;
                            bookingModel.UserRole = userRole;

                        UpdateModel(bookingModel);

                        var maxFloorBuildingSpace = floorRepository.GetFloorByFloorIdandBuildingIdandDepartmentId(bookingModel.IdFloor, bookingModel.IdBuilding, filterDepartmentId).BookableSeats;

                        var bookingsOnFloorBuildingDepartment = bookingRepository.GetBookingByDepartmentFloorBuilding(userDepartment, bookingModel.IdFloor, bookingModel.IdBuilding, bookingModel.BookingValidFrom, bookingModel.BookingValidTo);


                        

                        if (bookingsOnFloorBuildingDepartment ==null || bookingsOnFloorBuildingDepartment.Count ==0)
                            {
                            spaceAlreadyBooked =  bookingModel.BookedSeats;
                            }

                        else
                            {
                            foreach(BookingsModel booking in bookingsOnFloorBuildingDepartment)
                                {
                                spaceAlreadyBooked = spaceAlreadyBooked + booking.BookedSeats;
                                }
                            spaceAlreadyBooked = spaceAlreadyBooked  + bookingModel.BookedSeats;
                            }

                    
                        if (bookingRepository.IsDuplicateBooking(bookingModel) == false && spaceAlreadyBooked<= maxFloorBuildingSpace)
                            {
                                bookingRepository.InsertBooking(bookingModel);
                                return RedirectToAction("Index");
                            }
                        else
                            {
                                return RedirectToAction("IndexError");
                            }
                    }

                    else
                    {
                    return RedirectToAction("IndexFilterErrorBooking");
                    }
                    }
                else
                    return RedirectToAction("Contact", "Home");
             }
            catch
            {
                return View("CreateBooking");
            }
        }

        // GET: Booking/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var userId = User.Identity.GetUserName();
            var floorItems = new List<FloorsModel>();
            if (userRoleRepository.GetRoleByUserName(userId) != null)
            {
                var userRole = userRoleRepository.GetRoleByUserName(userId).IdUserType;
                var userDepartment = userRoleRepository.GetRoleByUserName(userId).IdDepartment;
                var filterDepartmentId = departmentRepository.GetDepartmentIdByName(userDepartment);
                

                    floorItems = floorRepository.GetFloorByDepartmentId(filterDepartmentId);



                if (floorItems != null && floorItems.Count != 0)
                {
                    ViewBag.datafloors = floorItems;
                }
                else
                {
                    ViewBag.Message = "No floor space has been allocated for your department. Please contact your admin.";
                }

            BookingsModel bookingsModel = bookingRepository.GetBookingById(id);

            return View("EditBooking", bookingsModel);
            }
            else
                return RedirectToAction("Contact", "Home");

        }

        // POST: Booking/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(Guid id, FormCollection collection)
        {
            try
            {
                var userId = User.Identity.GetUserName();
                var floorItems = new List<FloorsModel>();
                if (userRoleRepository.GetRoleByUserName(userId) != null)
                {
                    var userRole = userRoleRepository.GetRoleByUserName(userId).IdUserType;
                    var userDepartment = userRoleRepository.GetRoleByUserName(userId).IdDepartment;
                    var filterDepartmentId = departmentRepository.GetDepartmentIdByName(userDepartment);


                        floorItems = floorRepository.GetFloorByDepartmentId(filterDepartmentId);


                    if (floorItems != null && floorItems.Count != 0)
                    {
                        ViewBag.datafloors = floorItems;
                    }
                    else
                    {
                        ViewBag.Message = "No floor space has been allocated for your department. Please contact your admin.";
                    }


                // TODO: Add update logic here
                    if (ViewBag.Message != "No floor space has been allocated for your department. Please contact your admin.")

                    {
                    BookingsModel bookingModel = bookingRepository.GetBookingById(id);
                    bookingModel.IdFloor = Guid.Parse(Request.Form["Floor"]);
                    bookingModel.IdBuilding = floorRepository.GetFloorById(Guid.Parse(Request.Form["Floor"])).IdBuilding;
                    bookingModel.BookingTimeStamp = DateTime.Now;
                    bookingModel.BookingValidTo = bookingModel.BookingValidFrom;
                    UpdateModel(bookingModel);

                        if (bookingRepository.IsDuplicateBooking(bookingModel) == false)
                        {

                        bookingRepository.UpdateBooking(bookingModel);

                        return RedirectToAction("Index");
                        }
                        else
                        {
                        return RedirectToAction("IndexError");
                        }
                    }

                    else
                    {
                    return RedirectToAction("IndexFilterErrorBooking");
                    }
                }
                else
                    return RedirectToAction("Contact", "Home");

            }
            catch
            {
                return View("EditBooking");
            }
        }

        // GET: Booking/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        { 
        var userId = User.Identity.GetUserName();
            if (userRoleRepository.GetRoleByUserName(userId) != null)
            {
                var userRole = userRoleRepository.GetRoleByUserName(userId).IdUserType;
                var userDepartment = userRoleRepository.GetRoleByUserName(userId).IdDepartment;
                var filterDepartmentId = departmentRepository.GetDepartmentIdByName(userDepartment);

                BookingsModel bookingModel = bookingRepository.GetBookingById(id);

            return View("DeleteBooking", bookingModel);
            }
            else
                return RedirectToAction("Contact", "Home");
        }

        // POST: Booking/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            try
            {
                var userId = User.Identity.GetUserName();
                if (userRoleRepository.GetRoleByUserName(userId) != null)
                {
                    var userRole = userRoleRepository.GetRoleByUserName(userId).IdUserType;
                    var userDepartment = userRoleRepository.GetRoleByUserName(userId).IdDepartment;
                    var filterDepartmentId = departmentRepository.GetDepartmentIdByName(userDepartment);

                    // TODO: Add delete logic here
                    bookingRepository.DeleteBooking(id);
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
