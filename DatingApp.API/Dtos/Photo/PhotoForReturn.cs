using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Dtos.Photo
{
    public class PhotoForReturn:PhotoBase
    {
        public int Id { get; set; }
        public string PublicId { get; set; }
        public bool IsMain { get; set; }
    }
}
