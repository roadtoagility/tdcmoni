namespace MoniLogs.Core.Services
{
    public interface IService
    {
        void Execute();
    }

    public interface IFinisherService : IService
    {
    }

    public interface IJobServerService : IService
    {
    }

    public interface IValidationService : IService
    {
    }

    public interface IValidationEmpresaService : IService
    {
    }

    public interface IValidationPlacaService : IService
    {
    }
}