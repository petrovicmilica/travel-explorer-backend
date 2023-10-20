﻿using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourService : CrudService<TourDto, Tour>, ITourService
    {
        public TourService(ICrudRepository<Tour> repository, IMapper mapper) : base(repository, mapper) { }

        public Result<List<TourDto>> GetToursByAuthor(int page, int pageSize, int id) { 

            var allTours = CrudRepository.GetPaged(page, pageSize);
            List<Tour> toursByAuthor= allTours.Results.Where(t=>t.AuthorId == id).ToList();
            return MapToDto(toursByAuthor);

        }

    }
}