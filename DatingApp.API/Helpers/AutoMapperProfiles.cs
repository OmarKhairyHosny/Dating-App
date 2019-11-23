using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Dtos.Message;
using DatingApp.API.Dtos.Photo;
using DatingApp.API.Dtos.User;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForDetail>()
                .ForMember(des => des.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(des => des.Age, map => map.MapFrom((s, d) => s.DateOfBirth.CalculateAge()));
            
            CreateMap<User, UserForList>()
                .ForMember(des => des.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(des => des.Age, map => map.MapFrom((s, d) => s.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotosForDetail>();
            CreateMap<UserForUpdate, User>();
            CreateMap<Photo, PhotoForReturn>();
            CreateMap<PhotoForCreation, Photo>();
            CreateMap<UserForRegister, User>();

            CreateMap<MessageForCreation, Message>().ReverseMap();
            CreateMap<Message, MessageForReturn>()
                .ForMember(m => m.SenderPhotoUrl, opt => opt
                    .MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.RecipientPhotoUrl, opt => opt
                    .MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));
        }
    }
}
