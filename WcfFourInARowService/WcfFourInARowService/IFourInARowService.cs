using System.Collections.Generic;
using System.ServiceModel;

namespace WcfFourInARowService
{
    [ServiceContract(CallbackContract = typeof(IFourInARowCallback))]
    public interface IFourInARowService
    {
        [OperationContract]
        [FaultContract(typeof(UserNameInUse))]
        void Register(string userName, string hashedPassword); //new client sign-up

        [OperationContract]
        [FaultContract(typeof(UserConnectedFault))]
        [FaultContract(typeof(UserNotRegisteredFault))]
        [FaultContract(typeof(IncorrectPasswordFault))]
        void ClientConnect(string userName, string hashedPassword); //old client sign-in

        [OperationContract]
        [FaultContract(typeof(OpponentDisconnectedFault))]
        MoveResult ReportMove(int gameId, int col, int player);
        [OperationContract]
        void StartNewGame(string player1, string player2);
        [OperationContract]
        [FaultContract(typeof(OpponentDisconnectedFault))]
        bool ChallengeRival(string rival, string Challenger);
        [OperationContract]
        void Disconnect(string userName,int gameID);
        [OperationContract]
        Dictionary<string, IFourInARowCallback> GetConnectedClients(string myUser);

    }

    public interface IFourInARowCallback
    {
        [OperationContract]
        bool SendGameInvitation(string rival, string Challenger); //anounce player other player Challenged him
        [OperationContract(IsOneWay = true)]
        void StartGameAgainstRival(string Challenger);

        [OperationContract(IsOneWay = true)]
        void OtherPlayerDisconnected(string user);
        [OperationContract(IsOneWay = true)]
        void RivalStartGame(string p1, string p2);
        [OperationContract(IsOneWay = true)]
        void OtherPlayerMoved(MoveResult moveResult, int col); //update board according rival movment
        [OperationContract(IsOneWay = true)]
        void NewPlayerConnected(string user);
        [OperationContract(IsOneWay = true)]
        void NotifyNewGameId(int gameId);
    }
}
