using System.Runtime.Serialization;

namespace WcfFourInARowService
{
    [DataContract]
    public enum MoveResult
    {
        [EnumMember]
        YouWon,
        [EnumMember]
        Draw,
        [EnumMember]
        NotYourTurn,
        [EnumMember]
        GameOn,
        [EnumMember]
        InvalidMove
    }
}
