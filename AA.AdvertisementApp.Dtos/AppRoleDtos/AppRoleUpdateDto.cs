using AA.AdvertisementApp.Dtos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.AdvertisementApp.Dtos
{
    public class AppRoleUpdateDto : IDto
    {
        public int Id { get; set; }
        public string Definition { get; set; }
    }
}
