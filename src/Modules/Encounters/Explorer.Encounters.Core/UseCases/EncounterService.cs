﻿using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain.Encounters;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Internal;
using FluentResults;

namespace Explorer.Encounters.Core.UseCases
{
    public class EncounterService : CrudService<EncounterDto, Encounter>, IEncounterService
    {
        private readonly IEncounterRepository _encounterRepository;
        private readonly IInternalCheckpointService _internalCheckpointService;
        public EncounterService(IEncounterRepository encounterRepository,IInternalCheckpointService internalCheckpointService, IMapper mapper) : base(encounterRepository, mapper)
        {
            _encounterRepository= encounterRepository;
            _internalCheckpointService= internalCheckpointService;
        }

        public Result<EncounterDto> Create(EncounterDto encounterDto,long checkpointId,bool isSecretPrerequisite,long userId)
        {
            Encounter encounter = MapToDomain(encounterDto);
            Encounter result;
            if (!encounter.IsAuthor(userId)) 
                return Result.Fail(FailureCode.Forbidden); 

            try
            { 
                result = _encounterRepository.Create(new Encounter(encounter));
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }

            try
            {
                Result<CheckpointDto> updateCheckpointResult= _internalCheckpointService.SetEncounter((int)checkpointId, result.Id, isSecretPrerequisite, (int)result.AuthorId);
                if (!updateCheckpointResult.IsSuccess && updateCheckpointResult.Reasons[0].Metadata.ContainsValue(404))
                    return Result.Fail(FailureCode.NotFound);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
            return MapToDto(result);
        }

        public Result<EncounterDto> Update(EncounterDto encounter, long userId)
        {
            throw new NotImplementedException();
        }
    }
}
