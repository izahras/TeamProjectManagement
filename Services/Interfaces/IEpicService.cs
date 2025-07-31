using TeamProjectManagement.DTOs;
using TeamProjectManagement.Enums;

namespace TeamProjectManagement.Services.Interfaces
{
    public interface IEpicService
    {
        Task<IEnumerable<EpicListDto>> GetAllEpicsAsync();
        Task<EpicDto?> GetEpicByIdAsync(int id);
        Task<EpicDto> CreateEpicAsync(CreateEpicDto createEpicDto, int createdById);
        Task<EpicDto?> UpdateEpicAsync(int id, UpdateEpicDto updateEpicDto);
        Task<bool> DeleteEpicAsync(int id);
        Task<IEnumerable<EpicListDto>> GetEpicsByStatusAsync(WorkItemStatus status);
        Task<IEnumerable<EpicListDto>> GetEpicsByCreatorAsync(int creatorId);
        Task<IEnumerable<EpicListDto>> GetEpicsByPriorityAsync(TaskPriority priority);
    }
}