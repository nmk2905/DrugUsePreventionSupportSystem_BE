using DTO.UserAssessment;
using Repositories;
using Repositories.Models;

namespace Services
{
    public interface IUserAssessmentService
    {
        Task<AssessmentResultResponse> SubmitAssessmentAsync(int userId, int assessmentId, List<int> selectedOptionIds);
    }

    public class UserAssessmentService : IUserAssessmentService
    {
        private readonly UserAssessmentRepository _repo;

        public UserAssessmentService()
        {
            _repo = new UserAssessmentRepository();
        }

        public async Task<AssessmentResultResponse> SubmitAssessmentAsync(int userId, int assessmentId, List<int> selectedOptionIds)
        {
            var totalScore = await _repo.CalculateTotalScore(selectedOptionIds);

            int riskLevelCode = totalScore switch
            {
                <= 5 => 1, // Low
                <= 10 => 2, // Medium
                _ => 3      // High
            };

            string riskLevelName = riskLevelCode switch
            {
                1 => "Low",
                2 => "Medium",
                3 => "High",
                _ => "Unknown"
            };

            string recommendation = riskLevelCode switch
            {
                1 => "Bạn nên tham gia khóa học online.",
                2 or 3 => "Bạn nên đặt lịch tư vấn với chuyên gia.",
                _ => "Không có gợi ý phù hợp."
            };

            var ua = new UserAssessment
            {
                UserId = userId,
                AssessmentId = assessmentId,
                CompletedTime = DateTime.UtcNow,
                Score = totalScore,
                RiskLevel = riskLevelCode 
            };

            await _repo.CreateAsync(ua);

            return new AssessmentResultResponse
            {
                TotalScore = totalScore,
                RiskLevel = riskLevelName,
                Recommendation = recommendation
            };
        }
    }

}
