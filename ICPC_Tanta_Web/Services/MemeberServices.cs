using Core.DTO.memberDTO;
using Core.DTO.TeamDTO;
using Core.Entities;
using Core.IRepositories;
using Core.IServices;

namespace ICPC_Tanta_Web.Services
{
    public class MemeberServices : IMemeberServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileProcessingService _fileProcessingService;

        public MemeberServices(IUnitOfWork unitOfWork, IFileProcessingService fileProcessingService)
        {
            _unitOfWork = unitOfWork;
            _fileProcessingService = fileProcessingService;
        }
        public async Task AddAsync(CreatMemberDto member)
        {
            var newmember = new Member
            {
                FullName = member.FullName,
                Role = member.Role,
                FacebookUrl = member.FacebookUrl,
                LinkedInUrl = member.LinkedInUrl,
                TeamId = member.TeamId,
                YearJoin = member.YearJoin,

            };
            if (member.MemberImg != null)
            {
                newmember.ImgUrl = await _fileProcessingService.SaveFileAsync(member.MemberImg);
            }
            await _unitOfWork.MemberRepository.AddAsync(newmember);
            await _unitOfWork.SaveChangesAsync();
            
        }


        public async Task<IEnumerable<memberDto>> GetAllMemberAsync()
        {
            var member = _unitOfWork.MemberRepository.GetAll();
            var memberdto = member.Select(m => new memberDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Role = m.Role,
                FacebookUrl = m.FacebookUrl??null,
                LinkedInUrl = m.LinkedInUrl??null,
                ImgUrl = m.ImgUrl ??null
            });
            return memberdto;
        }

        public async Task<memberDto> GetMemberByIdAsync(int id)
        {
            var member=_unitOfWork.MemberRepository.GetById(id);
            if (member == null) throw new KeyNotFoundException("member not found");
            var memberdto = new memberDto
            {
                Id=member.Id,
                FullName = member.FullName,
                Role = member.Role,
                FacebookUrl = member.FacebookUrl ?? null,
                LinkedInUrl = member.LinkedInUrl ?? null,
                ImgUrl = member.ImgUrl ?? null
            };
            return memberdto;
        }

        public async Task UpdateAsync(memberUpdateDto memberUpdate)
        {
            var member = await _unitOfWork.MemberRepository.GetByIdAsync(memberUpdate.Id);
            if (member == null) throw new KeyNotFoundException("Member not found");
            member.Role = memberUpdate.Role;
            if (memberUpdate.MemberImg != null)
            {
                if (!string.IsNullOrEmpty(member.ImgUrl))
                {
                    _fileProcessingService.DeleteFile(member.ImgUrl);
                }

                member.ImgUrl = await _fileProcessingService.SaveFileAsync(memberUpdate.MemberImg);
            }
            _unitOfWork.MemberRepository.Update(member);
            await _unitOfWork.SaveChangesAsync();

        }
        
        public async Task DeleteAsync(int id)
        {
            var member = await _unitOfWork.MemberRepository.GetByIdAsync(id);
            if (member == null) throw new KeyNotFoundException("Member not found");

            if (member.ImgUrl != null)
            {
                _fileProcessingService.DeleteFile(member.ImgUrl);
            }
            _unitOfWork.MemberRepository.Delete(member);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
