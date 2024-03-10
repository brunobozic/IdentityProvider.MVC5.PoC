
using IdentityProvider.Repository.EFCore.EFDataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class CommandsDispatcher : ICommandsDispatcher
{
    private readonly AppDbContext _context;
    private readonly IMediator _mediator;

    public CommandsDispatcher(
        IMediator mediator,
        AppDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task DispatchCommandAsync(Guid id)
    {
        var internalCommand = await _context.InternalCommands.SingleOrDefaultAsync(x => x.Id == id);

        //var type = Assembly.GetAssembly(typeof(SendPhoneAddedToPersonCommand)).GetType(internalCommand.Type);
        //dynamic command = JsonConvert.DeserializeObject(internalCommand.Data, type);

        //internalCommand.ProcessedDate = DateTime.UtcNow;

        //await _mediator.Send(command);
    }
}