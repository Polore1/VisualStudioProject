using Microsoft.AspNetCore.Mvc;
using ProjectWeb.Data;
using ProjectWeb.Models.Entities;
using ProjectWeb.Models;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.Interfaces;
using Microsoft.Extensions.Logging;

namespace ProjectWeb.Controllers
{
    public class ZborController : Controller
    {
        private readonly ICacheService _cacheService;
        private readonly IZborService _zborService;
        private readonly ILogger<ZborController> _logger;

        public ZborController(ICacheService cacheService,IZborService zborService, ILogger<ZborController> logger)
        {
            _cacheService = cacheService;
            _zborService = zborService;
            _logger = logger;
        }

        public async Task<IActionResult> List()
        {
            try
            {
                _logger.LogInformation("Accesarea listei de zboruri.");

                // Verifică dacă lista de zboruri este deja în cache
                if (_cacheService.IsSet("zboruri_list"))
                {
                    _logger.LogInformation("Datele pentru lista de zboruri sunt disponibile în cache.");
                    var zboruri = _cacheService.Get<List<Zbor>>("zboruri_list");
                    return View(zboruri);
                }

                _logger.LogInformation("Datele pentru lista de zboruri nu sunt în cache. Se va obține din baza de date.");
                var zboruriDinBD = await _zborService.GetAllZboruriAsync();

                // Salvează lista în cache pentru utilizări ulterioare
                _cacheService.Set("zboruri_list", zboruriDinBD);

                _logger.LogInformation("Lista de zboruri a fost obținută și salvată în cache.");
                return View(zboruriDinBD);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la accesarea listei de zboruri.");
                return View("Error");
            }
            //var zboruri = await _zborService.GetAllZboruriAsync();
            //return View(zboruri);
        }

        public async Task<IActionResult> Detalii (int id)
        {
            try
            {
                _logger.LogInformation("Accesarea detaliilor zborului cu ID: {ZborId}", id);

                // Verifică dacă detaliile zborului sunt deja în cache
                if (_cacheService.IsSet($"zbor_{id}"))
                {
                    _logger.LogInformation("Detaliile zborului cu ID {ZborId} sunt disponibile în cache.", id);
                    var zbor = _cacheService.Get<Zbor>($"zbor_{id}");
                    return View(zbor);
                }

                _logger.LogInformation("Detaliile zborului cu ID {ZborId} nu sunt în cache. Se va obține din baza de date.", id);
                var zborDinBD = await _zborService.GetZborByIdAsync(id);

                if (zborDinBD == null)
                {
                    _logger.LogWarning("Zborul cu ID {ZborId} nu a fost găsit.", id);
                    return NotFound();
                }

                // Salvează detaliile zborului în cache
                _cacheService.Set($"zbor_{id}", zborDinBD);

                _logger.LogInformation("Detaliile zborului cu ID {ZborId} au fost obținute și salvate în cache.", id);
                return View(zborDinBD);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la accesarea detaliilor zborului cu ID: {ZborId}", id);
                return View("Error");
            }
            //var zbor = await _zborService.GetZborByIdAsync(id);
            //return View(zbor);
        }


        [HttpPost]
        public async Task<IActionResult> SoftDelete(int id)
        {

            try
            {
                _logger.LogInformation("Ștergerea logică a zborului cu ID: {ZborId}", id);
                await _zborService.SoftDeleteZborAsync(id);

                // Îndepărtează zborul din cache după ce a fost șters
                _cacheService.Remove($"zbor_{id}");
                _cacheService.Remove("zboruri_list"); // Resetează lista de zboruri din cache

                _logger.LogInformation("Zborul cu ID {ZborId} a fost șters cu succes.", id);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la ștergerea zborului cu ID: {ZborId}", id);
                return View("Error");
            }
            //await _zborService.SoftDeleteZborAsync(id);
            //return RedirectToAction("List");
        }


        //[HttpPost]
        //public async Task<IActionResult> Edit(AddZborViewModel viewModel)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            _logger.LogWarning("Validarea modelului a eșuat pentru Zbor cu ID {ZborId}", viewModel.IdZbor);
        //            return View(viewModel);
        //        }

        //        _logger.LogInformation("Actualizarea zborului cu ID {ZborId}.", viewModel.IdZbor);
        //        var zbor = await _zborService.GetZborByIdAsync(viewModel.IdZbor);
        //        if (zbor == null)
        //        {
        //            _logger.LogWarning("Zborul cu ID {ZborId} nu a fost găsit pentru actualizare.", viewModel.IdZbor);
        //            return NotFound();
        //        }

