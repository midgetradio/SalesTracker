using System.Data.SqlTypes;
using System.Text.Json;

namespace SalesTracker.Utility
{
    public static class EnvironmentVariableReplacer
    {
        public static string Replace(string? originalString, Settings settings)
        {
            if (String.IsNullOrEmpty(originalString))
            {
                originalString = "Data Source=localhost; Initial Catalog=SalesTracker; User ID=%SQL_UID%; Password=%SQL_PWD%";
            }

            var json = File.Open(@settings.SecretsPath, FileMode.Open);

            var secrets = JsonSerializer.Deserialize<Secrets>(json);

            string replacement = "";
            bool first = false;
            bool pauseReplace = false;
            int indexStart = -1;
            int indexEnd = -1;

            for (int i = 0; i < originalString.Length; i++)
            {
                if (originalString[i] == '%')
                {
                    if (indexStart == -1 && !first)
                    {
                        first = true;
                        pauseReplace = true;
                        indexStart = i;
                    }
                    else
                    {
                        first = false;
                        indexEnd = i;
                    }

                    if (indexStart < indexEnd)
                    {
                        var variableName = originalString.Substring(indexStart + 1, indexEnd - (indexStart + 1));
                        if (variableName == "SQL_UID")
                        {
                            replacement += secrets.SQLSERVER.UID;
                        }
                        else if(variableName == "SQL_PWD")
                        {
                            replacement += secrets.SQLSERVER.PWD;
                        }

                        
                        //try
                        //{
                        //    Console.WriteLine($"THIS IS THE ENVIRONMENT VARIABLE: {environmentVariable}");
                        //    val = Environment.GetEnvironmentVariable(environmentVariable);
                        //    Console.WriteLine($"THIS IS THE VARIABLE: {val}");
                        //}
                        //catch(Exception e)
                        //{
                        //    Console.WriteLine(e.ToString());
                        //    Console.WriteLine("Variable: " + environmentVariable);
                        //}
                        first = false;
                        indexStart = -1;
                        indexEnd = -1;
                        pauseReplace = false;

                    }
                }
                else if(!pauseReplace)
                {
                    replacement += originalString[i];
                }
            }

            return replacement;
        }
    }
}
