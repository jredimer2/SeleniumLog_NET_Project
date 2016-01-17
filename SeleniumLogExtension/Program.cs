using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace ClassPropertyName
{
    class Program
    {
        public static void CallerEval(params Expression<Func<object>>[] expressions)
        {
            //Console.WriteLine("input param  {0}.{1}", GetClassName(a), GetVarName(a));  //expected output:  Input param  TestClass.num
            foreach (Expression<Func<object>> memberExpression in expressions)
            {
                MemberExpression expression = null;
                if (memberExpression.Body is MemberExpression)
                    expression = (MemberExpression) memberExpression.Body;
                else if (memberExpression.Body is UnaryExpression)
                {
                    var op = ((UnaryExpression)memberExpression.Body).Operand;
                    expression = (MemberExpression) op;
                }

                if (expression != null)
                {
                    string declaringName = expression.Member.Name;
                    string typeOfMember = expression.Type.Name;
                    string declaringType = string.Empty;
                    object value;

                    if (expression.Member.DeclaringType != null)
                        declaringType = expression.Member.DeclaringType.Name;
                    value = (memberExpression.Compile())();

                    string output = string.Format("Input param: {0}.{1} has value: {2} (Type: {3})", declaringType,
                        declaringName, value, typeOfMember);
                    Console.WriteLine(output);
                }
            }
        }

        private static object GetValue(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }

        public class TestClass
        {
            public string name;
            public double feedback;
            public int age;
        }

        static void Main(string[] args)
        {
            TestClass test = new TestClass();
            test.name = "Joseph Redimerio";
            test.age = 25;
            test.feedback = 5.00;
            CallerEval(() => test.name, () => test.age, () => test.feedback);
            Console.ReadLine();
        }
    }
}
