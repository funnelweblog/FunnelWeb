namespace FunnelWeb.Web.Application.Mime
{
    public interface IMimeTypeLookup
    {
        string GetMimeType(string fileNameOrPathWithExtension);
    }
}
