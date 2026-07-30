using System;
using System.Collections.Generic;
using System.Linq;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;

namespace EnterprisePOS.Services
{
    public class AuditService
    {
        private readonly POSDbContext _context;

        public AuditService(POSDbContext context)
        {
            _context = context;
        }

        public void LogActivity(string username, string action, string details, string terminal = "POS-01")
        {
            try
            {
                var log = new AuditLog
                {
                    Timestamp = DateTime.Now,
                    Username = username,
                    Action = action,
                    Details = details,
                    IpOrTerminal = terminal
                };

                _context.AuditLogs.Add(log);
                _context.SaveChanges();
            }
            catch
            {
                // Silent fail for audit log resilience
            }
        }

        public List<AuditLog> GetRecentLogs(int maxCount = 100)
        {
            return _context.AuditLogs.OrderByDescending(a => a.Timestamp).Take(maxCount).ToList();
        }
    }
}
