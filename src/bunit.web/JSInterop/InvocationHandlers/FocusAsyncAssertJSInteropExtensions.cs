#if NET5_0
using Bunit.JSInterop.InvocationHandlers;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Bunit
{
	/// <summary>
	/// Extensions methods for verifying <see cref="ElementReferenceExtensions.FocusAsync(ElementReference)"/> method calls.
	/// </summary>
	public static class FocusAsyncAssertJSInteropExtensions
	{
		/// <summary>
		/// Verifies that the <see cref="ElementReferenceExtensions.FocusAsync(ElementReference)"/> method has been invoked one time.
		/// </summary>
		/// <param name="handler">Handler to verify against.</param>		
		/// <param name="userMessage">A custom user message to display if the assertion fails.</param>
		/// <returns>The <see cref="JSRuntimeInvocation"/>.</returns>
		public static JSRuntimeInvocation VerifyFocusAsyncInvoke(this BunitJSInterop handler, string? userMessage = null)
			=> handler.VerifyInvoke(FocusAsyncInvocationHandler.FocusIdentifier, userMessage);

		/// <summary>
		/// Verifies that the <see cref="ElementReferenceExtensions.FocusAsync(ElementReference)"/> method has been invoked <paramref name="calledTimes"/> times.
		/// </summary>
		/// <param name="handler">Handler to verify against.</param>
		/// <param name="calledTimes">The number of times the invocation is expected to have been called.</param>
		/// <param name="userMessage">A custom user message to display if the assertion fails.</param>
		/// <returns>The <see cref="JSRuntimeInvocation"/>.</returns>
		public static IReadOnlyList<JSRuntimeInvocation> VerifyFocusAsyncInvoke(this BunitJSInterop handler, int calledTimes, string? userMessage = null)
			=> handler.VerifyInvoke(FocusAsyncInvocationHandler.FocusIdentifier, calledTimes, userMessage);
	}
}
#endif
