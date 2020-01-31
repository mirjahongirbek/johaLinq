

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace JohaLinq
{
    public static class JLinq

    {
        public static List<string> ConvertToList<TProperty, TEntity>(
            this TEntity entity,
            Expression<Func<TEntity, TProperty>> property,
            char selector = ',')
            where TEntity : class
        {
            var pName = GetMemberName(property.Body);
            var value  =GetValue(entity, pName);
           return
                ConvertStringList(value, selector);
            //var list = value.Split(selector);
            //List<string> result = new List<string>();
            //result.AddRange(list);
            //return result;
        }
        public static List<int> ConvertIntList<TEntity, TProperty>(this TEntity entity,
            Expression<Func<TEntity, TProperty>> property,
            char selector=',')
        {
            var pName = GetMemberName(property.Body);
           var value= GetValue(entity, pName);
           return ConvertIntList(value, selector);
        }
        public static List<int> ConvertIntList(this string value, char selector=',')
        {
            if (string.IsNullOrEmpty(value)) return new List<int>();
            var list = value.Split(selector);
            var result = new List<int>();
            for (var i = 0; i < list.Length; i++)
            {
                if (string.IsNullOrEmpty(list[i])) continue;
                result.Add(Convert.ToInt32(list[i]));
            }
            return result;
        }
        public static string ConvertString<T>(this List<T> ts, char selector=',')
            where T:System.Enum
        {
            var result = "";
            foreach(var i in ts)
            {
                if (i== null) continue;
               result+=Convert.ToInt32(i).ToString()+',';
            }
            return result;
        }
        public static List<T> ConvetEnum<T>(this List<int> list)
           where T : System.Enum
        {
            List<T> result = new List<T>();
            foreach (var i in list)
            {
                result.Add((T)Enum.ToObject(typeof(T), i));
            }
            return result;
        }
        public static List<string> ConvertStringList(this string value,
            char selector = ',')
        {
            //TODO shu yerni to`g`irlashim kerak
            List<string> result = new List<string>();
            foreach(var i in value.Split(selector))
            {
                if (string.IsNullOrEmpty(i)) continue ;
                result.Add(i);
            }
           return result;
        }
        public static string ConvertString(this IEnumerable<string> list, string selector="\r\n")
        {
            string result = "";
            foreach(var i in list)
            {
                result += i + selector;
            }
            return result;
        }
        public static bool Contains<T>(this List<T>list, params T[] ts)
        {
            foreach(var i in list)
            {
                if (ts.Contains(i)) return true;
            }
            return false;
        }

        private static string GetValue<TEntity>(TEntity entity, string pName)
        {
            var tip = entity.GetType();
            var props = tip.GetProperty(pName);
            if (props == null) return null;
           var getValue= props.GetValue(entity);
            if (getValue== null) return "";
            var value = getValue.ToString();
            return value;
        }
        private static string GetMemberName(Expression expression)
        {
            if (expression == null)
            {
                //TODO
                throw new ArgumentException("expressionCannotBeNullMessage");
            }

            if (expression is MemberExpression)
            {
                // Reference type property or field
                var memberExpression = (MemberExpression)expression;
                return memberExpression.Member.Name;
            }

            if (expression is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression = (MethodCallExpression)expression;
                return methodCallExpression.Method.Name;
            }

            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression;
                return GetMemberName(unaryExpression);
            }
            //TODO
            throw new ArgumentException("invalidExpressionMessage");
        }
        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                var methodExpression = (MethodCallExpression)unaryExpression.Operand;
                return methodExpression.Method.Name;
            }

            return ((MemberExpression)unaryExpression.Operand).Member.Name;
        }

    }
    //public class JList<T> : List<T>
    //{
    //    public JLinq
    //    public event EventHandler OnAdd;
    //    public new void Add(T item) // "new" to avoid compiler-warnings, because we're hiding a method from base-class
    //    {
    //        if (null != OnAdd)
    //        {
    //            OnAdd(this, null);
    //        }
    //        base.Add(item);
    //    }
    //    public AddProps<TEntity> 
    //}

}
