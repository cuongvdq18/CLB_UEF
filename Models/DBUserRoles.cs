using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubPortalMS.Models
{
    public class DBUserRoles
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        [ForeignKey("RoleID")]
        public virtual DBRoles DBRoles { get; set; }
        [ForeignKey("UserID")]
        public virtual DBUser DBUser { get; set; }
    }
}