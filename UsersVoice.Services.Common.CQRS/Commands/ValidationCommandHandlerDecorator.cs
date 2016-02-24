using System;
using System.Threading.Tasks;
using MediatR;
using UsersVoice.Services.Infrastructure.Common.Validation;

namespace UsersVoice.Services.Common.CQRS.Commands
{
    public class ValidationCommandHandlerDecorator<TCommand> : IAsyncNotificationHandler<TCommand>
        where TCommand : IAsyncNotification
    {
        private readonly IValidator<TCommand> _validator;
        private readonly IAsyncNotificationHandler<TCommand> _handler;

        public ValidationCommandHandlerDecorator(IAsyncNotificationHandler<TCommand> handler, IValidator<TCommand> validator)
        {
            if (handler == null) 
                throw new ArgumentNullException("handler");
            _validator = validator;
            _handler = handler;
        }

        public async Task Handle(TCommand command)
        {
            if (null != _validator)
                await _validator.ValidateAsync(command);

            await _handler.Handle(command);
        }
    }
}
