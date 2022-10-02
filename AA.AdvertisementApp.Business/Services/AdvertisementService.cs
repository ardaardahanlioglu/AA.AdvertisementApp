using AA.AdvertisementApp.Business.Interfaces;
using AA.AdvertisementApp.Common;
using AA.AdvertisementApp.DataAccess.UnitOfWork;
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
    public class AdvertisementService : Service<AdvertisementCreateDto, AdvertisementUpdateDto,
        AdvertisementListDto, Advertisement>, IAdvertisementService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        //IService içindeki metotlar Service içerisinde implemente olduğunda burada implemente etmemize gerek yok.
        public AdvertisementService(IMapper mapper, IValidator<AdvertisementCreateDto> createValidator,
            IValidator<AdvertisementUpdateDto> updateVadalitor, IUow uow) :
            base(mapper, createValidator, updateVadalitor, uow)
        {
            _uow = uow;
            _mapper = mapper;
        }
        //Burada belli AdvertisemntService'a özel bir metot yazacağız bunun için uow'den veri çekmem lazım
        //uow'de constructor'dan ulşamıyorum.
        public async Task<IResponse<List<AdvertisementListDto>>> GetActivesAsync()
        {
            var data = await _uow.GetRespository<Advertisement>().GetAllAsync(x => x.Status, x => x.CreatedDate,
            Common.Enums.OrderByType.DESC);
            var dto = _mapper.Map<List<AdvertisementListDto>>(data);//gelen datayı AdvertisementListDto'ya çevireceğim.

            return new Response<List<AdvertisementListDto>>(ResponseType.Successs, dto);
        }
    }
}
