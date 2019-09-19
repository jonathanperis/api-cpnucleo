﻿using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Projeto;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Projeto
{
    public class RemoverTest
    {
        private readonly Mock<IAppService<ProjetoViewModel>> _projetoAppService;

        public RemoverTest()
        {
            _projetoAppService = new Mock<IAppService<ProjetoViewModel>>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = new Guid();

            ProjetoViewModel projetoMock = new ProjetoViewModel { };

            _projetoAppService.Setup(x => x.Consultar(id)).Returns(projetoMock);

            RemoverModel pageModel = new RemoverModel(_projetoAppService.Object);
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
            Guid id = new Guid();

            ProjetoViewModel projetoMock = new ProjetoViewModel { };

            _projetoAppService.Setup(x => x.Remover(id));

            RemoverModel pageModel = new RemoverModel(_projetoAppService.Object);
            pageModel.Projeto = new ProjetoViewModel { Id = id };

            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}