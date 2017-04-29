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
		public async Task<ActionResult> Create(int albumID)
		{
			return View(new TrackViewModel { AlbumID = albumID });
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(TrackViewModel newTrack)
		{
			// Is modelstate valid?
			if (ModelState.IsValid)
			{   // Yes
				// Save album to backend
				bool wasSaved = false;
				try { wasSaved = await backend.TrackAddAsync(autoMapper.Map<Track>(newTrack)); }
				catch(SqlException ex)
				{
					wasSaved = false;
					ModelState.AddModelError("DatabaseError", ex);
				}
				if (wasSaved) return RedirectToAction("Details", "Album", routeValues: new { albumID = newTrack.AlbumID });

				// Album didn't save, display error message
				ModelState.AddModelError("Track", "There was an issue saving the track");
				return View(newTrack);
			}

			return View(newTrack);
		}
		#endregion

		#region Edit View
		[HttpGet]
		public async Task<ActionResult> Edit(int albumID, int trackID)
		{
			Track desiredTrack = await backend.TrackGetByIDAsync(trackID);

			// Was the track found?
			if (desiredTrack != null)
			{   // Yes
				// Allow track to be edited
				return View(autoMapper.Map<TrackViewModel>(desiredTrack));
			}

			// Track wasn't found, redirect back to list
			return RedirectToAction("Details", "Album", routeValues: new { albumID = albumID });
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
				if (wasUpdated) return RedirectToAction("Details", "Album", routeValues: new { albumID = updatedTrack.AlbumID });

				// Track wasn't updated, display error message
				ModelState.AddModelError("Track", "Could not find track in database to update.");
				return View(updatedTrack);
			}

			return View(updatedTrack);
		}
		#endregion

		#region Details View
		[HttpGet]
		public async Task<ActionResult> Details(int trackID)
		{
			return View(autoMapper.Map<TrackViewModel>(await backend.TrackGetByIDAsync(trackID)));
		}
		#endregion

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(int albumID, int trackID)
		{
			// Delete track
			await backend.TrackDeleteByIDAsync(trackID);
			return RedirectToAction("Details", "Album", routeValues: new { albumID = albumID });
		}
		#endregion
	}
}