using System;

namespace Zephyr.Extensions
{
    public static class DelegateExtensions
    {
        /// <summary>
        ///     Calls the delegate if it is not null
        /// </summary>
        /// <param name="action">The action to be called</param>
        /// <param name="go">The generic object passed</param>
        public static void Run<T>(this Action<T> action, T go)
        {
            if (action == null) return;
            action(go);
        }


        /// <summary>
        ///     Calls the delegate if it is not null
        /// </summary>
        /// <param name="action">The action to be called</param>
        public static void Run(this Action action)
        {
            if (action == null) return;
            action();
        }

        /// <summary>
        ///     Run the specified func and returns true if all values where true.
        /// </summary>
        /// <param name="func">The Func to perform this on</param>
        public static bool Run(this Func<bool> func)
        {
            if (func == null)
            {
                return false;
            }

            // Store all attached Functions to this Func in an Array
            var functions = func.GetInvocationList();

            // For each Func in the Func Array...
            foreach (var t in functions)
            {
                // ...Get the inidividul function at position i...
                var function = (Func<bool>) t;

                // ...If the function attached returns false...
                if (function() == false)
                {
                    // ...Then return false, as no single function must false for this function to return true.
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///     Run the specified func with the passedParameter and return true if all funcs returned true.
        /// </summary>
        /// <param name="func">Func to Run</param>
        /// <param name="passedParameter">Passed parameter to include with the Func as a parameter</param>
        /// <typeparam name="T">The type of parameter and first Func value.</typeparam>
        public static bool Run<T>(this Func<T, bool> func, T passedParameter)
        {
            // Store all attached Functions to this Func in an Array
            var functions = func.GetInvocationList();

            // For each Func in the Func Array...
            foreach (var t in functions)
            {
                // ...Get the inidividul function at position i...
                var function = (Func<T, bool>) t;

                // ...If the function attached returns false...
                if (function(passedParameter) == false)
                {
                    // ...Then return false, as no single function must false for this function to return true.
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Calls the delegate if it is not null and returns true if the func is null or if all invocations return true
        /// </summary>
        /// <param name="func">The func to be called</param>
        public static bool RunOrIsNull(this Func<bool> func)
        {
            // If the Func that called this Method is null...
            return func == null || func.Run();
        }

        /// <summary>
        ///     Calls the delegate if it is not null and returns true if the func is null or if all invocations return true
        /// </summary>
        /// <param name="func">The func to be called</param>
        /// <param name="passedParameter">The passed parameter of the Func</param>
        public static bool RunOrIsNull<T>(this Func<T, bool> func, T passedParameter)
        {
            return func == null || func.Run(passedParameter);
        }
    }
}