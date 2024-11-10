using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using quiz.ModelBusiness;

namespace quiz.Shared
{
    /// <summary>
    /// Общие функции и данные для всего приложения.
    /// </summary>
    public static class CommonLogic
    {
        /// <summary>
        /// Получение имени класса из цепочки вызовов.
        /// По материалам https://stackoverflow.com/a/48758173
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string? GetStackTraceClassName(int frameIndex)
        {
            string? result = new StackTrace().GetFrame(frameIndex)?.GetMethod()?.GetStackTraceClassName();
        
            return result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string? GetStackTraceClassName(this MethodBase method)
        {
            string? result = null;
            var declaringType = method.DeclaringType;

            if (declaringType != null)
            {
                if (declaringType.GetInterfaces().Any(i => i == typeof(IAsyncStateMachine)))
                {
                    Type generatedType = declaringType;
                    Type? originalType = generatedType.DeclaringType;

                    if (originalType != null)
                    {
                        MethodInfo foundMethod = originalType.GetMethods(
                                BindingFlags.Instance | BindingFlags.Static
                                                      | BindingFlags.Public | BindingFlags.NonPublic
                                                      | BindingFlags.DeclaredOnly)
                            .Single(m => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType == generatedType);
                        Type? foundMethodDeclaringType = foundMethod.DeclaringType;

                        if (foundMethodDeclaringType != null)
                        {
                            result =  foundMethodDeclaringType.Name;
                        }
                    }
                }
                else
                {
                    return method.DeclaringType?.Name;
                }
            }

            return result;
        }

        /// <summary>
        /// Получение имени метода из цепочки вызовов.
        /// По материалам https://stackoverflow.com/a/48758173
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string? GetStackTraceMethodName(int frameIndex)
        {
            string? result = new StackTrace().GetFrame(frameIndex)?.GetMethod()?.GetStackTraceMethodName();
        
            return result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string? GetStackTraceMethodName(this MethodBase method)
        {
            string? result = null;
            var declaringType = method.DeclaringType;

            if (declaringType != null)
            {
                if (declaringType.GetInterfaces().Any(i => i == typeof(IAsyncStateMachine)))
                {
                    Type generatedType = declaringType;
                    Type? originalType = generatedType.DeclaringType;

                    if (originalType != null)
                    {
                        MethodInfo foundMethod = originalType.GetMethods(
                                BindingFlags.Instance | BindingFlags.Static
                                                      | BindingFlags.Public | BindingFlags.NonPublic
                                                      | BindingFlags.DeclaredOnly)
                            .Single(m => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType == generatedType);
                        Type? foundMethodDeclaringType = foundMethod.DeclaringType;

                        if (foundMethodDeclaringType != null)
                        {
                            result =  foundMethod.Name;
                        }
                    }
                }
                else
                {
                    result = method.Name;
                }
            }

            return result;
        }

        /// <summary>
        /// Получение текста выбранного пользователем ответа.
        /// </summary>
        public static string SelectedAnswerTextGet(QuestionModel proceedingQuestion)
        {
            string? result = null;

            if (proceedingQuestion.OptionIsSelected01)
            {
                result = proceedingQuestion.Option01;
            }
            else if (proceedingQuestion.OptionIsSelected02)
            {
                result = proceedingQuestion.Option02;
            }
            else if (proceedingQuestion.OptionIsSelected03)
            {
                result = proceedingQuestion.Option03;
            }
            else if (proceedingQuestion.OptionIsSelected04)
            {
                result = proceedingQuestion.Option04;
            }

            return result ?? string.Empty;
        }
    }
}