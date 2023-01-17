using Newtonsoft.Json;

namespace FleaMarket.Infrastructure.StateHandlers;

public interface IStateHandler<TState, in TParameter> where TState : BaseState
{
    TState DeserializeStateDate(string stateData) => JsonConvert.DeserializeObject<TState>(stateData);

    Task Handle(Guid telegramUserId, Guid? telegramBotId, TState state, TParameter parameter);

    Task Activate(Guid telegramUserId, Guid? telegramBotId, TState state);
}