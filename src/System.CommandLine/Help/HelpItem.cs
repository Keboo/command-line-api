// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace System.CommandLine.Help
{
    public class HelpItem : IEquatable<HelpItem?>
    {
        public string Name { get; }
        public string Description { get; }
        public string? AllowedValues { get; }
        public string? DefaultValue { get; }

        public HelpItem(string name, string description, string? allowedValues, string? defaultValue)
        {
            Name = name;
            Description = description;
            AllowedValues = allowedValues;
            DefaultValue = defaultValue;
        }

        public void Deconstruct(
            out string name, 
            out string description,
            out string? allowedValues,
            out string? defaultValue)
        {
            name = Name;
            description = Description;
            allowedValues = AllowedValues;
            defaultValue = DefaultValue;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as HelpItem);
        }

        public bool Equals(HelpItem? other)
        {
            return other != null &&
                   Name == other.Name &&
                   Description == other.Description &&
                   AllowedValues == other.AllowedValues &&
                   DefaultValue == other.DefaultValue;
        }

        public override int GetHashCode()
        {
            int hashCode = -244751520;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(AllowedValues);
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(DefaultValue);
            return hashCode;
        }

        public static bool operator ==(HelpItem? left, HelpItem? right)
        {
            return EqualityComparer<HelpItem?>.Default.Equals(left, right);
        }

        public static bool operator !=(HelpItem? left, HelpItem? right)
        {
            return !(left == right);
        }
    }
}
