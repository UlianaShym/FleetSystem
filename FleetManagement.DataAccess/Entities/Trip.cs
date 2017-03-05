using System;

namespace FleetManagement.DataAccess.Entities
{
    public class Trip
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public int Distance { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
