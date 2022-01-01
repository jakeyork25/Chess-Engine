using javax.jws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ChessEngine.Services
{
    public class ChessServices
    { 
        public class Options
        {
            public List<List<string>> GetAllPlayerPositions(string startPos, string piece, List<string> opponentLocationList, List<string> playerLocationList)
            {
                ChessServices services = new ChessServices();
                var positionList = new List<string>();
                var avaList = new List<string>();
                var letterList = services.GetLetterList();
                var letter = startPos.Split('-')[0];
                var letterIndex = letterList.IndexOf(letter);
                var number = Convert.ToInt32(startPos.Split('-')[1]);
                switch (piece)
                {
                    case "pawn": positionList = services.GetPawnPositions(startPos, playerLocationList, opponentLocationList); break;
                    case "rook": positionList = services.GetRookPositions(startPos, playerLocationList, opponentLocationList); break;
                    case "bishop": positionList = services.GetBishopPositions(startPos, playerLocationList, opponentLocationList); break;
                    case "queen": positionList = services.GetQueenPositions(startPos, playerLocationList, opponentLocationList); break;
                    case "knight": positionList = services.GetKnightPositions(startPos, playerLocationList); break;
                    case "king": positionList = services.GetKingPositions(startPos, playerLocationList); break;
                }
                var finalList = new List<List<string>>();
                var emptyList = new List<string>();
                var opponentList = new List<string>();
                foreach(var pos in positionList)
                {
                    if (!opponentLocationList.Contains(pos)) emptyList.Add(pos);
                    else opponentList.Add(pos);
                }
                finalList.Add(emptyList);
                finalList.Add(opponentList);
                return finalList;
            }
        }

        public List<string> UpdatePlayerLocationList(string originalPos, string newPos, List<string> playerLocationList)
        {
            var newList = new List<string>();
            foreach(var location in playerLocationList)
            {
                newList.Add(location);
            }
            var positionIndex = newList.IndexOf(originalPos);
            newList.Remove(originalPos);
            newList.Insert(positionIndex, newPos);
            return newList;
        }

        public List<string> UpdateOpponentPieceList(string newPos, List<string> opponentLocationList, List<string> opponentPieceList)
        {
            if (!opponentLocationList.Contains(newPos)) return opponentPieceList;
            var positionIndex = opponentLocationList.IndexOf(newPos);
            var removedPiece = opponentPieceList.ElementAt(positionIndex);
            opponentPieceList.Remove(removedPiece);
            return opponentPieceList;
        }

        public List<string> UpdateOpponentLocationList(string newPos, List<string> opponentLocationList)
        {
            if (!opponentLocationList.Contains(newPos)) return opponentLocationList;
            opponentLocationList.Remove(newPos);
            return opponentLocationList;
        }

        public bool CheckMethod(List<string> playerPieceList, List<string> playerLocationList, List<string> aiPieceList, List<string> aiLocationList)
        {
            Criteria criteria = new Criteria();
            AiServices aiServices = new AiServices();
            var pointList = new List<int>();
            foreach (var piece in aiPieceList)
            {
                var tempPositionList = new List<string>();
                var startPosIndex = aiPieceList.IndexOf(piece);
                var startPos = aiLocationList.ElementAt(startPosIndex);

                if (piece.Contains("pawn")) tempPositionList.AddRange(aiServices.GetAllPositionsForPawn(startPos, playerLocationList, aiLocationList, 1));
                else if (piece.Contains("rook")) tempPositionList.AddRange(aiServices.GetAllPositionsForRook(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("bishop")) tempPositionList.AddRange(aiServices.GetAllPositionsForBishop(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("queen")) tempPositionList.AddRange(aiServices.GetAllPositionsForQueen(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("knight")) tempPositionList.AddRange(aiServices.GetAllPositionsForKnight(startPos, aiLocationList));
                else if (piece.Contains("king")) tempPositionList.AddRange(aiServices.GetAllPositionsForKing(startPos, playerLocationList, aiLocationList));

                foreach (var position in tempPositionList)
                {
                    if (position.Equals("none")) continue;

                    var points = criteria.PlayerTakedown(position, playerLocationList, playerPieceList);
                    pointList.Add(points);
                }
            }
            var maxValue = 0;
            foreach (var points in pointList)
            {
                maxValue = Math.Max(maxValue, points);
            }
            var check = false;
            if (maxValue >= 100) check = true;
            return check;
        }

        public bool StalemateMethod(List<string> playerPieceList, List<string> playerLocationList, List<string> aiPieceList, List<string> aiLocationList)
        {
            Criteria criteria = new Criteria();
            AiServices aiServices = new AiServices();
            var pointList = new List<int>();
            foreach (var piece in aiPieceList)
            {
                var tempPositionList = new List<string>();
                var startPosIndex = aiPieceList.IndexOf(piece);
                var startPos = aiLocationList.ElementAt(startPosIndex);

                if (piece.Contains("pawn")) tempPositionList.AddRange(aiServices.GetAllPositionsForPawn(startPos, playerLocationList, aiLocationList, 1));
                else if (piece.Contains("rook")) tempPositionList.AddRange(aiServices.GetAllPositionsForRook(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("bishop")) tempPositionList.AddRange(aiServices.GetAllPositionsForBishop(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("queen")) tempPositionList.AddRange(aiServices.GetAllPositionsForQueen(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("knight")) tempPositionList.AddRange(aiServices.GetAllPositionsForKnight(startPos, aiLocationList));
                else if (piece.Contains("king")) tempPositionList.AddRange(aiServices.GetAllPositionsForKing(startPos, playerLocationList, aiLocationList));

                foreach (var position in tempPositionList)
                {
                    if (position.Equals("none")) continue;

                    var points = criteria.PlayerTakedown(position, playerLocationList, playerPieceList);
                    pointList.Add(points);
                }
            }
            var maxValue = 0;
            foreach (var points in pointList)
            {
                maxValue = Math.Max(maxValue, points);
            }
            var noDanger = true;
            if (maxValue >= 100) noDanger = false;
            if(noDanger)
            {
                var tempPosList = new List<string>();
                foreach(var piece in playerPieceList)
                {
                    if (piece.Contains("king"))
                    {
                        var index = playerPieceList.IndexOf(piece);
                        var startingPos = playerLocationList.ElementAt(index);
                        tempPosList.AddRange(aiServices.GetAllPositionsForKing(startingPos, aiLocationList, playerLocationList));

                        var noneCount = 0;
                        foreach (var position in tempPosList)
                        {
                            if (position.Equals("none"))
                            {
                                noneCount += 1;
                                continue;
                            }
                            var points = criteria.PlayerSafety(position, piece, aiLocationList, aiPieceList, playerLocationList);
                            if (points > -100) return false;
                        }
                        if (noneCount == 8) return false;
                    }

                }
            } else
            {
                return noDanger;
            }
            return true;
        }

        public bool CheckmateMethod(List<string> playerPieceList, List<string> playerLocationList, List<string> aiPieceList, List<string> aiLocationList)
        {
            Criteria criteria = new Criteria();
            AiServices aiServices = new AiServices();
            foreach (var piece in playerPieceList)
            {
                var tempPositionList = new List<string>();
                var startPosIndex = playerPieceList.IndexOf(piece);
                var startPos = playerLocationList.ElementAt(startPosIndex);

                if (piece.Contains("pawn")) tempPositionList.AddRange(aiServices.GetAllPositionsForPlayerPawn(startPos, aiLocationList, playerLocationList, -1));
                else if (piece.Contains("rook")) tempPositionList.AddRange(aiServices.GetAllPositionsForRook(startPos, aiLocationList, playerLocationList));
                else if (piece.Contains("bishop")) tempPositionList.AddRange(aiServices.GetAllPositionsForBishop(startPos, aiLocationList, playerLocationList));
                else if (piece.Contains("queen")) tempPositionList.AddRange(aiServices.GetAllPositionsForQueen(startPos, aiLocationList, playerLocationList));
                else if (piece.Contains("knight")) tempPositionList.AddRange(aiServices.GetAllPositionsForKnight(startPos, playerLocationList));
                else if (piece.Contains("king")) tempPositionList.AddRange(aiServices.GetAllPositionsForKing(startPos, aiLocationList, playerLocationList));

                foreach (var position in tempPositionList)
                {
                    if (position.Equals("none")) continue;

                    var points = criteria.PlayerSafety(position, piece, aiLocationList, aiPieceList, playerLocationList);
                    if (points > -100) return false;
                }
            }
            return true;
        }

        public bool CanPawnChange(string pieceClass, string endPosition)
        {
            if (!pieceClass.Contains("pawn")) return false;
            if (!endPosition.Contains("a")) return false;
            return true;
        }
        public List<string> GetAvailableSquares(string startSquare, string piece, List<string> opponentLocationList, List<string> playerLocationList)
        {
            var positionList = new List<string>();
            switch(piece)
            {
                case "pawn": positionList = GetPawnPositions(startSquare, playerLocationList, opponentLocationList); break;
                case "rook": positionList = GetRookPositions(startSquare, playerLocationList, opponentLocationList); break;
                case "bishop": positionList = GetBishopPositions(startSquare, playerLocationList, opponentLocationList); break;
                case "knight": positionList = GetKnightPositions(startSquare, playerLocationList); break;
                case "queen": positionList = GetQueenPositions(startSquare, playerLocationList, opponentLocationList); break;
                case "king": positionList = GetKingPositions(startSquare, playerLocationList); break;
            }
            return positionList;
        }

        public List<string> GetPawnPositions(string startPositon, List<string> playerLocationList, List<string> opponentLocationList)
        {
            var list = new List<string>();
            var letterList = GetLetterList();
            var letter = startPositon.Split('-')[0];
            if (letter.Equals("a")) return list;
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPositon.Split('-')[1]);
            var newLetter = letterList.ElementAt(letterIndex - 1);
            var posLeft = newLetter + "-" + (number - 1).ToString();
            var posRight = newLetter + "-" + (number + 1).ToString();
            var posCenter = newLetter + "-" + number.ToString();
            if (letter.Equals("g"))
            {
                var doubleLetter = letterList.ElementAt(letterIndex - 2);
                var posDouble = doubleLetter + "-" + number.ToString();
                if (!opponentLocationList.Contains(posDouble) && !playerLocationList.Contains(posDouble)) list.Add(posDouble);
            }
            if (opponentLocationList.Contains(posLeft)) list.Add(posLeft);
            if (opponentLocationList.Contains(posRight)) list.Add(posRight);
            if (!opponentLocationList.Contains(posCenter) && !playerLocationList.Contains(posCenter)) list.Add(posCenter);            
            return list;
        }
        public List<string> GetKnightPositions(string startPosition, List<string> playerLocationList)
        {
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.Add(getLegitimatePosition.Knight(playerLocationList, letterList, letterIndex, number, -2, -1));
            positionList.Add(getLegitimatePosition.Knight(playerLocationList, letterList, letterIndex, number, -2, 1));
            positionList.Add(getLegitimatePosition.Knight(playerLocationList, letterList, letterIndex, number, -1, 2));
            positionList.Add(getLegitimatePosition.Knight(playerLocationList, letterList, letterIndex, number, 1, 2));
            positionList.Add(getLegitimatePosition.Knight(playerLocationList, letterList, letterIndex, number, 2, 1));
            positionList.Add(getLegitimatePosition.Knight(playerLocationList, letterList, letterIndex, number, 2, -1));
            positionList.Add(getLegitimatePosition.Knight(playerLocationList, letterList, letterIndex, number, 1, -2));
            positionList.Add(getLegitimatePosition.Knight(playerLocationList, letterList, letterIndex, number, -1, -2));
            return positionList;
        }
        public List<string> GetKingPositions(string startPosition, List<string> playerLocationList)
        {
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.Add(getLegitimatePosition.King(playerLocationList, letterList, letterIndex, number, -1, 0));
            positionList.Add(getLegitimatePosition.King(playerLocationList, letterList, letterIndex, number, -1, 1));
            positionList.Add(getLegitimatePosition.King(playerLocationList, letterList, letterIndex, number, 0, 1));
            positionList.Add(getLegitimatePosition.King(playerLocationList, letterList, letterIndex, number, 1, 1));
            positionList.Add(getLegitimatePosition.King(playerLocationList, letterList, letterIndex, number, 1, 0));
            positionList.Add(getLegitimatePosition.King(playerLocationList, letterList, letterIndex, number, 1, -1));
            positionList.Add(getLegitimatePosition.King(playerLocationList, letterList, letterIndex, number, 0, -1));
            positionList.Add(getLegitimatePosition.King(playerLocationList, letterList, letterIndex, number, -1, -1));
            return positionList;
        }
        public List<string> GetRookPositions(string startPosition, List<string> playerLocationList, List<string> opponentLocationList)
        {
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, 0, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, -1, 0));
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, 0, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, 1, 0));
            return positionList;
        }
        public List<string> GetBishopPositions(string startPosition, List<string> playerLocationList, List<string> opponentLocationList)
        {
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, -1, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, 1, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, 1, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, -1, -1));
            return positionList;
        }
        public List<string> GetQueenPositions(string startPosition, List<string> playerLocationList, List<string> opponentLocationList)
        {
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, -1, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, 1, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, 1, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, -1, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, 0, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, 1, 0));
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, -1, 0));
            positionList.AddRange(getLegitimatePosition.MultiPath(playerLocationList, opponentLocationList, letterList, letterIndex, number, 0, -1));
            return positionList;
        }


        public bool CheckAvailability(List<string> posList, string startPos, string endPos, List<string> playerLocationList, List<string> playerPieceList, List<string> aiLocationList, List<string> aiPieceList)
        {
            var tempPositionList = UpdatePlayerLocationList(startPos, endPos, playerLocationList);
            var tempOppPositionList = UpdateOpponentLocationList(endPos, aiLocationList);
            var tempOppPieceList = UpdateOpponentPieceList(endPos, aiLocationList, aiPieceList);
            var check = CheckMethod(playerPieceList, tempPositionList, tempOppPositionList, tempOppPieceList);
            if (check == true) return false;
            if (posList.Contains(endPos))
            {
                return true;
            }
            return false;
        }
        public List<string> AddPositions(List<string> list, params string[] positions)
        {
            foreach(var pos in positions)
            {
                list.Add(pos);
            }
            return list;
        }
        public List<string> GetLetterList()
        {
            var list = new List<string>();
            list = AddPositions(list, "a", "b", "c", "d", "e", "f", "g", "h");
            return list;
        }

        public class GetLegitimatePositon
        {
            public List<string> MultiPath(List<string> opponentLocationList, List<string> playerLocationList, List<string> letterList, int letterIndex, int number, int letterDir, int numberDir)
            {
                var positionList = new List<string>();
                var i = number;
                var j = letterIndex;
                while (1 <= i && i <= 8 && 0 <= j && j <= 7)
                {
                    i += numberDir;
                    if (i < 1 || i > 8) break;
                    j += letterDir;
                    if (j < 0 || j > 7) break;
                    var position = letterList.ElementAt(j) + "-" + i.ToString();
                    if (!opponentLocationList.Contains(position)) positionList.Add(position);
                    else break;
                    if (playerLocationList.Contains(position)) break;
                }
                return positionList;
            }
            public string King(List<string> playerLocationList, List<string> letterList, int letterIndex, int number, int letterRest, int numberRest)
            {
                var newLetterIndex = letterIndex + letterRest;
                var newNumber = number + numberRest;
                if (newLetterIndex > 7 || newLetterIndex < 0) return "none";
                if (newNumber > 8 || newNumber < 1) return "none";
                var position = letterList.ElementAt(newLetterIndex) + "-" + newNumber.ToString();
                if (playerLocationList.Contains(position)) return "none";
                return position;
            }
            public string Knight(List<string> playerLocationList, List<string> letterList, int letterIndex, int number, int letterRest, int numberRest)
            {
                var newLetterIndex = letterIndex + letterRest;
                var newNumber = number + numberRest;
                if (newLetterIndex > 7 || newLetterIndex < 0) return "none";
                if (newNumber > 8 || newNumber < 1) return "none";
                var position = letterList.ElementAt(letterIndex + letterRest) + "-" + (number + numberRest).ToString();
                if (playerLocationList.Contains(position)) return "none";
                return position;
            }
        }
    }
}
