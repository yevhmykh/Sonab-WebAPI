using Sonab.Core.Errors;

namespace Sonab.Core.Interfaces;

public interface IPresenter<TResponse> where TResponse : ResponseDto
{
    // Let's try 2 methods instead of Result<TResponse>
    Task HandleSuccess(TResponse response);
    // TODO: Maybe errors should be typed, but it's not required now
    //Task HandleFailure(Dictionary<string, string[]> errors);
    Task HandleFailure(ErrorBase error);
}
