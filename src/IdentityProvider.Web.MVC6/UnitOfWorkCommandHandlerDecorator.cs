
using IdentityProvider.Repository.EFCore;
using IdentityProvider.Repository.EFCore.EFDataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
{
    private readonly AppDbContext _context;
    private readonly ICommandHandler<T> _decorated;

    private readonly IMyUnitOfWork _unitOfWork;

    public UnitOfWorkCommandHandlerDecorator(
        ICommandHandler<T> decorated,
        IMyUnitOfWork unitOfWork,
        AppDbContext context)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        await _decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase)
        {
            var internalCommand =
                await _context.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id,
                    cancellationToken);

            if (internalCommand != null) internalCommand.ProcessedDate = DateTime.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}