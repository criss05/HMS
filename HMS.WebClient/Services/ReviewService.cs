using HMS.Shared.DTOs;
using HMS.Shared.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.WebClient.Services
{
    public class ReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByDoctorIdAsync(int doctorId)
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return reviews.Where(r => r.DoctorId == doctorId).ToList();
        }

        public async Task<double> GetAverageRatingForDoctorAsync(int doctorId)
        {
            var reviews = await _reviewRepository.GetAllAsync();
            var doctorReviews = reviews.Where(r => r.DoctorId == doctorId).ToList();

            if (!doctorReviews.Any())
                return 0;

            return doctorReviews.Average(r => r.Value);
        }

        public async Task<bool> CreateReviewAsync(ReviewDto review)
        {
            var result = await _reviewRepository.AddAsync(review);
            return result != null;
        }
    }
}