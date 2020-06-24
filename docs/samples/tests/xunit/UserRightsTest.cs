using Bunit.TestDoubles.Authorization;
using Xunit;

namespace Bunit.Docs.Samples
{
  public class UserRightsTest
  {
    [Fact(DisplayName = "No roles or policies")]
    public void Test001()
    {
      // Arrange
      using var ctx = new TestContext();
      var authContext = ctx.Services.AddTestAuthorization();
      authContext.SetAuthorized("TEST USER");

      // Act
      var cut = ctx.RenderComponent<UserRights>();

      // Assert
      cut.MarkupMatches(@"<h1>Hi TEST USER, you have these rights:</h1>
                          <ul></ul>");
    }

    [Fact(DisplayName = "Superuser role")]
    public void Test002()
    {
      // Arrange
      using var ctx = new TestContext();
      var authContext = ctx.Services.AddTestAuthorization();
      authContext.SetAuthorized("TEST USER");
      authContext.SetRoles("superuser");

      // Act
      var cut = ctx.RenderComponent<UserRights>();

      // Assert
      cut.MarkupMatches(@"<h1>Hi TEST USER, you have these rights:</h1>
                          <ul>
                            <li>You have the role SUPER USER</li>
                          </ul>");
    }
    
    [Fact(DisplayName = "Superuser and admin role")]
    public void Test003()
    {
      // Arrange
      using var ctx = new TestContext();
      var authContext = ctx.Services.AddTestAuthorization();
      authContext.SetAuthorized("TEST USER");
      authContext.SetRoles("admin", "superuser");

      // Act
      var cut = ctx.RenderComponent<UserRights>();

      // Assert
      cut.MarkupMatches(@"<h1>Hi TEST USER, you have these rights:</h1>
                          <ul>
                            <li>You have the role SUPER USER</li>
                            <li>You have the role ADMIN</li>
                          </ul>");
    }

    [Fact(DisplayName = "content-editor policy")]
    public void Test004()
    {
      // Arrange
      using var ctx = new TestContext();
      var authContext = ctx.Services.AddTestAuthorization();
      authContext.SetAuthorized("TEST USER");
      authContext.SetPolicies("content-editor");

      // Act
      var cut = ctx.RenderComponent<UserRights>();

      // Assert
      cut.MarkupMatches(@"<h1>Hi TEST USER, you have these rights:</h1>
                          <ul>
                            <li>You are a CONTENT EDITOR</li>
                          </ul>");
    }

    [Fact(DisplayName = "multiple content-editor policy")]
    public void Test0041()
    {
      // Arrange
      using var ctx = new TestContext();
      var authContext = ctx.Services.AddTestAuthorization();
      authContext.SetAuthorized("TEST USER");
      authContext.SetPolicies("content-editor", "approver");

      // Act
      var cut = ctx.RenderComponent<UserRights>();

      // Assert
      cut.MarkupMatches(@"<h1>Hi TEST USER, you have these rights:</h1>
                          <ul>
                            <li>You are a CONTENT EDITOR</li>
                          </ul>");
    }

    [Fact(DisplayName = "All roles and policies")]
    public void Test005()
    {
      // Arrange
      using var ctx = new TestContext();
      var authContext = ctx.Services.AddTestAuthorization();
      authContext.SetAuthorized("TEST USER");
      authContext.SetRoles("admin", "superuser");
      authContext.SetPolicies("content-editor");

      // Act
      var cut = ctx.RenderComponent<UserRights>();

      // Assert
      cut.MarkupMatches(@"<h1>Hi TEST USER, you have these rights:</h1>
                          <ul>
                            <li>You have the role SUPER USER</li>
                            <li>You have the role ADMIN</li>
                            <li>You are a CONTENT EDITOR</li>
                          </ul>");
    }
  }
}
