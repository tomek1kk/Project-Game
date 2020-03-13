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
        private IGUIDataProvider _GUIDataProvider;
        public BoardController(IGUIDataProvider GUIDataProvider)
        {
            _GUIDataProvider = GUIDataProvider;
        }
        // GET: api/<controller>
        [HttpGet]
        public BoardModel GetBoardModel()
        {
            Random r = new Random();
            var baseModel = _GUIDataProvider.GetCurrentBoardModel();
            for(int i = 0; i < baseModel.Fields.GetLength(0); i++)
            {
                for(int j = 0; j < baseModel.Fields.GetLength(1); j++)
                {
                    baseModel.Fields[i, j] = (FieldType)(r.Next()%Enum.GetValues(typeof(FieldType)).Length);
                }
            }
            return baseModel;
        }

        // GET api/<controller>/5
        [HttpGet("{version}")]
        public string Get(int version)
        {
            return "value";
        }

    }
}
