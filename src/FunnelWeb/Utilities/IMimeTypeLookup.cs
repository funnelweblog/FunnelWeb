namespace FunnelWeb.Utilities
{
    public interface IMimeTypeLookup
    {
        string GetMimeType(string fileNameOrPathWithExtension);
    }
}
