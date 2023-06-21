using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace GisRazorPage.Pages
{
    public class C5Model : PageModel
    {
        public string jsonString { get; set; }
        public void OnGet()
        {
            C5();
        }

        public void C5()
        {
            string CS = "Data Source=KINKANOM;Initial Catalog=SpatialDB3;Integrated Security=True";
            SqlConnection con = new SqlConnection(CS);
            string command = "DECLARE @TH GEOMETRY = 'POLYGON EMPTY'; SELECT @TH = @TH.STUnion(geom.MakeValid()) FROM PM25DB.dbo.world WHERE NAME = 'Thailand' SELECT latitude, longitude FROM AirPollutionPM25 AS p WHERE p.country COLLATE SQL_Latin1_General_CP1_CI_AS IN(SELECT w.NAME FROM PM25DB.dbo.world AS w WHERE w.geom.MakeValid().STTouches(@TH.MakeValid()) = 1)";

            SqlDataAdapter da = new SqlDataAdapter(command, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
            Console.WriteLine(dt.DataSet);

            Console.WriteLine("C5");

            jsonString = JsonConvert.SerializeObject(dt);
            Console.WriteLine(jsonString);


        }
    }
}
