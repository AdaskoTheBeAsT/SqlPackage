using Cake.Core.IO;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("PackSqlPackage")]
    [IsDependentOn(typeof(UpdateNuspecsTask))]
    public class PackSqlPackageTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var dirPath = context
                .Environment
                .ApplicationRoot
                .Combine(new DirectoryPath("nuspecs"));
            Pack(context, dirPath, context.LinuxName);
            Pack(context, dirPath, context.MacOsName);
            Pack(context, dirPath, context.WindowsName);
            Pack(context, dirPath, context.MasterName);
        }

        private void Pack(BuildContext context, DirectoryPath dirPath, string name)
        {
            var nuspecPath = dirPath
                .Combine(new DirectoryPath(name))
                .CombineWithFilePath(new FilePath($"{name}.nuspec"))
                .MakeAbsolute(context.Environment);
            context.NuGetPack(nuspecPath, new NuGetPackSettings
            {
                OutputDirectory = context.Environment.WorkingDirectory
                    .Combine(new DirectoryPath(".."))
                    .Combine(new DirectoryPath("output"))
            });
        }
    }
}
