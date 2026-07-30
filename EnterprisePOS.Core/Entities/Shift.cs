using System;

namespace EnterprisePOS.Core.Entities
{
    public class Shift
    {
        public int Id { get; set; }
        public string ShiftNo { get; set; } = string.Empty;
        public string CashierUsername { get; set; } = string.Empty;
        public string TerminalName { get; set; } = "POS-01";
        public decimal OpeningCash { get; set; }
        public decimal? ClosingCash { get; set; }
        public decimal? ExpectedCash { get; set; }
        public decimal? CashDifference { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime? EndTime { get; set; }
        public bool IsClosed { get; set; } = false;
        public string? Notes { get; set; }
    }
}
