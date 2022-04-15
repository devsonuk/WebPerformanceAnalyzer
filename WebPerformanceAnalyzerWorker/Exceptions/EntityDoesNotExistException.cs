namespace WebPerformanceAnalyzerWorker
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Entity Does Not Exist Exception
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable CA2229 // Implement serialization constructors
    public class EntityDoesNotExistException : Exception
#pragma warning restore CA2229 // Implement serialization constructors
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDoesNotExistException"/> class.
        /// </summary>
        /// <param name="entityType">The entity type</param>
        /// <param name="parameter">The parameter used to find the entity</param>
        public EntityDoesNotExistException(MemberInfo entityType, string parameter)
            : base($"A {entityType?.Name} with the given {parameter} does not exist")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDoesNotExistException"/> class.
        /// </summary>
        /// <param name="entityType">The entity type</param>
        /// <param name="parameter">The parameter used to find the entity</param>
        /// <param name="value">The value of the parameter</param>
        public EntityDoesNotExistException(string entityType, string parameter, string value)
            : base($"A {entityType} with the given {parameter} '{value}' does not exist")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDoesNotExistException"/> class.
        /// </summary>
        /// <param name="message">The custom Exception messge</param>
        /// <param name="innerException">The inner exception of the thrown exception</param>
        public EntityDoesNotExistException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}