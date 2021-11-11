using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Build.Util;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("ScrapSqlPackagePage")]
    public class ScrapSqlPackagePageTask : AsyncFrostingTask<BuildContext>
    {
        private Regex _regexMac;
        private Regex _regexLinux;
        private Regex _regexWindows;

        public ScrapSqlPackagePageTask()
        {
            _regexMac = new Regex(
                "(?<=[<]td[^>]+[>][<]a[^>]+[>]macOS\\s\\.NET\\sCore[<][\\/]a[>][<][\\/]td[>]\\s+[<]td[^>]+[>][<]a\\shref=[\"])(?<url>[^\"]+)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _regexLinux = new Regex(
                "(?<=[<]td[^>]+[>][<]a[^>]+[>]Linux\\s\\.NET\\sCore[<][\\/]a[>][<][\\/]td[>]\\s+[<]td[^>]+[>][<]a\\shref=[\"])(?<url>[^\"]+)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _regexWindows = new Regex(
                "(?<=[<]td[^>]+[>][<]a[^>]+[>]Windows\\s\\.NET\\sCore[<][\\/]a[>][<][\\/]td[>]\\s+[<]td[^>]+[>][<]a\\shref=[\"])(?<url>[^\"]+)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public override async Task RunAsync(BuildContext context)
        {
            var sqlPackageDownloader = new SqlPackageDownloader();
            var content = await sqlPackageDownloader.GetPage(context.SqlPackageUri);
            RetrieveUrl(context, content, _regexMac, (buildContext, s) => buildContext.MacOsUri = s);
            RetrieveUrl(context, content, _regexLinux, (buildContext, s) => buildContext.LinuxUri = s);
            RetrieveUrl(context, content, _regexWindows, (buildContext, s) => buildContext.WindowsUri = s);
        }

        private void RetrieveUrl(
            BuildContext context,
            string content,
            Regex regex,
            Action<BuildContext, string> setAction)
        {
            if (content == null)
            {
                return;
            }

            var match = regex.Match(content);
            if (!match.Success)
            {
                return;
            }

            var value = match.Groups["url"].Value;
            setAction(context, value);
        }
    }
}