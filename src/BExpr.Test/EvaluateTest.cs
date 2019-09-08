using BExpr;
using BExpr.Model;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace Brifly.Expressions.Test
{
    [TestFixture]
    public class EvaluateTest
    {
        private object testObject = new {
            a = 1,
            b = 1.1,
            c = true,
            d = false,
            e = "test",
            f = (object)null,
            g = 0,
            h = 2.5m,
            i = 3.5f
        };
        
        [TestCase("0 ^ 0", 1)]
        [TestCase("0 ^ 1", 0)]
        [TestCase("1 ^ 0", 1)]
        [TestCase("1 ^ 1", 1)]
        [TestCase("2 ^ 2", 4)]
        [TestCase("a ^ b", 1)]
        [TestCase("b ^ a", 1.1)]
        [TestCase("h ^ i", 24.70529422)]
        [TestCase("2 ^ 2 ^ 2", 16)]
        public void EvaluatePow(string expr, object value) => Evaluate(expr, value);

        [TestCase("1 + 1", 2)]
        [TestCase("a + b", 2.1)]
        [TestCase("a + b + h", 4.6)]
        [TestCase("a + b + h + 1", 5.6)]
        [TestCase("1 - 1", 0)]
        [TestCase("a - b", -0.1)]
        [TestCase("a - b - h", -2.6)]
        [TestCase("a - b - h - 1", -3.6)]
        [TestCase("a - b + h", 2.4)]
        [TestCase("a - b + h - 1", 1.4)]
        public void EvaluateSum(string expr, object value) => Evaluate(expr, value);

        [TestCase("2 * 2", 4)]
        [TestCase("h * i", 8.75)]
        [TestCase("b * h * i", 9.625)]
        [TestCase("b * h * i * a", 9.625)]
        [TestCase("g * b * h * i", 0)]
        public void EvaluateProd(string expr, object value) => Evaluate(expr, value);
        
        [TestCase("b / h / i", 1.1 / 2.5 / 3.5)] // (b / h) / i
        [TestCase("b + h / i", 1.1 + 2.5 / 3.5)] // b + (h / i)
        [TestCase("b + h * i", 1.1 + 2.5 * 3.5)] // b + (h * i)
        [TestCase("b / h + i", 1.1 / 2.5 + 3.5)] // (b / h) + i
        [TestCase("b * h / i", 1.1 * 2.5 / 3.5)] // (b * h) / i
        [TestCase("b / h * i", 1.1 / 2.5 * 3.5)] // (b / h) * i
        public void EvaluateAssociativty(string expr, object value) => Evaluate(expr, value);

        [TestCase("a", 1)]
        public void EvaluateProperty(string expr, object value) => Evaluate(expr, value);

        [TestCase("1", 1)]
        [TestCase("1.1", 1.1)]
        [TestCase("'a'", "a")]
        [TestCase("true", true)]
        public void EvaluateLiteral(string expr, object value) => Evaluate(expr, value);

        [TestCase("-5", -5)]
        [TestCase("--5", 5)]
        [TestCase("+5", 5)]
        [TestCase("-a", -1)]
        [TestCase("!true", false)]
        [TestCase("!false", true)]
        [TestCase("!c", false)]
        [TestCase("!d", true)]
        public void EvaluateUnary(string expr, object value) => Evaluate(expr, value);

        [TestCase("1 == 1", true)]
        [TestCase("1 != 1", false)]
        [TestCase("1 == 0", false)]
        [TestCase("1 != 0", true)]
        public void EvaluateEqualityRelational(string expr, object value) => Evaluate(expr, value);

        [TestCase("1 > 0", true)]
        [TestCase("0 > 1", false)]
        [TestCase("1 > 1", false)]
        [TestCase("0 > -1", true)]
        [TestCase("1 >= 0", true)]
        [TestCase("0 >= 1", false)]
        [TestCase("1 >= 1", true)]
        [TestCase("-1 >= -1", true)]
        [TestCase("1 < 0", false)]
        [TestCase("0 < 1", true)]
        [TestCase("1 < 1", false)]
        [TestCase("1 <= 0", false)]
        [TestCase("0 <= 1", true)]
        [TestCase("1 <= 1", true)]
        public void EvaluateInequalityRelational(string expr, object value) => Evaluate(expr, value);

        [TestCase("1 between { 0, 2 }", true)]
        [TestCase("0 between { 0, 2 }", true)]
        [TestCase("2 between { 0, 2 }", true)]
        [TestCase("0 between { 1, 2 }", false)]
        [TestCase("3 between { 1, 2 }", false)]
        [TestCase("-1 between { 0, 2 }", false)]
        [TestCase("-1 between { -2, 2 }", true)]
        public void EvaluateBetween(string expr, object value) => Evaluate(expr, value);

        [TestCase("{ }")]
        [TestCase("{ null }", null)]
        [TestCase("{ 1 }", 1)]
        [TestCase("{ 1, 2, 3 }", 1, 2, 3)]
        [TestCase("{ 'a', 'b', 'c' }", "a", "b", "c")]
        [TestCase("{ 1.1, 1.2, 1.3 }", 1.1, 1.2, 1.3)]
        [TestCase("{ true, false, true }", true, false, true)]
        [TestCase("{ 1, 'b', 1.1, true, a, null }", 1, "b", 1.1, true, 1, null)]
        [TestCase("{ a, b, c, d, e, f }", 1, 1.1, true, false, "test", null)]
        public void EvaluateList(string exprString, params object[] rest)
        {
            var expr = Expression.Parse(exprString);
            var result = expr.Evaluate(testObject, new EvaluationContext());
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value, Is.EquivalentTo(rest));
        }

        [TestCase("a in { 1, 2, 4 }", true)]
        [TestCase("a in { 0, 2, 4 }", false)]
        [TestCase("'x' in { a, b, c }", false)]
        [TestCase("1.1 in { a, b, c }", true)]
        [TestCase("a in { }", false)]
        [TestCase("null in { null }", true)]
        public void EvaluateIn(string expr, object value) => Evaluate(expr, value);

        [TestCase("'abc' matches '.*'", true)]
        [TestCase(@"'abc' matches '\.\*'", false)]
        [TestCase(@"'.*' matches '\.\*'", true)]
        [TestCase("'abc' matches 'ab[c|d]'", true)]
        [TestCase("'abc' matches 'abd'", false)]
        public void EvaluateMatches(string expr, object value) => Evaluate(expr, value);

        [TestCase("'abc' like '%bc'", true)]
        [TestCase("'abc' like 'ab%'", true)]
        [TestCase("'abc' like 'a%c'", true)]
        [TestCase("'bac' like '%bc'", false)]
        [TestCase("'bac' like 'ab%'", false)]
        [TestCase("'bac' like 'a%c'", false)]
        [TestCase("'([a-zA-Z0-9]*)' like '%-zA-Z0-9]*)'", true)]
        public void EvaluateLike(string expr, object value) => Evaluate(expr, value);

        [Test]
        public void DivideByZero()
        {
            var sw = Stopwatch.StartNew();
            var expr = Expression.Parse("a / g");
            sw.Stop();
            Console.WriteLine("Parse: " + sw.ElapsedMilliseconds);
            sw.Restart();
            var res = expr.Evaluate(testObject, new EvaluationContext());
            sw.Stop();
            Console.WriteLine("Eval: " + sw.ElapsedMilliseconds);

            Assert.That(res.IsError, Is.True);
        }

        private void Evaluate(string exprString, object value)
        {
            var sw = Stopwatch.StartNew();
            var expr = Expression.Parse(exprString);
            sw.Stop();
            var parse = sw.Elapsed;
            sw.Restart();
            var res = expr.Evaluate(testObject, new EvaluationContext());
            sw.Stop();
            var eval = sw.Elapsed;

            Console.WriteLine("Parse: " + parse.TotalMilliseconds);
            Console.WriteLine("Eval: " + eval.TotalMilliseconds);

            Assert.That(res.IsError, Is.False);
            Assert.That(res.Value, Is.EqualTo(value).Within(0.0000001));
        }
    }
}
