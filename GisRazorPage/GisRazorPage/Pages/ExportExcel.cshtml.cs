using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Data.SqlClient;

namespace GisRazorPage.Pages
{
    public class ExportExcelModel : PageModel
    {
        public string filePathA4 = Directory.GetCurrentDirectory() + @"\Files\A4.xlsx";
        public string filePathB4 = Directory.GetCurrentDirectory() + @"\Files\B4.xlsx";
        public string filePathC4 = Directory.GetCurrentDirectory() + @"\Files\C4.xlsx";
        public string filePathD4 = Directory.GetCurrentDirectory() + @"\Files\D4.xlsx";
        public string country = "";
        public string color = "";
        public int Year = 0;

        public void OnGet()
        {
        }
        

        public ActionResult OnPostDownloadFileA4()
        {
            A4();
            var file = filePathA4;
            return File(new FileStream(file, FileMode.Open), "application/octet-stream", "A4.xlsx");
        }
        public ActionResult OnPostDownloadFileB4()
        {
            B4();
            var file = filePathB4;
            return File(new FileStream(file, FileMode.Open), "application/octet-stream", "B4.xlsx");
        }

        public ActionResult OnPostDownloadFileC4()
        {
            String tmp = Request.Form["country"];
            try
            {
                country = tmp;
                C4();
                var file = filePathC4;
                return File(new FileStream(file, FileMode.Open), "application/octet-stream", "C4.xlsx");
            }
            catch
            {
                return null;
            }
            return null;
        }

        public ActionResult OnPostDownloadFileD4()
        {
            String tmp1 = Request.Form["color"];
            String tmp2 = Request.Form["Year"];
            try
            {
                color = tmp1;
                Year = Int32.Parse(tmp2);
                D4();
                var file = filePathD4;
                return File(new FileStream(file, FileMode.Open), "application/octet-stream", "D4.xlsx");
            }
            catch
            {
                return null;
            }
            return null;
        }

