using System.Reflection;

namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     Reads captured output from <see cref="ITestOutputHelper" /> implementations.
/// </summary>
internal static class TestOutputOutputReader
{
    internal static string GetOutput( ITestOutputHelper testOutput )
    {
        var outputProperty = testOutput.GetType()
                                       .GetProperty( "Output", BindingFlags.Public | BindingFlags.Instance );

        if( outputProperty?.GetValue( testOutput ) is string output )
        {
            return output;
        }

        throw new InvalidOperationException(
            $"Cannot read captured output from {testOutput.GetType().FullName}. " +
            "Use an ITestOutputHelper implementation that exposes an Output property." );
    }
}
