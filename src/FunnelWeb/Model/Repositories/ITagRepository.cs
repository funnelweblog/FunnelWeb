using System.Collections.Generic;

namespace FunnelWeb.Model.Repositories
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetAll();

        Tag GetByName(string name);

        IEnumerable<Tag> GetByPartialName(string partial);
    }
}
