using Microsoft.Spark.Sql;
using static Microsoft.Spark.Sql.Functions;

class CountAndAggregateMnM
{
    public static void Run(){

        SparkSession spark = SparkSession
                                .Builder()
                                .AppName("MnMCount")
                                .GetOrCreate();

        var df = spark.Read().Format("csv")
                    .Option("header", "true")
                    .Option("inferSchema", "true")
                    .Load("data/mnm_dataset.csv");

    
        // Commands are very similar to Scala
        var countMnMdf = df.Select("State", "Color","Count")
        .Where(Col("State") == "WA")
        .GroupBy("State", "Color")
        .Agg(Count("Count").Alias("Total"))
        .OrderBy(Desc("Total"));

        countMnMdf.Show(10);

        spark.Stop();
    }    
}