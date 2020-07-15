using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Miscellaneous
{
    class ParamLibs
    {
        
        internal class ParamFile
        {
            internal string Author { get; set; } = "";
            internal string Department { get; set; } = "";
            internal string ParamName { get; set; } = "";
            internal short NumberOfScalarSingleRecord { get; set; } = 0;
            internal short NumberOfScalarListSingleRecord { get; set; } = 0;
            internal short NumberOfStructSingleRecord { get; set; } = 0;
            internal short NumberOfStructListSingleRecord { get; set; } = 0;

            bool paramGenerate(string filePath)
            {

                return true;
            }

            /// <summary>
            /// Parameter structure is loaded from json file
            /// </summary>
            /// <param name="filePath"></param>
            /// <returns>
            /// True if structure is loaded successfully
            /// False if there are any failures or invalid issues
            /// </returns>
            public bool GenerateParam(string filePath)
            {
                string l_fileName = "sensor.txt";
                string l_fileNameFull = Path.Combine(filePath, l_fileName);

                using (StreamReader reader = File.OpenText(@""))
                {
                    JObject readObject = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                    NumberOfScalarSingleRecord = Convert.ToInt16(readObject["ParameterObjects"]["Properties"]["numberOfScalarSingleRecord"]);
                    NumberOfScalarListSingleRecord = Convert.ToInt16(readObject["ParameterObjects"]["Properties"]["numberOfScalarListSingleRecord"]);
                    NumberOfStructSingleRecord = Convert.ToInt16(readObject["ParameterObjects"]["Properties"]["numberOfStructSingleRecord"]);
                    NumberOfStructListSingleRecord = Convert.ToInt16(readObject["ParameterObjects"]["Properties"]["numberOfStructListSingleRecord"]);


                    
                    if (File.Exists(l_fileNameFull))
                    {
                        File.Delete(l_fileNameFull);
                    }

                    using (FileStream fs = File.Create(l_fileNameFull))
                    {
                        fs.Write(new UTF8Encoding(true).GetBytes("Vector Parameter	1.0" + "\n"), 0, "Vector Parameter	1.0".Length + 1);

                        if (NumberOfScalarSingleRecord != 0)
                        {
                            //for (int idx = 0; idx < NumberOfScalarSingleRecord; idx++)
                            //{
                            //    fs.Write(new UTF8Encoding(true).GetBytes("StructSingleRecord" + "\n"), 0, "StructSingleRecord".Length + 1);
                            //}
                            Console.WriteLine(readObject.Value<JArray>("ScalarSingleRecord").Count());
                        }

                        if (NumberOfScalarListSingleRecord != 0)
                        {

                        }

                        if (NumberOfScalarSingleRecord != 0)
                        {

                        }

                        if (NumberOfScalarListSingleRecord != 0)
                        {

                        }
                    }

                }

                

                
                return true;
            }
        }
       

        
    }
}
