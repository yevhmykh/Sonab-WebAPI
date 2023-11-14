namespace Sonab.Core.Errors;

// TODO: Add stacktrace
public abstract class ErrorBase
{
    public Problem[] Problems { get; }

    protected ErrorBase(params Problem[] problems)
    {
        Problems = problems;
    }

    public abstract record Problem;
}
