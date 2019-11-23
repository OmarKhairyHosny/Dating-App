using DatingApp.API.Dtos.Photo;
using DatingApp.API.Dtos.User;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Dtos
{
    public class UserForDetail: UserBase
    { 
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; } 
        public ICollection<PhotosForDetail> Photos { get; set; }
    }
}
