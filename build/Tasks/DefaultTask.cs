using Cake.Frosting;

namespace Build.Tasks;

[TaskName("Default")]
[IsDependentOn(typeof(PackSqlPackageTask))]
public class DefaultTask
    : FrostingTask<BuildContext>
{
}