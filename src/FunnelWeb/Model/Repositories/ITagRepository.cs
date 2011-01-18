using System.Collections.Generic;
using System.Linq;

namespace FunnelWeb.Model.Repositories
{
    public interface ITagRepository
    {
        IQueryable<Tag> GetTags();
        IQueryable<Tag> GetTags(string tagName);
        IEnumerable<Entry> GetTaggedItems(string tagName, int skip, int take);
        int GetTaggedItemCount(string tagName);
        void Save(Tag tag);
        void Delete(Tag tag);
        Tag GetTag(string tagName);
    }
}
