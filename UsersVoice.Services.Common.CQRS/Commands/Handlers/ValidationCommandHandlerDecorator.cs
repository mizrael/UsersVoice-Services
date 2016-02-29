using System;
using System.Threading.Tasks;
using MediatR;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.Common.CQRS.Commands.Handlers
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
            {
                var result = await _validator.ValidateAsync(command);

                if(null == result)
                    throw new ValidationException(null, "Validation failed");

                if(!result.Success)
                    throw new ValidationException(result.Errors);
            }

            await _handler.Handle(command);
        }
    }

}
