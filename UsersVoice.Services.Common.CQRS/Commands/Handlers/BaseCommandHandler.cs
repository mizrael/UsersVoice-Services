using System;
using System.Threading.Tasks;
using MediatR;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.Common.CQRS.Commands.Handlers
{
    public abstract class BaseCommandHandler<TCommand> : IAsyncNotificationHandler<TCommand>
        where TCommand : IAsyncNotification
    {
        private readonly IValidator<TCommand> _validator;

        protected BaseCommandHandler(IValidator<TCommand> validator = null)
        {
            _validator = validator;
        }

        public async Task Handle(TCommand command)
        {
            if (null == command)
                throw new ArgumentNullException("command");

            if (null != _validator)
            {
                var result = await _validator.ValidateAsync(command);

                if(null == result)
                    throw new ValidationException(null, "Validation failed");

                if(!result.Success)
                    throw new ValidationException(result.Errors);
            }

            await RunCommand(command);
        }

        protected abstract Task RunCommand(TCommand command);
    }

}
