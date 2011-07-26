using System.Linq;
using System.Collections.Generic;

namespace FunnelWeb.Model.Repositories
{
    public interface IAdminRepository
    {
        IQueryable<Setting> GetSettings();

        void Save(IEnumerable<Setting> settings);
        void UpdateCommentCountFor(int entryId);
    }
}
