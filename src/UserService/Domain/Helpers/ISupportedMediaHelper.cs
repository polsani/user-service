namespace UserService.Domain.Helpers
{
    public interface ISupportedMediaHelper
    {
        bool IsMediaSupported(string contentType);
    }
}