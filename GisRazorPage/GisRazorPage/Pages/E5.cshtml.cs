using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace GisRazorPage.Pages
{
    public class E5Model : PageModel
    {
        public string jsonString { get; set; }
        public void OnGet()
        {
            E5();
        }

        public void E5()
        {
            string CS = "Data Source=KINKANOM;Initial Catalog=SpatialDB3;Integrated Security=True";
            SqlConnection con = new SqlConnection(CS);
            string command = "SELECT latitude, longitude FROM AirPollutionPM25 as p1 WHERE p1.country in (SELECT TOP 1 p2.country FROM AirPollutionPM25 as p2 WHERE p2.year = 2011 GROUP BY p2.country ORDER BY COUNT(p2.country) DESC) AND p1.year = 2011";

            SqlDataAdapter da = new SqlDataAdapter(command, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
            Console.WriteLine(dt.DataSet);

            Console.WriteLine("E5");

            jsonString = JsonConvert.SerializeObject(dt);
            Console.WriteLine(jsonString);


        }
    }
}
