﻿using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Luna.Pages.Recurso
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRecursoAppService _recursoAppService;

        public RemoverModel(IRecursoAppService recursoAppService)
        {
            _recursoAppService = recursoAppService;
        }

        [BindProperty]
        public RecursoViewModel Recurso { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Recurso = _recursoAppService.Consultar(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _recursoAppService.Remover(Recurso.Id);

            return RedirectToPage("Listar");
        }
    }
}