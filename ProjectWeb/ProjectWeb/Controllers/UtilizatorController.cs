using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.Data;
using ProjectWeb.Interfaces;
using ProjectWeb.Models;
using ProjectWeb.Models.Entities;

namespace ProjectWeb.Controllers
{
    public class UtilizatorController : Controller
    {

        private readonly IUtilizatorService _utilizatorService;

        public UtilizatorController(IUtilizatorService utilizatorService)
        {
            _utilizatorService = utilizatorService;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var utilizatori = await _utilizatorService.GetAllUtilizatoriAsync();
            return View(utilizatori);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Utilizator utilizator)
        {
            if (ModelState.IsValid)
            {
                await _utilizatorService.AddUtilizatorAsync(utilizator);
                return RedirectToAction("List");
            }
            return View(utilizator);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var utilizator = await _utilizatorService.GetUtilizatorByIdAsync(id);
            if (utilizator == null)
            {
                return NotFound();
            }
            return View(utilizator);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Utilizator utilizator)
        {
            if (id != utilizator.IdUtilizator)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _utilizatorService.UpdateUtilizatorAsync(utilizator);
                return RedirectToAction("List");
            }

            return View(utilizator);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _utilizatorService.DeleteUtilizatorAsync(id);
            return RedirectToAction("List");
        }


        //private readonly ApplicationDB dbContext;
        //public UtilizatorController(ApplicationDB dbContext)
        //{
        //    this.dbContext = dbContext;
        //}

        //[HttpGet]
        //public IActionResult Add()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Add(AddUtilizatorViewModel viewModel)
        //{
        //    //preluam datele care au fost introduse in ViewModel si le introducem in una locala
        //    var utilizator = new Utilizator
        //    {
        //        Nume = viewModel.Nume,
        //        Email = viewModel.Email,
        //        Parola = viewModel.Parola,
        //        Pasager = viewModel.Pasager
        //    };

        //    await dbContext.Utilizator.AddAsync(utilizator);

        //    //aici se salveaza modificarile in baza de date
        //    await dbContext.SaveChangesAsync();

        //    return RedirectToAction("List", "Utilizator");
        //}

        //[HttpGet]
        //public async Task<IActionResult> List()
        //{
        //    var utilizatori = await dbContext.Utilizator.ToListAsync();
        //    return View(utilizatori);
        //}

        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    Console.WriteLine($"ID primit: {id}"); // Log simplu
        //    var utilizator = await dbContext.Utilizator.FindAsync(id);

        //    return View(utilizator);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(Utilizator viewModel)
        //{
        //    var utilizator = await dbContext.Utilizator.FindAsync(viewModel.IdUtilizator);

        //    if (utilizator is not null)
        //    {
        //        //utilizator.IdUtilizator = viewModel.IdUtilizator;
        //        utilizator.Nume = viewModel.Nume;
        //        utilizator.Email = viewModel.Email;
        //        utilizator.Parola = viewModel.Parola;
        //        utilizator.Pasager = viewModel.Pasager;

        //        await dbContext.SaveChangesAsync();
        //    }
        //    //daca uldate-ul este realizat cu succes, ne vom intoarce la pagina principala, tabelul cu utilizatori
        //    return RedirectToAction("List", "Utilizator");
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(Utilizator viewModel)
        //{
        //    var utilizator = await dbContext.Utilizator
        //        .AsNoTracking ()
        //        .FirstOrDefaultAsync(x => x.IdUtilizator == viewModel.IdUtilizator);

        //    if (utilizator is not null)
        //    {
        //        dbContext.Utilizator.Remove(viewModel);

        //        await dbContext.SaveChangesAsync();

        //        TempData["SuccessMessage"] = $"Utilizatorul cu ID \"{viewModel.IdUtilizator}\" a fost șters cu succes!";
        //    }
        //    return RedirectToAction("List", "Utilizator");
        //}
    }
    
}
