using System.ComponentModel;
using System.Globalization;

namespace LTs.DependencyInjections.test.Infrastructure;

internal class GuidStringTypeConverter : TypeConverter
{
    public override bool CanConvertFrom( ITypeDescriptorContext? context, Type sourceType )
        => sourceType == typeof( string );

    public override object? ConvertFrom( ITypeDescriptorContext? context, CultureInfo? culture, object value )
        => Guid.Parse( (string)value );
}