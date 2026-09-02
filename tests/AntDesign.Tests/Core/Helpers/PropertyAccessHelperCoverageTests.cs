using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AntDesign.Core.Helpers;
using Xunit;

namespace AntDesign.Tests.Core.Helpers
{
    public class PropertyAccessHelperCoverageTests
    {
        [Fact]
        public void BuildsNonNullableAccessExpressionsFromTypesAndParameters()
        {
            var model = CreateModel();

            var untyped = typeof(TestModel).BuildAccessPropertyLambdaExpression("Child.Name").Compile();
            var typedFromPath = typeof(TestModel).BuildAccessPropertyLambdaExpression<TestModel, string>("Child.Name").Compile();
            var typedFromParts = typeof(TestModel).BuildAccessPropertyLambdaExpression<TestModel, int>(new[] { "Point", "X" }).Compile();
            var parameter = Expression.Parameter(typeof(TestModel));
            var typedFromParameter = parameter.BuildAccessPropertyLambdaExpression<TestModel, int>(new[] { "OptionalPoint", "X" }).Compile();

            Assert.Equal("nested", untyped.DynamicInvoke(model));
            Assert.Equal("nested", typedFromPath(model));
            Assert.Equal(7, typedFromParts(model));
            Assert.Equal(11, typedFromParameter(model));
        }

        [Fact]
        public void BuildsNullableAccessExpressionsForClassValueAndNullableValuePaths()
        {
            var model = CreateModel();
            var missing = new TestModel();

            var untyped = typeof(TestModel).BuildAccessNullablePropertyLambdaExpression("Child.Name").Compile();
            var typedFromPath = typeof(TestModel).BuildAccessNullablePropertyLambdaExpression<TestModel, int?>("Child.Score").Compile();
            var typedFromParts = typeof(TestModel).BuildAccessNullablePropertyLambdaExpression<TestModel, int?>(new[] { "OptionalPoint", "X" }).Compile();
            var parameter = Expression.Parameter(typeof(TestModel));
            var typedFromParameter = parameter.BuildAccessNullablePropertyLambdaExpression<TestModel, int?>(new[] { "Point", "X" }).Compile();

            Assert.Equal("nested", untyped.DynamicInvoke(model));
            Assert.Null(untyped.DynamicInvoke(missing));
            Assert.Equal(23, typedFromPath(model));
            Assert.Null(typedFromPath(missing));
            Assert.Equal(11, typedFromParts(model));
            Assert.Null(typedFromParts(missing));
            Assert.Equal(7, typedFromParameter(model));
        }

        [Fact]
        public void AccessPropertySupportsSeparatorsAndIndexableTypes()
        {
            var model = CreateModel();

            Assert.Equal("nested", Compile<TestModel, string>(typeof(TestModel).AccessProperty("Child/Name", "/"))(model));
            Assert.Equal("one", Compile<TestModel, string>(typeof(TestModel).AccessProperty(new[] { "Names[1]" }))(model));
            Assert.Equal("zero", Compile<TestModel, string>(typeof(TestModel).AccessProperty("Names[0]"))(model));
            Assert.Equal("value", Compile<TestModel, string>(typeof(TestModel).AccessProperty("Values[\"key\"]"))(model));
            Assert.Equal("one", Compile<string[], string>(typeof(string[]).AccessProperty("[1]"))(model.Names));
            Assert.Equal(7, Compile<TestModel, int>(Expression.Parameter(typeof(TestModel)).AccessProperty("Point.X"))(model));
            Assert.Throws<InvalidOperationException>(() => typeof(TestModel).AccessProperty("Name[0]"));
        }

        [Fact]
        public void AccessNullablePropertyChecksArrayListAndDictionaryBounds()
        {
            var model = CreateModel();

            var arrayItem = Compile<TestModel, string>(typeof(TestModel).AccessNullableProperty("Names[1]"));
            var arrayMissing = Compile<TestModel, string>(typeof(TestModel).AccessNullableProperty("Names[-1]"));
            var listItem = Compile<TestModel, string>(typeof(TestModel).AccessNullableProperty("Labels[0]"));
            var listMissing = Compile<TestModel, string>(typeof(TestModel).AccessNullableProperty("Labels[9]"));
            var dictionaryItem = Compile<TestModel, string>(typeof(TestModel).AccessNullableProperty("Values[\"key\"]"));
            var dictionaryMissing = Compile<TestModel, string>(typeof(TestModel).AccessNullableProperty("Values[\"missing\"]"));
            var directArrayItem = Compile<string[], string>(typeof(string[]).AccessNullableProperty("[0]"));

            Assert.Equal("one", arrayItem(model));
            Assert.Null(arrayMissing(model));
            Assert.Equal("label", listItem(model));
            Assert.Null(listMissing(model));
            Assert.Equal("value", dictionaryItem(model));
            Assert.Null(dictionaryMissing(model));
            Assert.Equal("zero", directArrayItem(model.Names));
            Assert.Throws<InvalidOperationException>(() => typeof(string[]).AccessNullableProperty("[\"key\"]"));
            Assert.Throws<InvalidOperationException>(() => typeof(List<string>).AccessNullableProperty("[\"key\"]"));
            Assert.Throws<InvalidOperationException>(() => typeof(TestModel).AccessNullableProperty("Name[0]"));
        }

