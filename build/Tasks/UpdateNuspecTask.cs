using System.Xml;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("UpdateNuspecs")]
    [IsDependentOn(typeof(Utf16ToUtf8Task))]
    public class UpdateNuspecsTask : FrostingTask<BuildContext>
    {
        private const string NugetNamespace = "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd";

        public override void Run(BuildContext context)
        {
            Process(context, context.WindowsName);
            Process(context, context.MacOsName);
            Process(context, context.LinuxName);
            ProcessMasterNuspec(context);
            ProcessMasterProps(context);
        }

        public void Process(
            BuildContext context,
            string name)
        {
            var dirPath = context
                .Environment
                .ApplicationRoot
                .Combine(new DirectoryPath("nuspecs"))
                .Combine(new DirectoryPath(name))
                .MakeAbsolute(context.Environment);
            var nuspecPath = dirPath.CombineWithFilePath(new FilePath($"{name}.nuspec"));
            var toolsPath = dirPath.Combine(new DirectoryPath("tools"));

            var doc = new XmlDocument();
            doc.Load(nuspecPath.FullPath);
            var nodeList = doc.GetElementsByTagName("files");
            if (nodeList.Count < 1)
            {
                return;
            }

            var filesNode = nodeList[0];
            var fullDir = context.FileSystem.GetDirectory(toolsPath);
            foreach (var file in fullDir.GetFiles("*", SearchScope.Current))
            {
                var referenceName = $"tools\\{file.Path.GetFilename()}";
                var newNode = doc.CreateNode(XmlNodeType.Element, "file", NugetNamespace);
                var srcAttribute = doc.CreateAttribute("src");
                srcAttribute.Value = referenceName;
                newNode.Attributes?.Append(srcAttribute);
                var targetAttribute = doc.CreateAttribute("target");
                targetAttribute.Value = referenceName;
                newNode.Attributes?.Append(targetAttribute);
                filesNode.AppendChild(newNode);
            }

            var versionNodeList = doc.GetElementsByTagName("version");
            if (versionNodeList.Count < 1)
            {
                return;
            }

            versionNodeList[0].InnerText = context.Version;

            doc.Save(nuspecPath.FullPath);
        }

        private void ProcessMasterNuspec(BuildContext context)
        {
            var dirPath = GetMasterDir(context);
            var nuspecPath = dirPath.CombineWithFilePath(new FilePath($"{context.MasterName}.nuspec"));
            var doc = new XmlDocument();
            doc.Load(nuspecPath.FullPath);

            var versionNodeList = doc.GetElementsByTagName("version");
            if (versionNodeList.Count < 1)
            {
                return;
            }

            versionNodeList[0].InnerText = context.Version;

            doc.Save(nuspecPath.FullPath);
        }

        private void ProcessMasterProps(BuildContext context)
        {
            var dirPath = GetMasterDir(context);

            var propsPath = dirPath
                .Combine(new DirectoryPath("build"))
                .CombineWithFilePath(new FilePath($"{context.MasterName}.props"));

            var doc = new XmlDocument();
            doc.Load(propsPath.FullPath);

            var nodeList = doc.GetElementsByTagName("PackageReference");
            for (int i = 0; i < nodeList.Count; i++)
            {
                var node = nodeList[i];
                node.Attributes["Version"].Value = context.Version;
            }

            doc.Save(propsPath.FullPath);
        }

        private DirectoryPath GetMasterDir(BuildContext context) => context
            .Environment
            .ApplicationRoot
            .Combine(new DirectoryPath("nuspecs"))
            .Combine(new DirectoryPath(context.MasterName))
            .MakeAbsolute(context.Environment);
    }
}
