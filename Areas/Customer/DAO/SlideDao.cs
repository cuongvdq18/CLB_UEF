using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ClubPortalMS.Models;

namespace ClubPortalMS.Areas.Customer.DAO
{
    public class SlideDao
    {
        ApplicationDbContext db = null;
        public SlideDao()
        {
            db = new ApplicationDbContext();
        }

        public List<Poster> ListAll()
        {
            return db.Poster.Where(x => x.Status == true).OrderBy(y => y.ID).ToList();
        }
    }
}