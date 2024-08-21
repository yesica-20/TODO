using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class TodoItem
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
public class ErrorResponse
{
    public string Message { get; set; }
    public int StatusCode { get; set; }
}

namespace TODOC_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private static List<TodoItem> TodoItems = new List<TodoItem>();
        private static int CurrentId = 1;

        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> Get()
        {
            try
            {
                return Ok(TodoItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = "An error occurred while retrieving the items.", StatusCode = 500 });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get(int id)
        {
            try
            {
                var item = TodoItems.FirstOrDefault(t => t.Id == id);
                if (item == null)
                    return NotFound(new ErrorResponse { Message = "Item not found.", StatusCode = 404 });

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = "An error occurred while retrieving the item.", StatusCode = 500 });
            }
        }

        [HttpPost]
        public ActionResult<TodoItem> Post([FromBody] TodoItem newTask)
        {
            if (newTask == null || string.IsNullOrEmpty(newTask.Name))
            {
                return BadRequest(new ErrorResponse { Message = "Invalid input data.", StatusCode = 400 });
            }

            try
            {
                newTask.Id = CurrentId++;
                TodoItems.Add(newTask);
                return CreatedAtAction(nameof(Get), new { id = newTask.Id }, newTask);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = "An error occurred while creating the item.", StatusCode = 500 });
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] TodoItem updatedTask)
        {
            if (updatedTask == null || string.IsNullOrEmpty(updatedTask.Name))
            {
                return BadRequest(new ErrorResponse { Message = "Invalid input data.", StatusCode = 400 });
            }

            try
            {
                var item = TodoItems.FirstOrDefault(t => t.Id == id);
                if (item == null)
                    return NotFound(new ErrorResponse { Message = "Item not found.", StatusCode = 404 });

                item.Name = updatedTask.Name;
                item.IsComplete = updatedTask.IsComplete;

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = "An error occurred while updating the item.", StatusCode = 500 });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var item = TodoItems.FirstOrDefault(t => t.Id == id);
                if (item == null)
                    return NotFound(new ErrorResponse { Message = "Item not found.", StatusCode = 404 });

                TodoItems.Remove(item);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = "An error occurred while deleting the item.", StatusCode = 500 });
            }
        }
    }
}
