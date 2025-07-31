using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TeamProjectManagement.Data;
using TeamProjectManagement.DTOs;
using TeamProjectManagement.Enums;
using TeamProjectManagement.Models;
using TeamProjectManagement.Services.Interfaces;

namespace TeamProjectManagement.Services.Repositories
{
    public class EpicService : IEpicService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EpicService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EpicListDto>> GetAllEpicsAsync()
        {
            var epics = await _context.Epics
                .Include(e => e.CreatedBy)
                .Include(e => e.Tasks)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EpicListDto>>(epics);
        }

        public async Task<EpicDto?> GetEpicByIdAsync(int id)
        {
            var epic = await _context.Epics
                .Include(e => e.CreatedBy)
                .Include(e => e.Tasks)
                .FirstOrDefaultAsync(e => e.Id == id);

            return _mapper.Map<EpicDto>(epic);
        }

        public async Task<EpicDto> CreateEpicAsync(CreateEpicDto createEpicDto, int createdById)
        {
            var epic = _mapper.Map<Epic>(createEpicDto);
            epic.CreatedById = createdById;

            _context.Epics.Add(epic);
            await _context.SaveChangesAsync();

            return await GetEpicByIdAsync(epic.Id) ?? throw new InvalidOperationException("Failed to create epic");
        }

        public async Task<EpicDto?> UpdateEpicAsync(int id, UpdateEpicDto updateEpicDto)
        {
            var epic = await _context.Epics.FindAsync(id);
            if (epic == null)
                return null;

            if (updateEpicDto.Title != null)
                epic.Title = updateEpicDto.Title;

            if (updateEpicDto.Description != null)
                epic.Description = updateEpicDto.Description;

            if (updateEpicDto.DueDate.HasValue)
                epic.DueDate = updateEpicDto.DueDate;

            if (updateEpicDto.Status.HasValue)
                epic.Status = updateEpicDto.Status.Value;

            if (updateEpicDto.Priority.HasValue)
                epic.Priority = updateEpicDto.Priority.Value;

            await _context.SaveChangesAsync();

            return await GetEpicByIdAsync(epic.Id);
        }

        public async Task<bool> DeleteEpicAsync(int id)
        {
            var epic = await _context.Epics.FindAsync(id);
            if (epic == null)
                return false;

            _context.Epics.Remove(epic);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EpicListDto>> GetEpicsByStatusAsync(WorkItemStatus status)
        {
            var epics = await _context.Epics
                .Include(e => e.CreatedBy)
                .Include(e => e.Tasks)
                .Where(e => e.Status == status)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EpicListDto>>(epics);
        }

        public async Task<IEnumerable<EpicListDto>> GetEpicsByCreatorAsync(int creatorId)
        {
            var epics = await _context.Epics
                .Include(e => e.CreatedBy)
                .Include(e => e.Tasks)
                .Where(e => e.CreatedById == creatorId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EpicListDto>>(epics);
        }

        public async Task<IEnumerable<EpicListDto>> GetEpicsByPriorityAsync(TaskPriority priority)
        {
            var epics = await _context.Epics
                .Include(e => e.CreatedBy)
                .Include(e => e.Tasks)
                .Where(e => e.Priority == priority)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EpicListDto>>(epics);
        }
    }
}