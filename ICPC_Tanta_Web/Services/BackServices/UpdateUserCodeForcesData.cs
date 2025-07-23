using Core.Entities.Identity;
using Core.IServices;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ICPC_Tanta_Web.Services.BackServices
{
    public class UpdateUserCodeForcesData 
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICodeforcesService _codeforcesService;

        private readonly string _logFilePath = "Logs/UpdateUserCodeForcesData.log";

        public UpdateUserCodeForcesData(
            UserManager<ApplicationUser> userManager,
            ICodeforcesService codeforcesService)
        {
            _userManager = userManager;
            _codeforcesService = codeforcesService;

            var logDir = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
        }

        public async Task UpdateAllUsersAsync()
        {
            var users = _userManager.Users
                .Where(u => !string.IsNullOrEmpty(u.CodeForcesHandel))
                .ToList();

            foreach (var user in users)
            {
                try
                {
                    var codeforcesUserInfo = await _codeforcesService.GetUserInfoAsync(user.CodeForcesHandel);
                    if (codeforcesUserInfo != null)
                    {
                        user.Rank = codeforcesUserInfo.Rank;
                        user.Rating = codeforcesUserInfo.Rating;
                        user.TitlePhoto = codeforcesUserInfo.TitlePhoto;
                        await _userManager.UpdateAsync(user);
                    }
                }
                catch (Exception ex)
                {
                    LogError(user.Email, ex.Message);
                }
            }
        }

        private void LogError(string userEmail, string errorMessage)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [UpdateUserCodeForcesData] " +
                              $"User: {userEmail} | Error: {errorMessage}{Environment.NewLine}";

            File.AppendAllText(_logFilePath, logEntry);
        }
    }
}
