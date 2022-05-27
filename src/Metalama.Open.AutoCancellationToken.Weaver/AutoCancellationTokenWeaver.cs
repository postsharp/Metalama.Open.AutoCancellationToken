// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using Metalama.Compiler;
using Metalama.Framework.Engine.AspectWeavers;
using Metalama.Framework.Engine.CodeModel;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;

namespace Metalama.Open.AutoCancellationToken.Weaver
{
    [MetalamaPlugIn]
    [AspectWeaver( typeof(AutoCancellationTokenAttribute) )]
    internal partial class AutoCancellationTokenWeaver : IAspectWeaver
    {
        public void Transform( AspectWeaverContext context )
        {
            var compilation = context.Compilation;
            var instancesNodes = context.AspectInstances.SelectMany( a => a.Key.DeclaringSyntaxReferences ).Select( x => x.GetSyntax() );

            RunRewriter( new AnnotateNodesRewriter( instancesNodes ) );
            RunRewriter( new AddCancellationTokenParameterRewriter( compilation.Compilation ) );
            RunRewriter( new AddCancellationTokenArgumentRewriter( compilation.Compilation ) );

            context.Compilation = compilation;

            void RunRewriter( CSharpSyntaxRewriter rewriter )
            {
                compilation = compilation.RewriteSyntaxTrees( rewriter );
            }
        }
    }
}