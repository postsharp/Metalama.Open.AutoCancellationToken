// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using Metalama.Framework.Aspects;
using System;

namespace Metalama.Open.AutoCancellationToken;

[AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface )]
public class AutoCancellationTokenAttribute : TypeAspect { }