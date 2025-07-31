using AutoMapper;
using TeamProjectManagement.DTOs;
using TeamProjectManagement.Models;

namespace TeamProjectManagement.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            // WorkItem (Task) mappings
            CreateMap<WorkItem, TaskDto>()
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedTo))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.Epic, opt => opt.MapFrom(src => src.Epic))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.AttachmentCount, opt => opt.MapFrom(src => src.Attachments.Count));

            CreateMap<WorkItem, TaskListDto>()
                .ForMember(dest => dest.AssignedToName, opt => opt.MapFrom(src => 
                    src.AssignedTo != null ? $"{src.AssignedTo.FirstName} {src.AssignedTo.LastName}" : "Unassigned"))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => 
                    $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}"));

            CreateMap<CreateTaskDto, WorkItem>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enums.WorkItemStatus.ToDo));

            // Epic mappings
            CreateMap<Epic, EpicDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.Tasks.Count))
                .ForMember(dest => dest.CompletedTaskCount, opt => opt.MapFrom(src => 
                    src.Tasks.Count(t => t.Status == Enums.WorkItemStatus.Done)));

            CreateMap<Epic, EpicListDto>()
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => 
                    $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}"))
                .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.Tasks.Count))
                .ForMember(dest => dest.CompletedTaskCount, opt => opt.MapFrom(src => 
                    src.Tasks.Count(t => t.Status == Enums.WorkItemStatus.Done)));

            CreateMap<CreateEpicDto, Epic>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enums.WorkItemStatus.ToDo));

            // RefreshToken mappings
            CreateMap<RefreshToken, RefreshTokenDto>();
        }
    }
} 