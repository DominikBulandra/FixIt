using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Naprawiam.Controllers;

namespace Naprawiam.Repositories
{
    public class GuideRepository :Repository<Guide>
    {
        static GuideRepository()
        {
            _data.Add(new Guide() { Id = 1, Adres = "wwww.naprawy.pl", Desc = "naprawa czegos", UserId = 24 });
        }
    }
}