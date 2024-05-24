select top 10 c.Name as CompanyName, SUM(tk.Price) as TotalRevenue from
Companies As c
join Drivers as d on c.CompanyId = d.CompanyId
join Buses AS b on b.DriverId = d.DriverId
join BusScheduleEntries as bse on b.BusId = bse.BusId
join Trips as t on t.BusScheduleEntryId = bse.BusScheduleEntryId
join TicketPayments as tp on t.TripId = tp.TripId
join Tickets as tk on tp.TicketId = tk.TicketId
group by c.Name
order by TotalRevenue desc


select top 10
	r.RouteId,
	(r.Origin + ' - ' + r.Destination) RouteName,
	ROUND(AVG(b.Rating), 2) as AvgBusRating
from Routes as r
join BusScheduleEntries as bse on r.RouteId = bse.RouteId
join BusDetailsView as b on b.BusId = bse.BusId
where Rating > 0.0
group by r.RouteId, r.Origin, r.Destination
order by AvgBusRating desc


select top 10
r.RouteId,
(r.Origin + ' - ' + r.Destination) RouteName,
 SUM(tk.Price) as TotalRevenue
from Routes as r
join BusScheduleEntries as bse on r.RouteId = bse.RouteId
join Trips as t on t.BusScheduleEntryId = bse.BusScheduleEntryId
join TicketPayments as tp on t.TripId = tp.TripId
join Tickets as tk on tp.TicketId = tk.TicketId
group by r.RouteId, r.Origin, r.Destination
order by TotalRevenue desc

-- the most crossed distance among buses
select b.Number, SUM(r.OverallDistance) as DistanceTravelled 
from Buses as b
join BusScheduleEntries as bse on b.BusId = bse.BusId
join Routes as r on r.RouteId = bse.RouteId
join Trips as t on t.BusScheduleEntryId = bse.BusScheduleEntryId
where t.Status != 'Canceled'
group by b.BusId
order by DistanceTravelled desc


WITH RankedDistances AS (
    SELECT 
        b.BusId, 
        r.OverallDistance AS DistanceTravelled,
        ROW_NUMBER() OVER (PARTITION BY r.OverallDistance ORDER BY b.BusId) AS RowNumber
    FROM 
        Buses AS b
    JOIN 
        BusScheduleEntries AS bse ON b.BusId = bse.BusId
    JOIN 
        Routes AS r ON r.RouteId = bse.RouteId
    JOIN 
        Trips AS t ON t.BusScheduleEntryId = bse.BusScheduleEntryId
    WHERE 
        t.Status != 'Canceled'
)
SELECT 
    BusId, 
    DistanceTravelled
FROM 
    RankedDistances
WHERE 
    RowNumber = 1
ORDER BY 
    DistanceTravelled DESC;

--var rankedDistances = from b in dbContext.Buses
--                      join bse in dbContext.BusScheduleEntries on b.BusId equals bse.BusId
--                      join r in dbContext.Routes on bse.RouteId equals r.RouteId
--                      join t in dbContext.Trips on bse.BusScheduleEntryId equals t.BusScheduleEntryId
--                      where t.Status != "Canceled"
--                      orderby r.OverallDistance descending, b.BusId
--                      select new 
--                      {
--                          b.BusId,
--                          DistanceTravelled = r.OverallDistance
--                      };

--var firstDistances = rankedDistances.GroupBy(rd => rd.DistanceTravelled)
--                                     .Select(g => g.First());

--var query = from fd in firstDistances
--            orderby fd.DistanceTravelled descending
--            select fd;
