using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubPortalMS.Models
{
    public class DBUser
    {
        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Identifier { get; set; }
        public bool? EmailConfirmation { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public bool? IsLocked { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid ActivationCode { get; set; }
        public DateTime? NgayXoa { get; set; }
        public int? UserDeleted { get; set; }
        public virtual ICollection<ThanhVien> ThanhVien { get; set; }
        public virtual ICollection<DBUserRoles> DBUserRoles { get; set; }
    }
}