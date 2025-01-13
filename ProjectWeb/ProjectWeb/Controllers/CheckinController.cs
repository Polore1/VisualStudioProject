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
        private readonly ILogger<CheckinController> _logger;

        public CheckinController(ICheckinService checkinService, IZborService zborService, IUtilizatorService utilizatorService, ILogger<CheckinController> logger)
        {
            _checkinService = checkinService;
            _zborService = zborService;
            _utilizatorService = utilizatorService;
            _logger = logger;
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

                // Creează un obiect temporar Checkin pentru calcul
                var checkinTemp = new Checkin
                {
                    GreutateBagaj = viewModel.GreutateBagaj,
                    Zbor = zbor
                };

                // Calculează taxa suplimentară folosind serviciul
                var (taxaSuplimentara, mesaj) = await _checkinService.CalculeazaTaxaBagaj(checkinTemp);


                // Afișează un mesaj de notificare pentru taxa suplimentară
                if (taxaSuplimentara > 0 && Request.Form["confirmTaxaSuplimentara"] != "true")
                {
                    ViewData["TaxaSuplimentaraMessage"] = mesaj;
                    viewModel.PretFinal = zbor.Pret + taxaSuplimentara;
                    return View("ConfirmTaxaSuplimentara", viewModel);
                }

                // Calculează prețul total
                viewModel.PretFinal = zbor.Pret + taxaSuplimentara;


                // Creează obiectul Checkin
                var checkin = new Checkin
                {
                    IdUtilizator = viewModel.IdUtilizator,
                    IdZbor = viewModel.IdZbor,
                    GreutateBagaj = viewModel.GreutateBagaj,
                    LocRezervat = viewModel.LocRezervat,
                    DataCheckin = viewModel.DataCheckin,
                    PretFinal = viewModel.PretFinal, // Adaugă taxa suplimentară în Checkin
                };

                // Salvează check-in-ul
                await _checkinService.AddCheckinAsync(checkin);

                // Transmite mesajul către view
                ViewData["TaxaSuplimentaraMessage"] = "Check-in-ul a fost adăugat cu succes.";

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

        // Metoda pentru confirmarea taxei suplimentare
        public async Task<IActionResult> ConfirmTaxaSuplimentara(int checkinId)
        {
            try
            {
                // Obține check-in-ul din baza de date
                var checkin = await _checkinService.GetCheckinByIdAsync(checkinId);

                if (checkin == null)
                {
                    _logger.LogError($"Check-in-ul cu ID {checkinId} nu a fost găsit.");
                    return View("Error", "Check-in-ul nu a fost găsit.");
                }

                // Confirmă taxa suplimentară
                checkin.IsValid = true;  // Marchează check-in-ul ca validat
                await _checkinService.UpdateCheckinAsync(checkin);

                _logger.LogInformation($"Check-in-ul cu ID {checkinId} a fost validat.");
                return RedirectToAction("Index");  // Sau o altă acțiune
            }
            catch (Exception ex)
            {
                _logger.LogError($"Eroare la confirmarea taxei suplimentare: {ex.Message}");
                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _checkinService.DeleteCheckinByIdAsync(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var checkin = await _checkinService.GetCheckinByIdAsync(id);

            if (checkin == null)
            {
                _logger.LogError($"Check-in-ul cu ID {id} nu a fost găsit.");
                return NotFound();
            }

            // Pregătește datele pentru dropdown-uri (Utilizatori și Zboruri)
            var utilizatori = await _utilizatorService.GetAllUtilizatoriAsync();
            var zboruri = await _zborService.GetAllZboruriAsync();

            var viewModel = new AddCheckinViewModel
            {
                idCheckin = checkin.IdCheckin,
                IdUtilizator = checkin.IdUtilizator,
                IdZbor = checkin.IdZbor,
                GreutateBagaj = checkin.GreutateBagaj,
                LocRezervat = checkin.LocRezervat,
                DataCheckin = checkin.DataCheckin,
                PretFinal = checkin.PretFinal,
                Utilizatori = utilizatori.Select(u => new SelectListItem
                {
                    Value = u.IdUtilizator.ToString(),
                    Text = u.Nume,
                    Selected = u.IdUtilizator == checkin.IdUtilizator
                }).ToList(),
                Zboruri = zboruri.Select(z => new SelectListItem
                {
                    Value = z.IdZbor.ToString(),
                    Text = $"{z.LocuriDisponibile} locuri disponibile - greutate bagaj: {z.GreutateMaximaBagaj} kg",
                    Selected = z.IdZbor == checkin.IdZbor
                }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AddCheckinViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Reîncarcă dropdown-urile în caz de eroare
                var utilizatori = await _utilizatorService.GetAllUtilizatoriAsync();
                var zboruri = await _zborService.GetAllZboruriAsync();

                viewModel.Utilizatori = utilizatori.Select(u => new SelectListItem
                {
                    Value = u.IdUtilizator.ToString(),
                    Text = u.Nume,
                    Selected = u.IdUtilizator == viewModel.IdUtilizator
                }).ToList();

                viewModel.Zboruri = zboruri.Select(z => new SelectListItem
                {
                    Value = z.IdZbor.ToString(),
                    Text = $"{z.LocuriDisponibile} locuri disponibile - greutate bagaj: {z.GreutateMaximaBagaj} kg",
                    Selected = z.IdZbor == viewModel.IdZbor
                }).ToList();

                return View(viewModel);
            }

            try
            {
                var checkin = await _checkinService.GetCheckinByIdAsync(viewModel.idCheckin);

                if (checkin == null)
                {
                    _logger.LogError($"Check-in-ul cu ID {viewModel.idCheckin} nu a fost găsit.");
                    return NotFound();
                }

                // Actualizează valorile
                checkin.IdUtilizator = viewModel.IdUtilizator;
                checkin.IdZbor = viewModel.IdZbor;
                checkin.GreutateBagaj = viewModel.GreutateBagaj;
                checkin.LocRezervat = viewModel.LocRezervat;
                checkin.DataCheckin = viewModel.DataCheckin;
                checkin.PretFinal = viewModel.PretFinal;

                await _checkinService.UpdateCheckinAsync(checkin);

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Eroare la editarea check-in-ului: {ex.Message}");
                ModelState.AddModelError("", "A apărut o eroare la salvarea modificărilor.");
                return View(viewModel);
            }
        }



    }
}
