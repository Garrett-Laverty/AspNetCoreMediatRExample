using System;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace RazorPagesLab.Pages.AddressBook;

public class EditModel : PageModel
{
	private readonly IMediator _mediator;
	private readonly IRepo<AddressBookEntry> _repo;

	public EditModel(IRepo<AddressBookEntry> repo, IMediator mediator)
	{
		_repo = repo;
		_mediator = mediator;
	}

	[BindProperty]
	public UpdateAddressRequest UpdateAddressRequest { get; set; }

	public void OnGet(Guid id)
	{
		// Find matching entries, set current entry and use those values in the new UAR
		IReadOnlyList<AddressBookEntry> entries = _repo.Find(new EntryByIdSpecification(id));

		if (entries.Count > 0) // Check count to make sure the entry hasn't been deleted
		{
			AddressBookEntry entry = entries.First();
			UpdateAddressRequest = new UpdateAddressRequest {
				Id = entry.Id,
				Line1 = entry.Line1,
				Line2 = entry.Line2,
				City = entry.City,
				State = entry.State,
				PostalCode = entry.PostalCode
			};
		}
	}

	public ActionResult OnPost()
	{
		// Send update command in similar fashion the Create functionality
		if (ModelState.IsValid)
		{
			_ = _mediator.Send(UpdateAddressRequest);
			return RedirectToPage("Index");
		}
		return Page();
	}
}