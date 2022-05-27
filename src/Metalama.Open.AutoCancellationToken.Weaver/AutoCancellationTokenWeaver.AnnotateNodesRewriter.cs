// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Metalama.Open.AutoCancellationToken.Weaver
{
    public partial class AutoCancellationTokenWeaver
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