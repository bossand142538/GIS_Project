using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Newtonsoft.Json;

namespace GisRazorPage.Pages
{
    public class A5Model : PageModel
    {
        public int Year { get; set; } = 2017;
        public string jsonString { get; set; }
        
        public void OnGet()
        {
            Console.WriteLine("Using Data Table");
            A5();
        }

        public void OnPost()
        {
            String tmp = Request.Form["Year"];

            
                try {

                Year = Int32.Parse(tmp);
                A5();
                }
                catch {
                 
                A5();
            }
            
            

            
        }

        

        public void A5()
        {
            string CS = "Data Source=KINKANOM;Initial Catalog=SpatialDB3;Integrated Security=True";
            SqlConnection con = new SqlConnection(CS);
            string command = "DECLARE @input_year INT Select @input_year = year From AirPollutionPM25 WHERE year = "+Year+" Select latitude, longitude From AirPollutionPM25 WHERE year = @input_year";

            SqlDataAdapter da = new SqlDataAdapter(command, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
            Console.WriteLine(dt.DataSet);

            Console.WriteLine("Test2");

            jsonString = JsonConvert.SerializeObject(dt);
            Console.WriteLine(jsonString);


        }


    }
}
