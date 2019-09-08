using BExpr;
using NUnit.Framework;

namespace Brifly.Expressions.Test
{
    [TestFixture]
    public class ParseTest
    {
        [TestCase("(\na;\nb\n)")]
        [TestCase("(\na;\nb;\nc\n)")]
        [TestCase("(\n1;\n2\n)")]
        [TestCase("(\n1;\n2;\nc\n)")]
        public void VisitExprList(string expression) => ParseExpr(expression);

        [TestCase("(a)")]
        [TestCase("(5)")]
        [TestCase("(a + b)")]
        [TestCase("a - (-b)")]
        public void ParseParenExpr(string expression) => ParseExpr(expression);

        [TestCase("false or true")]
        [TestCase("true or false")]
        [TestCase("true or false or true")]
        [TestCase("a or b")]
        [TestCase("a or true")]
        [TestCase("true or b")]
        [TestCase("true or b or c")]
        public void ParseOrExpression(string expression) => ParseExpr(expression);

        [TestCase("false xor true")]
        [TestCase("true xor false")]
        [TestCase("true xor false xor true")]
        [TestCase("a xor b")]
        [TestCase("a xor true")]
        [TestCase("true xor b")]
        [TestCase("true xor b xor c")]
        public void ParseXorExpression(string expression) => ParseExpr(expression);

        [TestCase("false and true")]
        [TestCase("true and false")]
        [TestCase("true and false and true")]
        [TestCase("a and b")]
        [TestCase("a and true")]
        [TestCase("true and b")]
        [TestCase("true and b and c")]
        public void ParseAndExpression(string expression) => ParseExpr(expression);

        [TestCase("false or true and true")]
        [TestCase("false xor true and true")]
        [TestCase("false or true xor true")]
        [TestCase("a and b or c")]
        [TestCase("a and b xor c")]
        [TestCase("a xor b or c")]
        [TestCase("a or true and false")]
        [TestCase("true or b xor c")]
        [TestCase("true or b and c")]
        public void ParseMixedLogical(string expression) => ParseExpr(expression);

        [TestCase("1 == 0")]
        [TestCase("1 != 0")]
        [TestCase("1 > 0")]
        [TestCase("1 < 0")]
        [TestCase("1 >= 0")]
        [TestCase("1 <= 0")]
        [TestCase("a == b")]
        [TestCase("a != b")]
        [TestCase("a > b")]
        [TestCase("a < b")]
        [TestCase("a >= b")]
        [TestCase("a <= b")]
        [TestCase("a == 0")]
        [TestCase("a != 0")]
        [TestCase("a > 0")]
        [TestCase("a < 0")]
        [TestCase("a >= 0")]
        [TestCase("a <= 0")]
        public void ParseRelational(string expr) => ParseExpr(expr);

        [TestCase("a in { 1, 2, 4 }")]
        [TestCase("'x' in { a, b, c }")]
        [TestCase("a in { }")]
        public void ParseIn(string expr) => ParseExpr(expr);

        [TestCase("a between { 0, 1 }")]
        [TestCase("a between x")]
        [TestCase("1 between { a, b }")]
        [TestCase("a between { 0, b }")]
        public void ParseBetween(string expr) => ParseExpr(expr);

        [TestCase("a matches '.*'")]
        [TestCase("'a' matches '.*'")]
        public void ParseMatches(string expr) => ParseExpr(expr);

        [TestCase("a like '%a'")]
        [TestCase("'a' matches '%a'")]
        public void ParseLike(string expr) => ParseExpr(expr);

        [TestCase("3 + 4")]
        [TestCase("2 - 1")]
        [TestCase("-5 + 1")]
        [TestCase("1 + 2 + 3")]
        [TestCase("3 + 4 - 5")]
        [TestCase("5 + (6 - 7)")]
        [TestCase("a + b")]
        [TestCase("a - b")]
        [TestCase("-a + b")]
        [TestCase("a + b + c")]
        [TestCase("a + b - c")]
        [TestCase("a + (b - c)")]
        [TestCase("a + 1")]
        [TestCase("3 - b")]
        [TestCase("-2 + b")]
        [TestCase("2 + b + 1")]
        [TestCase("a + 2 - 3")]
        [TestCase("a + (4 - c)")]
        public void ParseSumExpr(string expression) => ParseExpr(expression);

        [TestCase("a * b")]
        [TestCase("a / b")]
        [TestCase("a % b")]
        [TestCase("a * -b")]
        [TestCase("a * b * c")]
        [TestCase("a / b * c")]
        [TestCase("a / (b * c)")]
        public void ParseProdExpr(string expression) => ParseExpr(expression);

        [TestCase("2 ^ 2")]
        [TestCase("x ^ y")]
        [TestCase("x ^ (y + 1)")]
        [TestCase("(x + y) ^ (y + z)")]
        [TestCase("(x + y) ^ -z")]
        public void ParsePow(string expression) => ParseExpr(expression);

        [TestCase("+4")]
        [TestCase("-1")]
        [TestCase("!true")]
        [TestCase("+a")]
        [TestCase("-a")]
        [TestCase("!a")]
        public void ParseUnary(string expression) => ParseExpr(expression);

        [TestCase("#f()")]
        [TestCase("#f(a)")]
        [TestCase("#f(a, b)")]
        [TestCase("#f(a + 1, b - 1)")]
        [TestCase("#f(#g(x + 1))")]
        public void ParseFunction(string expression) => ParseExpr(expression);

        [TestCase("f()")]
        [TestCase("f(a)")]
        [TestCase("f(a, b)")]
        [TestCase("f(a + 1, b - 1)")]
        [TestCase("f(g(x + 1))")]
        public void ParseMethod(string expression) => ParseExpr(expression);

        [TestCase("a")]
        public void ParseProperty(string expression) => ParseExpr(expression);

        [TestCase("1")]
        [TestCase("1.1")]
        [TestCase("'a'")]
        [TestCase("true")]
        public void ParseLiteral(string expression) => ParseExpr(expression);
        
        
        [TestCase("{ }")]
        [TestCase("{ 1 }")]
        [TestCase("{ 'a' }")]
        [TestCase("{ 0.1 }")]
        [TestCase("{ null }")]
        [TestCase("{ PropA }")]
        [TestCase("{ 1, 2 }")]
        [TestCase("{ 'a', 'b' }")]
        [TestCase("{ true, false }")]
        [TestCase("{ null, null }")]
        [TestCase("{ PropA, PropB }")]
        [TestCase("{ PropA, 'a', 1, 0.1, false, null }")]
        public void ParseList(string expr) => ParseExpr(expr);

        private void ParseExpr(string expression)
        {
            var expr = Expression.Parse(expression);
            Assert.That(expr.ToString(), Is.EqualTo(expression));
        }
    }
}
