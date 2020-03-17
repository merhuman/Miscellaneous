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
            private string _name;
            private float _release;
            private string _variant;
            string Name { set; get; }
            float Release { set; get; }
            string Variant { set; get; }
            // May add more information.
        }

        class SignalDB
        {
            private string _projectName;
            private string _variant;
            private string _signalName;
            private float _minValue;
            private float _maxValue;
            private string _vsmName;
            private string _adiName;
            string ProjectName { set; get; }
            string Variant { set; get; }
            string SignalName { set; get; }
            float MinValue { set; get; }
            float MaxValue { set; get; }
            string VsmName { set; get; }
            string AdiName { set; get; }
            List<string> valueTable { set; get; }

            SignalDB()
            {

            }
        }

        class JsonSetupFile
        {
            private string _projectName;
            private string _nodePrefix;
            private string _messagePrefix;
            private string _signalPrefix;
            private string _valueTablePrefix;
            string ProjectName { get; set; }
            string NodePrefix { get; set; }
            string MessagePrefix { get; set; }
            string SignalPrefix { get; set; }
            string ValueTablePrefix { get; set; }

            JsonSetupFile(string projectName)
            {

            }
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
