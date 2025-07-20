using Core.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IContactUsServices
    {
        Task HandleContactUsAsync(ContactUsDto dto);
    }
}
