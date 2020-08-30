using System;
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
        MoveResult ReportMove(int gameId, int col, int player, bool middleOfGame);
        [OperationContract]
        void StartNewGame(string challanger, string rival);
        [OperationContract]
        [FaultContract(typeof(OpponentDisconnectedFault))]
        int ChallengeRival(string rival, string Challenger);
        [OperationContract]
        void Disconnect(string userName,int gameID);
        [OperationContract]
        void NoticeAll(string player, bool connected);
        [OperationContract]
        void NoticeAllGameStarted(string challanger, string rival, bool connected);
        [OperationContract]
        Dictionary<string, IFourInARowCallback> GetConnectedClients(string myUser);
        [OperationContract]
        List<string> createSortedList(string by);
        [OperationContract]
        List<string> createRivaryData(string p1, string p2);
        [OperationContract]
        List<string> getGamesHistory();
        [OperationContract]
        List<string> getAllUserNames();
        [OperationContract]
        List<string> getLiveGames();
        [OperationContract]
        Dictionary<string, string> getUserStats(string user);
        [OperationContract]
        Dictionary<int, Tuple<string, int>> getTopThreeUsers();
    }

    public interface IFourInARowCallback
    {
        [OperationContract]
        bool SendGameInvitation(string rival, string Challenger); //anounce player other player Challenged him
        [OperationContract(IsOneWay = true)]
        void StartGameAgainstRival(string Challenger, string rival, int gameID);

        [OperationContract(IsOneWay = true)]
        void OtherPlayerDisOConnnectd(string user, bool connect);
        [OperationContract(IsOneWay = true)]
        void RivalStartGame(string p1, string p2);
        [OperationContract(IsOneWay = true)]
        void OtherPlayerMoved(MoveResult moveResult, int col); //update board according rival movment
        [OperationContract(IsOneWay = true)]
        void NewPlayerConnected(string user);
    }
}
