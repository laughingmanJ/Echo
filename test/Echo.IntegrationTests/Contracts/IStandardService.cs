using System.ServiceModel;

namespace Echo.IntegrationTests.Contracts
{
    [ServiceContract]
    public interface IStandardService
    {
        [OperationContract]
        string GetStringValue();

        [OperationContract]
        string SetValues(int x, double y, string value);

        [OperationContract]
        void ThrowFault();

        [OperationContract(IsOneWay = true)]
        void SetValue(int x);

        [OperationContract]
        void SetValueLongRunning(int x);
    }
}
