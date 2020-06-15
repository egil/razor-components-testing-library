using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Xunit;

namespace Bunit.TestDoubles.Authorization
{
	public class FakeAuthorizationServiceTest
	{
		[Fact(DisplayName = "Get default authorization result from AuthorizationService.")]
		public void Test001()
		{
			// arrange
			var service = new FakeAuthorizationService();

			// act
			var result = service.NextResult;

			// assert
			Assert.NotNull(result);
			Assert.True(result.Succeeded);
		}

		[Fact(DisplayName = "Get AuthorizeAsync with an authorized result.")]
		public async Task Test002()
		{
			// arrange
			var service = new FakeAuthorizationService
			{
				NextResult = AuthorizationResult.Failed()
			};

			var user = new ClaimsPrincipal(new FakeIdentity { Name = "DarthPedro" });
			var requirements = new List<IAuthorizationRequirement>();

			// act
			var result = await service.AuthorizeAsync(user, "testResource", requirements).ConfigureAwait(false);

			// assert
			Assert.NotNull(result);
			Assert.False(result.Succeeded);
		}

		[Fact(DisplayName = "Get AuthorizeAsync with an authorized result.")]
		public async Task Test003()
		{
			// arrange
			var service = new FakeAuthorizationService();
			var user = new ClaimsPrincipal(new FakeIdentity { Name = "DarthPedro" });
			var requirements = new List<IAuthorizationRequirement>();

			// act
			var result = await service.AuthorizeAsync(user, "testResource", requirements).ConfigureAwait(false);

			// assert
			Assert.NotNull(result);
			Assert.True(result.Succeeded);
		}

		[Fact(DisplayName = "Get AuthorizeAsync with policy name.")]
		public async Task Test004()
		{
			// arrange
			var service = new FakeAuthorizationService
			{
				NextResult = AuthorizationResult.Failed()
			};

			var user = new ClaimsPrincipal(new FakeIdentity { Name = "DarthPedro" });

			// act
			var result = await service.AuthorizeAsync(user, "testResource", "testPolicy").ConfigureAwait(false);

			// assert
			Assert.NotNull(result);
			Assert.False(result.Succeeded);
		}
	}
}