using HueSharp.Enums;
using HueSharp.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HueSharp
{
    public class ConditionExpressionVisitor : ExpressionVisitor
    {
        public Condition Condition { get; } =  new Condition();
        private IHueRequest _request;

        public ConditionExpressionVisitor(IHueRequest request)
        {
            _request = request;
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Condition.Address = node.Name;
            base.VisitParameter(node);
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            base.Visit(node.Left);
            if (node.NodeType == ExpressionType.GreaterThan)
                Condition.Operator = ConditionOperator.GreatherThanCondition;
            else if (node.NodeType == ExpressionType.LessThan)
                Condition.Operator = ConditionOperator.LessThanCondition;
            else if (node.NodeType == ExpressionType.Equal)
                Condition.Operator = ConditionOperator.EqualsCondition;
            base.Visit(node.Right);

            return node;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Not)
                Condition.Operator = ConditionOperator.ValueChangedCondition;
            return base.VisitUnary(node);
        }
    }
}
