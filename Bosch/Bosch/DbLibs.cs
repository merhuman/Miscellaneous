using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Bosch
{
    class DbLibs
    {
        class ProjectInfomation
        {
            string name { set; get; }
            float release { set; get; }
            string variant { set; get; }
            // May add more information.
        }

        class SignalDB
        {
            string projectName { set; get; }
            string variant { set; get; }
            string signalName { set; get; }
            float minValue { set; get; }
            float maxValue { set; get; }
            string vsmName { set; get; }
            string adiName { set; get; }
            List<string> valueTable { set; get; }
        }

        static SQLiteConnection m_dbConnection;
        internal static void Run ()
        {
            m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "select * from highscores order by score desc";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);

            Console.ReadKey();
        }
    }
}
