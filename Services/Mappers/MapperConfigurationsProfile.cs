using AutoMapper;
using DTO.Course;
using DTO.User;
using Repositories.Models;
using System.Runtime.CompilerServices;

namespace Services.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<LoginUserDTO, User>();
            CreateMap<RegisterUserDTO, User>();
            CreateMap<UpdateProfileDTO, User>();

            CreateMap<Course, CourseDto>();
            CreateMap<CreateCourseRequestDto, Course>();
            CreateMap<UpdateCourseRequestDto, Course>();

        }


        //public static CourseDto ToCourseDto(this Course courseModel)
        //{
        //    return new CourseDto
        //    {
        //        CourseId = courseModel.CourseId,
        //        Title = courseModel.Title,
        //        Description = courseModel.Description,
        //        Duration = courseModel.Duration,
        //        CreatedAt = courseModel.CreatedAt,
        //        VideoUrl = courseModel.VideoUrl,
        //        DocumentContent = courseModel.DocumentContent
        //    };
        //}

        //public static Course ToCourseFromCreateDto(this CreateStockRequest stockRequest)
        //{
        //    return new Course
        //    {
        //        Title = stockRequest.Title,
        //        Description = stockRequest.Description,
        //        Duration = stockRequest.Duration,
        //        VideoUrl = stockRequest.VideoUrl,
        //        DocumentContent = stockRequest.DocumentContent
        //    };
        //}

        //public static Course ToCourseFromUpdateDto(this UpdateCourseDto update)
        //{
        //    return new Course
        //    {
        //        Title = update.Title,
        //        Description = update.Description,
        //        Duration = update.Duration,
        //        VideoUrl = update.VideoUrl,
        //        DocumentContent = update.DocumentContent
        //    };
        //}
    }
}
