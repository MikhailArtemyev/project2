using static System.Runtime.InteropServices.JavaScript.JSType;

public class DataBlock
{
    public DateTime Date { get; set; }
    public int MinTemp { get; set; }
    public int MaxTemp { get; set; }

    public DataBlock(DateTime a, int b, int c)
    {
        Date = a;
        MinTemp = b;
        MaxTemp = c;
    }
        
}