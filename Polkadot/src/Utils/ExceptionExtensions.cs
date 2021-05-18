using System;
using System.Collections.Generic;

namespace Polkadot.Utils
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<T> OfType<T>(this Exception ex)
        {
            if (ex is T required)
            {
                yield return required;
            }

            if (ex is AggregateException aggregate)
            {
                foreach (var inner in aggregate.InnerExceptions)
                {
                    foreach (var ir in inner.OfType<T>())
                    {
                        yield return ir;
                    }
                }
            }
            else if(ex.InnerException != null)
            {
                foreach (var ir in ex.InnerException.OfType<T>())
                {
                    yield return ir;
                }
            }
            
            
        }
    }
}