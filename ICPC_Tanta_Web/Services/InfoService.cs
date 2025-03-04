using Core.DTO.InfoDTO;
using Core.Entities;
using Core.IRepositories;
using Core.IServices;

namespace ICPC_Tanta_Web.Services
{
    public class InfoService: IInfoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileProcessingService _fileProcessingService;
        public InfoService(IUnitOfWork unitOfWork,IFileProcessingService fileProcessingService)
        {
            _unitOfWork = unitOfWork;
            _fileProcessingService = fileProcessingService;
        }

        public async Task AddAsync(CreateInfoDto createInfoDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createInfoDto.FacebookUrl) ||
                                    string.IsNullOrWhiteSpace(createInfoDto.YoutubeUrl) ||
                                    string.IsNullOrWhiteSpace(createInfoDto.LinkedInUrl) ||
                                    createInfoDto.Image == null)
                {
                    throw new ArgumentException("All required fields must be provided.");
                }

                var existingInfo = (await _unitOfWork.infoRepository.GetAllAsync()).FirstOrDefault();
                if (existingInfo != null)
                {
                    throw new InvalidOperationException("Info already exists. Use update instead.");
                }

                var imageUrl = await _fileProcessingService.SaveFileAsync(createInfoDto.Image);

                var newInfo = new info
                {
                    FacebookUrl = createInfoDto.FacebookUrl,
                    YoutubeUrl = createInfoDto.YoutubeUrl,
                    TwitterUrl = createInfoDto.TwitterUrl,
                    InstagramUrl = createInfoDto.InstagramUrl,
                    LinkedInUrl = createInfoDto.LinkedInUrl,
                    BackGroundUrl = imageUrl
                };

                await _unitOfWork.infoRepository.AddAsync(newInfo);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding info: {ex.Message}");
            }
        }

        public async Task<InfoDto> GetInfAsync()
        {
            var info = await _unitOfWork.infoRepository.GetAllAsync();
            var websiteInfo = info.FirstOrDefault();
            if (websiteInfo == null) return null;

            return new InfoDto
            {
                FacebookUrl = websiteInfo.FacebookUrl,
                YoutubeUrl = websiteInfo.YoutubeUrl,
                TwitterUrl = websiteInfo.TwitterUrl,
                InstagramUrl = websiteInfo.InstagramUrl,
                LinkedInUrl = websiteInfo.LinkedInUrl,
                BackGroundUrl = websiteInfo.BackGroundUrl
            };
        }

        public async Task UpdateAsync(UpdateInfoDto updateInfoDto)
        {
            try
            {
                var existingInfo = (await _unitOfWork.infoRepository.GetAllAsync()).FirstOrDefault();
                if (existingInfo == null)
                {
                    throw new KeyNotFoundException("Info record not found.");
                }
                existingInfo.FacebookUrl = updateInfoDto.FacebookUrl ?? existingInfo.FacebookUrl;
                existingInfo.YoutubeUrl = updateInfoDto.YoutubeUrl ?? existingInfo.YoutubeUrl;
                existingInfo.TwitterUrl = updateInfoDto.TwitterUrl ?? existingInfo.TwitterUrl;
                existingInfo.InstagramUrl = updateInfoDto.InstagramUrl ?? existingInfo.InstagramUrl;
                existingInfo.LinkedInUrl = updateInfoDto.LinkedInUrl ?? existingInfo.LinkedInUrl;
                if (updateInfoDto.Image != null)
                {
                    _fileProcessingService.DeleteFile(existingInfo.BackGroundUrl);
                    var newImageUrl = await _fileProcessingService.SaveFileAsync(updateInfoDto.Image, "uploads");
                    existingInfo.BackGroundUrl = newImageUrl;
                }

                _unitOfWork.infoRepository.Update(existingInfo);
                await _unitOfWork.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                throw new Exception($"An error occurred while updating info: {ex.Message}");
            }
        }
    }
}
