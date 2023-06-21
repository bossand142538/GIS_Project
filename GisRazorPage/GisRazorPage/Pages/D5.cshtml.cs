using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace GisRazorPage.Pages
{
    public class D5Model : PageModel
    {
        public string jsonString { get; set; }
        public void OnGet()
        {
            D5();
        }
        public void D5()
        {
            string CS = "Data Source=KINKANOM;Initial Catalog=SpatialDB3;Integrated Security=True";
            SqlConnection con = new SqlConnection(CS);
            string command = " DECLARE @TH geometry SELECT @TH = geometry::EnvelopeAggregate(Geom) FROM AirPollutionPM25 WHERE country = 'Thailand' AND Year = 2009 DECLARE @g geometry; SET @g = @TH; DECLARE @leg INT; SET @leg = @g.STNumPoints(); DECLARE @Points TABLE(geom GEOMETRY) DECLARE @cnt INT = 1; WHILE @cnt <= @leg BEGIN INSERT INTO @Points VALUES(@g.STPointN(@cnt)); SET @cnt = @cnt + 1; END; SELECT geom.STX AS longitude, geom.STY AS latitude FROM @Points";

            SqlDataAdapter da = new SqlDataAdapter(command, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
            Console.WriteLine(dt.DataSet);

            Console.WriteLine("D5");

            jsonString = JsonConvert.SerializeObject(dt);
            Console.WriteLine(jsonString);


        }
    }
}
