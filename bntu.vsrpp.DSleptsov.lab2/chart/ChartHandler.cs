using bntu.vsrpp.DSleptsov.lab2.api.loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static bntu.vsrpp.DSleptsov.lab2.api.Rates;

namespace bntu.vsrpp.DSleptsov.lab2.chart
{
    public static class ChartHandler
    {
        public static Rate rate { get; set; }
        public static DateTime fromDate { get; set; }
        public static DateTime toDate { get; set; }

        public static async Task LoadRateShort()
        {
            await RatesLoader.loadRateShort(rate.Cur_ID, fromDate, toDate);
        }

        public static DateTime FindDateWithMax()
        {
            decimal? maxOfficialRate = RatesLoader.RATES_SHORT.Max(r => r.Cur_OfficialRate);
            return RatesLoader.RATES_SHORT.FirstOrDefault(r => r.Cur_OfficialRate == maxOfficialRate)?.Date ?? DateTime.MinValue;
        }

        public static DateTime FindDateWithMin()
        {
            decimal? minOfficialRate = RatesLoader.RATES_SHORT.Min(r => r.Cur_OfficialRate);
            return RatesLoader.RATES_SHORT.FirstOrDefault(r => r.Cur_OfficialRate == minOfficialRate)?.Date ?? DateTime.MinValue;
        }

        public static decimal? FindAverageOfThePeriod()
        {
            decimal? average = RatesLoader.RATES_SHORT.Average(r => r.Cur_OfficialRate);
            return Math.Round(average.Value, 2);
        }

        public static async Task DrawChart(PictureBox canvas)
        {
            await LoadRateShort();
            Graphics g = Graphics.FromImage(canvas.Image);
            Pen pen = new Pen(Color.Blue);

            double canvasWidth = canvas.Width;
            double canvasHeight = canvas.Height;

            decimal? maxOfficialRate = RatesLoader.RATES_SHORT.Max(r => r.Cur_OfficialRate);
            decimal? minOfficialRate = RatesLoader.RATES_SHORT.Min(r => r.Cur_OfficialRate);

            double xScale = canvasWidth / (toDate - fromDate).TotalDays;
            double yScale = canvasHeight / (double)(maxOfficialRate - minOfficialRate);

            int segmentCount = RatesLoader.RATES_SHORT.Count;
            double segmentSpacing = canvasWidth / (segmentCount - 1);

            double startX = 0;
            double endY = 0;
            double startY = canvasHeight - ((double)(RatesLoader.RATES_SHORT[0].Cur_OfficialRate - minOfficialRate.Value) * yScale);

            double endX;
            Point startPoint = new Point((int)startX, (int)startY);

            for (int i = 1; i < segmentCount; i++)
            {
                endX = i * segmentSpacing;
                endY = canvasHeight - ((double)(RatesLoader.RATES_SHORT[i].Cur_OfficialRate - minOfficialRate.Value) * yScale);
                g.DrawLine(pen, (int)startX, (int)startY, (int)endX, (int)endY);
                canvas.Invalidate();
                startX = endX;
                startY = endY;

            }
        }

        public static bool CheckDates(DateTime? from, DateTime? to)
        {
            return from < to;
        }
    }
}
