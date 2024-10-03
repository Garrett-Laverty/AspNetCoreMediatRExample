using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Linq;
namespace RazorPagesLab.Pages.AddressBook;

public class UpdateAddressHandler
    : IRequestHandler<UpdateAddressRequest, Guid>
{
    private readonly IRepo<AddressBookEntry> _repo;

    public UpdateAddressHandler(IRepo<AddressBookEntry> repo)
    {
        _repo = repo;
    }

    public async Task<Guid> Handle(UpdateAddressRequest request, CancellationToken cancellationToken)
    {
        // Find the entry being updated, call update on that entry and ensure the repo is also updated
        AddressBookEntry entry = _repo.Find(new EntryByIdSpecification(request.Id)).First();
        entry.Update(request.Line1, request.Line2, request.City, request.State, request.PostalCode);
        _repo.Update(entry);

        return await Task.FromResult(request.Id);
    }
}