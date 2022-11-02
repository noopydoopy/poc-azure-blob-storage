namespace AzureBlobStorageApp.Configs
{
    public class AzureADBlobsConfig
    {
        public const string SECTION = "AzureADBlobs";
        public string EndPoint { get; set; }
        public string Key { get; set; }
        public string ConnectionString { get; set; }
    }
}
