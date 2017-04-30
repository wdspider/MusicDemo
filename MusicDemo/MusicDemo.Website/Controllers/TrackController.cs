using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MusicDemo.Website.Backend;
using MusicDemo.Website.Models;
using MusicDemo.Website.ViewModels;

namespace MusicDemo.Website.Controllers
{
	public class TrackController : Controller
    {
		#region Internal State
		private readonly BackendProvider backend;
		private readonly IMapper autoMapper;
		#endregion

		#region Constructors
		public TrackController(BackendProvider provider, IMapper mapper)
		{
			// Initialize internal state
			backend = provider;
			autoMapper = mapper;
		}
		#endregion

		#region Action Results
		#region Create View
		[HttpGet]
		public async Task<ActionResult> Create(int artistID, int albumID)
		{
			// Verify album exists
			Album album = await backend.AlbumGetByIDAsync(artistID, albumID);
			if (album == null) return RedirectToAction("Details", "Artist", routeValues: new { artistID = artistID });

			return View(new TrackViewModel { ArtistID = artistID, AlbumID = albumID });
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(TrackViewModel newTrack)
		{
			// Is modelstate valid?
			if (ModelState.IsValid)
			{   // Yes
				// Save album to backend
				bool wasSaved = await backend.TrackAddAsync(autoMapper.Map<Track>(newTrack));
				if (wasSaved) return RedirectToAction("Details", "Album", routeValues: new { artistID = newTrack.ArtistID, albumID = newTrack.AlbumID });

				// Album didn't save, display error message
				ModelState.AddModelError("", "There was an issue saving the track");
				return View(newTrack);
			}

			return View(newTrack);
		}
		#endregion

		#region Edit View
		[HttpGet]
		public async Task<ActionResult> Edit(int artistID, int albumID, int trackID)
		{
			Track desiredTrack = await backend.TrackGetByIDAsync(albumID, trackID);

			// Was the track found?
			if (desiredTrack != null)
			{   // Yes
				// Allow track to be edited
				TrackViewModel viewModel = autoMapper.Map<TrackViewModel>(desiredTrack);
				viewModel.ArtistID = artistID;
				return View(viewModel);
			}

			// Track wasn't found, redirect back to list
			return RedirectToAction("Details", "Album", routeValues: new { artistID = artistID, albumID = albumID });
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(TrackViewModel updatedTrack)
		{
			// Is modelstate valid?
			if (ModelState.IsValid)
			{   // Yes
				// Update track
				bool wasUpdated = await backend.TrackUpdateAsync(autoMapper.Map<Track>(updatedTrack));
				if (wasUpdated) return RedirectToAction("Details", "Album", routeValues: new { artistID = updatedTrack.ArtistID, albumID = updatedTrack.AlbumID });

				// Track wasn't updated, display error message
				ModelState.AddModelError("", "Could not find track in database to update.");
				return View(updatedTrack);
			}

			return View(updatedTrack);
		}
		#endregion

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(int artistID, int albumID, int trackID)
		{
			// Delete track
			await backend.TrackDeleteByIDAsync(albumID, trackID);
			return RedirectToAction("Details", "Album", routeValues: new { artistID = artistID, albumID = albumID });
		}
		#endregion
	}
}