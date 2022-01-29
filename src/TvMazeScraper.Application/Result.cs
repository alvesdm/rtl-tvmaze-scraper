namespace TvMazeScraper.Application;

public class Result<T>
{
    public Result(T data)
    {
        Data = data;
    }

    public Result(Exception ex)
    {
        Error = ex;
    }

    public T Data { get; }
    public Exception Error { get; }

    public bool IsSuccess => Error == null;
}