using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Windows;

namespace WcfFourInARowService
{
    [ServiceContract(CallbackContract = typeof(IFourInARowCallback))]
    public interface IFourInARowService
    {
        [OperationContract]
        [FaultContract(typeof(UserNameInUse))]
        void Register(string userName, string hashedPassword); //new client sign-up

        [OperationContract]
        [FaultContract(typeof(UserConnectdFault))]
        [FaultContract(typeof(UserNotRegisteredFault))]
        [FaultContract(typeof(IncorectPasswordFault))]
        void ClientConnect(string userName, string hashedPassword); //old client sign-in

        [OperationContract]
        [FaultContract(typeof(OpponentDisconnectedFault))]
        MoveResult ReportMove(int gameId, int location, int player);
        [OperationContract]
        void StartNewGame(string player1, string player2);
        [OperationContract]
        [FaultContract(typeof(OpponentDisconnectedFault))]
        bool ChalangeRival(string rival, string chalanger);
        [OperationContract]
        void Disconnect(string userName);
        [OperationContract]
        Dictionary<string, IFourInARowCallback> GetConnectedClients(string myUser);

    }

    public interface IFourInARowCallback
    {
        [OperationContract]
        bool SendGameInvitation(string rival, string chalanger); //anounce player other player chalanged him
        [OperationContract(IsOneWay = true)]
        void StartGameAgainstRival(string chalanger);

        [OperationContract(IsOneWay = true)]
        void OtherPlayerDisconnected(string user);
        [OperationContract(IsOneWay = true)]
        void RivalStartGame(string p1, string p2);
        [OperationContract(IsOneWay = true)]
        void OtherPlayerMoved(MoveResult moveResult, int location); //update board according rival movment
        [OperationContract(IsOneWay = true)]
        void NewPlayerConnected(string user);
        [OperationContract(IsOneWay = true)]
        void NotifyNewGameId(int gameId);
    }
}
