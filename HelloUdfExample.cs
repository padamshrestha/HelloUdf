using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Spark.Sql;
using static Microsoft.Spark.Sql.Functions;

static class HelloUdfExample
{
    public static void Run(){
                    // create the spark session
            SparkSession spark = SparkSession.Builder().GetOrCreate();

            // create a dataframe from the json file
            DataFrame df = spark.Read().Json("coordinates.json");

            // show the original content
            df.Show();

            // create a user defined function that will split the data on ';'
            Func<Column, Column> udfArray = Udf<string, string[]>((str) => SplitMethod(str));

            // perform the split and add a new column name "coordinateArray" to store the string array of the split data
            df = df.WithColumn("coordinateArray", udfArray(df["coordinate"]));

            // display the original column "coordinate" along with the added column "coordinateArray"
            // df.Show();

            // get the two items of the "coordinateArray" and put them in individual columns
            Column colLatitude = df.Col("coordinateArray").GetItem(0);
            Column colLongitude = df.Col("coordinateArray").GetItem(1);

            // add the two new columns to the dataframe and drop the other two columns that are no longer needed
            df = df
                .WithColumn("latitude", colLatitude)
                .WithColumn("longitude", colLongitude)
                .Drop("coordinate")
                .Drop("coordinateArray");

            // now there should only be two columns named "latitude" and "longitude"
            // df.Show();

            // collect the result from spark, ...
            List<Row> rows = df.Collect().ToList();

            // ... dispose the spark sessions ...
            spark.Dispose();

            // ... and do direct C# processing outside of spark
            foreach (Row row in rows)
            {
                object[] rowValues = row.Values;
                string latitude = rowValues[0].ToString();
                string longitude = rowValues[1].ToString();
                Console.WriteLine($"latitude: {latitude}, longitude:{longitude}");
            }
    }
    
        // The method that is used by Udf
        private static string[] SplitMethod(string stringToSplit)
        {
            Console.WriteLine($"\tsplitting: {stringToSplit}");
            return stringToSplit.Split(';');
        }
    
}