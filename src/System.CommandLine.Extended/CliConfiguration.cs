// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine.Parsing;

namespace System.CommandLine
{
    /// <summary>
    /// Represents the configuration used by the <see cref="CliParser"/>.
    /// </summary>
    public class CliConfiguration
    {
        /// <summary>
        /// Enables a default exception handler to catch any unhandled exceptions thrown during invocation. Enabled by default.
        /// </summary>
        public bool EnableDefaultExceptionHandler { get; set; } = true;

    }
}