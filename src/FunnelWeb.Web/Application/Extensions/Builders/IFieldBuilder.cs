namespace FunnelWeb.Web.Application.Extensions.Builders
{
    public interface IFieldBuilder<TReturn>
    {
        TReturn Default(object value);
        TReturn IsRequired();
    }
}