using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClubPortalMS.Models;
using PagedList;

namespace ClubPortalMS.Areas.Customer.DAO
{
    public class ListDAO
    {
        ApplicationDbContext db = null;
        public ListDAO()
        {
            db = new ApplicationDbContext();
        }
        public IPagedList<HoatDong> ListAllHD(int? page)
        {
            return db.HoatDong.OrderByDescending(x => x.ID).ToList().ToPagedList(page ?? 1, 3);
        }
        public IPagedList<HoatDong> ListAllHDs(int? page)
        {
            return db.HoatDong.OrderByDescending(x => x.ID).ToList().ToPagedList(page ?? 1, 6);
        }
        public List<HoatDong> listHD(int top)
        {
            return db.HoatDong.OrderByDescending(x => x.ID).Take(top).ToList();
        }
        public IPagedList<TinTuc> ListAllNews(int? page)
        {
            return db.TinTucs.OrderByDescending(x => x.ID).ToList().ToPagedList(page ?? 1, 3);
        }
        public IPagedList<TinTuc> ListAllNew(int? page)
        {
            return db.TinTucs.OrderByDescending(x => x.ID).ToList().ToPagedList(page ?? 1, 6);
        }
        public List<TinTuc> ListNews(int top)
        {
            return db.TinTucs.OrderByDescending(x => x.ID).Take(top).ToList();
        }
        public List<Album> ListAllAlbums()
        {
            return db.Album.OrderByDescending(x => x.ID).ToList();
        }
        public List<Album> ListAlbums(int top)
        {
            return db.Album.OrderByDescending(x => x.ID).Take(top).ToList();
        }
    }
   
}