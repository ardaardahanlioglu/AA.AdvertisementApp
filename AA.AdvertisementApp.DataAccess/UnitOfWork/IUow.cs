using AA.AdvertisementApp.DataAccess.Interfaces;
using AA.AdvertisementApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.AdvertisementApp.DataAccess.UnitOfWork
{
    public interface IUow
    {
        IRepository<T> GetRespository<T>() where T : BaseEntity;

        Task SaveChangesAsync();
    }
}
