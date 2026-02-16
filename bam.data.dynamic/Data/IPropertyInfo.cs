using System;

namespace Bam.Data.Dynamic.Data
{
    /// <summary>
    /// Specifies the name and type of a property being defined on a dynamic type.
    /// </summary>
    public interface IPropertyInfo
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the CLR type of the property.
        /// </summary>
        Type PropertyType { get; }
    }

    /// <summary>
    /// Default implementation of <see cref="IPropertyInfo"/> for defining dynamic type properties.
    /// </summary>
    public record PropertyInfoDescriptor(string Name, Type PropertyType) : IPropertyInfo;
}
