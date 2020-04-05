﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
    /// <summary>
    /// Represents a single parameter supplied to an <see cref="Microsoft.AspNetCore.Components.IComponent"/>
    /// component under test.
    /// </summary>    
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "<Pending>")]
    public readonly struct ComponentParameter<TComponent, TValue> : IEquatable<ComponentParameter<TComponent, TValue>> where TComponent : class, IComponent
    {
        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the value being supplied to the component.
        /// </summary>
        [AllowNull]
        public TValue Value { get; }

        /// <summary>
        /// Gets a value to indicate whether the parameter is for use by a <see cref="CascadingValue{TValue}"/>.
        /// </summary>
        public bool IsCascadingValue { get; }

        private ComponentParameter(Expression<Func<TComponent, TValue>> expression, [AllowNull] TValue value, bool isCascadingValue)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (isCascadingValue && value is null)
            {
                throw new ArgumentNullException(nameof(value), "Cascading values cannot be set to null");
            }

            Name = GetParameterNameFromMethodExpression(expression);
            Value = value;
            IsCascadingValue = isCascadingValue;
        }

        private static string GetParameterNameFromMethodExpression<T>(Expression<Func<TComponent, T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            throw new ArgumentException($"The expression '{expression}' does not resolve to a Property or Field on the class '{typeof(TComponent)}'.");
        }

        /// <summary>
        /// Create a parameter for a component under test.
        /// </summary>
        /// <param name="expression">The property or field expression</param>
        /// <param name="value">Value or null to pass the component</param>
        public static ComponentParameter CreateParameter(Expression<Func<TComponent, TValue>> expression, [AllowNull] TValue value)
            => ComponentParameter.CreateParameter(GetParameterNameFromMethodExpression(expression), value);

        /// <summary>
        /// Create a Cascading Value parameter for a component under test.
        /// </summary>
        /// <param name="expression">The property or field expression</param>
        /// <param name="value">The cascading value</param>
        public static ComponentParameter CreateCascadingValue(Expression<Func<TComponent, TValue>> expression, [DisallowNull] TValue value)
            => ComponentParameter.CreateCascadingValue(GetParameterNameFromMethodExpression(expression), value);

        /// <summary>
        /// Create a parameter for a component under test.
        /// </summary>
        /// <param name="input">A name/value pair for the parameter</param>
        public static implicit operator ComponentParameter<TComponent, TValue>((Expression<Func<TComponent, TValue>> expression, TValue value) input)
            => new ComponentParameter<TComponent, TValue>(input.expression, input.value, false);

        /// <summary>
        /// Create a parameter or cascading value for a component under test.
        /// </summary>
        /// <param name="input">A name/value/isCascadingValue triple for the parameter</param>
        public static implicit operator ComponentParameter<TComponent, TValue>((Expression<Func<TComponent, TValue>> expression, TValue value, bool isCascadingValue) input)
            => new ComponentParameter<TComponent, TValue>(input.expression, input.value, input.isCascadingValue);

        /// <inheritdoc/>
        public bool Equals(ComponentParameter<TComponent, TValue> other)
            => string.Equals(Name, other.Name, StringComparison.Ordinal) && Equals(Value, other.Value) && IsCascadingValue == other.IsCascadingValue;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is ComponentParameter<TComponent, TValue> other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Name, Value, IsCascadingValue);

        /// <inheritdoc/>
        public static bool operator ==(ComponentParameter<TComponent, TValue> left, ComponentParameter<TComponent, TValue> right) => left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(ComponentParameter<TComponent, TValue> left, ComponentParameter<TComponent, TValue> right) => !(left == right);
    }
}
