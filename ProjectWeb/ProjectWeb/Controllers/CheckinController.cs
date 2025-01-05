using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.Data;
using ProjectWeb.Interfaces;
using ProjectWeb.Models;
using ProjectWeb.Models.Entities;

namespace ProjectWeb.Controllers
{
    public class CheckinController : Controller
    {
        private readonly ICheckinService _checkinService;
        private readonly IZborService _zborService;
        private readonly IUtilizatorService _utilizatorService;

        public CheckinController(ICheckinService checkinService, IZborService zborService, IUtilizatorService utilizatorService)
        {
            _checkinService = checkinService;
            _zborService = zborService;
            _utilizatorService = utilizatorService;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var checkins = await _checkinService.GetAllCheckinsAsync();
            return View(checkins);
        }

        [HttpGet]
        public async Task<IActionResult> AddAsync(int? idZbor)
        {
            // Pregătește lista de utilizatori și zboruri pentru dropdown-uri
            var utilizatori = await _utilizatorService.GetAllUtilizatoriAsync();
            var zboruri = await _zborService.GetAllZboruriAsync();
            var greutateMaxima = 0m;
            var taxaSuplimentara = 0m;
            decimal pretFinal = 0m;

            if (idZbor.HasValue)
            {
                var zbor = await _zborService.GetZborByIdAsync(idZbor.Value);
                if (zbor != null)
                {
                    greutateMaxima = zbor.GreutateMaximaBagaj;
                    taxaSuplimentara = zbor.TaxaSuplimentara;
                    pretFinal = zbor.Pret;
                }
            }

            var viewModel = new AddCheckinViewModel
            {
                // Populăm dropdown-urile folosind SelectListItem
                Utilizatori = utilizatori.Select(u => new SelectListItem
                {
                    Value = u.IdUtilizator.ToString(),
                    Text = u.Nume // Poți modifica ce câmp dorești să fie afișat
                }).ToList(),

                Zboruri = zboruri.Select(z => new SelectListItem
                {
                    Value = z.IdZbor.ToString(),
                    Text = $"{z.LocuriDisponibile} locuri disponibile - greutate bagaj: {z.GreutateMaximaBagaj} kg", // Detalii despre zbor, poți modifica ce vrei să afișezi
                    Selected = idZbor.HasValue && z.IdZbor == idZbor // Preselectează zborul dacă idZbor este prezent
                }).ToList(),
                IdZbor = (int)idZbor, // Setează IdZbor pentru a putea fi folosit mai jos
                //GreutateMaximaBagaj = greutateMaxima
                PretFinal = pretFinal
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCheckinViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Obține detaliile zborului
                var zbor = await _zborService.GetZborByIdAsync(viewModel.IdZbor);
                if (zbor == null)
                {
                    ModelState.AddModelError("", "Zborul selectat nu este valid.");
                    return View(viewModel);
                }

                // Calculează taxa suplimentară
                decimal taxaSuplimentara = 0m;
                if (viewModel.GreutateBagaj > zbor.GreutateMaximaBagaj)
                {
                    //+= zbor.TaxaSuplimentara;
                    viewModel.PretFinal = zbor.Pret + zbor.TaxaSuplimentara;
                }
                else
                {
                    viewModel.PretFinal = zbor.Pret;
                }

                // Calculează prețul total
                //decimal pretFinal = zbor.Pret + taxaSuplimentara;

                // Creează obiectul Checkin
                var checkin = new Checkin
                {
                    IdUtilizator = viewModel.IdUtilizator,
                    IdZbor = viewModel.IdZbor,
                    GreutateBagaj = viewModel.GreutateBagaj,
                    LocRezervat = viewModel.LocRezervat,
                    DataCheckin = viewModel.DataCheckin,
                    PretFinal = viewModel.PretFinal // Adaugă taxa suplimentară în Checkin
                };

                // Salvează check-in-ul
                await _checkinService.AddCheckinAsync(checkin);
                return RedirectToAction("List", "Checkin");
            }

            // În caz de eroare, reîncarcă lista de utilizatori și zboruri
            var utilizatori = await _utilizatorService.GetAllUtilizatoriAsync();
            viewModel.Utilizatori = utilizatori.Select(u => new SelectListItem
            {
                Value = u.IdUtilizator.ToString(),
                Text = u.Nume
            }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _checkinService.DeleteCheckinByIdAsync(id);
            return RedirectToAction("List");
        }

    }
}
