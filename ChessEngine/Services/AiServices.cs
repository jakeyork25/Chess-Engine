using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessEngine.Services
{
    public class AiServices
    {

        public string ChangePawnPiece(string pieceClass, string endPosition)
        {
            if (!pieceClass.Contains("pawn")) return "pawn";
            if (!endPosition.Contains("h")) return "pawn";
            Random rnd = new Random();
            var classList = new List<string>();
            classList.Add("queen");
            classList.Add("rook");
            classList.Add("bishop");
            classList.Add("knight");
            var randomPiece = classList.ElementAt(rnd.Next(classList.Count));
            return randomPiece;
        }

        public List<string> UpdateAiLocationList(string originalPos, string newPos, List<string> opponentLocationList)
        {
            var positionIndex = opponentLocationList.IndexOf(originalPos);
            opponentLocationList.Remove(originalPos);
            opponentLocationList.Insert(positionIndex, newPos);
            return opponentLocationList;
        }

        public List<string> UpdatePlayerPieces(string newPos, List<string> playerLocationList, List<string> playerPieceList)
        {
            if (!playerLocationList.Contains(newPos)) return playerPieceList;
            var positionIndex = playerLocationList.IndexOf(newPos);
            var removedPiece = playerPieceList.ElementAt(positionIndex);
            playerPieceList.Remove(removedPiece);
            return playerPieceList;
        }

        public List<string> UpdatePlayerLocations(string newPos, List<string> playerLocationList)
        {
            if (!playerLocationList.Contains(newPos)) return playerLocationList;
            playerLocationList.Remove(newPos);
            return playerLocationList;
        }
        public string GetNewPositionForPawn(string startPositon, List<string> playerLocationList, List<string> opponentLocationList)
        {
            var letterList = GetLetterList();
            var letter = startPositon.Split('-')[0];
            if (letter.Equals("h")) return "none";
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPositon.Split('-')[1]);
            var newLetter = letterList.ElementAt(letterIndex + 1);
            if(letter.Equals("b"))
            {
                var doubleLetter = letterList.ElementAt(letterIndex + 2);
                var posDouble = doubleLetter + "-" + number.ToString();
                if (!playerLocationList.Contains(posDouble) && !opponentLocationList.Contains(posDouble)) return posDouble;
            }
            var posLeft = newLetter + "-" + (number - 1).ToString();
            var posRight = newLetter + "-" + (number + 1).ToString();
            var posCenter = newLetter + "-" + number.ToString();
            if (playerLocationList.Contains(posLeft)) return posLeft;
            if (playerLocationList.Contains(posRight)) return posRight;
            if (!playerLocationList.Contains(posCenter) && !opponentLocationList.Contains(posCenter)) return posCenter;
            return "none";
        }
        public string GetNewPositionForKnight(string startPosition, List<string> playerLocationList, List<string> opponentLocationList, List<string> playerPieceList)
        {
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            Random rnd = new Random();
            Criteria criteria = new Criteria();
            var positionList = new List<string>();
            var legitPositionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, -2, -1));
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, -2, 1));
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, -1, 2));
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, 1, 2));
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, 2, 1));
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, 2, -1));
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, 1, -2));
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, -1, -2));
            var pointList = CreatePointList(positionList);
            pointList = criteria.PlayerEliminations(positionList, pointList, playerLocationList, playerPieceList);
            var maxPoint = 0;
            int finalIndex;
            foreach (var value in pointList)
            {
                maxPoint = Math.Max(maxPoint, value);
            }
            if (maxPoint > 0)
            {
                finalIndex = pointList.IndexOf(pointList.Max());
            }
            else
            {
                finalIndex = rnd.Next(0, pointList.Count);
            }
            var finalPos = positionList.ElementAt(finalIndex);
            return finalPos;
        }
        public string GetNewPositionForKing(string startPosition, List<string> playerLocationList, List<string> opponentLocationList, List<string> playerPieceList)
        {
            Random rnd = new Random();
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            Criteria criteria = new Criteria();
            var positionList = new List<string>();
            var legitPositionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, -1, 0));
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, -1, 1));
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, 0, 1));
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, 1, 1));
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, 1, 0));
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, 1, -1));
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, 0, -1));
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, -1, -1));
            var pointList = CreatePointList(positionList);
            pointList = criteria.PlayerEliminations(positionList, pointList, playerLocationList, playerPieceList);
            var maxPoint = 0;
            int finalIndex;
            foreach (var value in pointList)
            {
                maxPoint = Math.Max(maxPoint, value);
            }
            if (maxPoint > 0)
            {
                finalIndex = pointList.IndexOf(pointList.Max());
            }
            else
            {
                finalIndex = rnd.Next(0, pointList.Count);
            }
            var finalPos = positionList.ElementAt(finalIndex);
            return finalPos;
        }
        public string GetNewPositionForRook(string startPosition, List<string> playerLocationList, List<string> opponentLocationList, List<string> playerPieceList)
        {
            Random rnd = new Random();
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            Criteria criteria = new Criteria();
            if (playerLocationList == null) playerLocationList = new List<string>();
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 0, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, -1, 0));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 0, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 1, 0));
            if (positionList.Count == 0) return "none";
            var pointList = CreatePointList(positionList);
            pointList = criteria.PlayerEliminations(positionList, pointList, playerLocationList, playerPieceList);
            var maxPoint = 0;
            int finalIndex;
            foreach (var value in pointList)
            {
                maxPoint = Math.Max(maxPoint, value);
            }
            if (maxPoint > 0)
            {
                finalIndex = pointList.IndexOf(pointList.Max());
            }
            else
            {
                finalIndex = rnd.Next(0, pointList.Count);
            }
            var finalPos = positionList.ElementAt(finalIndex);
            return finalPos;
        }
        public string GetNewPositionForBishop(string startPosition, List<string> playerLocationList, List<string> opponentLocationList, List<string> playerPieceList)
        {
            Random rnd = new Random();
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            Criteria criteria = new Criteria();
            if (playerLocationList == null) playerLocationList = new List<string>();
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, -1, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 1, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 1, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, -1, -1));
            if (positionList.Count == 0) return "none";
            var pointList = CreatePointList(positionList);
            pointList = criteria.PlayerEliminations(positionList, pointList, playerLocationList, playerPieceList);
            var maxPoint = 0;
            int finalIndex;
            foreach (var value in pointList)
            {
                maxPoint = Math.Max(maxPoint, value);
            }
            if (maxPoint > 0)
            {
                finalIndex = pointList.IndexOf(pointList.Max());
            }
            else
            {
                finalIndex = rnd.Next(0, pointList.Count);
            }
            var finalPos = positionList.ElementAt(finalIndex);
            return finalPos;
        }
        public string GetNewPositionForQueen(string startPosition, List<string> playerLocationList, List<string> opponentLocationList, List<string> playerPieceList)
        {
            Random rnd = new Random();
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            Criteria criteria = new Criteria();
            if (playerLocationList == null) playerLocationList = new List<string>();
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, -1, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 1, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 1, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, -1, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 0, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 1, 0));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, -1, 0));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 0, -1));
            if (positionList.Count == 0) return "none";
            var pointList = CreatePointList(positionList);
            pointList = criteria.PlayerEliminations(positionList, pointList, playerLocationList, playerPieceList);
            pointList = criteria.PieceSafety(positionList, pointList, playerLocationList, playerPieceList);
            var maxPoint = 0;
            int finalIndex;
            foreach(var value in pointList)
            {
                maxPoint = Math.Max(maxPoint, value);
            }
            if (maxPoint > 0)
            {
                var maxIndexList = new List<int>();
                for(var i = 0; i < pointList.Count; i++)
                {
                    var value = pointList.ElementAt(i);
                    if (value == maxPoint) maxIndexList.Add(i);
                }
                var maxListIndex = rnd.Next(maxIndexList.Count);
                finalIndex = maxIndexList.ElementAt(maxListIndex);
            }
            else
            {
                finalIndex = rnd.Next(0, pointList.Count);
            }

            var finalPos = positionList.ElementAt(finalIndex);
            return finalPos;
        }

        public List<string> GetAllPositionsForPawn(string startPositon, List<string> playerLocationList, List<string> opponentLocationList, int direction)
        {
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPositon.Split('-')[0];
            if (letter.Equals("h")) return positionList;
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPositon.Split('-')[1]);
            var newLetter = letterList.ElementAt(letterIndex + direction);
            var posLeft = newLetter + "-" + (number - 1).ToString();
            var posRight = newLetter + "-" + (number + 1).ToString();
            var posCenter = newLetter + "-" + number.ToString();
            if(letter.Equals("b"))
            {
                var doubleLetter = letterList.ElementAt(letterIndex + (direction * 2));
                var posDouble = doubleLetter + "-" + number.ToString();
                if (!playerLocationList.Contains(posDouble) && !opponentLocationList.Contains(posDouble)) positionList.Add(posDouble);
            }
            if (playerLocationList.Contains(posLeft)) positionList.Add(posLeft);
            if (playerLocationList.Contains(posRight)) positionList.Add(posRight);
            if (!playerLocationList.Contains(posCenter) && !opponentLocationList.Contains(posCenter)) positionList.Add(posCenter);
            return positionList;
        }

        public List<string> GetAllPositionsForPlayerPawn(string startPositon, List<string> playerLocationList, List<string> opponentLocationList, int direction)
        {
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPositon.Split('-')[0];
            if (letter.Equals("a")) return positionList;
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPositon.Split('-')[1]);
            var newLetter = letterList.ElementAt(letterIndex + direction);
            var posLeft = newLetter + "-" + (number - 1).ToString();
            var posRight = newLetter + "-" + (number + 1).ToString();
            var posCenter = newLetter + "-" + number.ToString();
            if (letter.Equals("g"))
            {
                var doubleLetter = letterList.ElementAt(letterIndex + (direction * 2));
                var posDouble = doubleLetter + "-" + number.ToString();
                if (!playerLocationList.Contains(posDouble) && !opponentLocationList.Contains(posDouble)) positionList.Add(posDouble);
            }
            if (playerLocationList.Contains(posLeft)) positionList.Add(posLeft);
            if (playerLocationList.Contains(posRight)) positionList.Add(posRight);
            if (!playerLocationList.Contains(posCenter) && !opponentLocationList.Contains(posCenter)) positionList.Add(posCenter);
            return positionList;
        }
        public List<string> GetAllPositionsForKnight(string startPosition, List<string> opponentLocationList)
        {
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, -2, -1));
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, -2, 1));
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, -1, 2));
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, 1, 2));
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, 2, 1));
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, 2, -1));
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, 1, -2));
            positionList.Add(getLegitimatePosition.Knight(opponentLocationList, letterList, letterIndex, number, -1, -2));
            return positionList;
        }
        public List<string> GetAllPositionsForKing(string startPosition, List<string> playerLocationList, List<string> opponentLocationList)
        {
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, -1, 0));
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, -1, 1));
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, 0, 1));
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, 1, 1));
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, 1, 0));
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, 1, -1));
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, 0, -1));
            positionList.Add(getLegitimatePosition.King(opponentLocationList, letterList, letterIndex, number, -1, -1));
            return positionList;
        }
        public List<string> GetAllPositionsForRook(string startPosition, List<string> playerLocationList, List<string> opponentLocationList)
        {
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 0, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, -1, 0));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 0, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 1, 0));
            return positionList;
        }
        public List<string> GetAllPositionsForBishop(string startPosition, List<string> playerLocationList, List<string> opponentLocationList)
        {
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, -1, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 1, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 1, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, -1, -1));
            return positionList;
        }
        public List<string> GetAllPositionsForQueen(string startPosition, List<string> playerLocationList, List<string> opponentLocationList)
        {
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var positionList = new List<string>();
            var letterList = GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, -1, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 1, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 1, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, -1, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 0, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 1, 0));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, -1, 0));
            positionList.AddRange(getLegitimatePosition.MultiPath(opponentLocationList, playerLocationList, letterList, letterIndex, number, 0, -1));
            return positionList;
        }

        public bool CheckMethod(List<string> playerPieceList, List<string> playerLocationList, List<string> aiPieceList, List<string> aiLocationList)
        {
            Criteria criteria = new Criteria();
            var pointList = new List<int>();
            foreach (var piece in aiPieceList)
            {
                var tempPositionList = new List<string>();
                var startPosIndex = aiPieceList.IndexOf(piece);
                var startPos = aiLocationList.ElementAt(startPosIndex);

                if (piece.Contains("pawn")) tempPositionList.AddRange(GetAllPositionsForPlayerPawn(startPos, aiLocationList, playerLocationList, -1));
                else if (piece.Contains("rook")) tempPositionList.AddRange(GetAllPositionsForRook(startPos, aiLocationList, playerLocationList));
                else if (piece.Contains("bishop")) tempPositionList.AddRange(GetAllPositionsForBishop(startPos, aiLocationList, playerLocationList));
                else if (piece.Contains("queen")) tempPositionList.AddRange(GetAllPositionsForQueen(startPos, aiLocationList, playerLocationList));
                else if (piece.Contains("knight")) tempPositionList.AddRange(GetAllPositionsForKnight(startPos, playerLocationList));
                else if (piece.Contains("king")) tempPositionList.AddRange(GetAllPositionsForKing(startPos, aiLocationList, playerLocationList));

                foreach (var position in tempPositionList)
                {
                    if (position.Equals("none")) continue;

                    var points = criteria.AiLoss(position, aiLocationList, aiPieceList);
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

        public List<string> MaxMethod(List<string> pieceList, List<string> aiLocationList, List<string> playerLocationList, List<string> playerPieceList, int depth)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            depth -= 1;
            var alpha = -999;
            Criteria criteria = new Criteria();
            var totalPositionCount = 0;
            var positionList = new List<string>();
            var pointList = new List<int>();
            var startPosList = new List<string>();
            var minPointList = new List<int>();
            var correspondingPieceList = new List<string>();
            foreach (var piece in pieceList)
            {
                var tempPositionList = new List<string>();
                var startPosIndex = pieceList.IndexOf(piece);
                var startPos = aiLocationList.ElementAt(startPosIndex);
                if (piece.Contains("pawn")) tempPositionList.AddRange(GetAllPositionsForPawn(startPos, playerLocationList, aiLocationList, 1));
                else if (piece.Contains("rook")) tempPositionList.AddRange(GetAllPositionsForRook(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("bishop")) tempPositionList.AddRange(GetAllPositionsForBishop(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("queen")) tempPositionList.AddRange(GetAllPositionsForQueen(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("knight")) tempPositionList.AddRange(GetAllPositionsForKnight(startPos, aiLocationList));
                else if (piece.Contains("king")) tempPositionList.AddRange(GetAllPositionsForKing(startPos, playerLocationList, aiLocationList));
                
                foreach(var position in tempPositionList)
                {             
                    if (position.Equals("none")) continue;

                    var points = criteria.PlayerTakedown(position, playerLocationList, playerPieceList);
                    points += criteria.AiSafety(position, piece, playerLocationList, playerPieceList, aiLocationList);

                    if (points >= alpha)
                    {
                        alpha = points;
                        totalPositionCount += 1;

                        var pieceIndex = pieceList.IndexOf(piece);
                        var originalSquare = aiLocationList.ElementAt(pieceIndex);
                        var updatedAiLocationList = new List<string>();

                        foreach (var location in aiLocationList)
                        {
                            if (location.Equals(originalSquare)) updatedAiLocationList.Add(position);
                            else updatedAiLocationList.Add(location);
                        }
                        var updatedPlayerLocationList = new List<string>();
                        updatedPlayerLocationList.AddRange(playerLocationList);
                        var updatedPlayerPieceList = new List<string>();
                        updatedPlayerPieceList.AddRange(playerPieceList);
                        if (updatedPlayerLocationList.Contains(position))
                        {
                            var playerIndex = updatedPlayerLocationList.IndexOf(position);
                            updatedPlayerLocationList.RemoveAt(playerIndex);
                            updatedPlayerPieceList.RemoveAt(playerIndex);
                        }

                        var tempList = MinMethod(updatedPlayerPieceList, updatedPlayerLocationList, pieceList, updatedAiLocationList, depth, totalPositionCount);
                        var minPoints = tempList.ElementAt(0);
                        totalPositionCount = tempList.ElementAt(1);
                        points = points - minPoints;
                        pointList.Add(points);
                        minPointList.Add(minPoints);
                        positionList.Add(position);
                        startPosList.Add(startPos);

                        correspondingPieceList.Add(piece);
                    }
                }
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return GetBestList(pointList, positionList, correspondingPieceList, minPointList, elapsedMs, totalPositionCount, startPosList);
        }
        public List<int> MinMethod(List<string> playerPieceList, List<string> playerLocationList, List<string> aiPieceList, List<string> aiLocationList, int depth, int totalPositionCount)
        {
            var returnList = new List<int>();
            returnList.Add(0);
            returnList.Add(totalPositionCount);
            if (depth <= 0) return returnList;
            var beta = -999;
            depth -= 1;
            Criteria criteria = new Criteria();
            var positionList = new List<string>();
            var pointList = new List<int>();
            var correspondingPieceList = new List<string>();
            foreach (var piece in playerPieceList)
            {
                var tempPositionList = new List<string>(); 
                var startPosIndex = playerPieceList.IndexOf(piece);
                var startPos = playerLocationList.ElementAt(startPosIndex);

                if (piece.Contains("pawn")) tempPositionList.AddRange(GetAllPositionsForPlayerPawn(startPos, aiLocationList, playerLocationList, -1));
                else if (piece.Contains("rook")) tempPositionList.AddRange(GetAllPositionsForRook(startPos, aiLocationList, playerLocationList));
                else if (piece.Contains("bishop")) tempPositionList.AddRange(GetAllPositionsForBishop(startPos, aiLocationList, playerLocationList));
                else if (piece.Contains("queen")) tempPositionList.AddRange(GetAllPositionsForQueen(startPos, aiLocationList, playerLocationList));
                else if (piece.Contains("knight")) tempPositionList.AddRange(GetAllPositionsForKnight(startPos, playerLocationList));
                else if (piece.Contains("king")) tempPositionList.AddRange(GetAllPositionsForKing(startPos, aiLocationList, playerLocationList));

                foreach (var position in tempPositionList)
                {
                    if (position.Equals("none")) continue;

                    var points = criteria.AiLoss(position, aiLocationList, aiPieceList);
                    points += criteria.PlayerSafety(position, piece, aiLocationList, aiPieceList, playerLocationList);

                    if (points >= beta)
                    {
                        beta = points;
                        totalPositionCount += 1;

                        var pieceIndex = playerPieceList.IndexOf(piece);
                        var originalSquare = playerLocationList.ElementAt(pieceIndex);
                        var updatedPlayerLocationList = new List<string>();


                        foreach (var location in playerLocationList)
                        {
                            if (location.Equals(originalSquare)) updatedPlayerLocationList.Add(position);
                            else updatedPlayerLocationList.Add(location);
                        }
                        var updatedAiLocationList = new List<string>();
                        updatedAiLocationList.AddRange(aiLocationList);
                        var updatedAiPieceList = new List<string>();
                        updatedAiPieceList.AddRange(aiPieceList);
                        if (updatedAiLocationList.Contains(position))
                        {
                            var aiIndex = updatedAiLocationList.IndexOf(position);
                            updatedAiLocationList.RemoveAt(aiIndex);
                            updatedAiPieceList.RemoveAt(aiIndex);
                        }

                        var tempList = SecondMaxMethod(updatedAiPieceList, updatedAiLocationList, updatedPlayerLocationList, playerPieceList, depth, totalPositionCount);
                        var maxPoints = tempList.ElementAt(0);
                        totalPositionCount = tempList.ElementAt(1);
                        points = points - maxPoints;
                        pointList.Add(points);
                    }
                }
            }
            var maxValue = 0;
            foreach(var points in pointList)
            {
                maxValue = Math.Max(maxValue, points);
            }
            var maxAndCountList = new List<int>();
            maxAndCountList.Add(maxValue);
            maxAndCountList.Add(totalPositionCount);
            return maxAndCountList;
        }
        public List<int> SecondMaxMethod(List<string> aiPieceList, List<string> aiLocationList, List<string> playerLocationList, List<string> playerPieceList, int depth, int totalPositionCount)
        {
            var returnList = new List<int>();
            returnList.Add(0);
            returnList.Add(totalPositionCount);
            if (depth <= 0) return returnList;
            depth -= 1;
            var alpha = -999;
            Criteria criteria = new Criteria();
            var positionList = new List<string>();
            var pointList = new List<int>();
            var minPointList = new List<int>();
            var correspondingPieceList = new List<string>();
            foreach (var piece in aiPieceList)
            {
                var tempPositionList = new List<string>();
                var startPosIndex = aiPieceList.IndexOf(piece);
                var startPos = aiLocationList.ElementAt(startPosIndex);
                if (piece.Contains("pawn")) tempPositionList.AddRange(GetAllPositionsForPawn(startPos, playerLocationList, aiLocationList, 1));
                else if (piece.Contains("rook")) tempPositionList.AddRange(GetAllPositionsForRook(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("bishop")) tempPositionList.AddRange(GetAllPositionsForBishop(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("queen")) tempPositionList.AddRange(GetAllPositionsForQueen(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("knight")) tempPositionList.AddRange(GetAllPositionsForKnight(startPos, aiLocationList));
                else if (piece.Contains("king")) tempPositionList.AddRange(GetAllPositionsForKing(startPos, playerLocationList, aiLocationList));

                foreach (var position in tempPositionList)
                {
                    if (position.Equals("none")) continue;

                    var points = criteria.PlayerTakedown(position, playerLocationList, playerPieceList);
                    points += criteria.AiSafety(position, piece, playerLocationList, playerPieceList, aiLocationList);

                    if (points >= alpha)
                    {
                        alpha = points;
                        totalPositionCount += 1;

                        var pieceIndex = aiPieceList.IndexOf(piece);
                        var originalSquare = aiLocationList.ElementAt(pieceIndex);
                        var updatedAiLocationList = new List<string>();


                        foreach (var location in aiLocationList)
                        {
                            if (location.Equals(originalSquare)) updatedAiLocationList.Add(position);
                            else updatedAiLocationList.Add(location);
                        }
                        var updatedPlayerLocationList = new List<string>();
                        updatedPlayerLocationList.AddRange(playerLocationList);
                        var updatedPlayerPieceList = new List<string>();
                        updatedPlayerPieceList.AddRange(playerPieceList);
                        if (updatedPlayerLocationList.Contains(position))
                        {
                            var playerIndex = updatedPlayerLocationList.IndexOf(position);
                            updatedPlayerLocationList.RemoveAt(playerIndex);
                            updatedPlayerPieceList.RemoveAt(playerIndex);
                        }

                        var tempList = MinMethod(updatedPlayerPieceList, updatedPlayerLocationList, aiPieceList, updatedAiLocationList, depth, totalPositionCount);
                        var minPoints = tempList.ElementAt(0);
                        totalPositionCount = tempList.ElementAt(1);
                        points = points - minPoints;
                        pointList.Add(points);
                        minPointList.Add(minPoints);
                        positionList.Add(position);
                        correspondingPieceList.Add(piece);
                    }
                }
            }
            var maxValue = 0;
            foreach (var points in pointList)
            {
                maxValue = Math.Max(maxValue, points);
            }
            var maxAndCountList = new List<int>();
            maxAndCountList.Add(maxValue);
            maxAndCountList.Add(totalPositionCount);
            return maxAndCountList; 
        }
        public List<string> UpdatePlayerPieceList(string position, List<string> tempPlayerPieceList, List<string> playerLocationList)
        {
            if (playerLocationList.Contains(position))
            {
                var playerIndex = playerLocationList.IndexOf(position);
                var pieceId = tempPlayerPieceList.ElementAt(playerIndex);
                tempPlayerPieceList.Remove(pieceId);
            }
            return tempPlayerPieceList;
        }
        public List<string> UpdatePlayerLocationList(string position, List<string> playerLocationList)
        {
            if (playerLocationList.Contains(position))
            {
                playerLocationList.Remove(position);
            }
            return playerLocationList;
        }

        public List<string> GetBestList(List<int> pointList, List<string> positionList, List<string> correspondingPieceList, List<int> minPointList, long elapsedMs, int totalPositionCount, List<string> startPosList)
        {
            var maxPoint = -100;
            Random rnd = new Random();
            foreach (var value in pointList)
            {
                maxPoint = Math.Max(maxPoint, value);
            }
            var updatedPointList = new List<int>();
            var maxIndexList = new List<int>();
            for(var i = 0; i < pointList.Count; i++)
            {
                var point = pointList.ElementAt(i);
                if (point == maxPoint)
                {
                    maxIndexList.Add(i);
                }
            }
            var randomMaxIndex = rnd.Next(maxIndexList.Count);
            var finalIndex = maxIndexList.ElementAt(randomMaxIndex);

            var finalPosition = positionList.ElementAt(finalIndex);
            var finalPiece = correspondingPieceList.ElementAt(finalIndex);
            var finalStartPos = startPosList.ElementAt(finalIndex);
            var bestList = new List<string>();
            bestList.Add(finalPosition);
            bestList.Add(finalPiece);
            bestList.Add(maxPoint.ToString());
            bestList.Add(elapsedMs.ToString());
            bestList.Add(totalPositionCount.ToString());
            bestList.Add(finalStartPos);
            return bestList;
        }
        public List<string> AddPositions(List<string> list, params string[] positions)
        {
            foreach (var pos in positions)
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
        public List<int> CreatePointList(List<string> PositionList)
        {
            var list = new List<int>();
            foreach(var pos in PositionList)
            {
                list.Add(0);
            }
            return list;
        }
    }

    public class Criteria
    {
        public List<int> PlayerEliminations(List<string> positionList, List<int> pointList, List<string> playerLocationList, List<string> playerPieceList)
        {
            foreach (var pos in positionList)
            {
                var index = positionList.IndexOf(pos);
                if (playerLocationList.Contains(pos))
                {
                    var playerIndex = playerLocationList.IndexOf(pos);
                    var pieceId = playerPieceList.ElementAt(playerIndex);
                    if (pieceId.Contains("pawn")) pointList = UpdatePointList(pointList, 1, index);
                    else if (pieceId.Contains("bishop")) pointList = UpdatePointList(pointList, 3, index);
                    else if (pieceId.Contains("knight")) pointList = UpdatePointList(pointList, 3, index);
                    else if (pieceId.Contains("rook")) pointList = UpdatePointList(pointList, 5, index);
                    else if (pieceId.Contains("queen")) pointList = UpdatePointList(pointList, 9, index);
                    else if (pieceId.Contains("king")) pointList = UpdatePointList(pointList, 12, index);
                }
            }
            return pointList;
        }

        public int PlayerTakedown(string position, List<string> playerLocationList, List<string> playerPieceList)
        {
            int score = 0;
            if (playerLocationList.Contains(position))
            {
                var playerIndex = playerLocationList.IndexOf(position);
                var pieceId = playerPieceList.ElementAt(playerIndex);
                if (pieceId.Contains("pawn")) score = 1;
                else if (pieceId.Contains("bishop")) score = 3;
                else if (pieceId.Contains("knight")) score = 3;
                else if (pieceId.Contains("rook")) score = 5;
                else if (pieceId.Contains("queen")) score = 9;
                else if (pieceId.Contains("king")) score = 200;
            }
            return score;
        }
        public int AiSafety(string position, string aiPiece, List<string> playerLocationList, List<string> playerPieceList, List<string> aiLocationList)
        {
            var score = 0;
            AiServices services = new AiServices();
            foreach (var piece in playerPieceList)
            {
                var tempPositionList = new List<string>();
                var startPosIndex = playerPieceList.IndexOf(piece);
                var startPos = playerLocationList.ElementAt(startPosIndex);
                if (piece.Contains("pawn")) tempPositionList.AddRange(services.GetAllPositionsForPawn(startPos, aiLocationList, playerLocationList, 1));
                else if (piece.Contains("rook")) tempPositionList.AddRange(services.GetAllPositionsForRook(startPos, aiLocationList, playerLocationList));
                else if (piece.Contains("bishop")) tempPositionList.AddRange(services.GetAllPositionsForBishop(startPos, aiLocationList, playerLocationList));
                else if (piece.Contains("queen")) tempPositionList.AddRange(services.GetAllPositionsForQueen(startPos, aiLocationList, playerLocationList));
                else if (piece.Contains("knight")) tempPositionList.AddRange(services.GetAllPositionsForKnight(startPos, playerLocationList));
                else if (piece.Contains("king")) tempPositionList.AddRange(services.GetAllPositionsForKing(startPos, aiLocationList, playerLocationList));
                var finalPositionList = new List<string>();
                foreach (var tempPosition in tempPositionList)
                {
                    if (tempPosition.Equals("none")) continue;
                    if(position == tempPosition)
                    {
                        if (aiPiece.Contains("pawn")) score = 1;
                        else if (aiPiece.Contains("bishop")) score = 3;
                        else if (aiPiece.Contains("knight")) score = 3;
                        else if (aiPiece.Contains("rook")) score = 5;
                        else if (aiPiece.Contains("queen")) score = 9;
                        else if (aiPiece.Contains("king")) score = 200;
                    }
                }
            }
            //Return negative score
            return -score;
        }

        public int AiLoss(string position, List<string> aiLocationList, List<string> aiPieceList)
        {
            int score = 0;
            if (aiLocationList.Contains(position))
            {
                var aiIndex = aiLocationList.IndexOf(position);
                var pieceId = aiPieceList.ElementAt(aiIndex);
                if (pieceId.Contains("pawn")) score = 1;
                else if (pieceId.Contains("bishop")) score = 3;
                else if (pieceId.Contains("knight")) score = 3;
                else if (pieceId.Contains("rook")) score = 5;
                else if (pieceId.Contains("queen")) score = 9;
                else if (pieceId.Contains("king")) score = 200;
            }
            return score;
        }
        public int PlayerSafety(string position, string playerPiece, List<string> aiLocationList, List<string> aiPieceList, List<string> playerLocationList)
        {
            var score = 0;
            AiServices services = new AiServices();
            foreach (var piece in aiPieceList)
            {
                var tempPositionList = new List<string>();
                var startPosIndex = aiPieceList.IndexOf(piece);
                var startPos = aiLocationList.ElementAt(startPosIndex);
                if (piece.Contains("pawn")) tempPositionList.AddRange(services.GetAllPositionsForPawn(startPos, playerLocationList, aiLocationList, 1));
                else if (piece.Contains("rook")) tempPositionList.AddRange(services.GetAllPositionsForRook(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("bishop")) tempPositionList.AddRange(services.GetAllPositionsForBishop(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("queen")) tempPositionList.AddRange(services.GetAllPositionsForQueen(startPos, playerLocationList, aiLocationList));
                else if (piece.Contains("knight")) tempPositionList.AddRange(services.GetAllPositionsForKnight(startPos, aiLocationList));
                else if (piece.Contains("king")) tempPositionList.AddRange(services.GetAllPositionsForKing(startPos, playerLocationList, aiLocationList));
                var finalPositionList = new List<string>();
                foreach (var tempPosition in tempPositionList)
                {
                    if (tempPosition.Equals("none")) continue;
                    if (position == tempPosition)
                    {
                        if (playerPiece.Contains("pawn")) score = 1;
                        else if (playerPiece.Contains("bishop")) score = 3;
                        else if (playerPiece.Contains("knight")) score = 3;
                        else if (playerPiece.Contains("rook")) score = 5;
                        else if (playerPiece.Contains("queen")) score = 9;
                        else if (playerPiece.Contains("king")) score = 200;
                    }
                }
            }
            //Return negative score
            return -score;
        }

        public List<int> PieceSafety(List<string> positionList, List<int> pointList, List<string> playerLocationList, List<string> playerPieceList)
        {
            PlayerIntel playerIntel = new PlayerIntel();
            foreach(var position in positionList)
            {
                var index = positionList.IndexOf(position);
                var allPossiblePlayerMoves = playerIntel.GetAllPossiblePlayerMoves(playerPieceList, playerLocationList);
                if (!allPossiblePlayerMoves.Contains(position)) pointList = UpdatePointList(pointList, 4, index);
            }
            return pointList;
        }

        public List<int> UpdatePointList(List<int> pointList, int points, int index)
        {
            var value = pointList.ElementAt(index);
            pointList.RemoveAt(index);
            value += points;
            pointList.Insert(index, value);
            return pointList;
        }
    }

    public class PlayerIntel
    {
        public List<string> GetAllPossiblePlayerMoves(List<string> playerPieceList, List<string> playerLocationList)
        {
            var positionList = new List<string>();
            foreach(var piece in playerPieceList)
            {
                var playerIndex = playerPieceList.IndexOf(piece);
                var startPosition = playerLocationList.ElementAt(playerIndex);
                if (piece.Contains("pawn")) positionList = GetPawnPositions(positionList, startPosition);
                else if (piece.Contains("knight")) positionList = GetKnightPositions(positionList, startPosition);
                else if (piece.Contains("king")) positionList = GetKingPositions(positionList, startPosition);
                else if (piece.Contains("rook")) positionList = GetRookPositions(positionList, startPosition, playerLocationList);
                else if (piece.Contains("bishop")) positionList = GetBishopPositions(positionList, startPosition, playerLocationList);
                else if (piece.Contains("queen")) positionList = GetQueenPositions(positionList, startPosition, playerLocationList);
            }
            return positionList;
        }

        public List<string> GetPawnPositions(List<string> positionList, string startPositon)
        {
            AiServices ai = new AiServices();
            var letterList = ai.GetLetterList();
            var letter = startPositon.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            if (letterIndex == 0) return positionList;
            var number = Convert.ToInt32(startPositon.Split('-')[1]);
            var newLetter = letterList.ElementAt(letterIndex - 1);
            var posLeft = newLetter + "-" + (number - 1).ToString();
            var posRight = newLetter + "-" + (number + 1).ToString();
            positionList.Add(posLeft);
            positionList.Add(posRight);
            return positionList;
        }
        public List<string> GetKnightPositions(List<string> positionList, string startPosition)
        {
            AiServices ai = new AiServices();
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var bypassList = new List<string>();
            var letterList = ai.GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.Add(getLegitimatePosition.Knight(bypassList, letterList, letterIndex, number, -2, -1));
            positionList.Add(getLegitimatePosition.Knight(bypassList, letterList, letterIndex, number, -2, 1));
            positionList.Add(getLegitimatePosition.Knight(bypassList, letterList, letterIndex, number, -1, 2));
            positionList.Add(getLegitimatePosition.Knight(bypassList, letterList, letterIndex, number, 1, 2));
            positionList.Add(getLegitimatePosition.Knight(bypassList, letterList, letterIndex, number, 2, 1));
            positionList.Add(getLegitimatePosition.Knight(bypassList, letterList, letterIndex, number, 2, -1));
            positionList.Add(getLegitimatePosition.Knight(bypassList, letterList, letterIndex, number, 1, -2));
            positionList.Add(getLegitimatePosition.Knight(bypassList, letterList, letterIndex, number, -1, -2));
            return positionList;
        }
        public List<string> GetKingPositions(List<string> positionList, string startPosition)
        {
            AiServices ai = new AiServices();
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var bypassList = new List<string>();
            var letterList = ai.GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.Add(getLegitimatePosition.King(bypassList, letterList, letterIndex, number, -1, 0));
            positionList.Add(getLegitimatePosition.King(bypassList, letterList, letterIndex, number, -1, 1));
            positionList.Add(getLegitimatePosition.King(bypassList, letterList, letterIndex, number, 0, 1));
            positionList.Add(getLegitimatePosition.King(bypassList, letterList, letterIndex, number, 1, 1));
            positionList.Add(getLegitimatePosition.King(bypassList, letterList, letterIndex, number, 1, 0));
            positionList.Add(getLegitimatePosition.King(bypassList, letterList, letterIndex, number, 1, -1));
            positionList.Add(getLegitimatePosition.King(bypassList, letterList, letterIndex, number, 0, -1));
            positionList.Add(getLegitimatePosition.King(bypassList, letterList, letterIndex, number, -1, -1));
            return positionList;
        }
        public List<string> GetRookPositions(List<string> positionList, string startPosition, List<string> playerLocationList)
        {
            AiServices ai = new AiServices();
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var bypassList = new List<string>();
            var letterList = ai.GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, 0, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, -1, 0));
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, 0, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, 1, 0));
            return positionList;
        }

        public List<string> GetBishopPositions(List<string> positionList, string startPosition, List<string> playerLocationList)
        {
            AiServices ai = new AiServices();
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var bypassList = new List<string>();
            var letterList = ai.GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, 1, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, 1, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, -1, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, -1, 1));
            return positionList;
        }

        public List<string> GetQueenPositions(List<string> positionList, string startPosition, List<string> playerLocationList)
        {
            AiServices ai = new AiServices();
            GetLegitimatePositon getLegitimatePosition = new GetLegitimatePositon();
            var bypassList = new List<string>();
            var letterList = ai.GetLetterList();
            var letter = startPosition.Split('-')[0];
            var letterIndex = letterList.IndexOf(letter);
            var number = Convert.ToInt32(startPosition.Split('-')[1]);
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, 0, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, -1, 0));
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, 0, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, 1, 0));
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, 1, 1));
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, 1, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, -1, -1));
            positionList.AddRange(getLegitimatePosition.MultiPath(bypassList, playerLocationList, letterList, letterIndex, number, -1, 1));
            return positionList;
        }
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
        public string King(List<string> opponentLocationList, List<string> letterList, int letterIndex, int number, int letterRest, int numberRest)
        {
            var newLetterIndex = letterIndex + letterRest;
            var newNumber = number + numberRest;
            if (newLetterIndex > 7 || newLetterIndex < 0) return "none";
            if (newNumber > 8 || newNumber < 1) return "none";
            var position = letterList.ElementAt(newLetterIndex) + "-" + newNumber.ToString();
            if (opponentLocationList.Contains(position)) return "none";
            return position;
        }
        public string Knight(List<string> opponentLocationList, List<string> letterList, int letterIndex, int number, int letterRest, int numberRest)
        {
            var newLetterIndex = letterIndex + letterRest;
            var newNumber = number + numberRest;
            if (newLetterIndex > 7 || newLetterIndex < 0) return "none";
            if (newNumber > 8 || newNumber < 1) return "none";
            var position = letterList.ElementAt(letterIndex + letterRest) + "-" + (number + numberRest).ToString();
            if (opponentLocationList.Contains(position)) return "none";
            return position;
        }
    }
}

