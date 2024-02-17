// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NET6_0_OR_GREATER
using System.Reflection;
#endif

namespace System.CommandLine.Utility;

internal static class OptionBuilder
{
#if NET6_0_OR_GREATER
    private static readonly ConstructorInfo _ctor;

    static OptionBuilder()
    {
        _ctor = typeof(CliOption<string>).GetConstructor(new[] { typeof(string), typeof(string[]) });
    }
#endif

    internal static CliOption CreateOption(string name, Type valueType, string description = null)
    {
        var optionType = typeof(CliOption<>).MakeGenericType(valueType);

#if NET6_0_OR_GREATER
        var ctor = (ConstructorInfo)optionType.GetMemberWithSameMetadataDefinitionAs(_ctor);
#else
        var ctor = optionType.GetConstructor(new[] { typeof(string), typeof(string[]) });
#endif

        var option = (CliOption)ctor.Invoke(new object[] { name, Array.Empty<string>() });

        option.Description = description;

        return option;
    }

}