using System.Collections.Generic;

namespace FunnelWeb.Model.Repositories
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetTags();
        IEnumerable<Tag> GetTags(string tagName);
        IEnumerable<Entry> GetTaggedItems(string tagName, int skip, int take);
        int GetTaggedItemCount(string tagName);
        void Save(Tag tag);
        void Delete(Tag tag);
        Tag GetTag(int id);
        Tag GetTag(string tagName);
    }
}
