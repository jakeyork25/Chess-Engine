using ChessEngine.Models;
using ChessEngine.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ChessEngine.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult GetQuote()
        {
            VisualServices services = new VisualServices();
            var finalQuote = services.GenerateAiQuote();
            return Json(new { quote = finalQuote });
        }

        public ActionResult ShortenMove(ShortModel model)
        {
            VisualServices services = new VisualServices();
            var newPosition = services.ShortenPosition(model.position);
            var newPiece = services.ShortenPiece(model.piece);
            return Json(new { shortPosition = newPosition, shortPiece = newPiece });
        }

        public ActionResult GetIconClass(string startClass)
        {
            var finalClass = "small-" + startClass;
            return Json(new { smallClass = finalClass });
        }
        public ActionResult MovePlayerPiece(OptionModel model)
        {
            ChessServices services = new ChessServices();
            AiServices ai = new AiServices();
            var pawnchange = services.CanPawnChange(model.pieceClass, model.endPosition); 
            var positionList = services.GetAvailableSquares(model.startPosition, model.pieceClass, model.opponentSquareList, model.playerSquareList);
            var availability = services.CheckAvailability(positionList, model.startPosition, model.endPosition, model.playerSquareList, model.playerPieceList, model.opponentPieceList, model.opponentSquareList);
            var tempAiPieceList = ai.UpdatePlayerPieces(model.endPosition, model.opponentSquareList, model.opponentPieceList);
            var tempAiLocationList = ai.UpdatePlayerLocations(model.endPosition, model.opponentSquareList);
            var tempPlayerLocationList = ai.UpdateAiLocationList(model.startPosition, model.endPosition, model.playerSquareList);
            var tempCheck = services.CheckMethod(tempAiPieceList, tempAiLocationList, model.playerPieceList, tempPlayerLocationList);
            var checkMate = services.CheckmateMethod(tempAiPieceList, tempAiLocationList, model.playerPieceList, tempPlayerLocationList);
            var staleMate = services.StalemateMethod(tempAiPieceList, tempAiLocationList, model.playerPieceList, tempPlayerLocationList);
            return Json(new { canMove = availability, check = tempCheck, checkmate = checkMate, stalemate = staleMate, pawnChange = pawnchange });
        }

        public ActionResult GetPlayerOptions(OptionModel model)
        {
            ChessServices.Options options = new ChessServices.Options();
            var combinedLists = options.GetAllPlayerPositions(model.startPosition, model.pieceClass, model.opponentSquareList, model.playerSquareList);
            var positionList = combinedLists.ElementAt(0);
            var hostilePositionList = combinedLists.ElementAt(1);
            return Json(new { options = positionList, hostileOptions = hostilePositionList });
        }

        public ActionResult MoveOpponentPiece(OpponentMoveModel model)
        {
            
            AiServices ai = new AiServices();
            ChessServices services = new ChessServices();
            var bestList = ai.MaxMethod(model.opponentPieceList, model.opponentSquareList, model.playerSquareList, model.playerPieceList, 3);
            var bestPos = bestList.ElementAt(0);
            var bestPiece = bestList.ElementAt(1);
            var bestScore = bestList.ElementAt(2);
            var startPos = bestList.ElementAt(5);
            var removePlayer = "";
            if (model.playerSquareList != null && model.playerSquareList.Contains(bestPos))
            {
                var tempIndex = model.playerSquareList.IndexOf(bestPos);
                removePlayer = model.playerPieceList.ElementAt(tempIndex);
            }
            var tempPlayerPieceList = ai.UpdatePlayerPieces(bestPos, model.playerSquareList, model.playerPieceList);
            var tempPlayerLocationList = ai.UpdatePlayerLocations(bestPos, model.playerSquareList);
            var tempAiLocationList = ai.UpdateAiLocationList(startPos, bestPos, model.opponentSquareList);
            var tempCheck = services.CheckMethod(tempPlayerPieceList, tempPlayerLocationList, model.opponentPieceList, tempAiLocationList);
            var checkMate = services.CheckmateMethod(tempPlayerPieceList, tempPlayerLocationList, model.opponentPieceList, tempAiLocationList);
            var staleMate = services.StalemateMethod(tempPlayerPieceList, tempPlayerLocationList, model.opponentPieceList, tempAiLocationList);
            var changePawn = ai.ChangePawnPiece(bestPiece, bestPos);
            return Json(new { opponentPiece = bestPiece, oppPosition = bestPos, removePlayerPiece = removePlayer, finalPosition = bestPos, score = bestScore, checkmate = checkMate, check = tempCheck, newPawn = changePawn });
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
