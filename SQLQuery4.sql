

select b.*, g.*, r.*, rt.*
from dbo.Bookings b
inner join dbo.Guests g on b.GuestId = g.Id
inner join dbo.Rooms r on b.RoomId = r.Id
inner join dbo.RoomTypes rt on r.RoomTypeId = rt.Id






