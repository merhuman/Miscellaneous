using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bosch
{
    class ParamLibs
    {
        
        internal class ParamFile
        {
            internal string Author { get; set; } = "";
            internal string Department { get; set; } = "";
            internal string ParamName { get; set; } = ""; 

            bool paramGenerate(string filePath)
            {

                return true;
            }

            bool paramGeneratorSensorFailure(string filePath, int test)
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
            private bool LoadParamStructure(string filePath)
            {
                
                return true;
            }
        }
       

        
    }
}
