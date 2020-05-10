﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TBoard.Dto;
using TBoard.Entities;

namespace TBoard.WebApi.Profiles
{
    public class PlayerProfile : Profile
    {

        public PlayerProfile()
        {
            CreateMap<Player, PlayerDto>();
            CreateMap<PlayerDto, Player>();
            CreateMap<PlayerForCreationDto, Player>();
            CreateMap<Player, PlayerForCreationDto>();
            CreateMap<Player, PlayerForUpdateDto>();
            CreateMap<PlayerForUpdateDto, Player>();

        }

    }
}
