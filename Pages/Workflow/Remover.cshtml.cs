﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.Workflow;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_cpnucleo_pages.Pages.Workflow
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IWorkflowRepository _workflowRepository;

        public RemoverModel(IWorkflowRepository workflowRepository)
        {
            _workflowRepository = workflowRepository;
        }

        [BindProperty]
        public WorkflowItem Workflow { get; set; }

        public async Task<IActionResult> OnGetAsync(int idWorkflow)
        {
            Workflow = await _workflowRepository.Consultar(idWorkflow);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(WorkflowItem workflow)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _workflowRepository.Remover(workflow);

            return RedirectToPage("Listar");
        }
    }
}