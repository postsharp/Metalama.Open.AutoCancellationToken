// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Compiler;
using Metalama.Framework.Engine.AspectWeavers;
using Metalama.Framework.Engine.CodeModel;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;

namespace Metalama.Open.AutoCancellationToken.Weaver
{
    [MetalamaPlugIn]
    public partial class AutoCancellationTokenWeaver : IAspectWeaver
    {
        public void Transform( AspectWeaverContext context )
        {
            var compilation = context.Compilation;
            var instancesNodes = context.AspectInstances.SelectMany( a => a.Key.DeclaringSyntaxReferences ).Select( x => x.GetSyntax() );

            RunRewriter( new AnnotateNodesRewriter( instancesNodes ) );
            RunRewriter( new AddCancellationTokenParameterRewriter( compilation.Compilation, context.GeneratedCodeAnnotation ) );
            RunRewriter( new AddCancellationTokenArgumentRewriter( compilation.Compilation, context.GeneratedCodeAnnotation ) );

            context.Compilation = compilation;

            void RunRewriter( CSharpSyntaxRewriter rewriter )
            {
                compilation = compilation.RewriteSyntaxTrees( rewriter );
            }
        }
    }
}