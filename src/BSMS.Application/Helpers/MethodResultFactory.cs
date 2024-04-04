using Microsoft.Extensions.Logging;

namespace BSMS.Application.Helpers;

public class MethodResultFactory
{
    private readonly ILogger<MethodResultFactory> _logger;

    public MethodResultFactory(ILogger<MethodResultFactory> logger)
    {
        _logger = logger;
    }

    public MethodResult<T> Create<T>()
    {
        return new MethodResult<T>(_logger);
    }
}
