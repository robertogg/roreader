namespace RoReader.Constants
{
    public static class CacheKeys
    {
        public static string Post(string partitionKey, string postTitle)
        {
            return string.Format("{0}_{1}", partitionKey, postTitle);
        }
    }
}
