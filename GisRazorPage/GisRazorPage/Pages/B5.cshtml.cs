using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Newtonsoft.Json;

namespace GisRazorPage.Pages
{
    public class B5Model : PageModel
    {
        public string jsonString { get; set; }


        public void OnGet()
        {
            Console.WriteLine("Using Data Table B5");
            B5();
        }



        public void B5()
        {
            string CS = "Data Source=KINKANOM;Initial Catalog=SpatialDB3;Integrated Security=True";
            SqlConnection con = new SqlConnection(CS);
            string command = "DECLARE @bangkok GEOMETRY Select @bangkok = geom From AirPollutionPM25 Where country = 'Thailand' AND city = 'Bangkok' Select TOP 50 country, city, Geom.MakeValid().STDistance(@bangkok) AS distance, latitude, longitude From AirPollutionPM25 Where country != 'Thailand' AND city != 'Bangkok' ORDER BY distance ASC";

            SqlDataAdapter da = new SqlDataAdapter(command, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
            Console.WriteLine(dt.DataSet);

            Console.WriteLine("B5");

            jsonString = JsonConvert.SerializeObject(dt);
            Console.WriteLine(jsonString);


        }


    }
}
