using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAppLibrary.Data
{
    public class SqlData : IDatabaseData
    {
        private readonly ISqlDataAccess _db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }
        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            return _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetAvailableTypes",
                                                 new { startDate, endDate },
                                                 connectionStringName,
                                                 true);
        }

        public void BookRoom(DateTime startDate,
                             DateTime endDate,
                             string firstName,
                             string lastName,
                             int roomTypeId)
        {
            GuestModel guest = _db.LoadData<GuestModel, dynamic>("dbo.spGuests_Insert",
                                                                 new { firstName, lastName },
                                                                 connectionStringName,
                                                                 true).First();

            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("SELECT * From dbo.RoomTypes where Id = @Id",
                                                                          new { Id = roomTypeId },
                                                                          connectionStringName,
                                                                          false).First(); //FALSE för det är EJ en stored procedure

            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date); //.Date för att bara få datumet. Inte timme och minut

            List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>("dbo.spRooms_GetAvailableRooms",
                                                                 new { startDate, endDate, roomTypeId },
                                                                 connectionStringName,
                                                                 true).ToList();

            _db.SaveData("dbo.spBookings_Insert",
                        new
                        {
                            roomId = availableRooms.First().Id,
                            guestId = guest.Id,
                            startDate = startDate,
                            endDate = endDate,
                            totalCost = timeStaying.Days * roomType.Price
                        },
                        connectionStringName,
                        true);
        }

        public List<BookingsFullModel> SearchBookings(string lastName)
        {
            return _db.LoadData<BookingsFullModel, dynamic>("dbo.spBookingsSearch",
                                                     new { lastName, startDate = DateTime.Now.Date },
                                                     connectionStringName,
                                                     true);
        }

        public void CheckInGuest(int bookingId)
        {
            _db.SaveData("dbo.spBookings_CheckIn", new { Id = bookingId }, connectionStringName, true);
        }

        public RoomTypeModel GetRoomTypeById(int id)
        {
            return _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetById",
                                                       new { id },
                                                       connectionStringName,
                                                       true).FirstOrDefault();
        }
    }
}
