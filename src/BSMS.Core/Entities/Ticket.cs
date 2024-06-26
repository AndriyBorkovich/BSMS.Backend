﻿using System.ComponentModel.DataAnnotations.Schema;
using BSMS.Core.Enums;

namespace BSMS.Core.Entities;

public class Ticket
{
    public int TicketId { get; set; }
    public int SeatId { get; set; }
    public int StartStopId { get; set; }
    public int EndStopId { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public bool IsSold { get; set; }
    public TicketStatus Status {get; set; }

    public Stop StartStop { get; set; } = null!;
    public Stop EndStop { get; set; } = null!;
    public Seat Seat { get; set; } = null!;
    public TicketPayment Payment { get; set; } = null!;
}
