
using System;
using System.CommandLine;
using System.Reflection;

internal static class ArgumentBuilder
{
    private static readonly ConstructorInfo _ctor;

    static ArgumentBuilder()
    {
        _ctor = typeof(CliArgument<string>).GetConstructor(new[] { typeof(string) });
    }

    public static CliArgument CreateArgument(Type valueType, string name = "value")
    {
        var argumentType = typeof(CliArgument<>).MakeGenericType(valueType);

#if NET6_0_OR_GREATER
        var ctor = (ConstructorInfo)argumentType.GetMemberWithSameMetadataDefinitionAs(_ctor);
#else
        var ctor = argumentType.GetConstructor(new[] { typeof(string) });
#endif

        return (CliArgument)ctor.Invoke(new object[] { name });
    }
}