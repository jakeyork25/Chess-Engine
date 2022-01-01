using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessEngine.Services
{
    public class VisualServices
    {
        public string GenerateAiQuote()
        {
            Random rnd = new Random();
            var quoteList = new List<string>();
            quoteList.Add("Hmm, interesting...");
            quoteList.Add("I think I know what you're doing.");
            quoteList.Add("What should I do now?");
            quoteList.Add("Nice move!");
            quoteList.Add("You sure know what you're doing!");
            quoteList.Add("Looks like I'm up against a pro!");
            for(var i = 0; i < 4; i++)
            {
                quoteList.Add("");
            }
            return quoteList.ElementAt(rnd.Next(quoteList.Count));
        }

        public string ShortenPosition(string position)
        {
            var shortPosition = position.Trim('-');
            return shortPosition;
        }
        public string ShortenPiece(string piece)
        {
            var shortPiece = "";
            if (piece.Contains("knight")) shortPiece = "N";
            else if (piece.Contains("bishop")) shortPiece = "B";
            else if (piece.Contains("rook")) shortPiece = "R";
            else if (piece.Contains("queen")) shortPiece = "Q";
            else if (piece.Contains("king")) shortPiece = "K";
            return shortPiece;
        }
    }
}
