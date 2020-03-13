using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameMaster.AspNet.Models;
using Microsoft.AspNetCore.Mvc;


namespace GameMaster.Aspnet.Controllers
{
    [Route("api/[controller]")]
    public class BoardController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public BoardModel GetBoardModel()
        {
            BoardModel res = new BoardModel
            {
                Width = 10,
                Height = 10,
                GoalAreaHeight = 3
            };
            var fields = new FieldType[res.Width, res.Height];
            fields[3, 3] = FieldType.Piece;
            res.Fields = fields;
            return res;
        }

        // GET api/<controller>/5
        [HttpGet("{version}")]
        public string Get(int version)
        {
            return "value";
        }

    }
}
