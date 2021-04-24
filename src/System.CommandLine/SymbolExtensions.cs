// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace System.CommandLine
{
    public static class SymbolExtensions
    {
        internal static IReadOnlyList<IArgument> Arguments(this ISymbol symbol)
        {
            return symbol switch
            {
                IOption option => new[] { option.Argument },
                ICommand command => command.Arguments,
                IArgument argument => new[] { argument },
                _ => throw new NotSupportedException(),
            };
        }

        public static IEnumerable<string?> GetSuggestions(this ISymbol symbol, string? textToMatch = null)
        {
            return symbol.GetSuggestions(null, textToMatch);
        }
    }
}