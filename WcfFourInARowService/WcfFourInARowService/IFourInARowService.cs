using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfFourInARowService
{
    [ServiceContract(CallbackContract = typeof(IFourInARowCallback))]
    public interface IFourInARowService
    {
        [OperationContract]
        int Register();

        [OperationContract]
        [FaultContract(typeof(OpponentDisconnectedFault))]
        MoveResult ReportMove(int location, int player);

        [OperationContract]
        void Disconnect(int player);

        [OperationContract]
        void DisconnectBeforeGame(int player);
    }

    public interface IFourInARowCallback
    {
        [OperationContract(IsOneWay = true)]
        void OtherPlayerConnected();

        [OperationContract(IsOneWay = true)]
        void OtherPlayerMoved(MoveResult moveResult, int location);
    }
}