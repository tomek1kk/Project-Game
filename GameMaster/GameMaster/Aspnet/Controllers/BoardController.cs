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
        private IGuiDataProvider _GUIDataProvider;
        public BoardController(IGuiDataProvider GUIDataProvider)
        {
            _GUIDataProvider = GUIDataProvider;
        }
        // GET: api/<controller>
        [HttpGet]
        public BoardModel GetBoardModel()
        {
            return _GUIDataProvider.GetCurrentBoardModel();
        }


    }
}
