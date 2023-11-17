namespace Sonab.Core.Errors;

public sealed class PostError : ErrorBase
{
    public static PostError NotFound(int id) => new(new NotFoundProblem(id));
    public static PostError NotOwner() => new(new NotOwnerProblem());
    public static PostError NotFoundTopics(List<int> ids) => new(new NotFoundTopicsProblem(ids));

    public static PostError MinLenghtViolation(string fieldName, int expected) =>
        new(new MinLenghtViolationProblem(fieldName, expected));
    
    public static PostError MaxLenghtViolation(string fieldName, int expected) =>
        new(new MaxLenghtViolationProblem(fieldName, expected));

    private PostError(params Problem[] problems) : base(problems)
    {
    }

    // TODO: Move out from class 
    public record NotFoundProblem(int Id) : Problem;
    public record NotOwnerProblem : Problem;
    public record NotFoundTopicsProblem(List<int> Ids) : Problem;
    public record MinLenghtViolationProblem(string FieldName, int Expected) : Problem;
    public record MaxLenghtViolationProblem(string FieldName, int Expected) : Problem;
}
