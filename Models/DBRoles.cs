using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubPortalMS.Models
{
    public class DBRoles
    {
   
        public int ID { get; set; }
        public string Name { get; set; }
        public string MoTa { get; set; }
    
        public virtual ICollection<DBUserRoles> DBUserRoles { get; set; }
    }


}