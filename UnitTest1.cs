
namespace SeleniumProject
{
    public class UnitTest1
    {

        private string homeUrl = $"https://www.metoffice.gov.uk/weather/forecast/gcn8t1p3y#?date=";
        [Fact]
        [Trait("Category", "base_1")]
        public void GettingTheData()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "localhost";
                builder.UserID = "sa";
                builder.Password = "C0mpl3xpa$$w0rd13";
                builder.InitialCatalog = "master";

                using (IWebDriver driver = new ChromeDriver())
                {
                    driver.Navigate().GoToUrl(homeUrl);
                    DateTime StartDate = DateTime.Today;
                    StringBuilder sql1 = new StringBuilder();
                    sql1.Append("USE weather; DELETE From weatherInBournemouth;");
                    sql1.Append("INSERT weatherInBournemouth (Date, Mint, Maxt) VALUES ");

                    string relatedData = driver.FindElement(By.Id($"tabDay{0}")).Text;
                    string[] sfComponents = relatedData.Split('\n');
                    string daTe = $"{StartDate}".Split(' ')[0];
                    string MinT = sfComponents[1].Split("°")[1];
                    string MaxT = sfComponents[1].Split("°")[0];

                    string qw = $"(N'{daTe}', N'{MinT}', N'{MaxT}'), ";
                    sql1.Append(qw);



                    for (int i = 1; i < 6; i++)
                    {
                        string relatedDataL = driver.FindElement(By.Id($"tabDay{i}")).Text;
                        string[] sfComponentsL = relatedDataL.Split('\n');
                        string daTeL = $"{StartDate.AddDays(i)}".Split(' ')[0];
                        string MinTL = sfComponentsL[2].Split("°")[0];
                        string MaxTL = sfComponentsL[1].Split("°")[0];
                        string qwL = $"(N'{daTeL}', N'{MinTL}', N'{MaxTL}'),";

                        sql1.Append(qwL);

                    }
                    string relatedDataX = driver.FindElement(By.Id($"tabDay{6}")).Text;
                    string[] sfComponentsX = relatedDataX.Split('\n');
                    string daTeX = $"{StartDate.AddDays(6)}".Split(' ')[0];
                    string MinTX = sfComponentsX[2].Split("°")[0];
                    string MaxTX = sfComponentsX[1].Split("°")[0];

                    string qwX = $"(N'{daTeX}', N'{MinTX}', N'{MaxTX}'); ";
                    sql1.Append(qwX);

                    string sql = sql1.ToString();

                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            File.WriteAllText($"/Users/michaelartemyev/Library/Mobile Documents/com~apple~CloudDocs/programming/DBemitation.txt",
                            rowsAffected + "");
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
