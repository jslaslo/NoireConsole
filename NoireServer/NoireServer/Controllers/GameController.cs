using Microsoft.AspNetCore.Mvc;
using NoireServer.Database.Model;
using NoireServer.Dto.Responses;

namespace NoireServer.Controllers
{
    public class GameController: Controller
    {
        public ActionResult GetCurrentGame()
        {
            Logic logic = new();
            ResponseGameData game = logic.GetCurrentGame()!;
            return Ok(game);
        }

        public ActionResult GetGame(int id)
        {
            Logic logic = new();
            GameData game = logic.GetGame(id);
            return Ok(game);
        }
        public ActionResult GetPlayGame(int id, string role)
        {
            Logic logic = new();
            ResponseGameData game = logic.GetPlayGame(id, role)!;
            return Ok(game);
        }
        public ActionResult GetMove(string stringJson)
        {
            Logic logic = new();
            ResponseGameData? game = logic.GetMove(stringJson);
            return Ok(game);
        }
    }
}
