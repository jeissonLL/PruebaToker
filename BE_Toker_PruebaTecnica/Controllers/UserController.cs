using AutoMapper;
using BE_Toker_PruebaTecnica.Models;
using BE_Toker_PruebaTecnica.Models.DTO;
using BE_Toker_PruebaTecnica.Models.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BE_Toker_PruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IMapper mapper, 
            IUserRepository userRepository, 
            ILogger<UserController> logger)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listUser = await _userRepository.GetListUser();
                var listUserDTO = _mapper.Map<IEnumerable<UserDTO>>(listUser);

                return Ok(listUserDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de usuarios");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var user = await _userRepository.GetUser(id);
                if (user == null)
                {
                    return NotFound("El Usuario no existe");
                }

                var userDTO = _mapper.Map<UserDTO>(user);

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el usuario con ID {id}");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _userRepository.GetUser(id);
                if (user == null)
                {
                    return NotFound();
                }

                await _userRepository.deleteUser(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar el usuario con ID {id}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserDTO userDTO)
        {
            try
            {
                var user = _mapper.Map<User>(userDTO);
                user = await _userRepository.AddUser(user);

                var userItemDTO = _mapper.Map<UserDTO>(user);

                _logger.LogInformation
                    ($"Te has creado con éxito, Bienvenido {userItemDTO.Name} " +
                    $"con número telefónico {userItemDTO.Telefono}");

                return Ok(
                    $"Te has creado con éxito, Bienvenido {userItemDTO.Name}" +
                    $" con número telefónico {userItemDTO.Telefono}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo usuario");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UserDTO userDTO)
        {
            try
            {
                var user = _mapper.Map<User>(userDTO);

                if (id != user.Id)
                {
                    return BadRequest();
                }

                var userItem = await _userRepository.GetUser(id);

                if (userItem == null)
                {
                    return NotFound();
                }

                await _userRepository.UpdateUser(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el usuario con ID {id}");
                return BadRequest(ex.Message);
            }
        }
    }
}
