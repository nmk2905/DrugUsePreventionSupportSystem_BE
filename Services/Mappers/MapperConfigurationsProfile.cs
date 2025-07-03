using AutoMapper;
using DTO.Course;
using DTO.CourseCategory;
using DTO.CourseQuestion;
using DTO.CourseQuestion.CourseOptions;
using DTO.UserAnswer;
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

            CreateMap<UserAnswer, UserAnswerDto>()
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.OptionId, opt => opt.MapFrom(src => src.OptionId))
                .ForMember(dest => dest.TotalPoint, opt => opt.MapFrom(src => src.TotalPoint));
            CreateMap<SubmitAnswerRequestDto, UserAnswer>().ReverseMap();

		}

    }
}
