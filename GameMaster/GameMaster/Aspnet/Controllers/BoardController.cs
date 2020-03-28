using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameMaster.AspNet.Models;
using Microsoft.AspNetCore.Mvc;
using GameMaster.GUI;


namespace GameMaster.Aspnet.Controllers
{
    [Route("api/[controller]")]
    public class BoardController : Controller
    {
        private readonly IGuiDataProvider _guiDataProvider;
        private readonly IGuiActionsExecutor _guiActionsExecutor;
        public BoardController(IGuiDataProvider guiDataProvider, IGuiActionsExecutor guiActionsExecutor)
        {
            _guiDataProvider = guiDataProvider;
            _guiActionsExecutor = guiActionsExecutor;
        }
        // GET: api/<controller>
        [HttpGet]
        public BoardModel GetBoardModel()
        {
            return _guiDataProvider.GetCurrentBoardModel();
        }

        // POST: api/<controller>/start
        [HttpPost("start")]
        public void Start()
        {
            _guiActionsExecutor.StartGame();
        }


    }
}
