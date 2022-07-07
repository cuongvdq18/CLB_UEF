using System.Collections.Generic;

namespace ClubPortalMS
{
    public class CustomSerializeModel
    { 
        public int ID { get; set; } 
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public List<string> RoleName { get; set; } 
    }
}