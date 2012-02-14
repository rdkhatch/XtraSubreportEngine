using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace XtraSubreport.Engine.Support
{
    class MefHelper
    {
        public static void ThrowReflectionTypeLoadException(ReflectionTypeLoadException tLException)
        {
            var loaderMessages = new StringBuilder();
            loaderMessages.AppendLine("While trying to load composable parts the following loader exceptions were found: ");
            foreach (var loaderException in tLException.LoaderExceptions)
            {
                loaderMessages.AppendLine(loaderException.Message);
            }

            throw new Exception(loaderMessages.ToString(), tLException);  
        }
    }
}
