using AutoMapper;
using BE_Toker_PruebaTecnica.Controllers;
using BE_Toker_PruebaTecnica.Models;
using BE_Toker_PruebaTecnica.Models.DTO;
using BE_Toker_PruebaTecnica.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestToker
{
    public class UnitTest1
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<UserController>> _mockLogger;
        private readonly UserController _controller;

        public UnitTest1()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UserController>>();

            _controller = new UserController(
                _mockMapper.Object,
                _mockUserRepository.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfUsers()
        {
            var users = new List<User> { new User { Id = 1, Name = "Test User" } };
            var userDTOs = new List<UserDTO> { new UserDTO { Id = 1, Name = "Test User" } };

            _mockUserRepository.Setup(repo => repo.GetListUser()).ReturnsAsync(users);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<UserDTO>>(users)).Returns(userDTOs);

            var result = await _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task Get_ById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            _mockUserRepository.Setup(repo => repo.GetUser(It.IsAny<int>())).ReturnsAsync((User?)null);

            var result = await _controller.Get(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsOkResult_WithWelcomeMessage()
        {
            var userDTO = new UserDTO { Name = "John Doe", Telefono = "123456789" };
            var user = new User { Name = "John Doe", Telefono = "123456789" };

            _mockMapper.Setup(mapper => mapper.Map<User>(userDTO)).Returns(user);
            _mockUserRepository.Setup(repo => repo.AddUser(It.IsAny<User>())).ReturnsAsync(user);
            _mockMapper.Setup(mapper => mapper.Map<UserDTO>(user)).Returns(userDTO);

            var result = await _controller.Post(userDTO);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<string>(okResult.Value);
            Assert.Contains("John Doe", message);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenUserExists()
        {
            var user = new User { Id = 1, Name = "Test User" };

            _mockUserRepository.Setup(repo => repo.GetUser(user.Id)).ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.deleteUser(user)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(user.Id);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Put_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            var userDTO = new UserDTO { Id = 1, Name = "Updated User" };
            var user = new User { Id = 1, Name = "Updated User" };

            _mockMapper.Setup(mapper => mapper.Map<User>(userDTO)).Returns(user);
            _mockUserRepository.Setup(repo => repo.GetUser(user.Id)).ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.UpdateUser(user)).Returns(Task.CompletedTask);

            var result = await _controller.Put(user.Id, userDTO);

            Assert.IsType<NoContentResult>(result);
        }
    }
}