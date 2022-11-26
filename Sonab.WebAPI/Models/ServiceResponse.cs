namespace Sonab.WebAPI.Models;

public class ServiceResponse
{
    public static ServiceResponse CreateOk(string message = null) => new(200, message);
    public static ServiceResponse CreateOk(object data) => new(200, data);
    public static ServiceResponse CreateBadRequest(string message, params string[] fields) => new(400, message, fields);
    public static ServiceResponse CreateForbidden(string message, params string[] fields) => new(403, message, fields);
    public static ServiceResponse CreateNotFound(string message, params string[] fields) => new(404, message, fields);
    public static ServiceResponse CreateConflict(string message, params string[] fields) => new(409, message, fields);
    public static ServiceResponse CreateServerError(string message = "Unexpected server error. Please contact dev team") => new(500, message);

    public int StatusCode { get; set; }
    public ErrorMessages Messages { get; set; }
    public object Data { get; set; }

    public ServiceResponse(int statusCode, string message)
    {
        StatusCode = statusCode;
        if (!string.IsNullOrEmpty(message))
        {
            Messages = new ErrorMessages("Error", message);
        }
    }

    public ServiceResponse(int statusCode, string message, string[] fields)
    {
        StatusCode = statusCode;
        if (!string.IsNullOrEmpty(message))
        {
            Messages = fields.Length == 0 ?
                new ErrorMessages("Error", message) :
                new ErrorMessages(fields, message);
        }
    }

    public ServiceResponse(int statusCode, object data)
    {
        StatusCode = statusCode;
        Data = data;
    }
}
