using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IJwtTokenGenerator
    {
        (string Token, DateTime ExpiresAt) GenerateToken(User user);
    }
}