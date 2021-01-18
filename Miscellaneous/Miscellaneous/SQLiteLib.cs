using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using System.Data;
using System.Data.SQLite;


namespace Miscellaneous
{
    public static class SQLiteLib
    {
        static string g_savePathFile = string.Empty;

        internal static void CreateSQLiteFile()
        {
            SaveFileDialog l_saveFileDialog = new SaveFileDialog();
            string l_outputPath = string.Empty;

            l_saveFileDialog.Filter = "db (*.db)|*.db|sql (*sql)|*.sql|sqlite(*.sqlite)|*.sqlite";
            l_saveFileDialog.Title = "Save database as";
            l_saveFileDialog.InitialDirectory = @"E:\Work";

            if (l_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                g_savePathFile = l_saveFileDialog.FileName;
            }
        }

        internal static SQLiteConnection CreateConnection()
        {
            SQLiteConnection l_sqliteConn;
            //l_sqliteConn = new SQLiteConnection("Data Source= database.db;Version=3;New=True;Compress=True;");
            l_sqliteConn = new SQLiteConnection("Data Source= database.db;Version=3;");

            try
            {
                l_sqliteConn.Open();
            }
            catch (Exception ex)
            {

            }
            return l_sqliteConn;
        }

        internal static void CreateTable(SQLiteConnection conn)
        {
            SQLiteCommand l_sqliteCmd;
            string sql;

            sql = "drop table if exists Info";
            l_sqliteCmd = new SQLiteCommand(sql, conn);
            l_sqliteCmd.ExecuteNonQuery();

            sql = "drop table if exists Message";
            l_sqliteCmd = new SQLiteCommand(sql, conn);
            l_sqliteCmd.ExecuteNonQuery();

            sql = "drop table if exists Signal";
            l_sqliteCmd = new SQLiteCommand(sql, conn);
            l_sqliteCmd.ExecuteNonQuery();

            sql = "create table Info (" +
                "prjname varchar(50), " +
                "bustype varchar(10), " +
                "ver double(8))";
            l_sqliteCmd = new SQLiteCommand(sql, conn);
            l_sqliteCmd.ExecuteNonQuery();

            sql = "create table Message (" +
                "mesid integer(64), " +
                "name varchar(50), " +
                "size integer(8), " +
                "txnode varchar(50))";
            l_sqliteCmd = new SQLiteCommand(sql, conn);
            l_sqliteCmd.ExecuteNonQuery();

            sql = "create table Signal (" +
                "name varchar(50), " +
                "size integer(8)," +
                "mesid integer(64)," +
                "rxnode varchar(50)," +
                "min double(64)," +
                "max double(64)," +
                "factor double(8)," +
                "offset integer(8))";
            l_sqliteCmd = new SQLiteCommand(sql, conn);
            l_sqliteCmd.ExecuteNonQuery();


        }

        internal static void InsertData(SQLiteConnection conn)
        {
            SQLiteCommand l_sqliteCmd;
            l_sqliteCmd = conn.CreateCommand();
            l_sqliteCmd.CommandText = "INSERT INTO SampleTable" +
                "(Col1, Col2) VALUES ('Test Text ', 1);";
            l_sqliteCmd.ExecuteNonQuery();
            l_sqliteCmd.CommandText = "INSERT INTO SampleTable" +
                "(Col1, Col2) VALUES ('Test1 Text1 ', 2);";
            l_sqliteCmd.ExecuteNonQuery();
            l_sqliteCmd.CommandText = "INSERT INTO SampleTable" +
                "(Col1, Col2) VALUES ('Test2 Text2 ', 3);";
            l_sqliteCmd.ExecuteNonQuery();

            l_sqliteCmd.CommandText = "INSERT INTO SampleTable1" +
                "(Col1, Col2) VALUES ('Test3 Text3 ', 3);";
            l_sqliteCmd.ExecuteNonQuery();
        }

        internal static void ReadData(SQLiteConnection conn)
        {
            SQLiteDataReader l_sqliteDataReader;
            SQLiteCommand l_sqliteCmd;
            l_sqliteCmd = conn.CreateCommand();
            l_sqliteCmd.CommandText = "SELECT * FROM SampleTable";

            l_sqliteDataReader = l_sqliteCmd.ExecuteReader();
            while (l_sqliteDataReader.Read())
            {
                string l_myReader = l_sqliteDataReader.GetString(0);
                Console.WriteLine(l_myReader);
            }
            conn.Close();
        }
    }
}

