
declare @roomTypeId int;
declare @startDate date;
declare @endDate date;

set @startDate = '2022-01-12';
set @endDate = '2022-01-30'
set @roomTypeId = 4;

select r.*
from dbo.Rooms r
inner join dbo.RoomTypes t on t.Id = r.RoomTypeId
where r.RoomTypeId = @roomTypeId
and r.Id not in (
select b.RoomId
from dbo.Bookings b
where (@startDate < b.StartDate and @endDate > b.EndDate)
	or (b.StartDate <= @endDate and @endDate < b.EndDate)
	or (b.StartDate <= @startDate and @startDate < b.EndDate)
)
