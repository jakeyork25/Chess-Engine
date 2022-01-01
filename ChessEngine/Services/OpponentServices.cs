using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessEngine.Services
{
    public class OpponentServices
    {
        public List<string> GetRandomPositionList(string position, string pieceClass)
        {
            var newPos = new List<string>();
            PawnMethods pawnMethods = new PawnMethods();
            RookMethods rookMethods = new RookMethods();
            KingMethods kingMethods = new KingMethods();
            QueenMethods queenMethods = new QueenMethods();
            KnightMethods knightMethods = new KnightMethods();
            BishopMethods bishopMethods = new BishopMethods();
            for (var i = 0; i < 8; i++)
            {
                switch (pieceClass)
                {
                    case "pawn": newPos.Add(pawnMethods.GetFinalPosition(position)); break;
                    case "rook": newPos.Add(rookMethods.GetFinalPosition(position)); break;
                    case "king": newPos.Add(kingMethods.GetFinalPosition(position)); break;
                    case "queen": newPos.Add(queenMethods.GetFinalPosition(position)); break;
                    case "knight": newPos.Add(knightMethods.GetFinalPosition(position)); break;
                    case "bishop": newPos.Add(bishopMethods.GetFinalPosition(position)); break;
                }
            }
            return newPos;
        }
        public class PawnMethods
        {
            public string GetFinalPosition(string position)
            {
                return position;
            }
        }
        public class KingMethods
        {
            public string GetFinalPosition(string position)
            {
                OpponentServices services = new OpponentServices();
                var newPos = "";
                var letterList = services.GetLetterList();
                var letter = position.Split('-')[0];
                var letterIndex = letterList.IndexOf(letter);
                var number = Convert.ToInt32(position.Split('-')[1]);
                var directionString = "";
                if (number < 8) directionString += "right";
                if (number > 1) directionString += "|left";
                if (letterIndex > 0) directionString += "|up";
                if (letterIndex < 7) directionString += "|down";
                if (number > 1 && letterIndex > 0) directionString += "|upLeft";
                if (number < 8 && letterIndex > 0) directionString += "|upRight";
                if (number > 1 && letterIndex < 7) directionString += "|downLeft";
                if (number < 8 && letterIndex < 7) directionString += "|downRight";
                directionString = directionString.TrimStart('|');
                string[] directionsList = directionString.Split('|');
                Random rnd = new Random();
                string direction = directionsList[rnd.Next(directionsList.Length)];
                switch (direction)
                {
                    case "left": newPos = GetLeftPosition(number, letter); break;
                    case "up": newPos = GetUpPosition(number, letterIndex); break;
                    case "right": newPos = GetRightPosition(number, letter); break;
                    case "down": newPos = GetDownPosition(number, letterIndex); break;
                    case "upLeft": newPos = GetUpLeftPosition(number, letterIndex, letterList); break;
                    case "upRight": newPos = GetUpRightPosition(number, letterIndex, letterList); break;
                    case "downLeft": newPos = GetDownLeftPosition(number, letterIndex, letterList); break;
                    case "downRight": newPos = GetDownRightPosition(number, letterIndex, letterList); break;
                }
                return newPos;
            }
            public string GetLeftPosition(int number, string letter)
            {
                var numberPos = number - 1;
                return letter + "-" + numberPos.ToString();
            }
            public string GetRightPosition(int number, string letter)
            {
                var numberPos = number + 1;
                return letter + "-" + numberPos.ToString();
            }
            public string GetUpPosition(int number, int letterIndex)
            {
                OpponentServices services = new OpponentServices();
                var letterList = services.GetLetterList();
                var letterPos = letterList.ElementAt(letterIndex - 1);
                return letterPos + "-" + number.ToString();
            }
            public string GetDownPosition(int number, int letterIndex)
            {
                OpponentServices services = new OpponentServices();
                var letterList = services.GetLetterList();
                var letterPos = letterList.ElementAt(letterIndex + 1);
                return letterPos + "-" + number.ToString();
            }
            public string GetUpLeftPosition(int number, int letterIndex, List<string> letterList)
            {
                var numberPos = number - 1;
                var letterPos = letterList.ElementAt(letterIndex - 1);
                return letterPos + "-" + numberPos.ToString();
            }
            public string GetUpRightPosition(int number, int letterIndex, List<string> letterList)
            {
                var numberPos = number + 1;
                var letterPos = letterList.ElementAt(letterIndex - 1);
                return letterPos + "-" + numberPos.ToString();
            }
            public string GetDownLeftPosition(int number, int letterIndex, List<string> letterList)
            {
                var numberPos = number - 1;
                var letterPos = letterList.ElementAt(letterIndex + 1);
                return letterPos + "-" + numberPos.ToString();
            }
            public string GetDownRightPosition(int number, int letterIndex, List<string> letterList)
            {
                var numberPos = number + 1;
                var letterPos = letterList.ElementAt(letterIndex + 1);
                return letterPos + "-" + numberPos.ToString();
            }
        }
        public class RookMethods
        {
            public string GetFinalPosition(string position)
            {
                OpponentServices services = new OpponentServices();
                var newPos = "";
                var letterList = services.GetLetterList();
                var letter = position.Split('-')[0];
                var letterIndex = letterList.IndexOf(letter);
                var number = Convert.ToInt32(position.Split('-')[1]);
                var directionString = "";
                if (number < 8) directionString += "right";
                if (number > 1) directionString += "|left";
                if (letterIndex > 0) directionString += "|up";
                if (letterIndex < 7) directionString += "|down";
                directionString = directionString.TrimStart('|');
                string[] directionsList = directionString.Split('|');
                Random rnd = new Random();
                string direction = directionsList[rnd.Next(directionsList.Length)];
                switch (direction)
                {
                    case "left": newPos = GetLeftPosition(number, letter); break;
                    case "right": newPos = GetRightPosition(number, letter); break;
                    case "up": newPos = GetUpPosition(letterIndex, number, letterList); break;
                    case "down": newPos = GetDownPosition(letterIndex, number, letterList); break;
                }
                return newPos;
            }
            public string GetLeftPosition(int number, string letter)
            {
                Random rnd = new Random();
                var posNumber = rnd.Next(1, number - 1);
                return letter + "-" + posNumber.ToString();
            }
            public string GetRightPosition(int number, string letter)
            {
                Random rnd = new Random();
                var posNumber = rnd.Next(number + 1, 8);
                return letter + "-" + posNumber.ToString();
            }
            public string GetUpPosition(int letterIndex, int number, List<string> letterList)
            {
                Random rnd = new Random();
                var posLetterIndex = rnd.Next(0, letterIndex - 1);
                var posLetter = letterList.ElementAt(posLetterIndex);
                return posLetter + "-" + number.ToString();
            }
            public string GetDownPosition(int letterIndex, int number, List<string> letterList)
            {
                Random rnd = new Random();
                var posLetterIndex = rnd.Next(letterIndex + 1, 7);
                var posLetter = letterList.ElementAt(posLetterIndex);
                return posLetter + "-" + number.ToString();
            }
        }
        public class QueenMethods
        {
            public string GetFinalPosition(string position)
            {
                OpponentServices services = new OpponentServices();
                RookMethods rookMethods = new RookMethods();
                BishopMethods bishopMethods = new BishopMethods();
                var newPos = "";
                var letterList = services.GetLetterList();
                var letter = position.Split('-')[0];
                var letterIndex = letterList.IndexOf(letter);
                var number = Convert.ToInt32(position.Split('-')[1]);
                var directionString = "";
                if (number < 8) directionString += "right";
                if (number > 1) directionString += "|left";
                if (letterIndex > 0) directionString += "|up";
                if (letterIndex < 7) directionString += "|down";
                if (number > 1 && letterIndex > 0) directionString += "|upLeft";
                if (number < 8 && letterIndex > 0) directionString += "|upRight";
                if (number > 1 && letterIndex < 7) directionString += "|downLeft";
                if (number < 8 && letterIndex < 7) directionString += "|downRight";
                directionString = directionString.TrimStart('|');
                string[] directionsList = directionString.Split('|');
                Random rnd = new Random();
                string direction = directionsList[rnd.Next(directionsList.Length)];
                switch (direction)
                {
                    case "left": newPos = rookMethods.GetLeftPosition(number, letter); break;
                    case "right": newPos = rookMethods.GetRightPosition(number, letter); break;
                    case "up": newPos = rookMethods.GetUpPosition(letterIndex, number, letterList); break;
                    case "down": newPos = rookMethods.GetDownPosition(letterIndex, number, letterList); break;
                    case "upLeft": newPos = bishopMethods.GetUpLeftPosition(number, letterIndex, letterList); break;
                    case "upRight": newPos = bishopMethods.GetUpRightPosition(number, letterIndex, letterList); break;
                    case "downLeft": newPos = bishopMethods.GetDownLeftPosition(number, letterIndex, letterList); break;
                    case "downRight": newPos = bishopMethods.GetDownRightPosition(number, letterIndex, letterList); break;
                }
                return newPos;
            }
        }
        public class KnightMethods
        {
            public string GetFinalPosition(string position)
            {
                OpponentServices services = new OpponentServices();
                RookMethods rookMethods = new RookMethods();
                BishopMethods bishopMethods = new BishopMethods();
                var newPos = "";
                var letterList = services.GetLetterList();
                var letter = position.Split('-')[0];
                var letterIndex = letterList.IndexOf(letter);
                var number = Convert.ToInt32(position.Split('-')[1]);
                var directionString = "";
                if (number > 1 && letterIndex > 1) directionString += "|upLeft";
                if (number < 8 && letterIndex > 1) directionString += "|upRight";
                if (number < 6 && letterIndex > 0) directionString += "|rightUp";
                if (number < 6 && letterIndex < 7) directionString += "|rightDown";
                if (number < 8 && letterIndex < 6) directionString += "|downRight";
                if (number > 1 && letterIndex < 6) directionString += "|downLeft";
                if (number > 2 && letterIndex < 7) directionString += "|leftDown";
                if (number > 2 && letterIndex > 0) directionString += "|leftUp";
                directionString = directionString.TrimStart('|');
                string[] directionsList = directionString.Split('|');
                Random rnd = new Random();
                string direction = directionsList[rnd.Next(directionsList.Length)];
                switch (direction)
                {
                    case "upLeft": newPos = GetKnightPosition(number, -1, letterIndex, -2); break;
                    case "upRight": newPos = GetKnightPosition(number, 1, letterIndex, -2); break;
                    case "rightUp": newPos = GetKnightPosition(number, 2, letterIndex, -1); break;
                    case "rightDown": newPos = GetKnightPosition(number, 2, letterIndex, 1); break;
                    case "downRight": newPos = GetKnightPosition(number, 1, letterIndex, 2); break;
                    case "downLeft": newPos = GetKnightPosition(number, -1, letterIndex, 2); break;
                    case "leftDown": newPos = GetKnightPosition(number, -2, letterIndex, 1); break;
                    case "leftUp": newPos = GetKnightPosition(number, -2, letterIndex, -1); break;
                }
                return newPos;
            }

            public string GetKnightPosition(int number, int numberDiff, int letterIndex, int letterDiff)
            {
                OpponentServices services = new OpponentServices();
                var letterList = services.GetLetterList();
                var numberPos = number + numberDiff;
                var letterPos = letterList.ElementAt(letterIndex + letterDiff);
                return letterPos + "-" + numberPos.ToString();
            }
        }
        public class BishopMethods
        {
            public string GetFinalPosition(string position)
            {
                OpponentServices services = new OpponentServices();
                var newPos = "";
                var letterList = services.GetLetterList();
                var letter = position.Split('-')[0];
                var letterIndex = letterList.IndexOf(letter);
                var number = Convert.ToInt32(position.Split('-')[1]);
                var directionString = "";
                if (number > 1 && letterIndex > 0) directionString += "upLeft";
                if (number < 8 && letterIndex > 0) directionString += "|upRight";
                if (number > 1 && letterIndex < 7) directionString += "|downLeft";
                if (number < 8 && letterIndex < 7) directionString += "|downRight";
                directionString = directionString.TrimStart('|');
                string[] directionsList = directionString.Split('|');
                Random rnd = new Random();
                string direction = directionsList[rnd.Next(directionsList.Length)];
                switch (direction)
                {
                    case "upLeft": newPos = GetUpLeftPosition(number, letterIndex, letterList); break;
                    case "upRight": newPos = GetUpRightPosition(number, letterIndex, letterList); break;
                    case "downLeft": newPos = GetDownLeftPosition(number, letterIndex, letterList); break;
                    case "downRight": newPos = GetDownRightPosition(number, letterIndex, letterList); break;
                }
                return newPos;
            }
            public string GetUpLeftPosition(int number, int letterIndex, List<string> letterList)
            {
                Random rnd = new Random();
                var smallestDistance = Math.Min(number - 1, letterIndex);
                var randomDistance = rnd.Next(1, smallestDistance);
                var posLetterIndex = letterIndex - randomDistance;
                var posLetter = letterList.ElementAt(posLetterIndex);
                var posNumber = number - randomDistance;
                return posLetter + "-" + posNumber.ToString();
            }
            public string GetUpRightPosition(int number, int letterIndex, List<string> letterList)
            {
                Random rnd = new Random();
                var smallestDistance = Math.Min(8 - number, letterIndex);
                var randomDistance = rnd.Next(1, smallestDistance);
                var posLetterIndex = letterIndex - randomDistance;
                var posLetter = letterList.ElementAt(posLetterIndex);
                var posNumber = number + randomDistance;
                return posLetter + "-" + posNumber.ToString();
            }
            public string GetDownLeftPosition(int number, int letterIndex, List<string> letterList)
            {
                Random rnd = new Random();
                var smallestDistance = Math.Min(number - 1, letterList.Count - 1 - letterIndex);
                var randomDistance = rnd.Next(1, smallestDistance);
                var posLetterIndex = letterIndex + randomDistance;
                var posLetter = letterList.ElementAt(posLetterIndex);
                var posNumber = number - randomDistance;
                return posLetter + "-" + posNumber.ToString();
            }
            public string GetDownRightPosition(int number, int letterIndex, List<string> letterList)
            {
                Random rnd = new Random();
                var smallestDistance = Math.Min(8 - number, letterList.Count - 1 - letterIndex);
                var randomDistance = rnd.Next(1, smallestDistance);
                var posLetterIndex = letterIndex + randomDistance;
                var posLetter = letterList.ElementAt(posLetterIndex);
                var posNumber = number + randomDistance;
                return posLetter + "-" + posNumber.ToString();
            }
        }

        public List<string> GetPositionsWithinPathway(string startPos, string endPos, string pieceClass)
        {
            var list = new List<string>();
            switch (pieceClass)
            {
                case "rook": list = GetRookPath(startPos, endPos); break;
                default: list.Add(endPos); break;
            }
            return list;
        }

        public List<string> GetRookPath(string startPos, string endPos)
        {
            var pathList = new List<string>();
            var letterList = GetLetterList();
            var startLetter = startPos.Split('-')[0];
            var startNumber = Convert.ToInt32(startPos.Split("-")[1]);
            var endLetter = endPos.Split('-')[0];
            var endNumber = Convert.ToInt32(endPos.Split("-")[1]);
            var startLetterIndex = letterList.IndexOf(startLetter);
            var endLetterIndex = letterList.IndexOf(endLetter);
            var letterDiffernce = endLetterIndex - startLetterIndex;
            var numberDiffernce = endNumber - startNumber;
            if (letterDiffernce != 0)
            {
                if (letterDiffernce < 0)
                {
                    for (var i = endLetterIndex + 1; i < startLetterIndex; i++)
                    {
                        pathList.Add(letterList.ElementAt(i) + "-" + startNumber);
                    }
                }
                else
                {
                    for (var i = endLetterIndex - 1; i > startLetterIndex; i--)
                    {
                        pathList.Add(letterList.ElementAt(i) + "-" + startNumber);
                    }
                }
            }
            else
            {
                if (numberDiffernce > 0)
                {
                    for (var i = endNumber - 1; i > startNumber; i--)
                    {
                        pathList.Add(startLetter + "-" + i.ToString());
                    }
                }
                else
                {
                    for (var i = endNumber + 1; i < startNumber; i++)
                    {
                        pathList.Add(startLetter + "-" + i.ToString());
                    }
                }
            }
            return pathList;
        }
        public string GetFinalPosition(List<string> positionList, string pieceClass)
        {
            var finalPos = "";
            return finalPos;
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

        public List<string> GetAllPositions()
        {
            var list = new List<string>();
            list = AddPositions(list, "a-1", "a-2", "a-3", "a-4", "a-5", "a-6", "a-7", "a-8", "b-1", "b-2", "b-3", "b-4", "b-5", "b-6", "b-7", "b-8", "c-1", "c-2", "c-3", "c-4", "c-5", "c-6", "c-7", "c-8", "d-1", "d-2", "d-3", "d-4", "d-5", "d-6", "d-7", "d-8", "e-1", "e-2", "e-3", "e-4", "e-5", "e-6", "e-7", "e-8", "f-1", "f-2", "f-3", "f-4", "f-5", "f-6", "f-7", "f-8", "g-1", "g-2", "g-3", "g-4", "g-5", "g-6", "g-7", "g-8", "h-1", "h-2", "h-3", "h-4", "h-5", "h-6", "h-7", "h-8");
            return list;
        }
    }
}
