//using System;
//using System.Data;
//using GDB.App;
//using GDB.App.Data;

//var ds = GDBInMemoryDB.CreateDataSet();

//Console.WriteLine("=== DATASET CREATED: " + ds.DataSetName + " ===");
//ListTables(ds);

//Console.WriteLine();
//PrintTableSchema(ds.Tables["ACCOUNT"]);

//Console.WriteLine();
//Console.WriteLine("=== SAMPLE QUERIES / OPERATIONS ===");

//// List first 5 account rows
//Console.WriteLine("\n-- Accounts (first 5) --");
//PrintRows(ds.Tables["ACCOUNT"], 5);


//void ListTables(DataSet ds)
//{
//    Console.WriteLine("Tables in DataSet:");
//    foreach (DataTable t in ds.Tables)
//    {
//        Console.WriteLine($"- {t.TableName} (Rows: {t.Rows.Count})");
//    }
//}

//void PrintTableSchema(DataTable table)
//{
//    Console.WriteLine($"Schema for {table.TableName}:");
//    foreach (DataColumn c in table.Columns)
//    {
//        Console.WriteLine($"  {c.ColumnName} ({c.DataType.Name}) AllowNull={c.AllowDBNull} MaxLen={c.MaxLength}");
//    }
//}

//void PrintRows(DataTable table, int maxRows = int.MaxValue)
//{
//    int count = 0;
//    foreach (DataRow r in table.Rows)
//    {
//        PrintRow(r);
//        if (++count >= maxRows) break;
//    }
//}

//void PrintRow(DataRow? row)
//{
//    if (row == null)
//    {
//        Console.WriteLine("  <null>");
//        return;
//    }

//    var values = row.Table.Columns.Cast<DataColumn>().Select(c => $"{c.ColumnName}={(row[c])}");
//    Console.WriteLine("  " + string.Join(", ", values));
//}

