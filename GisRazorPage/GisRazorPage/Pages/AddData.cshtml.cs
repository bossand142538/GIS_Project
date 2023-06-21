using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Data;

using ClosedXML.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data.SqlClient;

namespace GisRazorPage.Pages
{
    public class AddDataModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public string fileName = "";
        public string filePath = Directory.GetCurrentDirectory()+@"\Files\";
        public string filePathOutput = @"C:\\Users\\bossa\\Documents\\KMITL\\spatial\\Book2.xlsx";


        public AddDataModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        
        public IFormFile Uploadfiles { get; set; }

        public void OnGet()
        {
            
        }
       

        public async Task OnPostAsync()
        {
            //upload File to server
            var Fileupload = Path.Combine(_webHostEnvironment.ContentRootPath, "Files", Uploadfiles.FileName);
            using (var Fs = new FileStream(Fileupload, FileMode.Create))
            {
                await Uploadfiles.CopyToAsync(Fs);
                fileName = Uploadfiles.FileName;
                filePath = filePath + fileName;
                Console.WriteLine(filePath);



                ViewData["Message"] = "The Select File " + Uploadfiles.FileName + " Is Upload Sucessfully ";
                fileName = Uploadfiles.FileName;


            }

            ReadFile();
        }




        public void ReadFile()
        {
            try
            {
                //Creat for Read
                IWorkbook workbook = null;
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                if (filePath.IndexOf(".xlsx") > 0)
                    workbook = new XSSFWorkbook(fs);
                else if (filePath.IndexOf(".xls") > 0)
                    workbook = new HSSFWorkbook(fs);

                ISheet sheet = workbook.GetSheetAt(0);


                if (sheet != null)
                {
                    int rowCount = sheet.LastRowNum;
                    Console.WriteLine(rowCount);


                    for (int i = 1; i <= rowCount; i++)
                    {
                        IRow curRow = sheet.GetRow(i);

                        string cellValue0 = curRow.GetCell(0).ToString();
                        string cellValue1 = curRow.GetCell(1).ToString().Replace("'", "''");
                        string cellValue2 = curRow.GetCell(2).ToString();
                        string cellValue3 = curRow.GetCell(3).ToString();
                        string cellValue4 = curRow.GetCell(4).ToString();
                        string cellValue5 = curRow.GetCell(5).ToString();
                        string cellValue6 = curRow.GetCell(6).ToString();
                        string cellValue7 = curRow.GetCell(7).ToString();
                        string cellValue8 = curRow.GetCell(8).ToString();
                        string cellValue9 = curRow.GetCell(9).ToString();
                        string cellValue10 = curRow.GetCell(10).ToString();


                        //Console.WriteLine(cellValue0 + "  " + cellValue1 + "  " + cellValue2 + " " + cellValue3 + " " + cellValue4 + " " + cellValue5 + " " + cellValue6 + " " + cellValue7 + " " + cellValue8 + " " + cellValue9 + " " + cellValue10);


                        string command = "INSERT INTO AirPollutionPM25 ([country],[city],[Year],[pm25],[latitude],[longitude],[population],[wbinc16_text],[Regian],[conc_pm25],[color_pm25],[Geom]) VALUES('" + cellValue0 + "', '" + cellValue1 + "', " + cellValue2 + ", " + cellValue3 + ", " + cellValue4 + ", " + cellValue5 + ", " + cellValue6 + ", '" + cellValue7 + "', '" + cellValue8 + "', '" + cellValue9 + "', '" + cellValue10 + "', geometry::Point(" + cellValue5 + ", " + cellValue4 + ", 4326));";
                        Insert(command);
                        //Console.WriteLine(i);
                    }



                }

            }
            catch (Exception exception)
            {

            }
        }

        public void Insert(string command)
        {
            string CS = "Data Source=KINKANOM;Initial Catalog=SpatialDB3;Integrated Security=True";
            SqlConnection con = new SqlConnection(CS);


            con.Open();
            SqlCommand cmd = new SqlCommand(command, con);
            cmd.ExecuteNonQuery();
            con.Close();


        }

    }
}
