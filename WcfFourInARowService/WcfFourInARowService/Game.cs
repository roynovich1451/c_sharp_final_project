//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WcfFourInARowService
{
    using System;
    using System.Collections.Generic;
    
    public partial class Game
    {
        public int GameId { get; set; }
        public System.DateTime Date { get; set; }
        public string Winner { get; set; }
        public int WinnerPoint { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
    
        public virtual User p1AsUser { get; set; }
        public virtual User p2AsUser { get; set; }
    }
}
