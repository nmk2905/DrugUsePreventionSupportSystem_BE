using AutoMapper;
using DTO.Course;
using DTO.CourseCategory;
using DTO.User;
using Repositories.Models;

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

            CreateMap<Course, CourseDto>().ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category)).ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CategoryNavigation));
            CreateMap<CreateCourseRequestDto, Course>();
            CreateMap<UpdateCourseRequestDto, Course>();

            CreateMap<CourseCategory, CourseCategoryDto>();
            CreateMap<CreateCourseCategoryRequestDto, CourseCategory>();

        }

    }
}
