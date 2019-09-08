using System;
using System.Diagnostics;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using BExpr.Core.Lang;
using BExpr.Model;

[assembly: CLSCompliant(false)]

namespace BExpr
{
    public static class Expression 
    {
        public static IExpression<object> Parse(string expression) 
        {
            return Parse(expression, new ReflectionPropertyValueProvider<object>());
        }

        public static IExpression<T> Parse<T>(string expression, IPropertyValueProvider<T> valueProvider) 
        {
            var sw = Stopwatch.StartNew();
            var charStream = new AntlrInputStream(expression);
            var lexer = new ExpressionLexer(charStream);            
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new ExpressionParser(tokenStream);
            var visitor = new DefaultExpressionVisitor<T>(valueProvider);
            sw.Stop();
            Console.WriteLine("  Setup: " + sw.ElapsedMilliseconds);
            sw.Restart();
            var exprContext = parser.expr();
            sw.Stop();
            Console.WriteLine("  Parse: " + sw.ElapsedMilliseconds);
            sw.Restart();
            var expr = visitor.Visit(exprContext);
            sw.Stop();
            Console.WriteLine("  Visit: " + sw.ElapsedMilliseconds);
            return expr;
        }

        public static string ToStringParseTree(this IParseTree parseTree, int level = 0)
        {
            var children = Enumerable.Range(0, parseTree.ChildCount)
                .Select(t => parseTree.GetChild(t).ToStringParseTree(level + 1));
            var childText = string.Join("\n", children);

            if (parseTree is RuleContext rule)
            {
                var name = ExpressionParser.ruleNames[rule.RuleIndex];
                return new string(' ', level * 3) + name + "[" + rule.GetText() + "]\n" + childText;
            }

            return new string(' ', level * 3) + parseTree.GetText();
        }

    }
}
