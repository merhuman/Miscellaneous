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
    public class SQLiteLib
    {
        internal static void CreateSQLiteFile(string filePath)
        {
            OpenFileDialog l_fileBrowser = new OpenFileDialog();
            string l_outputPath = string.Empty;


            if (l_fileBrowser.ShowDialog() == DialogResult.OK)
            {
                
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
            string l_createSQL = "CREATE TABLE SampleTable" +
                "(Col1 VARCHAR(20), Col2 INT)";
            string l_createSQL1 = "CREATE TABLE SamppleTable1" +
                "(Col1 VARCHAR(20), Col2 INT)";
            l_sqliteCmd = conn.CreateCommand();
            l_sqliteCmd.CommandText = l_createSQL;
            l_sqliteCmd.ExecuteNonQuery();
            l_sqliteCmd.CommandText = l_createSQL1;
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

