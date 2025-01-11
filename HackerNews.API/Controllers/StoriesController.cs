using HackerNews.API.ControllerActionsExecutors;
using HackerNews.BusinessLogic.Models;
using HackerNews.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.API.Controllers
{
    /// <summary>
    /// The stories controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IStoriesService _storiesService;
        private readonly ControllerActionsExecutor _controllerActionsExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoriesController"/> class.
        /// </summary>
        /// <param name="storiesService">The stories service.</param>
        /// <param name="controllerActionsExecutor">The controller actions executor.</param>
        public StoriesController(IStoriesService storiesService, ControllerActionsExecutor controllerActionsExecutor)
        {
            _storiesService = storiesService;
            _controllerActionsExecutor = controllerActionsExecutor;
        }


        /// <summary>
        /// Gets the best stories.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>A Task.</returns>
        [HttpGet("best")]
        public async Task<IActionResult> GetBestStories([FromQuery] int count)
        {
            if (count <= 0)
            {
                return BadRequest("Count must be greater than zero.");
            }

            Task<IEnumerable<StoryDto>> Func() => _storiesService.GetBestStoriesAsync(count);

            return await _controllerActionsExecutor.ExecuteAndHandleErrorsAsync(this, Func).ConfigureAwait(false);
        }
    }
}
