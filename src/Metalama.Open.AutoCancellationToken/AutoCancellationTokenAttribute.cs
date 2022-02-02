using System;
using Metalama.Framework.Aspects;

namespace Metalama.Open.AutoCancellationToken;

[AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface )]
public class AutoCancellationTokenAttribute : TypeAspect { }