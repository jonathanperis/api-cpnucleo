﻿using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Recurso;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Recurso
{
    public class RemoverTest
    {
        private readonly Mock<IRecursoAppService> _recursoAppService;

        public RemoverTest()
        {
            _recursoAppService = new Mock<IRecursoAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            RecursoViewModel recursoMock = new RecursoViewModel { };

            RemoverModel pageModel = new RemoverModel(_recursoAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _recursoAppService.Setup(x => x.Consultar(id)).Returns(recursoMock);

            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Fact]
        public void Test_OnPost()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            RemoverModel pageModel = new RemoverModel(_recursoAppService.Object)
            {
                Recurso = new RecursoViewModel { Id = id },
                PageContext = PageContextManager.CreatePageContext()
            };

            _recursoAppService.Setup(x => x.Remover(id));

            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}