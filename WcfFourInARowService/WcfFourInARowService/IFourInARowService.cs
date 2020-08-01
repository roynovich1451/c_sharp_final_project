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
        [FaultContract(typeof(UserNameInUse))]
        [FaultContract(typeof(WeakPassword))]
        int Register(); //new client sign-up

        [OperationContract]
        [FaultContract(typeof(UserConnectdFault))]
        [FaultContract(typeof(UserNotRegisteredFault))]
        void ClientConnect(string userName, string hashedPassword); //client signed in

        [OperationContract]
        [FaultContract(typeof(OpponentDisconnectedFault))]
        MoveResult ReportMove(int location, int player);
        [OperationContract]
        void DisconnectDuringGame(string userName);
        [OperationContract]
        void Disconnect(string userName);
    }

    public interface IFourInARowCallback
    {
        [OperationContract(IsOneWay = true)]
        void sendGameInvitation(); //sent invitation for available client in connected list
        [OperationContract(IsOneWay = true)]
        void AcceptGameInvitation(); //accept other user invitation for game and start game
        [OperationContract(IsOneWay = true)]
        void OtherPlayerConnected();

        [OperationContract(IsOneWay = true)]
        void OtherPlayerMoved(MoveResult moveResult, int location); //update board according rival movment
    }
}
