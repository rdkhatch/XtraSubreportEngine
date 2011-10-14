using System;

namespace XtraSubReport.Tests
{
    public static partial class AssertHelper
    {
        public static class Exceptions
        {
            public static void ShouldThrowException(Action action)
            {
                ShouldThrowException<Exception>(action);
            }

            public static void ShouldThrowException<TException>(Action action) where TException : Exception
            {
                bool wasThrown = false;

                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    if (typeof(TException).IsInstanceOfType(ex))
                        wasThrown = true;
                }

                if (wasThrown == false)
                    throw new Exception(string.Format("Expected exception of type {0} was not thrown.", typeof(TException).Name));
            }

        }

    }
}
