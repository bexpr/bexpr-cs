using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using BExpr.Core.Lang;

namespace BExpr.Model
{
    public class DefaultExpressionVisitor<T> : ExpressionBaseVisitor<IExpression<T>>
    {
        private readonly IPropertyValueProvider<T> propertyValueProvider;
        private readonly IMethodProvider<T> methodProvider;

        public DefaultExpressionVisitor(
            IPropertyValueProvider<T> propertyValueProvider = null, 
            IMethodProvider<T> methodProvider = null)
        {
            this.propertyValueProvider = propertyValueProvider ?? new ReflectionPropertyValueProvider<T>();
            this.methodProvider = methodProvider ?? new ReflectionMethodProvider<T>(); 
        }
        public override IExpression<T> VisitExpr(ExpressionParser.ExprContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override IExpression<T> VisitExprList(ExpressionParser.ExprListContext context)
        {
            var exprList = Enumerable.Range(1, context.ChildCount - 2)
                .Where(i => context.GetChild(i).GetText() != ";")
                .Select(i => Visit(context.GetChild(i)))
                .ToList()
                .AsReadOnly();

            return new ExprList<T>(exprList);
        }

        public override IExpression<T> VisitParenExpr(ExpressionParser.ParenExprContext context)
        {
            return new ParenExpr<T>(Visit(context.GetChild(1)));
        }
        public override IExpression<T> VisitLogicalOrExpression([NotNull] ExpressionParser.LogicalOrExpressionContext context)
        {
            return VisitBinaryExpressionList(context.children, (l, r, op) => new Or<T>(l, r));
        }

        public override IExpression<T> VisitLogicalXorExpression([NotNull] ExpressionParser.LogicalXorExpressionContext context)
        {
            return VisitBinaryExpressionList(context.children, (l, r, op) => new Xor<T>(l, r));
        }

        public override IExpression<T> VisitLogicalAndExpression([NotNull] ExpressionParser.LogicalAndExpressionContext context)
        {
            return VisitBinaryExpressionList(context.children, (l, r, op) => new And<T>(l, r));
        }

        public override IExpression<T> VisitRelationalExpression([NotNull] ExpressionParser.RelationalExpressionContext context)
        {
            return VisitBinaryExpressionList(context.children, CreateRelationalExpr);

            IExpression<T> CreateRelationalExpr(IExpression<T> left, IExpression<T> right, IParseTree op)
            {
                switch (op.GetText())
                {
                    case "==":
                        return new Equals<T>(left, right);
                    case "!=":
                        return new NotEquals<T>(left, right);
                    case "<":
                        return new LessThan<T>(left, right);
                    case "<=":
                        return new LessThanOrEqual<T>(left, right);
                    case ">":
                        return new GreaterThan<T>(left, right);
                    case ">=":
                        return new GreaterThanOrEqual<T>(left, right);
                    case "in":
                        return new In<T>(left, right);
                    case "is":
                        return new Is<T>(left, right);
                    case "between":
                        return new Between<T>(left, right);
                    case "like":
                        return new Like<T>(left, right);
                    case "matches":
                        return new Matches<T>(left, right);
                }
                throw new Exception("Unsupported relational operator " + op.GetText());
            }
        }

        public override IExpression<T> VisitSumExpr(ExpressionParser.SumExprContext context)
        {
            return VisitBinaryExpressionList(context.children, CreateProdExpr);

            IExpression<T> CreateProdExpr(IExpression<T> left, IExpression<T> right, IParseTree op)
            {
                switch (op.GetText())
                {
                    case "-":
                        return new Minus<T>(left, right);
                    case "+":
                        return new Plus<T>(left, right);
                }
                throw new Exception("Unsupported sum operator " + op.GetText());
            }
        }

        public override IExpression<T> VisitProdExpr([NotNull] ExpressionParser.ProdExprContext context)
        {
            return VisitBinaryExpressionList(context.children, CreateProdExpr);

            IExpression<T> CreateProdExpr(IExpression<T> left, IExpression<T> right, IParseTree op)
            {
                switch (op.GetText())
                {
                    case "*":
                        return new Multipy<T>(left, right);
                    case "/":
                        return new Divide<T>(left, right);
                    case "%":
                        return new Modulus<T>(left, right);
                }
                throw new Exception("Unsupported product operator " + op.GetText());
            }
        }

        public override IExpression<T> VisitPowExpr([NotNull] ExpressionParser.PowExprContext context)
        {
            return VisitBinaryExpressionList(context.children, (b, e, op) => new Power<T>(b, e));
        }

        public override IExpression<T> VisitUnaryExpression([NotNull] ExpressionParser.UnaryExpressionContext context)
        {
            if (context.ChildCount == 1)
            {
                return Visit(context.GetChild(0));
            }

            var op = context.GetChild(0);
            var expr = Visit(context.GetChild(1));

            switch (op.GetText())
            {
                case "-":
                    return new UnaryMinus<T>(expr);
                case "+":
                    return new UnaryPlus<T>(expr);
                case "!":
                    return new Not<T>(expr);
            }

            throw new Exception($"Unsupported unary operator: {op.GetText()}");
        }

        public override IExpression<T> VisitFunction([NotNull] ExpressionParser.FunctionContext context)
        {
            var name = context.GetChild(1).GetText();
            var methodArgsNode = Visit(context.GetChild(2));
            return new Function<T>(name, methodArgsNode);
        }

        public override IExpression<T> VisitMethod([NotNull] ExpressionParser.MethodContext context)
        {
            var name = context.GetChild(0).GetText();
            var methodArgsNode = Visit(context.GetChild(1));
            return new Method<T>(name, methodArgsNode, methodProvider);
        }
        
        public override IExpression<T> VisitMethodArgs([NotNull] ExpressionParser.MethodArgsContext context)
        {
            var expressions = Enumerable.Range(1, context.ChildCount - 2)
                .Select(i => context.GetChild(i))
                .OfType<ExpressionParser.ArgumentContext>()
                .Select(c => Visit(c))
                .ToList();

            return new MethodArgs<T>(expressions);
        }

        public override IExpression<T> VisitProperty(ExpressionParser.PropertyContext context)
        {
            return new Property<T>(context.GetText(), propertyValueProvider);
        }

        public override IExpression<T> VisitBoolLiteral([NotNull] ExpressionParser.BoolLiteralContext context)
        {
            var value = context.GetText() == "true";
            return new BoolLiteral<T>(value);
        }

        public override IExpression<T> VisitStringLiteral([NotNull] ExpressionParser.StringLiteralContext context)
        {
            var str = context.GetText();
            return new StringLiteral<T>(str.Substring(1, str.Length - 2));
        }

        public override IExpression<T> VisitRealLiteral([NotNull] ExpressionParser.RealLiteralContext context)
        {
            var value = double.Parse(context.GetText());
            return new DoubleLiteral<T>(value);
        }

        public override IExpression<T> VisitIntegerLiteral([NotNull] ExpressionParser.IntegerLiteralContext context)
        {
            var value = int.Parse(context.GetText());
            return new IntLiteral<T>(value);
        }

        public override IExpression<T> VisitNullLiteral([NotNull] ExpressionParser.NullLiteralContext context)
        {
            return new NullLiteral<T>();
        }

        public override IExpression<T> VisitListInitializer([NotNull] ExpressionParser.ListInitializerContext context)
        {
            var expressions = Enumerable.Range(1, context.ChildCount - 2)
                .Select(i => context.GetChild(i))
                .OfType<ExpressionParser.ExpressionContext>()
                .Select(c => Visit(c));

            return new List<T>(expressions.ToList());
        }
        
        private IExpression<T> VisitBinaryExpressionList(IList<IParseTree> parseTrees,
            Func<IExpression<T>, IExpression<T>, IParseTree, IExpression<T>> create)
        {
            if (parseTrees.Count == 1)
            {
                return Visit(parseTrees[0]);
            }

            var pivot = parseTrees.Count - 2;
            var op = parseTrees[pivot];
            var left = parseTrees.Take(pivot).ToList();
            var right = parseTrees.Last();

            var leftExpr = VisitBinaryExpressionList(left, create);
            var rightExpr = VisitBinaryExpressionList(new[] { right }, create);

            return create(leftExpr, rightExpr, op);
        }
    }
}