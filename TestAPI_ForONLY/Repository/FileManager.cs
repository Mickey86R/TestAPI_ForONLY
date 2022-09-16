using OfficeOpenXml;
using System;
using System.Data;
using System.IO;


namespace TestAPI_ForONLY.Repository
{
    public static class FileManager
    {
        public static async Task<DataTable> GetTableFromFile(string path)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var excelPack = new ExcelPackage(path))
            {
                //Load excel stream
                using (var stream = File.OpenRead(path))
                {
                    excelPack.Load(stream);
                }

                var ws = excelPack.Workbook.Worksheets[0];

                DataTable excelTable = new DataTable();

                excelTable.Rows.Add();
                excelTable.Columns.Add();
                excelTable.Columns.Add();
                excelTable.Columns.Add();
                excelTable.Columns.Add();
                excelTable.Columns.Add();

                for (int i = 1; ws.Cells[i, 1].Value != null; i++)
                {
                    excelTable.Rows.Add(ws.Row(i));

                    excelTable.Rows[i][1] = ws.Cells[i, 1].Value;
                    if (ws.Cells[i, 2].Value != null)
                        excelTable.Rows[i][2] = ws.Cells[i, 2].Value;
                    else excelTable.Rows[i][2] = "";
                    excelTable.Rows[i][3] = ws.Cells[i, 3].Value;
                    excelTable.Rows[i][4] = ws.Cells[i, 4].Value;

                    //ShowCellsInRow(excelTable.Rows[i].ItemArray);
                }

                return excelTable;
            }
        }

        static void ShowCellsInRow(object?[] cells)
        {
            foreach (object cell in cells)
                Console.Write($"{(string)cell}\t");
            Console.WriteLine("\n");
        }
    }
}
