using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagement.Controllers;
using TaskManagement.Data;
using TaskManagement.DTO;
using TaskManagement.Models;
using TaskManagement.Utils;

namespace Tests
{
    public class TaskControllerTests
    {
        private readonly Mock<TaskContext> _contextMock;
        private readonly Mock<ILogger<TasksController>> _loggerMock;
        private readonly Log _log;
        private readonly TaskContext dbContext;
        private readonly TasksController _controller;

        public TaskControllerTests()
        {
            _contextMock = new Mock<TaskContext>();
            _loggerMock = new Mock<ILogger<TasksController>>();
            var logLoggerMock = new Mock<ILogger<Log>>();
            _log = new Log(logLoggerMock.Object);

            // Use InMemory database for EF Core operations
            var options = new DbContextOptionsBuilder<TaskContext>()
                .UseInMemoryDatabase(databaseName: "TaskManagementTestDb")
            .Options;

             dbContext = new TaskContext(options);
            _controller = new TasksController(dbContext, _loggerMock.Object, _log);


            // Mock the HttpContext and set it to the controller's context
            var httpContextMock = new Mock<HttpContext>();
            var requestMock = new Mock<HttpRequest>();
            var headers = new HeaderDictionary
            {
                { "name", "TestUser" }
            };
            requestMock.Setup(r => r.Headers).Returns(headers);
            httpContextMock.Setup(c => c.Request).Returns(requestMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object
            };
        }

        private void ClearDatabase()
        {
            dbContext.Tasks.RemoveRange(dbContext.Tasks);
            dbContext.SaveChanges();
        }


        [Fact]
        public async Task GetTasks_ReturnsOkResult_WithAListOfTasks()
        {
            ClearDatabase();

            // Arrange
            var taskItemDto1 = new TaskItemDto { Title = "Test Task 1", Description = "Description 1", DueDate = DateTime.Now };
            var taskItemDto2 = new TaskItemDto { Title = "Test Task 2", Description = "Description 2", DueDate = DateTime.Now };

            await _controller.PostTaskItem(taskItemDto1);
            await _controller.PostTaskItem(taskItemDto2);

            // Act
            var result = await _controller.GetTasks();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnTasks = okResult.Value as IEnumerable<TaskItem>;
            returnTasks.Should().NotBeNull();
            returnTasks.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetTaskItem_WithValidId_ReturnsOkResult_WithTaskItem()
        {
            ClearDatabase();

            // Arrange
            var taskItemDto = new TaskItemDto { Title = "Test Task", Description = "Test Description", DueDate = DateTime.Now };

            var postResult = await _controller.PostTaskItem(taskItemDto);
            var createdAtActionResult = postResult.Result as CreatedAtActionResult;
            var createdTask = createdAtActionResult.Value as TaskItem;

            // Act
            var getResult = await _controller.GetTaskItem(createdTask.Id);

            // Assert
            var okResult = getResult.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var task = okResult.Value as TaskItem;
            task.Should().NotBeNull();
            task.Id.Should().Be(createdTask.Id);
            task.Title.Should().Be("Test Task");
            task.Description.Should().Be("Test Description");
        }

        [Fact]
        public async Task PostTaskItem_WithValidTaskItem_ReturnsCreatedAtActionResult()
        {
            ClearDatabase();
            // Arrange
            var taskItemDto = new TaskItemDto { Title = "New Task", Description = "New Description", DueDate = DateTime.Now };

            // Act
            var result = await _controller.PostTaskItem(taskItemDto);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.StatusCode.Should().Be(201);

            var createdTask = createdAtActionResult.Value as TaskItem;
            createdTask.Should().NotBeNull();
            createdTask.Title.Should().Be("New Task");
            createdTask.Description.Should().Be("New Description");
        }

        [Fact]
        public async Task PutTaskItem_WithValidIdAndData_ReturnsOkResult_WithUpdatedTask()
        {
            // Clear the database before running the test
            ClearDatabase();

            // Arrange
            var taskItemDto = new TaskItemDto { Title = "Initial Task", Description = "Initial Description", DueDate = DateTime.Now };

            var postResult = await _controller.PostTaskItem(taskItemDto);
            var createdAtActionResult = postResult.Result as CreatedAtActionResult;
            var createdTask = createdAtActionResult?.Value as TaskItem;

            // Ensure the task was created successfully
            createdTask.Should().NotBeNull();

            var updatedTaskDto = new TaskItemDto
            {
                Id = createdTask.Id, 
                Title = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.Now
            };

            // Act
            var result = await _controller.PutTaskItem(createdTask.Id, updatedTaskDto);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull(); 
            okResult.StatusCode.Should().Be(200);

            var updatedTask = okResult.Value as TaskItem;
            updatedTask.Should().NotBeNull();
            updatedTask.Id.Should().Be(createdTask.Id);
            updatedTask.Title.Should().Be("Updated Task");
            updatedTask.Description.Should().Be("Updated Description");

        }

        [Fact]
        public async Task DeleteTaskItem_WithValidId_ReturnsNoContentResult()
        {
            ClearDatabase();

            // Arrange
            var taskItemDto = new TaskItemDto { Title = "Test Task", Description = "Test Description", DueDate = DateTime.Now };

            var postResult = await _controller.PostTaskItem(taskItemDto);
            var createdAtActionResult = postResult.Result as CreatedAtActionResult;
            var createdTask = createdAtActionResult.Value as TaskItem;

            // Act
            var result = await _controller.DeleteTaskItem(createdTask.Id);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(204);
        }

    }

   




}