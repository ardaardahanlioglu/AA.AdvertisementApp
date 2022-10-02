using AA.AdvertisementApp.Business.Extension;
using AA.AdvertisementApp.Business.Interfaces;
using AA.AdvertisementApp.Common;
using AA.AdvertisementApp.DataAccess.UnitOfWork;
using AA.AdvertisementApp.Dtos.Interfaces;
using AA.AdvertisementApp.Dtos;
using AA.AdvertisementApp.Entities;
using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.AdvertisementApp.Business.Services
{
    public class Service<CreateDto, UpdateDto, ListDto, T> : IService<CreateDto, UpdateDto, ListDto, T>
        where CreateDto : class, IDto, new()
        where UpdateDto : class, IUpdateDto, new()
        where ListDto : class, IDto, new()
        where T : BaseEntity
    {
        private readonly IMapper _mapper;
        private IValidator<CreateDto> _createDtoValidator;
        private IValidator<UpdateDto> _updateDtoValidator;
        private readonly IUow _uow;

        public Service(IMapper mapper, IValidator<CreateDto> createDtoValidator, IValidator<UpdateDto> updateDtoValidator, IUow uow)
        {
            _mapper = mapper;
            _createDtoValidator = createDtoValidator;
            _updateDtoValidator = updateDtoValidator;
            _uow = uow;
        }
        /* Create işleminde öncelikle ilgili olayın valid durumunu kontrol ediyorum.
         bir şeyin valid kontrolü içinde validator kullanıyoruz._createDtoValidator aracılığı ile 
         bu olayın validini kontrol edeceğim.valid olduğu zamanda uow aracılığla ilgili repo'yu 
         çağırıyorum.hangi repository ile çalışıyor ise o repoyu CreateAsync diyroum ve dto'yu değil mapper 
         ile ilgili entity'e maplenmiş dto'yu alıyorum.  
         */
        public async Task<IResponse<CreateDto>> CreateAsync(CreateDto dto)
        {
            var result = _createDtoValidator.Validate(dto);
            if (result.IsValid)
            {
                var createdEntity = _mapper.Map<T>(dto);
                await _uow.GetRespository<T>().CreateAsync(createdEntity);
                await _uow.SaveChangesAsync();
                return new Response<CreateDto>(ResponseType.Successs, dto);
            }
            return new Response<CreateDto>(dto, result.ConverToCustomValidationError());
        }

        public async Task<IResponse<List<ListDto>>> GetAllAsync()
        {
            var data = await _uow.GetRespository<T>().GetAllAsync();
            var dto = _mapper.Map<List<ListDto>>(data);
            return new Response<List<ListDto>>(ResponseType.Successs, dto);
        }

        public async Task<IResponse<IDto>> GetByIdAsync<IDto>(int id)
        {

            var data = await _uow.GetRespository<T>().GetByFilterAsync(x => x.Id == id);
            if (data == null)
                return new Response<IDto>(ResponseType.NotFound, $"{id} ye sahip data bulunamadı");
            var dto = _mapper.Map<IDto>(data);
            return new Response<IDto>(ResponseType.Successs,dto);
        }

        public async Task<IResponse> RemoveAsync(int id)
        {
            var data = await _uow.GetRespository<T>().FindAsync(id);
            if (data == null)
                return new Response(ResponseType.NotFound, $"{id} idisine sahip data bulunamadı");
            _uow.GetRespository<T>().Remove(data);
            await _uow.SaveChangesAsync();
            return new Response(ResponseType.Successs);
        }

        public async Task<IResponse<UpdateDto>> UpdateAsync(UpdateDto dto)
        {
            var result = _updateDtoValidator.Validate(dto);
            if (result.IsValid) 
            {
                var unchangedData = await _uow.GetRespository<T>().FindAsync(dto.Id);
                if (unchangedData == null)
                    return new Response<UpdateDto>(ResponseType.NotFound, $"{dto.Id} idisine sahip data bulunamadı");
                var entity = _mapper.Map<T>(dto);
                _uow.GetRespository<T>().Update(entity, unchangedData);
                await _uow.SaveChangesAsync();
                return new Response<UpdateDto>(ResponseType.Successs, dto);
            }
            return new Response<UpdateDto>(dto, result.ConverToCustomValidationError());




        }
    }
}
