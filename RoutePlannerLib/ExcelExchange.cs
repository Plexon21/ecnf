using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Export
{
    public class ExcelExchange
    {
        private Application app;
        public void WriteToFile(string fileName, IEnumerable<Link> links)
        {
                app = new Application();
                if (app == null)
                {
                    Console.WriteLine("Excel not available");
                    return;
                }
                app.Visible = false;
                Workbook wb = app.Workbooks.Add();
                Worksheet ws = wb.Worksheets[1];

                int columns = 4;

                object[,] dataToInsert = new object[links.Count() + 1, columns];

                dataToInsert[0, 0] = "From";
                dataToInsert[0, 1] = "To";
                dataToInsert[0, 2] = "Distance";
                dataToInsert[0, 3] = "Mode";


                int row = 1;
                foreach (var link in links)
                {
                    dataToInsert[row, 0] = link.FromCity.Name;
                    dataToInsert[row, 1] = link.ToCity.Name;
                    dataToInsert[row, 2] = link.Distance;
                    dataToInsert[row, 3] = link.TransportMode;
                    row++;
                }


                Range styleRange = ws.Range["A1:D1"];
                styleRange.Font.Size = 14;
                styleRange.Font.Bold = true;
                styleRange.EntireColumn.ColumnWidth = 20;



                Range cellRange = ws.Range[ws.Cells[1, 1], ws.Cells[row + 1, columns]];
                cellRange.Value2 = dataToInsert;
                cellRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                app.DisplayAlerts = false;
                wb.SaveAs(fileName);
                wb.Close();
                app.Quit();
            }
        }
    }
}
