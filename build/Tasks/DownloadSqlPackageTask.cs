using Cake.Common.IO;
using Cake.Common.Net;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("DownloadSqlPackage")]
    [IsDependentOn(typeof(ScrapSqlPackagePageTask))]
    public class DownloadSqlPackageTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            ProcessDownload(context, context.WindowsUri, context.WindowsName);
            ProcessDownload(context, context.MacOsUri, context.MacOsName);
            ProcessDownload(context, context.LinuxUri, context.LinuxName);
        }

        private void ProcessDownload(BuildContext context, string uri, string extractPath)
        {
            var filePath = context.DownloadFile(uri);
            var dir = context
                .Environment
                .ApplicationRoot
                .Combine(new DirectoryPath("nuspecs"))
                .Combine(new DirectoryPath(extractPath))
                .Combine(new DirectoryPath("tools"))
                .MakeAbsolute(context.Environment);
            if (context.DirectoryExists(dir))
            {
                context.DeleteDirectory(dir, new DeleteDirectorySettings
                {
                    Force = true,
                    Recursive = true,
                });
            }

            context.CreateDirectory(dir);

            context.Unzip(filePath, dir);
        }
    }
}
