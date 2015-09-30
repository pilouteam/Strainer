using System;
using System.Linq.Expressions;

namespace Strainer.Extensions
{
    public static class ExpressionExtensions
    {
        public static MemberExpression GetMemberExpression<T, TValue>(this Expression<Func<T, TValue>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException();
            }

            MemberExpression memberExpression = null;
            if (expression.Body is MemberExpression)
            {
                memberExpression = (MemberExpression)expression.Body;
            }
            else if (expression.Body is UnaryExpression)
            {
                UnaryExpression unaryExpression = (UnaryExpression)expression.Body;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new Exception("The body of the expression must be either a MemberExpression of a UnaryExpression.");
            }
            return memberExpression;
        }
    }
}

