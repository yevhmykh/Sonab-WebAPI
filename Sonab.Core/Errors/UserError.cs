namespace Sonab.Core.Errors;

public sealed class UserError : ErrorBase
{
    public static UserError Unauthorized() => new(new UnauthorizedProblem());
    public static UserError NotFound() => new(new NotFoundProblem());
    public static UserError NoInfo(string userId) => new(new NotInfoProblem(userId));
    public static UserError AlreadySubscribed() => new(new AlreadySubscribedProblem());
    public static UserError NotSubscribed() => new(new NotSubscribedProblem());

    private UserError(params Problem[] problems) : base(problems)
    {
    }

    public bool IsUnauthorized() => Problems.Any(problem => problem is UnauthorizedProblem);

    // TODO: Move out from class 
    public record UnauthorizedProblem : Problem;
    public record NotFoundProblem : Problem;
    public record NotInfoProblem(string UserId) : Problem;
    public record AlreadySubscribedProblem : Problem;
    public record NotSubscribedProblem : Problem;
}
