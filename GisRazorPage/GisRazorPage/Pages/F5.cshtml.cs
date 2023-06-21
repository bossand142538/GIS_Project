using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace GisRazorPage.Pages
{
    public class F5Model : PageModel
    {
        public int Year { get; set; } = 2013;
        public string jsonString { get; set; }


        public void OnGet()
        {
            Console.WriteLine("Using Data Table");
            F5();
        }

        public void OnPost()
        {
            String tmp = Request.Form["Year"];


            try
            {

                Year = Int32.Parse(tmp);
                F5();
            }
            catch
            {

                F5();
            }




        }

        public void F5()
        {
            string CS = "Data Source=KINKANOM;Initial Catalog=SpatialDB3;Integrated Security=True";
            SqlConnection con = new SqlConnection(CS);
            string command = "DECLARE @input_year INT Select @input_year = year From AirPollutionPM25 WHERE year = "+ Year + " SELECT latitude, longitude From AirPollutionPM25 Where year = @input_year AND wbinc16_text LIKE 'low income'";

            SqlDataAdapter da = new SqlDataAdapter(command, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
            Console.WriteLine(dt.DataSet);

            Console.WriteLine("F5");

            jsonString = JsonConvert.SerializeObject(dt);
            Console.WriteLine(jsonString);


        }
    }
}
