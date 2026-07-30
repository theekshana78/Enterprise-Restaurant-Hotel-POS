using System;
using System.Collections.Generic;
using System.Linq;
using EnterprisePOS.Core;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;

namespace EnterprisePOS.Services
{
    public class AISalesForecastResult
    {
        public string ProductName { get; set; } = string.Empty;
        public double CurrentStock { get; set; }
        public double PredictedWeeklyDemand { get; set; }
        public double RecommendedOrderQty { get; set; }
        public string RiskLevel { get; set; } = "Normal";
    }

    public class AIFraudAlert
    {
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime AlertTime { get; set; }
        public string RiskFactor { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    public class AIService
    {
        private readonly POSDbContext _context;

        public AIService(POSDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Real Production Sales Velocity & Demand Forecasting based on actual SQLite transaction records.
        /// </summary>
        public List<AISalesForecastResult> PredictSalesDemand()
        {
            var products = _context.Products.Where(p => p.ItemType == ItemType.SalableProduct).ToList();
            var invoiceItems = _context.InvoiceItems.ToList();
            var invoices = _context.Invoices.ToList();

            var results = new List<AISalesForecastResult>();

            // Determine active trading days window
            int tradingDays = 1;
            if (invoices.Any())
            {
                var minDate = invoices.Min(i => i.InvoiceDate);
                tradingDays = Math.Max(1, (int)Math.Ceiling((DateTime.Now - minDate).TotalDays));
            }

            foreach (var p in products)
            {
                double totalUnitsSold = invoiceItems.Where(i => i.ProductId == p.Id).Sum(i => i.Quantity);
                
                // Real sales velocity per day
                double dailyVelocity = totalUnitsSold / tradingDays;
                double predictedWeeklyDemand = dailyVelocity * 7;

                // Production safety stock formula: Safety Buffer = 1.25x Weekly Demand
                double targetStock = predictedWeeklyDemand * 1.25;
                double recommendedOrder = Math.Max(0, targetStock - p.CurrentStock);

                string riskLevel = "Normal (Adequate Stock)";
                if (p.CurrentStock <= 0)
                {
                    riskLevel = "CRITICAL (Stockout / Out of Stock)";
                }
                else if (dailyVelocity > 0 && p.CurrentStock < dailyVelocity * 2)
                {
                    riskLevel = "HIGH (Depletion Expected in < 48 Hours)";
                }
                else if (p.CurrentStock > (predictedWeeklyDemand * 4) + 10)
                {
                    riskLevel = "LOW (Overstocked / Slow Velocity)";
                }

                results.Add(new AISalesForecastResult
                {
                    ProductName = p.Name,
                    CurrentStock = p.CurrentStock,
                    PredictedWeeklyDemand = Math.Round(predictedWeeklyDemand, 1),
                    RecommendedOrderQty = Math.Round(recommendedOrder, 0),
                    RiskLevel = riskLevel
                });
            }

            return results;
        }

        /// <summary>
        /// Multi-Factor Production Security Audit Engine for Cashier Fraud & Anomaly Detection
        /// </summary>
        public List<AIFraudAlert> DetectFraudulentActivity()
        {
            var alerts = new List<AIFraudAlert>();
            var recentInvoices = _context.Invoices.OrderByDescending(i => i.InvoiceDate).Take(100).ToList();

            foreach (var inv in recentInvoices)
            {
                // Anomaly 1: Excessive Discount (> 20% or > LKR 1,500)
                if (inv.SubTotal > 0 && ((inv.Discount / inv.SubTotal) > 0.20m || inv.Discount >= 1500))
                {
                    alerts.Add(new AIFraudAlert
                    {
                        InvoiceNo = inv.InvoiceNo,
                        AlertTime = inv.InvoiceDate,
                        RiskFactor = "EXCESSIVE MANUAL DISCOUNT",
                        Details = $"Cashier '{inv.CashierName}' applied LKR {inv.Discount:N0} discount ({inv.Discount / inv.SubTotal:P0} of subtotal)"
                    });
                }

                // Anomaly 2: Zero-value sale transaction
                if (inv.GrandTotal == 0 && inv.SubTotal > 0)
                {
                    alerts.Add(new AIFraudAlert
                    {
                        InvoiceNo = inv.InvoiceNo,
                        AlertTime = inv.InvoiceDate,
                        RiskFactor = "ZERO VALUE SALE / VOID OVERRIDE",
                        Details = $"Transaction for LKR {inv.SubTotal:N0} cleared to LKR 0 by '{inv.CashierName}'"
                    });
                }

                // Anomaly 3: After-Hours Transaction (12:00 AM - 5:00 AM)
                if (inv.InvoiceDate.Hour >= 0 && inv.InvoiceDate.Hour < 5)
                {
                    alerts.Add(new AIFraudAlert
                    {
                        InvoiceNo = inv.InvoiceNo,
                        AlertTime = inv.InvoiceDate,
                        RiskFactor = "AFTER-HOURS SALES AUDIT",
                        Details = $"Transaction logged outside standard trading hours at {inv.InvoiceDate:HH:mm:ss}"
                    });
                }
            }

            return alerts;
        }
    }
}