        public void A4()
        {
            string CS = "Data Source=KINKANOM;Initial Catalog=SpatialDB3;Integrated Security=True";
            SqlConnection con = new SqlConnection(CS);
            string command = "Select country, city, pm25, year From AirPollutionPM25 Where pm25 > 50 AND year = 2015";

            SqlDataAdapter da = new SqlDataAdapter(command, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
            Console.WriteLine(dt.DataSet);

            Console.WriteLine("Test2");

            IWorkbook workbook = new XSSFWorkbook();
            ISheet worksheet = workbook.CreateSheet("Sheet1");


            int rowNumber = 0;
            IRow row = worksheet.CreateRow(rowNumber);

            using (var fs = new FileStream(filePathA4, FileMode.Create, FileAccess.Write))
            {
                row = worksheet.CreateRow(rowNumber++);

                ICell cell = row.CreateCell(0);
                cell = row.CreateCell(0);
                cell.SetCellValue("country");

                cell = row.CreateCell(1);
                cell.SetCellValue("city");

                cell = row.CreateCell(2);
                cell.SetCellValue("pm25");

                cell = row.CreateCell(3);
                cell.SetCellValue("year");

                foreach (DataRow rowTb in dt.Rows)
                {
                    row = worksheet.CreateRow(rowNumber++);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(rowTb["country"].ToString());

                    cell = row.CreateCell(1);
                    cell.SetCellValue(rowTb["city"].ToString());

                    cell = row.CreateCell(2);
                    cell.SetCellValue(rowTb["pm25"].ToString());

                    cell = row.CreateCell(3);
                    cell.SetCellValue(rowTb["year"].ToString());
                }

                workbook.Write(fs);
            }

        }

        public void B4()
        {
            string CS = "Data Source=KINKANOM;Initial Catalog=SpatialDB3;Integrated Security=True";
            SqlConnection con = new SqlConnection(CS);
            string command = "Select AVG(pm25) AS pm25avg, country From AirPollutionPM25 GROUP BY country ORDER BY pm25avg DESC";

            SqlDataAdapter da = new SqlDataAdapter(command, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
            Console.WriteLine(dt.DataSet);

            Console.WriteLine("Test2");

            IWorkbook workbook = new XSSFWorkbook();
            ISheet worksheet = workbook.CreateSheet("Sheet1");


            int rowNumber = 0;
            IRow row = worksheet.CreateRow(rowNumber);

            using (var fs = new FileStream(filePathB4, FileMode.Create, FileAccess.Write))
            {
                row = worksheet.CreateRow(rowNumber++);

                ICell cell = row.CreateCell(0);
                cell = row.CreateCell(0);
                cell.SetCellValue("pm25avg");

                cell = row.CreateCell(1);
                cell.SetCellValue("country");

                foreach (DataRow rowTb in dt.Rows)
                {
                    row = worksheet.CreateRow(rowNumber++);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(rowTb["pm25avg"].ToString());

                    cell = row.CreateCell(1);
                    cell.SetCellValue(rowTb["country"].ToString());
                }

                workbook.Write(fs);
            }

        }

        public void C4()
        {
            string CS = "Data Source=KINKANOM;Initial Catalog=SpatialDB3;Integrated Security=True";
            SqlConnection con = new SqlConnection(CS);
            string command = "DECLARE @input_country NVARCHAR(255) Select @input_country = '"+ country+"' From AirPollutionPM25 Select country, pm25, Year From AirPollutionPM25 Where country = @input_country ORDER BY year";

            SqlDataAdapter da = new SqlDataAdapter(command, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
            Console.WriteLine(dt.DataSet);

            Console.WriteLine("Test2");

            IWorkbook workbook = new XSSFWorkbook();
            ISheet worksheet = workbook.CreateSheet("Sheet1");


            int rowNumber = 0;
            IRow row = worksheet.CreateRow(rowNumber);

            using (var fs = new FileStream(filePathC4, FileMode.Create, FileAccess.Write))
            {
                row = worksheet.CreateRow(rowNumber++);

                ICell cell = row.CreateCell(0);
                cell = row.CreateCell(0);
                cell.SetCellValue("country");

                cell = row.CreateCell(1);
                cell.SetCellValue("pm25");

                cell = row.CreateCell(2);
                cell.SetCellValue("Year");

                foreach (DataRow rowTb in dt.Rows)
                {
                    row = worksheet.CreateRow(rowNumber++);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(rowTb["country"].ToString());

                    cell = row.CreateCell(1);
                    cell.SetCellValue(rowTb["pm25"].ToString());

                    cell = row.CreateCell(2);
                    cell.SetCellValue(rowTb["Year"].ToString());
                }

                workbook.Write(fs);
            }

        }

        public void D4()
        {
            string CS = "Data Source=KINKANOM;Initial Catalog=SpatialDB3;Integrated Security=True";
            SqlConnection con = new SqlConnection(CS);
            string command = "DECLARE @input_year NVARCHAR(255) Select @input_year = '"+Year+"' From AirPollutionPM25 DECLARE @input_color_pm NVARCHAR(255) Select @input_color_pm = '"+color+"' From AirPollutionPM25 Select @input_year AS year, @input_color_pm AS color, SUM(population) AS totalPm25 From AirPollutionPM25";

            SqlDataAdapter da = new SqlDataAdapter(command, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
            Console.WriteLine(dt.DataSet);

            Console.WriteLine("Test2");

            IWorkbook workbook = new XSSFWorkbook();
            ISheet worksheet = workbook.CreateSheet("Sheet1");


            int rowNumber = 0;
            IRow row = worksheet.CreateRow(rowNumber);

            using (var fs = new FileStream(filePathD4, FileMode.Create, FileAccess.Write))
            {
                row = worksheet.CreateRow(rowNumber++);

                ICell cell = row.CreateCell(0);
                cell = row.CreateCell(0);
                cell.SetCellValue("year");

                cell = row.CreateCell(1);
                cell.SetCellValue("color");

                cell = row.CreateCell(2);
                cell.SetCellValue("totalPm25");

                foreach (DataRow rowTb in dt.Rows)
                {
                    row = worksheet.CreateRow(rowNumber++);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(rowTb["year"].ToString());

                    cell = row.CreateCell(1);
                    cell.SetCellValue(rowTb["color"].ToString());

                    cell = row.CreateCell(2);
                    cell.SetCellValue(rowTb["totalPm25"].ToString());
                }

                workbook.Write(fs);
            }

        }

    }
}
