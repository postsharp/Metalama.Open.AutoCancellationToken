using System;
using Caravela.Framework.Aspects;

namespace Caravela.Open.AutoCancellationToken
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class AutoCancellationTokenAttribute : Attribute, IAspect { }
}
