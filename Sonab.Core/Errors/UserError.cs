namespace Sonab.Core.Errors;

public sealed class UserError : ErrorBase
{
    public static UserError Unauthorized() => new(new UnauthorizedProblem());
    public static UserError NotFound() => new(new NotFoundProblem());
    public static UserError AlreadySubscribed() => new(new AlreadySubscribedProblem());
    public static UserError NotSubscribed() => new(new NotSubscribedProblem());

    private UserError(params Problem[] problems) : base(problems)
    {
    }

    // TODO: Move out from class 
    public record UnauthorizedProblem : Problem;
    public record NotFoundProblem : Problem;
    public record AlreadySubscribedProblem : Problem;
    public record NotSubscribedProblem : Problem;
}
