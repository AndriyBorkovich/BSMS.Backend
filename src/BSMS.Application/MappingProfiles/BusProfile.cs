﻿using BSMS.Application.Features.Bus.Commands;
using BSMS.Application.Features.Bus.Commands.Create;
using BSMS.Core.Entities;
using Mapster;

namespace BSMS.Application.MappingProfiles;

public class BusProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateBusCommand, Bus>()
            .Map(dest => dest.BusScheduleEntries, src => src.BusScheduleEntries);
    }
}