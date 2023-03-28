using FluentAssertions;
using Xunit;

namespace FormulaeTests
{
    public class ValueTests
    {
        [Fact]
        public void DoubleValue_should_be_rounded_to_the_given_precision()
        {
            var value = new Number(23.14, 1);
            value.Value.Should().Be(23.1);
        }

        [Fact]
        public void Values_should_be_equivalent_when_rounded_to_the_given_precision()
        {
            var value = new Number(23.14, 1);
            var value2 = new Number(23.06, 1);
            value.Should().BeEquivalentTo(value2);
        }
    }
}
