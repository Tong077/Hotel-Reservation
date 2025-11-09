using H_application.DTOs.ReservationDto;
using H_application.DTOs.ReviewsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.Service
{
    public interface IReviewsService
    {
        Task<bool> CreateReview(ReviewsDtoCreate dto,CancellationToken cancellationToken = default);
        Task<bool> UpdateReview(ReviewsDtoUpdate dto,CancellationToken cancellationToken = default);
        Task<bool> DeleteReview(ReviewsDtoUpdate dto,CancellationToken cancellationToken = default);
        Task<ReviewsDtoUpdate> GetReviewById(int Id,CancellationToken cancellationToken = default);
        Task<IEnumerable<ReviewsResponse>> GetAllReview(CancellationToken cancellationToken = default);
    }
}
