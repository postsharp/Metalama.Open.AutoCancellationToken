// Copyright (c) SharpCrafters s.r.o. All rights reserved.
// This project is not open source. Please see the LICENSE.md file in the repository root for details.

using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Metalama.Open.AutoCancellationToken.Weaver
{
    internal partial class AutoCancellationTokenWeaver
    {
        private sealed class AnnotateNodesRewriter : RewriterBase
        {
            private readonly HashSet<SyntaxNode> _instancesNodes;

            public AnnotateNodesRewriter( IEnumerable<SyntaxNode> instancesNodes )
            {
                this._instancesNodes = new HashSet<SyntaxNode>( instancesNodes );
            }

            public static SyntaxAnnotation Annotation { get; } = new();

            protected override T VisitTypeDeclaration<T>( T node, Func<T, SyntaxNode?> baseVisit )
            {
                if ( !this._instancesNodes.Contains( node ) )
                {
                    return node;
                }

                return node.WithAdditionalAnnotations( Annotation );
            }
        }
    }
}