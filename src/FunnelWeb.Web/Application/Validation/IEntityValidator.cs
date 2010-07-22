namespace FunnelWeb.Web.Application.Validation
{
    public interface IEntityValidator
    {
        ValidationResult Validate(object source);
    }
}