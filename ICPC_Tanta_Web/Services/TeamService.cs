using Core.DTO.memberDTO;
using Core.DTO.TeamDTO;
using Core.Entities;
using Core.IRepositories;
using Core.IServices;
using ICPC_Tanta_Web.DTO.NewsDTO;

namespace ICPC_Tanta_Web.Services
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileProcessingService _fileProcessingService;

        public TeamService(IUnitOfWork unitOfWork, IFileProcessingService fileProcessingService)
        {
            _unitOfWork = unitOfWork;
            _fileProcessingService = fileProcessingService;
        }
        public async Task AddAsync(CreateTeamDTO createTeamDto)
        {
            var team = new Team
            {
                TeamName = createTeamDto.TeamName,
                Description = createTeamDto.Description,

            };
            if (createTeamDto.LogoImg != null)
            {
                team.LogoURL = await _fileProcessingService.SaveFileAsync(createTeamDto.LogoImg);
            }

            await _unitOfWork.TeamRepository.AddAsync(team);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var team = await _unitOfWork.TeamRepository.GetByIdAsync(id);
            if (team == null) throw new KeyNotFoundException("Team not found");

            // Delete image if exists
            if (!string.IsNullOrEmpty(team.LogoURL))
            {
                _fileProcessingService.DeleteFile(team.LogoURL);
            }

            _unitOfWork.TeamRepository.Delete(team);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<TeamDTO>> GetAllAsync()
        {
            var teams = await _unitOfWork.TeamRepository.GetAllAsync();

            // Manual mapping
            var teamDTOs = teams.Select(team => new TeamDTO
            {
                Id = team.Id,
                TeamName = team.TeamName,
                Description = team.Description,
                LogoURL = team.LogoURL ?? "default.jpg"
            });

            return teamDTOs;
        }

        public async Task<TeamWithMemberDto> GetAllByMember(int id)
        {
            var teams = await _unitOfWork.TeamRepository.AllTeamWithMember();

            var selectedTeam = teams.FirstOrDefault(team => team.Id == id);

            if (selectedTeam == null)
            {
                return null;  
            }

            return new TeamWithMemberDto
            {
                Id = selectedTeam.Id,
                TeamName = selectedTeam.TeamName,
                Description = selectedTeam.Description,
                LogoURL = selectedTeam.LogoURL ?? "default.jpg",
                Members = selectedTeam.Members?.Select(m => new memberDto
                {
                    Id=m.Id,
                    FullName = m.FullName,
                    LinkedInUrl = m.LinkedInUrl,
                    FacebookUrl = m.FacebookUrl,
                    Role = m.Role,
                    ImgUrl = m.ImgUrl
                }).ToList()
            };
        }

        public async Task<TeamWithMemberDto> GetAllByMemberByYear(int teamId, string year)
        {
            var team = await _unitOfWork.TeamRepository.AllTeamWithMember();
            var selectedTeam = team.FirstOrDefault(t => t.Id == teamId);
            if (selectedTeam == null)
            {
                return null;
            }

            return new TeamWithMemberDto
            {
                Id = selectedTeam.Id,
                TeamName = selectedTeam.TeamName,
                Description = selectedTeam.Description,
                LogoURL = selectedTeam.LogoURL ?? "default.jpg",
                Members = selectedTeam.Members?
                    .Where(m => m.YearJoin == year)  
                    .Select(m => new memberDto
                    {
                        Id = m.Id,
                        FullName = m.FullName,
                        LinkedInUrl = m.LinkedInUrl,
                        FacebookUrl = m.FacebookUrl,
                        Role = m.Role,
                        ImgUrl=m.ImgUrl
                    }).ToList()
            };
        }



        public async Task<TeamDTO> GetByIdAsync(int id)
        {
            var team = await _unitOfWork.TeamRepository.GetByIdAsync(id);
            if (team == null) throw new KeyNotFoundException("Team not found");

            // Manual mapping
            return new TeamDTO
            {
                Id = team.Id,
                TeamName = team.TeamName,
                Description = team.Description,
                LogoURL = team.LogoURL ?? "default.jpg"
            };
        }

        public async Task UpdateAsync(UpdateTeamDto updateTeamsDto)
        {
            var team = await _unitOfWork.TeamRepository.GetByIdAsync(updateTeamsDto.Id);
            if (team == null) throw new KeyNotFoundException("Team not found");

            // Update  fields
            team.TeamName = updateTeamsDto.TeamName;
            team.Description = updateTeamsDto.Description;

            // Update the logo if a new file is provided
            if (updateTeamsDto.LogoImg != null)
            {
                if (!string.IsNullOrEmpty(team.LogoURL))
                {
                    _fileProcessingService.DeleteFile(team.LogoURL);
                }

                team.LogoURL = await _fileProcessingService.SaveFileAsync(updateTeamsDto.LogoImg);
            }

            _unitOfWork.TeamRepository.Update(team);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
