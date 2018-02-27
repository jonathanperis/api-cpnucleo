﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.Workflow;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_cpnucleo_pages.Pages.Workflow
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IWorkflowRepository _workflowRepository;

        public IncluirModel(IWorkflowRepository workflowRepository)
        {
            _workflowRepository = workflowRepository;
        }

        [BindProperty]
        public WorkflowItem Workflow { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(WorkflowItem workflow)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _workflowRepository.Incluir(workflow);

            return RedirectToPage("Listar");
        }
    }
}