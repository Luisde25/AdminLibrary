using AdminLibrary.Constants;
using AdminLibrary.Controllers.UsersModel.request;
using AdminLibrary.Dtos;
using AdminLibrary.Models;
using AdminLibrary.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Controllers.UsersModel
{
    [ApiController]
    [Route("api/Users")]
    public class UsersController(
         AppDbContext context
        ) : Controller
    {
        
        private readonly AppDbContext _context = context;
        private readonly Users _user = new();

        #region Lista de usuarios que tienen un rol asignado
        [HttpGet("GetUsers")]
        public async Task<List<UsersDto>> GetUsers()
        {
            return await _user.CallingListUsers(_context);
        }
        #endregion

        #region Creación de un nuevo usuario
        [HttpPost("Create")]
        public async Task<ResponseDto> CreateUser(
           UserControllerRequest userControllerRequest
           )
        {
            return await _user.CreateNewUser(_context, userControllerRequest);
        }

        #endregion

        #region Actualizar usuario existe
        [HttpPut("Update")]
        public async Task<ResponseDto> UpdateUser(
          UserControllerUpdate userControllerUpdate
          )
        {
            return await _user.UpdateCurrentUser(_context, userControllerUpdate);
        }
        #endregion 

        #region Eliminar usuario existente
        [HttpDelete("Remove")]
        public async Task<ResponseDto> DeleteUser( int id
       )
        {
            return await _user.RemoveCurrentUser(_context, id);
        }
        #endregion
    }
}
