using System.IO;
using System.Text;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Utf16ToUtf8")]
    [IsDependentOn(typeof(DownloadSqlPackageTask))]
    public class Utf16ToUtf8Task : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            Process(context, context.WindowsName);
            Process(context, context.MacOsName);
            Process(context, context.LinuxName);
        }

        public void Process(
            BuildContext context,
            string name)
        {
            var toolsPath = context
                .Environment
                .ApplicationRoot
                .Combine(new DirectoryPath("nuspecs"))
                .Combine(new DirectoryPath(name))
                .Combine(new DirectoryPath("tools"));

            var srcFile = toolsPath
                .CombineWithFilePath(new FilePath("LICENSE.TXT"))
                .MakeAbsolute(context.Environment);

            var targetFile = toolsPath
                .CombineWithFilePath(new FilePath("LICENSE_UTF8.TXT"))
                .MakeAbsolute(context.Environment);

            var str = Encoding.Unicode.GetString(File.ReadAllBytes(srcFile.FullPath));
            File.WriteAllBytes(targetFile.FullPath, Encoding.UTF8.GetBytes(str));
        }
    }
}