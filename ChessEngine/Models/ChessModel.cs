using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessEngine.Models
{

    public class OpponentMoveModel
    {
        public List<string> totalSquareList { get; set; }
        public List<string> opponentSquareList { get; set; }
        public List<string> playerSquareList { get; set; }
        public List<string> opponentPieceList { get; set; }
        public List<string> playerPieceList { get; set; }
    }

    public class OptionModel
    {
        public List<string> opponentSquareList { get; set; }
        public List<string> playerSquareList { get; set; }
        public List<string> opponentPieceList { get; set; }
        public List<string> playerPieceList { get; set; }
        public string startPosition { get; set; }
        public string pieceClass { get; set; }
        public string endPosition { get; set; }
    }

    public class ShortModel
    {
        public string position { get; set; }
        public string piece { get; set; }
    }
}
