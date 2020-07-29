using NUnit.Framework;
using Moq;
using CrudDemo.Persistence.Interfaces;
using CrudDemo.Models;
using System.Collections.Generic;
using CrudDemo.Interfaces;
using System.Linq;
using NuGet.Frameworks;
using CrudDemo.Services;
using AutoMapper;
using CrudDemo.DTO;
using System.Security.Cryptography.X509Certificates;
using CrudDemo.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace CrudDemo.Tests
{
    [TestFixture]
    public class GlossaryItemsServiceTest
    {
        private Mock<IGlossaryRepository> _glossaryRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private GlossaryItemsService _glossaryItemsService;

        [SetUp]
        public void Init()
        {
            _glossaryRepositoryMock = new Mock<IGlossaryRepository>(MockBehavior.Strict);
            _unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            _mapperMock = new Mock<IMapper>();
            _glossaryItemsService = new GlossaryItemsService(_glossaryRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Add_item_success()
        {
            var itemToAdd = new GlossaryItem
            {
                Id = 3,
                Term = "alkaline",
                Definition = "Definition test"
            };

            var itemToAddDto = new GlossaryItemAddDto
            {
                Term = "alkaline",
                Definition = "Updated definition"
            };

            var itemAfterAddDto = new GlossaryItemDto
            {
                Id = 3,
                Term = "alkaline",
                Definition = "Updated definition"
            };

            _glossaryRepositoryMock.Setup(e => e.AddAsync(itemToAdd))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(e => e.Commit());

            _mapperMock
                .Setup(e => e.Map<GlossaryItemAddDto, GlossaryItem>(itemToAddDto))
                .Returns(itemToAdd);

            _mapperMock
                .Setup(e => e.Map<GlossaryItem, GlossaryItemDto>(itemToAdd))
                .Returns(itemAfterAddDto);

            var result = await _glossaryItemsService.AddAsync(itemToAddDto);
            _glossaryRepositoryMock.Verify(e => e.AddAsync(itemToAdd), Times.Once);
            _unitOfWorkMock.Verify(e => e.Commit(), Times.Once);
            _mapperMock.Verify(e => e.Map<GlossaryItemAddDto, GlossaryItem>(itemToAddDto));
            _mapperMock.Verify(e => e.Map<GlossaryItem, GlossaryItemDto>(itemToAdd));
            Assert.AreEqual(itemAfterAddDto, result);
        }

        [Test]
        public void Add_throws_db_update_exception_when_item_with_provided_term_name_already_exists()
        {
            var itemForAdd = new GlossaryItemAddDto
            {
                Term = "Existing term",
                Definition = "Test definition"
            };

            var existingGlossaryItem = new GlossaryItem
            {
                Term = "Existing term",
                Definition = "Test definition"
            };

            _mapperMock
                .Setup(e => e.Map<GlossaryItemAddDto, GlossaryItem>(itemForAdd))
                .Returns(existingGlossaryItem);

            _unitOfWorkMock.Setup(e => e.Commit())
                .Throws(new DbUpdateException());
            _glossaryRepositoryMock.Setup(e => e.AddAsync(existingGlossaryItem))
                .Returns(Task.CompletedTask);

            _glossaryItemsService = new GlossaryItemsService(_glossaryRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object);
            Assert.ThrowsAsync<DbUpdateException>(async () => await _glossaryItemsService.AddAsync(itemForAdd));
            _unitOfWorkMock.Verify(e => e.Commit(), Times.Once);
        }

        [Test]
        public async Task Update_change_item_definition_success()
        {
            var itemForUpdateDto = new GlossaryItemUpdateDto
            {
                Definition = "Updated definition"
            };

            var itemBeforeUpdate = new GlossaryItem
            {
                Id = 3,
                Term = "alkaline",
                Definition = "Definition test"
            };

            var itemAfterUpdate = new GlossaryItem
            {
                Id = 3,
                Term = "alkaline",
                Definition = "Updated definition"
            };

            var itemAfterUpdateDto = new GlossaryItemDto
            {
                Id = 3,
                Term = "alkaline",
                Definition = "Updated definition"
            };

            _unitOfWorkMock.Setup(e => e.Commit());
            _glossaryRepositoryMock.Setup(e => e.FindByIdAsync(3))
                .Returns(Task.FromResult(itemBeforeUpdate));

            _mapperMock
                .Setup(e => e.Map<GlossaryItem, GlossaryItemDto>(itemAfterUpdate))
                .Returns(itemAfterUpdateDto);

            _glossaryRepositoryMock.Setup(e => e.Update(itemBeforeUpdate));

            await _glossaryItemsService.UpdateAsync(itemForUpdateDto, 3);
            _glossaryRepositoryMock.Verify(e => e.FindByIdAsync(3), Times.Once);
            _unitOfWorkMock.Verify(e => e.Commit(), Times.Once);
        }

        [Test]
        public void Update_throws_non_existing_item_exception_when_item_not_found()
        {
            var itemForUpdateDto = new GlossaryItemUpdateDto
            {
                Definition = "Updated definition"
            };

            GlossaryItem glossaryItemNull = null;

            _unitOfWorkMock.Setup(e => e.Commit());
            _glossaryRepositoryMock.Setup(e => e.FindByIdAsync(3))
                .Returns(Task.FromResult(glossaryItemNull));

            _glossaryItemsService = new GlossaryItemsService(_glossaryRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object);

            Assert.ThrowsAsync<NonExistingItemException>(async () => await _glossaryItemsService.UpdateAsync(itemForUpdateDto, 3));
            _glossaryRepositoryMock.Verify(e => e.FindByIdAsync(3), Times.Once);
            _unitOfWorkMock.Verify(e => e.Commit(), Times.Never);
        }

        [Test]
        public void Get_list_should_retrieve_all_three_items_from_repository()
        {
            var item1 = new GlossaryItem
            {
                Id = 1,
                Term = "abyssal plain",
                Definition = "The ocean floor offshore from the continental margin, usually very flat with a slight slope."
            };

            var item2 = new GlossaryItem
            {
                Id = 2,
                Term = "accrete",
                Definition = "v. To add terranes (small land masses or pieces of crust) to another, usually larger, land mass."
            };

            var item3 = new GlossaryItem
            {
                Id = 3,
                Term = "alkaline",
                Definition = "Term pertaining to a highly basic, as opposed to acidic, subtance. For example, hydroxide or carbonate of sodium or potassium."
            };

            var repositoryItems = new List<GlossaryItem>
            {
                item1,
                item2,
                item3
            };

            var itemsDto = new List<GlossaryItemDto>
            {
                new GlossaryItemDto
                {
                    Id = 1,
                    Term = "abyssal plain",
                    Definition = "The ocean floor offshore from the continental margin, usually very flat with a slight slope."
                },
                new GlossaryItemDto
                {
                    Id = 2,
                    Term = "accrete",
                    Definition = "v. To add terranes (small land masses or pieces of crust) to another, usually larger, land mass."
                },
                new GlossaryItemDto
                {
                    Id = 3,
                    Term = "alkaline",
                    Definition = "Term pertaining to a highly basic, as opposed to acidic, subtance. For example, hydroxide or carbonate of sodium or potassium."
                },
            };

            _glossaryRepositoryMock.Setup(e => e.GetAllReadyOnlyOrderedAsync())
                .Returns(Task.FromResult(repositoryItems));

            _mapperMock
                .Setup(e => e.Map<List<GlossaryItem>, List<GlossaryItemDto>>(repositoryItems))
                .Returns(itemsDto);

            var resultTask = _glossaryItemsService.GetOrderedListAsync();
            _glossaryRepositoryMock.Verify(e => e.GetAllReadyOnlyOrderedAsync(), Times.Once);
            _mapperMock.Verify(e => e.Map<List<GlossaryItem>, List<GlossaryItemDto>>(repositoryItems), Times.Once);
            CollectionAssert.AreEqual(itemsDto, resultTask.Result);
        }

        [Test]
        public void Delete_throws_non_existing_item_exception_when_item_not_found()
        {
            GlossaryItem glossaryItemNull = null;
            _glossaryRepositoryMock.Setup(e => e.FindByIdAsync(3))
                .Returns(Task.FromResult(glossaryItemNull));
            _unitOfWorkMock.Setup(e => e.Commit());

            _glossaryItemsService = new GlossaryItemsService(_glossaryRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object);

            Assert.ThrowsAsync<NonExistingItemException>(() => _glossaryItemsService.DeleteAsync(3));
            _glossaryRepositoryMock.Verify(e => e.FindByIdAsync(3), Times.Once);
            _unitOfWorkMock.Verify(e => e.Commit(), Times.Never);
        }

        [Test]
        public async Task Delete_item_success()
        {
            var itemToDelete = new GlossaryItem
            {
                Id = 3,
                Term = "alkaline",
                Definition = "Definition test"
            };

            _unitOfWorkMock.Setup(e => e.Commit());
            _glossaryRepositoryMock.Setup(e => e.FindByIdAsync(3))
                .Returns(Task.FromResult(itemToDelete));

            _glossaryRepositoryMock.Setup(e => e.Remove(itemToDelete));
            await _glossaryItemsService.DeleteAsync(3);
            _glossaryRepositoryMock.Verify(e => e.FindByIdAsync(3), Times.Once);
            _glossaryRepositoryMock.Verify(e => e.Remove(itemToDelete), Times.Once);
            _unitOfWorkMock.Verify(e => e.Commit(), Times.Once);
        }
    }
}
