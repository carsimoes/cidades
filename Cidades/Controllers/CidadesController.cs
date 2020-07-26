using Cidades.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NonFactors.Mvc.Grid;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidades.Controllers
{
    public class CidadesController : Controller
    {
        private readonly CidadesContexto _context;

        public CidadesController(CidadesContexto context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(CreateExportableGrid());
        }

        [HttpGet]
        public IActionResult ExportIndex()
        {
            return Export(CreateExportableGrid(), "Cidades");
        }

        private FileContentResult Export(IGrid grid, String fileName)
        {
            Int32 col = 1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using ExcelPackage package = new ExcelPackage();

            ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Data");

            foreach (IGridColumn column in grid.Columns)
            {
                sheet.Cells[1, col].Value = column.Title;
                sheet.Column(col++).Width = 18;

                column.IsEncoded = false;
            }

            foreach (IGridRow<Object> row in grid.Rows)
            {
                col = 1;

                foreach (IGridColumn column in grid.Columns)
                    sheet.Cells[row.Index + 2, col++].Value = column.ValueFor(row);
            }

            return File(package.GetAsByteArray(), "application/unknown", $"{fileName}.xlsx");
        }

        private IGrid<Domain.Cidades> CreateExportableGrid()
        {
            var cidadesList = from c in _context.Cidades
                              select c;

            IGrid<Domain.Cidades> grid = new Grid<Domain.Cidades>(cidadesList);
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            grid.Query = Request.Query;

            grid.Columns.Add(model => model.Ibge).Titled("IBGE");
            grid.Columns.Add(model => model.Nome).Titled("Nome");
            grid.Columns.Add(model => model.Uf).Titled("UF");
            grid.Columns.Add(model => model.Longitude).Titled("Longitude");
            grid.Columns.Add(model => model.Latitude).Titled("Latitude");
            grid.Columns.Add(model => model.Regiao).Titled("Região");

            grid.Columns.Add(model => model).Encoded(false)
                .Sortable(false).Filterable(false)
                .RenderedAs(model => "<a href=\"" + Url.ActionLink("Edit", "Cidades", new { model.Id }) + "\"> Editar</a>");

            grid.Columns.Add(model => model).Encoded(false)
               .Sortable(false).Filterable(false)
               .RenderedAs(model => "<a href=\"" + Url.ActionLink("Delete", "Cidades", new { model.Id }) + "\"> Excluir</a>");

            grid.Pager = new GridPager<Domain.Cidades>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Processors.Add(grid.Sort);

            grid.Pager.CurrentPage = 1;
            grid.Pager.RowsPerPage = 4;
            grid.Pager.PageSizes = new Dictionary<Int32, String> { { 0, "Todos" }, { 2, "2" }, { 4, "4" }, { 10, "10" }, { 30, "30" }, { 50, "50" }, { 100, "100" } };
            grid.Pager.ShowPageSizes = true;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cidades = await _context.Cidades
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cidades == null)
            {
                return NotFound();
            }

            return View(cidades);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ibge,Uf,Nome,Longitude,Latitude,Regiao")] Domain.Cidades cidades)
        {
            var cidadeExistenteIbge = _context.Cidades.Where(m => m.Ibge == cidades.Ibge).Any();
            if (cidadeExistenteIbge)
            {
                ModelState.AddModelError("Ibge", "Número do Ibge existente.");
            }

            var cidadePorUf = _context.Cidades.Where(m => m.Uf == cidades.Uf);
            if (cidadePorUf.Any())
            {
                var cidadeComNomeIgual = cidadePorUf.Where(m => m.Nome == cidades.Nome).Any();
                if (cidadeComNomeIgual)
                {
                    ModelState.AddModelError("Nome", "Cidade existente no UF " + cidades.Uf);
                }
            }

            if (ModelState.IsValid)
            {
              

                _context.Add(cidades);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cidades);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cidades = await _context.Cidades.FindAsync(id);
            if (cidades == null)
            {
                return NotFound();
            }
            return View(cidades);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ibge,Uf,Nome,Longitude,Latitude,Regiao")] Domain.Cidades cidades)
        {
            if (id != cidades.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cidades);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CidadesExists(cidades.Id))
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
            return View(cidades);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cidades = await _context.Cidades
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cidades == null)
            {
                return NotFound();
            }

            return View(cidades);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cidades = await _context.Cidades.FindAsync(id);
            _context.Cidades.Remove(cidades);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CidadesExists(int id)
        {
            return _context.Cidades.Any(e => e.Id == id);
        }
    }
}
