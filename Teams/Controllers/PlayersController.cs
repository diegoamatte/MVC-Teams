#nullable disable
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Teams.Data;
using Teams.Models;
using Teams.ViewModels.Player;

namespace Teams.Controllers
{
    public class PlayersController : Controller
    {
        private readonly TeamsContext _context;

        public PlayersController(TeamsContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }

            var players = _context.Player.AsQueryable().ProjectToType<PlayerIndexViewModel>();

            if (!String.IsNullOrEmpty(searchString))
            {
                players = players.Where(p => p.Name.Contains(searchString)
                                       || p.Team.Name.Contains(searchString));
            }

            if (sortOrder == "name_desc")
            {
                players = players.OrderByDescending(p => p.Name);
            }
            else
            {
                players = players.OrderBy(t => t.Name);
            }
            int pageSize = 5;

            return View(await PaginatedList<PlayerIndexViewModel>.CreateAsync(players.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Player
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Players/Create
        public async Task<IActionResult> CreateAsync(Guid? teamId)
        {
            var viewData = new PlayerCreateViewModel { TeamList = new SelectList(_context.Team, "Id", "Name", teamId??_context.Team.First().Id) };
            return View(viewData);
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Age,ImgUrl,Nationality,TeamId,TeamList")] PlayerCreateViewModel playerCreated)
        {
            if (ModelState.IsValid)
            {
                var player = playerCreated.Adapt<Player>();
                player.Id = Guid.NewGuid();
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamId"] = new SelectList(_context.Team, "Id", "League", playerCreated.TeamId);
            return View(playerCreated);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Player.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }
            var viewModel = player.Adapt<PlayerEditViewModel>();
            viewModel.TeamList = new SelectList(_context.Team, "Id", "Name", player.TeamId);

            return View(viewModel);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Age,ImgUrl,Nationality,TeamId,TeamList")] PlayerEditViewModel playerViewModel)
        {
            if (id != playerViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var player = playerViewModel.Adapt<Player>();
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(playerViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(playerViewModel);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Player
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var player = await _context.Player.FindAsync(id);
            _context.Player.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(Guid id)
        {
            return _context.Player.Any(e => e.Id == id);
        }
    }
}
