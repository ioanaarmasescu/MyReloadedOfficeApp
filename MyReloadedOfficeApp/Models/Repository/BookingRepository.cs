using MyReloadedOfficeApp.Models.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace MyReloadedOfficeApp.Models.Repository
{
    public class BookingRepository
    {

        private Models.DBObjects.officeplanningDataContext dbContext;

        public BookingRepository()
        {
            this.dbContext = new Models.DBObjects.officeplanningDataContext();
        }

        public BookingRepository(Models.DBObjects.officeplanningDataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<BookingsModel> GetAllBookings()
        {
            List<BookingsModel> bookingsList = InitializeBookingsCollection();
            foreach (Booking dbBooking in dbContext.Bookings)
            {
                AddDbObjectTo(bookingsList, dbBooking);
            }
            return bookingsList;
        }

        public List<BookingsModel> GetAllBookingsbyUserName(string userName)
        {
            List<BookingsModel> bookingsList = InitializeBookingsCollection();
            foreach (Booking dbBooking in dbContext.Bookings)
            {
                if (dbBooking.UserName == userName)
                {
                    AddDbObjectTo(bookingsList, dbBooking);
                }
            }
            return bookingsList;
        }

        public List<BookingsModel> GetAllBookingsbyUserDepartment(string userDepartment)
        {
            List<BookingsModel> bookingsList = InitializeBookingsCollection();
            foreach (Booking dbBooking in dbContext.Bookings)
            {
                if (dbBooking.UserDepartment == userDepartment)
                {
                    AddDbObjectTo(bookingsList, dbBooking);
                }
            }
            return bookingsList;
        }

        public BookingsModel GetBookingById(Guid ID)
        {
            var booking = dbContext.Bookings.FirstOrDefault(a => a.IdBooking == ID);

            return MapDbObjectToModel(booking);
        }

        public List<BookingsModel> GetBookingByDepartmentFloorBuilding (string department, Guid IdFloor, Guid IdBuilding, DateTime validFrom, DateTime validTo)
        {
            List<BookingsModel> bookingsList = InitializeBookingsCollection();
            foreach (Booking dbBooking in dbContext.Bookings)
            { 
                if (dbBooking.IdFloor == IdFloor && dbBooking.IdBuilding == IdBuilding && dbBooking.UserDepartment == department && dbBooking.BookingValidFrom == validFrom && dbBooking.BookingValidTo == validTo)
                {
                    AddDbObjectTo(bookingsList, dbBooking);
                }
            }

            return bookingsList;
        }

        public BookingsModel GetIdenticalBooking(DateTime bookingValidFrom, DateTime bookingValidTo, Guid idFloor, Guid idBuilding, int bookedSeats)
        {
            var booking = dbContext.Bookings.FirstOrDefault(a => a.BookingValidFrom == bookingValidFrom && a.BookingValidTo == bookingValidTo && a.IdFloor == idFloor && a.IdBuilding == idBuilding && a.BookedSeats == bookedSeats);

            return MapDbObjectToModel(booking);
        }


        public bool IsDuplicateBooking(BookingsModel booking)
        {
            if (GetIdenticalBooking(booking.BookingValidFrom, booking.BookingValidTo, booking.IdFloor, booking.IdBuilding, booking.BookedSeats) == null)
                return false;
            else
                return true;
        }

        public void InsertBooking(BookingsModel booking)
        {

            booking.IdBooking = Guid.NewGuid();
            booking.BookingTimeStamp = DateTime.Now;
            dbContext.Bookings.InsertOnSubmit(MapModelToDbObject(booking));
            dbContext.SubmitChanges();

        }

        public void UpdateBooking(BookingsModel booking)
        {
            Booking bookingDb = dbContext.Bookings.FirstOrDefault(x => x.IdBooking == booking.IdBooking);
            if (bookingDb != null)
            {
                bookingDb.IdBooking = booking.IdBooking;
                bookingDb.BookingValidFrom = booking.BookingValidFrom;
                bookingDb.BookingValidTo = booking.BookingValidTo;
                bookingDb.BookingTimeStamp = booking.BookingTimeStamp;
                bookingDb.BookedSeats = booking.BookedSeats;
                bookingDb.IdFloor = booking.IdFloor;
                bookingDb.IdBuilding = booking.IdBuilding;
                bookingDb.UserName = booking.UserName;
                bookingDb.UserDepartment = booking.UserDepartment;
                bookingDb.UserRole = booking.UserRole;
                dbContext.SubmitChanges();
            }
        }

        public void DeleteBooking(Guid ID)
        {
            Booking bookingDb = dbContext.Bookings.FirstOrDefault(x => x.IdBooking == ID);

            if (bookingDb != null)
            {
                dbContext.Bookings.DeleteOnSubmit(bookingDb);
                dbContext.SubmitChanges();
            }

        }

        private Booking MapModelToDbObject(BookingsModel booking)
        {
            Booking bookingDb = new Booking();

            if (booking != null)
            {
                bookingDb.IdBooking = booking.IdBooking;
                bookingDb.BookingValidFrom = booking.BookingValidFrom;
                bookingDb.BookingValidTo = booking.BookingValidFrom;
                bookingDb.BookingTimeStamp = booking.BookingTimeStamp;
                bookingDb.BookedSeats = booking.BookedSeats;
                bookingDb.IdFloor = booking.IdFloor;
                bookingDb.IdBuilding = booking.IdBuilding;
                bookingDb.UserRole = booking.UserRole;
                bookingDb.UserDepartment = booking.UserDepartment;
                bookingDb.UserName = booking.UserName;
                return bookingDb;
            }

            return null;
        }

        private BookingsModel MapDbObjectToModel(Booking dbBooking)
        {
            BookingsModel booking = new BookingsModel();

            if (dbBooking != null)
            {
                booking.IdBooking = dbBooking.IdBooking;
                booking.BookingValidFrom = dbBooking.BookingValidFrom;
                booking.BookingValidTo = dbBooking.BookingValidFrom;
                booking.BookingTimeStamp = dbBooking.BookingTimeStamp;
                booking.BookedSeats = dbBooking.BookedSeats;
                booking.IdFloor = dbBooking.IdFloor;
                booking.IdBuilding = dbBooking.IdBuilding;
                booking.UserName = dbBooking.UserName;
                booking.UserDepartment = dbBooking.UserDepartment;
                booking.UserRole = dbBooking.UserRole;
                return booking;
            }

            return null;

        }

        private static List<BookingsModel> InitializeBookingsCollection()
        {
            return new List<BookingsModel>();
        }

        private void AddDbObjectTo(List<BookingsModel> bookingsList, Booking dbBooking)
        {
            bookingsList.Add(MapDbObjectToModel(dbBooking));
        }




    }
}