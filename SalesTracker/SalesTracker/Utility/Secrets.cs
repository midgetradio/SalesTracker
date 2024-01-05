namespace SalesTracker.Utility
{
    public class Secrets
    {
        public SQLSERVER SQLSERVER { get; set; } = null!;
    }

    public class SQLSERVER
    {
        public string UID { get; set; } = string.Empty;
        public string PWD { get; set; } = string.Empty;
    }

}
