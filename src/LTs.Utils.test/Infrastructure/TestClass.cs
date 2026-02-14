using JetBrains.Annotations;

namespace LTs.Utils.test.Infrastructure;

[ UsedImplicitly ]
public class TestClass
{
    [ UsedImplicitly ]
    public string PublicProperty { get; } = "MyPublicProperty";

    [ UsedImplicitly ]
    protected string ProtectedProperty { get; } = "MyProtectedProperty";

    [ UsedImplicitly ]
    internal string InternalProperty { get; } = "MyInternalProperty";

    [ UsedImplicitly ]
    private string PrivateProperty { get; } = "MyPrivateProperty";

    [ UsedImplicitly ]
    public readonly string PublicField;

    [ UsedImplicitly ]
    protected string ProtectedField = "MyProtectedField";

    [ UsedImplicitly ]
    internal string InternalField = "MyInternalField";

#pragma warning disable CS0414 // Field is assigned but its value is never used
    private string privateField = "MyPrivateField";
#pragma warning restore CS0414 // Field is assigned but its value is never used

    private TestClass()
        => PublicField = "Private Constructor called";

    public TestClass( string publicField )
        => PublicField = publicField;

    internal TestClass( bool flag )
        => PublicField = "Internal Constructor called";
}