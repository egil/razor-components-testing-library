﻿using System;
using System.Linq;
using Bunit.SampleComponents;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;

namespace Bunit
{
    public class ComponentParameterBuilderTests
    {
        private readonly ComponentParameterBuilder<AllTypesOfParams<string>> _sut;

        public ComponentParameterBuilderTests()
        {
            _sut = new ComponentParameterBuilder<AllTypesOfParams<string>>();
        }

        [Theory(DisplayName = "Add Nullable Integer and Build")]
        [InlineData(null)]
        [InlineData(42)]
        public void Test001(int? value)
        {
            // Arrange and Act
            _sut.Add(c => c.NamedCascadingValue, value);
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result.First();
            parameter.Name.ShouldBe("NamedCascadingValue");
            parameter.Value.ShouldBe(value);
        }

        [Theory(DisplayName = "Add String and Build")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("foo")]
        public void Test002(string? value)
        {
            // Arrange and Act
            _sut.Add(c => c.RegularParam, value);
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result.First();
            parameter.Name.ShouldBe("RegularParam");
            parameter.Value.ShouldBe(value);
        }

        [Fact(DisplayName = "AddCascading Integer without a name and Build")]
        public void Test003()
        {
            // Arrange
            const int value = 42;
            _sut.AddCascading(value);

            // Act
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result.First();
            parameter.Name.ShouldBeNull();
            parameter.Value.ShouldBe(value);
        }

        [Fact(DisplayName = "Add multiple and Build")]
        public void Test004()
        {
            // Arrange and Act
            _sut.Add(c => c.NamedCascadingValue, 42).Add(c => c.RegularParam, "bar");
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(2);

            var first = result.First();
            first.Name.ShouldBe("NamedCascadingValue");
            first.Value.ShouldBe(42);

            var second = result.Last();
            second.Name.ShouldBe("RegularParam");
            second.Value.ShouldBe("bar");
        }

        [Fact(DisplayName = "Add NonGenericCallback and Build")]
        public void Test005()
        {
            // Arrange
            EventCallback callback = EventCallback.Empty;

            // Act
            _sut.Add(c => c.NonGenericCallback, callback);
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result.First();
            parameter.Name.ShouldBe("NonGenericCallback");
            parameter.Value.ShouldBe(callback);
        }

        [Fact(DisplayName = "Add GenericCallback and Build")]
        public void Test006()
        {
            // Arrange
            EventCallback<EventArgs> callback = EventCallback<EventArgs>.Empty;

            // Act
            _sut.Add(c => c.GenericCallback, callback);
            var result = _sut.Build();

            // Assert
            result.Count.ShouldBe(1);

            var parameter = result.First();
            parameter.Name.ShouldBe("GenericCallback");
            parameter.Value.ShouldBe(callback);
        }

        [Fact(DisplayName = "Add duplicate name should throw Exception")]
        public void Test100()
        {
            // Arrange
            _sut.Add(c => c.NamedCascadingValue, null);

            // Act and Assert
            Assert.Throws<ArgumentException>(() => _sut.Add(c => c.NamedCascadingValue, null));
        }

        [Fact(DisplayName = "AddCascading with null value should throw Exception")]
        public void Test101()
        {
            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => _sut.AddCascading(c => c.NamedCascadingValue, null));
        }
    }
}