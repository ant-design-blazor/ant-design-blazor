using System;
using System.Reflection;
using AntDesign;
using Xunit;

namespace AntDesign.Tests.Input;

public class AntInputComponentBaseTests
{
    [Fact]
    public void SafeGetValue_ReturnsDefaultWhenModelValueHasWrongType()
    {
        var input = new TestInput();
        typeof(AntInputComponentBase<DateTime?[]>)
            .GetField("_getValueDelegate", BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(input, (Func<object, DateTime?[]>)(_ => throw new InvalidCastException()));

        var result = typeof(AntInputComponentBase<DateTime?[]>)
            .GetMethod("SafeGetValue", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(input, new object[] { new object() });

        Assert.Null(result);
    }

    private sealed class TestInput : AntInputComponentBase<DateTime?[]>
    {
        protected override bool TryParseValueFromString(string value, out DateTime?[] result, out string validationErrorMessage)
        {
            result = default;
            validationErrorMessage = string.Empty;
            return true;
        }

        protected override string FormatValueAsString(DateTime?[] value) => string.Empty;
    }
}
