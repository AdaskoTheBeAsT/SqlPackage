using System;
using Cake.Frosting;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .InstallTool(new Uri("nuget:?package=7-Zip.CommandLine"))
            .Run(args);
    }
}