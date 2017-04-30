using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MusicDemo.Website.Backend.BackendProviders;
using MusicDemo.Website.Backend.Models;
using MusicDemo.Website.ViewModels;

namespace MusicDemo.Website.Controllers
{
	public class AlbumController : Controller
    {
		#region Internal State
		private readonly BackendProvider backend;
		private readonly IMapper autoMapper;
		#endregion

		#region Constructors
		public AlbumController(BackendProvider provider, IMapper mapper)
		{
			// Initialize internal state
			backend = provider;
			autoMapper = mapper;
		}
		#endregion

		#region Action Results
		#region Create View
		[HttpGet]
		public async Task<ActionResult> Create(int artistID)
		{
			// Verify artist exists
			Artist artist = await backend.ArtistGetByIDAsync(artistID);
			if (artist == null) return RedirectToAction("Index", "Artist");

			return View(new AlbumViewModel { ArtistID = artistID });
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(AlbumViewModel newAlbum)
		{
			// Is modelstate valid?
			if (ModelState.IsValid)
			{   // Yes
				// Save album to backend
				bool wasSaved = await backend.AlbumAddAsync(autoMapper.Map<Album>(newAlbum));
				if (wasSaved) return RedirectToAction("Details", "Artist", routeValues: new { artistID = newAlbum.ArtistID });

				// Album didn't save, display error message
				ModelState.AddModelError("", "There was an issue saving the album");
				return View(newAlbum);
			}

			return View(newAlbum);
		}
		#endregion

		#region Edit View
		[HttpGet]
		public async Task<ActionResult> Edit(int artistID, int albumID)
		{
			Album desiredAlbum = await backend.AlbumGetByIDAsync(artistID, albumID);

			// Was the album found?
			if (desiredAlbum != null)
			{   // Yes
				// Allow album to be edited
				return View(autoMapper.Map<AlbumViewModel>(desiredAlbum));
			}

			// Album wasn't found, redirect back to list
			return RedirectToAction("Details", "Artist", routeValues: new { artistID = artistID });
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(AlbumViewModel updatedAlbum)
		{
			// Is modelstate valid?
			if (ModelState.IsValid)
			{   // Yes
				// Update album
				bool wasUpdated = await backend.AlbumUpdateAsync(autoMapper.Map<Album>(updatedAlbum));
				if (wasUpdated) return RedirectToAction("Details", "Artist", routeValues: new { artistID = updatedAlbum.ArtistID });

				// Album wasn't updated, display error message
				ModelState.AddModelError("", "Could not find album in database to update.");
				return View(updatedAlbum);
			}

			return View(updatedAlbum);
		}
		#endregion

		#region Details View
		[HttpGet]
		public async Task<ActionResult> Details(int artistID, int albumID)
		{
			// Get desired album
			Album album = await backend.AlbumGetByIDAsync(artistID, albumID);
			if(album == null) return RedirectToAction("Details", "Artist", routeValues: new { artistID = artistID});

			return View(autoMapper.Map<AlbumDetailsViewModel>(album));
		}
		#endregion

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(int artistID, int albumID)
		{
			// Delete album
			await backend.AlbumDeleteByIDAsync(artistID, albumID);
			return RedirectToAction("Details", "Artist", routeValues: new { artistID = artistID });
		}
		#endregion
	}
}