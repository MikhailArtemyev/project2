
using System.IO;
using System.Text;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

const string fPath = $"/Users/michaelartemyev/Library/Mobile Documents/com~apple~CloudDocs/programming/DBemitation.txt";

static DataBlock[] WListCreation() // building an array of blocks by a path to DB
{
    DataBlock[] res = new DataBlock[7];
    try
    {
        SqlConnectionStringBuilder builder1 = new SqlConnectionStringBuilder();
        builder1.DataSource = "localhost";
        builder1.UserID = "sa";
        builder1.Password = "C0mpl3xpa$$w0rd13";
        builder1.InitialCatalog = "master";

        using (SqlConnection connection = new SqlConnection(builder1.ConnectionString))
        {
            connection.Open();
            string sql = "USE weather; SELECT Date, Mint, Maxt FROM weatherInBournemouth;";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var i = 0;
                    while (reader.Read())
                    {
                        //reader.GetString(1) + reader.GetString(2));
                        DateTime dt = DateTime.Parse(reader.GetString(0));
                        int mnT = reader.GetInt32(1);
                        int mxT = reader.GetInt32(2);
                        DataBlock b = new DataBlock(dt, mnT, mxT);
                        res[i] = b;
                        i += 1;
                    }

                }
            }
        }
        return res;
    }
    catch (SqlException e)
    {
        File.WriteAllText(fPath, e.ToString());
        return res;
    }
}

DataBlock[] BC = WListCreation(); // building a blocks collection 

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/{date}", (string date) =>
{
    DateTime date1;
    DateTime.TryParse(date, out date1);
    
        DataBlock? b = BC.FirstOrDefault(u => u.Date == date1);
        // if not found than return an error 
        if (b == null)
        {   
            return Results.NotFound(new { message = $"No weather information for {date}. Check if the date submited is correct (day.month.year)" });
        }
        else
        {
            return Results.Json(b);
        }
});

app.Run();