        [Fact]
        public void DefaultIfNullHandlesValueNullableValueAndReferenceProperties()
        {
            var model = CreateModel();
            var missing = new TestModel();

            var valueFromStruct = Compile<TestPoint, int>(typeof(TestPoint).AccessPropertyDefaultIfNull("X", 41));
            var nullableFromStruct = Compile<TestPoint, int>(typeof(TestPoint).AccessPropertyDefaultIfNull("Optional", 42));
            var nullableResult = Compile<TestPoint, int?>(typeof(TestPoint).AccessPropertyDefaultIfNull("Optional", (int?)43));
            var valueFromClass = Compile<TestModel, int>(typeof(TestModel).AccessPropertyDefaultIfNull("Number", 44));
            var nestedValue = Compile<TestModel, int>(typeof(TestModel).AccessPropertyDefaultIfNull("Child/Score", "/", 45));
            var reference = Compile<TestModel, string>(typeof(TestModel).AccessPropertyDefaultIfNull("Name", "fallback"));

            Assert.Equal(7, valueFromStruct(model.Point));
            Assert.Equal(42, nullableFromStruct(model.Point));
            Assert.Equal(43, nullableResult(model.Point));
            Assert.Equal(5, valueFromClass(model));
            Assert.Equal(0, valueFromClass(missing));
            Assert.Equal(23, nestedValue(model));
            Assert.Equal(45, nestedValue(missing));
            Assert.Equal("root", reference(model));
            Assert.Equal("fallback", reference(missing));
            Assert.Throws<InvalidOperationException>(() => typeof(TestPoint).AccessPropertyDefaultIfNull("Optional", (long?)1));
            Assert.Throws<InvalidOperationException>(() => typeof(TestModel).AccessPropertyDefaultIfNull("Name", 1));
        }

        [Fact]
        public void ExpressionConversionHelpersFindTheRootParameter()
        {
            var parameter = Expression.Parameter(typeof(TestModel));
            var member = parameter.AccessProperty("Point.X");
            var nullableMember = parameter.AccessNullableProperty("Child.Score");

            var lambda = member.ToLambdaExpression();
            var function = member.ToFuncExpression<TestModel, int>().Compile();
            var nullableDelegate = nullableMember.ToDelegate();

            Assert.Equal(7, lambda.Compile().DynamicInvoke(CreateModel()));
            Assert.Equal(7, function(CreateModel()));
            Assert.Equal(23, nullableDelegate.DynamicInvoke(CreateModel()));
            Assert.Throws<NullReferenceException>(() => Expression.Constant(1).ToLambdaExpression());
        }

        [Theory]
        [InlineData("Name]")]
        [InlineData("Names[]")]
        [InlineData("Names[abc]")]
        [InlineData("Names[[0]")]
        [InlineData("Names[0]]")]
        public void InvalidIndexPathsAreRejected(string path)
        {
            Assert.Throws<InvalidOperationException>(() => typeof(TestModel).AccessProperty(path));
        }

        [Fact]
        public void PublicEntryPointsValidateNullAndEmptyArguments()
        {
            Type nullType = null!;
            Expression nullExpression = null!;
            ParameterExpression nullParameter = null!;

            Assert.Throws<ArgumentNullException>(() => nullType.BuildAccessPropertyLambdaExpression("Name"));
            Assert.Throws<ArgumentException>(() => typeof(TestModel).BuildAccessPropertyLambdaExpression(Array.Empty<string>()));
            Assert.Throws<ArgumentNullException>(() => nullParameter.BuildAccessPropertyLambdaExpression<TestModel, string>(new[] { "Name" }));
            Assert.Throws<ArgumentException>(() => typeof(TestModel).AccessNullableProperty(Array.Empty<string>()));
            Assert.Throws<ArgumentNullException>(() => nullExpression.AccessNullableProperty("Name"));
            Assert.Throws<ArgumentException>(() => typeof(TestModel).AccessProperty(Array.Empty<string>()));
            Assert.Throws<ArgumentNullException>(() => nullExpression.AccessProperty("Name"));
            Assert.Throws<ArgumentNullException>(() => nullExpression.ToDelegate());
            Assert.Throws<ArgumentNullException>(() => nullExpression.ToFuncExpression<TestModel, string>());
        }

        private static Func<TItem, TValue> Compile<TItem, TValue>(Expression body)
        {
            var parameterFinder = new ParameterFinder();
            parameterFinder.Visit(body);
            return Expression.Lambda<Func<TItem, TValue>>(body, parameterFinder.Parameter!).Compile();
        }

        private sealed class ParameterFinder : ExpressionVisitor
        {
            public ParameterExpression? Parameter { get; private set; }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                Parameter ??= node;
                return base.VisitParameter(node);
            }
        }

        private static TestModel CreateModel()
        {
            return new TestModel
            {
                Name = "root",
                Number = 5,
                Child = new TestChild { Name = "nested", Score = 23 },
                Point = new TestPoint { X = 7 },
                OptionalPoint = new TestPoint { X = 11 },
                Names = new[] { "zero", "one" },
                Labels = new List<string> { "label" },
                Values = new Dictionary<string, string> { ["key"] = "value" },
            };
        }

        private sealed class TestModel
        {
            public string Name { get; set; } = null!;

            public int Number { get; set; }

            public TestChild Child { get; set; } = null!;

            public TestPoint Point { get; set; }

            public TestPoint? OptionalPoint { get; set; }

            public string[] Names { get; set; } = null!;

            public List<string> Labels { get; set; } = null!;

            public Dictionary<string, string> Values { get; set; } = null!;
        }

        private sealed class TestChild
        {
            public string Name { get; set; } = null!;

            public int Score { get; set; }
        }

        private struct TestPoint
        {
            public int X { get; set; }

            public int? Optional { get; set; }
        }
    }
}
