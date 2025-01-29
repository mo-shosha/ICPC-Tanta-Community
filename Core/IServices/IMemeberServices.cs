using Core.DTO.memberDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IMemeberServices
    {
        Task AddAsync(CreatMemberDto member);
        Task<IEnumerable<memberDto>> GetAllMemberAsync();
        Task<memberDto> GetMemberByIdAsync(int id);
        
    }
}
