using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Dtos.Photo
{
    public class PhotosForDetail:PhotoBase
    {
        public int Id { get; set; } 
        public bool IsMain { get; set; }
        public bool IsApproved { get; set; }
    }
}
