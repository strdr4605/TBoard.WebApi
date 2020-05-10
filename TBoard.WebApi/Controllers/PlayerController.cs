﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TBoard.Dto;
using TBoard.Entities;
using TBoard.Entities.Auth;
using TBoard.WebApi.ResourceParameters;
using TBoard.WebApi.Services.Implementation;
using AllowAnonymousAttribute = Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute;


namespace TBoard.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {

        private readonly IPlayerService playerService;
        public PlayerController(IPlayerService playerService)
        {
            this.playerService = playerService;
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<PlayerDto>> GetAll([FromQuery]PlayerResourceParameters playerResourceParameters)
        {
            return Ok(playerService.GetAll(playerResourceParameters));
        }

        [HttpGet("byId")]
        public ActionResult<PlayerForUpdateDto> GetById()
        {
            var playerId = User.Identity.GetUserId();
            return Ok(playerService.GetById(Int32.Parse(playerId)));
        }

        [HttpDelete("{playerId}")]
        public void DeleteById(int playerId)
        {
            playerService.DeleteById(playerId);
        }


        [AllowAnonymous]
        [HttpGet("roles")]
        public ActionResult<Role> GetRoles()
        {
            return Ok(playerService.GetAllRoles());
        }
    }
}
