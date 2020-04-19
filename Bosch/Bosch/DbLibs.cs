using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bosch
{
    class DbLibs
    {
        class ProjectInfomation
        {
            string Name { set; get; }
            float Release { set; get; }
            string Variant { set; get; }
            // May add more information.
        }

        class SignalDB
        {
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
            string ProjectName { get; set; }
            string NodePrefix { get; set; } = "BU_";
            string MessagePrefix { get; set; } = "BO_";
            string SignalPrefix { get; set; } = "SG_";
            string ValueTablePrefix { get; set; } = "VAL_";

            JsonSetupFile(string projectName)
            {
            }

            bool ReadIniPrefix(string nodePrefix, string messagePrefix, string signalPrefix, string valueTablePrefix)
            {
                bool[] checkingStatus = new bool[4] { false, false, false, false };
                checkingStatus[0] = (nodePrefix == NodePrefix) ? true : false;
                checkingStatus[1] = (messagePrefix == MessagePrefix) ? true : false;
                checkingStatus[2] = (signalPrefix == SignalPrefix) ? true : false;
                checkingStatus[3] = (valueTablePrefix == ValueTablePrefix) ? true : false;
                
                return true;
            }

            public static void ReadJsonFile(string filePath)
            {
                using (StreamReader reader = File.OpenText(@"D:\TH\csharp\Miscellaneous\Bosch\Bosch\Configuration\ProjectConfiguration.json"))
                {
                    JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                    ;
                }
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
