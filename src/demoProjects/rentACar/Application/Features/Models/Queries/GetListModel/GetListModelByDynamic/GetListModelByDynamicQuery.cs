using Application.Features.Models.Models;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Models.Queries.GetListModel.GetListModelByDynamic
{
    public class GetListModelByDynamicQuery : IRequest<ModelListModel>
    {
        public Dynamic Dynamic { get; set; }
        public PageRequest PageRequest { get; set; }

        public class GetListModelByDynamicQueryHandler : IRequestHandler<GetListModelByDynamicQuery, ModelListModel>
        {
            private readonly IMapper _mapper;
            private readonly IModelRepository _modelRepository;

            public GetListModelByDynamicQueryHandler(IMapper mapper, IModelRepository modelRepository)
            {
                _mapper = mapper;
                _modelRepository = modelRepository;
            }

            public async Task<ModelListModel> Handle(GetListModelByDynamicQuery request, CancellationToken cancellationToken)
            {
                IPaginate<Model> models = await _modelRepository.GetListByDynamicAsync(request.Dynamic, //dinamik olarak arama yapabiliyor, filtreleme yapabiliyor
                                              include:
                                              m => m.Include(c => c.Brand),//Include ile join işlemi yapıyoruz. Burada araba modellerini markalar ile joinledik
                                              index: request.PageRequest.Page,
                                              size: request.PageRequest.PageSize
                                              );
                //datamodel
                ModelListModel mappedModels = _mapper.Map<ModelListModel>(models);
                return mappedModels;
            }
        }
    }
}
