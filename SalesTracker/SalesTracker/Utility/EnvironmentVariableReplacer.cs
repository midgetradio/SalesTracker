﻿namespace SalesTracker.Utility
{
    public static class EnvironmentVariableReplacer
    {
        public static string Replace(string originalString)
        {
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
                        var val = Environment.GetEnvironmentVariable(environmentVariable);
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
