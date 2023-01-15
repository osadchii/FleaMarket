using FleaMarket.Infrastructure.Services.UpdateHandlers;
using MediatR;
using Telegram.Bot.Types;

namespace FleaMarket.Infrastructure.Handlers.Telegram.Commands;

public static class ApplyUpdate
{
    public class Command : IRequest<Unit>
    {
        public Command(string token, Update update)
        {
            Token = token;
            Update = update;
        }

        public string Token { get; }
        public Update Update { get; }
    }
    
    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUpdateHandleService _updateHandleService;

        public Handler(IUpdateHandleService updateHandleService)
        {
            _updateHandleService = updateHandleService;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            await _updateHandleService.Handle(request.Token, request.Update);
            return Unit.Value;
        }
    }
}