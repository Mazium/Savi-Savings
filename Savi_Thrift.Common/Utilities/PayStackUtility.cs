namespace Savi_Thrift.Common.Utilities
{
    public static class PayStackUtility
    {
        public static string GenerateUniqueReference(string prefix = "SAVI", int length = 15)
        {
            if (prefix.Length > length)
            {
                throw new ArgumentException("Prefix length should not exceed the total length.");
            }

            string randomPart = Guid.NewGuid().ToString("N").Substring(0, length - prefix.Length);

            string uniqueReference = $"{prefix}{randomPart}";

            return uniqueReference;
        }
    }
}
