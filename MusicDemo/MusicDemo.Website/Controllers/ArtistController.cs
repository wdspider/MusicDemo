﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MusicDemo.Website.Backend.BackendProviders;
using MusicDemo.Website.Backend.Models;
using MusicDemo.Website.ViewModels;

namespace MusicDemo.Website.Controllers
{
	public class ArtistController : Controller
    {
		#region Internal State
		private readonly BackendProvider backend;
		private readonly IMapper autoMapper;
		#endregion

		#region Constructors
		public ArtistController(BackendProvider provider, IMapper mapper)
		{
			// Initialize internal state
			backend = provider;
			autoMapper = mapper;
		}
		#endregion

		#region Action Results
		#region Index View
		[HttpGet]
		public async Task<ActionResult> Index()
        {
            return View(autoMapper.Map<List<ArtistViewModel>>(await backend.ArtistGetAllAsync()));
        }
		#endregion

		#region Create View
		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(ArtistViewModel newArtist)
		{
			// Is modelstate valid?
			if (ModelState.IsValid)
			{   // Yes
				// Save artist to backend
				bool wasSaved = await backend.ArtistAddAsync(autoMapper.Map<Artist>(newArtist));
				if (wasSaved) return RedirectToAction("Index");

				// Artist wasn't saved, display error message
				ModelState.AddModelError("", "There was an issue saving the artist");
				return View(newArtist);
			}

			return View(newArtist);
		}
		#endregion

		#region Edit View
		[HttpGet]
		public async Task<ActionResult> Edit(int artistID)
		{
			Artist desiredArtist = await backend.ArtistGetByIDAsync(artistID);

			// Was the artist found?
			if(desiredArtist != null)
			{   // Yes
				// Allow artist to be edited
				return View(autoMapper.Map<ArtistViewModel>(desiredArtist));
			}

			// Artist wasn't found, redirect back to list
			return RedirectToAction("Index");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(ArtistViewModel updatedArtist)
		{
			// Is modelstate valid?
			if (ModelState.IsValid)
			{   // Yes
				// Update artist
				bool wasUpdated = await backend.ArtistUpdateAsync(autoMapper.Map<Artist>(updatedArtist));
				if (wasUpdated) return RedirectToAction("Index");

				// Artist wasn't updated, display error message
				ModelState.AddModelError("", "Could not find artist in database to update.");
				return View(updatedArtist);
			}

			return View(updatedArtist);
		}
		#endregion

		#region Details View
		[HttpGet]
		public async Task<ActionResult> Details(int artistID)
		{
			// Get details from database
			Artist artist = await backend.ArtistGetByIDAsync(artistID);
			if (artist == null) return RedirectToAction("Index");
			
			return View(autoMapper.Map<ArtistDetailsViewModel>(artist));
		}
		#endregion

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(int artistID)
		{
			// Delete artist
			await backend.ArtistDeleteByIDAsync(artistID);
			return RedirectToAction("Index");
		}
		#endregion
	}
}