using MyCalculator;
namespace Shop.Test
{
    public class CalculatorTests
    {
        [Fact]
        public void CalculationSumTest()
        {
            int a = 10, b = 20;
            Calculator calculator = new Calculator();
            int result = calculator.Sum(a, b);
            Assert.Equal(30, result);
           
        }
        [Fact]
        public void CalculationSumTest2()
        {
            int a = 20, b = 20;
            Calculator calculator = new Calculator();
            int result = calculator.Sum(a, b);
            Assert.Equal(50, result);

        }
    }
}
