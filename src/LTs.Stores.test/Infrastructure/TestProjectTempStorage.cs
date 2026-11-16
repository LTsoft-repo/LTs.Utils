namespace LTs.Stores.test.Infrastructure;

internal static class TestProjectTempStorage
{
    private static readonly string ProjectFolderName =
        typeof( TestProjectTempStorage ).Assembly.GetName().Name!;

    internal static string CreateUniqueDirectory()
    {
        var path = Path.Combine( Path.GetTempPath(), ProjectFolderName, Guid.NewGuid().ToString( "N" ) );
        Directory.CreateDirectory( path );

        return path;
    }
}