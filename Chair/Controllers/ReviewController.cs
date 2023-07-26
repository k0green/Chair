using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.CQRS.Review;
using Chair.BLL.Dto.Review;
using Chair.DAL.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chair.Controllers
{
    [ApiController]
    [Route("review")]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(IMediator mediator,
            ILogger<ReviewController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{executorServiceId}")]
        [ProducesResponseType(typeof(List<ReviewDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByContractId([FromRoute] Guid executorServiceId)
        {
            var query = new GetAllReviewsForServiceQuery() { ExecutorServiceId = executorServiceId };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] AddReviewDto addReviewDto)
        {
            var command = new AddReviewQuery() { AddReviewDto = addReviewDto };
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(UpdateReviewDto updateReviewDto)
        {
            var command = new UpdateReviewQuery() { UpdateReviewDto = updateReviewDto };
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete]
        [Route("remove/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveById([FromRoute] Guid id)
        {
            var command = new RemoveReviewQuery() { Id = id };
            await _mediator.Send(command);

            return NoContent();
        }
    }
}