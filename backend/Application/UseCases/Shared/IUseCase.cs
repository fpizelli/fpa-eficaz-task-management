namespace EficazAPI.Application.UseCases.Shared
{
    public interface IUseCase<in TRequest, TResponse>
    {
        Task<TResponse> ExecuteAsync(TRequest request);
    }

    public interface IUseCase<TResponse>
    {
        Task<TResponse> ExecuteAsync();
    }

    public interface IVoidUseCase<in TRequest>
    {
        Task ExecuteAsync(TRequest request);
    }
}
