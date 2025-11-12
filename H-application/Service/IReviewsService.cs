using H_application.DTOs.ReviewsDto;

namespace H_application.Service
{
    public interface IReviewsService
    {
        Task<bool> CreateReview(ReviewsDtoCreate dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateReview(ReviewsDtoUpdate dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteReview(ReviewsDtoUpdate dto, CancellationToken cancellationToken = default);
        Task<ReviewsDtoUpdate> GetReviewById(int Id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ReviewsResponse>> GetAllReview(CancellationToken cancellationToken = default);
    }
}