        //        zbor.NumeCompanie = viewModel.NumeCompanie;
        //        zbor.Imbarcare = viewModel.Imbarcare;
        //        zbor.Destinatie = viewModel.Destinatie;
        //        zbor.DataPlecare = viewModel.DataPlecare;
        //        zbor.GreutateMaximaBagaj = viewModel.GreutateMaximaBagaj;
        //        zbor.LocuriDisponibile = viewModel.LocuriDisponibile;
        //        zbor.Status = viewModel.Status;

        //        await _zborService.UpdateZborAsync(zbor);
        //        _cacheService.Remove($"zbor_{viewModel.IdZbor}");

        //        // Resetează cache-ul pentru lista de zboruri
        //        _cacheService.Remove("zboruri_list");

        //        _logger.LogInformation("Zborul cu ID {ZborId} a fost actualizat cu succes.", viewModel.IdZbor);
        //        return RedirectToAction("List");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Eroare la actualizarea zborului cu ID {ZborId}.", viewModel.IdZbor);
        //        return View("Error");
        //    }
        //}

        //private readonly ApplicationDB dbContext;
        //public ZborController(ApplicationDB dbContext)
        //{
        //    this.dbContext = dbContext;
        //}
        //[HttpGet]
        //public async Task<IActionResult> List()
        //{
        //    var zboruri = await dbContext.Zbor.ToListAsync();
        //    return View (zboruri);
        //}
        //[HttpGet]
        //public IActionResult Add()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Add(AddZborViewModel viewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        // Adaugă un mesaj general de eroare pentru a informa utilizatorul despre problemele de validare
        //        TempData["ErrorMessage"] = "Există câmpuri invalide. Te rugăm să le corectezi.";

        //        // Dacă modelul nu este valid, rămâi pe pagina de Add pentru corectarea erorilor
        //        return View(viewModel);
        //    }

        //    //preluam datele care au fost introduse in ViewModel si le introducem in una locala
        //    var zbor = new Zbor
        //    {
        //       NumeCompanie = viewModel.NumeCompanie,
        //       Imbarcare = viewModel.Imbarcare,
        //       Destinatie = viewModel.Destinatie,
        //       DataPlecare = viewModel.DataPlecare,
        //       GreutateMaximaBagaj = viewModel.GreutateMaximaBagaj,
        //       LocuriDisponibile = viewModel.LocuriDisponibile,
        //       Status = viewModel.Status
        //    };

        //    await dbContext.Zbor.AddAsync(zbor);

        //    //aici se salveaza modificarile in baza de date
        //    await dbContext.SaveChangesAsync();

        //    return RedirectToAction("List", "Zbor");
        //    //return View();
        //}


        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    Console.WriteLine($"ID primit: {id}"); // Log simplu
        //    var zbor = await dbContext.Zbor.FindAsync(id);

        //    return View(zbor);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(Zbor viewModel)
        //{
        //    var zbor = await dbContext.Zbor.FindAsync(viewModel.IdZbor);

        //    if (zbor is not null)
        //    {
        //        //zbor.IdZbor=viewModel.IdZbor;
        //        zbor.NumeCompanie=viewModel.NumeCompanie;
        //        zbor.Imbarcare=viewModel.Imbarcare;
        //        zbor.Destinatie=viewModel.Destinatie;
        //        zbor.DataPlecare=viewModel.DataPlecare;
        //        zbor.GreutateMaximaBagaj=viewModel.GreutateMaximaBagaj;
        //        zbor.LocuriDisponibile=viewModel.LocuriDisponibile;
        //        zbor.Status=viewModel.Status;

        //        await dbContext.SaveChangesAsync();
        //    }


        //    //daca uldate-ul este realizat cu succes, ne vom intoarce la pagina principala, tabelul cu zboruri
        //    return RedirectToAction("List", "Zbor");
        //}

        //[HttpGet]
        //public async Task<IActionResult> Detalii(int id)
        //{
        //    Console.WriteLine($"ID primit: {id}"); // Log simplu
        //    var zbor = await dbContext.Zbor.FindAsync(id);

        //    return View(zbor);
        //}
        //[HttpPost]
        //public async Task<IActionResult> Delete(Zbor viewModel)
        //{
        //    var zbor = await dbContext.Zbor
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(x => x.IdZbor == viewModel.IdZbor);

        //    if (zbor is not null)
        //    {
        //        dbContext.Zbor.Remove(viewModel);

        //        await dbContext.SaveChangesAsync();

        //        TempData["SuccessMessage"] = $"Zborul cu ID \"{viewModel.IdZbor}\" a fost șters cu succes!";
        //    }
        //    return RedirectToAction("List", "Zbor");
        //}


    }
}
