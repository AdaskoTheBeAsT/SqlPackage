using Cake.Core;
using Cake.Frosting;

public class BuildContext
    : FrostingContext
{
    public string SqlPackageUri { get; set; }

    public string WindowsUri { get; set; }

    public string MacOsUri { get; set; }

    public string LinuxUri { get; set; }

    public string WindowsName { get; set; }

    public string MacOsName { get; set; }

    public string LinuxName { get; set; }

    public string MasterName { get; set; }

    public string Version { get; set; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        Version = context.Arguments.HasArgument("version")
            ? context.Arguments.GetArgument("version")
            : "18.8.0";
        SqlPackageUri = context.Arguments.HasArgument("sqlpackageuri")
            ? context.Arguments.GetArgument("sqlpackageuri")
            : "https://docs.microsoft.com/en-us/sql/tools/sqlpackage/sqlpackage-download?view=sql-server-ver15";
        WindowsName = "AdaskoTheBeAsT.SqlPackage.native.win.x64";
        MacOsName = "AdaskoTheBeAsT.SqlPackage.native.osx.x64";
        LinuxName = "AdaskoTheBeAsT.SqlPackage.native.linux.x64";
        MasterName = "AdaskoTheBeAsT.SqlPackage.native";
    }
}