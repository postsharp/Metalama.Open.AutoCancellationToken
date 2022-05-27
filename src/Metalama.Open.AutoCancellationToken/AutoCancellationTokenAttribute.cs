// Copyright (c) SharpCrafters s.r.o. All rights reserved.
// This project is not open source. Please see the LICENSE.md file in the repository root for details.

using Metalama.Framework.Aspects;
using System;

namespace Metalama.Open.AutoCancellationToken;

[AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface )]
public class AutoCancellationTokenAttribute : TypeAspect { }