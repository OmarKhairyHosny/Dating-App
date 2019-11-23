using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Dtos.Photo
{
    public class PhotoForCreation:PhotoBase
    {
       
        public string PublicId { get; set; }
        public IFormFile File { get; set; }
        public PhotoForCreation()
        {
            DateAdded = DateTime.Now;
        }
    }
}
