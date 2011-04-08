using System.IO;
using System.Linq;
using System.Web;

namespace FunnelWeb.Web.Application.Themes
{
    public class ThemeProvider : IThemeProvider
    {
        public string[] GetThemes()
        {
            var themeFolder = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Themes"));
            var themes = themeFolder.GetDirectories().Select(x => x.Name).OrderBy(x => x).ToArray();
            return themes;
        }
    }
}