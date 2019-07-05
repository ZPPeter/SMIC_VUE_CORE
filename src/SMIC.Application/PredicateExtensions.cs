using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
namespace SMIC
{
    /// <summary>
    /// 动态 LINQ Lambda 表达式
    /// </summary>
    public static class PredicateExtensions
    {
        // public static Expression<Func<T, bool>> True<T>() { return f => true; }     // Error 不能用 - 新版本 .NET 不支持了
        // public static Expression<Func<T, bool>> False<T>() { return f => false; }   // Error 不能用
        
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var invokedExpression = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.Or(expression1.Body, invokedExpression), expression1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var invokedExpression = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.And(expression1.Body, invokedExpression), expression1.Parameters);
        }
    }
}