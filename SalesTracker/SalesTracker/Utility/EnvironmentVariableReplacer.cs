using SalesTracker.Migrations;
using System.Data.SqlTypes;

namespace SalesTracker.Utility
{
    public static class EnvironmentVariableReplacer
    {
        public static string Replace(string? originalString)
        {
            if (originalString == null)
            {
                originalString = "Data Source=localhost; Initial Catalog=SalesTracker; User ID=%SQL_UID%; Password=%SQL_PWD%";
            }

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
                        var environmentVariable = originalString.Substring(indexStart + 1, indexEnd - (indexStart + 1));
                        string? val = "";
                        try
                        {
                            val = Environment.GetEnvironmentVariable(environmentVariable);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.ToString());
                            Console.WriteLine("Variable: " + environmentVariable);
                        }
                        replacement += val;
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
