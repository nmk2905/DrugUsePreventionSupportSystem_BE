using DTO.UserAssessment;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Models;

namespace Services
{
    public interface IUserAssessmentService
    {
        Task<UserAssessment> SubmitAssessmentAsync(int userId, int assessmentId, List<int> selectedOptionIds);
    }
    public class UserAssessmentService : IUserAssessmentService
    {
        private readonly UserAssessmentRepository _userRepo;
        private readonly AssessmentOptionRepository _optionRepo;
        private readonly RiskLevelRepository _riskRepo;

        public UserAssessmentService()
        {
            _userRepo = new UserAssessmentRepository();
            _optionRepo = new AssessmentOptionRepository();
            _riskRepo = new RiskLevelRepository();
        }

        private int GetRiskLevelIdFromScore(decimal score)
        {
            if (score <= 6) return 1;
            if (score <= 13) return 2;
            return 3;
        }

        public async Task<UserAssessment> SubmitAssessmentAsync(int userId, int assessmentId, List<int> selectedOptionIds)
        {
            var totalScore = await _optionRepo.CalculateTotalScore(selectedOptionIds);
            var riskLevelId = GetRiskLevelIdFromScore(totalScore);

            var result = new UserAssessment
            {
                UserId = userId,
                AssessmentId = assessmentId,
                Score = totalScore,
                RiskLevel = riskLevelId,
                CompletedTime = DateTime.UtcNow
            };

            await _userRepo.CreateAsync(result);
            await _userRepo.SaveAsync();

            result.RiskLevelNavigation = await _riskRepo.GetByIdAsync(riskLevelId);

            return result;
        }
    }


}
