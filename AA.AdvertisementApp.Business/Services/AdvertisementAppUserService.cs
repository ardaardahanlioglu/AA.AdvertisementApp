using AA.AdvertisementApp.Business.Extension;
using AA.AdvertisementApp.Business.Interfaces;
using AA.AdvertisementApp.Common;
using AA.AdvertisementApp.Common.Enums;
using AA.AdvertisementApp.DataAccess.UnitOfWork;
using AA.AdvertisementApp.Dtos;
using AA.AdvertisementApp.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.AdvertisementApp.Business.Services
{
    public class AdvertisementAppUserService : IAdvertisementAppUserService
    {
        private readonly IUow _uow;
        private readonly IValidator<AdvertisementAppUserCreateDto> _createDtoValidator;
        private IMapper _mapper;
        public AdvertisementAppUserService(IUow uow, IValidator<AdvertisementAppUserCreateDto> createDtoValidator, IMapper mapper)
        {
            _uow = uow;
            _createDtoValidator = createDtoValidator;
            _mapper = mapper;
        }

        public async Task<IResponse<AdvertisementAppUserCreateDto>> CreateAsync(AdvertisementAppUserCreateDto dto)
        {
            var result = _createDtoValidator.Validate(dto);
            if (result.IsValid)
            {
                var control = await _uow.GetRespository<AdvertisementAppUser>().GetByFilterAsync(x => x.AppUserId == dto.AppUserId
                && x.AdvertisementId == dto.AdvertisementId);
                //ilgili bussiness logic çalışacak
                if(control == null)
                {
                    var createdAdvertisementAppUser = _mapper.Map<AdvertisementAppUser>(dto);//dto'yu entitye çeviridim CreateAsync entity istiyor.
                    await _uow.GetRespository<AdvertisementAppUser>().CreateAsync(createdAdvertisementAppUser);
                    await _uow.SaveChangesAsync();
                    return new Response<AdvertisementAppUserCreateDto>(ResponseType.Successs, dto);
                }
                List<CustomValidationError> errors = new List<CustomValidationError> { new CustomValidationError
                {
                    ErrorMessage="Bu ilana hali hazırda başvurunuz bulunmaktadır.",
                    PropertyName=""
                } };
                return new Response<AdvertisementAppUserCreateDto>(dto,errors);

            }
            return new Response<AdvertisementAppUserCreateDto>(dto, result.ConverToCustomValidationError());
        }

        public async Task<List<AdvertisementAppUserListDto>> GetList(AdvertisementAppUserStatusType type)
        {
            var query = _uow.GetRespository<AdvertisementAppUser>().GetQuery();//eager loading

            var list = await query.Include(x => x.Advertisement).Include(x => x.AdvertisementAppUserStatus).Include(x => x.MilitaryStatus)
                .Include(x => x.MilitaryStatus).Include(x => x.AppUser).ThenInclude(x=>x.Gender).
                Where(x => x.AdvertisementAppUserStatusId == (int)type).ToListAsync();

            return _mapper.Map<List<AdvertisementAppUserListDto>>(list);
        }
        public async Task SetStatusAsync(int advertisementAppUserId, AdvertisementAppUserStatusType type)
        {

            var query = _uow.GetRespository<AdvertisementAppUser>().GetQuery();

            var entity = await query.SingleOrDefaultAsync(x => x.Id == advertisementAppUserId);
            entity.AdvertisementAppUserStatusId = (int)type;
            await _uow.SaveChangesAsync();
        }
    }

}
