using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ReviewSystem.DTOs;
using ReviewSystem.Models;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;

namespace ReviewSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AppController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //New User

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost("user")]
        public async Task<ActionResult<User>> CreateUser(UserDtoCreate userDtoCreate)
        {
            var user = _mapper.Map<User>(userDtoCreate);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsers), new { id = user.UserId }, user);
        }

        // New Product
        [HttpGet("product")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpPost("product")]
        public async Task<ActionResult<Product>> CreateProduct(ProductDtoCreate productDtoCreate)
        {
            var product = _mapper.Map<Product>(productDtoCreate);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducts), new { id = product.ProductId }, product);
        }


        // New Review
        [HttpGet("review")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            // Assuming ReviewSentiment is a string that can be "Positive", "Neutral", or "Negative"
            // This will sort reviews to have "Positive" first, followed by others
            var sortedReviews = await _context.Reviews
                                               .OrderByDescending(r => r.ReviewSentiment == "Positive")
                                               .ThenByDescending(r => r.ReviewSentiment == "Negative")
                                               .ToListAsync();
            return sortedReviews;
        }


        [HttpPost("review")]
        public async Task<ActionResult<Review>> CreateReview(ReviewDtoCreate reviewDtoCreate)
        {
            // Check if the user has already reviewed the product
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == reviewDtoCreate.UserId && r.ProductId == reviewDtoCreate.ProductId);

            if (existingReview != null)
            {
                return BadRequest("User has already reviewed this product.");
            }

            var review = _mapper.Map<Review>(reviewDtoCreate);

            // Ensure ReviewSentiment is NULL or unset to be picked up by the sentiment analysis service
            review.ReviewSentiment = null;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(); // Save the initial review

            string pythonServiceUrl = "http://127.0.0.1:5000/analyze-sentiments";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Notify the sentiment analysis service to process pending reviews
                    HttpResponseMessage response = await client.PostAsync(pythonServiceUrl, null);
                    response.EnsureSuccessStatusCode();

                    // The service updates the sentiments directly in the database
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request exception: {e.Message}");
                    // Decide how to handle errors
                }
            }

            // Optionally refetch the updated review to return the latest data
            var updatedReview = await _context.Reviews.FindAsync(review.ReviewId);

            return CreatedAtAction(nameof(GetReviews), new { id = updatedReview.ReviewId }, updatedReview);
        }


    }
}
