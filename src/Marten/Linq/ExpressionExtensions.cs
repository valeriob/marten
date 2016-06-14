﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Baseline;
using Remotion.Linq.Clauses.Expressions;

namespace Marten.Linq
{
    public static class ExpressionExtensions
    {
        public static object Value(this Expression expression)
        {
            if (expression is PartialEvaluationExceptionExpression)
            {
                var partialEvaluationExceptionExpression = expression.As<PartialEvaluationExceptionExpression>();
                var inner = partialEvaluationExceptionExpression.Exception;

                throw new BadLinqExpressionException($"Error in value expression inside of the query for '{partialEvaluationExceptionExpression.EvaluatedExpression}'. See the inner exception:", inner);
            }

            if (expression is ConstantExpression)
            {
                // TODO -- handle nulls
                // TODO -- check out more types here.
                return expression.As<ConstantExpression>().Value;
            }

            throw new NotSupportedException();
        }

        public static bool IsValueExpression(this Expression expression)
        {
            Type[] valueExpressionTypes = {
                typeof (ConstantExpression), typeof (PartialEvaluationExceptionExpression)
            };
            return valueExpressionTypes.Any(t => t.IsInstanceOfType(expression));
        }
    }
}