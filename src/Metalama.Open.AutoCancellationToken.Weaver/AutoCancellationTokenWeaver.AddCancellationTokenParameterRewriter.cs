// Copyright (c) SharpCrafters s.r.o. All rights reserved.
// This project is not open source. Please see the LICENSE.md file in the repository root for details.

using Metalama.Framework.Engine.Formatting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Metalama.Open.AutoCancellationToken.Weaver
{
    internal partial class AutoCancellationTokenWeaver
    {
        private sealed class AddCancellationTokenParameterRewriter : RewriterBase
        {
            private readonly Compilation _compilation;

            public AddCancellationTokenParameterRewriter( Compilation compilation )
            {
                this._compilation = compilation;
            }

            protected override T VisitTypeDeclaration<T>( T node, Func<T, SyntaxNode?> baseVisit )
            {
                if ( !node.HasAnnotation( AnnotateNodesRewriter.Annotation ) )
                {
                    return node;
                }

                return (T) baseVisit( node )!;
            }

            public override SyntaxNode VisitMethodDeclaration( MethodDeclarationSyntax node )
            {
                var semanticModel = this._compilation.GetSemanticModel( node.SyntaxTree );

                var methodSymbol = semanticModel.GetDeclaredSymbol( node );

                if ( methodSymbol == null || !methodSymbol.IsAsync || methodSymbol.Parameters.Any( IsCancellationToken ) )
                {
                    return node;
                }

                var parameters = node.ParameterList.Parameters.GetWithSeparators().ToList();

                if ( parameters.Count > 0 )
                {
                    // Remove the trivia after the last argument.
                    parameters[parameters.Count - 1] = parameters[parameters.Count - 1].AsNode()!.WithoutTrailingTrivia();
                    parameters.Add( SyntaxFactory.Token( SyntaxKind.CommaToken ).WithTrailingTrivia( SyntaxFactory.ElasticSpace ).WithAdditionalAnnotations( FormattingAnnotations.GeneratedCode ) );
                }

                parameters.Add(
                    SyntaxFactory.Parameter(
                            default,
                            default,
                            CancellationTokenType,
                            SyntaxFactory.Identifier( "cancellationToken" ),
                            SyntaxFactory.EqualsValueClause( SyntaxFactory.Token( SyntaxKind.EqualsToken ).WithTrailingTrivia( SyntaxFactory.ElasticSpace ), SyntaxFactory.LiteralExpression( SyntaxKind.DefaultLiteralExpression ) ).WithTrailingTrivia( SyntaxFactory.ElasticSpace ) )
                    .WithTrailingTrivia( SyntaxFactory.ElasticSpace )
                        .WithAdditionalAnnotations( FormattingAnnotations.GeneratedCode ) );

                node = node.WithParameterList( SyntaxFactory.ParameterList( SyntaxFactory.SeparatedList<ParameterSyntax>( new SyntaxNodeOrTokenList( parameters ) ) ) );

                return node;
            }
        }
    }
}