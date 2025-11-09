using H_application.DTOs.ReviewsDto;
using H_application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.Repository
{
    public class ReviewsRepositoy : IReviewsService
    {
        private readonly EntityContext _context;

        public ReviewsRepositoy(EntityContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReview(ReviewsDtoCreate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<Review>();
            await _context.Reviews.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteReview(ReviewsDtoUpdate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<Review>();
             _context.Reviews.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ReviewsResponse>> GetAllReview(CancellationToken cancellationToken = default)
        {
            var review = await _context.Reviews
                .OrderBy(r => r.Rating)
                .Include(r => r.Guest)
                .Include(r => r.Reservation)
                .AsNoTracking()
                .ToListAsync();
            var entity = review.Adapt<List<ReviewsResponse>>();
            return entity;
        }

        public async Task<ReviewsDtoUpdate> GetReviewById(int Id, CancellationToken cancellationToken = default)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == Id, cancellationToken);
            var entity =  review.Adapt<ReviewsDtoUpdate>();
            return entity;
        }

        public async Task<bool> UpdateReview(ReviewsDtoUpdate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<Review>();
             _context.Reviews.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
