using AutoMapper;
using DTO.Course;
using DTO.CourseCategory;
using DTO.CourseQuestion;
using DTO.CourseQuestion.CourseOptions;
using Repositories.Models;

namespace Services.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CategoryNavigation))
                .ForMember(dest => dest.CourseQuestions, opt => opt.MapFrom(src => src.CourseQuestions));

			CreateMap<CreateCourseRequestDto, Course>();
            CreateMap<UpdateCourseRequestDto, Course>();

            CreateMap<CourseCategory, CourseCategoryDto>();
            CreateMap<CreateCourseCategoryRequestDto, CourseCategory>();

			CreateMap<CourseQuestion, CourseQuestionDto>()
                .ForMember(dest => dest.CourseQuestionOptions, opt => opt.MapFrom(src => src.CourseQuestionOptions));
			CreateMap<CreateQuestionRequestDto, CourseQuestion>();
			CreateMap<CourseQuestion, CreateQuestionRequestDto>();

            CreateMap<CourseQuestionOption, CourseQuestionOptionDto>();
            CreateMap<CreateCourseOptionRequestDto, CourseQuestionOption>().ReverseMap();
            CreateMap<UpdateCourseOptionRequestDto, CourseQuestionOption>().ReverseMap();

		}

    }
}
