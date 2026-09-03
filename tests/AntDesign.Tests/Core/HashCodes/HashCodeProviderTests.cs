using System;
using System.Collections.Generic;
using System.Reflection;
using AntDesign.Core.HashCodes;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesign.Tests.Core.HashCodes
{
    public class HashCodeProviderTests
    {
        [Fact]
        public void CreateSelectsAndReusesProvidersForSupportedTypeShapes()
        {
            var dictionaryProvider = HashCodeProvider.Create(typeof(Dictionary<string, object>));
            var enumerableProvider = HashCodeProvider.Create(typeof(int[]));
            var otherProvider = HashCodeProvider.Create(typeof(StableHashValue));

            Assert.Contains("DictionaryHashCodeProvider", dictionaryProvider.GetType().Name);
            Assert.Contains("EnumerableHashCodeProvider", enumerableProvider.GetType().Name);
            Assert.Contains("OtherHashCodeProvider", otherProvider.GetType().Name);
            Assert.Same(dictionaryProvider, HashCodeProvider.Create(typeof(IDictionary<string, object>)));
            Assert.Same(enumerableProvider, HashCodeProvider.Create(typeof(List<int>)));
            Assert.Same(otherProvider, HashCodeProvider.Create(typeof(int)));
            Assert.Same(otherProvider, HashCodeProvider.Create(null!));
        }

        [Fact]
        public void OtherProviderHandlesNullAndUsesTheObjectsHashCode()
        {
            var provider = HashCodeProvider.Create(typeof(StableHashValue));
            var value = new StableHashValue(37, 1);

            Assert.Equal(0, provider.GetHashCode(null!));
            Assert.Equal(37, provider.GetHashCode(value));
            Assert.Equal(37, HashCode<StableHashValue>.GetHashCode(value));
        }

        [Fact]
        public void EnumerableProviderHandlesInvalidEmptyNullAndOrderedSequences()
        {
            var provider = HashCodeProvider.Create(typeof(int[]));
            var values = new object[] { new StableHashValue(11, 1), null!, new StableHashValue(-7, 2) };
            var expected = CombineSequence(11, 0, -7);

            Assert.Equal(0, provider.GetHashCode(null!));
            Assert.Equal(0, provider.GetHashCode(new object()));
            Assert.Equal(0, provider.GetHashCode(Array.Empty<int>()));
            Assert.Equal(expected, provider.GetHashCode(values));
            Assert.Equal(expected, HashCode<object[]>.GetHashCode(values));
            Assert.NotEqual(expected, provider.GetHashCode(new object[] { values[2], values[1], values[0] }));
        }

        [Fact]
        public void EnumerableProviderUsesElementHashCodesWithoutRecursingIntoNestedSequences()
        {
            var nested = new StableHashSequence(43, 1, 2, 3);
            var values = new object[] { nested };

            Assert.Equal(System.HashCode.Combine(0, 43), HashCode<object[]>.GetHashCode(values));
        }

        [Fact]
        public void DictionaryProviderHandlesInvalidEmptyNullValuesAndInsertionOrder()
        {
            var provider = HashCodeProvider.Create(typeof(Dictionary<string, object>));
            var first = new Dictionary<string, object>
            {
                ["alpha"] = new StableHashValue(13, 1),
                ["null"] = null!,
            };
            var second = new Dictionary<string, object>
            {
                ["null"] = null!,
                ["alpha"] = new StableHashValue(13, 1),
            };
            var firstExpected = CombineDictionary(("alpha".GetHashCode(), 13), ("null".GetHashCode(), 0));
            var secondExpected = CombineDictionary(("null".GetHashCode(), 0), ("alpha".GetHashCode(), 13));

            Assert.Equal(0, provider.GetHashCode(null!));
            Assert.Equal(0, provider.GetHashCode(new[] { 1, 2 }));
            Assert.Equal(0, provider.GetHashCode(new Dictionary<string, object>()));
            Assert.Equal(firstExpected, provider.GetHashCode(first));
            Assert.Equal(secondExpected, HashCode<Dictionary<string, object>>.GetHashCode(second));
            Assert.NotEqual(firstExpected, secondExpected);
        }

        [Fact]
        public void DictionaryWithNonObjectValuesUsesEnumerableProvider()
        {
            var dictionary = new Dictionary<string, int> { ["one"] = 1 };
            var pairHash = ((KeyValuePair<string, int>)Assert.Single(dictionary)).GetHashCode();
            var provider = HashCodeProvider.Create(dictionary.GetType());

            Assert.Contains("EnumerableHashCodeProvider", provider.GetType().Name);
            Assert.Equal(System.HashCode.Combine(0, pairHash), provider.GetHashCode(dictionary));
        }

        [Fact]
        public void HashCodeEqualsComparesHashesIncludingNullsCollisionsAndOperators()
        {
            var first = new StableHashValue(23, 1);
            var sameHash = new StableHashValue(23, 2);
            var differentHash = new StableHashValue(29, 1);

            Assert.True(first != sameHash);
            Assert.False(first == sameHash);
            Assert.True(HashCode<StableHashValue>.HashCodeEquals(first, sameHash));
            Assert.False(HashCode<StableHashValue>.HashCodeEquals(first, differentHash));
            Assert.True(HashCode<StableHashValue>.HashCodeEquals(null!, null!));
            Assert.False(HashCode<StableHashValue>.HashCodeEquals(null!, first));
            Assert.True(HashCode<int[]>.HashCodeEquals(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }));
            Assert.False(HashCode<int[]>.HashCodeEquals(new[] { 1, 2, 3 }, new[] { 3, 2, 1 }));
        }

        [Fact]
        public void ParameterDescriptorsIncludeOrdinaryAndAttributedCallbacksButExcludeBareCallbacks()
        {
            Assert.Single(ParameterDescriptor<PlainComponent>.Descriptors);
            Assert.Empty(ParameterDescriptor<BareCallbackComponent>.Descriptors);
            Assert.Single(ParameterDescriptor<AttributedCallbackComponent>.Descriptors);
            Assert.Single(ParameterDescriptor<CascadingComponent>.Descriptors);

            var callbackComponent = new AttributedCallbackComponent();
            var descriptor = Assert.Single(ParameterDescriptor<AttributedCallbackComponent>.Descriptors);

            Assert.Equal(callbackComponent.Callback.GetHashCode(), descriptor.GetValueHashCode(callbackComponent));
        }

        [Fact]
        public void ParameterDescriptorPrivateConstructorValidatesAndRejectedDescriptorCannotReadValue()
        {
            var constructor = typeof(ParameterDescriptor<BareCallbackComponent>).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                binder: null,
                new[] { typeof(PropertyInfo) },
                modifiers: null)!;

            var nullError = Assert.Throws<TargetInvocationException>(() => constructor.Invoke(new object[] { null! }));
            Assert.IsType<ArgumentNullException>(nullError.InnerException);

            var property = typeof(BareCallbackComponent).GetProperty(nameof(BareCallbackComponent.Callback))!;
            var excluded = (ParameterDescriptor<BareCallbackComponent>)constructor.Invoke(new object[] { property });
            Assert.Throws<NotSupportedException>(() => excluded.GetValueHashCode(new BareCallbackComponent()));
        }

        [Fact]
        public void GetParametersHashCodeCombinesAllIncludedPropertyKinds()
        {
            var component = new CombinedComponent
            {
                Number = 7,
                Values = new[] { 3, 5 },
                Items = new Dictionary<string, object> { ["item"] = new StableHashValue(17, 1) },
            };
            var expected = 0;
            expected = System.HashCode.Combine(expected, 7);
            expected = System.HashCode.Combine(expected, CombineSequence(3, 5));
            expected = System.HashCode.Combine(expected, CombineDictionary(("item".GetHashCode(), 17)));

            Assert.Equal(expected, component.GetParametersHashCode());
        }

        [Fact]
        public void GetParametersHashCodeHandlesNoDescriptorsNullValuesAndNullComponentBoundaries()
        {
            var nullValues = new NullableValuesComponent();
            EmptyComponent nullEmpty = null!;
            PlainComponent nullPlain = null!;

            Assert.Equal(0, new EmptyComponent().GetParametersHashCode());
            Assert.Equal(0, nullEmpty.GetParametersHashCode());
            Assert.Equal(System.HashCode.Combine(0, 0), nullValues.GetParametersHashCode());
            Assert.Throws<NullReferenceException>(() => nullPlain.GetParametersHashCode());
        }

        private static int CombineSequence(params int[] hashes)
        {
            var result = 0;
            foreach (var hash in hashes)
            {
                result = System.HashCode.Combine(result, hash);
            }
            return result;
        }

        private static int CombineDictionary(params (int Key, int Value)[] hashes)
        {
            var result = 0;
            foreach (var hash in hashes)
            {
                result = System.HashCode.Combine(result, hash.Key);
                result = System.HashCode.Combine(result, hash.Value);
            }
            return result;
        }

        private sealed class StableHashValue
        {
            private readonly int _hashCode;
            private readonly int _identity;

            public StableHashValue(int hashCode, int identity)
            {
                _hashCode = hashCode;
                _identity = identity;
            }

            public override bool Equals(object? obj)
            {
                return obj is StableHashValue other && _identity == other._identity;
            }

            public override int GetHashCode()
            {
                return _hashCode;
            }

            public static bool operator ==(StableHashValue? left, StableHashValue? right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(StableHashValue? left, StableHashValue? right)
            {
                return !Equals(left, right);
            }
        }

        private sealed class StableHashSequence : List<int>
        {
            private readonly int _hashCode;

            public StableHashSequence(int hashCode, params int[] values)
                : base(values)
            {
                _hashCode = hashCode;
            }

            public override int GetHashCode()
            {
                return _hashCode;
            }
        }

        private sealed class EmptyComponent : ComponentBase
        {
        }

        private sealed class PlainComponent : ComponentBase
        {
            public int Number { get; set; }
        }

        private sealed class BareCallbackComponent : ComponentBase
        {
            public EventCallback Callback { get; set; }

            public EventCallback<int> GenericCallback { get; set; }
        }

        private sealed class AttributedCallbackComponent : ComponentBase
        {
            [Parameter]
            public EventCallback Callback { get; set; }
        }

        private sealed class CascadingComponent : ComponentBase
        {
            [CascadingParameter]
            public EventCallback<int> Callback { get; set; }
        }

        private sealed class CombinedComponent : ComponentBase
        {
            public int Number { get; set; }

            public int[] Values { get; set; } = Array.Empty<int>();

            public Dictionary<string, object> Items { get; set; } = new Dictionary<string, object>();

            public EventCallback Ignored { get; set; }
        }

        private sealed class NullableValuesComponent : ComponentBase
        {
            public string? Value { get; set; }
        }
    }
}