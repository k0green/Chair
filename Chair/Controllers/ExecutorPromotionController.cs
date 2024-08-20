using Chair.BLL.CQRS.ExecutorPromotion;
using Chair.BLL.Dto.ExecutorPromotion;
using Chair.DAL.Data.Entities;
using Chair.DAL.Extension.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using opr_lib;

namespace Chair.Controllers
{
    [ApiController]
    [Route("executor-promotion")]
    [Authorize]
    public class ExecutorPromotionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExecutorPromotionController> _logger;

        public ExecutorPromotionController(IMediator mediator,
            ILogger<ExecutorPromotionController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{executorId}")]
        [ProducesResponseType(typeof(List<ExecutorPromotionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByContractId([FromRoute] Guid executorId)
        {
            var query = new GetAllPromotionsByExecutorIdQuery() { ExecutorId = executorId };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(List<ExecutorPromotionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromBody] FilterModel filter)
        {
            var query = new GetAllPromotionsQuery() { Filter = filter };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] AddExecutorPromotionDto addExecutorPromotionDto)
        {
            var command = new AddExecutorPromotionQuery() { AddExecutorPromotionDto = addExecutorPromotionDto };
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(UpdateExecutorPromotionDto updateExecutorPromotionDto)
        {
            var command = new UpdateExecutorPromotionQuery() { UpdateExecutorPromotionDto = updateExecutorPromotionDto };
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete]
        [Route("remove/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveById([FromRoute] Guid id)
        {
            var command = new RemoveExecutorPromotionQuery() { Id = id };
            await _mediator.Send(command);

            return NoContent();
        }
    }
}